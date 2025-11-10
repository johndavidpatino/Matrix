<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="Feedback.aspx.vb" Inherits="WebMatrix.Feedback" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Feedback Matrix</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
<asp:Panel ID="pnlFeedback" runat="server">
<div class="form_left">
<label>Asunto:</label>
    <asp:DropDownList ID="ddlAsunto" runat="server"></asp:DropDownList>
</div>

<div class="actions">
<br />
<label>Por favor escriba aquí su Mensaje</label>
    <asp:TextBox ID="txtTextoMensaje" Width="100%" Height="200px" TextMode="MultiLine" runat="server"/>
</div>
<br />
<div class="actions">
<asp:Button ID="btnguardar" runat="server" Text="Enviar" />
<br />
<br />
</div>
</asp:Panel>
<asp:Panel ID="Pnlenviado" runat="server" Visible="false">
    <br />
    <p class="displayItems">Muchas gracias por su retroalimentación. Pronto nos pondremos en contacto con usted.</p>
    <br />
</asp:Panel>
</asp:Content>
