using System;

using jQueryTmpl.Parsing;
using jQueryTmpl.Rendering;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl
{
    public class TemplateEngine
    {
        public string Render(string template, object data)
        {
            if(template == null || data == null)
                return String.Empty;

            var tokens = new Tokenizer().Tokenize(template);
            var tmpl = new Parser().Parse(tokens);
            var result = new Renderer().Render(tmpl, data);

            return result;
        }
    }
}