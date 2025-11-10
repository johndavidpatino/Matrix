<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/IT_F.master"
    CodeBehind="AlmacenamientoDisco.aspx.vb" Inherits="WebMatrix.AlmacenamientoDisco" %>

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

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>ALMACENAMIENTO EN DISCO</a>
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
    <asp:LinkButton ID="lbtnVolver" Text="Volver" href="../IT/Default.aspx" runat="server"></asp:LinkButton>
    <br />
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
            <asp:HiddenField ID="hfIdMedio" runat="server" />
            Trabajos aún no cerrados en el centro de información
            <div id="trabajosCerrados">
                <asp:GridView ID="gvTrabajosCerrados" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="IdTrabajo" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                        <asp:BoundField DataField="GerenteProyectos" HeaderText="GerenteProyectos" />
                        <asp:BoundField DataField="NombreUnidad" HeaderText="NombreUnidad" />
                        <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                        <asp:TemplateField HeaderText="Centro Información" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgAlmacenar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Centroinformacion" ImageUrl="~/Images/Select_16.png" Text="Centroinformacion"
                                    ToolTip="Centroinformacion" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>

                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvTrabajosCerrados.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvTrabajosCerrados.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td><span class="pagingLinks">[<%= gvTrabajosCerrados.PageIndex + 1%>-<%= gvTrabajosCerrados.Rows.Count / gvTrabajosCerrados.PageSize%>]</span> </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvTrabajosCerrados.PageIndex + 1) >= (gvTrabajosCerrados.Rows.Count / gvTrabajosCerrados.PageSize), "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvTrabajosCerrados.PageIndex + 1) >= (gvTrabajosCerrados.Rows.Count / gvTrabajosCerrados.PageSize), "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
            <asp:Panel ID="pnlCierreActual" runat="server" Visible="false">
                <asp:Label ID="lblInfTrabajoActual1" runat="server" Text="Actualmente esta almacenando el trabajo "></asp:Label>
                <asp:Label ID="lblTrabajoActual" runat="server"></asp:Label>
                <asp:Panel ID="pnlMedios" runat="server" Visible="false">
                    <br />
                    <asp:Label ID="lblMsjMedios" runat="server" Text="Medios en los que se encuentra almacenado el trabajo actualmente" Visible="false"></asp:Label>
                    <fieldset>
                        <div class="form_right">
                            <asp:GridView ID="gvMedios" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IdMedio" HeaderText="IdMedio" />
                                    <asp:BoundField DataField="TipoAlmacenamiento" HeaderText="TipoAlmacenamiento" />
                                    <asp:BoundField DataField="Contiene" HeaderText="Contiene" />
                                    <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                    <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                                ToolTip="Actualizar" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                                <PagerTemplate>
                                    <div class="pagingButtons">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvMedios.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvMedios.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                </td>
                                                <td><span class="pagingLinks">[<%= gvMedios.PageIndex + 1%>-<%= gvMedios.PageCount%>]</span> </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvmedios.PageIndex + 1) = gvMedios.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvMedios.PageIndex + 1) = gvmedios.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </asp:Panel>
                <br />
                <br />
                <div style="text-align: right"><a>Paso 1 de 3 <a style="font-style: italic; color: White;">Obtiene el Id del DVD, Cinta u otro medio si no lo ha creado</a></div>

                <div class="block">
                    <fieldset class="validationGroup">
                        <div class="form_right">
                            <label>Tipo Almacenamiento </label>
                            <asp:DropDownList ID="ddlTipoAlmacenamiento" CssClass="mySpecificClass" runat="server">
                                <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="DVD" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Cinta" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblDescripAlmacena" runat="server" Text="Descripción general medio" AssociatedControlID="txtObservacion"></asp:Label>
                            <asp:TextBox ID="txtObservacion" runat="server" CssClass="textEntry" placeholder="Observacion" TextMode="MultiLine"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="btnadd" runat="server" Text="Obtener Id" CssClass="causesValidation" />
                            <br />
                            <br />
                            <asp:Label ID="lblMsgIdMedio" runat="server" Font-Italic="true" ForeColor="White" Text="El id asignado por Matrix para este medios es:" Visible="false"></asp:Label>
                            <asp:Label ID="lblIdMedio" CssClass="cambiocolor" runat="server" Visible="false"></asp:Label>
                        </div>
                    </fieldset>
                </div>

                <div style="text-align: right"><a>Paso 2 de 3 <a style="font-style: italic; color: White;">Ingrese el contenido del medio</a></div>

                <fieldset class="validationGroup">
                    <div class="actions">

                        <div class="form_rigth">
                            <label>
                                Id Medio Almacenamiento</label>
                            <asp:DropDownList ID="ddlIdMaestro" CssClass="mySpecificClass" AutoPostBack="true" runat="server"></asp:DropDownList>
                            <asp:Label ID="lblContiene" runat="server" Text="Contenido del medio" AssociatedControlID="txtContiene"></asp:Label>
                            <asp:TextBox ID="txtContiene" runat="server" CssClass="required text textEntry" placeholder="Contenido" TextMode="MultiLine"></asp:TextBox>

                            <asp:Label ID="lblObservacion" runat="server" Text="Observación general" AssociatedControlID="txtObservacionDetalle"></asp:Label>
                            <asp:TextBox ID="txtObservacionDetalle" runat="server" CssClass="textEntry" placeholder="Observación Detalle" TextMode="MultiLine"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="causesValidation" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                        </div>
                    </div>
                </fieldset>

                <div style="text-align: right"><a>Paso 3 de 3 <a style="font-style: italic; color: White;">Finalizar: Si ya termino de registrar todos los medios y contenidos para este trabajo, puede finalizar el cierre</a></div>
                <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" />

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
