var bField_Content = function ($scope, field) {
    if ($scope.isAdminPanel) {
        setTimeout(function () {
            let editor = ace.edit("pnlAce[FIELDNAME]");
            editor.setTheme("ace/theme/xcode");
            editor.getSession().setMode("ace/mode/html");
            editor.getSession().on('change', function () {
                $scope.$apply(function () {
                    field.Settings.Text = editor.getSession().getValue();
                });
            });
            if (field.Settings.Text)
                editor.getSession().setValue(field.Settings.Text);
        });
    }
}