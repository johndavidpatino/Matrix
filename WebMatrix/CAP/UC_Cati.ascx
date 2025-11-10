<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Cati.ascx.vb" Inherits="WebMatrix.UC_Cati" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<style type="text/css">

        .style2
        {
            width: 110px;
        }
        
        fieldset, form, label, legend
{
    margin: 0;
    padding: 0;
    border: 0;
    font-size: 100%;
    font: inherit;
    vertical-align: baseline;
}

        .style3
        {
            width: 98px;
        }
    </style>
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td>
                                                                <table style="width:100%;">
                                                                    <tr>
                                                                        <td class="style2">
                                                                            Etapas:</td>
                                                                        <td>
                                                                            <asp:CheckBoxList runat="server" RepeatDirection="Horizontal" TextAlign="Left" 
                                                                                Width="100%" ID="CheckBoxList1"><asp:ListItem Value="1">Diseno</asp:ListItem>
<asp:ListItem Value="2">Campo</asp:ListItem>
<asp:ListItem Value="3">Critica</asp:ListItem>
<asp:ListItem Value="4">Codificacion</asp:ListItem>
<asp:ListItem Value="5">Captura</asp:ListItem>
<asp:ListItem Value="6">Procesamiento</asp:ListItem>
<asp:ListItem Value="7">Estadistica</asp:ListItem>
<asp:ListItem Value="8">Analisis</asp:ListItem>
</asp:CheckBoxList>

                                                                            <asp:RoundedCornersExtender runat="server" Enabled="True" 
                                                                                TargetControlID="CheckBoxList1" ID="CheckBoxList1_RoundedCornersExtender"></asp:RoundedCornersExtender>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style2">
                                                                            Distribucion:</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="lstDistribucion_101"><asp:ListItem Value="0">Seleccione...</asp:ListItem>
<asp:ListItem Value="1">Nacional</asp:ListItem>
<asp:ListItem Value="2">Local</asp:ListItem>
</asp:DropDownList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style2">
                                                                            Incidencia:</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="lstIncidencia_101"></asp:DropDownList>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel runat="server" HorizontalAlign="Center" BackColor="#1C9993" 
                                                                    ID="Panel6">
                                                                    Preguntas custionario<br />
                                                                    <table style="width:100%;">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
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
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox1"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox2"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox3"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox4"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox5"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox6"></asp:TextBox>

                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Real</td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox7"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox9"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox12"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox13"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox16"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox17"></asp:TextBox>

                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Presupuesto</td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox8"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox10"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox11"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox14"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox15"></asp:TextBox>

                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Width="80px" ID="TextBox18"></asp:TextBox>

                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>

                                                                <asp:RoundedCornersExtender runat="server" Enabled="True" 
                                                                    TargetControlID="Panel6" ID="Panel6_RoundedCornersExtender"></asp:RoundedCornersExtender>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:100%;">
                                                                    <tr>
                                                                        <td class="style3">
                                                                            Duracion:</td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" Width="80px" ID="TextBox19"></asp:TextBox>

                                                                            &nbsp;minutos</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                
