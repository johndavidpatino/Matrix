<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="AsignacionJBI.aspx.vb" Inherits="WebMatrix.AsignacionJBI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });
            $("#<%= txtJobBook.ClientId %>").mask("99-999999-99-99");
            $("#<%= txtJobBookInt.ClientId %>").mask("99-999999-99-99-99");

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#GerenteAsignar').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Escriba EL JBI",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });

            loadPlugins();
        });
        function MostrarGerentesProyectos() {
            $('#GerenteAsignar').dialog("open");
        }


        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Asignación JBI</a>
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
            <div>
                <div id="accordion0">
                    <h3 style="float: left; text-align: left;">
                        <a>
                            <label>
                                Presupuestos para asignación de JBI<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="spacer">
                                <label>
                                   Gerencia Operaciones 
                                    </label>
                                <asp:DropDownList ID="ddlGrupoUnidades" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>

                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Idpresup,idpropuesta,alternativa,metcodigo,parnacional,IDProy" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="NombreProyecto" HeaderText="Proyecto" />
                                <asp:BoundField DataField="FechaInicioCampo" HeaderText="Fecha Estimada Inicio Campo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Metodología" HeaderText="Metodología" />
                                <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                <asp:BoundField DataField="IdPropuesta" HeaderText="IdPropuesta" />
                                <asp:BoundField DataField="Alternativa" HeaderText="Alt" />
                                <asp:BoundField DataField="GerenteCuentas" HeaderText="Gerente Cuentas" />
                                <asp:TemplateField HeaderText="Asignar JBI" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAsignarJBI" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/save_16.png" Text="Asignar"
                                            ToolTip="Asignar JBI" OnClientClick="MostrarGerentesProyectos()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgVerPresupuestos" runat="server" CausesValidation="False" CommandName="Presupuestos"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/Select_16.png"
                                            Text="Presupuestos" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="GerenteAsignar">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <label>Escriba el número del JobBook Interno</label>
                    <asp:TextBox ID="txtJobBook" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtJobBookInt" runat="server" Visible="false"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hfidPropuesta" runat="server" />
                <asp:HiddenField ID="hfParAlternativa" runat="server" />
                <asp:HiddenField ID="hfMetCodigo" runat="server" />
                <asp:HiddenField ID="hfParNacional" runat="server" />
                <asp:HiddenField ID="hfProyecto" runat="server" />
                <asp:HiddenField ID="hfJobBook" runat="server" />
                <div class="spacer">
                        <asp:Button ID="btnUpdate" runat="server" Text="Asignar JobBook Interno" OnClientClick="$('#GerenteAsignar').dialog('close');" />
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
