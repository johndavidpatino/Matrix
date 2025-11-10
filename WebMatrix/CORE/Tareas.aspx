<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="Tareas.aspx.vb" Inherits="WebMatrix.Tareas" %>

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
    Nombre trabajo:<asp:Label ID="lblNombreTrabajo" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Tareas X Realizar
                    </label>
                </a>
            </h3>
            <div class="block">
                <div class="form_left">
                    <fieldset>
                        <label>
                            Titulo
                        </label>
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                    </fieldset>
                    <asp:GridView ID="gvTareasXRealizar" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id, TareaId, HiloId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                            <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FIniR" HeaderText="FechaInicioReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinR" HeaderText="FechaFinReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
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
                                                Enabled='<%# IIF(gvTareasXRealizar.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvTareasXRealizar.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvTareasXRealizar.PageIndex + 1%>-<%= gvTareasXRealizar.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasXRealizar.PageIndex +1) = gvTareasXRealizar.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasXRealizar.PageIndex +1) = gvTareasXRealizar.PageCount, "false", "true") %>'
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
        <div id="accordion1">
            <h3>
                <a href="#">
                    <label>
                        Tareas devueltas
                    </label>
                </a>
            </h3>
            <asp:GridView ID="gvTareasDevueltas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="Id, TareaId, HiloId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                    <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FIniR" HeaderText="FechaInicioReal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinR" HeaderText="FechaFinReal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgIrVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
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
                                        Enabled='<%# IIF(gvTareasDevueltas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIF(gvTareasDevueltas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvTareasDevueltas.PageIndex + 1%>-<%= gvTareasDevueltas.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIF((gvTareasDevueltas.PageIndex +1) = gvTareasDevueltas.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIF((gvTareasDevueltas.PageIndex +1) = gvTareasDevueltas.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </div>
        <div id="accordion2">
            <h3>
                <a href="#">
                    <label>
                        Tareas realizadas
                    </label>
                </a>
            </h3>
            <div class="block">
                <div class="form_left">
                    <asp:GridView ID="gvTareasRealizadas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id, TareaId, HiloId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                            <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FIniR" HeaderText="FechaInicioReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinR" HeaderText="FechaFinReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
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
                                                Enabled='<%# IIF(gvTareasRealizadas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvTareasRealizadas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvTareasRealizadas.PageIndex + 1%>-<%= gvTareasRealizadas.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasRealizadas.PageIndex +1) = gvTareasRealizadas.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasRealizadas.PageIndex +1) = gvTareasRealizadas.PageCount, "false", "true") %>'
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
        <div id="accordion3">
            <h3>
                <a href="#">
                    <label>
                        Tareas en curso
                    </label>
                </a>
            </h3>
            <div class="block">
                <div class="form_left">
                    <asp:GridView ID="gvTareasEnCurso" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id, TareaId, HiloId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                            <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FIniR" HeaderText="FechaInicioReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FFinR" HeaderText="FechaFinReal" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
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
                                                Enabled='<%# IIF(gvTareasEnCurso.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvTareasEnCurso.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvTareasEnCurso.PageIndex + 1%>-<%= gvTareasEnCurso.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasEnCurso.PageIndex +1) = gvTareasEnCurso.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIF((gvTareasEnCurso.PageIndex +1) = gvTareasEnCurso.PageCount, "false", "true") %>'
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
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
