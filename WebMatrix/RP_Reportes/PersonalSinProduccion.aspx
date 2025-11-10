<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master"
    CodeBehind="PersonalSinProduccion.aspx.vb" Inherits="WebMatrix.REP_PersonalSinProduccion" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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

          }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Informe de personal sin producción
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" id="accordion">
                <div id="accordion0">
                    <h3>
                                Personal sin producción<asp:HiddenField ID="hfidTrabajo" runat="server" />
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <label>Seleccione la fecha a consultar</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <ñabeñ>Seleccione el cargo</ñabeñ>
                            <asp:DropDownList ID="ddlCargo" runat="server">
                                <asp:ListItem Text="Supervisor" Value="12"></asp:ListItem>
                                <asp:ListItem Text="Encuestador" Value="13"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="clear"></div>
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                            <br /><br />
                            <div style="text-align: center">
                                <asp:GridView ID="gvDatos" runat="server" Width="70%" AutoGenerateColumns="False" DataKeyNames="ID"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="Cedula" />
                                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                    <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                                    <asp:BoundField DataField="NombreCiudad" HeaderText="Ciudad" />
                                </Columns>
                            </asp:GridView>
                                <br />
                            </div>
                        </div>
                    </div>
                    <%--items--%>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
