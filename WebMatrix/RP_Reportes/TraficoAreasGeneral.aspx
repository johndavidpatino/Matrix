<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="TraficoAreasGeneral.aspx.vb" Inherits="WebMatrix.TraficoAreasGeneral" %>

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
    <a>Trafico General de Encuestas en Operaciones</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Tráfico<asp:HiddenField ID="hfIdTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:DropDownList ID="ddlAreasTrafico" runat="server">
                                <asp:ListItem Text="-- Seleccione un área --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="RMC" Value="38"></asp:ListItem>
                                <asp:ListItem Text="Crítica" Value="28"></asp:ListItem>
                                <asp:ListItem Text="Verificación" Value="20"></asp:ListItem>
                                <asp:ListItem Text="Captura" Value="21"></asp:ListItem>
                            </asp:DropDownList><br />
                            <a>Seleccione la fecha Inicio</a>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <a>Seleccione la fecha Fin</a>
                            <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                            <br /><br /><br />
                            <asp:GridView ID="gvTraficoAreas" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable2" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="RECIBIDO" HeaderText="Recibido" />
                                    <asp:BoundField DataField="ACUMRECIBIDO" HeaderText="Acumulado Recibido" />
                                    <asp:BoundField DataField="ENVIADO" HeaderText="Enviado"  />
                                    <asp:BoundField DataField="ACUMENVIADO" HeaderText="Acumulado Enviado"  />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Trabajos<asp:HiddenField ID="HiddenField1" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            
                            <fieldset>
                                <label>
                                    </label>
                                <a>Unidades</a><asp:DropDownList ID="ddlGrupoUnidades" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="FechaInicioCampo" HeaderText="Fecha Inicio"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFinalCampo" HeaderText="Fecha Final"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="COE" HeaderText="OMP" />
                                <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Ver avance" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgVerAvanceCampo" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Avance" ImageUrl="~/Images/select_16.png" Text="Ver Avance"
                                            ToolTip="Ver avance" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Seguimiento" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrProject" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Project" ImageUrl="~/Images/calendar_24.png" Text="Actualizar" ToolTip="Ir a Gantt de Planeación y Ejecución" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
