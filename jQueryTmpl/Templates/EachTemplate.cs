using System.Linq;
using System.Text;

namespace jQueryTmpl.Templates
{
    public class EachTemplate : Template
    {
        public Parameter IndexParameter { get; set; }
        public Parameter ValueParameter { get; set; }
        public Expression Expression { get; set; }
        public Template Content { get; set; }

        public override string Render(TemplateItem item)
        {
            var ss = new ExpressionEvaluator(item).Evaluate(Expression)
                .ToTemplateItemCollection(IndexParameter, ValueParameter)
                .Aggregate(new StringBuilder(), (s, t) => s.Append(Content.Render(t)))
                .ToString();

            return ss;
        }
    }
}