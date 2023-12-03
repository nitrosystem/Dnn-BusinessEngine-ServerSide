<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="studio.aspx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.StudioPage.Studio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="BusinessEngineStudioApp">
<head runat="server">
    <title>Business Engine Studio</title>
</head>
<body>
    <div ng-controller="studioController as $">
        <studio></studio>
    </div>

    <script type="text/javascript">
        var require = { paths: { 'vs': '/DesktopModules/BusinessEngine/client-resources/components/monaco-editor/0.44.0/vs' } };

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

    <asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="pnlResources" runat="server"></asp:PlaceHolder>
</body>
</html>
