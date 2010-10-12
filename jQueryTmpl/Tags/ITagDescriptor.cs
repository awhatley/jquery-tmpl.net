using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public interface ITagDescriptor
    {
        Regex Pattern { get; }
        bool IsStartTag { get; }
        bool IsEndTag { get; }
        bool CanClose(ITagDescriptor descriptor);
        Template CreateTemplate(Token token);
        bool IsFinalCloseFor(ITagDescriptor descriptor);
    }
}