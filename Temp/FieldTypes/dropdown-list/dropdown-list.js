var bField_DropDownList = function ($scope, field) {
    this.init = function () {
        if (field.Settings.DataSource && !field.Settings.DataSource.TextField) field.Settings.DataSource.TextField = 'Text';
        if (field.Settings.DataSource && !field.Settings.DataSource.ValueField) field.Settings.DataSource.ValueField = 'Value';

        if (!field.Settings.AllowMultiple && field.Settings.ShowAll && field.Options && field.Options.length)
            field.Options.splice(0, 0, { OptionText: field.Settings.AllText, OptionValue: null });
    }
}