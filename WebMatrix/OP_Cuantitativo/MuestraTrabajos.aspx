<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="MuestraTrabajos.aspx.vb" Inherits="WebMatrix.MuestraTrabajos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();


            validationForm();

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            function bindPickerIni() {
                $("input[type=text][id*=txtFInicio]").datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
                });
            }

            function bindPickerFin() {
                $("input[type=text][id*=txtFFin]").datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
                });
            }

            $("#<%= txtFechaInicioGeneral.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicioGeneral.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFinGeneral.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinGeneral.ClientId %>").datepicker({
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <asp:Label ID="txtNombreTrabajo" runat="server"></asp:Label>
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:HiddenField ID="hfidTrabajo" runat="server" />
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Muestra Por Ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:Panel ID="pnlEstimacion" runat="server" Visible="false">
                            <asp:Panel ID="pnlFechasGenerales" runat="server">
                                <div class="form_left">
                                    <fieldset>
                                        <label>Fecha Inicio</label>
                                        <asp:TextBox ID="txtFechaInicioGeneral" runat="server" CssClass="required bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>Fecha Fin</label>
                                        <asp:TextBox ID="txtFechaFinGeneral" runat="server" CssClass="required bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <asp:Button ID="btnCambiarGeneral" runat="server" CssClass="button" Text="Realizar cambio" />
                                </div>
                            </asp:Panel>
                            <div class="clear"></div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    ¿Festivos?</label>
                                <asp:CheckBox ID="chbFestivosExcluir" Text="Excluir Festivos" runat="server" />
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Días a estimar</label>
                                <asp:CheckBoxList ID="chbDias" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Lunes" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Martes" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Miércoles" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Jueves" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Viernes" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Sábado" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Domingo" Value="1"></asp:ListItem>
                                </asp:CheckBoxList>
                            </fieldset>
                        </div>
                        </asp:Panel>
                        <asp:GridView ID="gvMuestra" runat="server" Width="100%" AutoGenerateColumns="False"
                            PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="idMuestra" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:TemplateField HeaderText="Inicio Campo" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFInicio" runat="server" Text='<%#  Eval("FechaInicio", "{0:dd/MM/yy}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fin Campo" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFFin" runat="server" Text='<%#  Eval("FechaFin", "{0:dd/MM/yy}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Actualizar"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/select_16.png"
                                            OnClientClick="return confirm('Esta seguro de actualizar este registro ?');"
                                            Text="Actualizar" />
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
                                                    Enabled='<%# IIF(gvMuestra.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvMuestra.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvMuestra.PageIndex + 1%>-<%= gvMuestra.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvMuestra.PageIndex +1) = gvMuestra.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvMuestra.PageIndex +1) = gvMuestra.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <div class="actions">
                            <asp:Button ID="btnVolver" runat="server" Text="Volver" />
                        </div>
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
