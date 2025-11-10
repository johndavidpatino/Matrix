<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master"
    CodeBehind="REAsignacionProyectos.aspx.vb" Inherits="WebMatrix.ReAsignacionProyectos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>
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

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#GerenteProyectoAsignar').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Seleccione el Gerente de Proyecto",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                        $(this).parent().appendTo("form");

                         }
            });

            loadPlugins();
        });
        function MostrarGerentesProyectos() {
            $('#GerenteProyectoAsignar').dialog("open");
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
    <a>Re-asignación gerentes de proyectos</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Utilice esta opción para cambiar un gerente de proyectos que ya había sido asignado
    <br />
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
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
                <asp:Panel runat="server" id="accordion0">
                    <h3 style="float:left; text-align:left;">
                        <a>
                            <label>
                                Proyectos
                            </label>
                        </a>
                    </h3>
                    <div style="clear:both;">
                                <label>Unidades
                                   </label>
                                <asp:DropDownList ID="ddlUnidades" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                <label>
                                    Nombre</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        <asp:GridView ID="gvProyectos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id, EstudioId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="GP_Nombres" HeaderText="GerenteProyectos" />
                                <asp:BoundField DataField="EstudioId" HeaderText="Estudio" />
                                <asp:BoundField DataField="TipoProyecto" HeaderText="Tipos proyectos" />
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="PresupuestosAsignados" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Presupuestos asignados" OnClientClick="MostrarPresupuestosAsignadosXEstudio()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asignar GP" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAsignarGP" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/cliente.jpg" Text="Asignar"
                                            ToolTip="Asignar Gerente de Proyectos" OnClientClick="MostrarGerentesProyectos()" />
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
                                                    Enabled='<%# IIF(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvProyectos.PageIndex + 1%>-<%= gvProyectos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvProyectos.PageIndex +1) = gvProyectos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvProyectos.PageIndex +1) = gvProyectos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" id="accordion1" Visible="false">
                    <h3>
                        <a href="#">
                            <label>
                                Información del proyecto
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            
                            <div>
                                <asp:HiddenField ID="hfIdProyecto" runat="server" />
                                
                                <div style="clear: both">
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="GerenteProyectoAsignar">
        <asp:UpdatePanel ID="upGerenteProyectoAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <label>Seleccione el gerente de proyectos a asignar</label>
                    <asp:DropDownList ID="ddlLider" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnUpdate" runat="server" Text="Asignar Gerente"  OnClientClick="$('#GerenteProyectoAsignar').dialog('close');" />
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
