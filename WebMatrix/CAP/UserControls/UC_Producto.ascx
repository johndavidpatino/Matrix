<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Producto.ascx.vb" Inherits="WebMatrix.UC_Producto" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<style type="text/css">
    .style2
    {
        width: 139px;
    }
    .auto-style1 {
        text-align: left;
    }
    .auto-style2 {
        width: 311px;
    }
</style>

<script>
    function CalcularTiempo() {
        
        document.getElementById('<%= txtDuracion.ClientID %>').value = parseInt(((Number(document.getElementById('<%= CerradasReal.ClientID %>').value) + (Number(document.getElementById('<%= CerradasMultReal.ClientID %>').value) / 10)) + Number(document.getElementById('<%= AbiertasReal.ClientID %>').value) + Number(document.getElementById('<%= AbiertasMultReal.ClientID %>').value) + Number(document.getElementById('<%= OtrosReal.ClientID %>').value) + Number(document.getElementById('<%= DemoReal.ClientID %>').value)) / 6,0);
    }


</script>
<link href="../../Styles/skinStyle.css" rel="stylesheet" type="text/css" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table style="width:100%;">
            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                Oferta:</td>
                            <td>
                                <asp:DropDownList ID="LstOferta" runat="server" AutoPostBack="True" 
                                    style="margin-left: 0px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Producto:</td>
                            <td>
                                <asp:DropDownList ID="lstProducto" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel6" runat="server" BackColor="#1C9993" 
                        HorizontalAlign="Center">
                        Preguntas cuestionario<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnHitorial" runat="server" Text="Historial" />
                                            <asp:ModalPopupExtender ID="btnHitorial_ModalPopupExtender" runat="server" Enabled="True" PopupControlID="panel7" TargetControlID="btnHitorial" BackgroundCssClass="modalBackground">
                                            </asp:ModalPopupExtender>
                                        </td>
                                        <td>
                                            Cerradas</td>
                                        <td>
                                            Cerradas multiples</td>
                                        <td>
                                            Abiertas</td>
                                        <td>
                                            Abiertas multiples</td>
                                        <td>
                                            Otros</td>
                                        <td>
                                            Demograficos</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Propuestas</td>
                                        <td>
                                            <asp:TextBox ID="CerradasProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="CerradasMultProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AbiertasProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AbiertasMultProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="OtrosProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="DemoProp" runat="server" ReadOnly="True" Width="80px" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Real</td>
                                        <td>
                                            <asp:TextBox ID="CerradasReal" runat="server" Width="80px" 
                                                ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                    TargetControlID="CerradasReal">
                                                </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="CerradasMultReal" runat="server" 
                                                Width="80px" ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                    TargetControlID="CerradasMultReal">
                                                </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AbiertasReal" runat="server" Width="80px" 
                                                ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                    TargetControlID="AbiertasReal">
                                                </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AbiertasMultReal" runat="server" 
                                                Width="80px" ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                                                    TargetControlID="AbiertasMultReal">
                                                </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="OtrosReal" runat="server" Width="80px" ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                               <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                                    TargetControlID="OtrosReal">
                                                </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="DemoReal" runat="server" Width="80px" 
                                                Enabled="False" ClientIDMode="Static" onchange="Javascript:CalcularTiempo();" >0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Duracion</td>
                                        <td>
                                            <asp:TextBox ID="txtDuracion" runat="server" ReadOnly="True" Width="80px" 
                                                Font-Bold="True" ForeColor="#FF3300" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            Minutos</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel ID="Panel7" runat="server" BackColor="#1C9993" Height="500px" Width="800px" ScrollBars="Horizontal">
                            <asp:UpdatePanel ID="upHistorico" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td class="auto-style1">Unidad:</td>
                                                        <td class="auto-style1">
                                                            <asp:DropDownList ID="lstUnidad_hist" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="auto-style1">Jobbook:</td>
                                                        <td class="auto-style1">
                                                            <asp:TextBox ID="txtJobbook_Hist" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style1">Oferta:</td>
                                                        <td class="auto-style1">
                                                            <asp:DropDownList ID="lstOferta_Hist" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="auto-style1">Producto:</td>
                                                        <td class="auto-style1">
                                                            <asp:DropDownList ID="lstProducto_Hist" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Nombre:</td>
                                                        <td>
                                                            <asp:TextBox ID="txtNombres_Hist" runat="server" Width="250px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBuscarHist" runat="server" Text="Buscar" />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvHistPreg" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="displayTable" AllowPaging="True" EmptyDataText="No existen datos">
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                        <asp:BoundField DataField="CerradasRU" HeaderText="Cerradas" />
                                                        <asp:BoundField DataField="CerradasRM" HeaderText="Cerradas Mult" />
                                                        <asp:BoundField DataField="Abiertas" HeaderText="Abiertas" />
                                                        <asp:BoundField DataField="AbiertasMul" HeaderText="Abiertas Mult" />
                                                        <asp:BoundField DataField="Otros" HeaderText="Otros" />
                                                        <asp:BoundField DataField="Demograficos" HeaderText="Demo" />
                                                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" >
                                                        <ItemStyle Width="110px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                        <asp:ButtonField ButtonType="Button" CommandName="SEL" Text="Seleccionar" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td class="auto-style2">&nbsp;</td>
                                                        <td style="text-align: left">
                                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </asp:Panel>
                        <br />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

