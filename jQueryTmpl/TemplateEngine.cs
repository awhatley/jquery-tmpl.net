using System;

using jQueryTmpl.Templates;

namespace jQueryTmpl
{
    public class TemplateEngine
    {
        private static readonly TemplateCache _cache = new TemplateCache();

        public void Store(string name, string template)
        {
            _cache.Store(name ?? template, Compile(template));
        }
        
        public string Render(string template, object data = null, object options = null)
        {
            if(template == null)
                return String.Empty;

            var tmpl = _cache.Retrieve(template, () => Compile(template));

            if(tmpl == null)
                return String.Empty;

            return tmpl.Render(new TemplateItem {
                Data = data, 
                Options = options
            });
        }

        private static Template Compile(string template)
        {
            return new TemplateCompiler(_cache).Compile(template);
        }
    }
}