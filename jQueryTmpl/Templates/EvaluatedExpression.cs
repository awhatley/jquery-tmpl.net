using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace jQueryTmpl.Templates
{
    public class EvaluatedExpression
    {
        private readonly TemplateItem _item;
        private readonly object _value;

        public EvaluatedExpression(TemplateItem item, object value)
        {
            _item = item;
            _value = value;
        }

        public bool IsTruthy
        {
            get
            {
                return _value != null
                    && !_value.Equals(false)
                    && !_value.Equals(0)
                    && !_value.Equals(String.Empty);
            }
        }

        public object Value
        {
            get { return _value; }
        }

        public string ToHtmlEncodedString()
        {
            return HttpUtility.HtmlEncode(_value);
        }

        public override string ToString()
        {
            return Convert.ToString(_value);
        }

        public IEnumerable<TemplateItem> ToTemplateItemCollection(Parameter indexName, Parameter valueName)
        {
            var index = 0;
            var enumerable = _value as IEnumerable;
            
            if(enumerable == null)
                yield return new TemplateItem { 
                    Data = _item.Data, 
                    Html = _item.Html, 
                    Options = _item.Options, 
                    Parent = _item, 
                    Index = index, 
                    Value = _value 
                };

            else foreach(var item in enumerable)
                yield return new TemplateItem {
                    Data = item, 
                    Html = _item.Html, 
                    Options = _item.Options, 
                    Parent = _item,
                    Index = index++,
                    Value = item,
                };
        }
    }
}