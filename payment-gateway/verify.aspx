<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verify.aspx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.PaymentGateway.Verify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="BusinessEngineApp">
<head>
    <title>
        <%=DotNetNuke.Entities.Portals.PortalSettings.Current.PortalName%>
    </title>
    <link href="/DesktopModules/BusinessEngine/ClientComponents/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/DesktopModules/BusinessEngine/Styles/components.css?ver=<%=this.Version%>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1">
        <div id="pnlBusinessDirectory<%=this.ModuleID%>" ng-controller="pVerifyController">
            <div id="pnlContent" runat="server"></div>
        </div>
    </form>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/jQuery/jquery-3.1.1.min.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/angularjs/angular.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/ng-file-upload/ng-file-upload-shim.min.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/ng-file-upload/ng-file-upload.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/angular-filter/angular-filter.min.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/momentjs/moment.min.js"></script>
    <script src="/DesktopModules/BusinessEngine/ClientComponents/momentjs/moment-jalaali.js"></script>
    <script type="text/javascript">
        var businessEngineApp = angular.module('BusinessEngineApp', ['ngFileUpload', 'angular.filter']);

        var bEngineGlobal = {
            portalID: parseInt('<%=this.PortalID%>'),
            userID: parseInt('<%=this.UserID%>'),
            baseUrl: '<%=this.BaseUrl%>',
            modulePath: '<%=this.BaseUrl%>DesktopModules/BusinessEngine/',
            serviceUrl: '<%=this.SiteRoot%>DesktopModules/BusinessEngine/API/',
            version: Math.random()
        };
    </script>
    <script src="/DesktopModules/BusinessEngine/ClientApp/services/services.js?ver=<%=this.Version%>"></script>
    <script src="/DesktopModules/BusinessEngine/ClientApp/services/action-manager.js?ver=<%=this.Version%>"></script>
    <script src="/DesktopModules/BusinessEngine/ClientApp/filters/filters.js?ver=<%=this.Version%>"></script>
    <script type="text/javascript">
        businessEngineApp.controller('pVerifyController', function ($scope, $filter, $timeout, $q, $compile, panelService, actionManagerService) {
            $scope.service = panelService;
           
            var paymentParamsStr = '<%=this.Q%>';
            if (paymentParamsStr) {
                var paymentParams = panelService.getJsonString(paymentParamsStr);

                panelService.parseJsonItems(paymentParams);

                $scope.ajaxHeaders = paymentParams.AjaxHeader;

                panelService.ajax({
                    method: 'GET',
                    url: bEngineGlobal.serviceUrl + 'Module/GetPVA',
                    params: { moduleID: paymentParams.ModuleID }
                }).then(function (data) {
                    $scope.actions = data;
                    angular.forEach($scope.actions, function (a) {
                        panelService.parseJsonItems(a);
                    });

                    paymentParams.TransactionNumber = '<%=this.TN%>';

                    for (param in paymentParams) {
                        $scope[param] = paymentParams[param];
                    }

                    let conditionalActions = $filter('filter')($scope.actions, function (a) { return a.PaymentMethodID == paymentParams.PaymentMethodID && (a.ActionDetails.PaymentResultMode == 0 || a.ActionDetails.PaymentResultMode == '<%=this.PS%>') });

                    actionManagerService.callActionsByEvent($scope, 'OnPaymentCompleted', $scope.actions, conditionalActions);
                });
            }
        });
    </script>
</body>
</html>
