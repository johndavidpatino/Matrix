<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral.Master" CodeBehind="DefaultOLD.aspx.vb" Inherits="WebMatrix.DefaultHomeOLD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style>
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
	Inicio
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
	Matrix
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Mapa de Procesos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
	<div id="contenido1">


		<div id="dobleflecha1">
			<img src="images/menu/dobleflecha.png" width="35" height="35" alt="">
		</div>
		<div id="cajasarriba">
			<a href="../MBO/Default.aspx">
				<img src="images/menu/proceso-gerencial.png" width="290" height="45" alt=""></a>
		</div>
		<div id="cajasarriba">
			<a href="../GD_Documentos/Default.aspx">
				<img src="images/menu/gestion-calidad.png" width="290" height="45" alt=""></a>
		</div>
		<div id="dobleflecha2">
			<img src="images/menu/dobleflecha2.png" width="35" height="35" alt="">
		</div>
		<div id="barraizq">
			<a href="#">
				<img src="images/menu/necesidades-cte.png" width="60" height="270" alt=""></a>
		</div>
		<div id="content-centro" style="height: 270px;">
			<div id="btnscentro"><a href="../CU_Cuentas/Home.aspx">
				<img src="images/menu/cuentas.png" width="600" height="60" alt=""></a></div>
			<div id="btnscentro"><a href="../PY_Proyectos/Home.aspx">
				<img src="images/menu/proyectos.png" width="600" height="60" alt=""></a></div>
			<div id="btnscentro"><a href="../RE_GT/HomeRecoleccion.aspx">
				<img src="images/menu/recoleccion.png" width="600" height="60" alt=""></a></div>
			<div id="btnscentro"><a href="../RE_GT/HomeGestionTratamiento.aspx">
				<img src="images/menu/gestion.png" width="600" height="60" alt=""></a></div>
		</div>
		<div id="barrader">
			<a href="#">
				<img src="images/menu/satisfaccion-cte.png" width="60" height="270" alt=""></a>
		</div>
		<br />
		<div id="abajo">
			<div id="cajasabajo"><a href="../FI_AdministrativoFinanciero/Default.aspx">Administrativo y financiero</a></div>
			<div id="cajasabajo"><a href="../TH_TalentoHumano/Home.aspx">Recursos humanos</a></div>
			<div id="cajasabajo"><a href="../FI_AdministrativoFinanciero/Default-Compras.aspx">Compras y outsourcing</a></div>
			<div id="cajasabajo"><a href="../IT/Default.aspx">Tecnología (IT)</a></div>
			<div id="cajasabajo"><a href="../ES_Estadistica/Home.aspx">Estadística</a></div>
		</div>



	</div>
</asp:Content>
