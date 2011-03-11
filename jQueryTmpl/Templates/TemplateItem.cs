using System;

namespace jQueryTmpl.Templates
{
    public class TemplateItem
    {
        public TemplateItem Item { get { return this; } }
        public object Data { get; set; }
        public object Options { get; set; }
        public TemplateItem Parent { get; set; }
        public Func<string, bool, Template> Html { get; set; }
        public Parameter ValueParameter { get; set; }
        public object Value { get; set; }
        public Parameter IndexParameter { get; set; }
        public int Index { get; set; }
    }
}