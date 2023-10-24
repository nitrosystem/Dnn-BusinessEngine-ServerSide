var bField_CustomSelect = function ($scope, field) {
    this.init = function () {
        if (field.Settings.LayoutTemplate && field.Settings.ItemTemplate) {
            renderField();
        }

        $scope.$watch('Field.' + field.FieldName + '.IsShow', function (newVal, oldVal) {
            if (newVal && newVal != oldVal) {
                renderField();
            }
        });
    }

    function renderField() {
        var layoutTemplate = field.Settings.LayoutTemplate;
        var itemTemplate = field.Settings.ItemTemplate;

        if (field.Settings.CustomCss) {
            layoutTemplate = '<style type="text/css">' + field.Settings.CustomCss + '</style> \n ' + layoutTemplate;
        }

        layoutTemplate = layoutTemplate.replace('[ITEMTEMPLATE]', itemTemplate);
        layoutTemplate = layoutTemplate.replace('[ITEMSLOOP]', 'ng-repeat="option in Field.' + field.FieldName + '.Settings.Options" ng-click="onCustomSelectItemClick[FIELDNAME]($index)" ng-class="{\'active\':Field.' + field.FieldName + '.Value==($index+1)}"');
        layoutTemplate = layoutTemplate.replace(/{CustomField:(\w+)}/gim, '{{option.$1}}');

        $('#customSelect[FIELDNAME]').replaceWith($scope.$compile(layoutTemplate)($scope));
    };

    $scope.onCustomSelectItemClick[FIELDNAME] = function ($index) {
        field.Value = $index + 1;
    };
}