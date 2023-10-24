var bCustomSelectSettingsFunction = function ($scope, field) {
    $scope.onShowCustomSelectSettings = function () {
        $('#wnCustomSelectSettings').modal('show');

        $scope.$broadcast('reInitialEditor');
    };

    $scope.customSelect_onSaveTemplateClick = function () {
        var customFields = [];
        angular.forEach(field.Settings.ItemTemplate.match(/{CustomField:(\w+):?(\w+)?}/gm), function (match) {
            var fieldName = /{CustomField:(\w+):?(\w+)?}/.exec(match)[1];

            customFields.push({ FieldName: fieldName });
        });

        field.Settings.CustomFields = customFields;
    };

    $scope.customSelect_onAddOptionClick = function () {
        field.Settings.Options = field.Settings.Options || [];

        field.Settings.Options.push({});
    };

    $scope.customSelect_onSaveOptionsClick = function () {
        $scope.onSaveFieldClick();

        $('#wnCustomSelectSettings').modal('hide');
    };
}
