<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="Verificacion.aspx.vb" Inherits="WebMatrix.Verificacion" %>
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
<div id="lista" runat="server">
<h3><a href="#"><label>Critica</label></a></h3><br />
<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,TrabajoId,Ciudad,Cantidad,UsuarioEnvia,NombreUsuarioEnvia,UnidadEnvia,FechaEnvio,ObservacionesEnvio,UsuarioRecibe,UnidadRecibe,FechaRecibo,ObservacionesRecibo,Devolucion,MotivoDevolucion" AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true">
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
        <asp:BoundField DataField="NombreUsuarioEnvia" HeaderText="Usuario Envia" ReadOnly="True" SortExpression="NombreUsuarioEnvia" />
        <asp:BoundField DataField="FechaEnvio" HeaderText="FechaEnvio" SortExpression="FechaEnvio" />
        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
        <asp:BoundField DataField="ObservacionesEnvio" HeaderText="ObservacionesEnvio" SortExpression="ObservacionesEnvio" />
                    <asp:TemplateField HeaderText="Recibir este item" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Editar" ImageUrl="~/Images/Select_16.png" 
                                Text="Editar" ToolTip="Editar" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
    </Columns>    
</asp:GridView>
</div>
<div id="datos" runat="server" visible ="false">

<div class="form_left">
<label>Cantidad recibe:</label>
    <asp:TextBox ID="txtCantidadElegir" runat="server" AutoPostBack="True" Text="0"></asp:TextBox>
</div>
<div class="form_left">
<label>Unidad recibe:</label>
    <asp:DropDownList ID="ddlUnidadRecibe" runat="server" Enabled="false"></asp:DropDownList>
</div>
<div class="form_left">
<label>Unidad a enviar:</label>
    <asp:DropDownList ID="ddlUnidad" runat="server" >
        <asp:ListItem Value="21">Captura</asp:ListItem>
    </asp:DropDownList>
</div>
<div class="actions">
<label>Observaciones recibo</label>
    <cc1:Editor ID="txtObservaciones" Width="100%" Height="200px" runat="server"/>
<br />
</div>
<div class="form_left">
<label>Devolución:</label>
    <asp:CheckBox ID="chkDevolucion" runat="server" AutoPostBack="true"/>
</div>
<div class="actions" id="DvMotivoDev" runat="server" visible="false">
<label>Motivo devolución</label>
    <cc1:Editor ID="txtMotivoDevolucion" Width="100%" Height="200px" runat="server"/>
<br />
</div><br /><br /><br />
<div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" 
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Verificar" />
                <input id="btnCancelar" type="button" class="button" value="Cancelar"  runat="server"
                                        style="font-size: 11px;" onclick="location.href='TrabajosProyectos.aspx';" />
            </fieldset>
</div>
<div class="actions">
    <asp:Label ID="lblResult" runat="server"></asp:Label>
</div>
</div>
</asp:Content>
