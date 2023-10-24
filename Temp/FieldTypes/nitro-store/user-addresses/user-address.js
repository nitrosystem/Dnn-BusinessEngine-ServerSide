var bField_NitroStore_UserAddress = function ($scope, field) {
    this.init = function () {
        field.Address = {};
        field.state = 'default-address';
    }

    //Show Edit Address
    $scope.showEditAddress = function () {
        field.state = 'edit-address';
    }

    //Show Default Address
    $scope.showDefaultAddress = function () {
        field.state = 'default-address';
    }

    //Get User
    $scope.service.runService($scope, 'c4a8ebf3-b3d1-4532-b7ec-5a246987dcb0', { '@UserID': bEngineGlobal.userID }).then(function (data) {
        field.userInfo = data.Data;
    });

    //Get Provinces
    $scope.service.runService($scope, 'c313c954-c8a0-47f4-8c9a-9aa225d71286').then(function (data) {
        field.Provinces = data.Data;
    });

    //Get Cities
    $scope.showListCities = function (ProvinceID) {
        $scope.service.runService($scope, 'd8649051-f5e7-416c-9499-01cbfe84b5b3', { '@ProvinceID': ProvinceID }).then(function (data) {
            field.Cities = data.Data;
        });
    }

    //Get Row Address
    function getDefaultAddress() {
        $scope.service.runService($scope, 'd1e574de-b8a8-4a44-8e80-8231a621f421', { '@UserID': bEngineGlobal.userID }).then(function (data) {
            field.defaultAddress = data.Data;
        })
    }
    getDefaultAddress();

    //Get List Addresses
    function getListAddresses() {
        $scope.service.runService($scope, 'a4178de3-2f4f-49ad-b6ca-9af824075540', { '@UserID': bEngineGlobal.userID }).then(function (data) {
            field.listAddress = data.Data;
        })
    }
    getListAddresses();

    //Save Address
    $scope.saveAddress = function () {
        if (!field.Address.RecipientOrder) {
            field.Address.RecipientFirstName = field.userInfo.FirstName;
            field.Address.RecipientLastName = field.userInfo.LastName;
            field.Address.RecipientNationalCode = field.userInfo.NationalCode;
            field.Address.RecipientMobile = field.userInfo.Username;
        }
        $scope.service.runService($scope, '2ad5e277-8445-43b7-94f8-5bad87aae6e2', { '@AddressID': field.Address.AddressID, '@ProvinceID': field.Address.ProvinceID, '@CityID': field.Address.CityID, '@NeighbourhoodID': field.Address.NeighbourhoodID, '@Address': field.Address.Address, '@Plaque': field.Address.Plaque, '@Unit': field.Address.Unit, '@PostalCode': field.Address.PostalCode, '@RecipientFirstName': field.Address.RecipientFirstName, '@RecipientLastName': field.Address.RecipientLastName, '@RecipientNationalCode': field.Address.RecipientNationalCode, '@RecipientMobile': field.Address.RecipientMobile, '@RecipientOrder': field.Address.RecipientOrder, '@IsDefault': field.Address.IsDefault, '@UserID': bEngineGlobal.userID })
            .then(function () {
                $('#userAddressModal').modal('hide');
                getListAddresses()
            });
    }

    //Delete Address
    $scope.deleteAddress = function (address) {
        if (confirm("آیا از حذف آدرس مورد نظر اطمینان دارید؟")) {
            //Delete Address
            $scope.service.runService($scope, 'd9b29d87-4c11-4908-916b-fc0fa27871f3', { 'AddressID': address.AddressID }).then(function () {
                //Get List Addresses
                getListAddresses()
            });
        }
    }

    //Show Modal Address
    $scope.showModalAddress = function (address) {
        debugger;
        field.oldListAddress = [];
        field.Address = address;
        angular.copy(field.listAddress, field.oldListAddress);
        $scope.showListCities(address.CityID);
        $('#userAddressModal').modal('show');
    }

    //Set Default Address
    $scope.setAddressDefault = function (address) {
        $scope.service.runService($scope, '629e0d8c-0311-47b1-8d0d-c0b69bc74b95', { 'AddressID': address.AddressID, 'UserID': address.UserID }).then(function () {

            angular.forEach(field.listAddress, function (a) {
                a.IsDefault = null;
            });

            address.IsDefault = true;

            //Get Row Addresse
            getDefaultAddress();
        })
    }

    //Clear User Information
    $scope.clearUserInformation = function () {
        if (!field.Address.RecipientOrder) {
            field.Address.RecipientFirstName = null;
            field.Address.RecipientLastName = null;
            field.Address.RecipientNationalCode = null;
            field.Address.RecipientMobile = null;
        }
    }

    //Close Modal
    $scope.closeModalAddress = function () {
        field.listAddress = field.oldListAddress;
        $('#userAddressModal').modal('hide');
    }
}