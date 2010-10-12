using System;
using System.Runtime.Serialization;

namespace jQueryTmpl.Parsing
{
    [Serializable]
    public class TagMismatchException : Exception
    {
        public TagMismatchException() { }
        public TagMismatchException(string message) : base(message) { }
        public TagMismatchException(string message, Exception inner) : base(message, inner) { }
        protected TagMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}