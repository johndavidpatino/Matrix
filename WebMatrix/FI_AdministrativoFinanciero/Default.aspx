<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._DefaultFI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link rel="stylesheet" href="../Styles/slider.css" media="screen" />
	<script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
	<div class="prev"><a href="#anterior" title="Anterior"></a></div>
	<div id="slider">
		<div class="slidesContainer">
			<nav class="slide">

				<div class="menu-element1">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Control Presupuestos</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="Control Costos" href="../CAP/ControlPresupuestos.aspx">Control Costos</a></li>
								<li><a title="Listado Estudios" href="ListadoEstudios.aspx">Listado Estudios</a></li>
								<li><a title="Listado Propuestas" href="ListadoPropuestas.aspx">Listado Propuestas</a></li>
								<li><a title="NominaDistribucionCostos" href="NominaDistribucionCostos.aspx">Nomina Distribucion Costos</a></li>

								<li><a>&nbsp;</a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element1-->

				<div class="menu-element2">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Presupuestos Internos</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/ejecutivo.png" width="65" height="65" alt="polea" /></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="Opcion 1" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="Requerimientos" href="../CC_FinzOpe/GenerarRequerimientos.aspx">Requerimientos</a></li>
								<li><a title="Presupuestos" href="../CC_FinzOpe/PresupuestosInternosIndex.aspx">Presupuestos</a></li>
								<li><a title="Descargar Trabajos" href="../CC_FinzOpe/ListadoTrabajos.aspx">Descargar Trabajos</a></li>
								<li><a title="LogPersonas" href="../TH_TalentoHumano/ConsultaLog.aspx">LogPersonas</a></li>
								<li><a></a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element2-->

				<div class="menu-element3">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Procesos Internos</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="Opcion 1" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="Conteos" href="../CC_FinzOpe/ConteoTrabajos.aspx">Conteos</a></li>
								<li><a title="Reporte Conteos" href="../CC_FinzOpe/ReporteConteoTrabajos.aspx">Reporte Conteos</a></li>
								<li><a title="Resumen de Produccion" href="../CC_FinzOpe/ResumenesdeProduccion.aspx">ResumenProductividad</a></li>
								<li><a title="RequerimientoDeServicio" href="../CC_FinzOpe/OrdenesdeServicio.aspx">RequerimientoDeServicio</a></li>
								<li><a title="Contratistas" href="../TH_TalentoHumano/Contratistas.aspx">Contratistas</a></li>
								<li><a title="Módulo de Contratación" href="Contratacion.aspx">Contratistas</a></li>
								<li><a title="PST-Contratistas" href="PrestacionServicios-CT.aspx">PST-Contratistas</a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element3-->

				<div class="menu-element4">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Reportes</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/polea.png" width="65" height="65" alt="polea"></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="Opcion 1" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="RadicarCuentas" href="../CC_FinzOpe/RecepcionCuentasdeCobro.aspx">RadicarCuentas</a></li>
								<li><a title="AprobarCuentas" href="../CC_FinzOpe/ListadoCuentasRecibidas.aspx">AprobarCuentas</a></li>
								<li><a title="ReportePagos" href="../CC_FinzOpe/ReportePagos.aspx">ReportePagos</a></li>
								<li><a title="ReporteProduccion" href="../CC_FinzOpe/ReporteActividadesProduccion.aspx">ReporteProduccion</a></li>
								<li><a title="ReporteOrdenes" href="../CC_FinzOpe/ReporteOrdenesdeServicio.aspx">ReporteOrdenes</a></li>
								<li><a title="ReporteContabilizacionPST" href="../CC_FinzOpe/ReporteContabilizacionPST.aspx">ReporteContabilizacionPST</a></li>
								<li><a title="ReporteLegalizaciones" href="../Inventario/ReporteLegalizaciones.aspx">Reporte Legalizaciones</a></li>
								<li><a></a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element4-->

				<div class="menu-element5">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Producción</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/mundo.png" width="65" height="65" alt="mundo" /></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="Opcion 1" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="Produccion" href="../CC_FinzOpe/Produccion.aspx">Produccion</a></li>
								<li><a title="Eliminar cargue" href="../CC_FinzOpe/EliminarCargueProduccion.aspx">Eliminar cargue</a></li>
								<li><a title="Liquidacion Bono" href="../CC_FinzOpe/GenerarBonificacion.aspx">Liquidacion Bono</a></li>
								<li><a title="Descargar producción por IDs" href="../CC_FinzOpe/ExportarProduccionIDs.aspx">Descargar Producción</a></li>
								<li><a title="Modificar estados jobbooks" href="../CC_FinzOpe/EstadoJobBooks.aspx">Estado JobBooks</a></li>
								<li><a title="Reporte PST sin producción" href="../RP_Reportes/ReportePSTSinProduccion.aspx">Reporte PST sin producción</a></li>
								<li><a title="Cargue descuentos SS" href="../CC_FinzOpe/CargueDescuentosSS.aspx">Cargue Descuentos SS</a></li>
								<li><a title="Liquidar Planillas de Actividades de Campo" href="../CC_FinzOpe/LiquidarPlanillasActividades.aspx">Liquidar Planillas</a></li>
								<li><a title="Liquidar Productividad de PST" href="../CC_FinzOpe/LiquidarProductividadPST.aspx">Liquidar Productividad</a></li>
								<li><a title="" href="#">&nbsp;</a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element5-->

				<div class="menu-element6">
					<!-- espacio para elementos en el pie de la aplicación-->
					<div class="name-menu">Inventario</div>
					<div class="icon-menu">
						<img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea"></div>
					<div class="submenu">
						<div class="linea1">
							<ul>
								<li><a title="" href="#"></a></li>
							</ul>
						</div>
						<div class="linea2">
							<ul>
								<li><a title="Módulo de Inventario" href="../Inventario/RegistroArticulos.aspx">Módulo de Inventario</a></li>
								<li><a title="" href="#">&nbsp;</a></li>
							</ul>
						</div>
					</div>
				</div>
				<!-- menu-element6-->
			</nav>

		</div>

	</div>
	<div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
