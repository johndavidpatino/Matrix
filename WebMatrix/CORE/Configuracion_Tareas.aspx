<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="Configuracion_Tareas.aspx.vb" Inherits="WebMatrix.Configuracion_Tareas" %>

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

            $('#AsignarNuevoDocumento').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Grabar",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $('#AsignarNuevoDocumento').parent().appendTo("form");

            validationForm();
        }

        $(document).ready(function () {
            loadPlugins();
        });

        function AsignarNuevoDocumento(idDocumento) {
            $('#AsignarNuevoDocumento').dialog("open");
            $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIdDocumentoNoAsignado').val(idDocumento);
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
    Nombre tarea:<asp:Label ID="lblNombreTarea" runat="server"></asp:Label>
    <br />
    Tipo de Hilo:<asp:Label ID="lblTipoHilo" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkVolver" runat="server" Text="Volver a tarea"></asp:LinkButton>
    <asp:HiddenField ID="hfIdTarea" runat="server" />
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Tareas
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:GridView ID="gvTareas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                        <asp:BoundField DataField="NoEmpiezaAntesDe" HeaderText="NoEmpiezaAntesDe" />
                        <asp:BoundField DataField="NoTerminaAntesDe" HeaderText="NoTerminaAntesDe" />
                        <asp:BoundField DataField="TiempoPromedioDias" HeaderText="TiempoPromedioDias" />
                        <asp:BoundField DataField="RequiereEstimacion" HeaderText="RequiereEstimacion" />
                        <asp:BoundField DataField="TextoRolEstima" HeaderText="RolEstima" />
                        <asp:BoundField DataField="TextoUnidadEjecuta" HeaderText="UnidadEjecuta" />
                        <asp:BoundField DataField="UnidadRecibe" HeaderText="UnidadRecibe" />
                        <asp:BoundField DataField="TextoRolEjecuta" HeaderText="RolEjecuta" />
                        <asp:BoundField DataField="Visible" HeaderText="Visible" />
                        <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEditar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Editar" ImageUrl="~/Images/Select_16.png" Text="Editar" ToolTip="Editar" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Documentos Requeridos" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDocumentosRequeridos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="DocumentosRequeridos" ImageUrl="~/Images/Select_16.png" Text="DocumentosRequeridos" ToolTip="DocumentosRequeridos" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Documentos Entregables" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDocumentosEntregables" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="DocumentosEntregables" ImageUrl="~/Images/Select_16.png" Text="DocumentosEntregables" ToolTip="DocumentosEntregables" />
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
                                            Enabled='<%# IIF(gvTareas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvTareas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvTareas.PageIndex + 1%>-<%= gvTareas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvTareas.PageIndex +1) = gvTareas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvTareas.PageIndex +1) = gvTareas.PageCount, "false", "true") %>'
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
                        Detalle tarea:
                    </label>
                </a>
            </h3>
            <div class="block">
                <fieldset class="validationGroup">
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Tarea:
                            </label>
                            <asp:TextBox ID="txtTarea" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                No Empieza Antes De:
                            </label>
                            <asp:DropDownList ID="ddlNoEmpiezaAntesDe" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                No Termina Antes De:
                            </label>
                            <asp:DropDownList ID="ddlNoTerminaAntesDe" runat="server" CssClass="mySpecificClass dropdowntext"></asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Tiempo Promedio Días:
                            </label>
                            <asp:TextBox ID="txtTiempoPromedioDias" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Requiere Estimación:
                            </label>
                            <asp:CheckBox ID="chkRequiereEstimacion" runat="server" />
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Rol Estima:
                            </label>
                            <asp:DropDownList ID="ddlRolEstima" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Unidad Ejecuta:
                            </label>
                            <asp:DropDownList ID="ddlUnidadEjecuta" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Unidad Recibe:
                            </label>
                            <asp:DropDownList ID="ddlUnidadRecibe" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Rol Ejecuta:
                            </label>
                            <asp:DropDownList ID="ddlRolEjecuta" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Visible:
                            </label>
                            <asp:CheckBox ID="chkVisible" runat="server" />
                        </fieldset>
                    </div>
                    <div class="actions">
                        <div class="form_right">
                            <fieldset>
                                <asp:Button ID="btnGrabar" runat="server" Text="Grabar" CssClass="mySpecificClass causesValidation buttonText buttonSave corner-all" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                                    CausesValidation="False" />
                            </fieldset>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
         pageReqManger.add_initializeRequest(InitializeRequest);
         pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
