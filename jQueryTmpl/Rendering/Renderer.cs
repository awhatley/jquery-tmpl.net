using System;
using System.Linq;

using jQueryTmpl.Parsing;

namespace jQueryTmpl.Rendering
{
    public class Renderer
    {
        public string Render(Template template, object data)
        {
            var array = data as object[] ?? new[] { data };
            return String.Join(String.Empty, array.Select(template.Render));
        }
    }
}