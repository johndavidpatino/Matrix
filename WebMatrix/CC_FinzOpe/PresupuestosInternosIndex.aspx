<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="PresupuestosInternosIndex.aspx.vb" Inherits="WebMatrix.PresupuestosInternosIndex" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Listado de Trabajos y Propuestas
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    Este es el listado de trabajos que se encuentran creados en Matrix
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Búsqueda</h5>
            <p class="card-subtitle">Diligencie los campos por los cuales desea buscar</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Trabajo</button>
                        </div>
                        <asp:TextBox ID="txtTrabajo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">JobBook</button>
                        </div>
                        <asp:TextBox ID="txtJobBook" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Nombre trabajo</button>
                        </div>
                        <asp:TextBox ID="txtNombreTrabajo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">No Propuesta</button>
                        </div>
                        <asp:TextBox ID="txtPropuesta" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnSearch" class="btn btn-primary" Text="Buscar" OnClick="btnSearch_Click" ></asp:Button>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Resultados de búsqueda</h5>
            <div>
                <asp:GridView ID="gvTrabajos" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                            CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda" >
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo">
                                    <ItemStyle Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="COEAsignado" HeaderText="OMP Asignado">
                                                                   
                                <ItemStyle Wrap="true" />
                                </asp:BoundField>
                                                                   
                                <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                                <asp:BoundField DataField="ObservacionCOE" HeaderText="ObservacionOMP">
                                    <ItemStyle HorizontalAlign="Justify" Wrap="True" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Presupuestos" ImageUrl="~/Images/find_16.png" Text="Actualizar"
                                            ToolTip="Ir a presupuestos" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                              </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
