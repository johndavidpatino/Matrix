<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="Gestion-Ordenes1.aspx.vb" Inherits="WebMatrix.FI_Gestion_Ordenes1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" href="../Styles/NewSite.css" type="text/css" />--%>
    <link rel="stylesheet" href="../Styles/Newformulario.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();

            $('#DevolucionTarea').parent().appendTo("form");

            $('#BusquedaProveedores').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Proveedores disponibles",
                    width: "600px"
                });

            $('#BusquedaProveedores').parent().appendTo("form");

            $('#BusquedaSolicitantes').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Solicitantes",
                    width: "600px"
                });

            $('#BusquedaSolicitantes').parent().appendTo("form");

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
                    title: "Búsqueda JBEJBICC",
                    width: "600px"
                });

            $('#BusquedaCuentasContables').parent().appendTo("form");

            $('#BusquedaObservacionesAprobacion').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Observaciones de Aprobación",
                    width: "600px"
                });

            $('#BusquedaObservacionesAprobacion').parent().appendTo("form");

            $('#anularOrden').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Anular Orden",
                    width: "600px"
                });

            $('#anularOrden').parent().appendTo("form");



            $('#CargaArchivos').dialog({
                modal: true,
                autoOpen: false,
                title: "Carga archivos",
                width: "600px"
            });

        });

        function MostrarProveedores() {
            $('#BusquedaProveedores').dialog("open");
        }

        function CerrarProveedores() {
            $('#BusquedaProveedores').dialog("close");
        }

        function MostrarSolicitantes(tipo) {
            $('#CPH_Content_hfTipoAprobacion').val(tipo);
            $('#BusquedaSolicitantes').dialog("open");
        }

        function CerrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("close");
        }
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

        function MostrarObservacionesAprobacion() {
            $('#BusquedaObservacionesAprobacion').dialog("open");
        }
        function CerrarObservacionesAprobacion() {
            $('#BusquedaObservacionesAprobacion').dialog("close");
        }

        function bindPickerIni() {
            $("input[type=text][id*=txtFecha]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFin() {
            $("input[type=text][id*=txtFechaEntrega]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFin() {
            $("input[type=text][id*=txtFechaSolicitud]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function MostrarAnularOrden(tipo) {
            $('#anularOrden').dialog("open");
        }

        function CerrarAnularOrden() {
            $('#anularOrden').dialog("close");
        }

        function bloquearUI() {
            $.blockUI({ message: "Procesando ...." });
        }
    </script>
    <style>
        .transparent {
            background-color: transparent !important;
            float: none !important;
        }

        .btn {
            float: left !important;
            padding: 0 20px !important;
            height: 30px !important;
            font-size: 12px !important;
            color: #fff !important;
            outline: none !important;
            background-color: #00ada8 !important;
            border: none !important;
            transition: all 0.2s ease !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Gestión de Órdenes de Compra y Servicio
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../FI_AdministrativoFinanciero/Default-Compras.aspx">COMPRAS</a>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Gestión de Órdenes
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
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
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <asp:UpdatePanel ID="CPH_Content" runat="server">
        <ContentTemplate>
            <div class="col-md-12">
                <asp:HiddenField ID="hfId" runat="server" Value="0" />
                <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
                <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
                <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                <asp:HiddenField ID="hfTipoBusqueda" runat="server" Value="0" />
                <asp:HiddenField ID="hfProveedorSearch" runat="server" Value="0" />
                <asp:HiddenField ID="hfSolicitanteSearch" runat="server" Value="0" />
                <asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
                <asp:HiddenField ID="hfDuplicar" runat="server" Value="0" />
                <asp:HiddenField ID="hfIdAnterior" runat="server" Value="0" />
                <asp:HiddenField ID="hfEvaluador" runat="server" Value="0" />
                <asp:HiddenField ID="hfTipoAprobacion" runat="server" />

                <div class="col-md-12">
                    <ul class="mi-menu" style="float: left;">
                        <li style="border-bottom: solid 1px #00ada8; margin: 3px;">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="menuLink">Ordenes de Servicio</asp:LinkButton></a>
                        </li>
                        <li style="border-bottom: solid 1px #00ada8; margin: 3px;">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="menuLink">Ordenes de Compra</asp:LinkButton></a>
                        </li>
                        <li style="border-bottom: solid 1px #00ada8; margin: 3px;">
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="menuLink">Requerimientos de Servicio</asp:LinkButton></a>
                        </li>
                    </ul>
                </div>
                <div class="col-md-12">
                    <div class="panel panel-primary" id="pnlTitulo" runat="server" visible="false" style="border: none;">
                        <div class="panel-heading text-left">
                            <asp:Label ID="txtTitulo" runat="server" CssClass="panel-title"></asp:Label>
                        </div>
                        <div class="panel-body" style="background-image: url(../images/fnd-section.png); background-position: right; background-repeat: no-repeat;">

                            <div class="col-md-2" style="margin: 0 -10px;">
                                <%--<a href="#" class="nav-tabs-dropdown btn btn-block btn-primary">Tabs</a>--%>
                                <ul id="nav-tabs-wrapper" class="nav nav-tabs nav-pills nav-stacked well">
                                    <li id="liMenu1" runat="server">
                                        <a href="#vtab1" data-toggle="tab" style="display: none;">
                                            <asp:LinkButton ID="lbMenu1" runat="server">Nueva Orden</asp:LinkButton>
                                        </a>
                                    </li>
                                    <li id="liMenu2" runat="server">
                                        <a href="#vtab2" data-toggle="tab" style="display: none;">
                                            <asp:LinkButton ID="lbMenu2" runat="server">Buscar</asp:LinkButton>
                                        </a>
                                    </li>
                                    <li id="liMenu3" runat="server">
                                        <a href="#vtab3" data-toggle="tab" style="display: none;">
                                            <asp:LinkButton ID="lbMenu3" runat="server">Aprobaciones</asp:LinkButton>
                                        </a>
                                    </li>
                                    <li id="liMenu4" runat="server">
                                        <a href="#vtab4" data-toggle="tab" style="display: none;">
                                            <asp:LinkButton ID="lbMenu4" runat="server" OnClientClick="MostrarObservacionesAprobacion()">Generar PDF</asp:LinkButton>
                                        </a>
                                    </li>
                                </ul>
                            </div>

                            <div class="col-md-9">
                                <asp:Panel ID="pnlOrden" runat="server" Visible="false">
                                    <asp:HiddenField ID="hfIdWFid" runat="server" Value="0" />
                                    <div id="campo-formulario2" class="col-md-12">
                                        <div class="row">
                                            <div class="form-group">
                                                <asp:Label ID="lblAnulada" runat="server" Text="Esta orden esta anulada por lo que no podra modificarla" ForeColor="Red" Visible="false"></asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtNoOrden">No. Orden</label>
                                                <asp:TextBox ID="txtNoOrden" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtProveedor">Proveedor</label>
                                                <asp:TextBox ID="txtProveedor" CssClass="form-control" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                                <asp:Button ID="btnSearchProveedor" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarProveedores()" />
                                            </div>
                                            <div class="form-group">
                                                <label for="txtFecha">Fecha</label>
                                                <asp:TextBox ID="txtFecha" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtFechaEntrega">Fecha Entrega</label>
                                                <asp:TextBox ID="txtFechaEntrega" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group" id="divFechaSolicitud" runat="server" visible="false">
                                                <label for="txtFechaSolicitud">Fecha de Solicitud</label>
                                                <asp:TextBox ID="txtFechaSolicitud" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group" id="divSolicitado" runat="server" visible="false">
                                                <label for="txtSolicitado">Solicitado Por: </label>
                                                <asp:TextBox ID="txtSolicitado" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="ddlDepartamento">Departamento</label>
                                                <asp:DropDownList ID="ddlDepartamento" CssClass="form-control select-form" runat="server" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <label for="ddlCiudad">Ciudad</label>
                                                <asp:DropDownList ID="ddlCiudad" CssClass="form-control select-form" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtFormaPago">Forma de Pago</label>
                                                <asp:TextBox ID="txtFormaPago" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtBeneficiario">Evalua Proveedor</label>
                                                <asp:TextBox ID="txtBeneficiario" CssClass="form-control" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                                <asp:Button ID="btnBeneficiario" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarSolicitantes('EvaluaProveedor')" />
                                            </div>
                                            <div class="form-group">
                                                <label for="ddlTipo">Tipo</label>
                                                <asp:DropDownList ID="ddlTipo" CssClass="form-control select-form" runat="server" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group">
                                                <label id="lblCentroCostos" for="ddlCentroCostos" runat="server" visible="false">Centro de Costos</label>
                                                <asp:DropDownList ID="ddlCentroCostos" CssClass="form-control select-form" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                                                <asp:Button ID="btnSearchCentroCostos" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarCentrosCostos()" Visible="false" />
                                            </div>
                                            <div class="form-group">
                                                <label id="lblJBIJBE" for="txtJBIJBE" runat="server" visible="false">Código JBIJBE</label>
                                                <asp:TextBox ID="txtJBIJBE" runat="server" CssClass="form-control" Visible="false" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label id="lblNombreJBIJBE" for="txtNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                                                <asp:TextBox ID="txtNombreJBIJBE" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                            </div>
                                            <div class="form-group" id="divAprobarOrden" runat="server">
                                                <label id="lblApruebaOrden" for="ddlApruebaOrden" runat="server" visible="false">Aprueba la orden:</label>
                                                <asp:DropDownList ID="ddlApruebaOrden" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group" id="divAprobarFactura" runat="server">
                                                <label for="txtApruebaFactura">Aprueba Factura</label>
                                                <asp:TextBox ID="txtApruebaFactura" CssClass="form-control" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                                <asp:Button ID="btnApruebaFactura" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarSolicitantes('ApruebaFactura')" />
                                                <asp:DropDownList ID="ddlApruebaFactura" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div id="divAprobarFacturaCOExGerente" runat="server" visible="false">
                                                <div class="form-group">
                                                    <label id="lblApruebaFacturaCoe" for="ddlApruebaFacturaCoe" runat="server">OMP que Aprueba:</label>
                                                    <asp:DropDownList ID="ddlApruebaFacturaCoe" runat="server" CssClass="form-control select-form"></asp:DropDownList>
                                                </div>
                                                <div class="form-group">
                                                    <label id="lblApruebaFacturaGerente" for="ddlApruebaFacturaGerente" runat="server">Gerente que Aprueba:</label>
                                                    <asp:DropDownList ID="ddlApruebaFacturaGerente" runat="server" CssClass="form-control select-form"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div id="divAprobarFacturaAdmin" runat="server" visible="false">
                                                <div class="form-group">
                                                    <label id="lblApruebaFacturaAdmin" for="ddlApruebaFacturaAdmin" runat="server">Admin que Aprueba:</label>
                                                    <asp:DropDownList ID="ddlApruebaFacturaAdmin" runat="server" CssClass="form-control select-form"></asp:DropDownList>
                                                </div>
                                                <div class="form-group" id="divTipoPago" runat="server" visible="false">
                                                    <label id="txtTipoPago" for="ddlTipoPago" runat="server">Tipo de Pago:</label>
                                                    <asp:DropDownList ID="ddlTipoPago" runat="server" CssClass="form-control select-form">
                                                        <%--<asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="Producción" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Pago Parcial" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Nota Crédito" Value="3"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-12"></div>
                                            <div class="form-group" id="checkboxGenerica" runat="server">
                                                <div class="checkbox">
                                                    <div class="col-md-3">
                                                        <label class="checkbox-inline">
                                                            <asp:CheckBox ID="chbGenerica" runat="server" />Genérica
                                                        </label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <p>
                                                            (Marque el Check si es una Órden que se puede agregar a más de una Factura)
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12"></div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group" style="margin-top: 50px;">
                                                <asp:Button ID="btnSave" CssClass="col-md-3" runat="server" Text="Guardar" />
                                                <asp:Button ID="btnduplicar" runat="server" CssClass="col-md-3" Text="Duplicar" Visible="true" />
                                                <asp:Button ID="btnCancel" runat="server" CssClass="col-md-3" Text="Cancelar" />
                                                <asp:Button ID="btnAnular" runat="server" CssClass="col-md-3" Text="Anular" OnClientClick="MostrarAnularOrden();return false;" />
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group">
                                                <asp:Button ID="btnCancelEnvio" runat="server" Text="Cancelar Envío Aprobación" CssClass="col-md-3" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlBuscar" runat="server" Visible="false">
                                    <div id="campo-formulario1" class="col-md-12">
                                        <div class="row">
                                            <div class="form-group">
                                                <label for="txtOrdenSearch">No. Orden</label>
                                                <asp:TextBox ID="txtOrdenSearch" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtFechaSearch">Fecha</label>
                                                <asp:TextBox ID="txtFechaSearch" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="txtJobBookSearch">JobBook</label>
                                                <asp:TextBox ID="txtJobBookSearch" CssClass="form-control" runat="server"></asp:TextBox><br />
                                            </div>
                                            <div class="form-group">
                                                <label for="txtProveedorBusqueda">Proveedor</label>
                                                <asp:TextBox ID="txtProveedorBusqueda" CssClass="form-control" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                                <asp:Button ID="btnProveedorBusqueda" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarProveedores()" />
                                            </div>
                                            <div class="form-group">
                                                <label for="txtSolicitanteBusqueda">Solicitante</label>
                                                <asp:TextBox ID="txtSolicitanteBusqueda" CssClass="form-control" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                                <asp:Button ID="btnSolicitanteBusqueda" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarSolicitantes('BuscarSolicitante')" />
                                            </div>
                                            <div class="form-group">
                                                <label for="ddlCentroDeCostosSearch">Centro de Costos</label>
                                                <asp:DropDownList ID="ddlCentroDeCostosSearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <div class="checkbox">
                                                    <label class="checkbox-inline" style="width: 250px; margin-bottom: 20px;">
                                                        <asp:CheckBox ID="chbMisOrdenes" runat="server" />Ver solo mis órdenes
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group" style="margin: 0px auto; text-align: center; margin-bottom: 50px;">
                                                <div class="col-md-6 col-md-offset-3">
                                                    <asp:Button ID="btnSearchOk" runat="server" CssClass="col-md-3" Text="Buscar" />
                                                    <asp:Button ID="btnSearchCancel" runat="server" CssClass="col-md-3" Text="Cancelar" />
                                                </div>
                                            </div>
                                            <div style="width: 100%; max-height: 300px; overflow-y: auto; margin: 2px 5px;">
                                                <asp:GridView ID="gvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                    <SelectedRowStyle CssClass="SelectedRow" />
                                                    <AlternatingRowStyle CssClass="odd" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Id" HeaderText="No." />
                                                        <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                                        <asp:BoundField DataField="CargadoA" HeaderText="CC" />
                                                        <asp:BoundField DataField="SubTotal" HeaderText="Total" DataFormatString="{0:C0}" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                    CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAprobaciones" runat="server" Visible="false">
                                    <div id="campo-formulario4" class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <h3 class="text-left" id="lblComentarios" runat="server">Comentarios</h3>
                                                </div>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group">
                                                <asp:TextBox ID="txtComentarios" CssClass="form-control col-md-12" Height="90px" Width="97%" runat="server" placeholder="Agregue sus comentarios aquí" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                            <div class="col-md-12"></div>
                                            <div class="form-group" style="float: right;">
                                                <asp:Button ID="btnEnviarAprobacion" runat="server" Text="Enviar a Aprobación" CssClass="col-md-3" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12" style="margin: 2px;">
                                                <div style="width: 100%; max-height: 300px; overflow-y: auto;">
                                                    <asp:GridView ID="gvAprobaciones" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                        <SelectedRowStyle CssClass="SelectedRow" />
                                                        <AlternatingRowStyle CssClass="odd" />
                                                        <Columns>
                                                            <asp:CheckBoxField DataField="Aprobado" HeaderText="Aprobado" ItemStyle-CssClass="text-center transparent" />
                                                            <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha" />
                                                            <asp:BoundField DataField="Usuario" HeaderText="Aprobó" />
                                                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlDetalleOrden" runat="server" Visible="false">
                                    <div id="campo-formulario3" style="margin-top: 15px;" class="col-md-12">
                                        <asp:Panel ID="pnlDetalleOrdenes" runat="server">
                                            <asp:HiddenField ID="hfDetalleId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hfTypeFile" runat="server" />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <h3 class="text-left">Detalle de la orden</h3>
                                                    </div>
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="form-group">
                                                    <label for="txtFechaDetalle">Fecha</label>
                                                    <asp:TextBox ID="txtFechaDetalle" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="form-group">
                                                    <label for="txtDescripcionDetalle">Descripción</label>
                                                    <asp:TextBox ID="txtDescripcionDetalle" CssClass="form-control" Width="50%" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="form-group">
                                                    <label for="txtCantidad">Cantidad</label>
                                                    <asp:TextBox ID="txtCantidad" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <label for="txtValorUnitario">Valor Unitario</label>
                                                    <asp:TextBox ID="txtValorUnitario" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <label for="txtSolicitanteBusqueda">Cuenta Contable</label>
                                                    <asp:DropDownList ID="ddlCuentasContables" CssClass="form-control" runat="server" Enabled="false" Width="24%"></asp:DropDownList>
                                                    <asp:Button ID="btnCuentasContables" Text="..." runat="server" Width="35px" CssClass="btn btn-examinar" OnClientClick="MostrarCuentasContables()" />
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="form-group">
                                                    <label for="FileUpData">Cargar Archivo</label>
                                                    <asp:FileUpload ID="FileUpData" CssClass="form-control" runat="server" />
                                                    <asp:Button ID="btnLoadFile" runat="server" Text="Cargar Archivo" />
                                                    <asp:Label ID="lblFileIncorrect" runat="server" Text="Archivo Incorrecto, por favor asegurese que es un archivo excel" Visible="False"></asp:Label>
                                                </div>
                                                <div class="col-md-12"></div>
                                            </div>
                                            <div class="row">
                                                <div class="form-group" style="margin: 20px 0 90px 0; text-align: center;">
                                                    <div class="col-md-6 col-md-offset-3">
                                                        <asp:Button ID="btnAdd" CssClass="col-md-3" runat="server" Text="Agregar" />
                                                        <asp:Button ID="btnCancelDetalle" CssClass="col-md-3" runat="server" Text="Cancelar" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="form-group">
                                                    <asp:GridView ID="gvDetalle" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                        <SelectedRowStyle CssClass="SelectedRow" />
                                                        <AlternatingRowStyle CssClass="odd" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                            <asp:BoundField DataField="VrUnitario" HeaderText="Vr Unitario" DataFormatString="{0:c0}" />
                                                            <asp:BoundField DataField="NombreCuenta" HeaderText="Cuenta Contable" />
                                                            <asp:TemplateField HeaderText="Borrar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                        CommandName="Borrar" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <h3 style="float: right; padding: 10px; margin-right: 5px;" id="lblSubtotal" runat="server"></h3>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAprobacion" runat="server" Visible="false">
                                    <br />
                                </asp:Panel>
                            </div>
                            <br />
                            <br />
                            <asp:Panel ID="pnlEstimacionXTrabajo" runat="server" Visible="false">
                                <p>Estimación de tareas</p>
                                <asp:HiddenField ID="hfWfiDEstimacion" runat="server" />
                                <asp:HiddenField ID="hfTareaIdEstimacion" runat="server" />
                                <asp:GridView ID="gvEstimacion" runat="server" DataKeyNames="Id,TareaId,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                                    EmptyDataText="No se encuentran tareas para este trabajo">
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                        <asp:TemplateField HeaderText="Inicio" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFInicio" runat="server" Text='<%#  Eval("FIniP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fin" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFFin" runat="server" Text='<%#  Eval("FFinP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Observaciones" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObservacionesPlaneacion" runat="server" Text='<%#  Eval("ObservacionesPlaneacion") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No Aplica" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chbAplica" runat="server" Checked='<%# IIf(Eval("ESTADOID") = 6, True, False) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="btnGuardarEstimacion" runat="server" Text="Guardar Cambios" />
                            </asp:Panel>
                        </div>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="BusquedaProveedores" class="modalMatrix">
        <asp:UpdatePanel ID="UPanelProveedores" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row form-horizontal modaldiv">
                    <div class="col-md-5">
                        <asp:TextBox ID="txtNitProveedor" runat="server" placeholder="NIT o CC" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="txtNombreProveedor" runat="server" placeholder="Razón Social o Nombre" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnBuscarProveedor" runat="server" Text="Buscar" CssClass="btn btn-primary" />
                    </div>
                </div>
                <div class="col-md-12" style="margin: 2px;">
                    <div style="width: 100%; height: 300px; overflow-y: auto;">
                        <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Identificacion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="RazonSocial" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaSolicitantes" class="modalMatrix">
        <asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row form-horizontal modaldiv">
                    <div class="col-md-5">
                        <asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" CssClass="btn btn-primary" />
                    </div>
                </div>
                <div class="col-md-12" style="margin: 2px;">
                    <div style="width: 100%; height: 300px; overflow-y: auto;">
                        <asp:GridView ID="gvSolicitantes" runat="server" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                                <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarSolicitantes()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaJBEJBICC">
        <asp:UpdatePanel ID="upJBEJBICC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Valor busqueda"></asp:TextBox>
                <asp:Button ID="btnBuscarJBEJBICC" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvJBEJBICC" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarJBEJBICC()" />
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
    <div id="BusquedaCuentasContables" class="modalMatrix">
        <asp:UpdatePanel ID="upCuentasContables" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row form-horizontal modaldiv">
                    <div class="col-md-5">
                        <asp:TextBox ID="txtNumeroCuenta" runat="server" placeholder="Número cuenta" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="txtDescripcion" runat="server" placeholder="Descripción" BorderWidth="1px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnBuscarCuentaContable" runat="server" Text="Buscar" CssClass="btn btn-primary" />
                    </div>
                </div>
                <div class="col-md-12" style="margin: 2px;">
                    <div style="width: 100%; height: 300px; overflow-y: auto;">
                        <asp:GridView ID="gvCuentasContables" runat="server" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarCuentasContables()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaObservacionesAprobacion">
        <asp:UpdatePanel ID="upObservacionesAprobacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="margin: 30px 2px 2px; width: 95%;">
                    <asp:Panel runat="server" ID="pnlObservacionesAprobacion">
                        <div style="width: 100%; max-height: 300px; min-height: 100px; overflow-y: auto;">
                            <asp:GridView ID="gvObservacionesAprobacion" runat="server" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id" AllowPaging="False" EmptyDataText="No hay ninguna Observación registrada para esta Orden">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="IdOrden" HeaderText="Id Orden" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-md-11" style="margin: 0px auto;">
                    <div class="col-md-3">
                        <asp:Button ID="btnGenerarPDF" CssClass="btn btn-primary" runat="server" Text="Generar PDF Orden" OnClientClick="CerrarObservacionesAprobacion()" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnNotaCredito" CssClass="btn btn-primary" runat="server" Text="Nota Crédito" OnClientClick="CerrarObservacionesAprobacion()" />
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnDocEquivalente" CssClass="btn btn-primary" runat="server" Text="Documento Equivalente" OnClientClick="CerrarObservacionesAprobacion()" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="anularOrden">
        <asp:UpdatePanel ID="upAnularOrden" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="form-group">
                    <asp:Label Text="Observación de Anulación" ID="lblObserAnulacion" runat="server" />
                    <br />
                    <asp:TextBox ID="txtObservacionAnulacion" runat="server" placeholder="Observación de anulación" Width="100%" TextMode="MultiLine" Height="150px" BorderColor="#13b0a8" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                    <br />
                    <div class="col-md-1 col-md-offset-10">
                        <asp:Button ID="btnAnularOrden" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClientClick="CerrarAnularOrden()" />
                    </div>
                </div>
                <div class="actions"></div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#CPH_Content_txtCedulaSolicitante").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
            $("#CPH_Content_txtNombreSolicitante").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });
    </script>
</asp:Content>
