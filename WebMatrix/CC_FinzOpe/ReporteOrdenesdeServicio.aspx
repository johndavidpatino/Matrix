<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReporteOrdenesdeServicio.aspx.vb" Inherits="WebMatrix.ReporteOrdenesdeServicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
      <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtfechainicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechainicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtfechafin.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechafin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });



            validationForm();

        }
        $(document).ready(function () {
            loadPlugins();
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
            <asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
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
                    <h3><a href="#">Reporte Ordenes de Servicio</a></h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Inicio</label>
                                <asp:TextBox ID="txtfechainicio" runat="server" CssClass="textEntry"></asp:TextBox>
                                 <label>
                                <br />
                                Contratista</label>
                                <asp:DropDownList ID="ddlContratista" runat="server"  ></asp:DropDownList>
                               
                                
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Fin</label>
                                <asp:TextBox ID="txtfechafin" runat="server" CssClass="textEntry"></asp:TextBox>
                            </fieldset>

                        </div>
                        <div class="form_left">
                            <fieldset>
                                 <label>Numero Orden</label>
                                <asp:TextBox ID="txtorden" runat="server" CssClass="textEntry"></asp:TextBox>
                               
                            </fieldset>

                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>Estado</label>
                                <asp:DropDownList ID="ddlestado" runat="server"  >
                                    <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                    <asp:ListItem Value="1">Recibida</asp:ListItem>
                                    <asp:ListItem Value="2">Autorizada para Pago</asp:ListItem>
                                    <asp:ListItem Value="3">Devuelta</asp:ListItem>
                                    <asp:ListItem Value="4">Sin Radicar</asp:ListItem>
                                </asp:DropDownList>
                                <label>Busqueda</label>
                                <asp:TextBox ID="txtTodosCampos" runat="server" CssClass="textEntry" Width="185px"></asp:TextBox>
                                  &nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                  &nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="btnExportar" runat="server" Text="Exportar" />  
                            </fieldset>
                         </div>

                        <asp:GridView ID="GvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="IdOrden" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="IdOrden" HeaderText="Id Orden" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="IdTrabajo" HeaderText="Id Trabajo" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="GerenciaOP" HeaderText="Gerencia OP" />
                                <asp:BoundField DataField="COE" HeaderText="OMP" />
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" Visible="false" />
                                <asp:BoundField DataField="Contratista" HeaderText="Contratista" />
                                <asp:BoundField DataField="EstadoOrden" HeaderText="Estado Orden" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="Consecutivo" />
                                <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo Documento" />
                                <asp:BoundField DataField="ValorOrden" HeaderText="Valor Orden" />
                                <asp:BoundField DataField="UsuarioOrden" HeaderText="Usuario Orden" />
                                <asp:BoundField DataField="UsuarioRadica" HeaderText="Usuario Radica" />
                                <asp:BoundField DataField="FechaRadicado" HeaderText="Fecha Radicado" />
                                <asp:BoundField DataField="ValorRadicado" HeaderText="Valor Radicado" />
                                <asp:BoundField DataField="EstadoCuentaCobro" HeaderText="Estado Cuenta Cobro" />
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(GvOrdenes.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(GvOrdenes.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td><span class="pagingLinks">[<%= GvOrdenes.PageIndex + 1%>-<%= GvOrdenes.PageCount%>]</span> </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((GvOrdenes.PageIndex + 1) = GvOrdenes.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((GvOrdenes.PageIndex + 1) = GvOrdenes.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
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
