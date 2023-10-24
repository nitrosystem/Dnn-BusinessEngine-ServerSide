var bField[FIELDNAME] = function ($scope, field) {
    this.init = function () {
        if (1 == 2 && field.Settings.AttributeName) {
            $scope.$watch(field.Settings.DataSource.DataName.replace('{', '').replace('}', ''), function (newValue, oldValue) {
                if (newValue != oldValue) {
                    $.grep(newValue, function (a) { return a.AttributeName == field.Settings.AttributeName }).map(function (a) {
                        field.Options = a.Value;
                    });
                }
            }, true);
        }
    }




    $scope.onAttributeValueChange = function () {
        debugger
    }
}