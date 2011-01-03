namespace jQueryTmpl.Templates
{
    public class ElseTemplate : IfTemplate
    {
        public override string Render(TemplateItem item)
        {
            return Expression == null
                ? Content.Render(item)
                : base.Render(item);
        }
    }
}