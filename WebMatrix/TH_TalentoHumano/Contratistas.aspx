<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master" CodeBehind="Contratistas.aspx.vb" Inherits="WebMatrix.Contratistas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $("#<%= txtFechaingreso.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaingreso.ClientID%>").datepicker({
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
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
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
                <div id="accordion0">
                    <h3><a href="#">Listado de Contratistas</a></h3>
                    <div class="block">
                        <div class="actions">
                            <div class="form_left">
                                <label>Búsqueda por</label>
                                <asp:TextBox ID="txtIdentificacionBuscar" runat="server" placeholder="Identificacion"></asp:TextBox>
                                <asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo ingreso" />
                                </fieldset>
                            </div>
                        </div>
                        <asp:GridView ID="gvContratistas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Identificacion" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Activo" HeaderText="Activo" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="NumeroSymphony" HeaderText="NumeroSymphony" />
                                <asp:BoundField DataField="Servicio" HeaderText="Servicio" Visible="false" />
                                <asp:BoundField DataField="DescripcionCuenta" HeaderText="DescripcionCuenta" />
                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" />
                                <asp:BoundField DataField="Aprobado" HeaderText="Aprobado" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" />
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
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
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIf(gvContratistas.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(gvContratistas.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvContratistas.PageIndex + 1%>-<%= gvContratistas.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((gvContratistas.PageIndex + 1) = gvContratistas.PageCount, "false", "true")%>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((gvContratistas.PageIndex + 1) = gvContratistas.PageCount, "false", "true")%>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <div id="accordion1">
                        <h3>
                            <a href="#">Datos básicos
                            </a>
                        </h3>
                        <div class="block">
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Identificación</label>
                                    <asp:HiddenField ID="hfID" runat="server" />
                                    <asp:TextBox ID="txtIdentificacion" runat="server" TabIndex="1"></asp:TextBox>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Correo</label>
                                    <asp:TextBox ID="txtcorreo" MaxLength="100" runat="server" TabIndex="4"></asp:TextBox>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Servicio</label>
                                    <asp:DropDownList ID="ddlservicio" runat="server" TabIndex="7"></asp:DropDownList>
                                    <asp:Button ID="btnagregar" runat="server" Text="Agregar"></asp:Button>


                                         <br />
                                    <br />


                                         <asp:GridView ID="GvServicios" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="False" CssClass="displayTable" DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" PagerStyle-CssClass="headerfooter ui-toolbar" PageSize="15" Width="50%">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:BoundField DataField="IdentificacionId" HeaderText="IdentificacionId" Visible="false" />
                                            <asp:BoundField DataField="ServicioId" HeaderText="ServicioId" />
                                            <asp:BoundField DataField="ServicioDescripcion" HeaderText="ServicioDescripcion" />
                                             <asp:TemplateField HeaderText="Estado" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Estado" runat="server" CommandName="Estado"  Checked='<%# Eval("Estado")%>'  />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Telefono</label>
                                    <asp:TextBox ID="txttelefono" MaxLength="100" runat="server" TabIndex="10"></asp:TextBox>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Aprobado Por:</label>
                                     <asp:TextBox ID="txtAprobado" MaxLength="100" runat="server" TabIndex="13"></asp:TextBox>
                               
                                   
                                </fieldset>
                                <fieldset>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Nombre o Razon Social</label>
                                    <asp:TextBox ID="txtNombre" MaxLength="100" runat="server" TabIndex="2"></asp:TextBox>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Clasificación</label>
                                    <asp:DropDownList ID="ddlClasificacion" runat="server"></asp:DropDownList>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudad" runat="server" TabIndex="4"></asp:DropDownList>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Descripcion de la Cuenta</label>
                                    <asp:TextBox ID="txtdesripcion" MaxLength="100" runat="server" TabIndex="8"></asp:TextBox>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Estado</label>
                                    <asp:DropDownList ID="ddlestado" runat="server" TabIndex="11"></asp:DropDownList>
                                </fieldset>

                                <fieldset>
                                    <label>
                                        Observacion</label>
                                    <asp:TextBox ID="TxtObservacion" MaxLength="100" runat="server" TabIndex="14"></asp:TextBox>
                                </fieldset>


                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Direccion</label>
                                    <asp:TextBox ID="txtdireccion" MaxLength="100" runat="server" TabIndex="3"></asp:TextBox>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Numero Symphony</label>
                                    <asp:TextBox ID="txtsymphony" MaxLength="100" runat="server" TabIndex="6"></asp:TextBox>
                                </fieldset>


                                <fieldset>
                                    <label>
                                        Solicitado Por:</label>
                                    <asp:TextBox ID="txtsolicitud" MaxLength="100" runat="server" TabIndex="12"></asp:TextBox>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Fecha Ingreso</label>
                                    <asp:TextBox ID="txtFechaingreso" runat="server" CssClass="bgCalendar textCalendarStyle" TabIndex="15"></asp:TextBox>
                                </fieldset>

                            </div>

                            <div class="actions">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                            </div>
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
