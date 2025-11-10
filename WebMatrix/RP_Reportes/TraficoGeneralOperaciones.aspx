<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" AsyncTimeout="180"
    CodeBehind="TraficoGeneralOperaciones.aspx.vb" Inherits="WebMatrix.REP_TraficoGeneralOperaciones" %>

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

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Planeacion General Operaciones</a>
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
                                Carga planeada<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                                <fieldset>
                                    <label>
                                        Gerencias OP</label>
                                    <asp:DropDownList ID="ddlGerencias" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                        </div>
                        <div class="form_left">
                                <fieldset>
                                    <label>
                                        Unidades</label>
                                    <asp:DropDownList ID="ddlUnidades" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Metodología</label>
                                    <asp:DropDownList ID="ddlMetodologia" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="actions"></div>
                        <div class="form_left">
                            <fieldset>
                                <asp:Button ID="btnMostrar" runat="server" Text="Ver Reporte" />
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" />
                            </fieldset>
                        </div>
                        <div class="actions">
                            <asp:GridView ID="gvDatos" runat="server" Width="100%" DataSourceID="SqlDsDatos"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" 
                                EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsDatos" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                                SelectCommand="REP_TRAFICO_GENERAL_OPERACIONES" 
                                SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlGerencias" Name="GERENCIA" 
                                        PropertyName="SelectedValue" Type="Int64" />
                                    <asp:ControlParameter ControlID="ddlUnidades" Name="UNIDAD" 
                                        PropertyName="SelectedValue" Type="Int64" />
                                    <asp:ControlParameter ControlID="ddlMetodologia" Name="METODOLOGIA" 
                                        PropertyName="SelectedValue" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <%--items--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
