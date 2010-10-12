using System;
using System.Web;

using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class PrintTemplate : Template
    {
        public PrintTemplate(Token token) : base(token) { }

        public override string Render(object item)
        {
            return HttpUtility.HtmlEncode(Convert.ToString(GetDataValue(item)));
        }
    }
}