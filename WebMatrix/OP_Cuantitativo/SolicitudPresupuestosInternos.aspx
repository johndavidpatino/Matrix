<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="SolicitudPresupuestosInternos.aspx.vb" Inherits="WebMatrix.SolicitudPresupuestosInternos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         $('#accordion').accordion({
             change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
             header: "h3",
             autoHeight: false
         });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
 <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
            <label id="lblTextInfo">
            </label>
        </p>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
            <label id="lbltextError">
            </label>
        </p>
    </div>
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                               Muestras Por Ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                     <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                     <asp:HiddenField ID="hfidMetodologia" runat="server" />
                        <div class="form_left">
                            <fieldset>
                                <asp:Label ID="lblmetodologia" runat ="server" ></asp:Label> 
                            </fieldset>
                        </div>
                         &nbsp;
                        <label ></label>
                          &nbsp;
                          <label>Recuerde escribir todas las especificaciones para la generación del presupuesto</label>
                        <asp:TextBox ID="TxtObservacion" runat ="Server" Height="80px" Width="716px" 
                            TextMode="MultiLine" ></asp:TextBox>
                        &nbsp;
                        <fieldset>
                                <asp:Button ID="txtsolicitar" runat="server" Text="SolicitarPresupuesto" />
                            </fieldset>
                  </ContentTemplate>
    </asp:UpdatePanel>
        <script type="text/javascript">
            var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
            pageReqManger.add_initializeRequest(InitializeRequest);
            pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
