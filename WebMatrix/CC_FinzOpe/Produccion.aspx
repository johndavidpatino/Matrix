<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master"
    CodeBehind="Produccion.aspx.vb" Inherits="WebMatrix.Produccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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

            $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinalizacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });



        });
        $(document).ready(function () {

            $('#CargarArchivo').dialog({
                modal: true,
                autoOpen: false,
                title: "Cargar Archivo",
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

            loadPlugins();
        });

        function Cargar() {
            $('#CargarArchivo').dialog("open");
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
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Produccion
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Fecha Inicial</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>

                            <fieldset>
                                <label>
                                    Fecha Final</label>
                                <asp:TextBox ID="txtFechaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>


                            <fieldset>
                                <asp:Button ID="btnSinPresupuesto" runat="server" Text="TrabajosSinPresupuesto"/>
                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                                <asp:Button ID="btnexportar" runat="server" Text="Exportar" />
                                <asp:Button ID="btnliquidar" runat="server" Text="Liquidar" />
                                <asp:Button ID="btnCuentas" runat="server" Text="Cuentas Cobro" />
                                <asp:Button ID="btnCargar" runat="server" Text="CargarArchivo"/>
                                <asp:Button ID="btnnomina" runat="server" Text="ArchivoNomina"/>
                                <asp:Button ID="BtnBonificacion" runat="server" Text="Bonificacion"/>
                                <asp:Button ID="BtnEliminar" runat="server" Text="EliminarCargue"/>
                                <%-- <asp:Button ID="btnCargar" runat="server" Text="CargarArchivo" OnClientClick="Cargar()" />--%>

                            </fieldset>
                            <fieldset class="validationGroup">
                                <div>
                                    <asp:GridView ID="gvProduccion" runat="server" Width="100%" AutoGenerateColumns="False"
                                        PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="PersonaId" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <%--
                                             <asp:BoundField DataField="Estudio" HeaderText="Estudio" />
                                            <asp:BoundField DataField="JobBook" HeaderText="JobBook">
                                                <ItemStyle VerticalAlign="Top" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" />
                                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:BoundField DataField="FechaCargue" HeaderText="FechaCargue" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="TipoContrataciónMatrix" HeaderText="TipoContrataciónMatrix">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CargoMatrix" HeaderText="CargoMatrix">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NSE" HeaderText="NSE Solo para GCTS">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MUESTRA" HeaderText="MUESTRA Solo para GCTS">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Orden" HeaderText="Orden">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Observacion" HeaderText="Observacion">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField> --%>

                                            <asp:BoundField DataField="PersonaId" HeaderText="PersonaId" />
                                            <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                            <asp:BoundField DataField="Cantida" HeaderText="Cantida" />
                                            <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                            <asp:BoundField DataField="CiudadId" HeaderText="CiudadId" />
                                            <asp:BoundField DataField="DiasTrabajados" HeaderText="DiasTrabajados" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                        </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="CargarArchivo">
        <asp:UpdatePanel ID="upCargar" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>


                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="btnPasarServidor" runat="server" Text="Pasar a servidor" />
                <asp:Button ID="btnCargarDatos" runat="server" Text="Cargar a la base" />





            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
        --%>
</asp:Content>
