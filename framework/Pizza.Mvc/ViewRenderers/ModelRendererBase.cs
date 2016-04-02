using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using Pizza.Framework.Utils;

namespace Pizza.Mvc.ViewRenderers
{
    // TODO: rename derived classes
    internal abstract class ModelRendererBase
    {
        protected object model;
        protected HtmlHelper htmlHelper;
        protected HtmlTextWriter htmlTextWriter;

        protected ModelRendererBase(HtmlHelper html, object model)
        {
            this.htmlHelper = html;
            this.model = model;

            this.htmlTextWriter = new HtmlTextWriter(new StringWriter());
        }

        public MvcHtmlString Render()
        {
            if (this.model == null)
            {
                return MvcHtmlString.Empty;
            }

            var properties = this.model.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                this.RenderPropertySection(propertyInfo);
            }

            var htmlText = this.htmlTextWriter.InnerWriter.ToString();
            this.htmlTextWriter.Dispose();

            return MvcHtmlString.Create(htmlText);
        }

        protected abstract void RenderPropertySection(PropertyInfo propertyInfo);

        protected static bool MustBeHidden(PropertyInfo propertyInfo)
        {
            var hiddenInputAttribute = propertyInfo.GetAttribute<HiddenInputAttribute>();
            return (hiddenInputAttribute != null && !hiddenInputAttribute.DisplayValue);
            //return true;

            //var displayAttribute = propertyInfo.GetAttribute<DisplayAttribute>();
            //if (displayAttribute == null)
            //    return false;

            //return false;
        }

        protected static bool IsRenderedProperty(PropertyInfo propertyInfo)
        {
            var hiddenInputAttribute = propertyInfo.GetAttribute<HiddenInputAttribute>();
            if (hiddenInputAttribute != null && !hiddenInputAttribute.DisplayValue)
                return false;

            //var displayAttribute = propertyInfo.GetAttribute<DisplayAttribute>();
            //if (displayAttribute == null)
            //    return false;

            return true;
        }

        protected void AppendMvcHtmlString(MvcHtmlString htmlString)
        {
            if (htmlString != null)
            {
                this.htmlTextWriter.Write(htmlString.ToString());
            }
        }
    }
}