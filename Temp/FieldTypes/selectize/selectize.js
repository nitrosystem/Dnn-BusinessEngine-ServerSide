var bField_Selectize = function ($scope, field) {
    this.init = function () {
        if (field.Settings.DataSource && !field.Settings.DataSource.TextField) field.Settings.DataSource.TextField = 'Text';
        if (field.Settings.DataSource && !field.Settings.DataSource.ValueField) field.Settings.DataSource.ValueField = 'Value';

        if (!field.Settings.AllowMultiple && field.Settings.ShowAll) field.Options.splice(0, 0, { OptionText: field.Settings.AllText, OptionValue: null });

        if (field.Settings.DataSource && field.Settings.DataSource.ListName) {
            $scope.$watch(field.Settings.DataSource.ListName, function (newVal, oldVal) {
                if (newVal != oldVal) {
                    field.Options = newVal;

                    if (!field.Settings.AllowMultiple && field.Settings.ShowAll) field.Options.splice(0, 0, { OptionText: field.Settings.AllText, OptionValue: null });
                }
            });
        }
    }
}