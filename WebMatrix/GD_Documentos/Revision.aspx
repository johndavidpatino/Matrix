<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master" CodeBehind="Revision.aspx.vb" Inherits="WebMatrix.Revision" %>
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
<div Id="lista" runat="server">
<h3><a href="#"><label>Lista de pendientes por revisar</label></a></h3><br />
            <div class="form_left">
                <fieldset>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry" Height="17px"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  />
                </fieldset>
            </div>
<br />
<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="IdRevision,DocumentoId,UsuarioId,FechaAprobacion,TipoRevisionId,TipoRevision,DocumentoControladoId,NombreDocumento" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true"
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="IdRevision" HeaderText="IdRevision" ReadOnly="True" SortExpression="IdRevision" />
                    <asp:BoundField DataField="DocumentoId" HeaderText="DocumentoId" ReadOnly="True" SortExpression="DocumentoId" />
                    <asp:BoundField DataField="TipoRevision" HeaderText="TipoRevision" ReadOnly="True" SortExpression="TipoRevision" />
                    <asp:TemplateField HeaderText="Revisar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" 
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Editar" ImageUrl="~/Images/Select_16.png" 
                                Text="Editar" ToolTip="Editar" />
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
<div Id="datos" runat="server" visible="false">
    <h3><a href="#"><label>Gestión de datos</label></a></h3><br />
    <div class="actions">

        <div class="form_left" runat ="server" id="TPL">
            Datos de la revisión        
        </div>
        <div class="form_left">
            <label>Ruta del archivo:</label>
            <asp:TextBox ID="txtRutaArchivo" runat="server" ></asp:TextBox>
            </div>
            <div class="form_left">
            <label>Met recuperacion</label>
            <asp:TextBox ID="txtMetRec" runat="server" ></asp:TextBox>
            </div>
            <div class="form_left">
            <label>Tiempo de retencion:</label>
            <asp:TextBox ID="txtTiempoRete" runat="server" ></asp:TextBox>
            </div>
            <div class="form_left">
            <label>Disposición final:</label>
            <asp:TextBox ID="txtDisposicion" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>
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
                     
                    Text="Revisar" />
                &nbsp;
                <input Id="btnCancelar" type="button" class="button" value="Cancelar"  runat="server"
                                        style="font-size: 11px;" onclick="location.href='GD_SolicitudDocumentos.aspx';" />
            </fieldset>
        </div>
</div>
<div class="actions">
<div Id="detalles" runat="server" visible="false"></div>
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
