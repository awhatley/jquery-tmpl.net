using System;
using System.Collections.Generic;

using jQueryTmpl.Templates;

namespace jQueryTmpl
{
    public class TemplateEngine
    {
        private readonly IDictionary<string, Template> _cache = new Dictionary<string, Template>();

        public void Store(string name, string template)
        {
            _cache[name ?? template] = Compile(template);
        }
        
        public string Render(string template, object data = null, object options = null)
        {
            if(template == null)
                return String.Empty;

            var tmpl = _cache.ContainsKey(template) 
                ? _cache[template] 
                : _cache[template] = Compile(template);
            
            if(tmpl == null)
                return String.Empty;

            return tmpl.Render(new TemplateItem {
                Data = data, 
                Options = options
            });
        }

        private Template Compile(string template)
        {
            return new TemplateCompiler(_cache).Compile(template);
        }
    }
}