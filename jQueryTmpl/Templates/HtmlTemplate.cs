namespace jQueryTmpl.Templates
{
    public class HtmlTemplate : Template
    {
        public Expression Expression { get; set; }

        public override string Render(TemplateItem item)
        {
            return new ExpressionEvaluator(item).Evaluate(Expression).ToString();
        }
    }
}