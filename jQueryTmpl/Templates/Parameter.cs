using System;

namespace jQueryTmpl.Templates
{
    public class Parameter
    {
        private readonly string _name;

        public Parameter(string name)
        {
            _name = name;
        }

        public bool Matches(string name)
        {
            return _name == name;
        }
    }
}