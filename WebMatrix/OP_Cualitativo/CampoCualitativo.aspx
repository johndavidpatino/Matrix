<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_F.master"
    CodeBehind="CampoCualitativo.aspx.vb" Inherits="WebMatrix.CampoCualitativo" %>

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
                var idModerador = $('#<%= ddlModerador.ClientID %>').val();


                if (idModerador == '-1') {
                    validarSelect('<%= ddlModerador.ClientID %>', "Debe seleccionar un moderador");
                }
            });

            $('#<%= txtHora.ClientId %>').timeEntry({ show24Hours: true, spinnerImage: '', defaultTime: '00:00:00', showSeconds: true });
            $('#<%= txtHoraReal.ClientId %>').timeEntry({ show24Hours: true, spinnerImage: '', defaultTime: '00:00:00', showSeconds: true });

            $("#<%= txtFecha.ClientId %>").mask("99/99/9999");
            $("#<%= txtFecha.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaReal.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaReal.ClientId %>").datepicker({
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

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Campo Cualitativo</a>
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
            Trabajo:
    <asp:Label ID="lblTrabajo" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver a Segmentos"></asp:LinkButton>
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Lista de Planeación y Ejecución de Campo</label>
                            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                            <asp:HiddenField ID="hfIdSegmento" runat="server" />
                            <asp:HiddenField ID="hfIdCampo" runat="server" />
                        </a>
                    </h3>
                    <div class="block">
                    <div class="form_left">
                            <fieldset>
                                <label></label>
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  />
                            </fieldset>
                        </div>
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" />
                                <asp:BoundField DataField="Moderador" HeaderText="Moderador" />
                                <asp:BoundField DataField="Transcriptor" HeaderText="Transcriptor" />
                                <asp:BoundField DataField="Lugar" HeaderText="Lugar" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                <asp:CheckBoxField DataField="Ejecutada" HeaderText="Ejecutada" />
                                <asp:CheckBoxField DataField="Cancelada" HeaderText="Cancelada" />
                                <asp:CheckBoxField DataField="Caida" HeaderText="Caida" />
                                <asp:TemplateField HeaderText="Planeación" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbPlaneacion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="AbrirPlaneacion" ImageUrl="~/Images/calendar_24.png" Text="Seleccionar"
                                            ToolTip="Abrir Planeación" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ejecución" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbEjecucion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="AbrirEjecucion" ImageUrl="~/Images/edit.png" Text="Seleccionar"
                                            ToolTip="Abrir Ejecucion" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImbEliminar" runat="server" CausesValidation="False" CommandName="Eliminar"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                            OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
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
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Planeación del Campo</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Ciudad</label>
                                            <asp:TextBox ID="txtCiudad" runat="server" Enabled="false" Width="250px"></asp:TextBox>
                                        </fieldset>

                                        <fieldset>
                                            <label>
                                                Fecha:</label>
                                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Persona contacto</label>
                                            <asp:TextBox ID="txtPersonaContacto" runat="server" Width="250px" CssClass="required text textEntry" ></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Tipo de lugar</label>
                                            <asp:TextBox ID="txtTipoLugar" runat="server" Enabled="false" Width="250px"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Hora:</label>
                                            <asp:TextBox ID="txtHora" runat="server" Width="100px" ToolTip="Formato HH:mm:ss.<br/>Utilice el scroll del mouse para seleccionar una hora."
                                                ValidationGroup="Guardar" CssClass="textEntry toolTipFunction"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* La hora es requerida <br/>"
                                                Font-Bold="true" Font-Size="10px" ForeColor="#ff0000" Display="Dynamic" ControlToValidate="txtHora"
                                                Text="*" ValidationGroup="Guardar"></asp:RequiredFieldValidator>
                                        </fieldset>
                                        
                                        <fieldset>
                                            <label>
                                                Datos contacto</label>
                                            <asp:TextBox ID="txtDatosContacto" runat="server" Width="250px" CssClass="required text textEntry" ></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Locación</label>
                                            <asp:TextBox ID="txtLugar" runat="server" Width="250px" CssClass="required text textEntry" ></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Moderador</label>
                                            <asp:DropDownList ID="ddlModerador" runat="server" CssClass="dropdowntext">
                                            </asp:DropDownList>
                                        </fieldset>

                                        <fieldset>
                                            <label>
                                                Transcriptor</label>
                                            <asp:DropDownList ID="ddlTranscriptor" runat="server" CssClass="dropdowntext">
                                            </asp:DropDownList>
                                        </fieldset>
                                       
                                        <fieldset>
                                            <label>
                                                Dirección</label>
                                            <asp:TextBox ID="txtDireccion" runat="server" Width="250px" CssClass="required text textEntry" ></asp:TextBox>
                                        </fieldset>

                                    </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Observaciones</label>
                                            <asp:TextBox ID="txtObservacionesPrevias" runat="server" Width="100%" ></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    </div>
                                          
                                           
                                </asp:Panel>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                                                 />
                                            &nbsp;
                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                            &nbsp;
                                            <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" />
                                            &nbsp;
                                            <asp:ImageButton ID="imbDescargarCita" ToolTip="Descargar cita al calendario" 
                                                runat="server" ImageUrl="~/Images/calendar_24.png" />
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
                                Ejecución del Campo</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div>
                            <div class="form_left">
                                                                                <fieldset>
                                            <label>
                                                Fecha Real:</label>
                                            <asp:TextBox ID="txtFechaReal" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Hora Real:</label>
                                            <asp:TextBox ID="txtHoraReal" runat="server" Width="100px" ToolTip="Formato HH:mm:ss.<br/>Utilice el scroll del mouse para seleccionar una hora real."
                                                ValidationGroup="Guardar" CssClass="textEntry toolTipFunction"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* La hora real es requerida <br/>"
                                                Font-Bold="true" Font-Size="10px" ForeColor="#ff0000" Display="Dynamic" ControlToValidate="txtHoraReal"
                                                Text="*" ValidationGroup="Guardar"></asp:RequiredFieldValidator>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                       <fieldset>
                                        <label>
                                            Asistentes</label>
                                        <asp:TextBox ID="txtAsistentes" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                    </fieldset>
                                    </div>
                                    <div class="form_left">
                                       <fieldset>
                                        <label>
                                            Asistentes que no cumplieron el perfil</label>
                                        <asp:TextBox ID="txtAsistentesReales" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                    </fieldset>
                                    </div>
                                    <div class="actions">
                                    <div class="form_left">
                                     <fieldset>
                                            <label>
                                                Cancelada</label>
                                            <asp:RadioButton ID="rbtCancelada" GroupName="EstadoSesion" runat="server" />
                                        </fieldset>
                                         </div>
                                         <div class="form_left">
                                     <fieldset>
                                            <label>
                                                Caida</label>
                                            <asp:RadioButton ID="rbtCaida" GroupName="EstadoSesion" runat="server" />
                                        </fieldset>
                                         </div>
                                         <div class="form_left">
                                     <fieldset>
                                            <label>
                                                Ejecutada</label>
                                            <asp:RadioButton ID="rbtEjecutada" GroupName="EstadoSesion" runat="server" />
                                        </fieldset>
                                         </div>
                                    </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Observaciones de la ejecución</label>
                                            <asp:TextBox ID="txtObservacionesEjecucion" runat="server" Width="100%" ></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardarEjecucion" runat="server" Text="Guardar" CommandName="Guardar" ValidationGroup="Guardar"
                                                 />
                                            <asp:Button ID="btnCancelEjecucion" runat="server" Text="Cancelar" CommandName="Cancel" />
                                            <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Novedades"/>
                                        </fieldset>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
                <div id="accordion7">
                    <h3>
                        <a href="#">
                            <label>
                                Listado de Campo
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <div style="text-align:center"><asp:Button ID="btnExportar" runat="server" Text="Exportar listado" /></div>
        <br />
        <asp:GridView ID="gvExportado" runat="server" AutoGenerateColumns="True" Visible="False"
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" 
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
                        </div>
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
