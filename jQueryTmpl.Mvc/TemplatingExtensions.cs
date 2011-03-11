using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace jQueryTmpl.Mvc
{
    public static class TemplatingExtensions
    {
        public static string RenderTemplate(this HtmlHelper helper, string partialViewName, object model)
        {
            var partial = helper.Partial(partialViewName, model).ToHtmlString();
            return TemplateEngine.Render(partial, model);
        }
    }
}