var bField_Wizard = function ($scope, field) {
    this.init = function () {
        if (field.Settings.Steps && field.Settings.Steps.length) field.Settings.CurrentStep = field.Settings.Steps[0].Name;

        $scope.wizardShowHideStep = function (stepIndex) {
            field.Settings.Steps[stepIndex].IsShow = $scope.service.checkConditions($scope, field.Settings.Steps[stepIndex].ShowConditions);

            var paneName = 'WizardStep_{0}Pane_{1}'.replace('{0}', field.Settings.Steps[stepIndex].Name).replace('{1}', field.FieldID);
            $scope.reInitPaneFields(paneName);
        };

        $scope.wizardEnableDisableStep = function (stepIndex) {
            field.Settings.Steps[stepIndex].IsEnable = $scope.service.checkConditions($scope, field.Settings.Steps[stepIndex].EnableConditions);

            var paneName = 'WizardStep_{0}Pane_{1}'.replace('{0}', field.Settings.Steps[stepIndex].Name).replace('{1}', field.FieldID);
            $scope.reInitPaneFields(paneName);
        };

        field.Step = {};

        for (var i = 0; i < field.Settings.Steps.length; i++) {
            field.Step[field.Settings.Steps[i].Name] = field.Settings.Steps[i];

            $scope.wizardShowHideStep(i);
            $scope.wizardEnableDisableStep(i);
        }
    }

    $scope.bWizard_onStepClick = function (field, step, $event) {
        var isTrue = true;

        var currentStep = $.grep(field.Settings.Steps, function (s) { return s.Name == field.Settings.CurrentStep })[0];
        var currentStepIndex = field.Settings.Steps.indexOf(currentStep);

        if (field.Settings.Steps.indexOf(step) > currentStepIndex) {
            var paneName = 'WizardStep_{0}Pane_{1}'.replace('{0}', field.Settings.CurrentStep).replace('{1}', field.FieldID);

            $scope.validatePane(paneName);

            isTrue = $scope.Pane[paneName].IsValid;
        }

        if (isTrue) {
            if (step.IsShow && step.IsEnable)
                field.Settings.CurrentStep = step.Name;
            else if ($event) {
                $event.stopPropagation();
                return false;
            }
        }
    };

    $scope.bWizard_raiseStepConditions = function () {
        var index = 0;
        angular.forEach(field.Settings.Steps, function (s) {
            angular.forEach(s.ShowConditions, function (c) {
                $scope.manageWatches(c.LeftExpression, 'wizardShowHideStep', index);
            });

            angular.forEach(s.EnableConditions, function (c) {
                $scope.manageWatches(c.LeftExpression, 'wizardEnableDisableStep', index);
            });

            index++;
        });
    };

    $scope.$on('onValidatePane', function (e, args) {
        var paneName = args.PaneName;

        var $stepContent = $('*[data-pane="' + paneName + '"]');
        if ($stepContent.data('step'))
            field.Step[$stepContent.data('step')].IsValid = args.IsValid;
    });

    field['NextStep'] = function () {
        setTimeout(function () {
            var steps = $.grep(field.Settings.Steps, function (s) { return s.IsShow && s.IsEnable });

            var currentStep = $.grep(steps, function (s) { return s.Name == field.Settings.CurrentStep })[0];
            var currentStepIndex = steps.indexOf(currentStep);

            if ((currentStepIndex + 1) < steps.length)
                field.Settings.CurrentStep = steps[currentStepIndex + 1].Name;

            $scope.$apply();
        }, 100);
    }

    field['PreviousStep'] = function () {
        var steps = $.grep(field.Settings.Steps, function (s) { return s.IsShow && s.IsEnable });

        var currentStep = $.grep(steps, function (s) { return s.Name == field.Settings.CurrentStep })[0];

        var currentStepIndex = steps.indexOf(currentStep);

        if (currentStepIndex > 0)
            field.Settings.CurrentStep = steps[currentStepIndex - 1].Name;
    }

    field['ValidateCurrentStep'] = function () {
        var currentStep = $.grep(field.Settings.Steps, function (s) { return s.Name == field.Settings.CurrentStep })[0];

        var pane = 'WizardStep_' + currentStep.Name + 'Pane_' + field.FieldID;

        $scope.validatePane(pane);

        field.Settings.CurrentStepIsValid = currentStep.IsValid;
    }
}
