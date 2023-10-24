var bField_TextBox = function ($scope, field) {
    this.init = function () {
        if ($scope.isFieldVisible(field.FieldName) && field.Settings && field.Settings.InputMask || field.Settings.InputMaskOptions) setInputMask();
    }

    angular.forEach(field.Settings.Attributes, function (attr) {
        $('#bTextbox' + field.FieldName).attr(attr.Name, attr.Value);
    });

    $scope.bTextBox_onKeyPress = function (field, $event) {
        if ($event.which === 13) {
            if (field.Actions && field.Actions.length) $scope.actionManagerService.callActionsByEvent($scope, 'OnEnterKey', field.Actions);

            if (field.Settings.EnterAction) $scope.service.callActionByActionID($scope, field.Settings.EnterAction);

            if (field.Settings.EnterButtonClick) {
                var buttonField = $scope.getFieldByID(field.Settings.EnterButtonClick);
                if (buttonField) $scope.bButton_onClick(buttonField, $event);
            }

            if ($event) {
                $event.preventDefault();
                return false;
            }
        }
    };

    $scope.bTextBox_onBlur = function (field, $event) {
        field.IsPatternValidate = true;
        $scope.validateField(field);

        if (field.Actions && field.Actions.length) $scope.actionManagerService.callActionsByEvent($scope, 'OnTextboxBlur', field.Actions);
    };

    $scope.bTextBox_onFocus = function (field, $event) {
        if (field.Actions && field.Actions.length) $scope.actionManagerService.callActionsByEvent($scope, 'OnTextboxFocus', field.Actions);
    };

    $scope.bTextBox_onTogglePassword = function (field) {
        field.Settings.InputType = field.Settings.InputType == 'password' ? 'text' : 'password';
        field.Settings.StayTogglePassword = true;
    };

    function setInputMask() {
        setTimeout(function () {
            if (field.Settings.InputMask) $('#bTextbox' + field.FieldName).inputmask({ regex: field.Settings.InputMask });
            if (field.Settings.InputMaskType) {
                var options = $scope.service.getJsonString(field.Settings.InputMaskOptions);
                $('#bTextbox' + field.FieldName).inputmask(field.Settings.InputMaskType, options);
            }
        }, 500);
    }
}
