<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="AjustesCostosMystery.aspx.vb" Inherits="WebMatrix.AjustesCostosMystery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });
            $("#<%= txtPorcentajeGM.ClientId %>").mask("9.999999999");

            $.validator.addMethod('selectNone2',
          function (value, element) {
              return this.optional(element) ||
                ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
          }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#PresupuestosAsignadosXEstudio').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Presupuestos asignados",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();
        });
        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }

        $(function () {
            $("#gvReporte").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#btnCerrarTrabajo").click(function () {
                $("#gvReporte").dialog("open");
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <p>
        cerrar
    </p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>AJUSTES COSTOS MYSTERY SHOPPER</a>
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

    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                LISTADO DE COSTOS DE ACTIVIDADES
                            </label>
                        </a>
                    </h3>
                    <div id="accordion1">

                        <div class="block">
                            <asp:HiddenField ID="hfIdPropuesta" runat="server" />
                            <asp:HiddenField ID="hfIdAlternativa" runat="server" />
                            <asp:HiddenField ID="hfActualizar" runat="server" Value="0" />

                            <asp:Panel ID="pnlNewMuestra" runat="server">

                                <asp:GridView ID="gvActividades" runat="server" EmptyDataText="No se encuentran registros disponibles."
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="ActCodigo" AllowPaging="False" AutoGenerateColumns="false">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Actividad" HeaderText="Actividad" />
                                        <asp:TemplateField HeaderText="Valor Actividad" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValor" runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.CaCosto") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgQuitar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Quitar" ImageUrl="~/Images/delete_16.png" Text="Quitar" ToolTip="Quitar Actividad" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <h4>Agregar más Actividades</h4>
                                <div class="form_left">
                                    <fieldset>
                                        <label>Actividad</label>
                                        <asp:DropDownList ID="ddlActividades" runat="server"></asp:DropDownList>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>Valor Actividad</label>
                                        <asp:TextBox ID="txtValorAct" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <asp:Button ID="btnAddActividad" runat="server" Text="Agregar" />
                                    </fieldset>
                                </div>
                                <br />
                                <div class="form_left">
                                    <fieldset>
                                       <label>______________________________________________________________________________________________________________________________</label>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            GM OPS:
                                        </label>
                                        <asp:TextBox ID="txtGMOPS" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Subcontratadas Internas:
                                        </label>
                                        <asp:TextBox ID="txtSubContInternas" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Costo Operación/Venta Operación:
                                        </label>
                                        <asp:TextBox ID="txtCostoOperacion" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Subcontratadas Externas:
                                        </label>
                                        <asp:TextBox ID="txtSubContExternas" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Valor Venta:
                                        </label>
                                        <asp:TextBox ID="txtVrVenta" runat="server" TextMode="Number"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Porcentaje de GM (Escribir en decimales (0.4545)):
                                        </label>
                                        <asp:TextBox ID="txtPorcentajeGM" runat="server"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                                </div>
                            </asp:Panel>
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
