<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master"
    CodeBehind="GenerarRequerimientos.aspx.vb" Inherits="WebMatrix.GenerarRequerimientos" %>

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
        function Mensaje() {
            window.open("../CC_FinzOpe/ListadodeRequerimientos.aspx?TrabajoId=" + $("#" + "CPH_Section_CPH_Section_CPH_ContentForm_hfIdTrabajo")[0].value + "&IdMuestra=" + $("#" + "CPH_Section_CPH_Section_CPH_ContentForm_hfidmuestra")[0].value, "", "width=800,height=300");
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
    <asp:UpdatePanel ID="upDatosRequerimientos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Trabajos
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfJobook" runat="server" />
                        <asp:HiddenField ID="hfnombretrabajo" runat="server" />
                        <asp:HiddenField ID="hfIdCoordinador" runat="server" />
                        <asp:HiddenField ID="hfidmuestra" runat="server" />
                        <asp:HiddenField ID="hfidciudad" runat="server" />
                        <asp:HiddenField ID="hfidFechaIni" runat="server" />
                        <asp:HiddenField ID="hfiCiudatexto" runat="server" />
                        <asp:HiddenField ID="hfidMet" runat="server" />
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Buscar por</label>
                                <asp:TextBox ID="txtID" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtJobBook" placeholder="JobBook" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtNombreTrabajo" placeholder="Nombre Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtNoPropuesta" placeholder="No. Propuesta" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False"
                            PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="GerenteCuentas" HeaderText="GerenteProyecto" />
                                <asp:BoundField DataField="COEAsignado" HeaderText="OMPAsignado" />
                                <asp:BoundField DataField="GerenciaOperativa" HeaderText="GerenciaOperativa" />
                                <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="FechaTentativaInicioCampo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Ciudades" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Ciudades" ImageUrl="~/Images/Select_16.png" Text="Ciudades" ToolTip="Ver Muestra por Ciudad" />
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
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Muestra por ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:GridView ID="gvmuestra" runat="server" Width="100%" AutoGenerateColumns="False"
                                    PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="MuestraId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Muestraid" HeaderText="MuestraId" />
                                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                        <asp:BoundField DataField="CodCiudad" HeaderText="CodCiudad" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Coordinador" HeaderText="Coordinador" />
                                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader ="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkStatus" runat="server"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PersonasAsignadas" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgpersonas" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="PersonasAsigandas" ImageUrl="~/Images/cliente.jpg" Text="PersonasAsigandas"
                                                    ToolTip="PersonasAsignadas" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                 <div class="form_right">
                                    <fieldset>
                                        <asp:Button ID="btnrequisicion" runat="server" Text="Consultar" />
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Personas Asignadas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:GridView ID="gvPersonalAsignado" runat="server" Width="100%" AutoGenerateColumns="False"
                                    PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="TipoContratacion" HeaderText="Contratación" />
                                        <asp:BoundField DataField="TipoEncuestador" HeaderText="Encuestador" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="CodCiudad" HeaderText="CodCiudad" />
                                    </Columns>
                                </asp:GridView>
                                <div class="form_right">
                                    <fieldset>
                                        <asp:Button ID="btnGenerarRequisicion" runat="server" Text="Generar Requisicion" />
                                        
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
