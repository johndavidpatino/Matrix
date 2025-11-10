<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="ListaActas.aspx.vb" Inherits="WebMatrix.ListaActas" %>
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
<h3><a href="#"><label>Lista de Actas</label></a></h3><br />
            <div class="form_left">
                <fieldset>                    
                <input id="btnNew" type="button" value="Nuevo" class="button" runat="server"
                                        style="font-size: 11px;" onclick="location.href='ActasComite.aspx';" />
                </fieldset>
            </div>
<br />
<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id
      ,Fecha
      ,UnidadId
      ,NoActa
      ,TipoReunionId
      ,Contenedor
      ,ContenedorId
      ,OrdenDia
      ,Conclusiones
      ,Descripcion
      ,UsuarioCrea
      ,UsuarioLidera" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true"
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
        <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" />
        <asp:BoundField DataField="OrdenDia" HeaderText="Orden de Dia" ReadOnly="True" SortExpression="OrdenDia" />
        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="Descripcion" HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="EditAC" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="EditarActaComite" ImageUrl="~/Images/edit.png"
                                Text="Unidades" ToolTip="Unidades" />
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
</asp:Content>
