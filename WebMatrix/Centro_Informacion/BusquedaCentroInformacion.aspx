<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/IT_F.master"
    CodeBehind="BusquedaCentroInformacion.aspx.vb" Inherits="WebMatrix.BusquedaCentroInformacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-tabs-vertical {
            width: 55em;
        }

            .ui-tabs-vertical .ui-tabs-nav {
                padding: .2em .1em .2em .2em;
                float: left;
                width: 12em;
            }

                .ui-tabs-vertical .ui-tabs-nav li {
                    clear: left;
                    width: 100%;
                    border-bottom-width: 1px !important;
                    border-right-width: 0 !important;
                    margin: 0 -1px .2em 0;
                }

                    .ui-tabs-vertical .ui-tabs-nav li a {
                        display: block;
                    }

                    .ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active {
                        padding-bottom: 0;
                        padding-right: .1em;
                        border-right-width: 1px;
                    }

            .ui-tabs-vertical .ui-tabs-panel {
                padding: 1em;
                float: right;
                width: 40em;
            }
    </style>
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function visualizarDocumento(url) {
            $('#ifDocumento').attr('src', '\\co-file04\UnidadesEstudiosporCerrar\Loyalty\14-001214 Barcelona Colombia\Instrumentos\CE-14001214-Barcelona_V1.doc');
            $('#VisualizarDocumento').css('visibility', 'visible');
            $('#VisualizarDocumento').dialog("open");
        }

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

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#infoBrief').dialog(
           {
               modal: true,
               autoOpen: false,
               title: "Brief",
               width: "800px"
           });

            $('#tabs').tabs().addClass("ui-tabs-vertical ui-helper-clearfix");


            $('#VisualizarDocumento').dialog(
           {
               modal: true,
               autoOpen: false,
               title: "Documento",
               width: "600px"
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
    <a>BUSQUEDA&nbsp; - CENTRO DE INFORMACIÓN</a>
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
    <asp:LinkButton ID="lbtnVolver" Text="Volver" href="../IT/Default.aspx" runat="server"></asp:LinkButton>
    <br />
    <div class="form_right">
        <fieldset>
            <label>Busqueda</label>
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
        </fieldset>
    </div>
    <br />
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:HiddenField ID="hfIdMaestro" runat="server" />
            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
            <div id="accordion">
                <div id="accordion0">
                    <h3><a href="#">
                        <label>
                            RESULTADOS BUSQUEDA
                        </label>
                    </a>
                    </h3>
                    <fieldset>
                        <div class="form_right">
                            <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="IdTrabajo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="IdTrabajo" HeaderText="IdTrabajo" />
                                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                    <asp:BoundField DataField="GerenteProyecto" HeaderText="GerenteProyecto" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                    <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                                <PagerTemplate>
                                    <div class="pagingButtons">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                </td>
                                                <td><span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span> </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>' SkinID="paging">Ultimo »</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upInfoTrabajo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <asp:Label ID="lblMensaje" runat="server" Text="La información visualizada a continuación corresponde al trabajo:" Visible="false"></asp:Label>
                <asp:Label ID="lblIdTrabajo" runat="server"></asp:Label>
                <asp:Label ID="lblNombreTrabajo" runat="server"></asp:Label>
            </div>
            <div id="tabs">
                <ul>
                    <li><a href="#tabs-1">Brief</a></li>
                    <li><a href="#tabs-2">Documentos</a></li>
                    <li><a href="#tabs-3">Medios fisicos</a></li>
                </ul>
                <div id="tabs-1" style="overflow-y: auto">
                    <asp:Panel ID="pnlBrief" runat="server" Visible="false">
                        <div class="block">
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        0. Titulo:</label>
                                    <asp:TextBox ID="txtTitulo" Width="100%"
                                        runat="server" />
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        1. Antecedentes y Problema de Marketing:</label>
                                    <asp:Label ID="txtAntecedentes"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        2. Objetivos de la Investigación</label>
                                    <asp:Label ID="txtObjetivos" runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        3. Action Standards
                                    </label>
                                    <asp:Label ID="txtActionStandard"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        4. Metodología</label>
                                    <asp:Label ID="txtMetodologia"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        5. Target group de la Investigación de Mercado
                                    </label>
                                    <asp:Label ID="txtTargetGroup"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        6. Tiempos
                                    </label>
                                    <asp:Label ID="txtTiempos" runat="server" />
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        7. Presupuestos
                                    </label>
                                    <asp:Label ID="txtPresupuesto"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        8. Materiales disponibles</label>
                                    <asp:Label ID="txtMateriales"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        9. Resultados de estudios anteriores</label>
                                    <asp:Label ID="txtEstudiosAnteriores"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        10. Formato requerido por el cliente para el informe
                                    </label>
                                    <asp:Label ID="txtFormatos" runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        11. Aprobaciones
                                    </label>
                                    <asp:Label ID="txtAprobaciones"
                                        runat="server" />
                                </fieldset>
                                <fieldset>
                                    <label>
                                        12. Competencia
                                    </label>
                                    <asp:Label ID="txtCompetencia"
                                        runat="server" />
                                </fieldset>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div id="tabs-2">
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="actions">
                                    <div class="form_rigth">
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
                                        <br />
                                        <br />
                                        <asp:Panel ID="PnlListadoDocsDescargar" runat="server" Visible="false">
                                            <asp:GridView ID="gvDocumentosCargados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                DataKeyNames="IdDocumentoRepositorio" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                <SelectedRowStyle CssClass="SelectedRow" />
                                                <AlternatingRowStyle CssClass="odd" />
                                                <Columns>
                                                    <asp:BoundField DataField="DocumentoId" HeaderText="DocumentoId" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="Version" HeaderText="Version" Visible="false" />
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha cargue" />
                                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                                                    <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver"
                                                                ToolTip="Ver" OnClientClick="visualizarDocumento();return false;" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="tabs-3">
                    <asp:GridView ID="gvMedios" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="IdMedio" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="IdMedio" HeaderText="IdMedio" />
                            <asp:BoundField DataField="TipoAlmacenamiento" HeaderText="TipoAlmacenamiento" />
                            <asp:BoundField DataField="Contiene" HeaderText="Contiene" />
                            <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                            <asp:TemplateField HeaderText="Solicitar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSolicitar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Solicitar" ImageUrl="~/Images/Select_16.png" Text="Solicitar" ToolTip="Solicitar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>

                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td><span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span> </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>' SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="VisualizarDocumento" style="width: 100%; height: 500px; visibility: hidden">
        <iframe id="ifDocumento" style="width: 100%; height: 100%"></iframe>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
