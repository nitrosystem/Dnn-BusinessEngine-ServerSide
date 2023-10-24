var bField_AdmDateTimePicker = function ($scope, field) {
    field.Options = {
        autoClose: field.Settings.AutoClose,
        calType: field.Settings.CalendarType,
        format: field.Settings.Format,
        dtpType: field.Settings.DateType,
        placeholder: field.Settings.Placeholder,
        class: field.Settings.Theme
    };

    if (field.Value) field.ValueString = field.Settings.CalendarType === 'jalali' ? moment(field.Value).format('jYYYY/jM/jD') : moment(field.Value).format(field.Settings.Format);

    $scope.$watch('Field.' + field.FieldName + '.ValueString', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            var jFormat = field.Settings.Format.replace('y', 'jY').replace('Y', 'jY').replace('M', 'jM').replace('d', 'jD').replace('D', 'jD');
            field.Value = field.Settings.CalendarType === 'jalali' ? moment(newVal, jFormat).format('MM/DD/YYYY') : field.ValueString;
        }
    });
}