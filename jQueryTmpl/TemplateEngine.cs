using System;
using System.Collections.Generic;

using jQueryTmpl.Templates;

namespace jQueryTmpl
{
    public static class TemplateEngine
    {
        private static readonly ITemplateCache _cache = new InMemoryTemplateCache();

        public static Template Lookup(string name)
        {
            return _cache.Lookup(name);
        }

        public static void Store(string name, string template)
        {
            _cache.Store(name ?? template, Compile(template));
        }
        
        public static string Render(string template, object data = null, object options = null)
        {
            if(template == null)
                return String.Empty;

            var tmpl = _cache.Lookup(template);

            if(tmpl == null)
                _cache.Store(template, tmpl = Compile(template));
            
            if(tmpl == null)
                return String.Empty;

            return tmpl.Render(new TemplateItem {
                Data = data, 
                Options = options
            });
        }

        private static Template Compile(string template)
        {
            return new TemplateCompiler().Compile(template);
        }
    }

    public class InMemoryTemplateCache : ITemplateCache
    {
        private readonly IDictionary<string, Template> _cache = new Dictionary<string, Template>();

        public Template Lookup(string name)
        {
            return _cache.ContainsKey(name) ? _cache[name] : null;
        }

        public void Store(string name, Template template)
        {
            _cache[name] = template;
        }
    }

    public interface ITemplateCache
    {
        Template Lookup(string name);
        void Store(string name, Template template);
    }
}