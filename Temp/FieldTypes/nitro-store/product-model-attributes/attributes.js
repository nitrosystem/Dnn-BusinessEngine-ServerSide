var bField[FIELDNAME] = function ($scope, field) {
    this.init = function () {
        field.Value = field.Value || [];
    }

    $scope.$watch(field.Settings.DataSource.DataName, function (newValue, oldValue) {
        if (newValue != oldValue) {
            field.Options = newValue;
            var values = [];
            angular.forEach(field.Options, function (attribute) {
                var vals = [];
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
            });

            field.Value = values;
        }
    }, true);
}