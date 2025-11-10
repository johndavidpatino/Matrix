<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="EstimacionTareas.aspx.vb" Inherits="WebMatrix.EstimacionTareas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");

            $(":input[Tipo=Fecha]").mask("99/99/9999");
            $(":input[Tipo=Fecha]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("a[title=Actualizar]").removeAttr("class");
            $("a[title=Editar]").removeAttr("class");

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            loadPlugins();
            $('#HistoricoFechas').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Historico de fechas",
                width: "600px"
            });

            $('#HistoricoFechas').parent().appendTo("form");
        });
        function MostrarHistoricoFechas() {
            $('#HistoricoFechas').dialog("open");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Estimación de tareas</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <fieldset class="validationGroup">
        <div class="actions">
            <div class="form_rigth">
                <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver" />
            </div>
        </div>
        <asp:UpdatePanel ID="upGVEstimacionTareas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvEstimacionTareas" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id, HiloId, TareaId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:CommandField ValidationGroup="vgActualizarRegistro" ShowEditButton="True" EditText="Editar"
                            CancelText="Cancelar" UpdateText="Actualizar" ControlStyle-CssClass="causesValidation" />
                        <asp:TemplateField HeaderText="Tarea">
                            <ItemTemplate>
                                <asp:Label ID="lblTarea" Text='<%#Eval("Tarea")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTarea" Text='<%#Eval("Tarea")%>' runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FechaInicioPlaneada">
                            <ItemTemplate>
                                <asp:Label ID="lblFIniP" Text='<%#Eval("FIniP","{0:dd/M/yyyy}") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFIniP" runat="server" CssClass="required text textEntry" Tipo="Fecha"
                                    Text='<%#Eval("FIniP") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FechaFinPlaneada">
                            <ItemTemplate>
                                <asp:Label ID="lblFFinP" Text='<%#Eval("FFinP","{0:dd/M/yyyy}")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFFinP" runat="server" CssClass="required text textEntry" Tipo="Fecha"
                                    Text='<%#Eval("FFinP")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observacion">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtObservacion" runat="server" Tipo="Texto"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aplica">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAplica" runat="server" Checked='<%# IIF (Eval("EstadoWorkFlow_Id")<>6,"true","false") %>'
                                    AutoPostBack="true" OnCheckedChanged="chkAplica_CheckedChanged" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkAplica" runat="server" Checked='<%# IIF (Eval("EstadoWorkFlow_Id")<>6,"true","false") %>'
                                    Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Historico" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgVerHistorico" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Historico" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver"
                                    OnClientClick="MostrarHistoricoFechas()" />
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
                                            Enabled='<%# IIF(gvEstimacionTareas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvEstimacionTareas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvEstimacionTareas.PageIndex + 1%>-
                                            <%= gvEstimacionTareas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvEstimacionTareas.PageIndex +1) = gvEstimacionTareas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvEstimacionTareas.PageIndex +1) = gvEstimacionTareas.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <div id="HistoricoFechas">
        <div>
            <fieldset>
                <asp:UpdatePanel ID="upHistoricoFechas" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvHistoricoFechas" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvEstimacionTareas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvEstimacionTareas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvEstimacionTareas.PageIndex + 1%>-
                                                    <%= gvEstimacionTareas.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvEstimacionTareas.PageIndex +1) = gvEstimacionTareas.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvEstimacionTareas.PageIndex +1) = gvEstimacionTareas.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
