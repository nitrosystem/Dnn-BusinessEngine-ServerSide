var bField_NitroStore_SubmitProductModelAttributes = function ($scope, field) {
    this.init = function () {
        field.Value = field.Value || [];

        $scope.$watch('ProductModel', function (newVal, oldVal) {
            if (newVal != oldVal) {
                $scope.service.runService($scope, '6969f858-6538-423d-8c12-a16fe52b50d4', { '@ProductGroupID': $scope.Product.ProductGroupID, '@ProductID': newVal.ProductID, '@ProductModelID': newVal.ProductModelID }).then(function (data) {
                    $scope.service.parseJsonItems(data.Data);

                    var values = [];
                    angular.forEach(data.Data, function (attribute) {
                        if (attribute.Value || (attribute.Value instanceof Array && attribute.Value.length)) {
                            var vals = [];
                            if (attribute.Value instanceof Array == false) attribute.Value = [attribute.Value];
                            angular.forEach(attribute.Value, function (val) {
                                if (typeof val != 'object')
                                    vals.push({ Value: val });
                                else
                                    vals.push(val);

                            });
                            attribute.Value = vals;

                            var value = {
                                AttributeID: attribute.AttributeID,
                                AttributeTitle: attribute.AttributeTitle,
                                Options: attribute.Value,
                                ViewOrder: attribute.ViewOrder
                            };

                            if (field.Value && field.Value.length) $.grep(field.Value, function (a) { return a.AttributeID == attribute.AttributeID }).map(function (a) {
                                value.Value = a.Value;
                            });

                            values.push(value);
                        }
                    });

                    $scope.ProductModelAttributes = values;
                });
            }
        });
    }
}