var bField_SwitchButton = function ($scope, field) {
    this.init = function () {
        if (field.Settings.DataSource && !field.Settings.DataSource.TextField) field.Settings.DataSource.TextField = 'Text';
        if (field.Settings.DataSource && !field.Settings.DataSource.ValueField) field.Settings.DataSource.ValueField = 'Value';

    }

    $scope.onOptionChangeInSwitchButton = function (field) {
        if ($scope.moduleType === 'Search') {
            field.Value = field.Value ? true : false;
        }
    };
}