<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="RMC.aspx.vb" Inherits="WebMatrix.RMC" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
<div class="actions">
<div class="form_left">
<label>Res Ciudad:</label>
    <asp:TextBox ID="txtRCiudad" ReadOnly="true" runat="server"></asp:TextBox>
</div>
<div class="form_left">
<label>Cantidad disponible:</label>
    <asp:TextBox ID="txtCuenta" ReadOnly="true" runat="server"></asp:TextBox>    
</div>
<div class="form_left">
<label>Trabajo ID:</label>
    <asp:TextBox ID="txtTrabajoId" ReadOnly="true" runat="server"></asp:TextBox>    
</div>
<div class="form_left">
<label>Cantidad a elegir:</label>
    <asp:TextBox ID="txtCantidadElegir" runat="server" AutoPostBack="True" Text="0"></asp:TextBox>
</div>
<div class="form_left">
<label>Unidad:</label>
    <asp:DropDownList ID="ddlUnidad" runat="server" Enabled="false"></asp:DropDownList>
</div>
<div class="form_left">
<label>Unidad recibe:</label>
    <asp:DropDownList ID="ddlUnidadRecibe" runat="server" Enabled="false"></asp:DropDownList>
</div>
</div>
<div class="actions">
<label>Observaciones</label>
    <asp:Textbox ID="txtObservaciones" TextMode="MultiLine" runat="server"/>
<br />
</div>
<div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" 
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Guardar" />
                <input id="btnCancelar" type="button" class="button" value="Cancelar"  runat="server"
                                        style="font-size: 11px;" onclick="location.href='TrabajosProyectos.aspx';" />
            </fieldset>
</div>
<div class="actions">
    <center><asp:Label ID="lblResult" runat="server" Text=""></asp:Label></center>
    </div><br />
</asp:Content>