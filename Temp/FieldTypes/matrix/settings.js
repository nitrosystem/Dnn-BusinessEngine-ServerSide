var bMatrixSettingsFunction = function ($scope, field) {
    field.Settings.ValidationMethod = 'bMatrix_validateMatrix(field)';

    $scope.onShowMatrixColumnsClick = function () {
        field.Settings.Columns = field.Settings.Columns || [];

        angular.forEach(field.Settings.Columns, function (c) {
            if (typeof c.Options == 'object') c.Options = JSON.stringify(c.Options);
            if (typeof c.Settings == 'object') c.Settings = JSON.stringify(c.Settings);
        });

        $('#wnMartrixColumns').modal('show');
    };
}