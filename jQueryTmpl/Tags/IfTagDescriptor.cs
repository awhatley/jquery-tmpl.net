using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public class IfTagDescriptor : ITagDescriptor
    {
        public Regex Pattern
        {
            get { return new Regex(@"\{\{if\s+(?<data>[^}]+)\}\}"); }
        }

        public bool IsStartTag
        {
            get { return true; }
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
            return new IfTemplate(token);
        }

        public bool IsFinalCloseFor(ITagDescriptor descriptor)
        {
            return false;
        }
    }
}