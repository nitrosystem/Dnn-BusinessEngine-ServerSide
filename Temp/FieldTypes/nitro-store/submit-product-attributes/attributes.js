var bField_NitroStore_SubmitProductAttributes = function ($scope, field) {
    this.init = function () {
        $scope[field.FieldName + '_step'] = 0;

        $scope.$watch('Product', function (newVal, oldVal) {
            if (newVal != oldVal) {
                if (newVal) {
                    $scope.service.runService($scope, field.Settings.AttributesServiceID, { '@ProductID': newVal.ProductID, '@IsApproved': true }).then(function (data) {
                        $scope.service.parseJsonItems(data.Data);
                        angular.forEach(data.Data, function (attribute) {
                            attribute.Values = attribute.Value;
                            if (typeof attribute.Value == 'object') attribute.Value = JSON.stringify(attribute.Value);
                        })
                        $scope.ProductAttributes = data.Data;
                    });
                }
                else {
                    delete $scope.ProductAttributes;
                }
            }
        });
    }

    $scope[field.FieldName + '_onAttributeValueChange'] = function (attribute) {
        attribute.Value = JSON.stringify(attribute.Values);
    };
}