<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UsrCtrl_Usuarios.ascx.vb" Inherits="WebMatrix.UsrCtrl_Usuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" DataKeyNames="id,Usuario,Nombres,Apellidos,Email" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
        <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" 
            SortExpression="id" />
        <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
            SortExpression="Usuario" />
        <asp:BoundField DataField="Nombres" HeaderText="Nombres" 
            SortExpression="Nombres" />
        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
            SortExpression="Apellidos" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
            SortExpression="Activo" Visible="False" />
        <asp:TemplateField>
            <ItemTemplate>
            <asp:CheckBox ID="chkSeleccion" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<p><center>
    <asp:Label ID="lblRes" runat="server"></asp:Label>
</center></p>
<p>
    <asp:Button ID="btnObtenerValores" runat="server" Text="Obtener valores" 
        Visible="False" />
</p>
