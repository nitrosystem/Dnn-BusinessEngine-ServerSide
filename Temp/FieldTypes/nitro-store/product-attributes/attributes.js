var bField_NitroStore_ProductAttributes = function ($scope, field) {
    this.init = function () {
        $scope.$watch('Product', function (newVal, oldVal) {
            if (newVal != oldVal) {
                if (newVal) {
                    $scope.service.runService($scope, '234ed135-efc4-4568-b2d6-d32a8b40b409', { '@ProductID': newVal.ProductID, '@IsApproved': true }).then(function (data) {
                        $scope.service.parseJsonItems(data.Data);
                        $scope.ProductAttributes = data.Data;

                        $('#productAttributes' + field.FieldName).html($scope.$compile(field.Settings.LayoutTemplate)($scope));

                        $scope._Attribute = {};
                        angular.forEach($scope.ProductAttributes, function (a) {
                            $scope._Attribute[a.AttributeName] = a.Value;
                        });
                    });
                }
                else {
                    delete $scope.ProductAttributes;
                }
            }
        });
    }
}