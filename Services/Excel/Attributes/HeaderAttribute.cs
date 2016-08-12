using System;

namespace Services.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAttribute : Attribute
    {
        private readonly string _forHeader;

        /// <summary>
        /// The relevent header name for mapping this property
        /// </summary>
        /// <param name="forHeader"></param>
        public HeaderAttribute(string forHeader)
        {
            _forHeader = forHeader;
        }

        public string ForHeader {
            get { return _forHeader; }
        }
    }
}