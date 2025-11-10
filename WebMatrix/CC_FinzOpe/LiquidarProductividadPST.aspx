<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="LiquidarProductividadPST.aspx.vb" Inherits="WebMatrix.LiquidarProductividadPST" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="../FI_AdministrativoFinanciero/Default.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Volver al menú
        </a>
    </li>
    <li>
        <a href="../CC_FinzOpe/LiquidarPlanillasActividades.aspx" class="nav-link text-primary">
            <i class="metismenu-icon fa fa-table"></i>
            Liquidar Planillas
        </a>
    </li>
    <li>
        <a href="../CC_FinzOpe/LiquidarProductividadPST.aspx" class="nav-link text-success">
            <i class="metismenu-icon fa fa-table"></i>
            Liquidar Productividad
        </a>
    </li>
    <li>
        <a href="../Home/Home.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-home"></i>
            Ir a inicio
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Liquidación Productividad PST
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    Liquidación para el corte del <strong>
        <asp:Label ID="lblIni" runat="server"></asp:Label>
        al
        <asp:Label ID="lblFin" runat="server"></asp:Label></strong>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Content" runat="server">
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
    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
    <asp:HiddenField ID="hfidMetodologia" runat="server" />
    <asp:HiddenField ID="hfNew" runat="server" />
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Estatus trabajos y presupuestos</h5>
            <p class="card-subtitle">Estado de los trabajos y sus presupuestos para el corte actual</p>
            <div>
                <asp:GridView ID="gvEstatus" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No tiene trabajos en este corte">
                    <Columns>
                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                        <asp:BoundField DataField="VrUnitario" HeaderText="Vr. Encuesta" DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="Cargo" HeaderText="Actividad" />
                        <asp:BoundField DataField="StatusPresupuesto" HeaderText="Presupuesto" />
                        <asp:BoundField DataField="AprobadoCampo" HeaderText="Aprobado Campo" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="AprobadoPMO" HeaderText="Aprobado PMO" HeaderStyle-Wrap="true" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResultados" runat="server">
        <div class="main-card mb-3 card">
            <div class="card-body">
                <h5 class="card-title">Resumen Producción Total Corte Actual</h5>
                <p class="card-subtitle">Este es el valor liquidado de producción y planillas para el corte actual</p>
                <div>
                    <asp:GridView ID="gvResumenProduccion" runat="server" AutoGenerateColumns="false" DataKeyNames="Cedula" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                        <Columns>
                            <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="CargoMatrix" HeaderText="Cargo Matrix" ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="IdIStaff" HeaderText="IdSymphony" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <asp:BoundField DataField="DiasTrabajados" HeaderText="DiasTrabajados" />
                            <asp:BoundField DataField="VrTransporte" HeaderText="Vr Transporte" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="VrProduccion" HeaderText="Vr Producción" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="VrBono" HeaderText="Vr Bono" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="ValorSS" HeaderText="Vr SS" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="SaldoSS" HeaderText="Saldo SS" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="ValorICA" HeaderText="ICA" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="TotalDescuento" HeaderText="Sub. Descuentos" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="TotalAPagar" HeaderText="A Pagar" DataFormatString="{0:C0}" Visible="false" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnDownloadResumen" CssClass="btn btn-secondary" Text="Descargar a Excel" OnClick="btnDownloadResumen_Click"></asp:Button>
                </div>
            </div>
        </div>
        <div class="main-card mb-3 card">
            <div class="card-body">
                <h5 class="card-title">Cargar Descuentos SS</h5>
                <p class="card-subtitle">Estas son las planillas que tienen pendiente una revisión por parte del PMO</p>
                <div>
                    <div class="card-body">
                        <h5 class="card-title">Importar descuentos de seguridad social desde Excel</h5>
                        <div class="form-inline">
                            <label>Descargue el formato en Excel desde este icono para cargar los datos. No cambie la estructura ni inserte o elimine filas</label>
                            <a style="margin-left: 10px;" href="../Files/FormatoCargueDescuentosSS_V2.xlsx" target="_blank">
                                <img src="../Images/excel.jpg" alt="Excel de importar" style="width: 36px; height: 36px" /></a>
                        </div>
                        <div>
                            <div class="form-inline">
                                <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                    <label for="FUploadExcelMuestra" class="mr-sm-2">Seleccione el archivo a subir</label>
                                    <asp:FileUpload ID="FUploadExcel" runat="server" CssClass="form-control-min-select form-control" />
                                </div>
                                <br />
                                <div class="form-inline">
                                    <asp:Button ID="btnLoadDataExcel" runat="server" Text="Importar la información" CssClass="btn btn-primary" OnClick="btnLoadDataExcel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="main-card mb-3 card">
            <div class="card-body">
                <h5 class="card-title">Opciones Avanzadas</h5>
                <p class="card-subtitle">Estas son las planillas que tienen pendiente una revisión por parte del PMO</p>
                <div>
                    <asp:GridView ID="gvDataToLiquidar" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                        <Columns>
                            <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" Visible="false" />
                            <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" Visible="false" />
                            <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" />
                            <asp:BoundField DataField="PeriodoInicial" HeaderText="Periodo Inicial" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="PeriodoFinal" HeaderText="Periodo Final" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="Registros" HeaderText="Registros" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnLiquidar" class="btn btn-primary" Text="Actualizar Registros Sincronizados" OnClick="btnLiquidar_Click" OnClientClick="return confirm('Confirma que va a actualizar los registros?')"></asp:Button>
                    <asp:Button runat="server" ID="btnPasarProduccion" CssClass="btn btn-warning" Text="Pasar registros a producción" OnClick="btnPasarProduccion_Click" OnClientClick="return confirm('Confirmas que vas a liquidar estos registros y pasarlos a producción?')"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
