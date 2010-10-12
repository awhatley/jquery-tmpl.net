using System;
using System.Runtime.Serialization;

namespace jQueryTmpl.Rendering
{
    [Serializable]
    public class TemplateRenderingException : Exception
    {
        public TemplateRenderingException() { }
        public TemplateRenderingException(string message) : base(message) { }
        public TemplateRenderingException(string message, Exception inner) : base(message, inner) { }
        protected TemplateRenderingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}