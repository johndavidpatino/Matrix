<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_.master" CodeBehind="Gestion-Tareas-Trabajos.aspx.vb" Inherits="WebMatrix.Gestion_Tareas_Trabajos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            
            loadPlugins();
            
            $('#UsuariosAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios Asignados",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

        });


        function loadPlugins() {

            $("#<%= txtFInicioP.ClientId %>").mask("99/99/9999");
            $("#<%= txtFInicioP.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFFinP.ClientId %>").mask("99/99/9999");
            $("#<%= txtFFinP.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


        }

        function MostrarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("open");
        }

     </script>
</asp:Content>
    
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
                    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
                <ContentTemplate>
    <div style="width:100%">
    <div id="container">
    <h1><a>Gestión de </a>Tareas</h1>
    <asp:HiddenField ID="hfIdTrabajo" runat="server" Value="0" />
    <asp:HiddenField ID="hfRolEstima" runat="server" Value="0" />
    <asp:HiddenField ID="hfRolEjecuta" runat="server" Value="0" />
    <asp:HiddenField ID="hfUnidadEjecuta" runat="server" Value="0" />
    <asp:HiddenField ID="hfUsuarioAsignado" runat="server" Value="0" />
    <asp:HiddenField ID="hfUrlRetorno" runat="server" Value="0" />
    <div id="menu-form">
          <nav>
           <ul>
                <li><a><asp:LinkButton ID="LinkButton1" runat="server">Mis tareas</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="LinkButton2" runat="server">Asignar tareas masivas</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="LinkButton3" runat="server">Cronograma por unidades</asp:LinkButton></a></li>
            </ul>
           </nav>
       </div>
       <asp:Panel ID="pnlTrabajos" runat="server" Visible="true">
       <a>Tareas en curso</a>
        <div id="campo-formulario4">
            <p><a>Estado de tarea </a> <asp:DropDownList ID="ddEstado" runat="server" AutoPostBack="true">
                <asp:ListItem Text="--Seleccione--" Value="0"></asp:ListItem>
                <asp:ListItem Text="Ver todas" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Creada" Value="1"></asp:ListItem>
                <asp:ListItem Text="En curso" Value="2"></asp:ListItem>
                <asp:ListItem Text="Asignada" Value="3"></asp:ListItem>
                <asp:ListItem Text="Devuelta" Value="4"></asp:ListItem>
                <asp:ListItem Text="Finalizada" Value="5"></asp:ListItem>
                <asp:ListItem Text="No aplica" Value="6"></asp:ListItem>
            </asp:DropDownList></p>
            <asp:GridView ID="gvTrabajos" runat="server" DataKeyNames="Id,IdTrabajo" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false" 
                 EmptyDataText="No se encuentran trabajos con tareas asignadas">
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                    <asp:BoundField DataField="FIniP" HeaderText="FIni Planeada" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="FFinP" HeaderText="FFin Planeada" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Ir a Tareas" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgTareas" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                CommandName="IrTareas" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
       </asp:Panel>
       <asp:Panel ID="pnlAsignaciones" runat="server" Visible="false">
       <a>Asignaciones</a>
       <asp:HiddenField ID="hfWfIdAsignacion" runat="server" />
       <asp:HiddenField ID="hfWfTareaAsignacion" runat="server" />
        <div id="campo-formulario5">
            <p><a>Unidad que ejecuta </a> <asp:DropDownList ID="ddUnidad" runat="server" AutoPostBack="true">
            </asp:DropDownList></p>
            <asp:GridView ID="gvAsignaciones" runat="server" DataKeyNames="Id,UsuarioId,Retraso,RolEstima,RolEjecuta,TareaId" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                 AllowPaging="true" PageSize="50" 
                  PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="No se encuentran trabajos con tareas asignadas">
                 <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Asignar" ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="Info" ImageUrl="~/Images/cliente.jpg" Text="Info" OnClientClick="MostrarUsuariosAsignados()"
                            ToolTip="Asignar Usuario" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvAsignaciones.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvAsignaciones.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvAsignaciones.PageIndex + 1%>-<%= gvAsignaciones.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvAsignaciones.PageIndex +1) = gvAsignaciones.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvAsignaciones.PageIndex +1) = gvAsignaciones.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
        </div>
       </asp:Panel>
       <asp:Panel ID="pnlCronograma" runat="server" Visible="false">
       <a>Cronograma General</a>
        <div id="campo-formulario3">
            <p><a>Unidad que ejecuta </a> <asp:DropDownList ID="ddUnidadCronogramaGeneral" runat="server">
            </asp:DropDownList></p>
            <label>Fecha Inicio planeada</label>
            <asp:TextBox ID="txtFInicioP" runat="server"></asp:TextBox>
            <label>Fecha Fin planeada</label>
            <asp:TextBox ID="txtFFinP" runat="server"></asp:TextBox>
            <asp:Button ID="btnFiltrarCronograma" runat="server" Text="Filtrar" />
            <asp:GridView ID="gvCronograma" runat="server" DataKeyNames="Id,UsuarioId,Retraso,RolEstima,RolEjecuta,TareaId" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                 AllowPaging="false" PageSize="50" 
                  PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="No se encuentran trabajos con tareas asignadas">
                 <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="IDTRABAJO" HeaderText="Id" />
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
                    <asp:BoundField DataField="FIniP" HeaderText="FIni Planeada" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="FFinP" HeaderText="FFin Planeada" DataFormatString="{0:d}" />
                </Columns>
                <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvAsignaciones.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvAsignaciones.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvAsignaciones.PageIndex + 1%>-<%= gvAsignaciones.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvAsignaciones.PageIndex +1) = gvAsignaciones.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvAsignaciones.PageIndex +1) = gvAsignaciones.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
        </div>
       </asp:Panel>
    </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="UsuariosAsignados">
        <asp:UpdatePanel ID="upUsuariosAsignados" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <fieldset class="validationGroup">
                    <div class="form_left">
                        <label>
                            Usuarios disponibles para asignar</label>
                        <asp:DropDownList ID="ddlUsuariosDisponibles" runat="server" CssClass="mySpecificClass dropdowntext">
                        </asp:DropDownList>
                        <asp:Button ID="btnAdicionarUsuario" runat="server" Text="Asignar" OnClientClick="$('#UsuariosAsignados').dialog('close');"  />
                    </div>
                </fieldset>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
