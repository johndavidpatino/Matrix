<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="EmpleadosReporteGeneral.aspx.vb" Inherits="WebMatrix.EmpleadosReporteGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Reporte general empleados
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<asp:DropDownList runat="server" ID="ddlTipoReporte">
		<asp:ListItem Text="Información general" Value="1" />
		<asp:ListItem Text="Hijos" Value="2" />
		<asp:ListItem Text="Educación"  Value="3" />
		<asp:ListItem Text="Experiencia laboral"  Value="4" />
		<asp:ListItem Text="Contactos de emergencia"  Value="5" />
	</asp:DropDownList>
	<asp:Button Text="Generar" runat="server" id="btnGenerar"/>
</asp:Content>
