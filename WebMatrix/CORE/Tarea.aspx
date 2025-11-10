<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="Tarea.aspx.vb" Inherits="WebMatrix.Tarea" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#DevolucionTarea').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Devolucion Tarea",
                width: "600px"
            });

            $('#Observaciones').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Observaciones",
                width: "600px"
            });

            $('#DevolucionTarea').parent().appendTo("form");

        });
        function MostrarObservacionesDevolucion(WorkFlowIdTareaDevolver) {
            $('#DevolucionTarea').dialog("open");
            $('#CPH_Section_CPH_Section_CPH_ContentForm_hfWorkFlowIdTareaDevolver').val(WorkFlowIdTareaDevolver);
        }
        function mostrarObservaciones() {
            $('#Observaciones').dialog("open");
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
    <label>
        Tareas del trabajo:
    </label>
    Trabajo:<asp:Label ID="lblNombreTrabajo" runat="Server"></asp:Label>
    <br />
    Tarea:
    <asp:Label ID="lblTarea1" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lbtnVolverTareasTrabajo" runat="server" Text="Volver a tareas del trabajo" />
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Tareas anteriores
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:HiddenField ID="hfWorkFlowIdTareaDevolver" runat="server" />
                <asp:GridView ID="gvTareasAnteriores" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                        <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                        <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:TemplateField HeaderText="Devolver" ShowHeader="False">
                            <ItemTemplate>
                                <img id="imgIrDevolver" src="/Images/Select_16.png" title="Actualizar" alt="Ver"
                                    onclick='<%# IIF(Eval("EstadoWorkFlow_Id")=5,"MostrarObservacionesDevolucion(" & Eval("Id") & ")","") %>' />
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
                                            Enabled='<%# IIF(gvTareasAnteriores.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvTareasAnteriores.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvTareasAnteriores.PageIndex + 1%>-<%= gvTareasAnteriores.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasAnteriores.PageIndex +1) = gvTareasAnteriores.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasAnteriores.PageIndex +1) = gvTareasAnteriores.PageCount, "false", "true") %>'
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
                        Tarea
                    </label>
                </a>
            </h3>
            <asp:UpdatePanel ID="upTarea" runat="server">
                <ContentTemplate>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Tarea
                                </label>
                                <asp:Label ID="lblTarea" runat="server"></asp:Label>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Fecha inicio propuesta
                                </label>
                                <asp:Label ID="lblFIniP" runat="server"></asp:Label>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Fecha fin propuesta
                                </label>
                                <asp:Label ID="lblFFinP" runat="server"></asp:Label>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Fecha inicio real
                                </label>
                                <asp:Label ID="lblFIniR" runat="server"></asp:Label>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Fecha fin real
                                </label>
                                <asp:Label ID="lblFFinR" runat="server"></asp:Label>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Observaciones
                                </label>
                                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="required text textEntry"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="actions">
                            <div class="form_right">
                                <fieldset>
                                    <asp:Button ID="btnIniciarTarea" runat="server" Text="Iniciar Tarea" />
                                    <asp:Button ID="btnFinalizarTarea" runat="server" Text="Finalizar Tarea" CssClass="causesValidation buttonText buttonSave corner-all" />
                                    <asp:Button ID="btnArchivos" runat="server" Text="Archivos" />
                                    <asp:Button ID="btnVerTodasObservaciones" runat="server" Text="Observaciones" OnClientClick="mostrarObservaciones()" />
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div id="DevolucionTarea">
        <div>
            <fieldset>
                <label>
                    Observaciones
                </label>
                <asp:TextBox ID="txtObservacionDevolucion" runat="server" TextMode="MultiLine"></asp:TextBox>
            </fieldset>
        </div>
        <%-- <div>
            <fieldset>
                <label>
                    Archivo adicional:
                </label>
                <asp:FileUpload ID="fuCargarArchivo" runat="server" />
            </fieldset>
        </div>--%>
        <div class="actions">
            <div class="form_right">
                <fieldset>
                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" />
                </fieldset>
            </div>
        </div>
    </div>
    <div id="Observaciones">
        <asp:UpdatePanel ID="upObservaciones" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvObservaciones" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                    </Columns>
                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                            Enabled='<%# IIF(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvObservaciones.PageIndex + 1%>-<%= gvObservaciones.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvObservaciones.PageIndex +1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvObservaciones.PageIndex +1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <script type="text/javascript">
            var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
            pageReqManger.add_initializeRequest(InitializeRequest);
            pageReqManger.add_endRequest(EndRequest);
    </script>    

</asp:Content>
