<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master"
    CodeBehind="TraficoEncuestas.aspx.vb" Inherits="WebMatrix.TraficoEncuestas" %>

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



            $('#EnvioEncuestas').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Envío de Encuestas",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });

            $('#DevolucionEncuestas').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Devolución de Encuestas",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                }
            });

            validationForm();

        }

        $(document).ready(function () {
           

            loadPlugins();
        });
        function MostrarEnvioEncuestas() {
            $('#EnvioEncuestas').dialog("open");
        }

        function MostrarDevolucionEncuestas() {
            $('#DevolucionEncuestas').dialog("open");
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
    <a>Tráfico de Encuestas y Recursos</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <asp:Label ID="lblComentForm" runat="server"></asp:Label>
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:Label ID="txtNombreTrabajo" runat="server"></asp:Label>
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
                                    Nombre Trabajo</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="NombreMetodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Gestionar" ImageUrl="~/Images/list_16_.png" Text="Actualizar" ToolTip="Gestionar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detalles" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgdetalles" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Detalles" ImageUrl="~/Images/application_view_detail.png" Text="Detalles"
                                            ToolTip="Detalles" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Avance" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Avance" ImageUrl="~/Images/find_16.png" Text="Actualizar"
                                            ToolTip="Ir a Avance de Campo" />
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
                                Gestión de Tráfico
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="actions">
                                    <asp:Panel ID="pnlRecepcion" runat="server">
                                        <div>
                                            <label>
                                                Recepción</label>
                                            <asp:GridView ID="gvRecepcion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                DataKeyNames="Id" AllowPaging="True" EmptyDataText="No hay encuestas pendientes por recibir">
                                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                <SelectedRowStyle CssClass="SelectedRow" />
                                                <AlternatingRowStyle CssClass="odd" />
                                                <Columns>
                                                    <asp:BoundField DataField="FechaEnvio" HeaderText="Fecha Envío" />
                                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario Envía" />
                                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                    <asp:CheckBoxField DataField="Devolucion" HeaderText="Devolución?" />
                                                    <asp:BoundField DataField="Enviadas" HeaderText="Enc. Enviadas" />
                                                    <asp:TemplateField HeaderText="Enc. Recibidas" ShowHeader="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRecibidas" runat="server" Text='<%#Eval("Enviadas") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="fteTxtRecibidas" runat="server" FilterType="Numbers"
                                                                TargetControlID="txtRecibidas">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Observaciones" ShowHeader="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtObservaciones" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Recibir" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgRecibir" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                CommandName="Recibir" ImageUrl="~/Images/Select_16.png" Text="Recibir" ToolTip="Recibir estas encuestas" OnClientClick="return confirm('Está seguro de recibir estas encuestas?')"  />
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
                                            <div class="actions">
                                            </div>
                                    </asp:Panel>
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar Encuestas" OnClientClick="MostrarEnvioEncuestas()" />
                                            <asp:Button ID="btnDevolver" runat="server" Text="Devolver Encuestas" OnClientClick="MostrarDevolucionEncuestas()" />
                                            <asp:Button ID="btnEstimacion" runat="server" Text="Estimación de tiempos" />
                                            <asp:Button ID="btnAnulacion" runat="server" Text="Anular Encuestas" Visible="false" />
                                            <asp:Button ID="btnCapacitaciones" runat="server" Text="Capacitaciones" />
                                            <asp:Button ID="btnTareas" runat="server" Text="Módulo Tareas" />
                                            <asp:Button ID="btnPersonalAsignado" runat="server" Text="Personal Asignado" Visible="false" />
                                            <asp:Panel ID="pnlEstimacion" runat="server" Visible="false"><a>Producción estimada diaria del área para este trabajo</a><asp:TextBox ID="tbEstimacion" Width="50px" runat="server"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Numbers"
                                                runat="server" Enabled="True" TargetControlID="tbEstimacion">
                                            </asp:FilteredTextBoxExtender><asp:Button ID="btnEstimacionOk" runat="server" Text="Actualizar" /><asp:Button ID="btnEstimacionCAncel" runat="server" Text="Cancelar" /></asp:Panel>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    </fieldset>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles de tráfico
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfIdUnidad" runat="server" />
                        <asp:HiddenField ID="hfIdCargoEjecuta" runat="server" />
                        <fieldset class="validationGroup">
                            <asp:Panel ID="pnlEnvios" runat="server">
                                <div>
                                    <label>
                                        Envíos</label>
                                    <div class="form_left">
                                        <a>Unidad destino</a>
                                        <asp:DropDownList ID="ddlEnvios" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="lblEnvios" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                    <asp:GridView ID="gvEnvios" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id" AllowPaging="True" EmptyDataText="No hay encuestas registro de envíos">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="FechaEnvio" HeaderText="Fecha Envío" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Envio" HeaderText="Envió" />
                                            <asp:BoundField DataField="ObservacionesEnvio" HeaderText="Observaciones" />
                                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                            <asp:BoundField DataField="Encuestas" HeaderText="No. Encuestas" />
                                            <asp:BoundField DataField="FechaRecibo" HeaderText="Fecha Recibido" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Recibe" HeaderText="Recibe" />
                                            <asp:BoundField DataField="ObservacionesRecibo" HeaderText="Observaciones" />
                                            <asp:CheckBoxField DataField="Devolucion" HeaderText="Devolución" />
                                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" Visible="true">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgRecibir" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar" ToolTip="Eliminar este envío" OnClientClick="return confirm('Está seguro de eliminar este envío?')" />
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
                                                                Enabled='<%# IIF(gvEnvios.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                                Enabled='<%# IIF(gvEnvios.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <span class="pagingLinks">[<%= gvEnvios.PageIndex + 1%>-<%= gvEnvios.PageCount%>]</span>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                                Enabled='<%# IIF((gvEnvios.PageIndex +1) = gvEnvios.PageCount, "false", "true") %>'
                                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                                Enabled='<%# IIF((gvEnvios.PageIndex +1) = gvEnvios.PageCount, "false", "true") %>'
                                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </PagerTemplate>
                                    </asp:GridView>
                                </div>
                                <div class="actions">
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlMuestra" runat="server" Visible="false">
                                <div>
                                    <label>
                                        Muestra enviada</label>
                                    <asp:GridView ID="gvMuestra" runat="server" Width="40%" AutoGenerateColumns="False" PageSize="25"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        AllowPaging="True" EmptyDataText="No hay encuestas pendientes por recibir">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                            <asp:BoundField DataField="Cantidad" HeaderText="Encuestas Enviadas" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="actions">
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDevoluciones" runat="server">
                                <div>
                                    <label>
                                        Recepción</label>
                                    <div class="form_left">
                                        <a>Unidad origen</a>
                                        <asp:DropDownList ID="ddlDevoluciones" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="lblRecibidas" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                    <asp:GridView ID="gvRecibidas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id" AllowPaging="True" EmptyDataText="No hay registro de encuestas recibidas">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="FechaEnvio" HeaderText="Fecha Envío" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Envio" HeaderText="Envió" />
                                            <asp:BoundField DataField="ObservacionesEnvio" HeaderText="Observaciones" />
                                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                            <asp:BoundField DataField="Encuestas" HeaderText="No. Encuestas" />
                                            <asp:BoundField DataField="FechaRecibo" HeaderText="Fecha Recibido" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Recibe" HeaderText="Recibe" />
                                            <asp:BoundField DataField="ObservacionesRecibo" HeaderText="Observaciones" />
                                            <asp:CheckBoxField DataField="Devolucion" HeaderText="Devolución" />
                                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" Visible="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgRecibir" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar" ToolTip="Eliminar este envío" />
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
                                                                Enabled='<%# IIF(gvRecibidas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                                Enabled='<%# IIF(gvRecibidas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <span class="pagingLinks">[<%= gvRecibidas.PageIndex + 1%>-<%= gvRecibidas.PageCount%>]</span>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                                Enabled='<%# IIF((gvRecibidas.PageIndex +1) = gvRecibidas.PageCount, "false", "true") %>'
                                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                                Enabled='<%# IIF((gvRecibidas.PageIndex +1) = gvRecibidas.PageCount, "false", "true") %>'
                                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </PagerTemplate>
                                    </asp:GridView>
                                </div>
                                <div class="actions">
                                </div>
                            </asp:Panel>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Asignación personal a trabajo
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                            <label>Personal asignado</label>
                                <asp:GridView ID="gvPersonalAsignado" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="25"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar" OnClientClick="return confirm('Esta seguro de eliminar este registro ?');"
                                                    ToolTip="Eliminar envío" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="actions">
                                </div>
                                <label>Personal disponible para asignación</label>
                                <asp:GridView ID="gvPersonalPorAsignar" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="25"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                        <asp:BoundField DataField="TipoContratacion" HeaderText="Contratación" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:TemplateField HeaderText="Asignar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAsignar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Asignar" ImageUrl="~/Images/Select_16.png" Text="Asignar" 
                                                    ToolTip="Asignar Encuestador a Trabajo" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="actions">
                                </div>

                                <div class="actions">
                                </div>
                                <div class="form_right">
                                    <fieldset>
                                        <asp:Button ID="btnCancelAsignacion" runat="server" Text="Cancelar" />
                                    </fieldset>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <div id="EnvioEncuestas">
        <asp:UpdatePanel ID="upEnvioEncuestas" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <a>Unidad destino</a><asp:DropDownList ID="ddlDestinoPopupEnvio" runat="server"></asp:DropDownList><br />
                    <asp:Panel ID="pnlCiudadEnvioPopup" runat="server" Visible="false"><a>Ciudad</a><asp:DropDownList ID="ddlCiudadEnvio" runat="server" AutoPostBack="true"></asp:DropDownList><br /></asp:Panel>
                    <a>Encuestas disponibles: </a><asp:Label ID="lblEncDisponiblesEnvio" runat="server" Text=""></asp:Label><br />
                    <a>Cantidad a enviar: </a><asp:TextBox ID="tbCantidadEnvio" runat="server" Width="30px"></asp:TextBox><asp:FilteredTextBoxExtender ID="tbCantidadEstimada_FilteredTextBoxExtender" FilterType="Numbers"
                                                runat="server" Enabled="True" TargetControlID="tbCantidadEnvio">
                                            </asp:FilteredTextBoxExtender><br />
                    <a>Observaciones: </a><asp:TextBox ID="tbObservacionesEnvio" runat="server"></asp:TextBox><br />
                    <asp:Button ID="btnUpdateEnvio" runat="server" Text="Enviar" OnClientClick="$('#EnvioEncuestas').dialog('close');" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <div id="DevolucionEncuestas">
        <asp:UpdatePanel ID="upDevolucionEncuestas" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <a>Unidad devolucion</a><asp:DropDownList ID="ddlUnidadDevolucionPopup" runat="server"></asp:DropDownList><br />
                    <asp:Panel ID="pnlCiudadDevolucionPopup" runat="server" Visible="false"><a>Ciudad</a><asp:DropDownList ID="ddlCiudadDevolucionPopup" runat="server" AutoPostBack="true"></asp:DropDownList><br /></asp:Panel>
                    <a>Encuestas disponibles: </a><asp:Label ID="lblEncuestasDisponiblesDevolver" runat="server" Text=""></asp:Label><br />
                    <a>Cantidad a enviar: </a><asp:TextBox ID="tbCantidadDevolver" runat="server" Width="30px"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers"
                                                runat="server" Enabled="True" TargetControlID="tbCantidadDevolver">
                                            </asp:FilteredTextBoxExtender><br />
                    <a>Observaciones: </a><asp:TextBox ID="tbObservacionesDevolucion" runat="server"></asp:TextBox><br />
                    <asp:Button ID="btnDevolverPopup" runat="server" Text="Devolver" OnClientClick="$('#DevolucionEncuestas').dialog('close');" />
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
