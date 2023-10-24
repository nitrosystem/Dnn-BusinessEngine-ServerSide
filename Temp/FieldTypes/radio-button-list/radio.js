var bField_RadioButtonList = function ($scope, field) {
    this.init = function () {
        if (field.Settings.DataSource && !field.Settings.DataSource.TextField) field.Settings.DataSource.TextField = 'Text';
        if (field.Settings.DataSource && !field.Settings.DataSource.ValueField) field.Settings.DataSource.ValueField = 'Value';
    }
}