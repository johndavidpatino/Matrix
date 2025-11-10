<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="Seguimiento.aspx.vb" Inherits="WebMatrix.Seguimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div>
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="causesValidation buttonText buttonSave corner-all" />
        </div>
        <div id ="lista_actas" runat="server" visible="true">
            <h3><a href="#"><label>Lista de seguimientos</label></a></h3>
            <asp:GridView ID="gvDatos" runat="server" AlternatingRowStyle-CssClass="odd" 
                AutoGenerateColumns="false" CssClass="displayTable" 
                DataKeyNames="Id,Usuario,Seguimiento,NombreUsuario,CierraTarea,FechaSeguimiento" 
                EmptyDataText="No existen registros para mostrar" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" 
                        Visible="false" />
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
                        SortExpression="Usuario" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="NombreUsuario" 
                        SortExpression="NombreUsuario" />
                    <asp:BoundField DataField="Seguimiento" HeaderText="Seguimiento" 
                        SortExpression="Seguimiento" />
                    <asp:BoundField DataField="FechaSeguimiento" HeaderText="FechaSeguimiento" 
                        SortExpression="FechaSeguimiento" />
                    <asp:CheckBoxField DataField="CierraTarea" HeaderText="Cierra tarea" 
                        SortExpression="CierraTarea" />
                    <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Editar" ImageUrl="~/Images/Select_16.png" 
                                Text="Editar" ToolTip="Editar" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgDel" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" 
                                OnClientClick="return confirm('Esta seguro que desea eliminar este seguimiento?');" 
                                Text="eliminar" ToolTip="Eliminar" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            <div id ="gestion_seguimiento" runat="server" visible="false">
                    <h3><a href="#"><label>Gestión de seguimientos</label></a></h3><br />
                    <div class="form_left">
                    <label>Descripción:</label>
                    <asp:TextBox ID="txtSeguimiento" runat="server" TextMode="MultiLine" 
                            style=" max-height:31px; max-width:118px;" ></asp:TextBox>
                    </div>
                    <div class="form_left">
                    <label>Usuario:</label>
                    <asp:DropDownList ID="ddUsuario" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form_left">
                    <label>Cierra tarea:</label>
                    <asp:CheckBox ID="chkCierra" runat="server" />
                    </div>
            </div>
            <div class="actions">
            <div>
        <center>
            <asp:Label ID="lblResult" runat="server"></asp:Label>
        </center>
    </div>
        <div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Visible="false" 
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Guardar" />
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Visible="false" 
                    CssClass="causesValidation buttonText buttonSave corner-all" Text="Editar" />
                &nbsp;
                <input id="btnCancelar" type="button" value="Cancelar" class="buttonText buttonCancel corner-all" runat="server"
                                        style="font-size: 11px;" onclick="location.href='Actas.aspx';" />
            </fieldset>
        </div>
        </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
