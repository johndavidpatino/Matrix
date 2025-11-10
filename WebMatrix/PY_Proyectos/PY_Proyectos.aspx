<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master"
    CodeBehind="PY_Proyectos.aspx.vb" Inherits="WebMatrix.Form_PY_Proyectos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            loadplugins();
        });

        function loadplugins() {
            $('#tabs').tabs();
            $('#accordionFrame').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEsquema').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionPropuesta').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificaciones').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificacionesAdicionales').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $("#<%= txtBCPFechaBDD.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaBDD.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtBCPFechaConceptos.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaConceptos.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtBCPFechaCuestionario.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaCuestionario.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtBCPFechaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtBCPFechaInformeCuentas.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaInformeCuentas.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtBCPFechaInformeCliente.ClientId %>").mask("99/99/9999");
            $("#<%= txtBCPFechaInformeCliente.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#VersionesEspCuanti').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones Especificaciones de Cuentas a Proyectos",
                    width: "600px"
                });

            $('#VersionesEspCuanti').parent().appendTo("form");

            $('#VersionesEspCuali').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones Especificaciones de Cuentas a Proyectos",
                    width: "600px"
                });

            $('#VersionesEspCuali').parent().appendTo("form");
        }

        function MostrarVersionesEspCuali(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#VersionesEspCuali').dialog("open");
            }
        }

        function MostrarVersionesEspCuanti(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#VersionesEspCuanti').dialog("open");
            }
        }

    </script>
    <style>
        #stylized.leftAuto {
            text-align: initial;
        }

        #stylized label {
            text-align: left;
            margin-left: 10px;
            width: auto;
        }

        #stylized input[type=radio] {
            margin: 10px;
        }

        #stylized input[type=checkbox] {
            margin-top: 7px;
        }

        .ui-widget-header {
            background-color: #3b9f9a;
            border: 1px solid #3b9f9a;
        }

        .text-center {
            margin: 0px auto;
            text-align: center;
        }

        .cambioVersion1 {
            background-color: #ea7b7e;
            color: white;
            border: 1px solid #000 !important;
        }

        .cambioVersion {
            /*background-color: #52bb69;*/
            background-color: transparent;
            color: black;
            border: 1px solid #000 !important;
        }

        .lblScroll {
            overflow-x: scroll;
            overflow-y: scroll;
            border: 1px solid;
        }

        .lblScrolllbl {
            overflow-x: scroll;
            padding: 10px;
            border: 1px solid;
        }

        .versionIgual {
            background-color: transparent;
            color: black;
            border: 1px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Proyectos 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    En esta sección puede ver los proyectos que le han sido asignados. Complete la información
    del Brief de Cuentas a Proyectos, donde únicamente deberá diligenciar algunas especificaciones, ya que
    la información se trae directamente desde el Frame y el presupuesto aprobado.
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            loadplugins();
        }
    </script>
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver" Visible="false"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
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
            <asp:Panel runat="server" ID="accordion0">
                <h3 style="float: left; text-align: left;">
                    <a>
                        <label>
                            Proyectos
                            <asp:HiddenField ID="hfIdProyecto" runat="server" />
                            <asp:HiddenField ID="hfIdPropuesta" runat="server" />
                            <asp:HiddenField ID="hfIdEspCuenta" runat="server" />
                        </label>
                    </a>
                </h3>
                <div>
                    <label>
                        Titulo</label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    <asp:GridView ID="gvProyectos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id, EstudioId, TipoProyectoId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                            <asp:BoundField DataField="GP_Nombres" HeaderText="GerenteProyectos" />
                            <asp:BoundField DataField="TipoProyecto" HeaderText="Tipos proyectos" />
                            <asp:TemplateField HeaderText="Informacion" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgInformacion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Informacion" ImageUrl="~/Images/info_16.png" Text="Trabajos" ToolTip="Trabajos" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Trabajos" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrTrabajos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Trabajos" ImageUrl="~/Images/application_view_detail.png" Text="Trabajos" ToolTip="Trabajos" />
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
                                                Enabled='<%# IIf(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvProyectos.PageIndex + 1%>-<%= gvProyectos.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIf((gvProyectos.PageIndex + 1) = gvProyectos.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIf((gvProyectos.PageIndex + 1) = gvProyectos.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="accordion1" Visible="false">
                <div>
                    <asp:Panel ID="pnlBrief" runat="server" Visible="false">
                        <div class="actions"></div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    0. Titulo:</label>
                                <asp:TextBox ID="txtTitulo" Width="100%"
                                    runat="server" />
                            </fieldset>
                        </div>
                        <div class="actions"></div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    1. Antecedentes y Problema de Marketing:</label>
                                <asp:Label ID="txtAntecedentes"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    2. Objetivos de la Investigación</label>
                                <asp:Label ID="txtObjetivos" runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    3. Action Standards
                                </label>
                                <asp:Label ID="txtActionStandard"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    4. Metodología</label>
                                <asp:Label ID="txtMetodologia"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    5. Target group de la Investigación de Mercado
                                </label>
                                <asp:Label ID="txtTargetGroup"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    6. Tiempos
                                </label>
                                <asp:Label ID="txtTiempos" runat="server" />
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    7. Presupuestos
                                </label>
                                <asp:Label ID="txtPresupuesto"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    8. Materiales disponibles</label>
                                <asp:Label ID="txtMateriales"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    9. Resultados de estudios anteriores</label>
                                <asp:Label ID="txtEstudiosAnteriores"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    10. Formato requerido por el cliente para el informe
                                </label>
                                <asp:Label ID="txtFormatos" runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    11. Aprobaciones
                                </label>
                                <asp:Label ID="txtAprobaciones"
                                    runat="server" />
                            </fieldset>
                            <fieldset>
                                <label>
                                    12. Competencia
                                </label>
                                <asp:Label ID="txtCompetencia"
                                    runat="server" />
                            </fieldset>
                        </div>

                    </asp:Panel>
                    <div id="accordionFrame">
                        <h2>Información del frame</h2>
                        <asp:Panel ID="pnlFrame" runat="server" Visible="false">
                            <div class="spacer"></div>
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tabs-1">Objetivos de Negocio</a></li>
                                    <li><a href="#tabs-2">Decisiones</a></li>
                                    <li><a href="#tabs-3">Competencia</a></li>
                                    <li><a href="#tabs-4">Metodología</a></li>
                                    <li><a href="#tabs-5">Datos de Diseño</a></li>
                                    <li><a href="#tabs-6">Tiempos y Presupuesto</a></li>
                                    <li><a href="#tabs-7">Entregables</a></li>
                                </ul>
                                <div id="tabs-1">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                            <p>¿Qué necesidad de negocios está tratando de resolver?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO1" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuáles son sus objetivos estratégicos para este estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO2" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Hay algún factor importante a considerar para el diseño del estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO3" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿El problema planteado está relacionado a alguna área en específico de la empresa?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO4" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Por qué está haciendo este estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO5" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Qué hipótesis tiene sobre el tema (s) está estudiando?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO6" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuáles son los indicadores clave de rendimiento requerido?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtO7" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="tabs-2">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Qué decisiones va a tomar como resultado de este estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtD1" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Quién va a utilizar esta investigación y qué van a hacer con él?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtD2" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Hay alguna política de su empresa /reglas que se tienen que considerar ir, sin cambio en el precio, empaquetado, etc. protocolo global?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtD3" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="tabs-3">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuáles son los desafíos que enfrenta su  negocio en el mercado? ¿Cómo es el desempeño de la industria?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtC1" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Quiénes son sus competidores? ¿Necesita alguna evaluación comparativa del estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtC2" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuál es el posicionamiento de su marca y cuáles son las diferencias vs. su competencia?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtC3" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Qué actividades recientes hay en el mercado que podrían haber influido en el mercado?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtC4" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Dónde ve su oportunidad y las amenazas en su caso?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtC5" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div id="tabs-4">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Necesita comparar esta investigación contra alguna investigación previa?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtM1" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuál fue el diseño de esa investigación previa?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtM2" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuándo se realizó?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtM3" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="tabs-5">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿En dónde tiene pensado hacer el levantamiento?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI1" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuál es la definición de su target?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI2" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cómo segmenta su mercado? Por productos, por usuario, por zonas geográficas?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI3" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Tiene algún requisito de tamaño de la muestra?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI4" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Espera que la muestra debe sea representativa del mercado?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI5" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Tiene alguna metodología específica en mente, por ejemplo, cualitativa o cuantitativa?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI6" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Hay alguna información clave que necesite este en el reporte final?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI7" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="tabs-6">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Para cuándo necesita los resultados. ¿Hay alguna limitante de tiempo?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI8" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Para cuándo necesita la propuesta?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI9" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>¿Cuándo cree que se tomará para tomar una decisión para comisionar el proyecto?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI10" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Con la idea de dimensionar el alcance del estudio para usted, podría compartir con nosotros cuál es el presupuesto que ha destinado a este estudio?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI11" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="tabs-7">
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Por último, sólo quiero ver si usted tiene algún requerimiento especial en cuanto a formatos de entrega?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI12" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Topline de  resultados?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI13" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Presentación en ppt?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI14" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Transcripciones?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI15" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Presentaciones verbales?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI16" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Workshop?</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI17" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: both">
                                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                            <p>Otros, especifique:</p>
                                        </div>
                                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                            <asp:Label ID="txtDI18" runat="server" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <br />
                    <div id="accordionPropuesta">
                        <h2>Información de la Propuesta</h2>
                        <asp:Panel ID="pnlPropuesta" runat="server" Visible="false">
                            <asp:Button ID="btnDescargarPropuesta" runat="server" Text="Descargar propuesta" />
                            <div class="spacer"></div>
                            <p>Información General</p>
                            <asp:GridView ID="gvPropuestaInfoGeneral" runat="server" Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="NoPropuesta" HeaderText="No Prop" />
                                    <asp:BoundField DataField="Alternativa" HeaderText="Alt" />
                                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                                    <asp:BoundField DataField="GrupoObjetivo" HeaderText="Grupo Objetivo" />
                                    <asp:BoundField DataField="Productividad" HeaderText="Productividad" />
                                    <asp:BoundField DataField="Duracion" HeaderText="Duración" />
                                    <asp:BoundField DataField="DiasCampo" HeaderText="D Campo" />
                                    <asp:BoundField DataField="RequestHabeasData" HeaderText="Habeas Data" />
                                </Columns>
                            </asp:GridView>
                            <p>Información de Muestra</p>
                            <asp:GridView ID="gvPropuestaMuestra" runat="server" Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                </Columns>
                            </asp:GridView>
                            <p>Información de Preguntas</p>
                            <asp:GridView ID="gvPropuestaPreguntas" runat="server" Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                                    <asp:BoundField DataField="Abiertas" HeaderText="Abiertas" />
                                    <asp:BoundField DataField="AbiertasMultiples" HeaderText="Abiertas Multiples" />
                                    <asp:BoundField DataField="Cerradas" HeaderText="Cerradas" />
                                    <asp:BoundField DataField="CerradasMultiples" HeaderText="Cerradas Multiples" />
                                    <asp:BoundField DataField="Demograficos" HeaderText="Demograficos" />
                                    <asp:BoundField DataField="Otras" HeaderText="Otras" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <br />
                    <div id="accordionEspecificaciones" style="padding: 0,0,0,0; margin: 0,0,0,0">
                        <h2>Especificaciones de Cuentas a Proyectos</h2>
                        <asp:Panel ID="pnlBriefCuentasProyectos" runat="server" Visible="false">
                            <a href="#" id="lblVersionEspCuanti" style="text-decoration: none; float: right" onclick="MostrarVersionesEspCuanti(this)" runat="server">Sin Versiones</a>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Tipo de medición</p>
                                </div>
                                <div style="width: 70%; float: left; padding: 2px 2px 2px 2px;">
                                    <asp:RadioButtonList runat="server" ID="chbBCPMedicion" Width="100%" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text="Medición Puntual"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Medición Multifases"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Tracking Puntuales"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Tracking Continuo"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="width: 22%; float: left; padding: 2px 2px 2px 2px; margin-top: -10px;">
                                    <label>No. Olas</label>
                                    <asp:TextBox ID="txtBCPOlas" runat="server" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both; margin-top: 5px;">
                                <asp:CheckBox ID="chbBCPPilotos" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Pilotos" />
                            </div>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Pilotos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPPilotosEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both">
                                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 100%; clear: both">
                                <div style="width: 33%; float: left; display">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha BDD</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaBDD"></asp:TextBox>
                                    </div>
                                </div>

                                <div style="width: 33%; float: left;">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha Productos o Conceptos</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaConceptos"></asp:TextBox>
                                    </div>
                                </div>

                                <div style="width: 33%; float: left;">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha Cuestionario</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaCuestionario"></asp:TextBox>
                                    </div>
                                </div>

                                <div style="width: 33%; float: left;">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha Inicio de Campo</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaInicioCampo"></asp:TextBox>
                                    </div>
                                </div>

                                <div style="width: 33%; float: left;">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha Informe a Cuentas</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaInformeCuentas"></asp:TextBox>
                                    </div>
                                </div>

                                <div style="width: 33%; float: left;">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <label style="text-align: left; width: auto;">Fecha Informe a Cliente</label>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox runat="server" ID="txtBCPFechaInformeCliente"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 100%; float: left;">
                                <asp:Button ID="btnGuardar" Text="Guardar Especificaciones" runat="server" />
                                <asp:Button ID="btnCancelar" Text="Cancelar" runat="server" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlBriefCuentasProyectosCuali" runat="server" Visible="false">
                            <form id="formCuentasProyectosCuali">
                                <a href="#" id="lblVersionEspCuali" style="text-decoration: none; float: right" onclick="MostrarVersionesEspCuali(this)" runat="server">Sin Versiones</a>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Observaciones Generales</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtObservacionesCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Técnica</p>
                                    </div>
                                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:RadioButtonList runat="server" ID="chbBCPTecnicaCuali" Width="100%" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text="Entrevista"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Sesiones de grupo/Talleres"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Inmersiones"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Estudios online"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Otro"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div style="width: 18%; float: left; padding: 2px 2px 2px 2px; margin-top: -5px;">
                                        <label>Otra Técnica</label>
                                        <asp:TextBox ID="otraTecnica" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Especificaciones de Incentivos</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtBCPIncentivosEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Especificaciones de Base de Datos</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtBCPBDDEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Especificaciones de Productos o Conceptos</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtBCPProductoEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Método de reclutamiento</p>
                                    </div>
                                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:RadioButtonList runat="server" ID="chbBCPReclutamientoCuali" Width="100%" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text="Base de datos"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Convencional"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Referidos"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="En frío"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Especificaciones de Reclutamiento</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtBCPEspReclutamientoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 98%; clear: both; margin-top: 5px;">
                                    <asp:CheckBox ID="chbBCPEspProductoCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Especificaciones de Producto" />
                                </div>
                                <div style="width: 98%; clear: both; margin-top: 5px;">
                                    <asp:CheckBox ID="chbBCPMaterialEvalCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Materiales a Evaluar" />
                                </div>
                                <div style="width: 98%; clear: both">
                                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                                        <p>Observaciones sobre el Producto</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtBCPObsProductoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; float: left;">
                                    <asp:Button ID="btnGuardar2" Text="Guardar Especificaciones" runat="server" />
                                    <asp:Button ID="btnCancela2" Text="Cancelar" runat="server" />
                                </div>
                            </form>
                        </asp:Panel>
                    </div>
                    <br />
                    <%--<div id="divEspecificacionesAdicionales" runat="server" visible="false">
                        <div id="accordionEspecificacionesAdicionales" style="padding: 0,0,0,0; margin: 0,0,0,0;">
                            <h2>Especificaciones Adicionales a Operaciones</h2>
                            <asp:Panel ID="pnlEspecificacionesAdicionales" runat="server">
                                <p>Ayudas Adicionales</p>
                                <br />
                                <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                                </asp:CheckBoxList>
                                <br />
                                <p>Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</p>
                                <br />
                                <asp:TextBox ID="txtHabeasData" runat="server" CssClass="textMultiline" Width="100%" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <div style="width: 100%;">
                                    <p>Nombre de la moderadora(s)</p>
                                    <br />
                                    <asp:TextBox ID="TextBox1" runat="server" Width="50%"></asp:TextBox>
                                </div>
                                <br />
                                <div style="width: 100%; float: left;">
                                    <p>Variables de control</p>
                                    <br />
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="textMultiline" Width="100%" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>--%>
                    <br />
                    <br />
                    <asp:Button Text="Regresar" runat="server" ID="btnRegresar" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="VersionesEspCuanti">
        <asp:UpdatePanel ID="UPanelVersionesEspCuanti" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVerEspCuanti" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVerEspCuanti" runat="server" Width="95%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id,NoVersion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Codigo" Visible="false" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="NoVersion" HeaderText="Versión" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/application_view_detail.png" Text="Ver" ToolTip="Ver" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comparar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" OnClientClick="return true;"
                                                CommandName="Comparar" ImageUrl="~/Images/comparar_32.png" Text="Comparar con versión anterior" ToolTip="Comparar con versión anterior" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetalleVerEspCuanti" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Tipo de medición</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMedicionVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>No. Olas</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPOlasVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPPilotosVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Pilotos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPPilotosEspecificacionesVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspecificacionesVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspecificacionesVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspecificacionesVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha BDD</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaBDDVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Productos o Conceptos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaConceptosVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Cuestionario</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaCuestionarioVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Inicio de Campo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInicioCampoVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Informe a Cuentas</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeCuentasVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Informe a Cliente</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeClienteVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVerEspCuanti" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararVerEspCuanti" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVerEspCuanti" ForeColor="Red" />
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVerEspCuantiA" ForeColor="Red"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVerEspCuantiB" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Tipo de medición</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMedicionV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMedicionV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>No. Olas</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPOlasV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPOlasV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPPilotosV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPPilotosV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Pilotos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPPilotosEspecificacionesV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPPilotosEspecificacionesV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspecificacionesV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspecificacionesV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspecificacionesV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspecificacionesV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspecificacionesV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspecificacionesV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha BDD</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaBDDV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaBDDV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Productos o Conceptos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaConceptosV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaConceptosV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Cuestionario</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaCuestionarioV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaCuestionarioV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Inicio de Campo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInicioCampoV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInicioCampoV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Informe a Cuentas</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeCuentasV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeCuentasV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha Informe a Cliente</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeClienteV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPFechaInformeClienteV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVerEspCuanti2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="VersionesEspCuali">
        <asp:UpdatePanel ID="UPanelVersionesEspCuali" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVerEspCuali" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVerEspCuali" runat="server" Width="95%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id,NoVersion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Codigo" Visible="false" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="NoVersion" HeaderText="Versión" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/application_view_detail.png" Text="Ver" ToolTip="Ver" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comparar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" OnClientClick="return true;"
                                                CommandName="Comparar" ImageUrl="~/Images/comparar_32.png" Text="Comparar con versión anterior" ToolTip="Comparar con versión anterior" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetalleVerEspCuali" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtObservacionesCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaCualiVer" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otra Técnica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaVer" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Método de reclutamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoCualiVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Reclutamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Producto</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPEspProductoCualiVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Materiales a Evaluar</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMaterialEvalCualiVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones sobre el Producto</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoCualiVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVerEspCuali" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararVerEspCuali" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVerEspCuali" ForeColor="Red" />
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVerEspCualiA" ForeColor="Red"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVerEspCualiB" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtObservacionesCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtObservacionesCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaCualiV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaCualiV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otra Técnica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Método de reclutamiento</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoCualiV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoCualiV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Reclutamiento</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Producto</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPEspProductoCualiV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPEspProductoCualiV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Materiales a Evaluar</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMaterialEvalCualiV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPMaterialEvalCualiV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones sobre el Producto</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoCualiV1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoCualiV2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVerEspCuali2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
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
