<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="ListadoEstudios.aspx.vb" Inherits="WebMatrix.ListadoEstudios" %>

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


            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();

            $('#PresupuestosAsignadosXEstudio').parent().appendTo("form");

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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Estudios
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    </label>
                                <div class="form_left">
                                <label>Búsqueda por</label>
                                <asp:TextBox ID="txtNoPropuestaBuscar" runat="server" placeholder="No Propuesta"></asp:TextBox>
                                <asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre Estudio"></asp:TextBox>
                                <asp:TextBox ID="txtJobBook" runat="server" placeholder="JobBook"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </div>
                            </fieldset>
                        </div>
                        <asp:UpdatePanel ID="upEstudios" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvEstudios" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="No. Estudio" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                        <asp:BoundField DataField="PropuestaId" HeaderText="PropuestaId" />
                                        <asp:BoundField DataField="NomGerenteCuentas" HeaderText="GerenteCuentas" Visible="True" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                        <asp:BoundField DataField="FechaInicioCampo" HeaderText="F. Inicio Campo" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                        <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="PresupuestosAsignados" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                    ToolTip="Presupuestos asignados" OnClientClick="MostrarPresupuestosAsignadosXEstudio()" />
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
                                                            Enabled='<%# IIF(gvEstudios.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                            Enabled='<%# IIF(gvEstudios.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <span class="pagingLinks">[<%= gvEstudios.PageIndex + 1%>-
                                                            <%= gvEstudios.PageCount%>]</span>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                            Enabled='<%# IIF((gvEstudios.PageIndex +1) = gvEstudios.PageCount, "false", "true") %>'
                                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                            Enabled='<%# IIF((gvEstudios.PageIndex +1) = gvEstudios.PageCount, "false", "true") %>'
                                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="PresupuestosAsignadosXEstudio">
        <asp:UpdatePanel ID="upPresupuestosAsignadosXEstudio" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvPresupuestosAsignadosXEstudio" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor"  DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin"  DataFormatString="{0:P2}" />
                        <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                        <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                        <asp:TemplateField HeaderText="Consultar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrConsultarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="ConsultarPresupuesto" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                    ToolTip="Editar presupuestos" />
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
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXEstudio.PageIndex = 0, "false", "true") %>'
                                            SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXEstudio.PageIndex = 0, "false", "true") %>'
                                            SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPresupuestosAsignadosXEstudio.PageIndex + 1%>-
                                            <%= gvPresupuestosAsignadosXEstudio.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXEstudio.PageIndex +1) = gvPresupuestosAsignadosXEstudio.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXEstudio.PageIndex +1) = gvPresupuestosAsignadosXEstudio.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
