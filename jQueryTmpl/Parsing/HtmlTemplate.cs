using System;

using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class HtmlTemplate : Template
    {
        public HtmlTemplate(Token token) : base(token) { }

        public override string Render(object item)
        {
            return Convert.ToString(GetDataValue(item));
        }
    }
}