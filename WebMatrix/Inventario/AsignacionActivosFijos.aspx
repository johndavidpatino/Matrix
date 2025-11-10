<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="AsignacionActivosFijos.aspx.vb" Inherits="WebMatrix.AsignacionActivosFijos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/asignaciones-activos/asignacion-activos.js" type="module" defer></script>
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

            $("#<%= txtFechaAsignado.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaAsignado.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();

            $('#BusquedaUsuarios').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios",
                width: "600px"
            });

            $('#BusquedaUsuarios').parent().appendTo("form");

            $('#BusquedaJBEJBICC').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "JBEJBICC",
                width: "600px"
            });

            $('#BusquedaJBEJBICC').parent().appendTo("form");

            $('#CargaArchivos').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Carga archivos",
                width: "600px"
            });
        });

        function MostrarUsuarios() {
            $('#BusquedaUsuarios').dialog("open");
        }

        function CerrarUsuarios() {
            $('#BusquedaUsuarios').dialog("close");
        }

        function MostrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("open");
        }

        function CerrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("close");
        }


 


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>ASIGNACIÓN DE ACTIVOS FIJOS</a>

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
    <asp:LinkButton ID="lbtnVolver" Text="Volver" href="../Inventario/RegistroArticulos.aspx" runat="server"></asp:LinkButton>
    <br />
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">

                <div id="accordion0">
                    <h3><a href="#">
                        <label>
                            BUSQUEDA DE ACTIVOS FIJOS
                        </label>
                    </a></h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div class="actions">
                               
                                <div class="form_rigth">
                                    <label>Artículo</label>
                                    <asp:DropDownList ID="ddlIdArticulo" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    <label id="lblIdTipoComputador" runat="server" visible="false">Tipo Computador</label>
                                    <asp:DropDownList ID="ddlIdTipoComputador" runat="server" Visible="false">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Desktop" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Laptop" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label id="lblIdPertenecePC" runat="server" visible="false">Pertenece PC</label>
                                    <asp:DropDownList ID="ddlIdPertenecePC" runat="server" Visible="false">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Adquisición Ipsos" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Alquilado" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label id="lblIdPeriferico" runat="server" visible="false">Tipo Periférico</label>
                                    <asp:DropDownList ID="ddlIdPeriferico" runat="server" Visible="false"></asp:DropDownList>
                                    <label>Estado</label>
                                    <asp:DropDownList ID="ddlIdEstado" runat="server"></asp:DropDownList>
                                    <label>Sede</label>
                                    <asp:DropDownList ID="ddlIdSede" runat="server"></asp:DropDownList>
                                    <label>Asignado</label>
                                    <asp:DropDownList ID="ddlIdAsignado" runat="server">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtIdUsuario" runat="server" placeholder="Cedula"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtNomUsuario" runat="server" placeholder="Usuario"></asp:TextBox>
                                    <label>Búsqueda por Id Físico</label>
                                    <asp:TextBox ID="txtIdArticulo" runat="server" CssClass="textEntry" Width="244px"></asp:TextBox>
                                    <label>Búsqueda</label>
                                    <asp:TextBox ID="txtTodosCampos" runat="server" CssClass="textEntry" Width="244px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                    &nbsp;&nbsp;
                                        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="form_right">

                                <asp:GridView ID="gvArticulos" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="TipoArticulo" HeaderText="Tipo Articulo" Visible="false" />
                                        <asp:BoundField DataField="Articulo" HeaderText="Articulo" />
                                        <asp:BoundField DataField="FechaCompra" HeaderText="Fecha Compra" Visible="false" />
                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" />
                                        <asp:BoundField DataField="UsuarioRegistra" HeaderText="Usuario Registra" />
                                        <asp:BoundField DataField="CentroCosto" HeaderText="Centro de Costo" Visible="false" />
                                        <asp:BoundField DataField="BU" HeaderText="BU" Visible="false" />
                                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" Visible="false" />
                                        <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBookCodigo" Visible="false" />
                                        <asp:BoundField DataField="JobBookNombre" HeaderText="JobBookNombre" Visible="false" />
                                        <asp:BoundField DataField="NumeroCuentaContable" HeaderText="Numero Cuenta Contable" Visible="false" />
                                        <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable" Visible="false" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" Visible="false" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="Symphony" HeaderText="Symphony" Visible="false" />
                                        <asp:BoundField DataField="IdFisico" HeaderText="IdFisico" />
                                        <asp:BoundField DataField="Sede" HeaderText="Sede" />
                                        <asp:BoundField DataField="TipoComputador" HeaderText="TipoComputador" Visible="false" />
                                        <asp:BoundField DataField="PertenecePC" HeaderText="Pertenece PC" Visible="false" />
                                        <asp:BoundField DataField="TipoPeriferico" HeaderText="TipoPeriferico" Visible="false" />
                                        <asp:BoundField DataField="Marca" HeaderText="Marca" Visible="false" />
                                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" Visible="false" />
                                        <asp:BoundField DataField="Procesador" HeaderText="Procesador" Visible="false" />
                                        <asp:BoundField DataField="Memoria" HeaderText="Memoria" Visible="false" />
                                        <asp:BoundField DataField="Almacenamiento" HeaderText="Almacenamiento" Visible="false" />
                                        <asp:BoundField DataField="SistemaOperativo" HeaderText="SistemaOperativo" Visible="false" />
                                        <asp:BoundField DataField="Serial" HeaderText="Serial" Visible="false" />
                                        <asp:BoundField DataField="NombreEquipo" HeaderText="Nombre Equipo" Visible="false" />
                                        <asp:BoundField DataField="Office" HeaderText="Serial Windows" Visible="false" />
                                        <asp:BoundField DataField="Programas" HeaderText="Programas" Visible="false" />
                                        <asp:BoundField DataField="TipoServidor" HeaderText="TipoServidor" Visible="false" />
                                        <asp:BoundField DataField="Raid" HeaderText="Raid" Visible="false" />
                                        <asp:BoundField DataField="IdTablet" HeaderText="IdTablet" Visible="false" />
                                        <asp:BoundField DataField="IdSTG" HeaderText="IdSTG" Visible="false" />
                                        <asp:BoundField DataField="TamanoPantalla" HeaderText="Tamaño Pantalla" Visible="false" />
                                        <asp:BoundField DataField="Chip" HeaderText="Chip" Visible="false" />
                                        <asp:BoundField DataField="IMEI" HeaderText="IMEI" Visible="false" />
                                        <asp:BoundField DataField="Pertenece" HeaderText="Pertenece Tablet" Visible="false" />
                                        <asp:BoundField DataField="Operador" HeaderText="Operador" Visible="false" />
                                        <asp:BoundField DataField="NumeroCelular" HeaderText="Numero Celular" Visible="false" />
                                        <asp:BoundField DataField="CantidadMinutos" HeaderText="Cantidad Minutos" Visible="false" />
                                        <asp:BoundField DataField="TipoProducto" HeaderText="Tipo Producto" Visible="false" />
                                        <asp:BoundField DataField="Producto" HeaderText="Producto" Visible="false" />
                                        <asp:BoundField DataField="TipoBono" HeaderText="Tipo Bono" Visible="false" />
                                        <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" Visible="false" />
                                        <asp:BoundField DataField="Area" HeaderText="Area" Visible="false" />
                                        <asp:BoundField DataField="BU" HeaderText="BU" Visible="false" />
                                        <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado" />
                                        <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignacion" />
                                        <asp:BoundField DataField="ObservacionAsignacion" HeaderText="Observacion Asignacion" />
                                        <asp:TemplateField HeaderText="Asignar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnAsignar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Asignar" ImageUrl="~/Images/Select_16.png" Text="Asignar"
                                                    ToolTip="Asignar" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerTemplate>
                                        <div class="pagingButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvArticulos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvArticulos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td><span class="pagingLinks">[<%= gvArticulos.PageIndex + 1%>-<%= gvArticulos.PageCount%>]</span> </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvArticulos.PageIndex + 1) = gvArticulos.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvArticulos.PageIndex + 1) = gvArticulos.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
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
                <div id="accordion1">
                    <h3><a href="#">
                        <label>
                            ASIGNAR UN ACTIVO FIJO
                        </label>
                    </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
                                <asp:HiddenField ID="hfIdTrabajo" runat="server" Value="0" />
                                <asp:HiddenField ID="hfIdCargo" runat="server" Value="0" />
                                <asp:Label ID="lblAsignar" runat="server" Text="Asignación Artículo:" Visible="false"></asp:Label>
                                <asp:Label ID="lblIdAsignar" runat="server" Visible="false"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblArticulo" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div>
                                <label>Fecha de Asignación</label>
                                <asp:TextBox ID="txtFechaAsignado" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                <label id="lblCentroCosto" runat="server" visible="false">Centro de Costo</label>
                                <asp:DropDownList ID="ddlCentroCosto" runat="server" AutoPostBack="true" Visible="false">
                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="JBE - JobBookExterno" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="JBI - JobBookInterno" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="BU - Business Unit" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <label id="lblBU" runat="server" visible="false">Unidad de Negocio</label>
                                <asp:DropDownList ID="ddlBU" runat="server" Visible="false"></asp:DropDownList>
                                <label id="lblJBIJBE" runat="server" visible="false">Codigo JBIJBE</label>
                                <asp:TextBox ID="txtJBIJBE" runat="server" Visible="false" AutoPostBack="true" Width="245px"></asp:TextBox>
                                <asp:Button ID="btnSearchJBEJBICC" Text="..." runat="server" Width="25px" OnClientClick="MostrarJBEJBICC()" Visible="false" /><br />
                                <label id="lblNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                                <asp:TextBox ID="txtNombreJBIJBE" runat="server" Visible="false" Width="243px"></asp:TextBox>
                                <label>Ciudad</label>
                                <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="true"></asp:DropDownList>
                                <label id="lblSede" runat="server" visible="false">Sede</label>
                                <asp:DropDownList ID="ddlSede" runat="server" visible="false">
                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Calle 74" Value="74"></asp:ListItem>
                                    <asp:ListItem Text="Calle 78" Value="78"></asp:ListItem>
                                </asp:DropDownList>
                                <label>Tipo Grupo Unidad</label>
                                <asp:DropDownList ID="ddlTipoGrupoUnidad" runat="server" AutoPostBack="True"></asp:DropDownList>
                                <label>Grupo Unidad</label>
                                <asp:DropDownList ID="ddlGrupoUnidad" runat="server" AutoPostBack="True"></asp:DropDownList>
                                <label>Unidad</label>
                                <asp:DropDownList ID="ddlUnidad" runat="server"> </asp:DropDownList>
                                <label id="lblEstadoTablet" runat="server" visible="false">Estado Tablet</label>
                                <asp:DropDownList ID="ddlEstadoTablet" runat="server" Visible="false"></asp:DropDownList>
                                <label>Usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" ReadOnly="true" Width="245px"></asp:TextBox>
                                <asp:Button ID="btnSearchUsuario" Text="..." runat="server" OnClientClick="MostrarUsuarios()" /><br />
                                <label>Cargo</label>
                                <asp:TextBox ID="txtCargo" runat="server" ReadOnly="true" Width="244px"></asp:TextBox>
                                <label>Observación</label>
                                <asp:TextBox ID="txtObservacion" runat="server" CssClass="textEntry" Width="480px"></asp:TextBox>
                                <br />
                                <label>Correo donde se enviará la constancia</label>
                                <asp:TextBox ID="txtEmailToSendFile" runat="server" CssClass="textEntry" Width="480px"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnAsignar" runat="server" CssClass="causesValidation" Text="Asignar" />
                                &nbsp;
                           <asp:Button ID="btnDesasignar" runat="server" CssClass="causesValidation" Text="Desasignar" Visible="false" />
                                &nbsp; 
                           <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                            </div>
                        </fieldset>
                    </div>
                   <asp:Literal ID="correoAsignacion" runat="server" />
                </div>
                <div id="accordion2">
                    <h3><a href="#">
                        <label>
                            LISTADO DE ASIGNACIONES
                        </label>
                    </a></h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div class="actions">

                                <div>
                                    <label>Artículo</label>
                                    <asp:DropDownList ID="ddlArticuloAS" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    <label id="lblBUAS" runat="server" visible="false">Unidad de Negocio</label>
                                    <asp:DropDownList ID="ddlBUAS" runat="server" Visible="false"></asp:DropDownList>
                                    <label id="lblJBIJBEAS" runat="server" visible="false">Codigo JBIJBE</label>
                                    <asp:TextBox ID="txtJBIJBEAS" runat="server" Visible="false" Width="200px"></asp:TextBox>
                                    <label id="lblNombreJBIJBEAS" runat="server" visible="false">Nombre JBIJBE</label>
                                    <asp:TextBox ID="txtNombreJBIJBEAS" runat="server" Visible="false" Width="200px"></asp:TextBox>
                                    <label>Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudadAS" runat="server"></asp:DropDownList>
                                    <label id="lblEstadoTabletAS" runat="server" visible="false">Estado Tablet</label>
                                    <asp:DropDownList ID="ddlEstadoTabletAS" runat="server" Visible="false"></asp:DropDownList>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtIdUsuarioAs" runat="server" placeholder="Cedula" Width="158px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtUsuarioAs" runat="server" placeholder="Usuario" Width="158px"></asp:TextBox>
                                    <label>Tipo Cargo</label>
                                    <asp:DropDownList ID="ddlTipoCargoAS" runat="server">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Ipsos" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Contratista" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label>Asignado</label>
                                    <asp:DropDownList ID="ddlAsignadoAS" runat="server">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnBuscarAS" runat="server" Text="Buscar" />
                                    &nbsp;&nbsp;
                                        <asp:Button ID="btnExportarAS" runat="server" Text="Exportar" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="form_right">

                                <asp:GridView ID="gvAsignaciones" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="id,IdActivoFijo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IdActivoFijo" HeaderText="Id" />
                                        <asp:BoundField DataField="Articulo" HeaderText="Articulo" visible="false"/>
                                        <asp:BoundField DataField="TipoComputador" HeaderText="Tipo Computador" Visible="false" />
                                        <asp:BoundField DataField="TipoPeriferico" HeaderText="Tipo Periferico" Visible="false" />
                                        <asp:BoundField DataField="Marca" HeaderText="Marca" Visible="false" />
                                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" Visible="false" />
                                        <asp:BoundField DataField="Serial" HeaderText="Serial" Visible="false" />
                                        <asp:BoundField DataField="NombreEquipo" HeaderText="Nombre Equipo" Visible="false" />
                                        <asp:BoundField DataField="IdTablet" HeaderText="IdTablet" Visible="false" />
                                        <asp:BoundField DataField="Chip" HeaderText="Chip" Visible="false" />
                                        <asp:BoundField DataField="IMEI" HeaderText="IMEI" Visible="false" />
                                        <asp:BoundField DataField="NumeroCelular" HeaderText="Numero Celular" Visible="false" />
                                        <asp:BoundField DataField="CantidadMinutos" HeaderText="Cantidad Minutos" Visible="false" />
                                        <asp:BoundField DataField="UsuarioRegistra" HeaderText="Usuario Registra" />
                                        <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignacion" />
                                        <asp:BoundField DataField="CentroCosto" HeaderText="Centro de Costo" Visible="false" />
                                        <asp:BoundField DataField="BU" HeaderText="BU" Visible="false" />
                                        <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBookCodigo" Visible="false" />
                                        <asp:BoundField DataField="JobBookNombre" HeaderText="JobBookNombre" Visible="false" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="Sede" HeaderText="Sede" />
                                        <asp:BoundField DataField="GrupoUnidad" HeaderText="Grupo Unidad" />
                                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                        <asp:BoundField DataField="EstadoTablet" HeaderText="EstadoTablet" Visible="false" />
                                        <asp:BoundField DataField="IdFisico" HeaderText="Id Fisico" Visible="false" />
                                        <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                                        <asp:BoundField DataField="UsuarioAsignado" HeaderText="UsuarioAsignado" />
                                        <asp:BoundField DataField="TipoCargo" HeaderText="TipoCargo" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                                    ToolTip="Actualizar"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Abrir PDF" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnVerPDF" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="OpenPDF" ImageUrl="~/Images/pdf.png" Text="Abrir PDF"
                                                    ToolTip="Ver PDF"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerTemplate>
                                        <div class="pagingButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvAsignaciones.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvAsignaciones.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td><span class="pagingLinks">[<%= gvAsignaciones.PageIndex + 1%>-<%= gvAsignaciones.PageCount%>]</span> </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvAsignaciones.PageIndex + 1) = gvAsignaciones.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvAsignaciones.PageIndex + 1) = gvAsignaciones.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaUsuarios">
        <asp:UpdatePanel ID="UPanelUsuarios" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaUsuario" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreUsuario" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarUsuario" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvUsuarios" runat="server" Width="90%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarUsuarios()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <div id="BusquedaJBEJBICC">
        <asp:UpdatePanel ID="upJBEJBICC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Búsqueda" Width="176px"></asp:TextBox>
                <asp:Button ID="btnBuscarJBEJBICC" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvJBEJBICC" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarJBEJBICC()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
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
