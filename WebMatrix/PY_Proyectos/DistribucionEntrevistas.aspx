<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master" 
    CodeBehind="DistribucionEntrevistas.aspx.vb" Inherits="WebMatrix.DistribucionEntrevistas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
     <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
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


            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFin.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();

            $('#GuardarObservacion').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Observación",
                width: "600px"
            });

            $('#GuardarObservacion').parent().appendTo("form");
        });

        function MostrarObservacion() {
            $('#GuardarObservacion').dialog("open");
        }

        function CerrarObservacion() {
            $('#GuardarObservacion').dialog("close");
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

    <br />
    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver a Trabajos"></asp:LinkButton>

    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
        <p style="color: White;">Listado de Entrevistas del Trabajo</p>
        <div class="actions">
        <div class="actions">
        <asp:Label ID="lblTextoTrabajo" runat="server" Text="Trabajo:" ForeColor="White" Font-Bold="True"></asp:Label>
        <asp:Label ID="lblTrabajo" runat="server" ForeColor="White" Font-Bold="True"></asp:Label>
            <br /><br />
        </div>
        <div class="actions">
        <asp:Panel ID="pnlEntrevistas" runat="server">
        <asp:GridView ID="gvEntrevistas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="20"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="Id,CiudadId,IdModerador" AllowPaging="True" EmptyDataText="No existen Entrevistas para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id"/>
                    <asp:BoundField DataField="Descripcion" HeaderText="Grupo Objetivo"/>
                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad"/>
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="Moderador" HeaderText="Moderador" />
                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgSeleccionar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Seleccionar" ImageUrl="~/Images/select_16.png" Text="Seleccionar" ToolTip="Seleccionar Entrevista" />
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
                                        Enabled='<%# IIf(gvDistribucion.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIf(gvDistribucion.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvDistribucion.PageIndex + 1%>-<%= gvDistribucion.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIf((gvDistribucion.PageIndex + 1) = gvDistribucion.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIf((gvDistribucion.PageIndex + 1) = gvDistribucion.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
            </asp:Panel>
            </div>

        <div class="actions">
        <asp:Panel ID="pnlDistribucion" runat="server" Visible="false">
        <p style="color: White;">Listado de Distribución Entrevistas del Trabajo</p>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
        <asp:HiddenField ID="hfidProyecto" runat="server" />
        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
        <asp:HiddenField ID="hfIdDistribucion" runat="server" />
        <asp:HiddenField ID="hfIdEntrevista" runat="server" />    
        <asp:HiddenField ID="hfNumero" runat="server" />
        <asp:HiddenField ID="hfEstado" runat="server" />
        <asp:HiddenField ID="hfTipo" Value="0" runat="server" />
        <asp:GridView ID="gvDistribucion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="Id,IdEntrevista,CiudadId,IdModerador,IdEstado" AllowPaging="True" EmptyDataText="No existen Entrevistas para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="IdEntrevista" HeaderText="Id Entrevista"/>
                    <asp:BoundField DataField="Numero" HeaderText="Numero"/>
                    <asp:BoundField DataField="GrupoObjetivo" HeaderText="Grupo Objetivo"/>
                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad"/>
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="Moderador" HeaderText="Moderador" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Efectiva" ShowHeader="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="chbEfectiva" runat="server" Checked='<%# IIf(Eval("IdEstado") = 4, True, False) %>' 
                                AutoPostBack="true" OnCheckedChanged="chkEfectiva_CheckedChanged"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Caída" ShowHeader="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="chbCaida" runat="server" Checked='<%# IIf(Eval("IdEstado") = 2, True, False) %>' 
                                AutoPostBack="true" OnCheckedChanged="chkCaida_CheckedChanged" OnClick="MostrarObservacion();"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Anulada" ShowHeader="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="chbAnulada" runat="server" Checked='<%# IIf(Eval("IdEstado") = 3, True, False) %>' 
                                AutoPostBack="true" OnCheckedChanged="chkAnulada_CheckedChanged" OnClick="MostrarObservacion();"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reemplazar" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgReemplazar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                CommandName="Reemplazar" ImageUrl="~/Images/select_16.png" Text="Reemplazar" ToolTip="Reemplazar Entrevista" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalles" ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDetalle" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="Detalle" ImageUrl="~/Images/list_16_.png" Text="Detalle" ToolTip="Detalle" />
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
                                        Enabled='<%# IIf(gvDistribucion.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIf(gvDistribucion.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvDistribucion.PageIndex + 1%>-<%= gvDistribucion.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIf((gvDistribucion.PageIndex + 1) = gvDistribucion.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIf((gvDistribucion.PageIndex + 1) = gvDistribucion.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
            <br>
            <asp:Button ID="btnRegresar" runat="server" Text="Volver a Entrevistas" />
            <br>
            </asp:Panel>
            </div>
            
        <div class="actions">
        <asp:Panel ID="pnlNewEntrevista" runat="server" Visible="false">
        
            <h4>Nueva Entrevista</h4>
            <div class="form_left">
                <fieldset>
                    <label>Grupo Objetivo</label>
                    <asp:TextBox ID="txtGrupoObjetivo" runat="server" Width="241px"></asp:TextBox>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>Departamento</label>
                    <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="true"></asp:DropDownList>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>Ciudad a agregar</label>
                    <asp:DropDownList ID="ddlCiudad" runat="server"></asp:DropDownList>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>Fecha Inicio</label>
                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>Fecha Fin</label>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                </fieldset>
            </div>
            <div class="form_right">
                <fieldset>
                    <label>Moderador</label>
                    <asp:DropDownList ID="ddlModerador" runat="server" CssClass="dropdowntext"></asp:DropDownList>
                </fieldset>
            </div>
            <div class="actions">
            <fieldset>
                <asp:Button ID="btnAdd" runat="server" Text="Agregar" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
            </fieldset>
            </div>
        </asp:Panel>
        </div>
        
        <div class="actions">
        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
        <div class="actions">
        <p style="color: White;">Detalles de Entrevistas del Trabajo</p>
        <asp:GridView ID="gvDetalles" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="Id,IdDistribucion,IdEntrevista,IdUsuario,IdEstado" AllowPaging="True" EmptyDataText="No existen Detalles para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="IdEntrevista" HeaderText="Id Entrevista"/>
                    <asp:BoundField DataField="Numero" HeaderText="Número Entrevista"/>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha"/>
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                </Columns>
            </asp:GridView>
            <br>
            <asp:Button ID="btnvolver" runat="server" Text="Volver a Distribucion" />
            <br>
            <br></br>
            </br>
            </br>
            </div>
        </asp:Panel>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    <div id="GuardarObservacion">
        <asp:UpdatePanel ID="upObservacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <label>Razones de Cambio de Estado de la Entrevista</label>
                <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Width="400px" Height="50px"></asp:TextBox>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="CerrarObservacion()" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarObservacion()" />
                <div class="actions"></div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
