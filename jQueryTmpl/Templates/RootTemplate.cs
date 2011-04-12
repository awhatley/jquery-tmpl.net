using System.Collections;
using System.Linq;
using System.Text;

namespace jQueryTmpl.Templates
{
    public class RootTemplate : Template
    {
        public Template Content { get; set; }

        public override string Render(TemplateItem item)
        {
            var enumerable = item.Data as IEnumerable;
            
            if(enumerable == null)
                return Content.Render(item);

            return enumerable.Cast<object>()
                .Aggregate(new StringBuilder(), (sb, obj) => { item.Data = obj; return sb.Append(Content.Render(item)); })
                .ToString();
        }
    }
}