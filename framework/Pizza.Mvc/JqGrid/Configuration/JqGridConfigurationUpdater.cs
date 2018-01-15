using System.Globalization;
using System.Text;

namespace Pizza.Mvc.JqGrid.Configuration
{
    public static class JqGridConfigurationUpdater
    {
        private const string jQueryDatepickerConfigurationTemplate = ".datepicker({{changeYear:true, onSelect: function() {{var sgrid = $('#{0}')[0]; sgrid.triggerToolbar();}},dateFormat:'dd-mm-yy'}});}}";
        private const string bootstrapDatePickerConfigurationTemplate = ".datepicker({{language: '{0}', clearBtn: true, orientation: 'bottom', autoclose: true}}).on('hide', function(e){{var sgrid = $('#{1}')[0]; sgrid.triggerToolbar();}});}}";

        public static string FixGridConfiguration(string rawGridMarkup, string gridId, string clearFiltersInGridText, string searchDropDownAllItemsText)
        {
            string jqueryDatePickerConfiguration = string.Format(jQueryDatepickerConfigurationTemplate, gridId);
            string bootstrapDatepickerConfiguration = string.Format(bootstrapDatePickerConfigurationTemplate, CultureInfo.CurrentCulture.Name, gridId);

            var sb = new StringBuilder(rawGridMarkup)
                .ApplyBootstrapTheme()
                .ReplaceJqueryDatepickerWithBootstrapDatepicker(jqueryDatePickerConfiguration, bootstrapDatepickerConfiguration)
                .SetClearFiltersInGridText(clearFiltersInGridText)
                .AddAllItemTextToSearchDropdown(searchDropDownAllItemsText);

            return sb.ToString();
        }

        private static StringBuilder ApplyBootstrapTheme(this StringBuilder sb)
        {
            return sb.Replace(".jqGrid({", ".jqGrid({\r\nguiStyle:'bootstrap',");
        }

        private static StringBuilder ReplaceJqueryDatepickerWithBootstrapDatepicker(this StringBuilder sb, string jqueryDatePickerConfiguration, string bootstrapDatepickerConfiguration)
        {
            return sb.Replace(jqueryDatePickerConfiguration, bootstrapDatepickerConfiguration);
        }

        private static StringBuilder SetClearFiltersInGridText(this StringBuilder sb, string clearFiltersInGridText)
        {
            return sb.Replace("caption:\"Clear\"", $"caption:\"{clearFiltersInGridText}\"")
                .Replace("title:\"Clear Search\"", $"title:\"{clearFiltersInGridText}\"");
        }

        private static StringBuilder AddAllItemTextToSearchDropdown(this StringBuilder sb, string searchDropDownAllItemsText)
        {
            return sb.Replace("value:\":;", $"value:\":{searchDropDownAllItemsText};");
        }
    }
}