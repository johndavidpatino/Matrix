<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReporteActividadesProduccion.aspx.vb" Inherits="WebMatrix.ReporteActividadesProduccion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
     <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
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

             $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
             $("#<%= txtFechaFinalizacion.ClientId %>").datepicker({
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
                    <h3><a href="#">Reporte Actividades Produccion</a></h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Inicio</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                                             
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Final</label>
                                <asp:TextBox ID="txtFechaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>

                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Trabajo</label>
                                <asp:TextBox ID="txttrabajo" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                
                            </fieldset>
                           
                        </div>

                    
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                        <asp:GridView ID="GvActividadesProduccion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="TrabajoId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="CodActividad" HeaderText="CodActividad" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="VrUnitario" DataFormatString="{0:C0}" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:C0}" HeaderText="Total" />
                             </Columns>
                        </asp:GridView>

                        &nbsp;&nbsp;&nbsp;
                         

                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>


           </asp:Content>
