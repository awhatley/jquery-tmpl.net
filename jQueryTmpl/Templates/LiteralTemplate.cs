namespace jQueryTmpl.Templates
{
    public class LiteralTemplate : Template
    {
        public string Value { get; set; }

        public override string Render(TemplateItem item)
        {
            return Value;
        }
    }
}