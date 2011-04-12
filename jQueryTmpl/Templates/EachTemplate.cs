using System.Collections;
using System.Collections.Generic;
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
            var evaluation = new ExpressionEvaluator(item).Evaluate(Expression);
            return EachValueOf(evaluation)
                .Aggregate(new StringBuilder(), (s, t) => s.Append(Content.Render(t)))
                .ToString();
        }

        private IEnumerable<TemplateItem> EachValueOf(EvaluatedExpression expression)
        {
            var index = 0;
            var enumerable = expression.Value as IEnumerable ?? new[] { expression.Value };

            return enumerable
                .Cast<object>()
                .Select(item => new TemplateItem {
                    Data = expression.Item.Data,
                    Html = expression.Item.Html,
                    Options = expression.Item.Options,
                    Parent = expression.Item,
                    Index = index++,
                    IndexParameter = IndexParameter,
                    Value = item,
                    ValueParameter = ValueParameter,
                });
        }
    }
}