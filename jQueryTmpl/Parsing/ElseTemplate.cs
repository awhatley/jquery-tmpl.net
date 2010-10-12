using System;
using System.Linq;

using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class ElseTemplate : Template
    {
        public ElseTemplate(Token token) : base(token) { }

        public override string Render(object item)
        {
            var propertyName = GetDataProperty();
            var condition = propertyName.Length > 0 ? Boolean.Parse(Convert.ToString(GetDataValue(item))) : true;

            var trueChildren = Children.TakeWhile(c => !(c is ElseTemplate));
            var elseTemplate = Children.FirstOrDefault(c => c is ElseTemplate);

            if(condition)
                return Render(trueChildren, item);

            if(elseTemplate != null)
                return elseTemplate.Render(item);
            
            return String.Empty;
        }
    }
}