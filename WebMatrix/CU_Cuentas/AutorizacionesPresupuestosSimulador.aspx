<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="AutorizacionesPresupuestosSimulador.aspx.vb" Inherits="WebMatrix._AutorizacionesPresupuestosSimulador" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/theme.light.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= txtFInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
            $("#<%= txtFFin.ClientId %>").mask("99/99/9999");
            $("#<%= txtFFin.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
        });
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Autorizaciones Presupuestos</li>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Búsqueda de solicitudes para modificación de presupuestos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Búsqueda</h5>
            <p class="card-subtitle">Diligencie los campos por los cuales desea buscar</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Estado</button>
                        </div>
                        <asp:RadioButtonList ID="rbSearch" runat="server" CssClass="form-check-inline form-control form-control-sm" CellPadding="10" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="Pendientes" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Aprobados"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Rechazados"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="input-group col-md-2 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">No. Propuesta</button>
                        </div>
                        <asp:TextBox ID="txtPropuesta" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-2 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Fecha Inicio Solicitud</button>
                        </div>
                        <asp:TextBox ID="txtFInicio" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-2 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Fecha Fin Solicitud</button>
                        </div>
                        <asp:TextBox ID="txtFFin" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Unidad</button>
                        </div>
                        <asp:DropDownList ID="ddlSL" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Text="Ver todas"></asp:ListItem>
                            <asp:ListItem Value="2" Text="MSU - Market Strategic & Understanding- AG"></asp:ListItem>
                            <asp:ListItem Value="3" Text="CEX"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Public Affairs"></asp:ListItem>
                            <asp:ListItem Value="6" Text="IU"></asp:ListItem>
                            <asp:ListItem Value="26" Text="OPS"></asp:ListItem>
                            <asp:ListItem Value="29" Text="BHT"></asp:ListItem>
                            <asp:ListItem Value="30" Text="Innovation"></asp:ListItem>
                            <asp:ListItem Value="31" Text="Creative Excellence"></asp:ListItem>
                            <asp:ListItem Value="32" Text="Mystery Shopping"></asp:ListItem>
                            <asp:ListItem Value="33" Text="HealthCare"></asp:ListItem>
                            <asp:ListItem Value="34" Text="Corporate REputation"></asp:ListItem>
                            <asp:ListItem Value="35" Text="Observer"></asp:ListItem>
                            <asp:ListItem Value="36" Text="Strategy 3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>  
                </div>
                <asp:Button runat="server" ID="btnSearch" class="btn btn-primary" Text="Buscar" OnClick="btnSearch_Click"></asp:Button>
            </div>

        </div>
    </div>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Resultados</h5>
            <p class="card-subtitle">Resultados de búsqueda</p>
            <div>
                <asp:UpdatePanel ID="UPanelSearch" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbModals" runat="server"></asp:LinkButton>
                        <asp:Label ID="lblShowGMSimulator" runat="server"></asp:Label>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderSimulator" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelSimulator"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelSimulator"
                            PopupControlID="pnlGMSimulator" TargetControlID="lblShowGMSimulator">
                        </asp:ModalPopupExtender>
                        <asp:GridView ID="gvDataSearch" runat="server" AutoGenerateColumns="false" DataKeyNames="ID" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda" OnRowCommand="gvDataSearch_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="OpenRequest" ToolTip="Abrir este JobBook"><i class="metismenu-icon fa fa-folder-open"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Propuesta" HeaderText="Propuesta" />
                                <asp:BoundField DataField="Alternativa" HeaderText="Alt" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                <asp:BoundField DataField="NombrePropuesta" HeaderText="Nombre" />
                                <asp:BoundField DataField="SL" HeaderText="SL" HeaderStyle-Wrap="true" />
                                <asp:BoundField DataField="SolicitadoPor" HeaderText="Cuentas" />
                                <asp:BoundField DataField="SolicitadoPor" HeaderText="Gerente Cuentas" HeaderStyle-Wrap="true" />
                                <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    <asp:Panel ID="pnlGMSimulator" runat="server">
                        <asp:UpdatePanel ID="UPanelSimulator" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfIdSolicitud" runat="server" />
                                <asp:HiddenField ID="hfSimPropuesta" runat="server" />
                                <asp:HiddenField ID="hfSimAlternativa" runat="server" />
                                <asp:HiddenField ID="hfSimMetodologia" runat="server" />
                                <asp:HiddenField ID="hfSimFase" runat="server" />
                                <asp:HiddenField ID="hfGMMin" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myBasicModalLabelGMSimulator" aria-hidden="true">
                                    <div class="modal-dialog modal-xl show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Valor Venta</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-success">
                                                                                            <asp:Label ID="lblSIMVrVenta" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>


                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Otros Costos</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-secondary">
                                                                                            <asp:Label ID="lblSIMOtrosCostos" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">GM %</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-success">
                                                                                            <asp:Label ID="lblSIMGM" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Cargos del grupo</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-secondary">
                                                                                            <asp:Label ID="lblSIMCargosGrupo" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Costo OPS</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-primary">
                                                                                            <asp:Label ID="lblSIMCostoOPS" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Total Costos Directos</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-info">
                                                                                            <asp:Label ID="lblSIMCostosDirectos" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Target Professional Time</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-primary">
                                                                                            <asp:Label ID="lblSIMTargetProfessionalTime" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>


                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">OP</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-info">
                                                                                            <asp:Label ID="lblSIMOP" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">% GM OPS</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-warning">
                                                                                            <asp:Label ID="lblSIMGMOPS" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Margen Bruto</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-alt">
                                                                                            <asp:Label ID="lblSIMMargenBruto" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>

                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Professional Time</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-warning">
                                                                                            <asp:Label ID="lblSIMProfessionalTime" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>

                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">OP %</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-danger">
                                                                                            <asp:Label ID="lblSIMOPPercent" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Días de campo</button>
                                                        </div>
                                                        <asp:TextBox ID="txtDiasCampo" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Días de diseño</button>
                                                        </div>
                                                        <asp:TextBox ID="txtDiasDiseno" ReadOnly="true" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Días de Procesamiento</button>
                                                        </div>
                                                        <asp:TextBox ID="txtDiasProcesamiento" ReadOnly="true" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Días en informes</button>
                                                        </div>
                                                        <asp:TextBox ID="txtDiasInformes" runat="server" ReadOnly="true" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="input-group col-md-12 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Observaciones</button>
                                                        </div>
                                                        <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                    </div>

                                                </div>
                                                <div style="width: 100%; margin: 0 auto; text-align: center">
                                                    <asp:TextBox CssClass="form-text" ID="txtComentariosAprobacion" placeholder="Si desea opcionalmente puede agregar comentarios en este espacio" runat="server" Width="100%"></asp:TextBox>
                                                    <asp:Button runat="server" ID="btnAprobarSolicitud" OnClick="btnAprobarSolicitud_Click" class="btn btn-success" Text="Aprobar la solicitud"></asp:Button>
                                                    <asp:Button runat="server" ID="btnRechazarSolicitud" OnClick="btnRechazarSolicitud_Click" class="btn btn-danger" Text="Rechazar la solicitud"></asp:Button>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelSimulator" type="button" class="btn btn-secondary" runat="server" Text="Cerrar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCancelSimulator" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
</asp:Content>
