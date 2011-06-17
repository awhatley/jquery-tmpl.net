using System.Linq;
using System.Text.RegularExpressions;

namespace jQueryTmpl.Templates
{
    public class ExpressionParser
    {
        private static readonly Regex Pattern = new Regex(@"(?<dollar>\$)?(?<member>\w+)(?:\((?<args>[^)]+)\))?(?:\[(?<index>[^\]]+)\])?(?:\s*(?<operator>==|!=|>=?|<=?)?\s*)(?=\.)?|(?<member>[""'][^""']+[""'])|(?<operator>!)");

        public Expression Parse(string expression)
        {
            Expression prev = null, root = null;
            var matches = Pattern.Matches(expression.Trim());
            foreach(Match m in matches)
            {
                var current = new Expression {
                    Dollar = m.Groups["dollar"].Value.Length > 0, 
                    Member = m.Groups["member"].Value,
                    Indices = ParseParameters(m.Groups["index"].Value),
                    Arguments = ParseParameters(m.Groups["args"].Value),
                    Operator = m.Groups["operator"].Value,
                };

                if(prev != null)
                    prev.Next = current;

                else
                    root = current;

                prev = current;
            }

            return root;
        }

        private Expression[] ParseParameters(string value)
        {
            return value
                .Split(',')
                .Where(s => s.Length > 0)
                .Select(Parse)
                .ToArray();
        }
    }
}