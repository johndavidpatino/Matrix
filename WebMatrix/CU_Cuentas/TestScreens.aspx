<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestScreens.aspx.vb" Inherits="WebMatrix.TestScreens" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/AppUsersControls/UC_Header_Presupuesto.ascx" TagName="HeaderPresupuesto" TagPrefix="uch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Language" content="es"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Matrix - Ipsos Colombia</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no, shrink-to-fit=no" />
    <meta name="description" content="Matrix - Ipsos Colombia"/>
    <meta name="msapplication-tap-highlight" content="no"/>
    <link href="../css/main.css" rel="stylesheet" />
<script type="text/javascript">
        function TotalDias() {

            document.getElementById('<%= txtDiasTotal.ClientID %>').value = Number(document.getElementById('<%= txtDiasDiseno.ClientID %>').value) + Number(document.getElementById('<%= txtDiasCampo.ClientID %>').value) + Number(document.getElementById('<%= txtDiasProceso.ClientID %>').value) + Number(document.getElementById('<%= txtDiasInformes.ClientID %>').value);

        }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="app-container app-theme-white body-tabs-shadow fixed-sidebar fixed-header">
            <div class="app-header header-shadow">
            <div class="app-header__logo">
                <div class="logo-src"></div>
                <div class="header__pane ml-auto">
                    <div>
                        <button type="button" class="hamburger close-sidebar-btn hamburger--elastic" data-class="closed-sidebar">
                            <span class="hamburger-box">
                                <span class="hamburger-inner"></span>
                            </span>
                        </button>
                    </div>
                </div>
            </div>
            <div class="app-header__mobile-menu">
                <div>
                    <button type="button" class="hamburger hamburger--elastic mobile-toggle-nav">
                        <span class="hamburger-box">
                            <span class="hamburger-inner"></span>
                        </span>
                    </button>
                </div>
            </div>
            <div class="app-header__menu">
                <span>
                    <button type="button" class="btn-icon btn-icon-only btn btn-primary btn-sm mobile-toggle-header-nav">
                        <span class="btn-icon-wrapper">
                            <i class="fa fa-ellipsis-v fa-w-6"></i>
                        </span>
                    </button>
                </span>
            </div>
            <div class="app-header__content">
                <div class="app-header-left">
                    <ul class="header-menu nav">
                        <li class="nav-item">
                            <a href="../NewDesign/TempoNewHome.aspx" class="nav-link">
                                <i class="nav-link-icon fa fa-home"> </i>
                                Inicio
                            </a>
                        </li>
                        <li class="nav-item" style="margin-left:50px; background-color:#2F469C; font-size:1rem;">
                            <a class="nav-link" style="color:white;"></a>
                        </li>
                        
                    </ul>
                </div>
                <div class="app-header-right">
                    <div class="header-btn-lg pr-0">
                        <div class="widget-content p-0">
                            <div class="widget-content-wrapper">
                                <div class="widget-content-left">
                                    <div class="btn-group">
                                        <a data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" class="p-0 btn">
                                            <img width="42" runat="server" id="imgEmployee" class="rounded-circle" src="../Images/sin-foto.jpg" alt="">
                                            <i class="fa fa-angle-down ml-2 opacity-8"></i>
                                        </a>
                                        <div tabindex="-1" role="menu" aria-hidden="true" class="dropdown-menu dropdown-menu-right">
                                            <a href="../US_Usuarios/CambioContrasena.aspx" class="dropdown-item">Cambiar Password</a>
                                            <a href="../TH_TalentoHumano/EmpleadoUpdate.aspx" class="dropdown-item">Actualizar mis datos</a>
                                            <h6 tabindex="-1" class="dropdown-header">---------</h6>
                                            <a href="../TH_TalentoHumano/SolicitudAusencia.aspx" class="dropdown-item">Vacaciones / Ausencias</a>
                                            <a href="../FI_AdministrativoFinanciero/Gestion-Ordenes-Aprobacion.aspx" class="dropdown-item">Aprobar Órdenes</a>
                                            <a href="../FI_AdministrativoFinanciero/Aprobacion-Evaluacion-Facturas.aspx" class="dropdown-item">Aprobar Facturas</a>
                                            <a href="../FI_AdministrativoFinanciero/Evaluacion-Proveedor-Facturas.aspx" class="dropdown-item">Evaluar Proveedores</a>
                                            <a href="../CORE/Gestion-Tareas-Trabajos.aspx" class="dropdown-item">Tareas y asignaciones</a>
                                            <div tabindex="-1" class="dropdown-divider"></div>
                                            <a style="font-size:14px" title="Salir del sistema de forma segura" class="dropdown-item" href="#">
				  <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="dropdown-item" 
					  LogoutText="Cerrar mi sesión" Font-Size="14px" 
					  LogoutAction="RedirectToLoginPage"  /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="widget-content-left  ml-3 header-user-info">
                                    <div class="widget-heading">
                                        <asp:Label ID="lblNameEmployee" runat="server"></asp:Label>
                                    </div>
                                    <div class="widget-subheading">
                                        <asp:Label ID="lblPositionEmployee" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="widget-content-right header-user-info ml-3">
                                    <button type="button" class="btn-shadow p-1 btn btn-primary btn-sm show-toastr-example">
                                        <i class="fa text-white fa-calendar pr-1 pl-1"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            <div class="app-main">
            <div class="app-sidebar sidebar-shadow">
                <div class="app-header__logo">
                    <div class="logo-src"></div>
                    <div class="header__pane ml-auto">
                        <div>
                            <button type="button" class="hamburger close-sidebar-btn hamburger--elastic" data-class="closed-sidebar">
                                <span class="hamburger-box">
                                    <span class="hamburger-inner"></span>
                                </span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="app-header__mobile-menu">
                    <div>
                        <button type="button" class="hamburger hamburger--elastic mobile-toggle-nav">
                            <span class="hamburger-box">
                                <span class="hamburger-inner"></span>
                            </span>
                        </button>
                    </div>
                </div>
                <div class="app-header__menu">
                    <span>
                        <button type="button" class="btn-icon btn-icon-only btn btn-primary btn-sm mobile-toggle-header-nav">
                            <span class="btn-icon-wrapper">
                                <i class="fa fa-ellipsis-v fa-w-6"></i>
                            </span>
                        </button>
                    </span>
                </div>    <div class="scrollbar-sidebar">
                    <div class="app-sidebar__inner">
                        <ul class="vertical-nav-menu">
                        
                            </ul>
                        
                    </div>
                </div>
            </div>    
            <div class="app-main__outer">
                <div class="app-main__inner">
                    <div class="app-page-title">
                        <div class="page-title-wrapper">
                            <div class="page-title-heading">
                                <div>
                                    
                                    <div class="page-title-subheading">
                                    
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                    

                    <div id="contenido">
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <div class="form-row">
                <div class="input-group col-md-7 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Descripción de la alternativa</button>
                    </div>
                    <asp:TextBox ID="txtDescripcion" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">No. mediciones</button>
                    </div>
                    <asp:TextBox ID="txtNoMediciones" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Periodicidad meses</button>
                    </div>
                    <asp:TextBox ID="txtPeriodicidad" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                </div>
                <div class="input-group col-md-1 mb-3">
                    <asp:CheckBox ID="chbObserver" Text="Observer" runat="server" CssClass="form-check-inline" />
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Días hábiles estimados</span>
                    </div>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Diseño</button>
                    </div>
                    <asp:TextBox ID="txtDiasDiseno" CssClass="form-control" TextMode="Number" runat="server" onchange="Javascript:TotalDias();">0</asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Campo</button>
                    </div>
                    <asp:TextBox ID="txtDiasCampo" CssClass="form-control" TextMode="Number" runat="server" onchange="Javascript:TotalDias();">0</asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3" data-tooltip="Esta es la fecha en que se aprobó o se canceló la propuesta">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Proceso</button>
                    </div>
                    <asp:TextBox ID="txtDiasProceso" CssClass="form-control" TextMode="Number" runat="server" onchange="Javascript:TotalDias();">0</asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Informes</button>
                    </div>
                    <asp:TextBox ID="txtDiasInformes" CssClass="form-control" TextMode="Number" runat="server" onchange="Javascript:TotalDias();">0</asp:TextBox>
                </div>
                <div class="input-group col-md-2 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Total</button>
                    </div>
                    <asp:TextBox ID="txtDiasTotal" CssClass="form-control" ReadOnly="true" TextMode="Number" runat="server"></asp:TextBox>
                </div>
                <div class="input-group col-md-12 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Observaciones</button>
                    </div>
                    <asp:TextBox ID="txtObservaciones" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
                <asp:Button runat="server" ID="btnNew" class="btn btn-primary" Text="Crear Nuevo"></asp:Button>
            </div>
        </div>
    </div>
    <div>
        <ul class="body-tabs body-tabs-layout tabs-animated body-tabs-animated nav">
            <li class="nav-item">
                <a role="tab" class="nav-link active" id="tab-0" data-toggle="tab" href="#tab-content-0">
                    <span>CARA A CARA</span>
                </a>
            </li>
            <li class="nav-item">
                <a role="tab" class="nav-link" id="tab-1" data-toggle="tab" href="#tab-content-1">
                    <span>CATI</span>
                </a>
            </li>
            <li class="nav-item">
                <a role="tab" class="nav-link" id="tab-2" data-toggle="tab" href="#tab-content-2">
                    <span>ONLINE</span>
                </a>
            </li>
            <li class="nav-item">
                <a role="tab" class="nav-link" id="tab-3" data-toggle="tab" href="#tab-content-3">
                    <span>SESIONES DE GRUPO</span>
                </a>
            </li>
            <li class="nav-item">
                <a role="tab" class="nav-link" id="tab-4" data-toggle="tab" href="#tab-content-4">
                    <span>ENTREVISTAS</span>
                </a>
            </li>
        </ul>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="tab-content">
                    <div class="tab-pane tabs-animation fade show active" id="tab-content-0" role="tabpanel">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="main-card mb-3 card">
                                    <div class="card-header">
                                        Presupuestos F2F
                                               
                                <div class="btn-actions-pane-right">
                                    <div class="nav">

                                        <asp:Button ID="btnAddF2F" runat="server" class="btn-pill btn-wide active btn btn-outline-alternate btn-sm" Text="Agregar presupuesto"></asp:Button>

                                        <asp:ModalPopupExtender ID="lkb1_ModalPopupExtender" runat="server"
                                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancel"
                                            DropShadow="True" Enabled="True"
                                            PopupControlID="pnlPresupuestoModal" TargetControlID="btnAddF2F">
                                        </asp:ModalPopupExtender>

                                    </div>
                                </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="form-row">
                                            <p>Listado de presupuestos</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane tabs-animation fade" id="tab-content-1" role="tabpanel">
                        <div class="row">
                            <p>Cati</p>
                        </div>
                    </div>
                    <div class="tab-pane tabs-animation fade" id="tab-content-2" role="tabpanel">
                        <div class="row">
                            <p>Online</p>
                        </div>
                    </div>
                    <div class="tab-pane tabs-animation fade" id="tab-content-3" role="tabpanel">
                        <div class="row">
                            <p>Sesiones</p>
                        </div>
                    </div>
                    <div class="tab-pane tabs-animation fade" id="tab-content-4" role="tabpanel">
                        <div class="row">
                            <p>Entrevistas</p>
                        </div>
                    </div>

                </div>
                <asp:Panel ID="pnlPresupuestoModal" runat="server">
                    <div class="bd-example-modal-lg show" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg show">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <uch:HeaderPresupuesto id="UCHeader" runat="server" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnCancel" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                                    <button type="button" class="btn btn-primary">Save changes</button>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
                    </div>



                </div>
                <div class="app-wrapper-footer">
                    <div class="app-footer">
                        <div class="app-footer__inner">
                            <div class="app-footer-left">
                                <ul class="nav">
                                    <li class="nav-item">
                                        <a href="../PNC/PNC_Productos.aspx" class="nav-link">
                                            Producto No Conforme
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="../US_Usuarios/Feedback.aspx" class="nav-link">
                                            Retroalimentar
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="../TH_TalentoHumano/HWH-Admin.aspx" class="nav-link">
                                            EasyWork Manager
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="../TH_TalentoHumano/HWH-RH.aspx" class="nav-link">
                                            EasyWork TH
                                        </a>
                                    </li>
                                </ul>
                            </div>
                            <div class="app-footer-right">
                                <ul class="nav">
                                    <li class="nav-item">
                                        <a href="http://symphony.ipsos.com/" target="_blank" class="nav-link">
                                            Symphony
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="http://nwb.ipsos.com/" target="_blank" class="nav-link">
                                            iQuote
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="javascript:void(0);" class="nav-link">
                                            <div class="badge badge-success mr-1 ml-0">
                                                <small>NEW</small>
                                            </div>
                                            EasyPD
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
            
        </div>
    </form>
    <script type="text/javascript" src="../assets/scripts/main.js"></script>
</body>
</html>
