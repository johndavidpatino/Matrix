<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="RegistroArticulos.aspx.vb" Inherits="WebMatrix.RegistroArticulos" %>

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

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFechaCompra.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaCompra.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();

            $('#BusquedaJBEJBICC').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "JBEJBICC",
                width: "600px"
            });

            $('#BusquedaJBEJBICC').parent().appendTo("form");

            $('#BusquedaCuentasContables').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Cuentas Contables",
                width: "600px"
            });

            $('#BusquedaCuentasContables').parent().appendTo("form");

            $('#BusquedaProveedores').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Proveedores disponibles",
                width: "600px"
            });

            $('#BusquedaProveedores').parent().appendTo("form");

        });

        function MostrarCentrosCostos() {
            $('#BusquedaJBEJBICC').dialog("open");
        }
        function MostrarCuentasContables() {
            $('#BusquedaCuentasContables').dialog("open");
        }
        function CerrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("close");
        }
        function CerrarCuentasContables() {
            $('#BusquedaCuentasContables').dialog("close");
        }

        function MostrarProveedores() {
            $('#BusquedaProveedores').dialog("open");
        }

        function CerrarProveedores() {
            $('#BusquedaProveedores').dialog("close");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <p>
        &nbsp;</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>REGISTRO DE ARTICULOS</a>
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
    <div id="menu-form">
    <Nav>
           <ul>
                <li><a><asp:LinkButton ID="lbtnAsignaciones" Text ="Asignaciones" href="../Inventario/AsignacionActivosFijos.aspx" runat="server"></asp:LinkButton></a></li>
               <li><a><asp:LinkButton ID="lbtnMantenimiento" Text ="Mantenimiento Equipos" href="../Inventario/MantenimientoEquipos.aspx" runat="server"></asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lbtnStock" Text ="Stock Consumibles" href="../Inventario/EntregaConsumibles.aspx" runat="server"></asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lbtnLegalizaciones" Text ="Legalizaciones" href="../Inventario/Legalizaciones.aspx" runat="server"></asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lbtnRemanente" Text ="Remanente" href="../Inventario/ReporteRemanente.aspx" runat="server"></asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lbtnVolver" Text ="Volver" href="../Home.aspx" runat="server"></asp:LinkButton></a></li>
           </ul>
    </Nav>
        </div>

    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>  <a href="#">
                        <label>
                           DATOS DEL ARTÍCULO
                        </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                    <asp:label ID="lblActualizar" runat="server" Text="Se esta actualizando el Id:" Visible="false" ></asp:label>
                                    <asp:label ID="lblIdActualizar" runat="server" Visible="false" ></asp:label>
                                    </div>
                            <div>
                            <asp:HiddenField ID="hfIdTrabajo" runat="server" Value="0" />
                            <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                            <asp:Label ID="lblTipoArticulo" AssociatedControlID="ddlTipoArticulo" Text="Tipo Artículo" runat="server" ForeColor="White" Font-Bold="True" Font-Size="14px" Font-Names="'Metrophobic', Arial, serif" />
                            <asp:DropDownList ID="ddlTipoArticulo" CssClass="mySpecificClass" autopostback="true" runat="server"> 
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Activo Fijo" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Consumible" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblArticulo" AssociatedControlID="ddlArticulo" Text="Artículo" runat="server" ForeColor="White" Font-Bold="True" Font-Size="14px" Font-Names="'Metrophobic', Arial, serif" />
                            <asp:DropDownList ID="ddlArticulo" CssClass="mySpecificClass" autopostback="true" runat="server"> </asp:DropDownList>
                            <asp:Label ID="lblPapeleria" AssociatedControlID="ddlPapeleria" Text="Producto Papeleria" runat="server" ForeColor="White" Font-Bold="True" Font-Size="14px" Font-Names="'Metrophobic', Arial, serif" Visible="false" />
                            <asp:DropDownList ID="ddlPapeleria" runat="server" Visible="false"> </asp:DropDownList>
                            <asp:Label ID="lblFechaCompra" AssociatedControlID="txtFechaCompra" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Fecha de Ingreso" />
                            <asp:TextBox ID="txtFechaCompra" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <asp:Label ID="lblCentroCosto" AssociatedControlID="ddlCentroCosto" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Centro de Costo" Visible="false"/>
                            <asp:DropDownList ID="ddlCentroCosto" runat="server" AutoPostBack="true" Visible="false">
                            <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="JBE - JobBookExterno" Value="1"></asp:ListItem>
                            <asp:ListItem Text="JBI - JobBookInterno" Value="2"></asp:ListItem>    
                            <asp:ListItem Text="BU - Business Unit" Value="3"></asp:ListItem>    
                            </asp:DropDownList>
                            <asp:Label ID="lblBU" AssociatedControlID="ddlBU" runat="server" Font-Bold="True" Font-Names="'Metrophobic',Arial,serif" Font-Size="14px" ForeColor="White" Text="Unidad de Negocio" Visible="False" Height="16px"/>
                            <asp:DropDownList ID="ddlBU" runat="server" Visible="false"></asp:DropDownList>
                            <asp:Label ID="lblJBIJBE" AssociatedControlID="txtJBIJBE" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Codigo JBIJBE" Visible="false"/>
                            <asp:TextBox ID="txtJBIJBE" runat="server" Visible="false" AutoPostBack="true"></asp:TextBox>
                            <asp:Label ID="lblNombreJBIJBE" AssociatedControlID="txtNombreJBIJBE" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Nombre JBIJBE" Visible="false"/>
                            <asp:TextBox ID="txtNombreJBIJBE" runat="server" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblCuentasContables" AssociatedControlID="ddlCuentasContables" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Cuenta Contable" Visible="false"/>
                            <asp:DropDownList ID="ddlCuentasContables" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                            <asp:Button ID="btnCuentasContables" Text="..." runat="server" Width="25px" OnClientClick="MostrarCuentasContables()" visible="false"/><br />
                            <asp:Label ID="lblValorUnitario" AssociatedControlID="txtValorUnitario" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Valor Unitario" visible="false"/>
                            <asp:TextBox ID="txtValorUnitario" runat="server" CssClass="required text textEntry" visible="false"></asp:TextBox>
                            <asp:Label ID="lblCantidad" AssociatedControlID="txtCantidad" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Cantidad" visible="false"/>
                            <asp:TextBox ID="txtCantidad" runat="server" CssClass="required text textEntry" visible="false"></asp:TextBox>
                            <asp:Label ID="lblEstado" AssociatedControlID="ddlEstado" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Estado" Visible="false"/>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="mySpecificClass" Visible="false">
                            </asp:DropDownList>
                            <asp:Label ID="lblDescripcion" AssociatedControlID="txtDescripcion" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Descripción" />
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textEntry" Width="480px"></asp:TextBox>
                            <asp:Label ID="lblNumeroPV" AssociatedControlID="txtNumeroPV" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Número PV" Visible="false"/>
                            <asp:TextBox ID="txtNumeroPV" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblProveedor" AssociatedControlID="txtProveedor" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Proveedor" Visible="false"/>
                            <asp:TextBox ID="txtProveedor" runat="server" ReadOnly="true" Width="205px" Visible="false"></asp:TextBox>
                            <asp:Button ID="btnSearchProveedor" Text="..." runat="server" Width="25px" OnClientClick="MostrarProveedores()" Visible="false" /><br />
                            <asp:Label ID="lblSymphony" AssociatedControlID="txtSymphony" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Symphony" Visible="false"/>
                            <asp:TextBox ID="txtSymphony" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblIdFisico" AssociatedControlID="txtIdFisico" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Id Físico" Visible="false"/>
                            <asp:TextBox ID="txtIdFisico" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                           <asp:Label ID="lblSede" AssociatedControlID="ddlSede" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Sede"/>
                           <asp:DropDownList ID="ddlSede" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                            <asp:Label ID="lblTipoComputador" AssociatedControlID="ddlTipoComputador" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo Computador" Visible="false" />
                            <asp:DropDownList ID="ddlTipoComputador" runat="server" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Desktop" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Laptop" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblPertenecePC" AssociatedControlID="ddlPertenecePC" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Pertenece" Visible="false" />
                            <asp:DropDownList ID="ddlPertenecePC" runat="server" autopostback="true" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Adquisición Ipsos" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Rentado" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblFechaFin" AssociatedControlID="txtFechaFin" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Fecha Fin Renta" Visible="false" />
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="bgCalendar textCalendarStyle" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblTipoPeriferico" AssociatedControlID="ddlTipoPeriferico" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo Periférico" Visible="false" />
                            <asp:DropDownList ID="ddlTipoPeriferico" runat="server" Visible="false">
                            </asp:DropDownList>
                            <asp:Label ID="lblMarca" AssociatedControlID="txtMarca" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Marca" Visible="false" />
                            <asp:TextBox ID="txtMarca" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblModelo" AssociatedControlID="txtModelo" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Modelo" Visible="false" />
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblProcesador" AssociatedControlID="txtProcesador" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Procesador" Visible="false" />
                            <asp:TextBox ID="txtProcesador" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblMemoria" AssociatedControlID="txtMemoria" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Memoria" Visible="false" />
                            <asp:TextBox ID="txtMemoria" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblAlmacenamiento" AssociatedControlID="txtAlmacenamiento" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Almacenamiento" Visible="false" />
                            <asp:TextBox ID="txtAlmacenamiento" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblSistemaOperativo" AssociatedControlID="txtSistemaOperativo" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Sistema Operativo" Visible="false" />
                            <asp:TextBox ID="txtSistemaOperativo" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblSerial" AssociatedControlID="txtSerial" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Serial" Visible="false" />
                            <asp:TextBox ID="txtSerial" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblNombreEquipo" AssociatedControlID="txtNombreEquipo" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Nombre Equipo" Visible="false" />
                            <asp:TextBox ID="txtNombreEquipo" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblOffice" AssociatedControlID="txtOffice" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Serial Windows" Visible="false" />
                            <asp:TextBox ID="txtOffice" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblProgramas" AssociatedControlID="txtProgramas" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Programas" Visible="false" />
                            <asp:TextBox ID="txtProgramas" runat="server" CssClass="textEntry" Visible="false" Width="392px"></asp:TextBox>
                            <asp:Label ID="lblTipoServidor" AssociatedControlID="txtTipoServidor" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo de Servidor" Visible="false" />
                            <asp:TextBox ID="txtTipoServidor" runat="server" CssClass="textEntry" Visible="false" Width="260px"></asp:TextBox>
                            <asp:Label ID="lblRaid" AssociatedControlID="txtRaid" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Raid" Visible="false" />
                            <asp:TextBox ID="txtRaid" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblIdTablet" AssociatedControlID="txtIdTablet" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Id Tablet" Visible="false" />
                            <asp:TextBox ID="txtIdTablet" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblIdSTG" AssociatedControlID="txtIdSTG" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Id STG" Visible="false" />
                            <asp:TextBox ID="txtIdSTG" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblTamanoPantalla" AssociatedControlID="txtTamanoPantalla" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tamaño Pantalla" Visible="false" />
                            <asp:TextBox ID="txtTamanoPantalla" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblChip" AssociatedControlID="txtChip" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Chip" Visible="false" />
                            <asp:TextBox ID="txtChip" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblIMEI" AssociatedControlID="txtIMEI" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="IMEI" Visible="false" />
                            <asp:TextBox ID="txtIMEI" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblPertenece" AssociatedControlID="ddlPertenece" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Pertenece" Visible="false" />
                            <asp:DropDownList ID="ddlPertenece" runat="server" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Adquisición Ipsos" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Leasing BANCOLOMBIA" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Alquilado a Proveedor" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblOperador" AssociatedControlID="ddlOperador" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Operador" Visible="false" />
                            <asp:DropDownList ID="ddlOperador" runat="server" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Claro" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ETB" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Movistar" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Tigo" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Virgin" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblNumCelular" AssociatedControlID="txtNumCelular" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Numero Celular" Visible="false" />
                            <asp:TextBox ID="txtNumCelular" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblMinutos" AssociatedControlID="txtMinutos" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Cantidad Minutos" Visible="false" />
                            <asp:TextBox ID="txtMinutos" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblTipoProducto" AssociatedControlID="ddlTipoProducto" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo de Producto" Visible="false" />
                            <asp:DropDownList ID="ddlTipoProducto" runat="server" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Obsequio" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Prueba Producto" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblProducto" AssociatedControlID="txtProducto" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Producto" Visible="false" />
                            <asp:TextBox ID="txtProducto" runat="server" CssClass="textEntry" Visible="false" Width="261px"></asp:TextBox>
                            <asp:Label ID="lblTipoObsequio" AssociatedControlID="txtProducto" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo de Obsequio" Visible="false" />
                            <asp:DropDownList ID="ddlTipoObsequio" runat="server" Visible="false">
                            <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Comprado por Ipsos" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Dado por Cliente" Value="2"></asp:ListItem>  
                            </asp:DropDownList>
                            <asp:Label ID="lblTipoBono" AssociatedControlID="ddlTipoBono" runat="server" Font-Bold="True" Font-Names="'Metrophobic', Arial, serif" Font-Size="14px" ForeColor="White" Text="Tipo de Bono" Visible="false" />
                            <asp:DropDownList ID="ddlTipoBono" runat="server" Visible="false">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Sodexo" Value="1"></asp:ListItem>
                                <asp:ListItem Text="SmartGift" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Otro" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            
                                <br />
                            <asp:Button ID="btnGuardar" runat="server" CssClass="causesValidation" Text="Guardar" />
                            &nbsp;
                           <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                       </div>
                      </fieldset>
                    </div>
                 </div>

                <div id="accordion1">
                    <h3><a href="#">
                        <label>
                        LISTA DE ARTÍCULOS REGISTRADOS
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
                                            <asp:ListItem Text="Rentado" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label id="lblIdPeriferico" runat="server" visible="false">Tipo Periférico</label>
                                        <asp:DropDownList ID="ddlIdPeriferico" runat="server" Visible="false"></asp:DropDownList>
                                        <label id="lblIdTipoProducto" runat="server" visible="false">Tipo de Producto</label>
                                        <asp:DropDownList ID="ddlIdTipoProducto" runat="server" Visible="false">
                                            <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Obsequio" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Prueba Producto" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label>Estado</label>
                                        <asp:DropDownList ID="ddlIdEstado" runat="server"></asp:DropDownList>
                                        <label>Sede</label>
                                        <asp:DropDownList ID="ddlIdSede" runat="server"></asp:DropDownList>
                                        <label>Busqueda</label>
                                        <asp:TextBox ID="txtTodosCampos" runat="server" CssClass="textEntry" Width="244px"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"/>
                                        &nbsp;&nbsp;&nbsp;
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
                                <asp:BoundField DataField="TipoArticulo" HeaderText="Tipo Articulo" />
                                <asp:BoundField DataField="Articulo" HeaderText="Articulo" />
                                <asp:BoundField DataField="FechaCompra" HeaderText="Fecha Ingreso" DataFormatString="{0:dd/MM/yyyy}"  />
                                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy}"  />
                                <asp:BoundField DataField="UsuarioRegistra" HeaderText="Usuario Registra" />
                                <asp:BoundField DataField="CentroCosto" HeaderText="Centro de Costo" Visible="false"/>
                                <asp:BoundField DataField="BU" HeaderText="BU" Visible="false"/>
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" Visible="false"/>
                                <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBook Codigo" Visible="false"/>
                                <asp:BoundField DataField="JobBookNombre" HeaderText="JobBook Nombre" Visible="false"/>
                                <asp:BoundField DataField="NumeroCuentaContable" HeaderText="Numero Cuenta Contable" Visible="false"/>
                                <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable" Visible="false"/>
                                <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" Visible="false" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Symphony" HeaderText="Symphony" Visible="false"/>
                                <asp:BoundField DataField="IdFisico" HeaderText="IdFisico" Visible="false"/>
                                <asp:BoundField DataField="Sede" HeaderText="Sede"/>
                                <asp:BoundField DataField="IdUsuarioAsignado" HeaderText="Id Usuario Asignado"/>
                                <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado"/>
                                <asp:BoundField DataField="TipoComputador" HeaderText="Tipo Computador" Visible="false"/>
                                <asp:BoundField DataField="PertenecePC" HeaderText="Pertenece PC" Visible="false"/>
                                <asp:BoundField DataField="TipoPeriferico" HeaderText="Tipo Periferico" Visible="false"/>
                                <asp:BoundField DataField="Marca" HeaderText="Marca" Visible="false"/>
                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" Visible="false"/>
                                <asp:BoundField DataField="Procesador" HeaderText="Procesador" Visible="false"/>
                                <asp:BoundField DataField="Memoria" HeaderText="Memoria" Visible="false"/>
                                <asp:BoundField DataField="Almacenamiento" HeaderText="Almacenamiento" Visible="false"/>
                                <asp:BoundField DataField="SistemaOperativo" HeaderText="Sistema Operativo" Visible="false"/>
                                <asp:BoundField DataField="Serial" HeaderText="Serial" Visible="false"/>
                                <asp:BoundField DataField="NombreEquipo" HeaderText="Nombre Equipo" Visible="false"/>
                                <asp:BoundField DataField="Office" HeaderText="Office" Visible="false"/>
                                <asp:BoundField DataField="Programas" HeaderText="Programas" Visible="false"/>
                                <asp:BoundField DataField="TipoServidor" HeaderText="Tipo Servidor" Visible="false"/>
                                <asp:BoundField DataField="Raid" HeaderText="Raid" Visible="false"/>
                                <asp:BoundField DataField="IdTablet" HeaderText="Id Tablet" Visible="false"/>
                                <asp:BoundField DataField="IdSTG" HeaderText="Id STG" Visible="false"/>
                                <asp:BoundField DataField="TamanoPantalla" HeaderText="Tamaño Pantalla" Visible="false"/>
                                <asp:BoundField DataField="Chip" HeaderText="Chip" Visible="false"/>
                                <asp:BoundField DataField="IMEI" HeaderText="IMEI" Visible="false"/>
                                <asp:BoundField DataField="Pertenece" HeaderText="Pertenece Tablet" Visible="false"/>
                                <asp:BoundField DataField="Operador" HeaderText="Operador" Visible="false"/>
                                <asp:BoundField DataField="NumeroCelular" HeaderText="Numero Celular" Visible="false"/>
                                <asp:BoundField DataField="CantidadMinutos" HeaderText="Cantidad Minutos" Visible="false"/>
                                <asp:BoundField DataField="TipoProducto" HeaderText="Tipo Producto" Visible="false"/>
                                <asp:BoundField DataField="Producto" HeaderText="Producto" Visible="false"/>
                                <asp:BoundField DataField="TipoObsequio" HeaderText="Tipo Obsequio" Visible="false"/>
                                <asp:BoundField DataField="TipoBono" HeaderText="Tipo Bono" Visible="false"/>
                                <asp:BoundField DataField="FechaFinRenta" HeaderText="Fecha Fin Renta" Visible="false"/>
                                <asp:BoundField DataField="ProductoPapeleria" HeaderText="Producto Papeleria" Visible="false"/>
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="BusquedaCuentasContables">
        <asp:UpdatePanel ID="upCuentasContables" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtNumeroCuenta" runat="server" placeholder="Número cuenta"></asp:TextBox>
                <asp:TextBox ID="txtDescripcionCC" runat="server" placeholder="Descripción"></asp:TextBox>
                <asp:Button ID="btnBuscarCuentaContable" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvCuentasContables" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarCuentasContables()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="BusquedaProveedores">
        <asp:UpdatePanel ID="UPanelProveedores" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtNitProveedor" runat="server" placeholder="NIT o CC"></asp:TextBox>
                <asp:TextBox ID="txtNombreProveedor" runat="server" placeholder="Razón Social o Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarProveedor" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvProveedores" runat="server" Width="90%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Identificacion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                            <asp:BoundField DataField="Nombre" HeaderText="RazonSocial" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
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
