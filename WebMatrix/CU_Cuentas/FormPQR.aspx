<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="FormPQR.aspx.vb" Inherits="WebMatrix.FormPQR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {

            });

            $("#<%= txtFecha.ClientId %>").mask("99/99/9999");
            $("#<%= txtFecha.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaCierre.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaCierre.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
            

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
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Lista de PQR</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Palabra a Buscar</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                                <asp:HiddenField ID="hfIdPQR" runat="server" />
                            </fieldset>
                            <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                        HtmlEncode="False" />
                                    <asp:BoundField DataField="EstablecidaPor" HeaderText="EstablecidaPor" />
                                    <asp:TemplateField HeaderText="Empresa">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpresa" runat="server" Text='<%# Eval("Empresa") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AccionInmediata" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccionInmediata" runat="server" Text='<%# Eval("AccionInmediata") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="F. Recibe">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecibe" runat="server" Text='<%# Eval("Recibe") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="F. Designado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignado" runat="server" Text='<%# Eval("Designado") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Situación" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSituacion" runat="server" Text='<%# Eval("Situacion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Accion" HeaderText="Acción" Visible="false" />
                                    <asp:BoundField DataField="FechaCierre" HeaderText="Fecha Cierre" DataFormatString="{0:dd/MM/yyyy}"
                                        HtmlEncode="False" />
                                    <asp:BoundField DataField="RespuestaPQR" HeaderText="RespuestaPQR" Visible="false" />
                                    <asp:BoundField DataField="Cierra" HeaderText="F. Cierra" />
                                    <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                ToolTip="Modificar" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                                OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cerrar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgdetalles" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Detalles" ImageUrl="~/Images/application_view_detail.png" Text="Detalles"
                                                ToolTip="Detalles" />
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
                                                    <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                        <%= gvDatos.PageCount%>]</span>
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
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Información del PQR</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Fecha:</label>
                                        <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle" Enabled="false"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Funcionario Designado:</label>
                                        <asp:DropDownList ID="ddlfuncionariodesignado" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                </div>
                               <div class="form_left">
                                 <fieldset>
                                        <label>
                                            Establecida Por</label>
                                        <asp:TextBox ID="txtEstablecidaPor" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Recibida Por</label>
                                          <asp:DropDownList ID="ddlrecibida" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                  </div>
                                  <div class="form_left">
                                 <fieldset>
                                        <label>
                                            Empresa Cliente</label>
                                        <asp:TextBox ID="TxtEmpresa" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                  </div>
                                   <div class="actions">
                                </div>                          
                            
                                  <fieldset>
                                            <label>
                                               Descripción de la PQR en términos de quien la estableció:</label>
                                            <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                                        </fieldset>
                                   <div class="actions">
                                </div>
                                  <fieldset>
                                            <label>
                                               Acción inmediata tomada por quien la recibió:</label>
                                            <asp:TextBox ID="txtAccionInmediata" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                                        </fieldset>
                                   <div class="actions">
                                   </div>
                                  <fieldset>
                                            <label>
                                               Situación que motivó la PQR y causa (resultado de la investigación):</label>
                                            <asp:TextBox ID="txtSituacion" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                                        </fieldset>
                                   <div class="actions">
                                   </div>
                                  <fieldset>
                                            <label>
                                              Acción para resolver la PQR:</label>
                                            <asp:TextBox ID="txtAccion" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                                        </fieldset>
                                    <div class="actions">
                                </div>
                                  
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar" style="font-size: 11px;"
                                                onclick="location.href='FormPQR.aspx';" />
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Cerrar PQR</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                             <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Fecha Cierre:</label>
                                        <asp:TextBox ID="txtFechaCierre" runat="server" CssClass="bgCalendar textCalendarStyle" Enabled="false"></asp:TextBox>

                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Funcionario Cierre:</label>
                                        <asp:DropDownList ID="ddlfuncionariocierre" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                </div>
                                   <div class="actions">
                                </div>
                                  <fieldset>
                                            <label>
                                              Respuesta PQR:</label>
                                            <asp:TextBox ID="txtRespuestaPQR" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                                        </fieldset>
                                    <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnCerrarPQR" runat="server" Text="Guardar" CommandName="Guardar" />
                                            &nbsp;
                                            <input id="Button3" type="button" class="button" value="Cancelar" style="font-size: 11px;"
                                                onclick="location.href='FormPQR.aspx';" />
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
