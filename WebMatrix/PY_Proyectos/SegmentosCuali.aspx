<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="SegmentosCuali.aspx.vb" Inherits="WebMatrix.SegmentosCuali" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
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

            $("#<%= txtFechaInicial.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicial.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFinal.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinal.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            validationForm();

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
    Trabajo:
    <asp:Label ID="lblTrabajo" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver a Trabajos"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Segmentos
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left2">
                            <fieldset>
                                <label>
                                    </label>
                                   <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                            </fieldset>
                        </div>
                        <asp:HiddenField ID="hfTrabajoId" runat="server" />
                        <asp:HiddenField ID="hfSegmentoId" runat="server" />
                        <asp:GridView ID="gvSegmentos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="AvancePlaneacion" HeaderText="Avance Planeación" DataFormatString="{0:P1}" />
                                <asp:BoundField DataField="AvanceEjecucion" HeaderText="Avance Ejecución" DataFormatString="{0:P1}" />
                                <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Abrir" ImageUrl="~/Images/list_16_.png" Text="Actualizar" ToolTip="Actualizar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Campo" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Campo" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Ir a Avance de Campo" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Moderador" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgModerador" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Moderador" ImageUrl="~/Images/cliente.jpg" Text="Agregar" ToolTip="Agregar Moderador" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reclutador" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgReclutador" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Reclutador" ImageUrl="~/Images/cliente.jpg" Text="Agregar" ToolTip="Agregar Reclutador" />
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
                                                    Enabled='<%# IIF(gvSegmentos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvSegmentos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvSegmentos.PageIndex + 1%>-<%= gvSegmentos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvSegmentos.PageIndex +1) = gvSegmentos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvSegmentos.PageIndex +1) = gvSegmentos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>

                    <div class="actions"></div>
                        <asp:Panel ID="pnlModeradores" runat="server" Visible="false">
                            <p>Listado de Moderadores</p>
                            <fieldset>
                                <label id="lblModerador" runat="server">
                                    Moderador</label>
                                <asp:DropDownList ID="ddlModerador" runat="server" CssClass="dropdowntext">
                                </asp:DropDownList>
                                <br /><br />
                                <asp:Button ID="btnAsignarModerador" runat="server" Text="Asignar" />
                            </fieldset>
                            <br />
                             <asp:HiddenField ID="hfIdModerador" runat="server" />
                            <asp:GridView ID="gvModeradores" runat="server" DataKeyNames="Id,TrabajoId,SegmentoId,Identificacion" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                                EmptyDataText="No se encuentran Moderadores para este segmento">
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignacion" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBorrarModerador" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="BorrarModerador" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            
                        </asp:Panel>


                    <div class="actions"></div>
                        <asp:Panel ID="pnlReclutadores" runat="server" Visible="false">
                            <p>Listado de Reclutadores</p>
                            <fieldset>
                                <label id="lblReclutador" runat="server">
                                    Reclutador</label>
                                <asp:DropDownList ID="ddlReclutador" runat="server" CssClass="dropdowntext">
                                </asp:DropDownList>
                                <br /><br />
                                <asp:Button ID="btnAsignarReclutador" runat="server" Text="Asignar" />
                            </fieldset>
                            <br />
                            <asp:HiddenField ID="hfIdReclutador" runat="server" />
                            <asp:GridView ID="gvReclutadores" runat="server" DataKeyNames="Id,TrabajoId,SegmentoId,Identificacion" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                                EmptyDataText="No se encuentran Reclutadores para este segmento">
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignacion" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBorrarReclutador" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="BorrarReclutador" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            
                        </asp:Panel>


                    <div id="accordion1">
                        <h3>
                            <a href="#">
                                <label>
                                    Información del segmento
                                </label>
                            </a>
                        </h3>
                        <div class="block">
                            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                            <asp:HiddenField ID="hfIdSegmento" runat="server" />
                            <fieldset class="validationGroup">
                                <div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Descripcion general de segmento:</label>
                                            <asp:TextBox ID="txtDescripcion" Width="100%" runat="server" />
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                        <h4>
                                            <a style="font-weight: bold">Metodología</a> - Cómo lo vamos a hacer?</h4>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Cantidad requerida de sesiones/entrevistas:</label>
                                                <asp:TextBox ID="txtCantidad" runat="server" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                    TargetControlID="txtCantidad">
                                                </asp:FilteredTextBoxExtender>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Observaciones:</label>
                                                <asp:TextBox ID="txtObservacionesMetodologia" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Método Reclutamiento:</label>
                                                <asp:TextBox ID="txtMetodoReclutamiento" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    No. Reclutadores:</label>
                                                <asp:TextBox ID="txtNoReclutadores" runat="server" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                    TargetControlID="txtNoReclutadores">
                                                </asp:FilteredTextBoxExtender>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Ayudas Requeridas</label>
                                                <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Otras ayudas requeridas:</label>
                                                <asp:TextBox ID="txtOtrasAyudas" runat="server" />
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <h4>
                                            <a style="font-weight: bold">Ubicación y Fechas</a> - Cuándo y dónde lo vamos a
                                            hacer?</h4>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Departamento</label>
                                                <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Ciudad</label>
                                                <asp:DropDownList ID="ddlCiudad" runat="server">
                                                </asp:DropDownList>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Lugar</label>
                                                <asp:DropDownList ID="ddlLugar" runat="server">
                                                    <asp:ListItem Text="Salas Ipsos"></asp:ListItem>
                                                    <asp:ListItem Text="Hotel"></asp:ListItem>
                                                    <asp:ListItem Text="Sala Gesell"></asp:ListItem>
                                                    <asp:ListItem Text="Sala del Cliente"></asp:ListItem>
                                                    <asp:ListItem Text="Restaurante"></asp:ListItem>
                                                    <asp:ListItem Text="Otro"></asp:ListItem>
                                                </asp:DropDownList>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Especificaciones Lugar:</label>
                                                <asp:TextBox ID="txtEspecificacionesLugar" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Fecha inicial:
                                                </label>
                                                <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Fecha final:
                                                </label>
                                                <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <h4>
                                            <a style="font-weight: bold">Grupo Objetivo</a> - a quiénes buscamos?</h4>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Nivel socioeconómico:</label>
                                                <asp:TextBox ID="txtNSE" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Edades:</label>
                                                <asp:TextBox ID="txtEdades" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Género:</label>
                                                <asp:TextBox ID="txtGenero" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Otras características:</label>
                                                <asp:TextBox ID="txtOtrasCaracteristicas" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Exclusiones y restricciones:</label>
                                                <asp:TextBox ID="txtExclusionesRestricciones" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    No. Personas:</label>
                                                <asp:TextBox ID="txtNoPersonas" runat="server" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                    TargetControlID="txtNoPersonas">
                                                </asp:FilteredTextBoxExtender>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <h4>
                                            <a style="font-weight: bold">Especificaciones de Gastos</a> - qué más debemos saber?</h4>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Gastos de Viaje:</label>
                                                <asp:TextBox ID="txtGastosDeViaje" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Incentivos:</label>
                                                <asp:TextBox ID="txtIncentivos" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Alimentación:</label>
                                                <asp:TextBox ID="txtAlimentacion" runat="server" />
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <h4>
                                            <a style="font-weight: bold">Entregables</a> - qué requerimos después?</h4>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbTranscripciones" runat="server" Text="Transcripciones" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbTraducciones" runat="server" Text="Traducciones" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbVideo" runat="server" Text="Videos" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbAudios" runat="server" Text="Audios" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbFiltros" runat="server" Text="Filtros" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <asp:CheckBox ID="chbFlashReport" runat="server" Text="Flash Report" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left2">
                                            <fieldset>
                                                <label>
                                                    Otros Entregables:</label>
                                                <asp:TextBox ID="txtOtros" runat="server" />
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Observaciones generales:</label>
                                            <asp:TextBox ID="txtObservacionesGenerales" Width="100%" runat="server" />
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                        <div class="form_right">
                                            <fieldset>
                                                <asp:Button ID="btnGrabar" runat="server" Text="Guardar" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                                <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" Enabled="false" />
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
