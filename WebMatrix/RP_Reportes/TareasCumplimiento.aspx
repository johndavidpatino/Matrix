<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="TareasCumplimiento.aspx.vb" Inherits="WebMatrix.TareasCumplimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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

            $("#<%= txtFechaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFinalizacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

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
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                            Tareas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Unidad</label>
                                <asp:DropDownList ID="ddlUnidad" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </fieldset>
                            <fieldset>
                                <label>
                                    Gerente Proyectos</label>
                                <asp:DropDownList ID="ddlGerenteProyectos" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </fieldset>
                            <fieldset>
                                <label>
                                    Trabajo</label>
                                <asp:DropDownList ID="ddlTrabajo" runat="server">
                                </asp:DropDownList>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Gerencias OP</label>
                                <asp:DropDownList ID="ddlGerenciasOp" runat="server">
                                </asp:DropDownList>
                            </fieldset>
                            <fieldset>
                                <label>
                                    Fecha Inicial</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                            <fieldset>
                                <label>
                                    Fecha Final</label>
                                <asp:TextBox ID="txtFechaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </fieldset>
                        <fieldset>
                        <fieldset>
                                <label>
                                    Fechas</label>
                                <asp:DropDownList ID="ddlFechas" runat="server">
                                    <asp:ListItem Value="-1">Seleccione...</asp:ListItem>
                                    <asp:ListItem Value="1">Fechas Propuestas</asp:ListItem>
                                    <asp:ListItem Value="2">Fechas Reales</asp:ListItem>
                                </asp:DropDownList>

                            </fieldset>
                    
                </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Gerencias Ejecuta</label>
                                <asp:DropDownList ID="ddlGerenciaEjecuta" runat="server">
                                </asp:DropDownList>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <p>
                                <asp:Label ID="LblIndCum" runat="server" Text="Indicador Cumplimiento:  " /><asp:Label
                                    ID="IndCumplimiento" runat="server" />
                            </p>
                            <p>
                                <asp:Label ID="lblIndOpor" runat="server" Text="Indicador Oportunidad:  " /><asp:Label
                                    ID="IndOportundad" runat="server" />
                            </p>
                        </div>
                        <div class="actions">
                            <div class="form_left">
                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                                <p>
                              <asp:Label ID="LblTextCuenta" runat="server" Text ="Numero de Tareas: "/><asp:Label
                                    ID="lblCuenta" runat="server" /></p>

                                    
                            </div>
                           
                            <asp:GridView ID="gvTareas" runat="server" Width="100%" AutoGenerateColumns="False"  
                                 CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="IdTrabajo" EmptyDataText="No existen registros para mostrar">
                                                               <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" />
                                    <asp:BoundField DataField="idTrabajo" HeaderText="idTrabajo" />
                                    <asp:BoundField DataField="JobBook" HeaderText="JBI" />
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                    <asp:BoundField DataField="Finip" HeaderText="Finip" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Ffinp" HeaderText="Ffinp" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Finir" HeaderText="Finir" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Ffinr" HeaderText="Ffinr" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                    <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                    <asp:BoundField DataField="ObservacionesPlaneacion" HeaderText="ObservacionesPlaneacion" />
                                    <asp:BoundField DataField="ObservacionesEjecucion" HeaderText="ObservacionesEjecucion" />
                                    <asp:BoundField DataField="EstadoId" HeaderText="EstadoId" />
                                    <asp:BoundField DataField="UsuarioId" HeaderText="UsuarioId" />
                                    <asp:BoundField DataField="Rolestima" HeaderText="RolEstima" />
                                    <asp:BoundField DataField="RolEjecuta" HeaderText="RolEjecuta" />
                                    <asp:BoundField DataField="TareaId" HeaderText="TareaId" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                    <asp:BoundField DataField="Retraso" HeaderText="Retraso" />
                                    <asp:BoundField DataField="RetrasoEnFechas" HeaderText="RetrasoEnFechas" />
                                    <asp:BoundField DataField="DiasPlaneados" HeaderText="DiasPlaneados" />
                                    <asp:BoundField DataField="DiasReales" HeaderText="DiasReales" />
                                    <asp:BoundField DataField="TiempoDias" HeaderText="TiempoDias" />
                                    <asp:BoundField DataField="MesAño" HeaderText="MesAño" />
                                    <asp:BoundField DataField="GrupoUnidad" HeaderText="GrupoUnidad"  />
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
