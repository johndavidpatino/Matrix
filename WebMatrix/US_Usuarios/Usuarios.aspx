<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master"
    CodeBehind="Usuarios.aspx.vb" Inherits="WebMatrix.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../css/jquery-ui.css" rel="stylesheet">
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
            $('#unidadesAsignadas').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Unidades asignadas",
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

            $('#permisosAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Permisos asignados",
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

            $('#rolesAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Roles asignados",
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

            loadPlugins();
        });
        function MostrarUnidadesAsignadas() {
            $('#unidadesAsignadas').dialog("open");
        }
        function MostrarPermisosAsignados() {
            $('#permisosAsignados').dialog("open");
        }
        function MostrarRolesAsignados() {
            $('#rolesAsignados').dialog("open");
        }
    </script>
    <style type="text/css"> 
        .lblTextInfo {
            display: block;
            font-weight: lighter;
            text-align: right;
            padding-top: 5px;
            width: 150px;
            float: left;
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }
        .lblTitleInfo {
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }
    </style>
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
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;" class="lblTitleInfo">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo" class="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;" class="lblTitleInfo">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError" class="lblTextInfo">
            </label>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="actions">
                <div id="lista" runat="server">
                    <h3>
                        <a href="#">
                            <label>
                                Lista de usuarios</label></a></h3>
                    <br />
                    <div class="form_left">
                        <fieldset>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry" Height="17px"></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        </fieldset>
                    </div>
                    <br />
                    <asp:HiddenField ID="hfIdUsuario" runat="server" />
                    <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,Usuario,Nombres,Apellidos,Email,Activo"
                        AlternatingRowStyle-CssClass="odd" CssClass="displayTable" AllowPaging="true" PageSize="25"
                        PagerStyle-CssClass="headerfooter ui-toolbar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" SortExpression="Nombres" />
                            <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" SortExpression="Apellidos" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:TemplateField HeaderText="Activo" ShowHeader="False" Visible="false">
                                <ItemTemplate>
                                   <asp:CheckBox ID="chkActivo" runat="server" Checked='<%# Eval("Activo") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" SortExpression="Activo" />
                            <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Editar" ImageUrl="~/Images/Select_16.png" Text="Editar" ToolTip="Editar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidades" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgUU" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="UsuariosUnidades" ImageUrl="~/Images/Select_16.png" Text="Unidades" ToolTip="Unidades"
                                        OnClientClick="MostrarUnidadesAsignadas()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Permisos" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgPU" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Permisos" ImageUrl="~/Images/Select_16.png" Text="Permisos" ToolTip="Permisos"
                                        OnClientClick="MostrarPermisosAsignados()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roles" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgRU" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Roles" ImageUrl="~/Images/Select_16.png" Text="Permisos" ToolTip="Permisos"
                                        OnClientClick="MostrarRolesAsignados()" />
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
                                            <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                <%= gvDatos.PageCount%>]</span>
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
                <div id="datos" runat="server" visible="false">
                    <h3>
                        <a href="#">
                            <label>
                                Gestión de usuarios</label></a></h3>
                    <br />
                    <div class="actions">
                        <div class="form_left">
                            <label>
                                Identificación:</label>
                            <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_left">
                            <label>
                                Alias Usuario:</label>
                            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_left">
                            <label>
                                Nombres:</label>
                            <asp:TextBox ID="txtNombres" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_left">
                            <label>
                                Apellidos:</label>
                            <asp:TextBox ID="txtApellidos" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_left">
                            <label>
                                Email:</label>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            <p>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ForeColor="Red" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="El formato de el email no es correcto!"></asp:RegularExpressionValidator>
                            </p>
                        </div>
                        <div class="form_left">
                            <label>
                                Activo</label>
                            <asp:CheckBox ID="chkActivo" runat="server" />
                        </div>
                        <div class="form_left">
                            <label>
                                Contraseña:</label>
                            <asp:TextBox ID="txtContraseña2" MaxLength="20" runat="server" TextMode="Password"></asp:TextBox>
                            <label>
                                Confirme Contraseña:</label>
                            <asp:TextBox ID="txtContraseña" MaxLength="20" runat="server" TextMode="Password"></asp:TextBox>
                            <br />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtContraseña2"
                                ControlToValidate="txtContraseña" ErrorMessage="Las contraseñas no coinciden"
                                ForeColor="Red" SetFocusOnError="True"></asp:CompareValidator>
                        </div>
                        <br />
                        <br />
                    </div>
                    <div class="actions">
                        <div>
                            <center>
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                            </center>
                        </div>
                    </div>
                    <div class="form_right">
                        <fieldset>
                            <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Visible="false"
                                Text="Guardar" />
                            <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Visible="false" Text="Editar" />
                            &nbsp;
                            <input id="btnCancelar" type="button" class="button" value="Cancelar" runat="server"
                                style="font-size: 11px;" onclick="location.href='Usuarios.aspx';" />
                        </fieldset>
                    </div>
                </div>
                <div class="actions">
                    <div id="detalles" runat="server" visible="false">
                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="unidadesAsignadas">
        <asp:UpdatePanel ID="upUnidadesAsignadas" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                        <label>
                            Tipo Grupo Unidad:</label>
                        <asp:DropDownList ID="ddlTipoGrupoUnidad" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div class="form_left">
                        <label>
                            Grupo Unidad:</label>
                        <asp:DropDownList ID="ddlGrupoUnidad" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div class="form_left">
                        <label>
                            Unidad:</label>
                        <asp:DropDownList ID="ddlUnidad" runat="server">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <br />
                </div>
                <div class="form_right">
                    <fieldset>
                        <asp:Button ID="btnGrabar" runat="server" CommandName="Grabar" Text="Agregar" />
                    </fieldset>
                </div>
                <asp:GridView ID="gvUnidadesAsignadas" runat="server" AutoGenerateColumns="False" PageSize="25"
                    DataKeyNames="UsuarioId,UnidadId,Unidad" AlternatingRowStyle-CssClass="odd" CssClass="displayTable"
                    AllowPaging="true" PagerStyle-CssClass="headerfooter ui-toolbar" Caption="Unidades asignadas">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="Unidad" />
                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar" />
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
                                            Enabled='<%# IIF(gvUnidadesAsignadas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvUnidadesAsignadas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                            <%= gvUnidadesAsignadas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvUnidadesAsignadas.PageIndex +1) = gvUnidadesAsignadas.PageCount, "false", "true") %>'
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="permisosAsignados">
        <asp:UpdatePanel ID="upPermisos" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                        <label>
                            Permisos:</label>
                        <asp:DropDownList ID="ddlPermisos" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <br />
                </div>
                <div class="form_right">
                    <fieldset>
                        <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar" />
                    </fieldset>
                </div>
                <asp:GridView ID="gvPermisosAsignados" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="PermisoId,UsuarioId" AlternatingRowStyle-CssClass="odd" CssClass="displayTable"
                    AllowPaging="true" PagerStyle-CssClass="headerfooter ui-toolbar" Caption="Permisos asignados">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Permiso" HeaderText="Permiso" ReadOnly="True" SortExpression="Permiso" />
                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar" />
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
                                            Enabled='<%# IIF(gvPermisosAsignados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPermisosAsignados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPermisosAsignados.PageIndex + 1%>-<%= gvPermisosAsignados.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPermisosAsignados.PageIndex +1) = gvPermisosAsignados.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPermisosAsignados.PageIndex +1) = gvPermisosAsignados.PageCount, "false", "true") %>'
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
    <div id="rolesAsignados">
        <asp:UpdatePanel ID="upRolesAsignados" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                        <label>
                            Roles:</label>
                        <asp:DropDownList ID="ddlRolesDisponibles" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <br />
                </div>
                <div class="form_right">
                    <fieldset>
                        <asp:Button ID="btnAgregarRol" runat="server" CommandName="Agregar" Text="Agregar" />
                    </fieldset>
                </div>
                <asp:GridView ID="gvRolesAsignados" runat="server" AutoGenerateColumns="False" PageSize="25"
                    DataKeyNames="RolId,UsuarioId" AlternatingRowStyle-CssClass="odd" CssClass="displayTable"
                    AllowPaging="true" PagerStyle-CssClass="headerfooter ui-toolbar" Caption="Roles asignados">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Rol" HeaderText="Rol" ReadOnly="True" SortExpression="Permiso" />
                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar" />
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
                                            Enabled='<%# IIF(gvRolesAsignados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvRolesAsignados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvRolesAsignados.PageIndex + 1%>-<%= gvRolesAsignados.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvRolesAsignados.PageIndex +1) = gvRolesAsignados.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvRolesAsignados.PageIndex +1) = gvRolesAsignados.PageCount, "false", "true") %>'
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
