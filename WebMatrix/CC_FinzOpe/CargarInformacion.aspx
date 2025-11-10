<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="CargarInformacion.aspx.vb" Inherits="WebMatrix.CargarInformacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">
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

            $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinalizacion.ClientId %>").datepicker({
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
            loadPlugins();
        });
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
    <asp:UpdatePanel ID="upVista" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfTypeFile" runat="server" />
            <asp:HiddenField ID="hfNombreArchivo" runat="server" />
            <div class="form_left">
                <table style="width: 100%;">
                    <tr>
                        <td width="20%">
                            <label>Fecha Inicial</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <label>Fecha Final</label>
                            <asp:TextBox ID="txtFechaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                        </td>
                        <td width="50%">
                            <label>Tipo Cargue</label>
                            <asp:DropDownList ID="ddlTipoCargue" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="1">Por días</asp:ListItem>
                                <asp:ListItem Value="2">Por productividad</asp:ListItem>
                            </asp:DropDownList>
                            <label>Seleccione Tipo</label>
                            <asp:DropDownList ID="ddlTipo" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td width="10%">
                            <label>Solo Validar:</label>
                            <asp:CheckBox ID="chkSoloValida" runat="server" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td width="50%">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:Button ID="btnPasarServidor" runat="server" Text="Pasar a servidor" />
                        </td>
                        <td width="50%">
                            <asp:DropDownList ID="lsthojas" runat="server"></asp:DropDownList>
                            <asp:Button ID="btnCargarDatos" runat="server" Text="Cargar a la base" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td width="50%">
                            <asp:Button ID="btnActualizarDiasTrabajados" runat="server" Text="Actualizar días trabajados" />
                        </td>
                    </tr>
                </table>

                <div id="accordion">
                    <div id="accordion0">
                        <h3><a href="#">
                            <label>Cedulas con horas > 12</label></a></h3>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvHorasPorDia" runat="server" AllowPaging="False" AutoGenerateColumns="False" CssClass="displayTable"
                                        PageSize="10" Width="100%" EmptyDataText="No existen datos" AutoGenerateSelectButton="True">
                                        <AlternatingRowStyle VerticalAlign="Top" />
                                        <Columns>
                                            <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hlErrores" runat="server" Text="Descargar Archivo Errores" Visible="false" NavigateUrl="~/Files/ErroresCargue.xlsx"></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <table style="width: 100%;">
                    <tr>
                        <td width="25%">
                            <asp:Label ID="LblCantidad" runat="server"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="LblSuma" runat="server"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="LblIdInicial" runat="server"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="LblIdFinal" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
