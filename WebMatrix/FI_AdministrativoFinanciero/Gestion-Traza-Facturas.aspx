<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Gestion-Traza-Facturas.aspx.vb" Inherits="WebMatrix.Gestion_Traza_Facturas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            
            loadPlugins();
            
            $('#UsuariosAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios Asignados",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

        });


        function loadPlugins() {

            $("#<%= txtFInicioI.ClientID%>").mask("99/99/9999");
            $("#<%= txtFInicioI.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFFinP.ClientId %>").mask("99/99/9999");
            $("#<%= txtFFinP.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#LoadFiles').dialog(
{
    modal: true,
    autoOpen: false,
    title: "Cargar archivo",
    width: "600px",
    closeOnEscape: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            },
            buttons: {
                Cerrar: function () {
                    $(this).dialog("close");
                }
            }
});


        }

        function MostrarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("open");
        }

        function MostrarLoadFiles() {
            $('#LoadFiles').dialog("open");
        }

        function CerrarLoadFiles() {
            $('#LoadFiles').dialog("close");
        }



     </script>
</asp:Content>
    
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
                    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
                <ContentTemplate>
    <div style="width:100%">
    <div id="container">
    <h1><a>Gestión de </a>Facturas</h1>
    <div id="menu-form">
          <nav>
           <ul>
                <li><a><asp:LinkButton ID="lkb1" runat="server">Facturas Recibidas</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lkb2" runat="server">Facturas Aprobadas</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="lkb3" runat="server">Facturas Contabilizadas</asp:LinkButton></a></li>
               <li><a><asp:LinkButton ID="lkb4" runat="server">Facturas en Tesorería</asp:LinkButton></a></li>
            </ul>
           </nav>
       </div>
       <asp:Panel ID="pnlCronograma" runat="server" Visible="true">
           <asp:HiddenField ID="hfEstado" runat="server" Value="-1" />
           <asp:HiddenField ID="hfTypeFile" runat="server" />
           <div class="clear"></div>
       <a><asp:Label ID="lblTitle" runat="server"></asp:Label></a>
           <div class="clear"></div>
        <div id="campo-formulario3" style="min-width:600px;">
            <label id="lblNumRadicado" runat="server">Número Radicado</label>
            <asp:TextBox ID="txtNumRadicado" runat="server"></asp:TextBox>
            <label id="lblFechaIni" runat="server">Fecha Inicio Radicación (Opcional)</label>
            <asp:TextBox ID="txtFInicioI" runat="server"></asp:TextBox>
            <label id="lblFechaFin" runat="server">Fecha Fin Radicación (Opcional)</label>
            <asp:TextBox ID="txtFFinP" runat="server"></asp:TextBox>
            <asp:Button ID="btnFiltrarCronograma" runat="server" Text="Filtrar" />
            <asp:GridView ID="gvDatos" runat="server" DataKeyNames="Id" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                 AllowPaging="false" PageSize="50" 
                  PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="No se encuentran facturas para los filtros seleccionados">
                 <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Marcar">
            <ItemTemplate>
                <asp:CheckBox ID="CBSel" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="Id" />
                    <asp:BoundField DataField="FechaRadicacion" HeaderText="FechaRadicacion" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                    <asp:BoundField DataField="NoFactura" HeaderText="NoFactura" />
                    <asp:BoundField DataField="NoOrden" HeaderText="NoOrden" visible="false"/>
                    <asp:BoundField DataField="UsuarioRadica" HeaderText="Usuario Radica" />
                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                    <asp:BoundField DataField="NIT_CC" HeaderText="NIT/CC" />
                    <asp:BoundField DataField="ValorFactura" HeaderText="Valor" 
                        DataFormatString="{0:C0}"/>
                    <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" visible="false"/>
                    <asp:TemplateField HeaderText="Escáner" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Load" ImageUrl="~/Images/list_16.png" Text="Escáner" ToolTip="Imagen de la factura"  />
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
                                                    Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
            <asp:Button ID="btnEnviar" Text="Enviar a " runat="server" CssClass="button" Visible="false" Width="150px" />
            <br /><br />
            <asp:FileUpload ID="FileUpData" runat="server" Visible="false" />
            <br /><br />
            <asp:Button ID="btnLoadFile" runat="server" Text="Cargar Archivo" Visible="false"/>
            &nbsp;&nbsp;
            <asp:Button ID="btnDownloadFile" runat="server" Text="Descargar Plantilla" Visible="false"/>
            <br /><br />
            <asp:Label ID="lblFileIncorrect" runat="server" Text="Archivo Incorrecto, por favor asegurese que es un archivo excel" Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="gvErrores" runat="server" Visible="false" Caption="LISTADO DE ERRORES" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                 AllowPaging="false" PageSize="50" 
                  PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="No se encuentran errores para las facturas seleccionadas">
                 <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="llave" HeaderText="Llave" />
                    <asp:BoundField DataField="descripcion" HeaderText="Error" />    
                </Columns>
                <PagerTemplate>
                <div class="pagingButtons">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                    Enabled='<%# IIf(gvErrores.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                    Enabled='<%# IIf(gvErrores.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                            </td>
                            <td>
                                <span class="pagingLinks">[<%= gvErrores.PageIndex + 1%>-<%= gvErrores.PageCount%>]</span>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                    Enabled='<%# IIf((gvErrores.PageIndex + 1) = gvErrores.PageCount, "false", "true")%>'
                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                    Enabled='<%# IIf((gvErrores.PageIndex + 1) = gvErrores.PageCount, "false", "true")%>'
                                    SkinID="paging">Ultimo »</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </PagerTemplate>
            </asp:GridView>
            <label id="lblmsgError" runat="server" visible="false"></label>
        </div>
           
       </asp:Panel>
    </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    

        <div id="LoadFiles">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
        <asp:Button ID="btnViewFile" runat="server" Text="Ver archivo" />
        <div class="actions"></div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
