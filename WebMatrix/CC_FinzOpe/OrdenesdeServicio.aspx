<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="OrdenesdeServicio.aspx.vb" Inherits="WebMatrix.OrdenesdeServicioF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $("#<%= txtfechaduracion.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechaduracion.ClientID%>").datepicker({
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
            $('#BusquedaSolicitantes').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Solicitantes",
                    width: "600px"
                });

            $('#BusquedaSolicitantes').parent().appendTo("form");

            $('#ObservacionAnulacion').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Observación de anulación",
                    width: "600px"
                });

            $('#ObservacionAnulacion').parent().appendTo("form");


            $.validator.addMethod('selectNone',
         function (value, element) {
             return (this.optional(element) == true) || ((value != -1) && (value != ''));
         }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('validTrabajo',
         function (value, element) {
             return this.optional(element) == true || $('#CPH_Section_CPH_Section_CPH_ContentForm_lblidtrabajo').text() != '';
         }, "*Trabajo no valido");
            $.validator.addClassRules("mySpecificClass2", { validTrabajo: true });

            validationForm();

        }
        function MostrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("open");
        }

        function CerrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("close");
        }

        function MostrarObservacionAnulacion(id) {
            $('#CPH_Section_CPH_Section_CPH_ContentForm_HfOSAnulacionId').val(id);
            $('#ObservacionAnulacion').dialog("open");
        }

        function CerrarObservacionAnulacion() {
            $('#ObservacionAnulacion').dialog("close");
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                <asp:HiddenField ID="hfID" runat="server" />
                                <asp:HiddenField ID="HfOrdenId" runat="server" />
                                <asp:HiddenField ID="HfOSAnulacionId" runat="server" />
                                <asp:HiddenField ID="hfidActividad" runat="server" />
                                <asp:TextBox ID="txtIdentificacionBuscar" runat="server" placeholder="Identificacion"></asp:TextBox>
                                <asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </div>
                            <div class="form_left">
                                <fieldset>
                                </fieldset>
                            </div>
                        </div>
                        <asp:GridView ID="gvContratistas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Identificacion, Activo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
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
                                <asp:BoundField DataField="NumeroSymphony" HeaderText="NumeroSymphony" Visible="false" />
                                <asp:BoundField DataField="Servicio" HeaderText="Servicio" />
                                <asp:BoundField DataField="DescripcionCuenta" HeaderText="DescripcionCuenta" />
                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro" Visible="false" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" Visible="false" />
                                <asp:BoundField DataField="Aprobado" HeaderText="Aprobado" Visible="false" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" />

                                <asp:TemplateField HeaderText="OrdendeServicio" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Orden" ImageUrl="~/Images/Select_16.png" Text="OrdendeServio"
                                            ToolTip="OrdendeServicio" />
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
                            <a href="#">Detalle de la orden
                            </a>
                        </h3>
                        <div class="block">
                            <fieldset class="validationGroup">
                                <fieldset>
                                    <asp:label id="lblContratistaInactivo" runat="server" Text="¡No se pueden crear requerimientos para este contratista, debido a que esta inactivo!" ForeColor="Red"></asp:label>
                                    <label>
                                        Buscar Trabajo</label>
                                    <asp:TextBox ID="txtTrabajo" runat="server" TabIndex="1" CssClass="mySpecificClass2"></asp:TextBox>
                                    <asp:Button ID="btnbuscartrabajo" runat="server" Text="Buscar" />
                                    <asp:Label ID="lblidtrabajo" runat="server"></asp:Label>
                                    <asp:Label ID="lblnombretrabajo" runat="server"></asp:Label>
                                    <asp:Label ID="lbljob" runat="server"></asp:Label>
                                </fieldset>

                                <div class="form_left">
                                    <fieldset>
                                        <label>Seleccione Servicio</label>
                                        <asp:DropDownList ID="ddlServicio" runat="server" CssClass="mySpecificClass" AutoPostBack="true"></asp:DropDownList>
                                        <label>Seleccione Cuenta contable</label>
                                        <asp:DropDownList ID="ddlCuentasContables" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        <asp:HiddenField ID="hfCuentaContableId" runat="server" />
                                        <label>Solicitado por: </label>
                                        <asp:TextBox ID="txtsolicitado" runat="server" CssClass="required text textEntry" Height="18px"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>Departamento</label>
                                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="true" CssClass="mySpecificClass"></asp:DropDownList>
                                        <label>Ciudad</label>
                                        <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>Cantidad</label>
                                        <asp:TextBox ID="txtcantidad" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                        <label>Evalua Proveedor:</label>
                                        <asp:HiddenField ID="hfIdEvaluaFactura" runat="server" />
                                        <asp:TextBox ID="txtEvaluaFactura" runat="server" ReadOnly="true" CssClass="required text textEntry"></asp:TextBox>
                                        <asp:Button ID="btnSolicitanteBusqueda" Text="..." runat="server" Width="25px" OnClientClick="MostrarSolicitantes();return false;" />
                                    </fieldset>
                                </div>

                                <div class="form_left">
                                    <fieldset>
                                        <label>VrUnitario</label>
                                        <asp:TextBox ID="txtvrunitario" runat="server" CssClass="required text textEntry"></asp:TextBox>

                                    </fieldset>
                                </div>

                                <div class="form_left">
                                    <fieldset>
                                        <label>Fecha Duracion de la Orden</label>
                                        <asp:TextBox ID="txtfechaduracion" runat="server" CssClass="required text textEntry"></asp:TextBox>

                                    </fieldset>
                                </div>
                                <div class="actions">
                                    <asp:Button ID="btnagregar" runat="server" Text="Agregar" CssClass="causesValidation" Height="22px" />
                                    <asp:Button ID="btngenerar" runat="server" Text="Guardar y Generar" />
                                </div>
                            </fieldset>
                            Detalle de la orden actual:
                            <div>

                                <asp:GridView ID="GvDetalleOrden" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id, ServicioId, CuentaContableId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                        <asp:BoundField DataField="Servicio" HeaderText="Servicio" />
                                        <asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta" />
                                        <asp:BoundField DataField="CuentaContable" HeaderText="CuentaContable" />                                        
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                        <asp:BoundField DataField="VrTotal" HeaderText="VrTotal" />

                                    </Columns>
                                </asp:GridView>

                            </div>
                            <br />
                            <div>
                                <asp:HiddenField ID="hfPorcentaje" runat="server" />
                                <asp:HiddenField ID="hfVrAnticipo" runat="server" />
                                <asp:HiddenField ID="hfPagoFinal" runat="server" />
                                <label id="lblPorcentaje" runat="server" visible="false">Porcentaje Anticipo %:</label>
                                <asp:TextBox ID="txtPorcentaje" runat="server" visible="false" CssClass="required text textEntry"></asp:TextBox>
                                 &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPorcentaje" runat="server" Text="Agregar Porcentaje" visible="false"/>
                                <label id="lblActicipo" runat="server" visible="false">Valor Anticipo Pago Parcial:</label>
                                <label id="lblVrAnticipo" runat="server" visible="false"></label>
                            </div>
                            <br />
                            <br />
                            Ordenes asociadas al trabajo y el contratista
                            <div>
                                <asp:GridView ID="GvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                        <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                        <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="VrTotal" HeaderText="VrTotal" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="PersonaEvalua" HeaderText="Persona Evalua" />
                                        <asp:TemplateField HeaderText="Anular" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgAnular" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Anular" ImageUrl="~/Images/Select_16.png" Text="Anular"
                                                    ToolTip="Anular" OnClientClick='<%# Eval("Id", "MostrarObservacionAnulacion({0});return false;")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaSolicitantes">
        <asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <asp:GridView ID="gvSolicitantes" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarSolicitantes()" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="ObservacionAnulacion">
        <asp:UpdatePanel ID="upObservacionAnulacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtObservacionAnulacion" runat="server" TextMode="MultiLine" ></asp:TextBox>
                <asp:Button ID="btnGuardarAnulacion" runat="server" Text="Guardar" OnClientClick="CerrarObservacionAnulacion();" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
