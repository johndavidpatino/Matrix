<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="EstudiosXEntregarCCT.aspx.vb" Inherits="WebMatrix.EstudiosXEntregarCCT" %>

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

            $("#<%= txtFechaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


            $('#GerenteAsignar').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Actualización de datos",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });


        }

        function MostrarGerentesProyectos() {
            $('#GerenteAsignar').dialog("open");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Listado de Estudios sin entregar a Operaciones</a>
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
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Listado de Estudios<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Unidad</label>
                                <asp:DropDownList ID="ddlUnidades" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </fieldset>
                        </div>
                        <div class="form_left">
                        <fieldset>
                                <label>
                                    Gerente de Cuentas</label>
                                <asp:DropDownList ID="ddlGerenteCuentas" runat="server">
                                </asp:DropDownList>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                    <label>
                                        Gerencias OP</label>
                                    <asp:DropDownList ID="ddlGerencias" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label> </label>
                            </fieldset>
                            <asp:Button ID="btnBuscar" runat="server" Text="Ver estudios" />
                        </div>
                        <div class="actions">
                            <asp:HiddenField ID="hfEstudio" runat="server" />
                            <asp:GridView ID="gvEstudios" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar" DataKeyNames="Id" >
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="NoPropuesta" HeaderText="No. Propuesta" />
                                    <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="FechaTerminacion" HeaderText="Fecha Terminación" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="FechaInicioCampo" HeaderText="Fecha Campo Estimada" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="GerenteCuentas" HeaderText="GerenteCuentas" />
                                    <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                    <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrProject" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Actualizar fecha estimada de campo y probabilidad de aprobación" OnClientClick="MostrarGerentesProyectos()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <%--items--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="GerenteAsignar">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
            <asp:HiddenField ID="hfEstudio2" runat="server" />
                <div class="form_left">
                                            <fieldset>
                                                <label>
                                                    Fecha de Inicio de Campo tentativa
                                                </label>
                                                <asp:TextBox ID="txtFechaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                            </fieldset>
                                            </div>
                                       
                <div class="actions">
                    <div class="form_rigth">
                        <asp:Button ID="btnUpdate" runat="server" Text="Actualizar estudio" OnClientClick="$('#GerenteAsignar').dialog('close');" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
