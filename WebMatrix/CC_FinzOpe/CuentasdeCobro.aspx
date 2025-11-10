<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="CuentasdeCobro.aspx.vb" Inherits="WebMatrix.CuentasdeCobro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="../Styles/Newformulario.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFIni);
            bindPickerFIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFFin);
            bindPickerFFin();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFFact);
            bindPickerFFact();

            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFactura.ClientID%>").mask("99/99/9999");

        });

        function bindPickerFIni() {
            $("input[type=text][id*=txtFechaInicio]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFFin() {
            $("input[type=text][id*=txtFechaFinalizacion]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFFact() {
            $("input[type=text][id*=txtFechaFactura]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Gestión Liquidación PST
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Generación cuentas de cobro
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../FI_AdministrativoFinanciero/Default.aspx">FINANZAS OPERACIONES</a>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">

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
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="10000" Enabled="false" OnTick="Timer1_Tick">
            </asp:Timer>
            <div class="col-md-12">
                <div class="panel panel-primary" id="pnlTitulo" runat="server" style="border: none;">
                    <div class="panel-heading text-left">
                        <asp:Label ID="txtTitulo" runat="server" Text="Generar ordenes" CssClass="panel-title"></asp:Label>
                    </div>
                    <div class="panel-body" style="background-image: url(../images/fnd-section.png); background-position: right; background-repeat: no-repeat;">
                        <div class="col-md-2" style="margin: 0 -10px;">
                            <ul id="nav-tabs-wrapper" style="list-style:none" class="nav nav-tabs nav-pills well">
                                <li>
                                    <a>Fecha Inicial</a>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle required" Width="100px"></asp:TextBox>
                                </li>
                                <li>
                                    <a>Fecha Final
                                    </a>
                                    <asp:TextBox ID="txtFechaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle required" Width="100px"></asp:TextBox>
                                </li>

                                <li>
                                    <a>Fecha Factura
                                    </a>
                                    <asp:TextBox ID="txtFechaFactura" runat="server" CssClass="bgCalendar textCalendarStyle required" Width="100px"></asp:TextBox>
                                </li>
                                <li>
                                        <asp:Button ID="btnCargarFiltros" runat="server" Text="Cargar filtros" Width="100px" CssClass="causesValidation" />
                                </li>
                                </ul>
                        </div>

                        <div class="col-md-9">
                            <div id="campo-formulario1" class="col-md-12">
                                <div class="row">


                                    <div class="col-md-12"></div>
                                    <div class="form-group">
                                        <label>
                                            Ciudades
                                        </label>
                                        <asp:DropDownList ID="ddlciudades" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        <label>
                                            Tipos de encuestador
                                        </label>
                                        <asp:DropDownList ID="ddlTiposEncuestador" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            Cedulas
                                        </label>
                                        <asp:DropDownList ID="ddlCedulas" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-12"></div>
                                    <div class="form-group" style="margin: 0px auto; text-align: center; margin-bottom: 50px;">
                                        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="causesValidation" />
                                        <asp:Button ID="btnCuentas" runat="server" Text="Generar PDF cuentas de cobro" CssClass="causesValidation" />
                                    </div>

                                </div>
                                <div class="row">
                                    <div>
                                        <asp:Label ID="lblTxtCantidadOrdenesGenerar" runat="server" Text="Ordenes a generar:"></asp:Label>
                                        <asp:Label ID="lblCantidadOrdenesGenerar" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblTxtProgreso" runat="server" Text="Ordenes generadas:"></asp:Label>
                                        <asp:Label ID="lblProgreso" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblTxtEstadp" runat="server" Text="Estado:"></asp:Label>
                                        <asp:Label ID="lblEstadp" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblTxtMensaje" runat="server" Text="Mensaje:"></asp:Label>
                                        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblTxtGTotalProduccion" runat="server" Text="TotalProduccion:"></asp:Label>
                                        <asp:Label ID="lblGTotalProduccion" runat="server"></asp:Label>
                                    </div>
                                    <asp:Label ID="lblMensajeActualizacion" runat="server" Text="Esta pagina se actualiza cada 10 segundos para mostrar el avance del proceso" ForeColor="Red"></asp:Label>
                                    <br />
                                    <asp:PlaceHolder ID="phArchivos" runat="server"></asp:PlaceHolder>
                                    <fieldset class="validationGroup">
                                        <div>
                                            <asp:GridView ID="gvProduccion" runat="server" Width="100%" AutoGenerateColumns="False"
                                                PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                DataKeyNames="PersonaId" EmptyDataText="No existen registros para mostrar">
                                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                <SelectedRowStyle CssClass="SelectedRow" />
                                                <AlternatingRowStyle CssClass="odd" />
                                                <Columns>
                                                    <asp:BoundField DataField="PersonaId" HeaderText="PersonaId" />
                                                    <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                                    <asp:BoundField DataField="Cantida" HeaderText="Cantida" />
                                                    <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                                    <asp:BoundField DataField="CiudadId" HeaderText="CiudadId" />
                                                    <asp:BoundField DataField="DiasTrabajados" HeaderText="DiasTrabajados" />
                                                    <asp:BoundField DataField="DescuentoSS" HeaderText="Otros" />
                                                    <asp:BoundField DataField="ValorICA" HeaderText="ValorICA" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
