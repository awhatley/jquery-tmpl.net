using System;
using System.Collections.Generic;

using jQueryTmpl.Templates;

namespace jQueryTmpl
{
    public class TemplateCache
    {
        private readonly IDictionary<string, Template> _cache = new Dictionary<string, Template>();

        public void Store(string key, Template value)
        {
            _cache[key] = value;
        }
        
        public Template Retrieve(string key, Func<Template> getValue = null)
        {
            Template value;
            
            if(!_cache.TryGetValue(key, out value) && getValue != null)
                value = _cache[key] = getValue();

            return value;
        }
    }
}