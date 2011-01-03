namespace jQueryTmpl.Templates
{
    public class IfTemplate : Template
    {
        public Expression Expression { get; set; }
        public Template Else { get; set; }
        public Template Content { get; set; }

        public override string Render(TemplateItem item)
        {
            return new ExpressionEvaluator(item).Evaluate(Expression).IsTruthy
                ? Content.Render(item)
                : Else.Render(item);
        }
    }
}