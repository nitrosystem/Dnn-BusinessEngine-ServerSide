<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dashboard.ascx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.Dashboard.Dashboard" %>

<asp:PlaceHolder ID="pnlStyles" runat="server"></asp:PlaceHolder>

<div b-ng-app="BusinessEngineClientApp" data-module="<%=this.ModuleGuid%>" id="pnlDashboard<%=this.ModuleGuid%>" ng-controller="dashboardController as $" ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')" class="b-engine-module <%=this.bRtlCssClass%>">
    <div id="pnlBusinessEngine<%=this.ModuleGuid%>" data-module="<%=this.ModuleGuid%>" ng-controller="moduleController as $"
        ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')">
    </div>
    <div id="dashboardNgScripts"></div>
</div>

<!-- ////////////////// -->
<!-- angularjs 1.8.2-->
<!-- ////////////////// -->
<script src="/DesktopModules/BusinessEngine/client-resources/framework/angular/angular.js?ver=<%=this.Version%>"></script>
<script src="/DesktopModules/BusinessEngine/client-resources/framework/angular/angular-sanitize.min.js?ver=<%=this.Version%>"></script>

<!-- ////////////////// -->
<!-- lodash 4.17.15-->
<!-- ////////////////// -->
<script src="/DesktopModules/BusinessEngine/client-resources/libraries/lodash/lodash.min.js?ver=<%=this.Version%>"></script>

<!-- ////////////////// -->
<!-- momentjs 4.17.15-->
<!-- ////////////////// -->
<script src="/DesktopModules/BusinessEngine/client-resources/libraries/momentjs/moment.min.js?ver=<%=this.Version%>"></script>
<script src="/DesktopModules/BusinessEngine/client-resources/libraries/moment-jalali/moment-jalaali.js?ver=<%=this.Version%>"></script>

<script type="text/javascript">
    window.bEngineGlobalSettings = {
        siteRoot: '<%=this.SiteRoot%>',
        apiBaseUrl: '<%=this.ApiBaseUrl%>',
        modulePath: '/DesktopModules/BusinessEngine/',
        userID: parseInt('<%=this.UserId%>'),
        version: '<%=this.Version%>',
        debugMode: false
    };

    var bAppRegistered = [];
    $(document).ready(function () {
        $('*[b-ng-app]').each(function () {
            const module = $(this).data('module');
            if (module && bAppRegistered.indexOf(module) == -1) {
                angular.bootstrap($(this), ['BusinessEngineClientApp']);

                bAppRegistered.push(module);
            }
        });
    });
</script>

<!-- ////////////////// -->
<!-- client app-->
<!-- ////////////////// -->
<link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-app/main.css?ver=<%=this.Version%>" />
<%--<link rel="stylesheet" type="text/css" href="http://localhost:6060/main.css" />--%>

<script src="/DesktopModules/BusinessEngine/client-app/app.bundle.js?ver=<%=this.Version%>"></script>
<%--<script src="http://localhost:6060/app.bundle.js?ver=<%=this.Version%>"></script>--%>

<asp:PlaceHolder ID="pnlScripts" runat="server"></asp:PlaceHolder>

