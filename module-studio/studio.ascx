<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="studio.ascx.cs" Inherits="NitroSystem.Dnn.BusinessEngine.Web.Modules.Studio" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<div id="bEngineStudioBoxs" class="container studio-type-boxs">
    <div class="row">
        <div class="col-sm-4">
            <div class="st-box">
                <small class="note-ribbon">Recommended</small>
                <img src="/Portals/0/Images/logo.png" />
                <hr />
                <p class="box-text">Open studio in standalone page(high performance)</p>
                <button type="button" class="box-button" data-mode="1">Goto Admin Panel</button>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="st-box">
                <img src="/Portals/0/Images/logo.png" />
                <hr />
                <p class="box-text">Open studio on dnn full screen(with dnn opetions)</p>
                <button type="button" class="box-button" data-mode="2">Goto Admin Panel</button>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="st-box">
                <small class="note-ribbon not-recommended">Not Recommended</small>
                <img src="/Portals/0/Images/logo.png" />
                <hr />
                <p class="box-text">Open studio on dnn(bad ui & ux screen)</p>
                <button type="button" class="box-button" data-mode="3">Goto Admin Panel</button>
            </div>
        </div>
    </div>
</div>

<div id="bEngineStudioApp"></div>

<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/BusinessEngine/Scripts/studio-setup.js"></dnn:DnnJsInclude>

<script type="text/javascript">
    window.bEngineGlobalSettings = {
        scenarioName: '<%=this.ScenarioNameParam%>',
        portalID: parseInt('<%=this.PortalId%>'),
        portalAliasID: parseInt('<%=this.PortalAlias.PortalAliasID%>'),
        dnnModuleID: parseInt('<%=this.ModuleId%>'),
        moduleID: '<%=this.ModuleGuid%>',
        moduleType: '<%=this.ModuleTypeParam%>',
        scenarioID: '<%=this.ScenarioID%>',
        siteRoot: '<%=this.SiteRoot%>',
        apiBaseUrl: '<%=this.ApiBaseUrl%>',
        modulePath: '/DesktopModules/BusinessEngine/',
        version: '<%=this.Version%>',
        debugMode: false
    };
</script>

