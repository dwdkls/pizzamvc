using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;

namespace Pizza.Mvc.ViewRenderers
{
    internal sealed class ModelDisplayRenderer : ModelRendererBase
    {
        public ModelDisplayRenderer(HtmlHelper html, object model)
            : base(html, model)
        {
        }

        protected override void RenderPropertySection(PropertyInfo propertyInfo)
        {
            if (!MustBeHidden(propertyInfo))
            {
                this.htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Dt);
                this.htmlTextWriter.Write(propertyInfo.Name);
                this.htmlTextWriter.RenderEndTag();

                this.htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Dd);
                var value = this.htmlHelper.Value(propertyInfo.Name); //(this.htmlHelper.ViewContext.ViewData.Model));
                this.AppendMvcHtmlString(value);
                this.htmlTextWriter.RenderEndTag();
            }
        }
    }
}