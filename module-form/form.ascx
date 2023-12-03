<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="form.ascx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Modules.Form" %>
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

<asp:PlaceHolder ID="pnlAntiForgery" runat="server"></asp:PlaceHolder>
