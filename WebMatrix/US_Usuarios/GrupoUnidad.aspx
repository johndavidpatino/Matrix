<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="GrupoUnidad.aspx.vb" Inherits="WebMatrix.GrupoUnidad" %>
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
        <div class="actions">
<div id="lista" runat="server">
<h3><a href="#"><label>Lista de grupos unidad</label></a></h3><br />
            <div class="form_left">
                <fieldset>                    
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry" Height="17px"></asp:TextBox>                    
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  />
                </fieldset>
            </div>
<br />
<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,GrupoUnidad,TipoGrupoUnidad,NombreTipoGrupoUnidad,Activo" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true"
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
        <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" />
        <asp:BoundField DataField="GrupoUnidad" HeaderText="Nombre" ReadOnly="True" SortExpression="GrupoUnidad" />
        <asp:BoundField DataField="NombreTipoGrupoUnidad" HeaderText="Tipo Grupo" ReadOnly="True" SortExpression="NombreTipoGrupoUnidad" />
        <asp:BoundField DataField="GrupoUnidad" HeaderText="Nombre" ReadOnly="True" SortExpression="GrupoUnidad" />
        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
            SortExpression="Activo" />
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
                                OnClientClick="return confirm('Esta seguro que desea eliminar este registro?');" 
                                Text="eliminar" ToolTip="Eliminar" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unidad" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgU" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Unidad" ImageUrl="~/Images/seg.png"
                                Text="Unidad" ToolTip="Unidad" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
    </Columns>
    <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" 
                                        CommandName="Page" Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' 
                                        SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" 
                                        CommandName="Page" Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' 
                                        SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>- <%= gvDatos.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" 
                                        CommandName="Page" 
                                        Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>' 
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" 
                                        CommandName="Page" 
                                        Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>' 
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
</asp:GridView>
</div>
<div id="datos" runat="server" visible="false">
    <h3><a href="#"><label>Gestión de datos</label></a></h3><br />
    <div class="actions">

        <div class="form_left">
        <label>Nombres:</label>
        <asp:TextBox ID="txtNombre" runat="server" ></asp:TextBox>
        </div>

        <div class="form_left">
        <label>Activo</label>
        <asp:CheckBox ID="chkActivo" runat="server" />
        </div>
        <br /><br />
        </div>
        <div class="actions"></div>
            <div class="actions">
                <div>
                    <center>
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </center>
                </div>  
            </div>
        <div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Visible="false" 
                     
                    Text="Guardar" />
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Visible="false" 
                     Text="Editar" />
                &nbsp;
                <input id="btnCancelar" type="button" class="button" value="Cancelar"  runat="server"
                                        style="font-size: 11px;" onclick="location.href='TipoGrupoUnidad.aspx';" />
            </fieldset>
        </div>
</div>
<div class="actions">
<div id="detalles" runat="server" visible="false"></div>
</div>
<br />
<br />
<br />
<br />
<br />
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
