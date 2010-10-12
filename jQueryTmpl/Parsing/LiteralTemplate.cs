using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class LiteralTemplate : Template
    {
        public LiteralTemplate(Token token) : base(token) { }

        public override string Render(object item)
        {
            return Value;
        }
    }
}