<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterEstadistica.master"
    CodeBehind="MetodologiaDeCampo.aspx.vb" Inherits="WebMatrix.MetodologiaDeCampo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {


            });

            $("#<%= txtFecha.ClientId %>").mask("99/99/9999");
            $("#<%= txtFecha.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });


        }
    </script>
    <style>
        .text-center {
            margin: 0px auto;
            text-align: center;
            float: none !important;
        }

        .readonly {
            border: none !important;
            background-color: transparent !important;
            font-size: large !important;
        }

        #stylized label {
            width: auto;
        }

        table#CPH_Content_CPH_ContentForm_chklista label {
            width: 150px !important;
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
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
                <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
                    x
                </div>
                <p>
                    <span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
                    <label id="lblTextInfo">
                    </label>
                </p>
            </div>
            <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
                <div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
                    x
                </div>
                <p>
                    <span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
                    <label id="lbltextError">
                    </label>
                </p>
            </div>
            <div>
                <asp:Panel runat="server" ID="accordion0" Visible="true">
                    <h3 style="float: left; text-align: left;">
                        <a>Trabajos
                        </a>
                    </h3>
                    <div style="clear: both">
                        <label>
                            Nombre Trabajo</label>
                        <asp:TextBox ID="txtBusquedaTrabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:Button ID="btnBuscarTrabajo" runat="server" Text="Buscar" />
                        <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="Fecha Tentativa Inicio Campo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="Fecha Tentativa Finalizacion Trabajo"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="NombreMetodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Metodologia" ShowHeader="False" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" CssClass="text-center"
                                            ToolTip="Actualizar" />
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
                </asp:Panel>
                <asp:Panel runat="server" ID="accordion1" Visible="false">
                    <h3><a>Lista de Metodología de Campo</a></h3>
                    <div style="clear: both">
                        <div style="float: left; display: block; margin-top: 20px;">
                            <label>Texto a Buscar</label>
                            <asp:HiddenField ID="hfidtrabajo" runat="server" />
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        </div>
                        <div class="spacer">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:TemplateField HeaderText="Objetivo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblobjetivos" runat="server" Text='<%# Eval("Objetivo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mercado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmercado" runat="server" Text='<%# Eval("Mercado") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Marco M">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmarco" runat="server" Text='<%# Eval("Marco") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Técnica">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltecnica" runat="server" Text='<%# Eval("Tecnica") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Diseño M">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldiseno" runat="server" Text='<%# Eval("Diseno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instrucciones" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblinstrucciones" runat="server" Text='<%# Eval("Instrucciones") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Distribución" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldistribucion" runat="server" Text='<%# Eval("Distribucion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nivel Confianza" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnivelconfianza" runat="server" Text='<%# Eval("NivelConfianza") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margen Error" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmargen" runat="server" Text='<%# Eval("MargenError") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Desagregación" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesagregacion" runat="server" Text='<%# Eval("Desagregacion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfuente" runat="server" Text='<%# Eval("Fuente") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Variables" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvariables" runat="server" Text='<%# Eval("Variables") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tasa" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltasa" runat="server" Text='<%# Eval("Tasa") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Procedimiento" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblprocedimiento" runat="server" Text='<%# Eval("Procedimiento") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton0" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Duplicar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Duplicar" ImageUrl="~/Images/Duplicar_32.png" Text="Duplicar" Width="16px" CssClass="text-center"
                                            ToolTip="Duplicar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                            OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                    <%= gvDatos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="accordion2" Visible="false">
                    <h3><a>Información Metodología de Campo</a></h3>
                    <div class="spacer">
                        <div style="float: left; display: block; margin-top: 20px;">
                            <asp:TextBox ID="txtNombreEstudio" runat="server" CssClass="required number textEntry" Visible="false"></asp:TextBox>
                            <label>Fecha:</label>
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="hfidmetodologia" runat="server" />
                        </div>
                        <div class="spacer" style="padding: 5px;"></div>
                        <div style="float: left; display: block; margin-top: 20px;">
                            <label>MUESTREO PROBABILISTICO</label>
                        </div>
                        <div style="float: left; display: block;  width: 100%;">
                            <asp:CheckBoxList ID="chklista" runat="server" AutoPostBack="True" RepeatColumns="4"
                                Width="100%">
                                <asp:ListItem Value="1">Grupo Objetivo</asp:ListItem>
                                <asp:ListItem Value="2">Mercado: Cubrimiento geográfico</asp:ListItem>
                                <asp:ListItem Value="3">Marco Muestral</asp:ListItem>
                                <asp:ListItem Value="4">Técnica</asp:ListItem>
                                <asp:ListItem Value="5">Diseño Muestral</asp:ListItem>
                                <asp:ListItem Value="6">Instrucciones para la recolección</asp:ListItem>
                                <asp:ListItem Value="7">Distribución de la muestra</asp:ListItem>
                                <asp:ListItem Value="8">Nivel de confianza</asp:ListItem>
                                <asp:ListItem Value="9">Margen de Error Esperado</asp:ListItem>
                                <asp:ListItem Value="10">Desagregación básica de los resultados</asp:ListItem>
                                <asp:ListItem Value="11">Fuente para la elaboración de la distribución muestral</asp:ListItem>
                                <asp:ListItem Value="12">Variables básicas de ponderación</asp:ListItem>
                                <asp:ListItem Value="13">Tasa de respuesta</asp:ListItem>
                                <asp:ListItem Value="14">Procedimiento de imputación</asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:Panel ID="pnlVersion" runat="server" Visible="false">
                                    <br />
                                    <label>Versión: </label>
                                    <asp:TextBox ID="txtVersion" runat="server" ReadOnly="true" CssClass="readonly" Width="30" />
                                    <div class="spacer"></div>
                                </asp:Panel>
                                <asp:Panel ID="pnlobjetivos" runat="server" Visible="false">
                                    <label>
                                        Grupo Objetivo</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtObjetivos" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer"></div>
                                </asp:Panel>
                                <asp:Panel ID="pnlmercado" runat="server" Visible="false">
                                    <label>
                                        Mercado, Cubrimiento geográfico</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtMercado" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlmarco" runat="server" Visible="false">
                                    <label>
                                        Marco muestral</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtMarcoMuestral" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnltecnica" runat="server" Visible="false">
                                    <label>
                                        Técnica</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtTecnica" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnldiseno" runat="server" Visible="false">
                                    <label>
                                        Diseño Muestral</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtDiseno" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlinstrucciones" runat="server" Visible="false">
                                    <label>
                                        Instrucciones para la recolección</label>
                                    <cc1:Editor ID="txtInstrucciones" NoUnicode="true" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnldistribucion" runat="server" Visible="false">
                                    <label>
                                        Distribución de la muestra</label>
                                    <cc1:Editor ID="txtDistribucionDeMuestra" NoUnicode="true" Width="100%" Height="50px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlnivelconfianza" runat="server" Visible="false">
                                    <label>
                                        Nivel de confianza</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtNivelConfianza" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlmargenerror" runat="server" Visible="false">
                                    <label>
                                        Margen de Error Esperado</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtMargenError" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnldesagregacion" runat="server" Visible="false">
                                    <label>
                                        Desagregación básica de los resultados</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtDesagregacion" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlfuente" runat="server" Visible="false">
                                    <label>
                                        Fuente para la elaboración de la distribución muestral</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtFuente" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlVariables" runat="server" Visible="false">
                                    <label>
                                        Variables básicas de ponderación</label>
                                    <cc1:Editor ID="txtVariables" NoUnicode="true" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnltasa" runat="server" Visible="false">
                                    <label>
                                        Tasa de Respuesta</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtTasa" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlprocedimiento" runat="server" Visible="false">
                                    <label>
                                        Procedimiento de Imputación</label>
                                    <asp:TextBox TextMode="Multiline" ID="txtprocedimiento" Width="100%" Height="100px" runat="server" />
                                    <div class="spacer">
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                            <div style="float: left; display: block; margin-top: 20px; width: 100%;">
                                <div class="spacer">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                                    &nbsp;
                                            <input id="Button1" type="submit" class="button" value="Cancelar"
                                                style="font-size: 11px;" onclick="location.href = 'MetodologiaDeCampo.aspx';" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
