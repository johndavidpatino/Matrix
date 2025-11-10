<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/SG_F.master" CodeBehind="ActasComite.aspx.vb" Inherits="WebMatrix.ActasComite" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>
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
<div id="lista" runat="server">

<div id="datos" runat="server" >
    <h3><a href="#"><label>Gestión de actas</label></a></h3><br />
    <div class="actions">
        <div class="form_left">
        <label>No Acta:</label>
        <asp:TextBox ID="txtNoActa" runat="server"></asp:TextBox>
        </div>

        <div class="form_left">
        <label>Tipo de reunión:</label>
        <asp:DropDownList ID="ddlTipoReunion" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left">
        <label>Unidad:</label>
        <asp:DropDownList ID="ddlUnidad" runat="server"></asp:DropDownList>
        </div>

        <div class="form_left">
            <label>Lider</label>
            <asp:DropDownList ID="ddlLider" runat="server"></asp:DropDownList>
        </div>
        </div>
        <div class="actions">
        <label>Orden del dia</label>
            <asp:TextBox ID="txtOrdenDia" Height="100px" MaxLength="550" TextMode="MultiLine" runat="server"></asp:TextBox>       
        </div>
        <div class="actions">
        <label>Conclusiones de la lectura del acta anterior:Aquí debe usted dejar por escrito que se dio lectura</label>
            <cc1:Editor ID="txtConclusiones" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
        </div>
        <div class="actions">
        <label>Desarrollo de temas tratados: descripción</label>
            <cc1:Editor ID="txtDescripcion" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
            </div>            
            <div class="actions" id="DvParticipantes" visible ="false" runat="server">
    <label>Asistentes</label>
        <div class="actions" id="DvControlUsuarios" runat="server">
        <label>Selecciona la lista de asistentes</label><asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
        <Control:Usuarios id="SelUser" runat="server"/>
        </div><br />
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
            <asp:Button ID="btnAddAsistentes" runat="server" 
                Text="Añadir Asistentes" />
        </p>
            </div>
            <div class="actions" id="DvSeguimientos" visible ="false" runat="server">
            <label>Seguimientos</label>
            <div class="form_left">
            <label>Acción:</label>
            <asp:TextBox ID="txtAccion" runat="server" TextMode="MultiLine" 
                    style=" max-height:31px; max-width:118px;" ></asp:TextBox>
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
            <label>Fecha de compromiso</label>            
                <asp:TextBox ID="calFechaCompromiso" runat="server" ></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" 
                    TargetControlID="calFechaCompromiso" runat="server" />
            </div>
            <div class="form_left">
            <label>Fecha de cierre</label>            
                <asp:TextBox ID="calFechaCierre" runat="server" ></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="calFechaCierre" runat="server" />
            </div>
            <div class="form_left">
            <label>Estado:</label>
                <asp:DropDownList ID="ddlEstadoSeg" runat="server"></asp:DropDownList>
            </div>
            <div class="form_left">
                <asp:Button ID="btnAddTarea" runat="server" Text="Nuevo seguimiento" />            
            </div>
            <div class="actions"></div>
        <asp:GridView ID="gvTareas" runat="server"
                AlternatingRowStyle-CssClass="odd"  AutoGenerateColumns="false" 
                CssClass="displayTable" DataKeyNames="Id,ActaId,Accion,ResponsableId,Responsable,FechaInicio,FechaCompromiso,Estado,EstadoId,FechaCierre" EmptyDataText="No existen registros para mostrar" 
                PagerStyle-CssClass="headerfooter ui-toolbar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" 
                        SortExpression="Id" Visible="false"/>
                    <asp:BoundField DataField="ActaId" HeaderText="ActaId" 
                        SortExpression="ActaId" Visible="false" />
                    <asp:BoundField DataField="Accion" HeaderText="Accion" 
                        SortExpression="Accion" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" 
                        SortExpression="Estado" />
                    <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdtTarea" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Editar" ImageUrl="~/Images/Select_16.png" 
                                        Text="Editar" ToolTip="Editar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgTarea" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="eliminar" ImageUrl="~/Images/delete_16.png" 
                                        Text="eliminar" ToolTip="Eliminar" OnClientClick="return confirm('Esta seguro que desea eliminar este seguimiento?');"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
        </asp:GridView>
            </div>
            </div>
            <div class="actions">
                <div>
                    <center>
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </center>
                </div>  
            </div>
        <div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" 
                    CssClass="button" 
                    Text="Guardar" />
                <input id="btnCancelar" type="button" value="Cancelar" class="button" runat="server"
                                        style="font-size: 11px;" onclick="location.href='ListaActasComite.aspx';" />
            </fieldset>
        </div>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
