<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="AsignacionTareas.aspx.vb" Inherits="WebMatrix.AsignacionTareas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $(":input[Tipo=Fecha]").mask("99/99/9999");
            $(":input[Tipo=Fecha]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("a[title=Actualizar]").removeAttr("class");
            $("a[title=Editar]").removeAttr("class");
            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            loadPlugins();

            $('#UsuariosAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios Asignados",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

        });
        function MostrarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("open");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Actualización de tareas</a>
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
    <asp:UpdatePanel ID="UpanelGeneral" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfidWorkFlow" runat="server" />
            <asp:HiddenField ID="hfidTarea" runat="server" />
            <fieldset class="validationGroup">
                <div class="actions">
                    <div class="form_rigth">
                        <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver" />
                    </div>
                </div>
                <asp:GridView ID="gvTareas" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id, HiloId, TareaId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:TemplateField HeaderText="Tarea">
                            <ItemTemplate>
                                <asp:Label ID="lblTarea" Text='<%#Eval("Tarea")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTarea" Text='<%#Eval("Tarea")%>' runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FechaInicioPlaneada">
                            <ItemTemplate>
                                <asp:Label ID="lblFIniP" Text='<%#Eval("FIniP","{0:dd/M/yyyy}") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFIniP" runat="server" CssClass="required text textEntry" Tipo="Fecha"
                                    Text='<%#Eval("FIniP") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FechaFinPlaneada">
                            <ItemTemplate>
                                <asp:Label ID="lblFFinP" Text='<%#Eval("FFinP","{0:dd/M/yyyy}")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFFinP" runat="server" CssClass="required text textEntry" Tipo="Fecha"
                                    Text='<%#Eval("FFinP")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observacion">
                            <ItemTemplate>
                                <asp:Label ID="lblObservacion" runat="server" Text='<%#Eval("Observacion")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtObservacion" runat="server" Tipo="Texto" Text='<%#Eval("Observacion")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UsuariosAsignados">
                            <ItemTemplate>
                                <asp:Label ID="lblUsuarioAsignado" runat="server" Text='<%#Eval("CantidadUsuariosAsignados")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblUsuarioAsignado" runat="server" Tipo="Texto" Text='<%#Eval("CantidadUsuariosAsignados")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:Label ID="lblEstado" runat="server" Text='<%#Eval("Estado")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblEstado" runat="server" Tipo="Texto" Text='<%#Eval("Estado")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuarios Asignados" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrUsuariosAsignados" runat="server" CausesValidation="False"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Asignar"
                                    ImageUrl="~/Images/cliente.jpg" Text="Asignar" ToolTip="Ver usuarios asignados"
                                    OnClientClick="MostrarUsuariosAsignados()" Enabled='<%# IIF (Eval("EstadoWorkFlow_Id")<>6,"true","false") %>' />
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
                                        <span class="pagingLinks">[<%= gvTareas.PageIndex + 1%>-
                                            <%= gvTareas.PageCount%>]</span>
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
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="UsuariosAsignados">
        <asp:UpdatePanel ID="upUsuariosAsignados" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset class="validationGroup">
                    <div class="form_left">
                        <label>
                            Usuarios disponibles para asignar</label>
                        <asp:DropDownList ID="ddlUsuariosDisponibles" runat="server" CssClass="mySpecificClass dropdowntext">
                        </asp:DropDownList>
                        <asp:Button ID="btnAdicionarUsuario" runat="server" Text="Adicionar" CssClass="causesValidation" />
                    </div>
                </fieldset>
                <br />
                <div>
                    <asp:GridView ID="gvUsuariosAsignados" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="odd"
                        AutoGenerateColumns="False" CssClass="displayTable" DataKeyNames="Id, CORE_WorkFlow_UsuariosAsignados_Id, UsuarioId"
                        EmptyDataText="No existen registros para mostrar" PagerStyle-CssClass="headerfooter ui-toolbar"
                        Width="100%">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:TemplateField HeaderText="Usuario">
                                <ItemTemplate>
                                    <asp:Label ID="lblTarea" runat="server" Text='<%#Eval("Nombres") + " " + Eval("Apellidos")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Excluir" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgExcluir" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Excluir" ImageUrl="~/Images/delete_16.png" Text="Excluir" ToolTip="Excluir" />
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
                                                Enabled='<%# IIF(gvUsuariosAsignados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvUsuariosAsignados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvTareas.PageIndex + 1%>-
                                                <%= gvUsuariosAsignados.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIF((gvUsuariosAsignados.PageIndex +1) = gvUsuariosAsignados.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIF((gvUsuariosAsignados.PageIndex +1) = gvUsuariosAsignados.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
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
