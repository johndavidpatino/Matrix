<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReportePagos.aspx.vb" Inherits="WebMatrix.ReportePagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
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
                    <asp:HiddenField ID="hfidtrabajo" runat="server" />
                    <asp:HiddenField ID="hfidOrden" runat="server" />
                    <h3><a href="#">Reporte Total</a></h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>Ingrese Trabajo o Job a Buscar</label>
                                <asp:TextBox ID="txttrabajo" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:TextBox ID="txtjob" placeholder="JobBook" runat="server" CssClass="textEntry"></asp:TextBox>

                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>Muestra</label>
                                <asp:Label ID="lblmuestra" runat="server"></asp:Label>
                            </fieldset>

                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Total Produccion</label>
                                <asp:Label ID="lblTotalPagado" runat="server"></asp:Label>
                            </fieldset>

                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Total Presupuestado</label>
                                <asp:Label ID="lblPresupuestado" runat="server"></asp:Label>
                            </fieldset>

                        </div>
                        
                        <asp:GridView ID="GvDetallePagos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="VrUnitario" DataFormatString="{0:C0}" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:C0}" HeaderText="Total" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                                <asp:BoundField DataField="OrdenId" HeaderText="OrdenId" />
                            </Columns>
                        </asp:GridView>
                        <label>
                        <br />
                        Consolidado Cuentas</label>

                         &nbsp;<asp:GridView ID="GvTotales" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="VrUnitario" DataFormatString="{0:C0}" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:C0}" HeaderText="Total" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                            </Columns>
                        </asp:GridView>
                         <label>
                        <br />
                        Actividades Presupuesto</label>
                        &nbsp;&nbsp;&nbsp;
                             <fieldset>
                                 <asp:GridView ID="GvActividades" runat="server" Width="100%" AutoGenerateColumns="false"
                                     PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                     DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar" ShowFooter="True">
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
                                         <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" HeaderText="PRODUCCION" Visible="False" />
                                         <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" HeaderText="PRESUP VS PROD" Visible="False" />
                                         <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" HeaderText="%" Visible="False" />
                                         <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" HeaderText="PRESUP VS EJEC" Visible="False" />
                                         <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%" Visible="False" />
                                     </Columns>

                                 </asp:GridView>
                             </fieldset>

                    </div>

                </div>

                <div id="accordion1">

                    <h3><a href="#">Reporte Campo</a></h3>
                    <div class="block">
                        <div class="form_left">
                        </div>


                        <asp:GridView ID="GvCampo" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="VrUnitario" DataFormatString="{0:C0}" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="Total" DataFormatString="{0:C0}" HeaderText="Total" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                                <asp:BoundField DataField="OrdenId" HeaderText="OrdenId" />
                            </Columns>
                        </asp:GridView>

                         &nbsp;&nbsp;&nbsp;
                             <fieldset>
                                 <asp:GridView ID="GvActividadesCampo" runat="server" Width="100%" AutoGenerateColumns="false"
                                     PageSize="25" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                     DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar" ShowFooter="True">
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
                                         <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" HeaderText="PRODUCCION" Visible="False" />
                                         <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" HeaderText="PRESUP VS PROD" Visible="False" />
                                         <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" HeaderText="%" Visible="False" />
                                         <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" HeaderText="PRESUP VS EJEC" Visible="False" />
                                         <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%" Visible="False" />
                                     </Columns>

                                 </asp:GridView>
                             </fieldset>


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
