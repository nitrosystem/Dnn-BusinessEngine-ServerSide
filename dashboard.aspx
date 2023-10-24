<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" dir="rtl" ng-app="BusinessEngineClientApp">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Business Engine Dashboard" />
    <title><%=this.PageTitle %></title>

    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-resources/libraries/bootstrap-icons/bootstrap-icons.min.css?ver=<%=this.Version%>" />
    <asp:PlaceHolder ID="pnlStyles" runat="server"></asp:PlaceHolder>
</head>
<body id="<%=this.BodyID%>" class="<%=this.BodyClass%>">
    <div id="pnlDashboard<%=this.ModuleGuid%>" ng-controller="dashboardController as $" ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')">
        <div id="pnlBusinessEngine<%=this.ModuleGuid%>" data-module="<%=this.ModuleGuid%>" ng-controller="moduleController as $"
            ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')">
        </div>
        <div id="dashboardNgScripts"></div>
    </div>

    <asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>

    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/jquery/jquery.min.js?ver=<%=this.Version%>"></script>

    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/momentjs/moment.min.js?ver=<%=this.Version%>"></script>
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/moment-jalali/moment-jalaali.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- angularjs 1.8.2-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/framework/angular/angular.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- lodash 4.17.15-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/lodash/lodash.min.js?ver=<%=this.Version%>"></script>

    <script type="text/javascript">
        window.bEngineGlobalSettings = {
            portalID: parseInt('<%=this.PortalID%>'),
            userID: parseInt('<%=this.UserID%>'),
            userDisplayName: '<%=this.UserDisplayName%>',
            userPhoto:'<%=this.UserPhoto%>',
            userRoles: '<%=this.UserRoles%>',
            apiBaseUrl: '<%=this.ApiBaseUrl%>',
            modulePath: '/DesktopModules/BusinessEngine/',
            version: '<%=this.Version%>',
        };
    </script>

    <!-- ////////////////// -->
    <!-- client app-->
    <!-- ////////////////// -->
    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-app/main.css?ver=<%=this.Version%>" />
    <%--<link rel="stylesheet" type="text/css" href="http://localhost:6060/main.css" />--%>

    <script src="/DesktopModules/BusinessEngine/client-app/app.bundle.js?ver=<%=this.Version%>"></script>
    <%--<script src="http://localhost:6060/app.bundle.js?ver=<%=this.Version%>"></script>--%>

    <asp:PlaceHolder ID="pnlScripts" runat="server"></asp:PlaceHolder>
</body>
</html>
