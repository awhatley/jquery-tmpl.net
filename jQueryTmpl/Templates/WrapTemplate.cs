using System;

namespace jQueryTmpl.Templates
{
    public class WrapTemplate : Template
    {
        public TemplateEngine Engine { get; set; }
        public Expression Data { get; set; }
        public Expression Options { get; set; }
        public Template Template { get; set; }
        public Template Content { get; set; }

        public override string Render(TemplateItem item)
        {
            var wrapped = Template.Render(new TemplateItem {
                Parent = item,
                Data = Data,
                Options = Options,
                Html = Html,
            });

            return Engine.Render(wrapped, 
                new ExpressionEvaluator(item).Evaluate(Data), 
                new ExpressionEvaluator(item).Evaluate(Options));
        }

        public Template Html(string filter = "*", bool textOnly = false)
        {
            throw new NotImplementedException();
        }
    }
}