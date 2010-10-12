using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public class EachTagDescriptor : ITagDescriptor
    {
        public Regex Pattern
        {
            get { return new Regex(@"\{\{each\s+(?<data>\w+)\}\}"); }
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
            return new EachTemplate(token);
        }

        public bool IsFinalCloseFor(ITagDescriptor descriptor)
        {
            return false;
        }
    }
}