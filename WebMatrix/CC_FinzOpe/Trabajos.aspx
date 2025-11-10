<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_F.master"
    CodeBehind="Trabajos.aspx.vb" Inherits="WebMatrix.TrabajosFI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
          function (value, element) {
              return this.optional(element) ||
                ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
          }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();

        }

        $(document).ready(function () {
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

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <li>
        <asp:LinkButton Text="Avance de Campo" PostBackUrl="~/RP_Reportes/AvanceDeCampo.aspx"
            runat="server" ID="lkbAvanceDeCampo"></asp:LinkButton></li>
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
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Trabajos
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Buscar por</label>
                                <asp:TextBox ID="txtID" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtJobBook" placeholder="JobBook" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtNombreTrabajo" placeholder="Nombre Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtNoPropuesta" placeholder="No. Propuesta" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="false"
                            PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="COEAsignado" HeaderText="OMPAsignado">
                                                                   
                                <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                                                   
                                <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                                <asp:BoundField DataField="ObservacionCOE" HeaderText="ObservacionOMP">
                                    <ItemStyle HorizontalAlign="Justify" Wrap="True" Width="100px" Height="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Presupuestos" ImageUrl="~/Images/find_16.png" Text="Actualizar"
                                            ToolTip="Ir a presupuestos" />
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
                 <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Información del trabajo
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:Panel ID="pnlPreguntas" runat="server" Visible="false">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        Cerradas
                                    </td>
                                    <td>
                                        Cerradas multiples
                                    </td>
                                    <td>
                                        Abiertas
                                    </td>
                                    <td>
                                        Abiertas multiples
                                    </td>
                                    <td>
                                        Otros
                                    </td>
                                    <td>
                                        Demograficos
                                    </td>
                                    <td>
                                        Paginas
                                    </td>
                                    <td>
                                        Observacion
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Propuestas
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CerradasProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CerradasMultProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AbiertasProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AbiertasMultProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="OtrosProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="DemoProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="PagProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Real
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CerradasReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CerradasMultReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AbiertasReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AbiertasMultReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="OtrosReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="DemoReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="PagReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ObsReal" runat="server" Width="80px" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:GridView ID="GvConteos" runat="server" Width="100%" AutoGenerateColumns="false"
                                        PageSize="5" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="CerradasRU" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="CerradasRU" HeaderText="CerradasRU" />
                                            <asp:BoundField DataField="CerradasRM" HeaderText="CerradasRM" />
                                            <asp:BoundField DataField="Abiertas" HeaderText="Abiertas" />
                                            <asp:BoundField DataField="AbiertasMul" HeaderText="AbiertasMul" />
                                            <asp:BoundField DataField="Otros" HeaderText="Otros" />
                                            <asp:BoundField DataField="Demograficos" HeaderText="Demograficos" />
                                            <asp:BoundField DataField="Paginas" HeaderText="Paginas" />
                                            <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                            </Columns>


                                    </asp:GridView>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <asp:Button ID="btnGuardarPreguntas" runat="server" Text="Guardar conteo" CssClass="button" />
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
