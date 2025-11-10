<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="RecuperacionCierre.aspx.vb" Inherits="WebMatrix.RecuperacionCierre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
          function (value, element) {
              return this.optional(element) ||
                ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
          }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

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
    <asp:LinkButton ID="lbtnVolver" Text ="Volver" href="../IT/Default.aspx" runat="server"></asp:LinkButton>
    <br />
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Trabajos Cerrados
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Nombre Trabajo</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="IdTrabajo" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="GerenteProyectos" HeaderText="GerenteProyectos" />
                                <asp:BoundField DataField="CoeAsignado" HeaderText="OMP" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Gestionar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Gestionar" />
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
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Documentos Recuperables
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="actions">
                                        <div class="form_rigth">
                                          <asp:Panel ID="pnlListadoDocumentos" runat="server" Visible="false">
                                                <asp:HiddenField ID="hfIdDocDescarga" runat="server" Value="0" />
                                                <asp:Panel ID="pnlListadoDocsTotal" runat="server" Visible="false">
                                                    <asp:GridView ID="gvTareasXDocumentos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                        DataKeyNames="IdDocumento" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                        <SelectedRowStyle CssClass="SelectedRow" />
                                                        <AlternatingRowStyle CssClass="odd" />
                                                        <Columns>
                                                            <asp:BoundField DataField="IdDocumento" HeaderText="IdDocumento" />
                                                            <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                            <asp:TemplateField HeaderText="Archivos" ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                        CommandName="Archivos" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Archivos" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                                <asp:Panel ID="PnlListadoDocsDescargar" runat="server" Visible="false">
                                                    <asp:GridView ID="gvDocumentosCargados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                        DataKeyNames="IdDocumentoRepositorio" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                        <SelectedRowStyle CssClass="SelectedRow" />
                                                        <AlternatingRowStyle CssClass="odd" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                            <asp:BoundField DataField="Version" HeaderText="Version" Visible="false" />
                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha cargue" />
                                                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                                                            <asp:TemplateField HeaderText="Descargar" ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                        CommandName="Descargar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                                        ToolTip="Descargar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:Button ID="btnVolverDescarga" runat="server" Text="Volver" />
                                                </asp:Panel>
                                            </asp:Panel>
                                        </div>
                                    </div>
                            </div>
                        </fieldset>
                    </div>
                    </fieldset>
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
