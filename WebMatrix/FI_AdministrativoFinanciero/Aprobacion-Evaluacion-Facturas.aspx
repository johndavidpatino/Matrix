<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_.master" CodeBehind="Aprobacion-Evaluacion-Facturas.aspx.vb" Inherits="WebMatrix.Aprobacion_Evaluacion_Facturas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        function loadPlugins() {
            $("#VisualizarDocumento").resizable();
        }

        $(document).ready(function () {
            loadPlugins();
        });

        function cargarFactura(id) {
            $('#ifDocumento').attr('src', '/Facturas/' + id + '.pdf');
            $('#CPH_Section_CPH_Section_pnlAprobaciones').css('visibility', 'visible');
            $('#VisualizarDocumento').css('visibility', 'visible');
        }

        function validarMotivo() {
            var comentario = $('#CPH_Section_CPH_Section_txtComentarios').val();
            if (comentario == "") {
                alert("Debe indicar una razón por la cual no aprueba la factura");
                return false;
            } else {
                return true;
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <div style="width: 100%">
                        <h1><a>Gestión de </a>Facturas</h1>


                        <asp:Panel ID="pnlCronograma" runat="server" Visible="true">
                            <asp:HiddenField ID="hfEstado" runat="server" Value="1" />
                            <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                            <asp:HiddenField ID="hfFactura" runat="server" Value="-1" />
                            <div class="clear"></div>
                            <a>
                            <asp:Label ID="lblTitle" runat="server">Aprobación de facturas de la compra o servicio</asp:Label></a>
                            <br /><br />
                            <label id="lblNumRadicado" runat="server">Número Radicado</label>
                            <asp:TextBox ID="txtNumRadicado" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                            <br /><br />
                            <div class="clear"></div>
                            <div class="block">
                                <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false" PageSize="5"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="true" EmptyDataText="No se encuentran facturas para los filtros seleccionados">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnEvaluar" runat="server" Text="Aprobar" Width="60px" CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" HeaderText="Id" />
                                        <asp:BoundField DataField="FechaRadicacion" HeaderText="FechaRadicacion" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="NoFactura" HeaderText="NoFactura" />
                                        <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                                        <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                                        <asp:BoundField DataField="NIT_CC" HeaderText="NIT/CC" />
                                        <asp:BoundField DataField="ValorFactura" HeaderText="Valor"
                                            DataFormatString="{0:C0}" />
                                        <%--<asp:TemplateField HeaderText="Escáner" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Load" ImageUrl="~/Images/list_16.png" Text="Escáner" ToolTip="Imagen de la factura" OnClientClick="MostrarLoadFiles()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                                    </Columns>
                                    <PagerTemplate>
                                        <div class="pagingButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td><span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span> </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>

                        </asp:Panel>
                    </div>
                </div>

                <asp:Panel ID="pnlAprobaciones" runat="server" Style="visibility: hidden">
                    <br />
                    <div id="aprobaciones">
                        <asp:Label ID="lblEstadoAutorizaciones" runat="server" Text="Estado de aprobaciones"></asp:Label>
                        <asp:GridView ID="gvAprobaciones" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha" />
                                <asp:BoundField DataField="Aprobado" HeaderText="Aprobado" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lblAprobacionActual" runat="server" Text="Aprobación actual"></asp:Label>
                    <div>
                        <asp:TextBox ID="txtComentarios" Width="150px" runat="server" placeholder="Agregue sus comentarios aquí" TextMode="MultiLine"></asp:TextBox>
                        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" OnClientClick="$('#CPH_Section_CPH_Section_pnlAprobaciones').css('visibility', 'hidden');" />
                        <asp:Button ID="btnNoAprobar" runat="server" Text="No Aprobar" OnClientClick="if (!validarMotivo()) { return false; }" />
                    </div>
                </asp:Panel>
                <div id="VisualizarDocumento" style="width: 100%; height: 500px; visibility: hidden">
                    <iframe id="ifDocumento" style="width: 100%; height: 100%"></iframe>
                </div>

            </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="LoadFiles">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
