<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="EstimacionProduccion.aspx.vb" Inherits="WebMatrix.EstimacionProduccion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            validationForm();

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });


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
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Estimación de Producción
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        &nbsp;<asp:Button ID="btnVolver" runat="server" Text="Volver" />
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="idEstim" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="FechaEstimacion" HeaderText="Fecha Estimación" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Usuario" HeaderText="Estimación realizada por" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                <asp:CheckBoxField DataField="Activa" HeaderText="Activa" />
                                <asp:TemplateField HeaderText="Ver Planeación" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Ver planeación" />
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
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span>
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
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Distribución de Estimación</label></a></h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <asp:GridView ID="gvEstimacion" runat="server" Width="30%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha Estimación" DataFormatString="{0:dddd d 'de' MMMM}" />
                                    <asp:TemplateField HeaderText="Cantidad estimada" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbCantidadEstimada" Enabled="false" AutoCompleteType="Disabled"
                                                Width="50px" runat="server" EnableViewState="true" Text='<%#  DataBinder.Eval(Container, "DataItem.Cantidad") %>'></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="tbCantidadEstimada_FilteredTextBoxExtender" FilterType="Numbers"
                                                runat="server" Enabled="True" TargetControlID="tbCantidadEstimada">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" Wrap="False" />
                                    </asp:TemplateField>
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
                                                    <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span>
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
                            <asp:Button ID="btnActualizarEstimacion" runat="server" Text="Guardar" />
                            &nbsp;
                            <asp:Button ID="btnActivarEstimacion" runat="server" Text="Activar" />
                            &nbsp;
                            <asp:Button ID="btnNuevo" runat="server" Text="Nueva estimación" />
                            &nbsp;
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                        </fieldset>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                AGREGAR NUEVA ESTIMACIÓN
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdEstimacion" runat="server" />
                        <asp:HiddenField ID="hfdiTrabajo" runat="server" />
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Observaciones</label>
                                        <asp:TextBox ID="tbObservaciones" MaxLength="150" runat="server"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                </div>
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
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGenerarPlaneacion" runat="server" Text="Generar Planeación" CssClass="buttonText buttonSave corner-all" />
                                            &nbsp;
                                            <asp:Button ID="btnCAncel" runat="server" Text="Cancelar" CssClass="buttonText buttonSave corner-all" />
                                        </fieldset>
                                    </div>
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
