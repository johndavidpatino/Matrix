<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReportePSTSinProduccion.aspx.vb" Inherits="WebMatrix.ReportePSTSinProduccion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
               monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
               monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
           });

           $("#<%= txtFechaTerminacion.ClientID%>").mask("99/99/9999");
           $("#<%= txtFechaTerminacion.ClientID%>").datepicker({
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
               monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
               monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
           });

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

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
            <div id="accordion">
                <div id="accordion0">


                    <h3><a href="#">Reporte PST Sin Produccion</a></h3>
                    <div class="block">
                         <div class="form_left2">
                            <fieldset>
                                <label>
                                    Fecha de Inicio</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left2">
                            <fieldset>
                                <label>
                                    Fecha de Finalización</label>
                                <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <asp:button ID="BtnConsultar" runat="server" Text="Consultar" />
                            <asp:button ID="btnExport" runat="server" Text="Exportar" />
                        </div>

                        <asp:GridView ID="GVPersonas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="ID" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                                <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>