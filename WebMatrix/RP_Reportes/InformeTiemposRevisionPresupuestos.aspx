<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master"
    CodeBehind="InformeTiemposRevisionPresupuestos.aspx.vb" Inherits="WebMatrix.REP_InformeTiemposRevisionPresupuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTerminacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTerminacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Informe de Revisión de Presupuestos</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <a>ADVERTENCIA: Los datos del resumen están dados en horas totales, no en horas hábiles</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" id="accordion">
                <div id="accordion0">
                    <h3>
                                Resumen<asp:HiddenField ID="hfidTrabajo" runat="server" />
                    </h3>
                    <div>
                        <div>
                            <label>Seleccione la fecha Inicio según envío a revisión</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <br />
                            <label>Seleccione la fecha Fin según envío a revisión</label>
                            <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                            <div class="spacer"></div>
                            <div>
                                <asp:GridView ID="gvResumen" runat="server" Width="70%" AutoGenerateColumns="False"
                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="GerenciaOP" HeaderText="Gerencia" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="Minimo" HeaderText="Minimo" />
                                    <asp:BoundField DataField="Maximo" HeaderText="Maximo" />
                                    <asp:BoundField DataField="Promedio" HeaderText="Promedio" />
                                </Columns>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <%--items--%>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvListado" runat="server" Width="70%" AutoGenerateColumns="False" 
                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Propuesta" HeaderText="Propuesta" />
                                    <asp:BoundField DataField="No" HeaderText="No" />
                                    <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                    <asp:BoundField DataField="Probabilidad" HeaderText="Probabilidad" />
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                    <asp:BoundField DataField="EnviadaPor" HeaderText="Enviada por" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                    <asp:BoundField DataField="FechaEnvio" HeaderText="Fecha Envío" />
                                    <asp:BoundField DataField="FechaRevision" HeaderText="Fecha Revisión" />
                                    <asp:BoundField DataField="HorasHabiles" HeaderText="Tiempo respuesta horas hábiles" />
                                    <asp:BoundField DataField="RevisadaPor" HeaderText="Revisada Por" />
                                    <asp:BoundField DataField="GerenciaOperaciones" HeaderText="Gerencia" />
                                </Columns>
                            </asp:GridView>
                            <div style="text-align:center"><asp:Button ID="btnExportar" runat="server" Text="Generar exportado" /></div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
