<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="RP_RegistroProduccionOP.aspx.vb" Inherits="WebMatrix.RP_RegistroProduccionOP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });
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

            $("#<%= txtFechaInicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
            $("#<%= txtFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            validationForm();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <p>
        <br />
    </p>
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
    <fieldset class="validationGroup">
        <div class="form_left">
            <label>FechaInicio</label>
            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="required text textEntry"></asp:TextBox>
            <label>FechaFin</label>
            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="required text textEntry"></asp:TextBox>
        </div>
        <div class="form_left">
            <label>Area</label>
            <asp:DropDownList ID="ddlAreas" runat="server"></asp:DropDownList>
            <label>Usuario</label>
            <asp:DropDownList ID="ddlUsuario" runat="server"></asp:DropDownList>
        </div>
        <div class="actions">
            <div class="form_right">
                <asp:Button ID="btnBuscar" runat="server" Text="Reporte general" CssClass="causesValidation" />
                <asp:Button ID="btnInfConsEjec" runat="server" Text="Informe Consolidado Ejeccución" CssClass="causesValidation" />
            </div>
        </div>
    </fieldset>

    <div>
        <asp:ImageButton ID="btnImgExportarInforme0" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" Visible="false" />
        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
            <PagerStyle CssClass="headerfooter ui-toolbar" />
            <SelectedRowStyle CssClass="SelectedRow" />
            <AlternatingRowStyle CssClass="odd" />
            <Columns>
                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:g}" />
                <asp:BoundField DataField="PersonaNombre" HeaderText="Persona" />
                <asp:BoundField DataField="UnidadNombre" HeaderText="Area" />
                <asp:BoundField DataField="ActividadNombre" HeaderText="Actividad" />
                <asp:BoundField DataField="SubActividadNombre" HeaderText="SubActividad" />
                <asp:BoundField DataField="TipoNombre" HeaderText="Tipo" />
                <asp:BoundField DataField="NombreTipoAplicativoProceso" HeaderText="TipoAplicativoProceso" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha Actividad" DataFormatString="{0:d}" />
                <asp:BoundField DataField="HoraInicio" HeaderText="HoraInicio" />
                <asp:BoundField DataField="HoraFin" HeaderText="HoraFin" />
                <asp:BoundField DataField="CantidadGeneral" HeaderText="Cantidad" />
            </Columns>
            <PagerTemplate>
                <div class="pagingButtons">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                            </td>
                            <td>
                                <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                    SkinID="paging">Ultimo »</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </PagerTemplate>
        </asp:GridView>
    </div>
    <div>
        <asp:ImageButton ID="btnImgExportarInforme1" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" Visible="false" />
        <asp:GridView ID="gvInformeConsolidadoEjecucion" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="PersonaId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
            <PagerStyle CssClass="headerfooter ui-toolbar" />
            <SelectedRowStyle CssClass="SelectedRow" />
            <AlternatingRowStyle CssClass="odd" />
            <Columns>
                <asp:BoundField DataField="Nombres" HeaderText="Nombres" DataFormatString="{0:g}" />
                <asp:BoundField DataField="DiasHabiles" HeaderText="Dias Habiles" />
                <asp:BoundField DataField="DiasReportados" HeaderText="Dias Reportados" />
                <asp:BoundField DataField="TotalHoras" HeaderText="Total Horas" />
                <asp:BoundField DataField="PromedioHoras" HeaderText="Promedio Horas" />
                <asp:BoundField DataField="HorasProductivas" HeaderText="Horas Productivas" />
                <asp:BoundField DataField="PorcentajeHorasProductivas" HeaderText="% Horas Productivas" />
                <asp:BoundField DataField="HorasNoProductivas" HeaderText="Horas No Productivas" />
                <asp:BoundField DataField="PorcentajeHorasNoProductivas" HeaderText="% Horas No Productivas" />
                <asp:BoundField DataField="HorasOtros" HeaderText="Horas Otros" />
                <asp:BoundField DataField="PorcentajeHorasOtros" HeaderText="% Horas Otros" />
                <asp:BoundField DataField="HorasReproceso" HeaderText="Horas Reproceso" />
                <asp:BoundField DataField="PorcentajeHorasReproceso" HeaderText="% Horas Reproceso" />
            </Columns>
            <PagerTemplate>
                <div class="pagingButtons">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                    Enabled='<%# IIf(gvInformeConsolidadoEjecucion.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Enabled='<%# IIf(gvInformeConsolidadoEjecucion.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                            </td>
                            <td>
                                <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                    Enabled='<%# IIf((gvInformeConsolidadoEjecucion.PageIndex + 1) = gvInformeConsolidadoEjecucion.PageCount, "false", "true")%>'
                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                    Enabled='<%# IIf((gvInformeConsolidadoEjecucion.PageIndex + 1) = gvInformeConsolidadoEjecucion.PageCount, "false", "true")%>'
                                    SkinID="paging">Ultimo »</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </PagerTemplate>
        </asp:GridView>

    </div>
    <div>
        <asp:GridView ID="gvInformeConsolidadoEjecucionDetalle" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
            DataKeyNames="PersonaId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
            <PagerStyle CssClass="headerfooter ui-toolbar" />
            <SelectedRowStyle CssClass="SelectedRow" />
            <AlternatingRowStyle CssClass="odd" />
            <Columns>
                <asp:BoundField DataField="Nombres" HeaderText="Nombres" DataFormatString="{0:g}" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha"  />
                <asp:BoundField DataField="Actividad" HeaderText="Actividad"  />
                <asp:BoundField DataField="TotalHoras" HeaderText="Total Horas" />
                <asp:BoundField DataField="HorasProductivas" HeaderText="Horas Productivas" />
                <asp:BoundField DataField="PorcentajeHorasProductivas" HeaderText="% Horas Productivas" />
                <asp:BoundField DataField="HorasNoProductivas" HeaderText="Horas No Productivas" />
                <asp:BoundField DataField="PorcentajeHorasNoProductivas" HeaderText="% Horas No Productivas" />
                <asp:BoundField DataField="HorasOtros" HeaderText="Horas Otros" />
                <asp:BoundField DataField="PorcentajeHorasOtros" HeaderText="% Horas Otros" />
                <asp:BoundField DataField="HorasReproceso" HeaderText="Horas Reproceso" />
                <asp:BoundField DataField="PorcentajeHorasReproceso" HeaderText="% Horas Reproceso" />
            </Columns>
            <PagerTemplate>
                <div class="pagingButtons">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                    Enabled='<%# IIf(gvInformeConsolidadoEjecucion.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Enabled='<%# IIf(gvInformeConsolidadoEjecucion.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                            </td>
                            <td>
                                <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                    Enabled='<%# IIf((gvInformeConsolidadoEjecucion.PageIndex + 1) = gvInformeConsolidadoEjecucion.PageCount, "false", "true")%>'
                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                    Enabled='<%# IIf((gvInformeConsolidadoEjecucion.PageIndex + 1) = gvInformeConsolidadoEjecucion.PageCount, "false", "true")%>'
                                    SkinID="paging">Ultimo »</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </PagerTemplate>
        </asp:GridView>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
