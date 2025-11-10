<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_LoadFiles.ascx.vb" Inherits="WebMatrix.UC_LoadFiles" %>

<h5 class="card-title">Subir documentos</h5>
<div class="form-row">
    <asp:GridView ID="gvDocumentosCargados" runat="server" Width="100%" AutoGenerateColumns="False"
        CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0"
        DataKeyNames="IdDocumentoRepositorio" EmptyDataText="No existen registros para mostrar" OnRowCommand="gvDocumentosCargados_RowCommand" >    
        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Version" HeaderText="Version" Visible="false" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha cargue" />
            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
            <asp:TemplateField HeaderText="Descargar" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                        CommandName="Descargar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                        ToolTip="Tareas" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>

    </asp:GridView>
</div>
<div class="form-row">
    <div class="col-md-5">
        <div class="position-relative form-group">
            <label for="txtCerradas" class="">Nombre</label><asp:TextBox runat="server" ID="txtNombre" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-3">
        <div class="position-relative form-group">
            <label for="txtComentarios" class="">Observaciones</label><asp:TextBox runat="server" ID="txtComentarios" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="ufArchivo" class=""></label>
            <asp:FileUpload ID="ufArchivo" runat="server" Text="CargarArchivo" CssClass="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="btnGrabar" class=""></label>
            <asp:Button ID="btnGrabar" runat="server" Text="Guardar" CssClass="btn btn-info" />
        </div>
    </div>
</div>
