<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master"
    CodeBehind="TrabajosConAtraso.aspx.vb" Inherits="WebMatrix.TrabajosConAtraso" %>

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

            $('#div-dialog').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Informe de Avance",
                width: "1200px",
                position: 'center',
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });


            loadPlugins();
        });

        function Mostrardivdialog() {
            $('#div-dialog').dialog("open");
        }
        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Trabajos que presentan algún nivel de atraso</a>
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
            <asp:Panel runat="server" id="accordion">
                <div id="accordion0">
                    <h3 style="float: left; text-align: left;">
                                Trabajos<asp:HiddenField ID="hfidTrabajo" runat="server" />
                    </h3>
                    <div>
                                <label>Unidades
                                    </label>
                                <asp:DropDownList ID="ddlGrupoUnidades" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                <label>
                                    Ingrese Criterio de Busqueda</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="TrabajoId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="TrabajoId" HeaderText="Id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="FechaInicioCampo" HeaderText="Fecha Inicio"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFinalCampo" HeaderText="Fecha Final"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="MUESTRA" HeaderText="Muestra" />
                                <asp:BoundField DataField="AVANCEGENERAL" HeaderText="Avance" DataFormatString="{0:P2}" />
                                <asp:BoundField DataField="PORCENTAJEDESFASE" HeaderText="Desfase" DataFormatString="{0:P2}" />
                                <asp:BoundField DataField="DIASFALTANTES" HeaderText="Dias faltantes" />
                                <asp:BoundField DataField="ENCUESTASFALTANTES" HeaderText="Encuestas faltantes" />
                                <asp:BoundField DataField="COE" HeaderText="OMP" />
                                <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Ver avance" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAsignarCOE" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Avance" ImageUrl="~/Images/select_16.png" Text="Ver Avance" OnClientClick="Mostrardivdialog()"
                                            ToolTip="Ver avance" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Seguimiento" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrProject" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Project" ImageUrl="~/Images/calendar_24.png" Text="Actualizar" ToolTip="Ir a Gantt de Planeación y Ejecución" />
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
                    </asp:Panel>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="div-dialog">
    <asp:UpdatePanel ID="upDialog" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
    <asp:Panel ID="pnlAvance" runat="server" Visible="false">
    <iframe src="AvanceDeCampoDialog.aspx" width="1200" height="800">
        </iframe>
        </asp:Panel>
<%--    <div id="div-dialog">
        <iframe src="AvanceDeCampoDialog.aspx" width="1024" height="800">
        </iframe>
    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
