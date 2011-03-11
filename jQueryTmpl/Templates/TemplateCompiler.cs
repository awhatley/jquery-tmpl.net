using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace jQueryTmpl.Templates
{
    public class TemplateCompiler
    {
        private static readonly Regex ShortTag = new Regex(@"\$\{([^\}]*)\}");
        private static readonly Regex TokenPattern = new Regex(@"\{\{(?<slash>\/?)(?<tag>\w+|.)(?:\((?<parameters>(?:[^\}]|\}(?!\}))*?)?\))?(?:\s+(?<expression>.*?)?)?\s*\}\}");

        private readonly IDictionary<string, Func<Token, Queue<Token>, Template>> _builders;
        private readonly IDictionary<string, Template> _cache;
        private string _template;
        private int _index;

        public TemplateCompiler(IDictionary<string, Template> cache)
        {
            _cache = cache;
        
            _builders = new Dictionary<string, Func<Token, Queue<Token>, Template>> {
                { "=", BuildEval },
                { "if", BuildIf },
                { "else", BuildElse },
                { "each", BuildEach },
                { "html", BuildHtml },
                { "tmpl", BuildTmpl },
                { "wrap", BuildWrap },
            };
        }

        public Template Compile(string template)
        {
            _index = 0;
            _template = ShortTag.Replace(template, @"{{= $1}}");

            var tokens = TokenPattern.Matches(_template)
                .Cast<Match>()
                .Select(m => new Token {
                    Index = m.Index,
                    Length = m.Length,
                    Value = m.Value,
                    Closing = m.Groups["slash"].Length > 0,
                    Name = m.Groups["tag"].Value,
                    Parameters = m.Groups["parameters"].Value.Split(',')
                                  .Where(p => p.Length > 0)
                                  .Select(p => p.Trim()).ToArray(),
                    Expression = m.Groups["expression"].Value
                })
                .OrderBy(t => t.Index);

            return new EachTemplate {
                Content = Compile(new Queue<Token>(tokens)),
                Expression = new ExpressionParser().Parse("$data"),
            };
        }

        private Template Compile(Queue<Token> tokens, Func<Token, bool> stopCondition = null)
        {
            var templates = new List<Template>();
            while(tokens.Count > 0 && (stopCondition == null || !stopCondition(tokens.Peek())))
            {
                var current = tokens.Dequeue();

                if(current.Index > _index)
                    templates.Add(new LiteralTemplate { Value = _template.Substring(_index, current.Index - _index) });

                _index = current.Index + current.Length;

                if(current.Closing)
                    return new CompositeTemplate(templates);

                var builder = _builders[current.Name];
                var tmpl = builder(current, tokens);
                templates.Add(tmpl);
            }

            if(tokens.Count == 0 && _index < _template.Length)
                templates.Add(new LiteralTemplate { Value = _template.Substring(_index) });

            return new CompositeTemplate(templates);
        }

        private Template BuildTmpl(Token current, Queue<Token> remaining)
        {
            return new NestedTemplate {
                Data = new ExpressionParser().Parse(current.Parameters.ElementAtOrDefault(0) ?? "$data"),
                Options = new ExpressionParser().Parse(current.Parameters.ElementAtOrDefault(1) ?? "$options"),
                Template = _cache[current.Expression.Trim('"').Trim('\'')],
            };
        }

        private Template BuildEval(Token current, Queue<Token> remaining)
        {
            return new EvalTemplate { Expression = new ExpressionParser().Parse(current.Expression) };
        }

        private Template BuildEach(Token current, Queue<Token> remaining)
        {
            return new EachTemplate {
                IndexParameter = new Parameter(current.Parameters.ElementAtOrDefault(0) ?? "index"),
                ValueParameter = new Parameter(current.Parameters.ElementAtOrDefault(1) ?? "value"),
                Expression = new ExpressionParser().Parse(current.Expression),
                Content = Compile(remaining),
            };
        }

        private Template BuildIf(Token current, Queue<Token> remaining)
        {
            var elseFound = false;
            return new IfTemplate {
                Expression = new ExpressionParser().Parse(current.Expression),
                Content = Compile(remaining, t => elseFound = (t.Name == "else")),
                Else = elseFound && remaining.Count > 0 && remaining.Peek().Name == "else" ? BuildElse(remaining.Dequeue(), remaining) : new LiteralTemplate(),
            };
        }

        private Template BuildElse(Token current, Queue<Token> remaining)
        {
            _index = current.Index + current.Length;

            return new ElseTemplate {
                Expression = new ExpressionParser().Parse(current.Expression),
                Content = Compile(remaining, t => t.Name == "else"),
                Else = remaining.Count > 0 && remaining.Peek().Name == "else" ? BuildElse(remaining.Dequeue(), remaining) : new LiteralTemplate(),
            };
        }

        private Template BuildHtml(Token current, Queue<Token> remaining)
        {
            return new HtmlTemplate { Expression = new ExpressionParser().Parse(current.Expression) };
        }

        private Template BuildWrap(Token current, Queue<Token> remaining)
        {
            return new WrapTemplate {
                Data = new ExpressionParser().Parse(current.Parameters.ElementAtOrDefault(0)),
                Options = new ExpressionParser().Parse(current.Parameters.ElementAtOrDefault(1)),
                Content = Compile(remaining),
                Template = _cache[current.Expression.Trim('"').Trim('\'')],
            };
        }
    }

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

    public class Token
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string Value { get; set; }
        public bool Closing { get; set; }
        public string Name { get; set; }
        public string[] Parameters { get; set; }
        public string Expression { get; set; }
    }
}