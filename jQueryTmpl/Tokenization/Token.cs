using jQueryTmpl.Parsing;
using jQueryTmpl.Tags;

namespace jQueryTmpl.Tokenization
{
    public class Token
    {
        public string Value { get; private set; }
        public ITagDescriptor Descriptor { get; private set; }

        public bool IsStartTag  
        {
            get { return Descriptor.IsStartTag; }
        }

        public bool IsEndTag
        {
            get { return Descriptor.IsEndTag; }
        }

        public Token(string value, ITagDescriptor descriptor)
        {
            Value = value;
            Descriptor = descriptor;
        }

        public bool CanClose(Template template)
        {
            return Descriptor.CanClose(template.Descriptor);
        }

        public Template CreateTemplate()
        {
            return Descriptor.CreateTemplate(this);
        }

        public bool IsFinalCloseFor(Template template)
        {
            return Descriptor.IsFinalCloseFor(template.Descriptor);
        }
    }
}