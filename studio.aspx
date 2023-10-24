<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="studio.aspx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.StudioPage.Studio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="BusinessEngineStudioApp">
<head runat="server">
    <title>Business Engine Studio</title>
    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-resources/libraries/bootstrap-icons/bootstrap-icons.min.css?ver=<%=this.Version%>" />
</head>
<body>
    <div ng-controller="studioController as $">
        <studio></studio>
    </div>

    <asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>

    <!-- ////////////////// -->
    <!-- angularjs 1.8.2-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/framework/angular/angular.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- bootstrap 5.1.3-->
    <!-- ////////////////// -->
    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-resources/libraries/bootstrap/css/bootstrap.min.css?ver=<%=this.Version%>" />
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/bootstrap/js/bootstrap.bundle.min.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- jquery 3.6.0-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/jquery/jquery.min.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- jquery ui 1.13.1-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/jquery-ui/js/jquery-ui.min.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- lodash 4.17.15-->
    <!-- ////////////////// -->
    <script src="/DesktopModules/BusinessEngine/client-resources/libraries/lodash/lodash.min.js?ver=<%=this.Version%>"></script>

    <!-- ////////////////// -->
    <!-- monaco editor 0.27.0-->
    <!-- ////////////////// -->
    <script>var require = { paths: { 'vs': '/DesktopModules/BusinessEngine/client-resources/components/monaco-editor/vs' } };</script>
    <script src="/DesktopModules/BusinessEngine/client-resources/components/monaco-editor/vs/loader.js?ver=<%=this.Version%>"></script>
    <script src="/DesktopModules/BusinessEngine/client-resources/components/monaco-editor/vs/editor/editor.main.nls.js?ver=<%=this.Version%>"></script>
    <script src="/DesktopModules/BusinessEngine/client-resources/components/monaco-editor/vs/editor/editor.main.js?ver=<%=this.Version%>"></script>

    <script type="text/javascript">
        window.bEngineGlobalSettings = {
            scenarioName: '<%=this.ScenarioNameParam%>',
            portalID: parseInt('<%=this.PortalIDParam%>'),
            portalAliasID: parseInt('<%=this.PortalAliasIDParam%>'),
            dnnModuleID: parseInt('<%=this.DnnModuleIDParam%>'),
            moduleID: '<%=this.ModuleIDParam%>',
            moduleType: '<%=this.ModuleTypeParam%>',
            scenarioID: '<%=this.ScenarioID%>',
            siteRoot: '<%=this.SiteRoot%>',
            apiBaseUrl: '<%=this.ApiBaseUrl%>',
            modulePath: '/DesktopModules/BusinessEngine/',
            version: '<%=this.Version%>',
            debugMode: false
        };
    </script>

    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/studio/main.css?ver=<%=this.Version%>" />
    <script type="text/javascript" src="/DesktopModules/BusinessEngine/studio/studio.bundle.js?ver=<%=this.Version%>"></script>

    <link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/extensions/basic/basic.css?ver=<%=this.Version%>" />
    <script type="text/javascript" src="/DesktopModules/BusinessEngine/extensions/basic/basic.bundle.js?ver=<%=this.Version%>"></script>

    <script type="text/javascript" src="/DesktopModules/BusinessEngine/extensions/providers/smsir-template-base.bundle.js?ver=<%=this.Version%>"></script>



    <%--For Debug--%>

   <%-- <link rel="stylesheet" type="text/css" href="http://localhost:8080/main.css" />
    <script type="text/javascript" src="http://localhost:8080/studio.bundle.js"></script>

    <link rel="stylesheet" type="text/css" href="http://localhost:9256/main.css" />
    <script type="text/javascript" src="http://localhost:9256/basic.bundle.js"></script>--%>
</body>
</html>
