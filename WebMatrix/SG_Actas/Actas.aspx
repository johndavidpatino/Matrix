<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="Actas.aspx.vb" Inherits="WebMatrix.Actas" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>

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
        <div id ="lista_actas" runat="server" visible="true">
            <h3><a href="#"><label>Lista de actas</label></a></h3>
            <p>
            <label>Filtro por</label>
                <asp:DropDownList ID="ddTipoFiltro" runat="server" Width="154px" 
                    AutoPostBack="True">
                    <asp:ListItem Value="0">No de acta</asp:ListItem>
                    <asp:ListItem Value="1">Tipo de acta</asp:ListItem>
                    <asp:ListItem Value="2">Unidad</asp:ListItem>
                </asp:DropDownList>
            </p>
            <div class="form_left">
                <fieldset>
                    
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry" Height="17px"></asp:TextBox>
                    <asp:DropDownList ID="ddTipoActaQuery" runat="server" Visible="false" ></asp:DropDownList>
                    <asp:DropDownList ID="ddUnidadQuery" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="causesValidation buttonText buttonSave corner-all" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="causesValidation buttonText buttonSave corner-all" />
                </fieldset>
            </div>
            <asp:GridView ID="gvDatos" runat="server" AllowPaging="True" 
                AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="False" 
                CssClass="displayTable" 
                DataKeyNames="id,Denominacion,NoActa,TipoId,Tipo,UnidadId,Unidad,Secretario,NombreSecretario,NombreLider,Lider,SeguimientoCompromisos,SeguimientoAcciones,TemasTratados,CompromisosConclusiones,Activa,FormId,Publica,FechaCreacion" EmptyDataText="No existen registros para mostrar" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" 
                        SortExpression="Id" Visible="false"/>
                    <asp:BoundField DataField="Denominacion" HeaderText="Denominacion" 
                        SortExpression="Denominacion" />
                    <asp:BoundField DataField="NoActa" HeaderText="NoActa" 
                        SortExpression="NoActa" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" 
                        SortExpression="Tipo" />
                    <asp:BoundField DataField="UnidadId" HeaderText="UnidadId" 
                        SortExpression="UnidadId" Visible="false"/>
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" 
                        SortExpression="Unidad" />
                    <asp:BoundField DataField="SeguimientoCompromisos" HtmlEncode="false" 
                        HeaderText="SeguimientoCompromisos" SortExpression="SeguimientoCompromisos" />
                    <asp:BoundField DataField="SeguimientoAcciones" 
                        HeaderText="SeguimientoAcciones" SortExpression="SeguimientoAcciones" />
                    <asp:BoundField DataField="TemasTratados" HeaderText="TemasTratados" 
                        SortExpression="TemasTratados" />
                    <asp:BoundField DataField="CompromisosConclusiones" Visible="false"
                        HeaderText="CompromisosConclusiones" SortExpression="CompromisosConclusiones" />
                    <asp:BoundField DataField="FechaCreacion" 
                        HeaderText="FechaCreacion" SortExpression="FechaCreacion" />
                    <asp:CheckBoxField DataField="Activa" HeaderText="Activa" 
                        SortExpression="Activa" />
                    <asp:CheckBoxField DataField="Publica" HeaderText="Publica" 
                        SortExpression="Publica" />
                    <asp:CommandField ShowSelectButton="True" buttontype="Image" SelectImageUrl="~/Images/Select_16.png"/>
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
            <br />
            </div>
        <div id ="gestion_actas" runat="server" visible="false">
            <h3><a href="#"><label>Gestión de actas</label></a></h3><br />
            <div class="form_left">
            <label>Denominación:</label>
            <asp:TextBox ID="txtDenominacion" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>
            </div>
            <div class="form_left">
            <label>No de Acta</label>
            <asp:TextBox ID="txtNoActa" runat="server" Width="50px"></asp:TextBox>
            </div>
            <div class="form_left">
            <label>Tipo de Acta</label>
            <asp:DropDownList ID="ddTipoActa" runat="server" AutoPostBack="True"></asp:DropDownList>
            </div>
            <div class="form_left">
            <label>Lider</label>
            <asp:DropDownList ID="ddLider" runat="server"></asp:DropDownList>
            </div>
            <div class="actions">

            <div class="form_left">
            <label>Publica</label>
            <asp:CheckBox ID="chkPublica" runat="server" />
            </div>
            <div class="form_left">
            <label>Activa</label>
            <asp:CheckBox ID="chkActiva" runat="server" />
            </div></div>
            <div class="actions">
            <label>Seguimiento de compromisos </label>
            <%--<asp:TextBox ID="txtSeguimientoCompromisos" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>--%>
            <cc1:Editor ID="txtSeguimientoCompromisos" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
        <br />
            </div>
            <div class="actions">
            <label>Seguimiento de acciones</label>
            <%--<asp:TextBox ID="txtSeguimientoAcciones" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>--%>
            <cc1:Editor ID="txtSeguimientoAcciones" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
            </div>
            <div class="actions">
            <label>Temas tratados</label>
            <%--<asp:TextBox ID="txtTemasTratados" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>--%>
            <cc1:Editor ID="txtTemasTratados" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
            </div>
            <div class="form_left">
            <label>Compromisos conclusiones</label>
            <%--<asp:TextBox ID="txtCompromisosConclusiones" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>--%>
            <cc1:Editor ID="txtCompromisosConclusiones" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
            </div>
        <br />
        <div class="actions" id="DvControlUsuarios" runat="server">
        <label>Selecciona la lista de participantes</label><asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
        <Control:Usuarios id="SelUser" runat="server"/>
        </div><br />
        
    <div class="actions" id="DvParticipantes" runat="server">
    <label>Participanes</label>
        <asp:GridView ID="gvParticipantes" runat="server"
                AlternatingRowStyle-CssClass="odd"  AutoGenerateColumns="false" 
                CssClass="displayTable" DataKeyNames="Id,UsuarioId,ActaId,Usuario" EmptyDataText="No existen registros para mostrar" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" 
                        SortExpression="Id" />
                    <asp:BoundField DataField="ActaId" HeaderText="ActaId" 
                        SortExpression="ActaId" />
                    <asp:BoundField DataField="UsuarioId" HeaderText="UsuarioId" 
                        SortExpression="UsuarioId" Visible="false"/>
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
                        SortExpression="Usuario" />
                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgParticipante" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="eliminar" ImageUrl="~/Images/delete_16.png" 
                                        Text="eliminar" ToolTip="Eliminar" OnClientClick="return confirm('Esta seguro que desea eliminar este participante?');"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                </Columns>
        </asp:GridView>
        <p>
            <asp:Button ID="btnAddParticipantes" runat="server" 
                Text="Añadir participantes" />
        </p>
            </div>
    <div class="actions" id="DvTareas" runat="server" visible="true">
    <label>Tareas</label>
            <div class="form_left">
            <label>Denominación:</label>
            <asp:TextBox ID="txtTarea" runat="server" TextMode="MultiLine" style=" max-height:31px; max-width:118px;" ></asp:TextBox>
            </div>
            <div class="form_left">
            <label>Responsable:</label>
            <asp:DropDownList ID="ddResponsable" runat="server"></asp:DropDownList>
            </div>
            <div class="form_left">
            <label>Fecha inicio ejecución</label>
                <asp:TextBox ID="calFechaInicio" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="calFechaInicio" runat="server" />
            </div>
            <div class="form_left">
            <label>Fecha limite</label>            
                <asp:TextBox ID="calFechaLimite" runat="server" ></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="calFechaLimite" runat="server" />
            </div>
            <div class="form_left">
            <label>Fecha de cierre</label>            
                <asp:TextBox ID="calFechaCierre" runat="server" ></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="calFechaCierre" runat="server" />
            </div>
            <div class="form_left">
            <label>Cerrada:</label>
            <asp:CheckBox ID="chkCerrada" runat="server" />
            </div>
            <div class="form_left">
            
                <asp:Button ID="btnAddTarea" runat="server" Text="Nueva tarea" Width="90px" />
            
            </div>
        <asp:GridView ID="gvTareas" runat="server"
                AlternatingRowStyle-CssClass="odd"  AutoGenerateColumns="false" 
                CssClass="displayTable" DataKeyNames="Id,ActaId,Tarea,Responsable,NombreResponsable,Cerrada,Fecha,FechaInicioEjecucion,FechaLimite,FechaCierre,EstadoId,TareaEstado" EmptyDataText="No existen registros para mostrar" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" 
                        SortExpression="Id" Visible="false"/>
                    <asp:BoundField DataField="ActaId" HeaderText="ActaId" 
                        SortExpression="ActaId" Visible="false" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" 
                        SortExpression="Tarea" />
                    <asp:BoundField DataField="FechaInicioEjecucion" HeaderText="FechaInicioEjecucion" 
                        SortExpression="FechaInicioEjecucion" />
                    <asp:BoundField DataField="FechaLimite" HeaderText="FechaLimite" 
                        SortExpression="FechaLimite" />
                    <asp:BoundField DataField="FechaCierre" HeaderText="FechaCierre" 
                        SortExpression="FechaCierre" />
                    <asp:BoundField DataField="TareaEstado" HeaderText="TareaEstado" 
                        SortExpression="TareaEstado" />
                    <asp:BoundField DataField="NombreResponsable" HeaderText="NombreResponsable" 
                        SortExpression="NombreResponsable" />                        
                    <asp:CheckBoxField DataField="Cerrada" HeaderText="Cerrada" 
                        SortExpression="Cerrada" />
                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgTarea" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="eliminar" ImageUrl="~/Images/delete_16.png" 
                                        Text="eliminar" ToolTip="Eliminar" OnClientClick="return confirm('Esta seguro que desea eliminar esta tarea?');"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdtTarea" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Editar" ImageUrl="~/Images/Select_16.png" 
                                        Text="Editar" ToolTip="Editar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seguimientos" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgSeg" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                CommandName="seguimiento" ImageUrl="~/Images/seg.png" 
                                Text="Seguimientos" ToolTip="Seguimientos" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
        </asp:GridView>
            </div>
        </div>
        <div id ="detalle_actas" runat="server" visible="false">
        <h3><a href="#"><label>Detalles del registro</label></a></h3>
            <div class="block" id="TplD" runat = "server">

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
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" 
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Guardar" />
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" 
                    CssClass="causesValidation buttonText buttonSave corner-all" Text="Editar" 
                    Visible="False" />
                <asp:Button ID="btnVerDetalle" runat="server" CommandName="Detalle" 
                    CssClass="causesValidation buttonText buttonSave corner-all" Text="Ver detalle" 
                    Visible="False" />
                &nbsp;
                <input id="btnCancelar" type="button" value="Cancelar" class="buttonText buttonCancel corner-all" runat="server"
                                        style="font-size: 11px;" onclick="location.href='Actas.aspx';" />
            </fieldset>
        </div></div>
        <br />
        <br />
        <br />
        <br />
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>