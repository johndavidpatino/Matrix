<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Preguntas.ascx.vb" Inherits="WebMatrix.UC_Preguntas1" %>
                                                                <asp:Panel runat="server" HorizontalAlign="Center" BackColor="#1C9993" 
    ID="Panel6">
                                                                    Preguntas cuestionario<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <table style="width:100%;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="Button1" runat="server" Text="Button" />
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
                                                                                        <asp:TextBox ID="CerradasProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CerradasMultProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasMultProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="OtrosProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="DemoProp" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Real</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CerradasReal" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CerradasMultReal" runat="server" Width="80px" 
                                                                                            AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasReal" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasMultReal" runat="server" Width="80px" 
                                                                                            AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="OtrosReal" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="DemoReal" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Presupuesto</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CerradasPresup" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CerradasMultPresup" runat="server" Width="80px" 
                                                                                            ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasPresup" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="AbiertasMultPresup" runat="server" Width="80px" 
                                                                                            ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="OtrosPresup" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="DemoPresup" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Duracion</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtDuracion" runat="server" ReadOnly="True" Width="80px"></asp:TextBox>
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
                                                                    <br />
                                                                </asp:Panel>

                                                                
