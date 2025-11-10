<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="ReporteTablets.aspx.vb" Inherits="WebMatrix.ReporteTablets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
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

            $("#<%= txtFechaInicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientId %>").datepicker({
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



            loadPlugins();
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <%-- <p>
        OK<br />
    </p>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Reporte Uso de Tablets</a>
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
    <asp:UpdatePanel ID="upBuscar" runat="server">
        <ContentTemplate>
            <div style="width: 45%; float: left; border: double">
                <fieldset class="validationGroup">

                    <div style="float: left; margin-right: 5px">
                        <fieldset>
                            <label>
                                Fecha Inicio
                            </label>
                            <asp:TextBox ID="txtFechaInicio" runat="server"> </asp:TextBox>
                        </fieldset>
                    </div>
                    <div style="float: left">
                        <fieldset>
                            <label>
                                Fecha Fin
                            </label>
                            <asp:TextBox ID="txtFechaFin" runat="server"> </asp:TextBox>
                        </fieldset>
                    </div>
                </fieldset>
            </div>
            <div class="actions">
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upReporteTablets" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <label>Reporte Agrupado</label>
            <asp:ImageButton ID="btnImgExportarAgrupado" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" Visible="false" />
            <asp:GridView ID="gvAgrupado" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="TabletsDisponibles" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="TabletsDisponibles" HeaderText="Tablets Disponibles" />
                    <asp:BoundField DataField="TabletsConSIM" HeaderText="Tablets Con SIM" />
                    <asp:BoundField DataField="TabletsEnCampo" HeaderText="Tablets En Campo" />
                    <asp:BoundField DataField="TabletConSIM_TabletDisponible" HeaderText="Tablet Con SIM_Tablet Disponible" />
                    <asp:BoundField DataField="TabletConProduccion_TabletDisponible" HeaderText="Tablet Con Produccion_Tablet Disponible" />
                    <asp:BoundField DataField="TabletConProduccion_TabletConSIM" HeaderText="Tablet Con Produccion_Tablet Con SIM" />
                    <asp:BoundField DataField="TabletConProduccion_TabletEnCampo" HeaderText="Tablet Con Produccion_Tablet En Campo" />
                    <asp:BoundField DataField="DiasCalendario" HeaderText="Dias Calendario" />
                    <asp:BoundField DataField="DiasLaborales" HeaderText="Dias Laborales" />
                    <asp:BoundField DataField="DiasTabletDisponiblesCalendario" HeaderText="Dias Tablet Disponibles Calendario" />
                    <asp:BoundField DataField="DiasTabletDisponiblesLaborales" HeaderText="Dias Tablet Disponibles Laborales" />
                    <asp:BoundField DataField="TabletsConProduccion" HeaderText="Tablets Con Produccion" />
                    <asp:BoundField DataField="TotalDiasTabletConProduccion" HeaderText="Total Dias Tablet Con Produccion" />
                    <asp:BoundField DataField="CantidadEncuestas" HeaderText="Cantidad Encuestas" />
                    <asp:BoundField DataField="PromedioProduccionTabletsDisponibles" HeaderText="Promedio Produccion Tablets Disponibles" />
                    <asp:BoundField DataField="PromedioProduccionTabletsEnCampo" HeaderText="Promedio Produccion Tablets En Campo" />
                    <asp:BoundField DataField="PromedioProduccionTabletsConProduccion" HeaderText="Promedio Produccion Tablets Con Produccion" />
                </Columns>
                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        Enabled='<%# IIf(gvAgrupado.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIf(gvAgrupado.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvAgrupado.PageIndex + 1%>-<%= gvAgrupado.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIf((gvAgrupado.PageIndex + 1) = gvAgrupado.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIf((gvAgrupado.PageIndex + 1) = gvAgrupado.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>

            <div></div>

            <label>Reporte Diario</label>
            <asp:ImageButton ID="btnImgExportarDiario" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" Visible="false" />
            <asp:GridView ID="gvDiario" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="FECHA" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="FECHA" HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="TabletsDisponibles" HeaderText="Tablets Disponibles" />
                    <asp:BoundField DataField="TabletsConSIM" HeaderText="Tablets Con SIM" />
                    <asp:BoundField DataField="TabletsEnCampo" HeaderText="Tablets En Campo" />
                    <asp:BoundField DataField="TabletConSIM_TabletDisponible" HeaderText="Tablet Con SIM_Tablet Disponible" />
                    <asp:BoundField DataField="TabletConProduccion_TabletDisponible" HeaderText="Tablet Con Produccion_Tablet Disponible" />
                    <asp:BoundField DataField="TabletConProduccion_TabletConSIM" HeaderText="Tablet Con Produccion_Tablet Con SIM" />
                    <asp:BoundField DataField="TabletConProduccion_TabletEnCampo" HeaderText="Tablet Con Produccion_Tablet En Campo" />
                    <asp:BoundField DataField="DiasCalendario" HeaderText="Dias Calendario" />
                    <asp:BoundField DataField="DiasLaborales" HeaderText="Dias Laborales" />
                    <asp:BoundField DataField="DiasTabletDisponiblesCalendario" HeaderText="Dias Tablet Disponibles Calendario" />
                    <asp:BoundField DataField="DiasTabletDisponiblesLaborales" HeaderText="Dias Tablet Disponibles Laborales" />
                    <asp:BoundField DataField="TabletsConProduccion" HeaderText="Tablets Con Produccion" />
                    <asp:BoundField DataField="TotalDiasTabletConProduccion" HeaderText="Total Dias Tablet Con Produccion" />
                    <asp:BoundField DataField="CantidadEncuestas" HeaderText="Cantidad Encuestas" />
                    <asp:BoundField DataField="PromedioProduccionTabletsDisponibles" HeaderText="Promedio Produccion Tablets Disponibles" />
                    <asp:BoundField DataField="PromedioProduccionTabletsEnCampo" HeaderText="Promedio Produccion Tablets En Campo" />
                    <asp:BoundField DataField="PromedioProduccionTabletsConProduccion" HeaderText="Promedio Produccion Tablets Con Produccion" />

                </Columns>
                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        Enabled='<%# IIf(gvDiario.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIf(gvDiario.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvDiario.PageIndex + 1%>-<%= gvDiario.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIf((gvDiario.PageIndex + 1) = gvDiario.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIf((gvDiario.PageIndex + 1) = gvDiario.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
