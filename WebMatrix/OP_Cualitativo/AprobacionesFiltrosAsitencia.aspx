<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="AprobacionesFiltrosAsitencia.aspx.vb" Inherits="WebMatrix.AprobacionesFiltrosAsitencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <link rel="stylesheet" href="../Styles/Filtros.css" type="text/css" />
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

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });

            $('#DetalleReclutamiento').dialog(
          {
              modal: true,
              autoOpen: false,
              title: "Detalle Filtro Reclutamiento",
              width: "1000px",
              buttons: {
                  Ok: function () {
                      $(this).dialog("close");
                  }
              }

          });


            $('#DetalleReclutamiento').parent().appendTo("form");

            $('#AprobacionFiltros').dialog(
           {
               modal: true,
               autoOpen: false,
               title: "Aprobación de Filtros",
               width: "1000px"

           });


            $('#AprobacionFiltros').parent().appendTo("form");


            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();
        });

        function MostrarDetalleReclutamiento() {
            $("#DetalleReclutamiento").dialog("option", "width", 800);
            $("#DetalleReclutamiento").dialog("option", "height", 600);
            $('#DetalleReclutamiento').dialog("open");
        }

        function CerrarDetalleReclutamiento() {
            $('#DetalleReclutamiento').dialog("close");
        }

        function MostrarAprobacionFiltros() {
            $("#AprobacionFiltros").dialog("option", "width", 800);
            $("#AprobacionFiltros").dialog("option", "height", 600);
            $('#AprobacionFiltros').dialog("open");
        }

        function CerrarAprobacionFiltros() {
            $('#AprobacionFiltros').dialog("close");
        }


        function MostrarObservacion() {
            $('#GuardarObservacion').dialog("open");
        }

        function CerrarObservacion() {
            $('#GuardarObservacion').dialog("close");
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

    <br />
    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver a Trabajos"></asp:LinkButton>

    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>

            <p style="color: White;">Respuestas Filtros Asistencia</p>
            <div class="actions">
                <div class="actions">
                    <asp:Label ID="lblTextoTrabajo" runat="server" Text="Trabajo:" ForeColor="White" Font-Bold="True"></asp:Label>
                    <asp:Label ID="lblTrabajo" runat="server" ForeColor="White" Font-Bold="True"></asp:Label>
                    <br />
                    <br />
                </div>
                <asp:HiddenField ID="hfIdProyecto" runat="server" />
                <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                <asp:HiddenField ID="hfIdFiltro" runat="server" Value="0" />
                <asp:HiddenField ID="hfIdFiltroReclutamiento" runat="server" Value="0" />
                <asp:HiddenField ID="hfIdRespuesta" runat="server" />
                <asp:HiddenField ID="hfIdRespuestaReclutamiento" runat="server" />
                <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
                <asp:HiddenField ID="hfPY" runat="server" />
                <label style="color: white; font-size: 18px">Información Filtros Asistencia</label>
                <br />
                <br />
                <div>
                    <asp:ImageButton ID="btnImgExportarInforme" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" />
                </div>
                <p style="color: White;">Filtros Reclutamiento</p>
                <asp:GridView ID="gvRespuestas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id,IdFiltro,IdEstado" AllowPaging="True" EmptyDataText="No existen Filtros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:BoundField DataField="Barrio" HeaderText="Barrio" />
                        <asp:BoundField DataField="Edad" HeaderText="Edad" />
                        <asp:BoundField DataField="Estrato" HeaderText="Estrato" />
                        <asp:BoundField DataField="Reclutador" HeaderText="Reclutador" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDetalle" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Detalle" ImageUrl="~/Images/select_16.png" Text="Detalle" ToolTip="Detalle" OnClientClick="MostrarDetalleReclutamiento()" />
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
                                            Enabled='<%# IIf(gvRespuestas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIf(gvRespuestas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvRespuestas.PageIndex + 1%>-<%= gvRespuestas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIf((gvRespuestas.PageIndex + 1) = gvRespuestas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIf((gvRespuestas.PageIndex + 1) = gvRespuestas.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>


            <div class="actions">
                <p style="color: White;">Filtros Asistencia</p>
                <asp:GridView ID="gvFiltro" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id,IdFiltro,IdEstado" AllowPaging="True" EmptyDataText="No existen Filtros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:BoundField DataField="Barrio" HeaderText="Barrio" />
                        <asp:BoundField DataField="Edad" HeaderText="Edad" />
                        <asp:BoundField DataField="Estrato" HeaderText="Estrato" />
                        <asp:BoundField DataField="Reclutador" HeaderText="Reclutador" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:TemplateField HeaderText="Aprobación" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgAprobacion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Aprobacion" ImageUrl="~/Images/select_16.png" Text="Aprobación" ToolTip="Aprobación" OnClientClick="MostrarAprobacionFiltros()" />
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
                                            Enabled='<%# IIf(gvRespuestas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIf(gvRespuestas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvRespuestas.PageIndex + 1%>-<%= gvRespuestas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIf((gvRespuestas.PageIndex + 1) = gvRespuestas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIf((gvRespuestas.PageIndex + 1) = gvRespuestas.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="DetalleReclutamiento">
        <asp:UpdatePanel ID="upDetalleReclutamiento" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlReclutamiento" runat="server" CssClass="actions">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="AprobacionFiltros">
        <asp:UpdatePanel ID="upAprobacionFiltros" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlFiltros" runat="server" CssClass="actions">
                </asp:Panel>
                <asp:Panel ID="pnlAprobar" runat="server" CssClass="actions">
                    <asp:TextBox ID="txtComentarios" Width="300px" runat="server" placeholder="Agregue sus comentarios aquí" TextMode="MultiLine"></asp:TextBox>
                    <asp:Button ID="btnAprobar" Text="Aprobar" runat="server" OnClientClick="CerrarAprobacionFiltros()" />
                    &nbsp;
                    <asp:Button ID="btnNoAprobar" Text="No Aprobar" runat="server" OnClientClick="CerrarAprobacionFiltros()" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <asp:UpdatePanel runat="server" ID="upReporteFiltros" ChildrenAsTriggers="false" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <asp:GridView ID="gvReporteFiltros" runat="server" Width="100%" AutoGenerateColumns="true"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar" AllowPaging="True"
                EmptyDataText="No existen Filtros para mostrar" DataSourceID="SqlDataSource1">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />

                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        Enabled='<%# IIf(gvReporteFiltros.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIf(gvReporteFiltros.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvReporteFiltros.PageIndex + 1%>-<%= gvReporteFiltros.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIf((gvReporteFiltros.PageIndex + 1) = gvReporteFiltros.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIf((gvReporteFiltros.PageIndex + 1) = gvReporteFiltros.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>

            </asp:GridView>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" SelectCommand="REP_OP_Respuestas_Filtro" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfIdFiltro" Name="IdFiltro" PropertyName="Value" Type="Int64" />
                </SelectParameters>
            </asp:SqlDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>





    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
