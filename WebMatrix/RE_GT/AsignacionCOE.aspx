<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="AsignacionCOE.aspx.vb" Inherits="WebMatrix.AsignacionCOE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
                function (value, element) {
                    return this.optional(element) ||
                        (value != -1);
                }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#GerenteAsignar').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Seleccione el COE",
                    width: "600px",
                    closeOnEscape: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");
                    }
                });

            loadPlugins();
        });
        function MostrarGerentesProyectos() {
            $('#GerenteAsignar').dialog("open");
        }


        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a></a>
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
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div>
                <asp:Panel runat="server" ID="accordion0">
                    <h3 style="float: left; text-align: left;">Trabajos<asp:HiddenField ID="hfidTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidTipoProyecto" runat="server" />

                    </h3>
                    <div>
                        <label>
                            Titulo</label>
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        <label>Unidades</label><asp:DropDownList ID="ddlGrupoUnidades" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id,TipoProyectoId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="FechaTentativaInicioCampo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="FechaTentativaFinalizacion"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Asignar PMO" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAsignarCOE" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/cliente.jpg" Text="Asignar"
                                            ToolTip="Asignar OMP" OnClientClick="MostrarGerentesProyectos()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgVerPresupuestos" runat="server" CausesValidation="False" CommandName="Presupuestos"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/Select_16.png"
                                            Text="Presupuestos" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="GerenteAsignar">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <label>Seleccione el OMP a asignar</label>
                <asp:DropDownList ID="ddlLider" runat="server"></asp:DropDownList>
                <div class="spacer" />
                <label id="lblTipoRecoleccion" runat="server">Tipo de Recolección</label>
                <asp:DropDownList ID="ddlTipoRecoleccion" runat="server">
                </asp:DropDownList>
                <div class="spacer">
                    <asp:Button ID="btnUpdate" runat="server" Text="Asignar OMP" OnClientClick="$('#GerenteAsignar').dialog('close');" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
