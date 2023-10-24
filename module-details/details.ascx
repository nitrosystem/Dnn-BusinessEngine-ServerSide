<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="details.ascx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.Details.Details" %>

<asp:PlaceHolder ID="pnlStyles" runat="server"></asp:PlaceHolder>

<script type="text/javascript">
    window.bEngineGlobalSettings = {
        siteRoot: '<%=this.SiteRoot%>',
        apiBaseUrl: '<%=this.ApiBaseUrl%>',
        modulePath: '/DesktopModules/BusinessEngine/',
        userID: parseInt('<%=this.UserId%>'),
        version: '<%=this.Version%>',
        debugMode: false
    };
</script>

<link rel="stylesheet" type="text/css" href="/DesktopModules/BusinessEngine/client-app/main.css?ver=<%=this.Version%>" />
<script src="/DesktopModules/BusinessEngine/client-app/app.bundle.js?ver=<%=this.Version%>"></script>

<asp:PlaceHolder ID="pnlScripts" runat="server"></asp:PlaceHolder>

<%if (this.IsSSR && this.IsDisabledFrontFramework)
    { %>
<div class="b-engine-module">
    <asp:PlaceHolder ID="pnlSSR1" runat="server"></asp:PlaceHolder>
</div>
<%} %>

<%else
    { %>
<div id="pnlBusinessEngine<%=this.ModuleGuid%>" data-module="<%=this.ModuleGuid%>" b-ng-app="BusinessEngineClientApp" ng-controller="moduleController as $" ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>','<%=this.IsSSR%>','<%=this.IsDisabledFrontFramework%>')" class="b-engine-module  <%=this.bRtlCssClass%>">
    <asp:PlaceHolder ID="pnlSSR2" runat="server"></asp:PlaceHolder>
</div>
<%} %>

<script type="text/javascript">
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