var bField_DashboardMenu = function ($scope, field) {
    this.init = function () {
        setTimeout(function () {
            if (field.Settings.MenuType == 'horizontal') {
                KTLayoutHeaderMenu.init('menu' + field.FieldName, 'kt_header_menu_wrapper');
            }
            else {
                KTLayoutAsideMenu.init('menu' + field.FieldName);
            }
        }, 1000);
    }
}