<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="SimuladorCostosOperaciones.aspx.vb" Inherits="WebMatrix.SimuladorCostosOperaciones" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <style type="text/css">
     



        .auto-style2 {
            width: 206px;
            height: 22px;
            text-align: left;
        }
        .auto-style3 {
            height: 22px;
        }



      



        .auto-style4 {
            text-align: left;
        }
        .auto-style5 {
            height: 22px;
            text-align: left;
        }
        .auto-style6 {
            width: 206px;
            text-align: left;
        }
        .auto-style7 {
            color: #FFFFFF;
        }
        .auto-style9 {
            color: #000000;
            width: 229px;
            text-align: left;
        }
        .auto-style13 {
            color: #000000;
            width: 214px;
            text-align: left;
        }
        .auto-style15 {
            width: 141px;
            text-align: left;
        }



      



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%;">
        <tr>
            <td class="auto-style7">
                <strong>*Escribir valores decimales separados con coma (,)</strong></td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="auto-style4">Muestra:</td>
                        <td class="auto-style4">
                            <asp:TextBox ID="txtMuestra" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtMuestra_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtMuestra">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="auto-style4">Productividad:</td>
                        <td class="auto-style4">
                            <asp:TextBox ID="txtProductividad" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtProductividad_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtProductividad" ValidChars=",">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="auto-style4">% verificacion:</td>
                        <td class="auto-style4">
                            <asp:TextBox ID="txtPorcVerificacion" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtPorcVerificacion_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtPorcVerificacion" ValidChars=",">
                            </asp:FilteredTextBoxExtender>
                        &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="auto-style7">
                <strong>PREGUNTAS</strong></td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>Cerradas</td>
                        <td>Cerradas Multiples</td>
                        <td>Abiertas</td>
                        <td>Abiertas Multiples</td>
                        <td>Otros Cuales</td>
                        <td>Demgraficos</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCerradas" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtCerradas_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCerradas">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCerradasMult" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtCerradasMult_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCerradasMult">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbiertas" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtAbiertas_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAbiertas">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbiertasMult" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtAbiertasMult_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAbiertasMult">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOtrosCuales" runat="server" Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtOtrosCuales_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtOtrosCuales">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDemograficos" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td >
                <asp:UpdatePanel ID="upCalcular" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnCalcular" runat="server" Text="   Calcular   " />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                
                <asp:UpdatePanel ID="upResultados" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width:100%;" align="left">
                            <tr>
                                <td class="auto-style6">CAMPO</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtCampo" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">CRITICA</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtCritica" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">VERIFICACION</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtVerificion" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">CODIFICACION</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtCodificacion" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">SCRIPTING</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtScripting" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">CAPTURA</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtCaptura" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2">DATA CLEANING</td>
                                <td class="auto-style5">
                                    <asp:TextBox ID="txtDataCleaning" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style3"></td>
                            </tr>
                            <tr>
                                <td class="auto-style6">PROCESAMIENTO</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtProcesamiento" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6">GENERACION DE ARCHIVOS</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtGeneracionArchivos" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style4">&nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
