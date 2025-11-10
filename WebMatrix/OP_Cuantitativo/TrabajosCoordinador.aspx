<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="TrabajosCoordinador.aspx.vb" Inherits="WebMatrix.TrabajosCoordinador" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
          function (value, element) {
              return this.optional(element) ||
                ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
          }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();
            $("#<%= txtFechaEnvio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaEnvio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#PresupuestosAsignadosXEstudio').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Presupuestos asignados",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            loadPlugins();
        });
        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
            <label id="lblTextInfo">
            </label>
        </p>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
            <label id="lbltextError">
            </label>
        </p>
    </div>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:Panel runat="server" id="accordion">
                <asp:Panel runat="server" id="accordion1">
                    <h3>
                                Trabajos
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                            <label>Búsqueda</label>
                                <div class="form_left">
                                <label>Estado</label>
                                <asp:DropDownList ID="ddlEstado" runat="server">
                                    <asp:ListItem Value="-1" Text="--Ver todos--"  Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Cerrado en OP"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Cerrado"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="Anulado"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="Cerrado en Proyectos"></asp:ListItem>
                                        <asp:ListItem Value="13" Text="En proceso de cierre Proyectos"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="En proceso de cierre Operaciones"></asp:ListItem>
                                </asp:DropDownList>
                                    </div>
                            <asp:TextBox ID="txtID" runat="server" placeholder="Por ID"></asp:TextBox>
                            <asp:TextBox ID="txtNombre" runat="server" placeholder="Por Nombre"></asp:TextBox>
                            <asp:TextBox ID="txtJobBook" runat="server" placeholder="Por JobBook"></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text ="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignación"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="Fecha Tentativa Inicio Campo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="Fecha Tentativa Finalizacion Trabajo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="CoeAsignado" HeaderText="OMP" />
                                <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrGestionar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/list_16_.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Avance" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Avance" ImageUrl="~/Images/find_16.png" Text="Actualizar"
                                            ToolTip="Ir a Avance de Campo" />
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
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" id="accordion2">
                    <h3>
                                Opciones del trabajo
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidCiudadId" runat="server" />
                        <asp:HiddenField ID="hfidCiudad" runat="server" />
                        <asp:HiddenField ID="hfidIdMuestra" runat="server" />
                        
                        <fieldset class="validationGroup">
                            <div>
                                <label>
                                    Ciudades asignadas</label>
                                <asp:GridView ID="gvCiudades" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id,CiudadId,Ciudad" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                        <asp:TemplateField HeaderText="Asignar Equipos" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAsignarEquipos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Asignar" ImageUrl="~/Images/cliente.jpg" Text="Asignar" ToolTip="Asignar Equipo de Trabajo" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enviar Encuestas" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEnviarEncuestas" runat="server" CausesValidation="False"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Enviar"
                                                    ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Enviar Encuestas a Bogotá" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ejecución Por Encuestador" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEjecucionEnc" runat="server" CausesValidation="False"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EjecucionEnc"
                                                    ImageUrl="~/Images/Select_16.png" Text="Ejecucion" ToolTip="Ejecucion Encuestador" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ejecución Por Supervisor" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEjecucionSup" runat="server" CausesValidation="False"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EjecucionSup"
                                                    ImageUrl="~/Images/Select_16.png" Text="Ejecucion" ToolTip="Ejecucion Supervisor" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="pnlDatos" runat="server">
                                    <div class="actions">
                                        <div class="form_right">
                                            <fieldset>
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <div class="form_right">
                                            <fieldset>
                                                <asp:Button ID="btnEstimaciones" runat="server" Text="Ver Estimación de Producción" Width="200px" />
                                                <asp:Button ID="btnEspecificaciones" runat="server" Text="Ver Información General" />
                                                <asp:Button ID="btnCapacitaciones" runat="server" Text="Información Técnica a Equipo" Width="200px" />
                                                <asp:Button ID="btnEstadoTareas" runat="server" Text="Ver todas las tareas" />
                                                <asp:Button ID="btnListadoDocumentos" runat="server" Text="Listado de documentos" />
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                    </div>
                                </asp:Panel>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" id="accordion3">
                    <h3>
                                Envío Paquetes de Encuestas
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:GridView ID="gvEnvios" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Guia" HeaderText="Guia" />
                                        <asp:BoundField DataField="Courier" HeaderText="Courier" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha Envío" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="Usuario" HeaderText="Registrado por" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar" OnClientClick="return confirm('Esta seguro de eliminar este registro ?');"
                                                    ToolTip="Eliminar envío" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="actions">
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Guia</label>
                                        <asp:TextBox ID="txtGuia" runat="server"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Courier</label>
                                        <asp:TextBox ID="txtCourier" runat="server"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Fecha de envío</label>
                                        <asp:TextBox ID="txtFechaEnvio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Cantidad</label>
                                        <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftetxtCantidad" runat="server" FilterType="Numbers"
                                            TargetControlID="txtCantidad">
                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                </div>
                                <div class="form_right">
                                    <fieldset>
                                        <asp:Button ID="btnEnvio" runat="server" Text="Enviar" />
                                        <asp:Button ID="btnCancelEnvio" runat="server" Text="Cancelar" />
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" id="accordion4">
                    <h3>
                                Asignación personal a trabajo
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                            <label>Personal asignado</label>
                                <asp:ImageButton ID="btnImgExportar1" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" />
                                <asp:GridView ID="gvPersonalAsignado" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id"/>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre"/>
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="TipoContratacion" HeaderText="Contratación" />
                                        <asp:BoundField DataField="TipoEncuestador" HeaderText="Encuestador" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar" OnClientClick="return confirm('Esta seguro de eliminar este registro ?');"
                                                    ToolTip="Eliminar envío" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                                <div class="actions">
                                </div>
                                <label>Personal disponible para asignación</label>
                                <asp:GridView ID="gvEncuestadoresPorAsignar" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="TipoContratacion" HeaderText="Contratación" />
                                        <asp:BoundField DataField="TipoEncuestador" HeaderText="Encuestador" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:TemplateField HeaderText="Asignar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAsignar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Asignar" ImageUrl="~/Images/Select_16.png" Text="Asignar" 
                                                    ToolTip="Asignar Encuestador a Trabajo" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="actions">
                                </div>

                                <div class="actions">
                                </div>
                                <div class="form_right">
                                    <fieldset>
                                        <asp:Button ID="btnCancelAsignacion" runat="server" Text="Cancelar" />
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" id="accordion5">
                    <h3>
                                Ejecución por Encuestador y Supervisores
                    </h3>
                    
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:HiddenField ID="hfTipoEjec" runat="server" />
                                <asp:ImageButton ID="btnImgExportar" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" />
                                <asp:GridView ID="gvEjecucion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="Contratacion" HeaderText="Contratacion" />
                                        <asp:BoundField DataField="NombreSupervisor" HeaderText="Nombre Supervisor" />
                                        <asp:BoundField DataField="NombreContratista" HeaderText="Nombre Contratista" />
                                        <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                                        <asp:BoundField DataField="Supervisadas" HeaderText="Supervisadas" />
                                        <asp:BoundField DataField="Porc_Supervision" HeaderText="% Supervision" />
                                        <asp:BoundField DataField="EncuestasVerificadasAnuladas" HeaderText="Verificadas Anuladas" />
                                        <asp:BoundField DataField="EncuestasVerificadasObservadas" HeaderText="Verificadas Observadas" />
                                        <asp:BoundField DataField="EncuestasVerificadasOK" HeaderText="Verificadas OK" />
                                        <asp:BoundField DataField="EncuestasVerificadasOKAudio" HeaderText="Verificadas OK Audio" />
                                        <asp:BoundField DataField="EncuestasVerificadasOKTelefonica" HeaderText="Verificadas OK Telefónica" />
                                        <asp:BoundField DataField="Verificadas" HeaderText="Verificadas" />
                                        <asp:BoundField DataField="Porc_Verificacion" HeaderText="% Verificacion" />
                                        <asp:BoundField DataField="EncuestasNoContactadas" HeaderText="No Contactadas" />
                                        <asp:BoundField DataField="EncuestasNoColabora_NodaInformacion" HeaderText="No Colabora / No da Informacion" />
                                        <asp:BoundField DataField="NoContacto" HeaderText="No Contacto" />
                                        <asp:BoundField DataField="Porc_NoContactado" HeaderText="% No Contactado" />
                                        <asp:BoundField DataField="Anuladas" HeaderText="Anuladas" />
                                        <asp:BoundField DataField="Porc_Anuladas" HeaderText="% Anuladas" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
