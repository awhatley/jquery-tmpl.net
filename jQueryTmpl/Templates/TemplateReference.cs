namespace jQueryTmpl.Templates
{
    public class TemplateReference
    {
        private readonly string _templateName;

        public TemplateReference(string templateName)
        {
            _templateName = templateName;
        }

        public Template Resolve()
        {
            return TemplateEngine.Lookup(_templateName);
        }
    }
}