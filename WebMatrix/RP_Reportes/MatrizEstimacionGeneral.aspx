<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master"
    CodeBehind="MatrizEstimacionGeneral.aspx.vb" Inherits="WebMatrix.MatrizEstimacionGeneral" %>

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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Matriz Estimacion General</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Matriz<asp:HiddenField ID="HiddenField1" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvMatriz" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable2" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="CAMPOP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CAMPOP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAMPOE" HeaderText="#"  />
                                    <asp:BoundField DataField="CAMPOE_S" HeaderText="&sum;"  />
                                    <asp:BoundField DataField="CAMPO_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="RMCP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="RMCP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="RMCE" HeaderText="#" />
                                    <asp:BoundField DataField="RMCE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="RMC_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="CRITICAP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CRITICAP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CRITICAE" HeaderText="#" />
                                    <asp:BoundField DataField="CRITICAE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CRITICA_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="VERIFICACIONP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="VERIFICACIONP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="VERIFICACIONE" HeaderText="#" />
                                    <asp:BoundField DataField="VERIFICACIONE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="VERIFICACION_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="CAPTURAP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CAPTURAP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAPTURAE" HeaderText="#" />
                                    <asp:BoundField DataField="CAPTURAE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAPTURA_DIF" HeaderText="< >" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
