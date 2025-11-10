<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master" CodeBehind="ProductoInterno.aspx.vb" Inherits="WebMatrix.ProductoInterno" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
<h3><a href="#"><label>Lista de roles</label></a></h3><br />
            <div class="form_left">
                <fieldset>                    
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry" Height="17px"></asp:TextBox>                    
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="causesValidation buttonText buttonSave corner-all" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="causesValidation buttonText buttonSave corner-all" />
                </fieldset>
            </div>
<br />
<asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,FechaEnvio,Tipo,Producto,Descripcion,UnidadRecibe,UnidadEnvia,NombreUnidadRecibe,NombreUnidadEnvia,Cantidad,Envia,Recibe,FechaRecepcion,Observaciones,ProyectoId" 
                AlternatingRowStyle-CssClass="odd" 
                CssClass="displayTable"  AllowPaging="true"
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
        <asp:BoundField DataField="Producto" HeaderText="Producto" ReadOnly="True" SortExpression="Producto" />
        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ReadOnly="True" SortExpression="Descripcion" />
        <asp:BoundField DataField="NombreUnidadRecibe" HeaderText="Unidad recibe" ReadOnly="True" SortExpression="NombreUnidadRecibe" />
        <asp:BoundField DataField="NombreUnidadEnvia" HeaderText="Unidad envia" ReadOnly="True" SortExpression="NombreUnidadEnvia" />
        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ReadOnly="True" SortExpression="Cantidad" />
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

        <div class="form_left">
        <label>Fecha de envío</label>
            <asp:TextBox ID="calFechaEnvio" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="calFechaEnvio" runat="server" />
        </div>

        <div class="form_left">
        <label>Producto:</label>
        <asp:TextBox ID="txtNombre" runat="server" ></asp:TextBox>
        </div>

        <div class="form_left">
        <label>Descripción:</label>
        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" ></asp:TextBox>
        </div>

        <div class="form_left">
        <label>Cantidad:</label>
        <asp:TextBox ID="txtCantidad" runat="server" ></asp:TextBox>
        </div>

        <div class="form_left">
        <label>Fecha de recepcion:</label>
            <asp:TextBox ID="calFechaRecepcion" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="calFechaRecepcion" runat="server" />
        </div>

        <div class="form_left">
        <label>Observaciones:</label>
        <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" >Recibido del cliente</asp:TextBox>
        </div>

        <div class="form_left">
        <label>Unidad envia:</label>
            <asp:DropDownList ID="ddlUnidadEnvia" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left" runat="server" >
        <label>Envia:</label>
        <asp:DropDownList ID="ddlEnvia" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left">
        <label>Unidad recibe:</label>
            <asp:DropDownList ID="ddlUnidadRecibe" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left">
        <label>Recibe:</label>
        <asp:DropDownList ID="ddlRecibe" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left">
        <label>Tipo Movimiento:</label>
            <asp:DropDownList ID="ddlTipoMovimiento" runat="server"></asp:DropDownList>
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
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Guardar" />
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Visible="false" 
                    CssClass="causesValidation buttonText buttonSave corner-all" Text="Editar" />
                &nbsp;
                <input Id="btnCancelar" type="button" value="Cancelar" class="buttonText buttonCancel corner-all" runat="server"
                                        style="font-size: 11px;" onclick="location.href='ProductoInterno.aspx';" />
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