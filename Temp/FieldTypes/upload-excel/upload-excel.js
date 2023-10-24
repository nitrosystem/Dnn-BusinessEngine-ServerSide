var bField_UploadExcel = function ($scope, field) {
    this.init = function () {
    }

    $scope.bUploadExcel_onUploadExcel = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
        if ($files.length || $newFiles.length || $duplicateFiles.length || $invalidFiles.length) {
            field.Value = null;

            angular.forEach($files, function (f) {
                uploadExcel(f);
            });
        }
    };

    function uploadExcel(file) {
        var headers = {
            TabId: $scope.ajaxHeaders.TabId,
            ModuleId: $scope.ajaxHeaders.ModuleId,
            'Content-Type': file.type
        }

        $scope.videoWait = true;

        var params = { files: file, FileUploadType: 1, Columns: field.Settings.Columns };

        $scope.service.uploadFiles1(bEngineGlobal.serviceUrl + 'Common/UploadExcel', headers, params).then(function (data) {
            if (data.status === 200 && data.data) {
                field.Value = data.data;

                field.Settings.ColumnList = field.Settings.Columns.split(',');
            }

            $scope.videoWait = false;
        }, function (error) {
            $scope.videoWait = false;
        }, function (data) {
            var progressPercentage = parseInt(100.0 * data.loaded / data.total);
        });
    }
}