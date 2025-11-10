<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="CambioContrasena.aspx.vb" Inherits="WebMatrix.CambioContraseña" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <asp:Label ID="LblContraseña" runat="server" Text="Antigua Contraseña"></asp:Label>
    <br />
    <asp:TextBox ID="txtcontraseña" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="Lblcontraseñanueva" runat="server" Text="Nueva Contraseña"></asp:Label>
    <br />
    <asp:TextBox ID="txtcontraseñanueva" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="Lblconfirmacion" runat="server" Text="Confirme Nueva Contraseña"></asp:Label>
    <br />
    <asp:TextBox ID="txtconfirmacion" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="lblresultado" runat="server" ForeColor="#CC0000"></asp:Label>
    <br />
    <asp:Button ID="btnguardar" runat="server" Text="Guardar" />
    <asp:Button ID="btncancelar" runat="server" Text="Cancelar" />
    <br />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
</asp:Content>
