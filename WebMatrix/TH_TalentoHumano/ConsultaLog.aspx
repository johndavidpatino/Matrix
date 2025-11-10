<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master"
    CodeBehind="ConsultaLog.aspx.vb" Inherits="WebMatrix.ConsultaLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
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

            $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinalizacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


        };
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
                                Consulta Personas
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
                                <label>
                                    Cedula</label>
                                <asp:TextBox ID="txtcedula" runat="server"></asp:TextBox>
                            </fieldset>
                            <fieldset>
                                <label>Tipo cambio</label>
                                <asp:DropDownList ID="ddlTipoCambio" runat="server">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="TipoContrato" Value="TipoContrato"></asp:ListItem>
                                    <asp:ListItem Text="Motivo Retiro" Value="Motivo Retiro"></asp:ListItem>
                                    <asp:ListItem Text="Estado" Value="Estado"></asp:ListItem>
                                    <asp:ListItem Text="Contratista" Value="Contratista"></asp:ListItem>
                                </asp:DropDownList>
                            </fieldset>
                            <fieldset>
                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                            </fieldset>

                            <fieldset class="validationGroup">
                                <div>
                                    <asp:GridView ID="gvPersonas" runat="server" Width="130%" AutoGenerateColumns="False"
                                        PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="PersonaId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="PersonaId" HeaderText="Persona Id" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="Cambio" HeaderText="Cambio" />
                                            <asp:BoundField DataField="TextoValorOriginal" HeaderText="ValorOriginal" />
                                            <asp:BoundField DataField="TextoValorNuevo" HeaderText="ValorNuevo" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="UsuarioId" HeaderText="Usuario Id" />
                                            <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre Usuario" />

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
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
