var bField_ScanQrCode = function ($scope, field) {
    $scope.bButton_onClick = function (field, $event) {
        if (field.Settings.ButtonType == 'submit')
            $scope.validateForm();
        else {
            angular.forEach(field.Settings.ValidationGroups, function (group) {
                var field = $scope.getFieldByID(group);
                $scope.validateField(field);
            });
        }

        if (field.Actions && field.Actions.length) $scope.actionManagerService.callActionsByEvent($scope, 'OnButtonClick', field.Actions);

        if ($event) $event.stopPropagation();
    };
}