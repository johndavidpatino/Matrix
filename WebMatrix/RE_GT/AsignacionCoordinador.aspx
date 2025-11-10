<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="AsignacionCoordinador.aspx.vb" Inherits="WebMatrix.AsignacionCoordinador" %>

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
                    title: "Seleccione el coordinador",
                    width: "600px",
                    closeOnEscape: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");

                    }
                });

            loadPlugins();
        });
        function MostrarCoordinadores() {
            $('#GerenteAsignar').dialog("open");
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:Panel runat="server" ID="accordion">
                <asp:Panel runat="server" ID="accordion0">
                    <h3 style="float: left; margin: 0;">
                        Lista de Trabajos<asp:HiddenField ID="hfidTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidTipoTecnica" runat="server" />
                    </h3>
                    <div>
                        <div>
                            <label>Búsqueda</label>
                            <asp:TextBox ID="txtID" runat="server" placeholder="Por ID"></asp:TextBox>
                            <asp:TextBox ID="txtNombre" runat="server" placeholder="Por Nombre"></asp:TextBox>
                            <asp:TextBox ID="txtJobBook" runat="server" placeholder="Por JobBook"></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                        <div class="spacer"></div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="50"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="FechaTentativaInicioCampo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="FechaTentativaFinalizacion"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Asignar Coordinador" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAsignarCOE" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/Select_16.png" Text="Asignar"
                                            ToolTip="Asignar Gerente de Proyectos" />
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
                <asp:Panel runat="server" ID="accordion1" Visible="false">
                    <h3 style="float: left; margin: 0;">
                                Ciudades<asp:HiddenField ID="hfidMuestra" runat="server" />
                    </h3>
                    <div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Coordinador" HeaderText="Coordinador" />
                                <asp:TemplateField HeaderText="Asignar Coordinador" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/cliente.jpg" Text="Seleccionar" OnClientClick="MostrarCoordinadores()"
                                            ToolTip="Ver planeación" />
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
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <asp:Button ID="btnVolver" runat="server" Text="Ver trabajos por asignar" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="GerenteAsignar">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <label>Seleccione el coordinador de campo a asignar</label>
                    <asp:DropDownList ID="ddlLider" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnUpdate" runat="server" Text="Asignar Coordinador" OnClientClick="$('#GerenteAsignar').dialog('close');" />
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
