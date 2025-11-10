<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGestionTratamiento.master" CodeBehind="SeleccionarPreguntasTabular.aspx.vb" Inherits="WebMatrix.SeleccionarPreguntasTabular" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <style type="text/css">
        .auto-style3 {
            width: 107px;
        }
        .auto-style5 {
            width: 779px;
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
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width:100%;">
                            <tr>
                                <td class="auto-style3">Seleccinar estudio:</td>
                                <td class="auto-style5">
                                    <asp:DropDownList ID="lstEstudios" runat="server" Width="500px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="lstEstudios_ListSearchExtender" runat="server" QueryPattern="Contains" TargetControlID="lstEstudios">
                                    </asp:ListSearchExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnTabular" runat="server" Text="Tabular" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">Preguntas:</td>
                                <td class="auto-style5">
                                    <asp:DropDownList ID="lstPreguntas" runat="server" Width="550px">
                                    </asp:DropDownList>
                                    <asp:ListSearchExtender ID="lstPreguntas_ListSearchExtender" runat="server" QueryPattern="Contains" TargetControlID="lstPreguntas">
                                    </asp:ListSearchExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3">&nbsp;</td>
                                <td class="auto-style5">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar  pregunta" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvPregTab" runat="server" AutoGenerateColumns="False" CssClass="displayTable" EmptyDataText="No existen registros" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="E_Id" HeaderText="ESTUDIO" />
                                <asp:BoundField DataField="Pr_Id" HeaderText="PR_ID" />
                                <asp:BoundField DataField="Pr_Nombre" HeaderText="PREGUNTA NOMBRE" />
                                <asp:BoundField DataField="Pr_Texto" HeaderText="PREGUNTA DESC" />
                                <asp:ButtonField ButtonType="Button" CommandName="DEL" Text="Eliminar" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
