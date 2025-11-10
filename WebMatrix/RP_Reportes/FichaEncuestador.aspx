<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master"
    CodeBehind="FichaEncuestador.aspx.vb" Inherits="WebMatrix.FichaEncuestador" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>

    <script type="text/javascript">

        function loadPlugins() {

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

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
                                Datos del Encuestador<asp:HiddenField ID="hfPersona" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
        <fieldset class="validationGroup">
            <div class="form_left">
                <fieldset>
                    <label>
                        Identificación:
                    </label>
                    <asp:Label ID="lblIdentificacion" runat="server"></asp:Label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Nombres:
                    </label>
                    <asp:Label ID="lblNombres" runat="server"></asp:Label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Apellidos:
                    </label>
                    <asp:Label ID="lblApellidos" runat="server"></asp:Label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Cargo:
                    </label>
                    <asp:Label ID="lblCargos" runat="server"></asp:Label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Tipo encuestador:
                    </label>
                    <asp:Label ID="lblTipoEncuestador" runat="server"></asp:label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Ciudad:
                    </label>
                    <asp:Label ID="lblCiudad" runat="server"></asp:Label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Tipo contratación:
                    </label>
                    <asp:Label ID="lblTipoContratacion" runat="server"></asp:label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Fecha ingreso:
                    </label>
                    <asp:label ID="lblFechaIngreso" runat="server"></asp:label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Fecha nacimiento:
                    </label>
                    <asp:label ID="lblFechaNacimiento" runat="server"></asp:label>
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Activo:
                    </label>
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="true" Enabled="false" />
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <label>
                        Vetado:
                    </label>
                    <asp:CheckBox ID="chkVetado" runat="server" Enabled="false" />
                </fieldset>
            </div>
        </fieldset>
        <div class="form_left">
                <fieldset>
                    <label>
                        Razon del veto
                    </label>
                    <asp:Label ID="lblRazon" runat="server"></asp:Label>
                </fieldset>
            </div>
    </div>
                    <%--items--%>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Datos último mes
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                        <h4>Datos de Ejecución</h4>
                            <table class="displayTable">
                                <tr style="font-weight:bold">
                                    <td>Encuestas realizadas</td>
                                    <td>Encuestas anuladas</td>
                                    <td>Encuestas con error</td>
                                    <td>Encuestas revisadas (verificación)</td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblRealizadas1" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblAnuladas1" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblError1" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblRevisadas1" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <br /><br />
                        <h4>Encuestas por metodología</h4>
                        <asp:GridView ID="gvEncuestasPorMetodologia1" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                </Columns>
                            </asp:GridView>
                                <br /><br />
                        <h4>Trabajos en los que ha participado</h4>
                        <asp:GridView ID="gvTrabajos1" runat="server" Width="80%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre de Trabajo" />
                                    <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                    <asp:BoundField DataField="BU" HeaderText="Unidad" />
                                    <asp:BoundField DataField="COE" HeaderText="OMP" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" DataFormatString="{0:d}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Datos últimos tres mes
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                        <h4>Datos de Ejecución</h4>
                            <table class="displayTable">
                                <tr style="font-weight:bold">
                                    <td>Encuestas realizadas</td>
                                    <td>Encuestas anuladas</td>
                                    <td>Encuestas con error</td>
                                    <td>Encuestas revisadas (verificación)</td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblRealizadas3" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblAnuladas3" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblError3" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblRevisadas3" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <br /><br />
                        <h4>Encuestas por metodología</h4>
                        <asp:GridView ID="gvEncuestasPorMetodologia3" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                </Columns>
                            </asp:GridView>
                                <br /><br />
                        <h4>Trabajos en los que ha participado</h4>
                        <asp:GridView ID="gvTrabajos3" runat="server" Width="80%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre de Trabajo" />
                                    <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                    <asp:BoundField DataField="BU" HeaderText="Unidad" />
                                    <asp:BoundField DataField="COE" HeaderText="OMP" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" DataFormatString="{0:d}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Datos últimos seis meses
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                        <h4>Datos de Ejecución</h4>
                            <table class="displayTable">
                                <tr style="font-weight:bold">
                                    <td>Encuestas realizadas</td>
                                    <td>Encuestas anuladas</td>
                                    <td>Encuestas con error</td>
                                    <td>Encuestas revisadas (verificación)</td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblRealizadas6" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblAnuladas6" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblError6" runat="server"></asp:Label></td>
                                    <td><asp:Label ID="lblRevisadas6" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <br /><br />
                        <h4>Encuestas por metodología</h4>
                        <asp:GridView ID="gvEncuestasPorMetodologia6" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                </Columns>
                            </asp:GridView>
                                <br /><br />
                        <h4>Trabajos en los que ha participado</h4>
                        <asp:GridView ID="gvTrabajos6" runat="server" Width="80%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre de Trabajo" />
                                    <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente de Proyectos" />
                                    <asp:BoundField DataField="BU" HeaderText="Unidad" />
                                    <asp:BoundField DataField="COE" HeaderText="OMP" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" DataFormatString="{0:d}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion4">
                    <h3>
                        <a href="#">
                            <label>
                                Vetar al encuestador
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                        <h4>Para vetar al encuestador haga clic en el siguiente botón</h4>
                            <asp:button ID="btnVetar" runat="server" Text="Vetar al encuestador" OnClientClick="return confirm('Está seguro que desea vetar este encuestador?')" />
                            &nbsp;Razón del veto <asp:TextBox ID="txtRazonVeto" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
