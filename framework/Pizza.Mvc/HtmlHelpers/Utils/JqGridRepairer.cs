using System.Globalization;

namespace Pizza.Mvc.HtmlHelpers.Utils
{
    public class JqGridRepairer
    {
        private const string configurationToReplaceTemplate = ".datepicker({{changeYear:true, onSelect: function() {{var sgrid = $('#{0}')[0]; sgrid.triggerToolbar();}},dateFormat:'dd-mm-yy'}});}}";
        private const string bootstrapDatePickerConfigTemplate =
            ".datepicker({{language: '{0}', clearBtn: true, orientation: 'bottom', autoclose: true}}).on('hide', function(e){{var sgrid = $('#{1}')[0]; sgrid.triggerToolbar();}});}}";

        private readonly string configurationToReplace;
        private readonly string validBootstrapDatepickerConfiguration;

        public JqGridRepairer(string gridId)
        {
            this.configurationToReplace = string.Format(configurationToReplaceTemplate, gridId);
            this.validBootstrapDatepickerConfiguration = string.Format(bootstrapDatePickerConfigTemplate, CultureInfo.CurrentCulture.Name, gridId);
        }

        public string FixDatepickerConfiguration(string rawGridMarkup)
        {
            var gridMarkup = rawGridMarkup.Replace(configurationToReplace, validBootstrapDatepickerConfiguration);
            return gridMarkup;
        }
    }
}