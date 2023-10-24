var bField_NitroStore_SearchAttributes = function ($scope, field) {
    var brandPageIndex = 1;

    this.init = function () {
        $scope.service.runService($scope, 'fbb7b2ca-143c-4be4-9503-0f4ee9346323', { '@PageIndex': 1, '@PageSize': 20 }).then(function (data) { // Get Brands
            $scope.service.parseJsonItems(data.Data);
            $scope._Brands = data.Data;

            $scope.service.runService($scope, '4940c2fa-47b0-49f4-9a5d-f2e35a695ea0', { '@PageIndex': 1, '@PageSize': 20, '@IsApproved': true }).then(function (data) { //Get Product Groups
                $scope.service.parseJsonItems(data.Data);
                $scope._ProductGroups = data.Data;

                $scope.service.runService($scope, '53005641-c201-472e-aa5a-6b0c98a17410', { '@IsSearchable': true }).then(function (data) { //Get Attributes
                    $scope.service.parseJsonItems(data.Data);
                    $scope.Attributes = data.Data;

                    bindData();
                });
            });
        });

        $scope._Filter = {};
        $scope.SliderOptions = {
            start: [0, 1000000],
            connect: true,
            step: 5000,
            range: {
                min: 0,
                max: 1000000
            }
        };
    }

    $scope.onProductGroupClick = function (group) {
        $scope._Filter.ProductGroup = group ? group.ProductGroupUrl : undefined;

        $scope.onFiltersChange();
    };

    $scope.onApplyPriceClick = function () {
        $scope._Filter.FromPrice = $scope.SliderOptions.start[0];
        $scope._Filter.ToPrice = $scope.SliderOptions.start[1];

        $scope.onFiltersChange();
    };

    $scope.onFiltersChange = function () {
        var params = [];
        var attributes = [];
        var brands = [];

        if ($scope._Filter.ProductGroup) {
            params.push('/' + $scope._Filter.ProductGroup + '?');
        }
        else
            params.push('?');

        angular.forEach($scope._Brands, function (b) {
            if (b.IsSelected) brands.push(b.BrandID);
        });
        if (brands.length) params.push('brand=' + brands.join());

        if ($scope._Filter.SearchText) params.push('title=' + $scope._Filter.SearchText);

        if ($scope._Filter.IsAvaliable) {
            params.push('isavailable=' + true);
        }

        angular.forEach($scope._Attributes, function (a) {
            if (a.FieldTypeInSearch == 'textbox' && a._Filter) {
                attributes.push({ a: a.AttributeID, v: a._Filter, t: 1 });
            }
            else if (a.Options) {
                var vals = [];

                $.grep(a.Options, function (o) { return o.IsSelected }).map(function (o) {
                    vals.push(o.Value);
                });

                if (vals.length) attributes.push({ a: a.AttributeID, v: vals.join() });
            }
        });
        if (attributes.length) params.push('attribute=' + JSON.stringify(attributes));

        if ($scope._Filter.FromPrice) params.push('from=' + $scope._Filter.FromPrice);
        if ($scope._Filter.ToPrice) params.push('to=' + $scope._Filter.ToPrice);

        setTimeout(function () {
            $scope.$window.history.pushState({ pageTitle: document.title }, '', '/list' + params.join('&'));

            setTimeout(function () {
                $scope.callAction('GetProducts');

                bindData();
            });
        });
    };

    $scope.onBrandChange = function (brandName) {
        brandPageIndex = 1;

        $scope.getBrands(0, brandName);
    };

    $scope.getBrands = function (type, brandName) {
        $scope.service.runService($scope, 'fbb7b2ca-143c-4be4-9503-0f4ee9346323', { '@PageIndex': brandPageIndex++, '@PageSize': 20, '@BrandName': brandName }).then(function (data) {
            $scope.service.parseJsonItems(data.Data);
            if (type == 0)
                $scope._Brands = data.Data;
            else
                $scope._Brands = $scope._Brands.concat(data.Data);
        });
    }

    $scope.onRemoveFilterClick = function (filter) {
        switch (filter.Field) {
            case 1:
                delete $scope._Filter.SearchText;
                break;
            case 2:
                delete $scope._Filter.IsAvaliable;
                break;
            case 3:
                delete $scope._Filter.ProductGroup
                break;
            case 4:
                $.grep($scope._Brands, function (b) { return b.BrandID == filter.EntityID }).map(function (b) {
                    delete b.IsSelected;
                });
                break;
            case 5:
                $.grep($scope._Attributes, function (a) { return a.AttributeID == filter.EntityID }).map(function (a) {
                    if (a.FieldTypeInSearch == 'textbox')
                        delete a._Filter;
                    else {
                        $.grep(a.Options, function (o) { return o.Value == filter.SubEntityID }).map(function (o) {
                            delete o.IsSelected;
                        });
                    }
                });
                break;
            default:
        }

        $scope.onFiltersChange();
    };

    function bindData() {
        $scope.filters = [];

        var _title = $scope.service.getParameterByName('title');
        if (_title) {
            $scope._Filter.SearchText = _title;

            $scope.filters.push({ Title: _title, Field: 1 });
        }

        var _isAvailable = $scope.service.getParameterByName('isavailable');
        if (_isAvailable && _isAvailable == 'true') {
            $scope._Filter.IsAvaliable = true;

            $scope.filters.push({ Title: 'فقط کالاهای موجود', Field: 2 });
        }

        var _productGroup = $scope.service.getParameterByName('list');
        if (_productGroup) {
            $scope._Filter.ProductGroup = decodeURIComponent(_productGroup);

            $scope.filters.push({ Title: decodeURIComponent(_productGroup), Field: 3 });
        }

        var _brands = $scope.service.getParameterByName('brand');
        if (_brands) {
            angular.forEach(_brands.split(','), function (brandID) {
                $.grep($scope._Brands, function (b) { return b.BrandID == brandID }).map(function (b) {
                    b.IsSelected = true;

                    $scope.filters.push({ Title: 'انتشارات ' + b.BrandName, Field: 4, EntityID: b.BrandID });
                });
            });
        }

        var _attributes = $scope.service.getParameterByName('attribute');
        if (_attributes) {
            var attrs = JSON.parse(_attributes);
            angular.forEach(attrs, function (attribute) {
                $.grep($scope._Attributes, function (a) { return a.AttributeID == attribute.a }).map(function (a) {
                    if (attribute.t == 1 && attribute.v)
                        a._Filter = attribute.v;
                    else if (!attribute.t && attribute.v) {
                        angular.forEach(attribute.v.split(','), function (val) {
                            $.grep(a.Options, function (o) { return o.Value == val }).map(function (o) {
                                o.IsSelected = true;

                                $scope.filters.push({ Title: a.AttributeTitle + ': ' + val, Field: 5, EntityID: a.AttributeID, SubEntityID: o.Value });
                            });
                        });
                    }
                });
            });
        }
    }
}