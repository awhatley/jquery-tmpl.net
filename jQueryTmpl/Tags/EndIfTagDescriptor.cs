using System;
using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public class EndIfTagDescriptor : ITagDescriptor
    {
        public Regex Pattern
        {
            get { return new Regex(@"\{\{/if\}\}"); }
        }

        public bool IsStartTag
        {
            get { return false; }
        }

        public bool IsEndTag
        {
            get { return true; }
        }

        public bool CanClose(ITagDescriptor descriptor)
        {
            return descriptor is IfTagDescriptor || descriptor is ElseTagDescriptor;
        }

        public Template CreateTemplate(Token token)
        {
            throw new NotSupportedException("End tags cannot create templates.");
        }
        
        public bool IsFinalCloseFor(ITagDescriptor descriptor)
        {
            return descriptor is IfTagDescriptor;
        }
    }
}