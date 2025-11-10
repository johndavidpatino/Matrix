<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_F.master"
    CodeBehind="Entrevista.aspx.vb" Inherits="WebMatrix.Entrevista" %>
    
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
                var idpais = $('#<%= ddlpais.ClientID %>').val();
                var iddpto = $('#<%= ddldepartamento.ClientID %>').val();
                var idciudad = $('#<%= ddlCiudad.ClientID %>').val();
                var identrevistador = $('#<%= ddlEntrevistador.ClientID %>').val();

                if (idpais == '-1') {
                    validarSelect('<%= ddlpais.ClientID %>', "Debe seleccionar un pais");
                }

                if (iddpto == '-1') {
                    validarSelect('<%= ddldepartamento.ClientID %>', "Debe seleccionar un departamento");
                }

                if (idciudad == '-1') {
                    validarSelect('<%= ddlCiudad.ClientID %>', "Debe seleccionar una ciudad");
                }

                if (identrevistador == '-1') {
                    validarSelect('<%= ddlEntrevistador.ClientID %>', "Debe seleccionar un entrevistador");
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
        function redireccion() {
            var identrevista= $('#<%= hfFichaEntrevistaID.ClientID %>').val();
            document.location.href = 'Entrevista.aspx?identrevista=' + identrevista;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Planeación Entrevistas</a>
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
                                Lista de Detalles de Entrevista</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Ciudad, Persona, Dirección o Teléfono<asp:HiddenField ID="hfFichaEntrevistaID" 
                                    runat="server" />
                                </label>
                                &nbsp;<asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  />
                            </fieldset>
                        </div>
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="FichaEntrevistaId" HeaderText="ID Ficha Entrevista" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="PersonaAEntrevistar" HeaderText="Persona A Entrevistar" />
                                <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                   <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
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
                                Información de la Entrevista</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Persona A Entrevistar</label>
                                            <asp:HiddenField ID="hfEntrevistaID" runat="server" />
                                            <asp:TextBox ID="txtPersonaAEntrevistar" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                País</label>
                                            <asp:DropDownList ID="ddlpais" runat="server" AutoPostBack="True" CssClass="dropdowntext">
                                            </asp:DropDownList>
                                        </fieldset>
                                         <fieldset>
                                            <label>
                                                Fecha:</label>
                                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Fecha Real:</label>
                                            <asp:TextBox ID="txtFechaReal" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>

                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Dirección</label>
                                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Departamento</label>
                                            <asp:DropDownList ID="ddldepartamento" runat="server" AutoPostBack="True" CssClass="dropdowntext">
                                            </asp:DropDownList>
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
                                                Teléfono</label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Ciudad</label>
                                            <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="dropdowntext">
                                            </asp:DropDownList>
                                        </fieldset>
                                        

                                         <fieldset>
                                            <label>
                                                Entrevistador</label>
                                            <asp:DropDownList ID="ddlEntrevistador" runat="server" CssClass="dropdowntext">
                                            </asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>
                                                Cancelada</label>
                                            <asp:CheckBox ID="chkCancelada" runat="server" />
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Grupo Objetivo</label>
                                            <asp:TextBox ID="txtGrupoObjetivo" runat="server" CssClass="textMultiline" Height="100px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Caracteristicas Especiales</label>
                                            <asp:TextBox ID="txtCaracteristicasEspeciales" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Observaciones Adicionales</label>
                                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="textMultiline" Height="100px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                </asp:Panel>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" ValidationGroup="Guardar"
                                                 />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar" 
                                                style="font-size: 11px;" onclick="redireccion();" />
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
                                Detalles del registro</label>
                        </a>
                    </h3>
                    <div class="block">
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
