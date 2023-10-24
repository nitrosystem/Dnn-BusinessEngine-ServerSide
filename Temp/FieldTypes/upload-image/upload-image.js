var bField_UploadImage = function ($scope, field) {
    this.init = function () {
    }

    $scope.$watch('Field.' + field.FieldName + '.Value', function (newVal, oldVal) {
        if (newVal != oldVal) {
            field.Files = angular.copy(newVal);
            if (field.Files && field.Files.length) field.Files.map(function (f) { f.Status = 1; })
        }
    });

    $scope.onUploadImages = function (field, $files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
        if (!field.Files || !field.Settings.AllowUploadMultipleFile) {
            field.Files = [];
        }

        var params = {};

        if (field.Settings.ResizeLargeImage) {
            params.ResizeLargeImage = true;
            params.LargeImageWidth = field.Settings.LargeImageWidth;
            params.LargeImageHeight = field.Settings.LargeImageHeight;
        }

        if (field.Settings.Thumbnails && field.Settings.Thumbnails.length) {
            params.CreateThumbnail1 = true;
            params.Thumbnail1Name = field.Settings.Thumbnails[0].Name;
            params.Thumbnail1Width = field.Settings.Thumbnails[0].Width;
            params.Thumbnail1Height = field.Settings.Thumbnails[0].Height;
        }

        if (field.Settings.Thumbnails && field.Settings.Thumbnails.length > 1) {
            params.CreateThumbnail2 = true;
            params.Thumbnail2Name = field.Settings.Thumbnails[1].Name;
            params.Thumbnail2Width = field.Settings.Thumbnails[1].Width;
            params.Thumbnail2Height = field.Settings.Thumbnails[1].Height;
        }

        if (field.Settings.EnableWatermark) {
            params.WatermarkImagePath = field.Settings.WatermarkImagePath;
            params.WatermarkPosition = field.Settings.WatermarkPosition;
            params.WatermarkOpacity = field.Settings.WatermarkOpacity;
            params.WatermarkMarginLeft = field.Settings.WatermarkMarginLeft;
            params.WatermarkMarginRight = field.Settings.WatermarkMarginRight;
            params.WatermarkMarginTop = field.Settings.WatermarkMarginTop;
            params.WatermarkMarginBottom = field.Settings.WatermarkMarginBottom;
        }

        var headers = {
            TabId: $scope.ajaxHeaders.TabId,
            ModuleId: $scope.ajaxHeaders.ModuleId,
        }

        $scope.service.uploadFiles(field.Files, "Photo", bEngineGlobal.serviceUrl + 'Common/UploadPhoto', headers, params, field.Settings.MaxFileCount, $files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event).then(function (data) {
            if (data && data.length) {
                setValue(field, data);
            }
        });
    };

    $scope.onDeleteImageClick = function (field, $index, $event) {
        field.Files.splice($index, 1);

        setValue(field, field.Files);

        if ($event) $event.stopPropagation();
    };

    $scope.onSetMainImageClick = function (field, file) {
        if (file.Status != 1) return;

        angular.forEach(field.Files, function (f) {
            f.IsMain = false;
        });

        file.IsMain = true;

        setValue(field, field.Files);
    };

    $scope.onTryAgainUploadImageClick = function (field, file, $event) {
        if (field.Value.length < field.Settings.MaxFileCount) {
            file.Status = 0;
            $scope.onUploadImages(fieldID);
        }

        if ($event) $event.stopPropagation();
    };

    function setValue(field, data) {
        var files = [];
        angular.forEach(data, function (f) {
            var file = {};
            file.Status = f.Status;
            file.PreviewImage = f.PreviewImage;
            file.Message = f.Message;
            file.File = f.File;

            if (f.Status == 1) {
                if (f.Data) {
                    file.FilePath = f.Data.FilePath;
                    file.Thumbnails = f.Data.Thumbnails;
                }
                else {
                    file.FilePath = f.FilePath;
                    file.Thumbnails = f.Thumbnails;
                    if (f.IsMain) file.IsMain = f.IsMain;
                }
            }

            files.push(file);
        });

        if (files.length && !$.grep(files, function (f) { return f.Status == 1 && f.IsMain }).length) {
            var filesUploaded = $.grep(files, function (f) { return f.Status == 1 });
            if (filesUploaded.length) filesUploaded[0].IsMain = true;
        }

        field.Files = files;
        field.Value = $.grep(field.Files, function (f) { return f.Status == 1 }).map(function (f) {
            var obj = {
                FileName: f.FileName,
                FilePath: f.FilePath,
                IsMain: f.IsMain,
                Thumbnails: f.Thumbnails
            }

            return obj;
        });

        if (!field.Value.length) field.Value = null;
    }
}
