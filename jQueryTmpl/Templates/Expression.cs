using System;

namespace jQueryTmpl.Templates
{
    public class Expression
    {
        public bool Dollar;
        public string Member;
        public Expression[] Arguments;
        public Expression[] Indices;
        public Expression Next;

        public override string ToString()
        {
            return (Dollar ? "$" : String.Empty)
                + (Member)
                + (Arguments.Length > 0 ? "(" + String.Join<Expression>(", ", Arguments) + ")" : String.Empty)
                + (Indices.Length > 0 ? "[" + String.Join<Expression>(",", Indices) + "]" : String.Empty)
                + (Next != null ? "." + Next : String.Empty);
        }
    }
}