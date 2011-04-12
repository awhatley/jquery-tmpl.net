using System;

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

        public TemplateItem Item
        {
            get { return _item; }
        }

        public object Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return Convert.ToString(_value);
        }
    }
}