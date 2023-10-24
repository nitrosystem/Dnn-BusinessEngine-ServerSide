var bField_CloudZoomGallery = function ($scope, field) {
    this.init = function () {
        $('#cloudZoomGallery' + field.FieldName).hide();

        var layoutTemplate = field.Settings.LayoutTemplate;
        var thumbnailItemTemplate = field.Settings.ThumbnailItemTemplate;

        if (field.Settings.CustomCss) {
            layoutTemplate = '<style type="text/css">' + field.Settings.CustomCss + '</style> \n ' + layoutTemplate;
        }

        layoutTemplate = layoutTemplate.replace('[THUMBNAILS]', thumbnailItemTemplate);

        $('#cloudZoomGallery' + field.FieldName).append($scope.$compile(layoutTemplate)($scope));

        setTimeout(function () {
            $('#cloudZoomGallery' + field.FieldName).find('*[data-cloud-zoom], *[data-cloud-zoom-gallery]').CloudZoom({
                position: 'left'
            });

            $('#cloudZoomGallery' + field.FieldName).fadeIn(50);
        }, 1000);
    }
}