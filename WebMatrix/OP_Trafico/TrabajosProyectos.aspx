<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="TrabajosProyectos.aspx.vb" Inherits="WebMatrix.TrabajosProyectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">

<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,ProyectoId,TecnicaId,Muestra,FechaTentativaInicioCampo,FechaTentativaFinalizacion,COE,JobBook,NombreTrabajo,UnidadID,Unidad,GrupoUnidadId" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true"
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false"/>
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" SortExpression="JobBook" />
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" SortExpression="NombreTrabajo" />
                    <asp:TemplateField HeaderText="Inicio Trafico Encuestas" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgITE" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="ITE" ImageUrl="~/Images/seg.png" 
                                Text="ITE" ToolTip="ITE" />
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

</asp:Content>