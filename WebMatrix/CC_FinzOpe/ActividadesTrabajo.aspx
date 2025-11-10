<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ActividadesTrabajo.aspx.vb" Inherits="WebMatrix.ActividadesTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            $('#NuevoPresupuesto').dialog({
                modal: true,
                autoOpen: false,
                title: "Otras Activiadades",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

        };
        function Presupuestos() {
            $('#NuevoPresupuesto').dialog("open");
        };
        function NuevoPresupuestoCerrar() {
            $('#NuevoPresupuesto').dialog("close");
        };

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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Actividades
                            </label>
                        </a>
                    </h3>
                    <div class="block">

                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <div class="form_left">
                            <fieldset>
                                <asp:GridView ID="GvActividades" runat="server" Width="100%" AutoGenerateColumns="false"
                                    PageSize="100" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>

                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                        <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                        <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}" HeaderText="PRESUPUESTADO" />
                                        <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}" HeaderText="AUTORIZADO" />
                                        <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}" HeaderText="PRESUP VS AUTO" />
                                        <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" HeaderText="PRODUCCION" />
                                        <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" HeaderText="PRESUP VS PROD" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" HeaderText="PRESUP VS EJEC" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%" Visible="False" />
                                    </Columns>

                                </asp:GridView>
                                <asp:Button ID="btnPresupuestos" runat="server" Text="GenerarPresupuestos" />
                                <asp:Button ID="btnNuevo" runat="server" Text="NuevoPresupuesto" OnClientClick="Presupuestos()" />
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                            </fieldset>
                        </div>
                    </div>
                </div>
                 <div id="accordion1">
                      <asp:GridView ID="GvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" EmptyDataText="No existen registros para mostrar">
                            <Columns>
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" /> 
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />                             
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Actividad" HeaderText="Actividad" />
                                <asp:TemplateField HeaderText="VrUnitario" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVrUnitario" runat="server" Text='<%# Eval("VrUnitario")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>                          
                                <asp:BoundField DataField="Contratista" HeaderText="VrContratista" DataFormatString="{0:C0}" />
                            </Columns>
                        </asp:GridView>
                 </div> 
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="NuevoPresupuesto">
        <asp:UpdatePanel ID="upPresupuestos" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="actions">
                    <div class="form_left">
                    </div>
                </div>
                <asp:DropDownList ID="ddlActividades" runat ="server" ></asp:DropDownList>
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClientClick="NuevoPresupuestoCerrar();" />


            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
