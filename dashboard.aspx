<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Dashboard" %>

<%@ Register TagPrefix="b" TagName="PageResource" Src="page-resources.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="BusinessEngineClientApp">
<head id="Head" runat="server" ViewStateMode="Disabled">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Business Engine Dashboard" />
    <title><%=this.PageTitle %></title>
</head>
<body id="<%=this.BodyID%>" class="<%=this.BodyClass%>">
    <b:PageResource id="CtlPageResource" runat="server" />
    <div id="pnlDashboard<%=this.ModuleGuid%>" ng-controller="dashboardController as $" ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>','<%=this.Today %>')">
        <div id="pnlBusinessEngine<%=this.ModuleGuid%>" data-module="<%=this.ModuleGuid%>" ng-controller="moduleController as $"
            ng-init="$.onInitModule(<%=this.DnnModuleID%>,'<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')">
        </div>
        <div id="dashboardNgScripts"></div>
    </div>

    <asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="pnlResources" runat="server"></asp:PlaceHolder>

    <script type="text/javascript">
        window.bEngineGlobalSettings.userDisplayName = "<%=this.UserDisplayName%>";
        window.bEngineGlobalSettings.userPhoto = "<%=this.UserPhoto%>";
        window.bEngineGlobalSettings.userRoles = "<%=this.UserRoles%>";
    </script>
</body>
</html>
