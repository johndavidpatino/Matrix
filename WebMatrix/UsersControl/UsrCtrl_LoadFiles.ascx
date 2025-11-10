<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UsrCtrl_LoadFiles.ascx.vb" Inherits="WebMatrix.UsrCtrl_LoadFiles" %>
<h1>Subir Archivo</h1> 
<asp:Label ID="lblIdDoc" runat="server" Text="Id Documento"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
<asp:TextBox ID="txtidDoc" runat="server"></asp:TextBox>
<asp:Label ID="lbldoccheck" runat="server"></asp:Label>
<br />
<br />
<asp:Label ID="IdUsuario" runat="server" Text="Id Usuario"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:TextBox ID="txtIdUsu" runat="server"></asp:TextBox>
<asp:Label ID="lblusucheck" runat="server"></asp:Label>
<br />
<br />
<asp:Label ID="lblContenedor" runat="server" Text="Id Contenedor"></asp:Label>
&nbsp;&nbsp;&nbsp;
<asp:TextBox ID="TxtIdCon" runat="server"></asp:TextBox>
<asp:Label ID="lblconcheck" runat="server"></asp:Label>
<br />
<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Label ID="Label1" runat="server" Text="Comentarios"></asp:Label>
<br />
<asp:TextBox ID="txtcoment" runat="server" Height="82px" TextMode="MultiLine" 
    Width="230px"></asp:TextBox>
<br />
<br />
<asp:FileUpload ID="UpLoadFile" runat="server" Width="196px" />
<br />
<asp:Button ID="btnUpload" runat="server" Text="Button" />
<br />
<asp:Label ID="lblMensaje2" runat="server"></asp:Label>

