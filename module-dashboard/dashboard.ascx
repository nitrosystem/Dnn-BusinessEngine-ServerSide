<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dashboard.ascx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.Dashboard" %>
<%@ Register TagPrefix="b" TagName="PageResource" Src="../page-resources.ascx" %>

<b:PageResource id="CtlPageResource" runat="server" />

<%if (this.IsSSR && this.IsDisabledFrontFramework)
    { %>
<div class="b-engine-module">
    <asp:PlaceHolder ID="pnlSSR1" runat="server"></asp:PlaceHolder>
</div>
<%} %>

<%else
    { %>
<div b-ng-app="BusinessEngineClientApp" data-module="<%=this.ModuleGuid%>" id="pnlDashboard<%=this.ModuleGuid%>" ng-controller="dashboardController as $" ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')" class="b-engine-module <%=this.bRtlCssClass%>">
    <div id="pnlBusinessEngine<%=this.ModuleGuid%>" data-module="<%=this.ModuleGuid%>" ng-controller="moduleController as $"
        ng-init="$.onInitModule('<%=this.ModuleGuid%>', '<%=this.ModuleName%>','<%=this.ConnectionID%>')">
        <asp:PlaceHolder ID="pnlSSR2" runat="server"></asp:PlaceHolder>
    </div>
    <div id="dashboardNgScripts"></div>
</div>
<%} %>

<asp:LinkButton ID="lnkOpenPanel"  CssClass="btn btn-primary margin-auto" Text="Goto Dashboard Panel" Visible="false" runat="server"/>

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

<asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>

