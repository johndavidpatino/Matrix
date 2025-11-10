<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="PresupuestoInterno.aspx.vb" Inherits="WebMatrix.PresupuestoInterno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Presupuestos Internos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Content" runat="server">
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
            <asp:HiddenField ID="hfidpresupuesto" runat="server" Visible="false" />
            <asp:HiddenField ID="hftipopresupuesto" runat="server" Visible="false" />
            <asp:HiddenField ID="hfidTrabajo" runat="server" Visible="false" />
            <asp:HiddenField ID="hfIdMet" runat="server" Visible="false" />
            <div class="main-card mb-3 card">
                <div class="card-body">
                    <h5 class="card-title">Búsqueda</h5>
                    <p class="card-subtitle">Diligencie los campos por los cuales desea buscar</p>
                    <div>
                        <div class="form-row">
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Criterio de búsqueda</button>
                                </div>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Tipo Presupuesto</button>
                                </div>
                                <asp:DropDownList ID="ddlTipoPresupuesto" AutoPostBack="true" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-6 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Valor día año</button>
                                </div>
                                <asp:DropDownList ID="ddlAño" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <asp:Button runat="server" ID="btbbuscar" class="btn btn-primary" Text="Buscar"></asp:Button>
                        <asp:Button runat="server" ID="btnCrear" class="btn btn-info" Text="Crear Presupuesto"></asp:Button>
                    </div>

                    <div>
                        <asp:GridView ID="gvMuestra" runat="server" AutoGenerateColumns="False"
                            CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0"
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
                    </div>
                </div>
            </div>
            <br />
            <asp:Panel ID="pnlDetail" runat="server" Visible="false">
            <div class="main-card mb-3 card">
                <div class="card-body">
                    <h5 class="card-title">Detalle Presupuesto Interno</h5>
                    <p class="card-subtitle">
                        <asp:Label ID="lblObservacion" runat="server"></asp:Label>
                    </p>
                    <p class="text-focus">
                        Metodología:
                <asp:Label ID="lblMetodologia" runat="server"></asp:Label>
                    </p>
                    <div>
                        <div class="form-row">
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Productividad</button>
                                </div>
                                <asp:TextBox ID="TxtProductividad" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Muestra</button>
                                </div>
                                <asp:TextBox ID="txtMuestra" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <asp:Button runat="server" ID="btnCalcular" class="btn btn-primary" Text="Volver a calcular"></asp:Button>
                            </div>
                            <div class="input-group col-md-3 mb-2">
                                <label></label>
                                <asp:Button runat="server" ID="btnAjustar" class="btn btn-info" Text="Ajustar Valor Encuestador Contratista" OnClientClick="Muestras()" ></asp:Button>
                        
                            </div>
                            <div class="input-group col-md-3 mb-2">
                                <label></label>
                                <asp:Button runat="server" ID="btnAjustarSupervision" class="btn btn-info" Text="Ajustar Valores Supervision" OnClientClick="AjustesSup()" ></asp:Button>
                            </div>
                        </div>
                        
                        
                    </div>

                    <div>
                        <asp:GridView ID="gvCostos" runat="server" AutoGenerateColumns="False"
                            CssClass="mb-0 table table-striped" BorderStyle="NotSet" BorderWidth="0"
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
                    </div>
                    <div>
                        <asp:GridView ID="gvReclutamiento" Width="50%" runat="server" AutoGenerateColumns="False" OnRowCommand="gvReclutamiento_RowCommand"
                            CssClass="mb-0 table table-dark" BorderStyle="NotSet" BorderWidth="0"
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
                    <br />
                    <div class="form-row">
                        <div class="input-group col-md-8 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Observaciones</button>
                            </div>
                            <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="input-group cold-md-4 mb-3">
                            <asp:Button ID="btnSaveObservaciones" runat="server" CssClass="btn btn-alternate" Text="Guardar Observacion" OnClick="btnSaveObservaciones_Click" />
                        </div>
                    </div>
                </div>
            </div>
                </asp:Panel>
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

</asp:Content>
