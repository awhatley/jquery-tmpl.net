using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public class HtmlTagDescriptor : ITagDescriptor
    {
        public Regex Pattern
        {
            get { return new Regex(@"\{\{html\s+(?<data>\w+)\}\}"); }
        }

        public bool IsStartTag
        {
            get { return false; }
        }

        public bool IsEndTag
        {
            get { return false; }
        }

        public bool CanClose(ITagDescriptor descriptor)
        {
            return false;
        }

        public Template CreateTemplate(Token token)
        {
            return new HtmlTemplate(token);
        }
        
        public bool IsFinalCloseFor(ITagDescriptor descriptor)
        {
            return false;
        }
    }
}