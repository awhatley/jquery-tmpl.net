using System.Web;

namespace jQueryTmpl.Templates
{
    public class EvalTemplate : Template
    {
        public Expression Expression { get; set; }

        public override string Render(TemplateItem item)
        {
            return HttpUtility.HtmlEncode(new ExpressionEvaluator(item).Evaluate(Expression));
        }
    }
}