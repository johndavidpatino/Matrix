<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterEstadistica.master"
    CodeBehind="DisenoDeMuestra.aspx.vb" Inherits="WebMatrix.DisenoDeMuestra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

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
    Diseño de Muestra
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
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0" style="margin: 5px 10px; background-color: #fff;">
                    <h3><a href="#">Consulta</a></h3>
                    <div>
                        <div style="float: left;">
                            <asp:Label ID="lbltitulo" runat="server"></asp:Label>
                            <br />
                            <asp:Label ID="lblGerente" runat="server"></asp:Label>
                        </div>
                        <div style="float: left; display: block; margin-top: 20px;">
                            <label>Texto a Buscar</label>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                            <asp:Button ID="btnIraBrief" runat="server" Text="Ir a Brief" />
                        </div>
                        <div class="spacer" style="padding: 5px;"></div>
                        <div style="float: left; display: block;">
                            <asp:HiddenField ID="hfidbrief" runat="server" />
                            <div class="spacer"></div>
                            <div class="spacer"></div>
                            <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
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
                                    <asp:TemplateField HeaderText="Población">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpoblacion" runat="server" Text='<%# Eval("Poblacion") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Tamaño">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltamano" runat="server" Text='<%# Eval("Tamano") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fiabilidad">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfiabilidad" runat="server" Text='<%# Eval("Fiabilidad") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desagregación">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesagregacion" runat="server" Text='<%# Eval("Desagregacion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fuente">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfuente" runat="server" Text='<%# Eval("Fuente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ponderación">
                                        <ItemTemplate>
                                            <asp:Label ID="lblponderacion" runat="server" Text='<%# Eval("Ponderacion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                ToolTip="Modificar" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                                OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
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
                                                        Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                        Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="spacer"></div>
                </div>
                <div id="accordion1" style="margin: 5px 10px; background-color: #fff;">
                    <h3><a href="#">Información del Diseño Muestral</a></h3>
                    <div>
                        <asp:HiddenField ID="hfiddiseno" runat="server" />
                        <div style="float: left;">
                            <asp:Label ID="lbltitulo2" runat="server"></asp:Label>
                            <br />
                            <asp:Label ID="lblGerente2" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div style="float: left; display: block; margin-top: 20px; width: 80%;">
                            <label>Fecha:</label>
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle" Enabled="false"></asp:TextBox> 
                        </div> 
                        <div style="float: left; display: block; margin-top: 20px; width: 99%;">
                            <asp:CheckBoxList ID="chklista" runat="server" AutoPostBack="True" RepeatColumns="4"
                                Width="100%">
                                <asp:ListItem Value="0">Muestreo Probabilístico</asp:ListItem>
                                <asp:ListItem Value="1">Objetivo de la investigación</asp:ListItem>
                                <asp:ListItem Value="2">Población Objetivo</asp:ListItem>
                                <asp:ListItem Value="3">Mercado: Cubrimiento geográfico</asp:ListItem>
                                <asp:ListItem Value="4">Marco Muestral</asp:ListItem>
                                <asp:ListItem Value="5">Técnica</asp:ListItem>
                                <asp:ListItem Value="6">Diseño Muestral</asp:ListItem>
                                <asp:ListItem Value="7">Tamaño de la muestra</asp:ListItem>
                                <asp:ListItem Value="8">Fiabilidad de los resultados</asp:ListItem>
                                <asp:ListItem Value="9">Desagregación básica de los resultados</asp:ListItem>
                                <asp:ListItem Value="10">Fuente para la elaboración de la distribución muestral</asp:ListItem>
                                <asp:ListItem Value="11">Ponderación</asp:ListItem>
                                <asp:ListItem Value="12">Variable</asp:ListItem>
                                <asp:ListItem Value="13">Observaciones</asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:Panel ID="pnlobjetivos" runat="server" Visible="false">
                                    <label>
                                        Objetivos de la Investigación</label>
                                    <asp:TextBox ID="txtObjetivos" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnlpoblacion" runat="server" Visible="false">
                                    <label>
                                        Población Objetivo</label>
                                    <asp:TextBox ID="txtPoblacion" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnlmercado" runat="server" Visible="false">
                                    <label>
                                        Mercado, Cubrimiento geográfico</label>
                                    <asp:TextBox ID="txtMercado" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnlmarco" runat="server" Visible="false">
                                    <label>
                                        Marco muestral</label>
                                    <asp:Editor ID="txtMarcoMuestral" NoUnicode="true" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnltecnica" runat="server" Visible="false">
                                    <label>
                                        Técnica</label>
                                    <asp:TextBox ID="txtTecnica" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnldiseno" runat="server" Visible="false">
                                    <label>
                                        Método de selección de los entrevistados y procedimiento de muestreo</label>
                                    <asp:TextBox ID="txtDiseno" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnltamano" runat="server" Visible="false">
                                    <label>
                                        Tamaño de la muestra</label>
                                    <asp:Editor ID="txtTamano" NoUnicode="true" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlfiabilidad" runat="server" Visible="false">
                                    <label>
                                        Fiabilidad de los resultados</label>
                                    <asp:TextBox ID="txtFiabilidad" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>
                                <asp:Panel ID="pnldesagregacion" runat="server" Visible="false">
                                    <label>
                                        Nivel de desagregación de los resultados</label>
                                    <asp:TextBox ID="txtDesagregacion" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlfuente" runat="server" Visible="false">
                                    <label>
                                        Fuente para la elaboración de la distribución muestral</label>
                                    <asp:TextBox ID="txtFuente" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlPonderacion" runat="server" Visible="false">
                                    <label>
                                        Método de ponderación</label>
                                    <asp:Editor ID="txtPonderacion" NoUnicode="true" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlVariable" runat="server" Visible="false">
                                    <label>
                                        Variables de ponderación o factores de expansión</label>
                                    <asp:Editor ID="txtVariable" NoUnicode="true" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlObservaciones" runat="server" Visible="false">
                                    <label>Observaciones</label>
                                    <asp:TextBox ID="txtObservaciones" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                                </asp:Panel>

                            </asp:Panel>
                        </div>
                        <div class="spacer"></div>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                        &nbsp;
                        <input id="Button1" type="submit" class="button" value="Cancelar" style="font-size: 11px;" onclick="location.href = 'BriefDisenoDeMuestra.aspx';" />
                        <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Documentos" />
                    </div>
                    <div class="spacer"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
