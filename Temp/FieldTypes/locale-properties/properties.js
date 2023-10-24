var bField_LocaleProperties = function ($scope, field) {
    var entityID;

    $scope.waiting = true;

    $scope.service.ajax({
        method: 'GET',
        url: bEngineGlobal.serviceUrl + 'Studio/GetLocaleProperties',
        headers: $scope.ajaxHeaders,
        params: { groupName: field.Settings.GroupName }
    }).then(function (data) {
        $scope.waiting = false;

        $scope['[FIELDNAME]_Properties'] = data;
    });

    $scope['[FIELDNAME]_onSubmitClick'] = function () {
        if (entityID) {
            var data = [];
            angular.forEach($scope['[FIELDNAME]_Properties'], function (property) {
                data.push({
                    Language: field._Language,
                    EntityID: entityID,
                    LocaleKeyGroup: field.Settings.GroupName,
                    LocaleKey: property.PropertyName,
                    LocaleValue: property.Value
                });
            });

            $scope.waiting = true;

            $scope.service.ajax({
                method: 'POST',
                url: bEngineGlobal.serviceUrl + 'Studio/SaveLocalizedProperties',
                headers: $scope.ajaxHeaders,
                data: data
            }).then(function (data) {
                $scope.waiting = false;
            });
        }
    };

    $scope['[FIELDNAME]_onLanguageChange'] = function () {
        entityID = $scope.service.getScopePropertyValue($scope, field.Settings.EntityPropertyName);
        if (entityID) {
            $scope.waiting = true;

            $scope.service.ajax({
                method: 'GET',
                url: bEngineGlobal.serviceUrl + 'Studio/GetLocalizedProperties',
                headers: $scope.ajaxHeaders,
                params: { language: field._Language, groupName: field.Settings.GroupName, entityID: entityID }
            }).then(function (data) {
                $scope.waiting = false;

                angular.forEach($scope['[FIELDNAME]_Properties'], function (property) {
                    var result = $.grep(data, function (p) { return p.LocaleKey == property.PropertyName });
                    property.Value = result.length ? result[0].LocaleValue : undefined;
                });
            });
        }
    };
}
