using System.Web.Mvc;

namespace BelibaHoma.Areas.Tutor
{
    public class TutorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Tutor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Tutor_default",
                "Tutor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "BelibaHoma.Areas.Tutor.Controllers" }
            );
        }
    }
}