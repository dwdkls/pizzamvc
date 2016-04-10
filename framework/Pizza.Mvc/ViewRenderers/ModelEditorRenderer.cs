using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using Pizza.Contracts;
using Pizza.Mvc.ViewRenderers.DropDown;
using Pizza.Utils;

namespace Pizza.Mvc.ViewRenderers
{
    internal sealed class ModelEditorRenderer : ModelRendererBase
    {
        readonly DropDownRendererBase dropDownRenderer;

        public ModelEditorRenderer(HtmlHelper html, object model, DropDownRendererBase dropDownRenderer)
            : base(html, model)
        {
            this.dropDownRenderer = dropDownRenderer;
        }

        protected override void RenderPropertySection(PropertyInfo propertyInfo)
        {
            if (MustBeHidden(propertyInfo))
            {
                this.RenderHiddenProperty(propertyInfo);
            }
            else
            {
                this.RenderVisibleProperty(propertyInfo);
            }
        }

        private void RenderHiddenProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo.Name == ObjectHelper.GetPropertyName<IViewModelBase>(q => q.Id))
            {
                return;
            }

            if (propertyInfo.Name == ObjectHelper.GetPropertyName<IVersionableEditModelBase>(q => q.Version))
            {
                byte[] value = (byte[])propertyInfo.GetValue(this.htmlHelper.ViewContext.ViewData.Model);
                string asString = Convert.ToBase64String(value);

                var hiddenVersion = this.htmlHelper.Hidden(propertyInfo.Name, asString);
                this.htmlTextWriter.Write(hiddenVersion);
            }
            else
            {
                var hidden = this.htmlHelper.Hidden(propertyInfo.Name);
                this.htmlTextWriter.Write(hidden);
            }
        }

        private void RenderVisibleProperty(PropertyInfo propertyInfo)
        {
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "form-group");
            this.htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            this.RenderLabel(propertyInfo.Name);
            this.RenderPropertyControl(propertyInfo);

            this.htmlTextWriter.RenderEndTag();
        }

        private void RenderLabel(string propertyName)
        {
            var labelString = this.htmlHelper.Label(propertyName, new { @class = "control-label col-sm-2" });
            this.AppendMvcHtmlString(labelString);
        }

        private void RenderPropertyControl(PropertyInfo propertyInfo)
        {
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-10");
            this.htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            if (IsEditableProperty(propertyInfo))
            {
                this.RenderEditableProperty(propertyInfo);
            }
            else
            {
                this.RenderReadonlyProperty(propertyInfo);
            }

            this.htmlTextWriter.RenderEndTag();
        }

        private void RenderReadonlyProperty(PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(this.model);
            MvcHtmlString editor;

            var dataType = propertyInfo.GetAttribute<DataTypeAttribute>();
            if (dataType != null && dataType.DataType == DataType.Html)
            {
                editor = this.htmlHelper.TextArea(propertyInfo.Name, value.ToString(), new { @class = "form-control", disabled = true });
            }
            else
            {
                editor = this.htmlHelper.TextBox(propertyInfo.Name, value, new { @class = "form-control", disabled = true });
            }

            this.AppendMvcHtmlString(editor);
        }

        private void RenderEditableProperty(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType.GetRealType();
            var value = propertyInfo.GetValue(this.model);

            if (typeof(Enum).IsAssignableFrom(type))
            {
                var dropDownHtml = this.dropDownRenderer.RenderDropDownForEnum(this.htmlHelper, propertyInfo, value);
                this.htmlTextWriter.Write(dropDownHtml);
            }
            else
            {
                this.RenderStandardEditorWIthValidator(propertyInfo.Name);
            }
        }

        private void RenderStandardEditorWIthValidator(string propertyName)
        {
            var editor = this.htmlHelper.Editor(propertyName, new { htmlAttributes = new { @class = "form-control" } });
            var validation = this.htmlHelper.ValidationMessage(propertyName, string.Empty, new { @class = "text-danger" });

            this.AppendMvcHtmlString(editor);
            this.AppendMvcHtmlString(validation);
        }

        private static bool IsEditableProperty(PropertyInfo propertyInfo)
        {
            var editableAttribute = propertyInfo.GetAttribute<EditableAttribute>();
            return editableAttribute == null || editableAttribute.AllowEdit;
        }
    }
}