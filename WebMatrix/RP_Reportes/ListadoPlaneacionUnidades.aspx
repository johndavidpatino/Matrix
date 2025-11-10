<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="ListadoPlaneacionUnidades.aspx.vb" Inherits="WebMatrix.REP_ListadoPlaneacionUnidades" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function MostrarRazonNoViabilidad() {
            $('#RazonNoViabilidad').dialog("open");
        }

        function loadPlugins() {

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#RazonNoViabilidad').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Escriba la razón de no viabilidad",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Listado de Planeación de la Unidad</a>
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
                    <h3>
                        <a href="#">
                            <label>
                                Estudios, Proyectos y Trabajos en Planeación Operaciones<asp:HiddenField ID="hfidTrabajo" runat="server" />
                                <asp:HiddenField ID="hfidbriefnoviab" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Unidades</label>
                                <asp:DropDownList ID="ddlGrupoUnidades" runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="btnBuscar" runat="server" Text="Ver" />
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                            </fieldset>
                        </div>
                        <div class="actions">
                            <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                DataKeyNames="Id" CssClass="displayTable" AlternatingRowStyle-CssClass="odd"
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                    <asp:BoundField DataField="RazonSocial" HeaderText="Cliente" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Contacto" />
                                    <asp:BoundField DataField="Antecedentes" HeaderText="Antecedentes" />
                                    <asp:BoundField DataField="TargetGroup" HeaderText="TargetGroup" />
                                    <asp:BoundField DataField="GerenteCuentas" HeaderText="GerenteCuentas" />
                                    <asp:TemplateField HeaderText="Viabilidad" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("Id") %>' Visible="False"></asp:Label>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Viabilidad") %>' Enabled="false"
                                                Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")),false,true) %>' />
                                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="SI" ImageUrl="~/Images/si.jpg" Text="SI" OnClientClick="return confirm('Está seguro de Elegir la Viabilidad?')"
                                                ToolTip="SI" Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")),true,false) %>' />
                                            &nbsp;
                                            <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="NO" ImageUrl="~/Images/no.jpg" Text="NO" OnClientClick="MostrarRazonNoViabilidad()"
                                                Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")),true,false) %>' ToolTip="NO" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <%--items--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="RazonNoViabilidad">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <label>Escriba la razón de no viabilidad</label>
                    <asp:TextBox ID="txtRazonNoViabilidad" Width="300px" MaxLength="250" TextMode="MultiLine" Height="100px" runat="server" CssClass="required text textEntry"></asp:TextBox>
                </div>
                <div class="actions">
                    <div class="form_rigth">
                        <asp:Button ID="btnGuardarRazonViabilidad" runat="server" Text="Guardar" OnClientClick="$('#RazonNoViabilidad').dialog('close');" />
                        <asp:Button ID="btnCancelarRazonViabilidad" runat="server" Text="Cancelar" OnClientClick="$('#RazonNoViabilidad').dialog('close');" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
