<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master"
    CodeBehind="PresupuestosInternos.aspx.vb" Inherits="WebMatrix.PresupuestosInternos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">

    <script type="text/javascript">
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#Contratista').dialog({
                modal: true,
                autoOpen: false,
                title: "Ingrese Valor de Contratista",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

            $('#AjustesSupervision').dialog({
                modal: true,
                autoOpen: false,
                title: "Ingrese Valores de Supervision",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        $(document).ready(function () {
            loadPlugins();
        });
        function Muestras() {
            $('#Contratista').dialog("open");
        }
        function AjustesSup() {
            $('#AjustesSupervision').dialog("open");
        }
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
                <div id="accordion0">
                    <h3><a href="#">
                        <label>Presupuestos Internos</label></a></h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidpresupuesto" runat="server" />
                        <asp:HiddenField ID="hfIdMet" runat="server" />
                        <asp:HiddenField ID="hfidmuestra" runat="server" />
                        <asp:HiddenField ID="hftipopresupuesto" runat="server" />
                        <div class="form_left">
                            <table>
                                <tr>
                                    <td width="20%">
                                        <label>Buscar</label>
                                        <asp:TextBox ID="txtBuscar" runat="server"></asp:TextBox>
                                        <asp:Button ID="btbbuscar" runat="server" Text="Buscar" />
                                    </td>
                                    <td width="20%">
                                        <label>Tipo</label>
                                        <asp:DropDownList ID="ddlTipoPresupuesto" AutoPostBack="true" Width="140px" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="30%">
                                        <label>Valor dia año</label>
                                        <asp:DropDownList ID="ddlAño" AutoPostBack="true" Width="250px" runat="server"></asp:DropDownList>
                                    </td>
                                    <td width="20%">
                                        <asp:Button ID="btnCrear" Width="130px" runat="server" Text="Crear Presupuesto" />
                                        <a href="Trabajos.aspx" class="btn">Volver</a>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <asp:GridView ID="gvMuestra" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="PresupuestoId" EmptyDataText="No existen registros para mostrar">
                            <Columns>
                                <asp:BoundField DataField="PresupuestoId" HeaderText="PresupuestoId" />
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" Text='<%# Bind("Descripcion") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="DiasDuracion" HeaderText="Dias" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Productividad" HeaderText="Productividad" />
                                <asp:BoundField DataField="ValorTotal" HeaderText="ValorTotal" DataFormatString="{0:c0}" />
                                <asp:TemplateField HeaderText="VerDetalle" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPresupuesto" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="VerDetalle" ImageUrl="~/Images/Select_16.png" ToolTip="Ver Detalle Presupuesto" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/save_16.png" ToolTip="Actualizar Presupuesto" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEliminar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" ToolTip="Eliminar Presupuesto" OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                    </div>
                </div>
                <div id="accordion1">
                    <h3><a href="#">
                        <label>Detalle Presupuesto Interno</label></a></h3>
                    <div class="block">
                        <div class="block">
                            <div class="form_left">
                                <fieldset>
                                    <label>Descripcion</label>
                                    <asp:Label ID="lblObservacion" runat="server"></asp:Label>
                                    <label>Metodologia</label>
                                    <asp:Label ID="lblMetodologia" runat="server"></asp:Label>
                                    <fieldset>
                                        &nbsp;
                                        <label>Productividad</label>
                                        <asp:TextBox ID="TxtProductividad" runat="server"></asp:TextBox><br />
                                        <label>Muestra</label>
                                        <asp:TextBox ID="TxtMuestra" runat="server"></asp:TextBox>
                                    </fieldset>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <asp:Label ID="lblTotalPresupuesto" runat="server" ForeColor="Black" Font-Size="Medium" />
                            </div>
                            <div class="actions">
                                <div class="form_left">
                                    <asp:Button ID="btnCalcular" runat="server" Text="Calcular" />
                                    &nbsp;
                                    <asp:Button ID="btnVolver" runat="server" Text="Volver" />
                                </div>
                                <div class="form_left">
                                    <asp:Button ID="btnduplicar" runat="server" Text="Duplicar" OnClientClick="Muestras()" Visible="false" />
                                    <asp:Button ID="btnAjustar" runat="server" Text="Ajustar Valor Encuestador Contratista" OnClientClick="Muestras()" />
                                    <asp:Button ID="btnAjustarSupervision" runat="server" Text="Ajustar Valores Supervision" OnClientClick="AjustesSup()" />
                                </div>
                                <asp:GridView ID="gvCostos" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="DiasHombre" HeaderText="DiasHombre" DataFormatString="{0:D}" />
                                        <asp:BoundField DataField="ValorUnitario2" HeaderText="Valor sin provisión" DataFormatString="{0:c0}" />
                                        <asp:BoundField DataField="Ajustado" HeaderText="Valor Encuesta" DataFormatString="{0:c0}" />
                                        <asp:BoundField DataField="Presupuesto" HeaderText="Total Presupuesto" DataFormatString="{0:c0}" />
                                        <asp:BoundField DataField="TotalIngreso2" HeaderText="Total Costo" DataFormatString="{0:c0}" />
                                        <asp:BoundField DataField="Ahorro" HeaderText="Ahorro $" DataFormatString="{0:c0}" />
                                        <asp:BoundField DataField="AhorroPercent" HeaderText="Ahorro %" DataFormatString="{0:P2}" />
                                        <asp:BoundField DataField="CostoTelefonico" HeaderText="Costo Telefonico" DataFormatString="{0:c0}" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <br />
                                <asp:GridView ID="gvReclutamiento" runat="server" Width="100%" AutoGenerateColumns="False" OnRowCommand="gvReclutamiento_RowCommand"
                                    CssClass="displayTable"
                                    DataKeyNames="id" EmptyDataText="No existen registros para mostrar" ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                                        <asp:BoundField DataField="NSE" HeaderText="NSE" />
                                        <asp:TemplateField HeaderText="Muestra">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMuestra" runat="server" Text='<%# Bind("Muestra", "{0:N0}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TarifaPST" HeaderText="Tarifa Encuestador" DataFormatString="{0:c0}" />
                                        <asp:TemplateField HeaderText="Tarifa Contratista">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTarifaContratista" runat="server" Text='<%# Bind("TarifaContratista", "{0:c0}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Actualizar" ImageUrl="~/Images/save_16.png" ToolTip="Actualizar Tarifa" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form_left">
                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Size="Medium" Text="Observaciones" />
                                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:Button ID="btnSaveObservaciones" runat="server" Text="Guardar Observacion" OnClick="btnSaveObservaciones_Click" />
                            </div>
                            

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="Contratista">
        <asp:UpdatePanel ID="upPresupuestos" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                    </div>
                </div>
                <asp:TextBox ID="txtValor" runat="server"></asp:TextBox>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="MuestrasCerrar();" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="AjustesSupervision">
        <asp:UpdatePanel ID="upSupervision" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                    </div>
                </div>
                <asp:TextBox ID="txtValorSupervision" runat="server"></asp:TextBox>
                <asp:RadioButtonList ID="rbTipoSupervision" runat="server">
                    <asp:ListItem Value="40" Text="Supervisor PST"></asp:ListItem>
                    <asp:ListItem Value="50" Text="Supervisor Contratista"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:Button ID="btnActualizarVrSupervision" runat="server" Text="Guardar" OnClientClick="SupervisionCerrar();" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
