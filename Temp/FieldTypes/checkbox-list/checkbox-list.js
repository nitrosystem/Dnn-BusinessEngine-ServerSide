var bField_CheckBoxList = function ($scope, field) {
    this.init = function () {
        if (!field.Settings.DataSource.TextField) field.Settings.DataSource.TextField = 'Text';
        if (!field.Settings.DataSource.ValueField) field.Settings.DataSource.ValueField = 'Value';
    }

    $scope.bCheckBoxList_onOptionChange = function (field) {
        var values = [];
        angular.forEach($.grep(field.Options, function (o) { return o.Selected }), function (o) {
            values.push(o.Value);
        });

        field.Value = values.join();
    };

    $scope.$watch('[FIELD].Value', function (newValue, oldValue) {
        angular.forEach(field.Options, function (o) {
            o.Selected = false;

            if (field.Value) {
                $.grep(field.Value, function (a) { return o.Value == a }).map(function (a) {
                    o.Selected = true;
                });
            }
        });
    }, true);
}
