using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using jQueryTmpl.Tags;

namespace jQueryTmpl.Tokenization
{
    public class Tokenizer
    {
        private static readonly IEnumerable<ITagDescriptor> SupportedTags = new List<ITagDescriptor> {
            new PrintTagDescriptor(), 
            new IfTagDescriptor(), 
            new ElseTagDescriptor(),
            new EndIfTagDescriptor(),
            new EachTagDescriptor(),
            new EndEachTagDescriptor(),
            new HtmlTagDescriptor(),
        };

        public IEnumerable<Token> Tokenize(string template)
        {
            var matches = SupportedTags
                    .SelectMany(d => d.Pattern.Matches(template)
                        .Cast<Match>()
                        .Select(m => new { m.Index, m.Value, Descriptor = d }))
                    .OrderBy(t => t.Index);

            var index = 0;
            var tokens = new List<Token>();
            foreach(var match in matches)
            {
                if(match.Index > index)
                    tokens.Add(new Token(template.Substring(index, match.Index - index), new LiteralTagDescriptor()));

                tokens.Add(new Token(match.Value, match.Descriptor));
                index = match.Index + match.Value.Length;
            }

            if(index < template.Length)
                tokens.Add(new Token(template.Substring(index), new LiteralTagDescriptor()));

            return tokens;
        }
    }
}