<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="ProgramacionCampo.aspx.vb" Inherits="WebMatrix.ProgramacionCampo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script>
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

        }

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cargarProgramarFechaSubir);
            //cargarProgramarFechaSubir
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cargarProgramarFecha);
            //cargarProgramarFecha();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cargarFechaNacimiento);
            //cargarFechaNacimiento();

            loadPlugins();

            $('#ProgramarCita').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Programar Campo",
                    width: "600px"
                });

            $('#ProgramarCita').parent().appendTo("form");

            $('#CambiarCampo').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Actualizar Campo",
                    width: "600px"
                });

            $('#CambiarCampo').parent().appendTo("form");

            $('#ObservacionesCampo').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Observaciones de Campo",
                    width: "600px"
                });

            $('#ObservacionesCampo').parent().appendTo("form");


            $('#subirDatosEntrevistados').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Subir Datos Entrevistados",
                    width: "600px"
                });

            $('#subirDatosEntrevistados').parent().appendTo("form");
        });

        function cargarFechaNacimiento() {
            $("#CPH_Content_txtFechaNacimiento").mask("99/99/9999");
            $("#CPH_Content_txtFechaNacimiento").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function calcularFecha() {
            var fecha = $("#CPH_Content_txtFechaNacimiento").val();
            if (fecha != "__/__/____" && fecha != "") {
                var hoy = new Date();
                var parts = fecha.split('/');
                var cumpleanos = new Date(parts[2], parts[1] - 1, parts[0]);
                var edad = hoy.getFullYear() - cumpleanos.getFullYear();
                var m = hoy.getMonth() - cumpleanos.getMonth();

                if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
                    edad--;
                }
                $("#CPH_Content_txtEdad").val(edad);
            }
        }
        
        function cargarProgramarFecha() {
            $("#CPH_Content_txtProgramarFecha").mask("99/99/9999");
            $("#CPH_Content_txtProgramarFecha").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                minDate: 0,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
        
        function cargarProgramarFechaSubir() {
            $("#CPH_Content_txtFechaSubir").mask("99/99/9999");
            $("#CPH_Content_txtFechaSubir").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                minDate: 0,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function abrirProgramar() {
            cerrarObservaciones();
            $('#ProgramarCita').dialog("open");
        }

        function cerrarProgramar() {
            $('#ProgramarCita').dialog("close");
        }


        function abrirCambiar() {
            $('#CambiarCampo').dialog("open");
        }

        function cerrarCambiar() {
            $('#CambiarCampo').dialog("close");
        }

        function abrirObservaciones() {
            cerrarProgramar();
            $('#ObservacionesCampo').dialog("open");
        }

        function cerrarObservaciones() {
            $('#ObservacionesCampo').dialog("close");
        }

        function abrirsubirDatos() {
            $('#subirDatosEntrevistados').dialog("open");
        }

        function cerrarsubirDatos() {
            $('#subirDatosEntrevistados').dialog("close");
        }
        function bloquearUI() {
            $.blockUI({ message: "Procesando ...." });
        }
    </script>
    <style>
        #stylized label,
        #stylized select {
            width: auto;
        }

        #stylized input {
            width: inherit;
        }

        .cssButton {
            width: 150px !important;
        }

        .text-center {
            margin: 0px auto;
            text-align: center;
            float: none !important;
        }

            .text-center input[type=image] {
                outline: none !important;
            }

        .obs-rojo {
            background-color: red;
            color: white;
        }

        .obs-verde {
            background-color: green;
            color: white;
        }

        .text-clear {
            display: initial;
            font-weight: normal;
        }

            .text-clear label {
                display: initial;
                font-weight: normal;
                float: left;
                margin-right: 10px;
            }

        .mt10 {
            margin-top: 10px;
        }

        .m10 {
            margin: 10px;
        }

        .izq {
            float: left;
        }

        .der {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu">
        <li>
            <a href="#">GERENCIA OP</a>
            <ul>
                <li><a title="Asignación de coordinación de estudios" href="../RE_GT/AsignacionCOE.aspx">AsignarOMP</a></li>
                <li><a title="Asignación JBI a Proyectos" href="../RE_GT/AsignacionJBI.aspx">AsignarJBI</a></li>
                <li><a title="Revisar Presupuestos" href="../CU_Cuentas/RevisionPresupuestos.aspx">RevisarPresupuestos</a></li>
                <li><a title="AjustarCostos" href="../CAP/PresupuestosAprobados.aspx?opt=2">AjustarCostos</a></li>
                <li><a title="Trabajos por gerencia" href="../RP_Reportes/TrabajosPorGerencia.aspx">Seguimiento</a></li>
                <li><a title="Cambios de JobBook Interno" href="../RE_GT/CambiosJBI.aspx">Cambios JBI</a></li>
            </ul>
        </li>
        <li>
            <a href="#">PLANEACIÓN</a>
            <ul>
                <li><a title="Planeación Operaciones General" href="../RP_Reportes/PlaneacionGeneralOperaciones.aspx">Planeación Encuestas y Encuestadores</a></li>
                <li><a title="Listado propuestas" href="../RP_Reportes/ListadoPropuestasSeguimiento.aspx" target="_blank">Seguimiento de Propuestas</a></li>
                <li><a title="Listado anuncios de aprobación" href="../RP_Reportes/ListadoEstudiosSeguimiento.aspx" target="_blank">Seguimiento de Anuncios</a></li>
                <li><a title="Trabajos atrasados" href="../RP_Reportes/TrabajosConAtraso.aspx" target="_blank">Trabajos atrasados</a></li>
                <li><a title="Listado encuestadores - Ficha Encuestador" href="../RP_Reportes/ListadoEncuestadores.aspx" target="_blank">Ficha de Encuestador</a></li>
                <li><a title="Tiempos de revisión de presupuestos" href="../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx" target="_blank">Tiempos revisión</a></li>

            </ul>
        </li>
        <li>
            <a href="#">COORDINACIÓN</a>
            <ul>
                <li><a title="Asignación de coordinación de estudios" href="AsignacionCoordinador.aspx?TipoTecnicaid=1">Asignar Coordinador de Campo</a></li>
                <li><a title="Personal PST-Contratistas" href="../TH_TalentoHumano/PrestacionServicios-CT.aspx" target="_blank">Personal PST-Contratistas</a></li>
            </ul>
        </li>
        <li>
            <a href="#">CAMPO</a>
            <ul>
                <li><a title="Encuestadores y supervisores sin producción" href="../RP_Reportes/PersonalSinProduccion.aspx">Personas sin producción</a></li>
            </ul>
        </li>
        <li>
            <a href="#">OP CUANTI</a>
            <ul>
                <li>
                    <a href="#">OMP</a>
                    <ul>
                        <li><a title="Trabajos" href="../op_cuantitativo/Trabajos.aspx" runat="server" id="liTrabajosCuanti">Trabajos</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Coordinador Campo</a>
                    <ul>
                        <li><a title="Trabajos" href="../op_cuantitativo/TrabajosCoordinador.aspx">Trabajos</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Call Center</a>
                    <ul>
                        <li><a title="Trabajos" href="../OP_CUANTITATIVO/TrabajosCallCenter.aspx">Trabajos</a></li>
                        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=14&RolId=49&URLRetorno=18" target="_blank">Gestionar tareas</a></li>
                        <li><a title="Cargar productividad" href="ImportarDatos.aspx">Cargar productividad</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Scripting</a>
                    <ul>
                        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=11&RolId=28&URLRetorno=5" target="_blank">Gestionar tareas</a></li>
                        <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=112&ProcesoId=10" target="_blank">Trafico tareas</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Mystery</a>
                    <ul>
                        <li><a title="Cargar productividad" href="../OP_CUANTITATIVO/ImportarDatos.aspx">Cargar productividad</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">RMC</a>
                    <ul>
                        <li><a title="Cargar productividad" href="../OP_Cuantitativo/ImportarDatos.aspx">Cargar productividad</a></li>
                        <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=38">Tráfico y Recursos</a></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li>
            <a href="#">OP CUALI</a>
            <ul>
                <%--<li><a href="../OP_Cualitativo/Trabajos.aspx" runat="server" id="liTrabajosCuali">Trabajos</a></li>--%>
                <li><a href="../OP_Cualitativo/TrabajosCoordinador.aspx">Trabajos</a></li>
                <li><a title="Calendario" href="../OP_Cualitativo/Calendario.aspx" runat="server" id="liCalendarioCuali">Calendario</a></li>
            </ul>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Programación de Campo
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <br />
    <br />
    <div class="row">
        <div class="col-md-12">
            <asp:HiddenField ID="hfId" runat="server" />
            <asp:HiddenField ID="hfEntrevistadoId" runat="server" />
            <asp:HiddenField ID="hfTrabajoId" runat="server" />

            <div id="accordion">
                <div id="accordion0">
                    <h3><a href="#">Programación de Campo - Lista </a></h3>
                    <div class="block">
                        <div class="form-horizontal">
                            <div class="col-md-11">
                                <div class="form-group col-md-11">
                                    <h4 style="width: auto; text-align: left;">Buscar por Nombre del Entrevistado:</h4>
                                </div>
                                <div class="form-group col-md-3">
                                    <asp:TextBox runat="server" Width="85%" class="form-control" ID="txtSearchNombre"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-8">
                                    <asp:UpdatePanel runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnDescargar" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Button Text="Buscar" CssClass="cssButton" runat="server" ID="btnSearchNombre" />
                                            <asp:Button Text="Nuevo" CssClass="cssButton" runat="server" ID="btnNuevo" />
                                            <asp:Button Text="Subir" CssClass="cssButton" runat="server" ID="btnSubir" OnClientClick="abrirsubirDatos();" Visible="false" />
                                            <asp:Button Text="Descargar Excel" CssClass="cssButton der" runat="server" ID="btnDescargar" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <br />
                        <asp:UpdatePanel ID="pnlBusqueda" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvCamposProgramados" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id, EntrevistadoId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="EntrevistadoId" HeaderText="Id" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                        <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                        <asp:BoundField DataField="Audio" HeaderText="Audio" />
                                        <asp:BoundField DataField="Transcripcion" HeaderText="Transcripción" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                                        <asp:BoundField DataField="Hora" HeaderText="Hora" DataFormatString="{0:hh\:mm}" HtmlEncode="False" />
                                        <asp:BoundField DataField="Moderador" HeaderText="Moderador" />
                                        <asp:BoundField DataField="NumeroObservaciones" HeaderText="Número Obs" ItemStyle-CssClass="text-center" />
                                        <asp:TemplateField HeaderText="Programar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgProgramar" runat="server" CausesValidation="False" CssClass="text-center" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Programar" ImageUrl="~/Images/Select_16.png" ToolTip="Programar" Width="16px" OnClientClick="abrirProgramar();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modificar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgModificar" runat="server" CausesValidation="False" CssClass="text-center" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Modificar" ImageUrl="~/Images/edit.png" ToolTip="Modificar Entrevistado" Width="16px" />
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
                                                            Enabled='<%# IIf(gvCamposProgramados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                            Enabled='<%# IIf(gvCamposProgramados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <span class="pagingLinks">[<%= gvCamposProgramados.PageIndex + 1%>-
                                                    <%= gvCamposProgramados.PageCount%>]</span>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                            Enabled='<%# IIf((gvCamposProgramados.PageIndex + 1) = gvCamposProgramados.PageCount, "false", "true") %>'
                                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                            Enabled='<%# IIf((gvCamposProgramados.PageIndex + 1) = gvCamposProgramados.PageCount, "false", "true") %>'
                                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </div>
                </div>
                <div id="accordion1">
                    <h3><a href="#">Programación de Campo - Detalle</a></h3>
                    <div class="block">
                        <h4>Agregar datos de Programación de Campo</h4>
                        <br />
                        <br />
                        <div class="form-horizontal">
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Nombre</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="50%" ID="txtNombre"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">CC</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="40%" ID="txtCC"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Teléfono de residencia</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="30%" ID="txtTelefono"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Celular</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="40%" ID="txtCelular"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Fecha de Nacimiento</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="30%" ID="txtFechaNacimiento"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Edad</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" TextMode="Number" Width="40%" ID="txtEdad"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Estado Civil</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" class="form-control" Width="30%" ID="ddlEstadoCivil">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Género</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" class="form-control" Width="40%" ID="ddlSexo">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Ciudad</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="50%" ID="txtCiudadResidencia"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Estrato</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" class="form-control" Width="40%" ID="ddlEstrato">
                                                <asp:ListItem Text="--Seleccione--" Value="-1" />
                                                <asp:ListItem Text="NSE1" Value="1" />
                                                <asp:ListItem Text="NSE2" Value="2" />
                                                <asp:ListItem Text="NSE3" Value="3" />
                                                <asp:ListItem Text="NSE4" Value="4" />
                                                <asp:ListItem Text="NSE5" Value="5" />
                                                <asp:ListItem Text="NSE6" Value="6" />
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Dirección</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="50%" ID="txtDireccion"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Barrio</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="40%" ID="txtBarrio"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Correo Electrónico</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="50%" ID="txtCorreo"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5 hidden">
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Perfil</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" class="form-control" Width="50%" ID="txtPerfil"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group col-md-11">
                                    <label style="width: 80px; text-align: right;">Nivel de escolaridad</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" class="form-control" Width="40%" ID="ddlNivelEducativo">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <label style="width: 80px; text-align: right;">Reclutador</label>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" class="form-control" Width="50%" ID="ddlReclutador">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-5 hidden">
                            </div>
                            <br />
                            <br />
                            <div class="col-md-6">
                                <div class="form-group col-md-12">
                                    <div style="margin: 0 70px;">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Button Text="Guardar" ID="btnSave" runat="server" />
                                                <asp:Button Text="Limpiar" ID="btnLimpiar" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div id="ProgramarCita">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="divProgramarCita">
                    <h4>Fecha y Hora de Programación</h4>
                    <div class="form-group col-md-5">
                        <asp:TextBox runat="server" CssClass="form-control col-md-5" ID="txtProgramarFecha" Height="25px"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:TextBox runat="server" TextMode="Time" CssClass="form-control col-md-5" ID="txtProgramarHora" Height="25px"></asp:TextBox>
                    </div>
                    <br />
                    <br />
                </asp:Panel>
                <asp:Panel runat="server" ID="divModerador">
                    <h4 style="padding: 15px 0;">Moderador</h4>
                    <div class="form-group col-md-11">
                        <asp:DropDownList runat="server" CssClass="form-control" Width="60%" ID="ddlModerador">
                        </asp:DropDownList>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="divAudioTranscripcion">
                    <div class="m10">
                        <asp:CheckBox CssClass="text-clear" Text="¿Se realizó la entrevista?:  " ID="chbRealizado" runat="server" />
                    </div>
                    <div class="m10">
                        <asp:CheckBox CssClass="text-clear m10" Text="¿Se recibió el Audio?:  " ID="chbAudio" runat="server" />
                    </div>
                    <div class="m10">
                        <asp:CheckBox CssClass="text-clear m10" Text="¿Se recibió la Transcripción?:  " ID="chbTranscripcion" runat="server" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <h4 class="text-clear">Observaciones</h4>
                <div class="form-group col-md-11">
                    <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtProgramarObservacion" Rows="5" />
                </div>
                <div class="form-group col-md-11">
                    <asp:DropDownList runat="server" ID="ddlEstadoCancelar" AutoPostBack="true" CssClass="form-control" Width="50%" Visible="false">
                        <asp:ListItem Text="¿Quién Cancela?" Value="-1" />
                        <asp:ListItem Text="Cancela Ipsos" Value="6" />
                        <asp:ListItem Text="Cancela Entrevistado" Value="7" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-11" style="text-align: right;">
                    <asp:Button Text="Ver Observaciones" runat="server" ID="btnObservaciones" OnClientClick="abrirObservaciones()" CssClass="btn btn-primary" />
                    <asp:Button Text="Guardar" runat="server" ID="btnSaveProgramar" CssClass="btn btn-primary" />
                    <asp:Button Text="Cancelar" runat="server" ID="btnCancelar" CssClass="btn btn-warning" Visible="false" />
                    <asp:Button Text="Cerrar" runat="server" ID="btnCerrar" CssClass="btn btn-danger" OnClientClick="cerrarProgramar()" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ObservacionesCampo">
        <asp:UpdatePanel ID="udpObservacionesCampo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvObservaciones" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="5"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="Observador" HeaderText="Observación" />
                        <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha Creación" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                    </Columns>
                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                            Enabled='<%# IIf(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIf(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvObservaciones.PageIndex + 1%>-
                                                    <%= gvObservaciones.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIf((gvObservaciones.PageIndex + 1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIf((gvObservaciones.PageIndex + 1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Último »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
                <div class="row">
                    <div class="form-group col-md-11" style="text-align: right; margin: 15px;">
                        <asp:Button Text="Cerrar" runat="server" ID="btnCerrarObs" CssClass="btn btn-primary" OnClientClick="cerrarObservaciones()" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="subirDatosEntrevistados">
        <h4>Cargar listado de Entrevistado:  </h4>
        <asp:FileUpload runat="server" ID="FileUpData" />
        <br />
        <h4>Fecha y Hora de Programación</h4>
        <div class="form-group col-md-5">
            <asp:TextBox runat="server" CssClass="form-control col-md-5" ID="txtFechaSubir" Height="25px"></asp:TextBox>
        </div>
        <div class="form-group col-md-3">
            <asp:TextBox runat="server" TextMode="Time" CssClass="form-control col-md-5" ID="txtHoraSubir" Height="25px"></asp:TextBox>
        </div>
        <br />
        <br />
        <h4 style="padding: 15px 0;">Moderador</h4>
        <div class="form-group col-md-11">
            <asp:DropDownList runat="server" CssClass="form-control" Width="60%" ID="ddlModeradorSubir">
            </asp:DropDownList>
        </div>
        <div class="m10">
            <asp:CheckBox CssClass="text-clear" Text="Todos tienen Audio:  " ID="chbTodosAudio" runat="server" />
        </div>
        <div class="m10">
            <asp:CheckBox CssClass="text-clear m10" Text="Todos tienen Transcripción:  " ID="chbTodosTranscripcion" runat="server" />
        </div>
        <div style="float: right;">
            <asp:Button Text="Subir" ID="btnSubirData" CssClass="btn btn-primary" runat="server" OnClientClick="bloquearUI()" />
            <asp:Button Text="Cerrar" CssClass="btn btn-danger" runat="server" OnClientClick="cerrarsubirDatos()" />
        </div>
    </div>
</asp:Content>
