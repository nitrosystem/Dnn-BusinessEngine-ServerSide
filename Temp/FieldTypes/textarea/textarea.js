var bField_TextArea = function ($scope, field) {
    this.init = function () {
        if (field.Settings && field.Settings.InputMask || field.Settings.InputMaskOptions) setInputMask();

        if (!field.IsShow) {
            $scope.$watch('Field.' + field.FieldName + '.IsShow', function (newVal, oldVal) {
                if (newVal != oldVal && newVal) {
                    setInputMask();
                }
            });
        }

        function setInputMask() {
            setTimeout(function () {
                if (field.Settings.InputMask) $('#bTextarea' + field.FieldName).inputmask({ regex: field.Settings.InputMask });
                if (field.Settings.InputMaskType) {
                    var options = $scope.service.getJsonString(field.Settings.InputMaskOptions);
                    $('#bTextarea' + field.FieldName).inputmask(field.Settings.InputMaskType, options);
                }
            });
        }
    }
}