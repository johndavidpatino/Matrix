<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ControlPresupuestos.aspx.vb" Inherits="WebMatrix.IngresarCostosAutorizados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">

 <style type="text/css">
        
        table { text-align : center
        }
        tr {text-align : left}
        th {text-align : left}
        td{text-align : left       }
        
        .centrar { text-align :center }
      </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            Unidad:</td>
                        <td>
                            <asp:DropDownList ID="lstUnidades" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            JobBook:</td>
                        <td>
                            <asp:TextBox ID="txtJobBook" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    Tecnica:</td>
                                <td>
                                    <asp:DropDownList ID="lstTecnicas" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Metodologia:</td>
                                <td>
                                    <asp:DropDownList ID="lstMetodologias" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upBuscar" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvPresupuestos" runat="server" CssClass="displayTable" 
                    Width="100%" AllowPaging="True" AutoGenerateColumns="False" PageSize="12">
                            <Columns>
                                <asp:BoundField DataField="IdPropuesta" HeaderText="Id" />
                                <asp:BoundField DataField="ParNomPresupuesto" HeaderText="Nombre Propuesta" />
                                <asp:BoundField DataField="ParAlternativa" HeaderText="Alter" />
                                <asp:TemplateField HeaderText="TecCodigo" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("TecCodigo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TecCodigo") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TecNombre" HeaderText="Tecnica" />
                                <asp:TemplateField HeaderText="MetCodigo" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("MetCodigo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("MetCodigo") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MetNombre" HeaderText="Metodologia" />
                                <asp:TemplateField HeaderText="idFase" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ParNacional") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ParNacional") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DescFase" HeaderText="Fase" />
                                <asp:BoundField DataField="ParNumJobBook" HeaderText="JobBook" />
                                <asp:TemplateField HeaderText="Observaciones">
                                                            <ItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" TextMode="MultiLine" Width="200px" 
                                                                    Text='<%# Bind("ParObservaciones") %>' Height="50px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="false" 
                                            CommandName="ADD" Text="Agregar"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
