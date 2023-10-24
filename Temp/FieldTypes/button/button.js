var bField_Button = function ($scope, field) {
    this.init = function () {
    }

    $scope.bButton_onClick = function (field, $event) {
        if (field.Settings.ButtonType == 'submit')
            $scope.validateForm();
        else {
            angular.forEach(field.Settings.ValidationGroups, function (group) {
                var field = $scope.getFieldByID(group);
                if (field) $scope.validateField(field);
            });

            angular.forEach(field.Settings.ValidationPanes, function (pane) {
                $scope.validatePane(pane);
            });
        }

        if (field.Actions && field.Actions.length) $scope.actionManagerService.callActionsByEvent($scope, 'OnButtonClick', field.Actions);

        if ($event) $event.stopPropagation();
    };
}