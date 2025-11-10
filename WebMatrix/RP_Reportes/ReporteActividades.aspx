<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReporteActividades.aspx.vb" Inherits="WebMatrix.ReporteActividades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
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
                <div id="accordion0">


                    <h3><a href="#">Reporte Actividades por Trabajo</a></h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>Seleccione Actividad</label>
                                <asp:DropDownList ID="ddlactividad" runat="server"></asp:DropDownList>
                            </fieldset>

                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Ingrese Trabajo </label>
                                <asp:TextBox ID="txtidtrabajo" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>

                                <asp:Button ID="btnbuscar" runat="server" Text="Buscar" />
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                            </fieldset>
                        </div>

                        <asp:GridView ID="GvActividades" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="ID" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Cod" HeaderText="Codigo" />
                                <asp:BoundField DataField="Actividad" HeaderText="Actividad" />
                                <asp:BoundField DataField="Presupuestado" DataFormatString="{0:C0}" HeaderText="Presupuestado" />
                                <asp:BoundField DataField="Autorizado" DataFormatString="{0:C0}" HeaderText="Autorizado" />
                                <asp:BoundField DataField="Ejecutado" DataFormatString="{0:C0}" HeaderText="Ejecutado" />
                                <asp:BoundField DataField="Produccion" DataFormatString="{0:C0}" HeaderText="Produccion" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
