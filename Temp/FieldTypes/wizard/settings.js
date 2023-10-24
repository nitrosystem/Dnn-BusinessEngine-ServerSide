var bWizardSettingsFunction = function ($scope, field) {
    this.preRender = function ($adminField, $userField) {
        var $index = 0;
        angular.forEach(field.Settings.Steps, function (s) {
            $adminField.find('#wizard' + field.FieldName + 'Panes').append($scope.moduleBuilderService.getBoardPane('WizardStep_' + s.Name + 'Pane_' + field.FieldID, s.Name + ' Pane', field.FieldName));
            $userField.find('#wizard' + field.FieldName + 'Panes').append('<div data-step="' + s.Name + '" ng-show="Field.' + field.FieldName + '.Settings.CurrentStep==\'' + s.Name + '\' && Field.' + field.FieldName + '.Settings.Steps[' + $index + '].IsShow && Field.' + field.FieldName + '.Settings.Steps[' + $index + '].IsEnable" data-pane="WizardStep_' + s.Name + 'Pane_' + field.FieldID + '" data-pane-title="' + s.Name + ' Pane" data-parent-id="' + field.FieldID + '" class="' + s.StepCss + '"></div>');

            $index++;
        })
    }

    $scope.onShowStepsClick = function () {
        field.Settings.Steps = field.Settings.Steps || [];

        $('#wnFieldSettings').modal('show')
    };

    $scope.onAddStepClick = function () {
        $scope.step = angular.copy({ Contents: [{ Content: '' }] });

        $('#wnEditStep').modal('show');
    };

    $scope.onEditStepClick = function (step) {
        $scope.oldStep = step;
        $scope.step = angular.copy(step);
        $scope.step.IsEdit = true;

        $('#wnEditStep').modal('show');
    };

    $scope.onSwapStepClick = function (from, to) {
        var temp = field.Settings.Steps[from];
        field.Settings.Steps[from] = field.Settings.Steps[to];
        field.Settings.Steps[to] = temp;
    };

    $scope.onDeleteStepClick = function ($index) {
        field.Settings.Steps.splice($index, 1);
    };

    $scope.onSaveStepClick = function () {
        if ($scope.step.IsEdit)
            field.Settings.Steps[field.Settings.Steps.indexOf($scope.oldStep)] = $scope.step;
        else
            field.Settings.Steps.push($scope.step);

        delete $scope.step;
        delete $scope.oldStep;

        $('#wnEditStep').modal('hide');
    };

    $scope.onStepActionChange = function () {
        var index = $scope.step.ContentscurrentRow || 0;

        if ($scope.stepAction == 'updateItem') {
            $scope.stepContentUpdateActions = angular.copy([]);
            $('#wnStepContentUpdateActions').modal('show');
        }
        else {
            var content = $scope.step.Contents[index].Content ? $scope.step.Contents[index].Content : '';
            $scope.step.Contents[index].Content = content + $scope.stepAction;
        }

        delete $scope.stepAction;
    };

    $scope.onStepContentConditionsClick = function (content) {
        $scope.stepContentConditions = angular.copy(content.Conditions || []);
        $('#wnStepContentConditions').modal('show');
    };

    $scope.onSaveStepContentConditionClick = function () {
        var conditions = $filter('filter')($scope.stepContentConditions, function (c) { return !c.IsHide });

        var index = $scope.step.ContentscurrentRow || 0;
        $scope.step.Contents[index].Conditions = conditions;

        $('#wnStepContentConditions').modal('hide');

        delete $scope.stepContentConditions;
    };

    $scope.onupdateItemActionsClick = function (content) {
        $scope.stepContentUpdateActions = angular.copy(content.UpdateItemActions);
        $('#wnStepContentUpdateActions').modal('show');
    };

    $scope.onSaveStepContentUpdateActionsClick = function () {
        var actions = $filter('filter')($scope.stepContentUpdateActions, function (c) { return !c.IsHide });

        var index = $scope.step.ContentscurrentRow || 0;
        $scope.step.Contents[index].UpdateItemActions = actions;

        $scope.step.Contents[index].Content = '<a href="" {Action:UpdateItem}>Update Item</a>';

        $('#wnStepContentUpdateActions').modal('hide');

        delete $scope.stepContentConditions;
    };

    $scope.onSaveSettingsClick = function () {
        field.Settings.WatchesCallBack = 'bWizard_raiseStepConditions()';

        $scope.onSaveFieldClick();

        $('#wnGridSteps').modal('hide');
    };
}
