<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="ReporteInconsistencias.aspx.vb" Inherits="WebMatrix.ReporteInconsistencias" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTerminacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTerminacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Reporte Observaciones</a>
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
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Registros Exportados<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left2">
                            <fieldset>
                                <label>
                                    Fecha de Inicio</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left2">
                            <fieldset>
                                <label>
                                    Fecha de Finalización</label>
                                <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left2">
                            <a>Tareas</a><asp:DropDownList ID="ddlTareas" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="--Ver todas--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Instrumentos" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Revisión de instrumentos" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Campo" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Critica" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Verificación" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Captura - Generación preguntas abiertas" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Captura - Archivo BarbWin" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Codificación" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Revisión y aprobación codificación" Value="9"></asp:ListItem>
                            <asp:ListItem Text="DataCleaning" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - PDC" Value="12"></asp:ListItem>
                            <asp:ListItem Text="Procesamiento total" Value="13"></asp:ListItem>
                            <asp:ListItem Text="Procesamiento preguntas cerradas" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Elaboración informe" Value="16"></asp:ListItem>
                            <asp:ListItem Text="Cuentas - Revisión de informe" Value="17"></asp:ListItem>
                            <asp:ListItem Text="Scripting - RMC" Value="18"></asp:ListItem>
                            <asp:ListItem Text="Pilotos Scripting" Value="19"></asp:ListItem>
                            <asp:ListItem Text="Scripting" Value="20"></asp:ListItem>
                            <asp:ListItem Text="Captura - Digitación / Transformación" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Estadistica - Ponderación" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Estadistica - Metodología" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Gestion de Citas" Value="24"></asp:ListItem>
                            <asp:ListItem Text="Depuracion Listado de Contactos" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Intrumentos" Value="26"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Revisión de instrumentos" Value="27"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Reclutamiento" Value="28"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Alistamiento logístico" Value="29"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Campo" Value="30"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Transcripciones" Value="31"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Control calidad transcripciones" Value="32"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Elaboración informe" Value="33"></asp:ListItem>
                            <asp:ListItem Text="Cualitativo - Revisión de informe" Value="34"></asp:ListItem>
                            <asp:ListItem Text="Call Center Generacion Preguntas Abiertas" Value="35"></asp:ListItem>
                            <asp:ListItem Text="Call Center Generacion de Base" Value="36"></asp:ListItem>
                            <asp:ListItem Text="Estadistica - Proceso especiales" Value="37"></asp:ListItem>
                            <asp:ListItem Text="DataCleaning - Preguntas Cerradas" Value="38"></asp:ListItem>
                            <asp:ListItem Text="Datacleaning - Generación datos para ponderar" Value="39"></asp:ListItem>
                            <asp:ListItem Text="Campo - Pilotos" Value="40"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Ajustes después de pilotos de campo" Value="41"></asp:ListItem>
                            <asp:ListItem Text="Campo - Capacitación" Value="42"></asp:ListItem>
                            <asp:ListItem Text="Campo - Revisión primer día de campo" Value="43"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Ajustes después de capacitación a campo" Value="44"></asp:ListItem>
                            <asp:ListItem Text="DataCleaning - Generación informe de cuotas" Value="45"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Revisión y aprobación de la muestra" Value="46"></asp:ListItem>
                            <asp:ListItem Text="DataCleaning - Aplicar ponderadores" Value="47"></asp:ListItem>
                            <asp:ListItem Text="Estadistica - Aprobación ponderación" Value="48"></asp:ListItem>
                            <asp:ListItem Text="Online - Envio de correos" Value="49"></asp:ListItem>
                            <asp:ListItem Text="Proyectos - Piloto scripting" Value="50"></asp:ListItem>
                            <asp:ListItem Text="Seleccion de IDMs" Value="51"></asp:ListItem>

                            </asp:DropDownList>
                        </div>
                        <div class="form_left">
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                            <asp:button ID="btnExport" runat="server" Text="Exportar" />
                        </div>
                        <div class="actions">
                            <asp:GridView ID="gvProduccion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="50"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar"  DataSourceID="SQLDsDatos" DataKeyNames="id">
                                <Columns>
                                    <asp:BoundField DataField="MES" HeaderText="MES" ReadOnly="True" SortExpression="MES" />
                                    <asp:BoundField DataField="AÑO" HeaderText="AÑO" SortExpression="AÑO" ReadOnly="True" />
                                    <%--<asp:BoundField DataField="id" HeaderText="id" SortExpression="id" InsertVisible="False" ReadOnly="True" />--%>
                                    <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" SortExpression="TrabajoId" />
                                    <%--<asp:BoundField DataField="TareaId" HeaderText="TareaId" SortExpression="TareaId" />--%>
                                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" SortExpression="Tarea" />
                                    <asp:BoundField DataField="FechaHoraObservacion" HeaderText="FechaHoraObservacion" SortExpression="FechaHoraObservacion" />
                                    <asp:BoundField DataField="Instrumento" HeaderText="Instrumento" SortExpression="Instrumento" />
                                    <asp:BoundField DataField="Pregunta" HeaderText="Pregunta" SortExpression="Pregunta" />
                                    <asp:BoundField DataField="Aplicativo" HeaderText="Aplicativo" SortExpression="Aplicativo" />
                                    <asp:BoundField DataField="Proceso" HeaderText="Proceso" SortExpression="Proceso" />
                                    <asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
                                    <asp:BoundField DataField="DescripcionObservacion" HeaderText="DescripcionObservacion" SortExpression="DescripcionObservacion" />
                                    <asp:BoundField DataField="VersionScript" HeaderText="Version" SortExpression="VersionScript" />
                                    <asp:BoundField DataField="FechaHoraRevision" HeaderText="FechaHoraRevision" SortExpression="FechaHoraRevision" />
                                    <asp:BoundField DataField="Rechazar" HeaderText="Rechazar" SortExpression="Rechazar" />
                                    <asp:BoundField DataField="RespuestaRevisor" HeaderText="RespuestaRevisor" SortExpression="RespuestaRevisor" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                    <asp:BoundField DataField="UsuarioRegistra" HeaderText="UsuarioRegistra" ReadOnly="True" SortExpression="UsuarioRegistra" />
                                    <asp:BoundField DataField="UsuarioRevisor" HeaderText="UsuarioRevisor" ReadOnly="True" SortExpression="UsuarioRevisor" />
                                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" ReadOnly="True" SortExpression="JobBook" />
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" ReadOnly="True" SortExpression="NombreTrabajo" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="Unidad" />
                                    <asp:BoundField DataField="UsuarioAsignado" HeaderText="UsuarioAsignado" ReadOnly="True" SortExpression="UsuarioAsignado" />
                                    <asp:BoundField DataField="COE" HeaderText="OMP" ReadOnly="True" SortExpression="COE" />
                                    <asp:BoundField DataField="GerenteProyecto" HeaderText="GerenteProyecto" ReadOnly="True" SortExpression="GerenteProyecto" />
                                </Columns>
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                
                                <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                            Enabled='<%# IIF(gvProduccion.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvProduccion.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvProduccion.PageIndex + 1%>-<%= gvProduccion.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvProduccion.PageIndex +1) = gvProduccion.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvProduccion.PageIndex +1) = gvProduccion.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                            </asp:GridView>
                            <asp:GridView ID="gvExport" runat="server" Width="100%" 
                                AutoGenerateColumns="False"  Visible="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" 
                                EmptyDataText="No existen registros para mostrar" DataSourceID="SQLDsDatos" DataKeyNames="id">
                                <Columns>
                                    <asp:BoundField DataField="MES" HeaderText="MES" ReadOnly="True" SortExpression="MES" />
                                    <asp:BoundField DataField="AÑO" HeaderText="AÑO" SortExpression="AÑO" ReadOnly="True" />
                                    <%--<asp:BoundField DataField="id" HeaderText="id" SortExpression="id" InsertVisible="False" ReadOnly="True" />--%>
                                    <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" SortExpression="TrabajoId" />
                                    <%--<asp:BoundField DataField="TareaId" HeaderText="TareaId" SortExpression="TareaId" />--%>
                                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" SortExpression="Tarea" />
                                    <asp:BoundField DataField="FechaHoraObservacion" HeaderText="FechaHoraObservacion" SortExpression="FechaHoraObservacion" />
                                    <asp:BoundField DataField="Instrumento" HeaderText="Instrumento" SortExpression="Instrumento" />
                                    <asp:BoundField DataField="Pregunta" HeaderText="Pregunta" SortExpression="Pregunta" />
                                    <asp:BoundField DataField="Aplicativo" HeaderText="Aplicativo" SortExpression="Aplicativo" />
                                    <asp:BoundField DataField="Proceso" HeaderText="Proceso" SortExpression="Proceso" />
                                    <asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
                                    <asp:BoundField DataField="DescripcionObservacion" HeaderText="DescripcionObservacion" SortExpression="DescripcionObservacion" />
                                    <asp:BoundField DataField="VersionScript" HeaderText="Version" SortExpression="VersionScript" />
                                    <asp:BoundField DataField="FechaHoraRevision" HeaderText="FechaHoraRevision" SortExpression="FechaHoraRevision" />
                                    <asp:BoundField DataField="Rechazar" HeaderText="Rechazar" SortExpression="Rechazar" />
                                    <asp:BoundField DataField="RespuestaRevisor" HeaderText="RespuestaRevisor" SortExpression="RespuestaRevisor" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                    <asp:BoundField DataField="UsuarioRegistra" HeaderText="UsuarioRegistra" ReadOnly="True" SortExpression="UsuarioRegistra" />
                                    <asp:BoundField DataField="UsuarioRevisor" HeaderText="UsuarioRevisor" ReadOnly="True" SortExpression="UsuarioRevisor" />
                                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" ReadOnly="True" SortExpression="JobBook" />
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" ReadOnly="True" SortExpression="NombreTrabajo" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="Unidad" />
                                    <asp:BoundField DataField="UsuarioAsignado" HeaderText="UsuarioAsignado" ReadOnly="True" SortExpression="UsuarioAsignado" />
                                    <asp:BoundField DataField="COE" HeaderText="OMP" ReadOnly="True" SortExpression="COE" />
                                    <asp:BoundField DataField="GerenteProyecto" HeaderText="GerenteProyecto" ReadOnly="True" SortExpression="GerenteProyecto" />
                                </Columns>
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                            </asp:GridView>
                            <br />
                            <asp:SqlDataSource ID="SQLDsDatos" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                                SelectCommand="REP_Inconsistencias" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtFechaInicio" DbType="Date" Name="FECHAI" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtFechaTerminacion" DbType="Date" Name="FECHAF" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="ddlTareas" Name="Tarea" PropertyName="SelectedValue" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <br />
                        </div>
                    </div>
                    <%--items--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
