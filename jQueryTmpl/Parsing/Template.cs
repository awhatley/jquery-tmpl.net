using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using jQueryTmpl.Rendering;
using jQueryTmpl.Tags;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class Template
    {
        public string Value { get; private set; }
        public IList<Template> Children { get; private set; }
        public ITagDescriptor Descriptor { get; private set; }

        public Template()
        {
            Value = String.Empty;
            Descriptor = new LiteralTagDescriptor();
            Children = new List<Template>();
        }

        public Template(Token token)
        {
            Value = token.Value;
            Descriptor = token.Descriptor;
            Children = new List<Template>();
        }

        public void AddChild(Template template)
        {
            Children.Add(template);
            Value += template.Value;
        }

        public void Close(Token token)
        {
            Value += token.Value;
        }

        public virtual string Render(object item)
        {
            return Render(Children, item);
        }

        protected object GetDataValue(object item)
        {
            var propertyValue = item;
            var expressionParts = Descriptor.Pattern.Match(Value).Groups["data"].Value.Split('.');

            for(var i = 0; (i < expressionParts.Length) && (propertyValue != null); i++)
            {
                var propertyParts = expressionParts[i].Split('[', ']');
                var propertyName = propertyParts[0].Trim();
                var propertyInfo = propertyValue.GetType().GetProperty(propertyName, 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if(propertyInfo == null)
                    return null;

                propertyValue = propertyInfo.GetValue(propertyValue, null);

                if(propertyValue is Array && propertyParts.Length > 1)
                    propertyValue = ((Array)propertyValue).GetValue(Int32.Parse(propertyParts[1]));
            }

            return propertyValue;
        }

        protected bool IsTruthy(object value)
        {
            return value != null 
                && !value.Equals(false) 
                && !value.Equals(0) 
                && !value.Equals(String.Empty);
        }

        protected string Render(IEnumerable<Template> templates, object item)
        {
            return String.Join(String.Empty, templates.Select(t => t.Render(item)));
        }

        protected string Render(IEnumerable<Template> templates, IEnumerable<object> items)
        {
            return String.Join(String.Empty, items.Select(item => Render(templates, item)));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}