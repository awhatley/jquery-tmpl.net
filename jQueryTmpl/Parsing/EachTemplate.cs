using System.Collections.Generic;

using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class EachTemplate : Template
    {
        public EachTemplate(Token token) : base(token) { }

        public override string Render(object item)
        {
            var value = GetDataValue(item);
            var array = (value as IEnumerable<object>) ?? new[] { value };

            return Render(Children, array);
        }
    }
}