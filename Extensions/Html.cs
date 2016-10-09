using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, string template)
        {
            if (!IsScriptLoaded(htmlHelper,template))
            {
                htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            }
            return MvcHtmlString.Empty;
        }

        private static bool IsScriptLoaded(HtmlHelper htmlHelper,string script)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as string;
                    if (script == template)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static MvcHtmlString Script(this HtmlHelper htmlHelper, List<string> templates)
        {
            foreach (var template in templates)
            {
                Script(htmlHelper, template);
            }
            return MvcHtmlString.Empty;
        }

        private static bool IsStyleLoaded(HtmlHelper htmlHelper, string script)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_link_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as string;
                    if (script == template)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static MvcHtmlString Style(this HtmlHelper htmlHelper, string template)
        {
            if (!IsScriptLoaded(htmlHelper, template))
            {
                htmlHelper.ViewContext.HttpContext.Items["_link_" + Guid.NewGuid()] = template;
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Style(this HtmlHelper htmlHelper, List<string> templates)
        {
            foreach (var template in templates)
            {
                Script(htmlHelper, template);
            }
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as string;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template);
                    }
                }
            }
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderStyles(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_link_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
    }
}
