using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jQueryTmpl.Templates
{
    public class CompositeTemplate : Template
    {
        private readonly IEnumerable<Template> _children;

        public CompositeTemplate(IEnumerable<Template> children)
        {
            _children = children;
        }

        public override string Render(TemplateItem item)
        {
            return _children
                .Aggregate(new StringBuilder(), (sb, t) => sb.Append(t.Render(item)))
                .ToString();
        }
    }
}