<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterEstadistica.master"
    CodeBehind="BriefDisenoDeMuestra.aspx.vb" Inherits="WebMatrix.BriefDisenoDeMuestra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
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

            $('#BusquedaVersionesBDM').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones de Brief Diseño Muestral",
                    width: "600px"
                });

            $('#BusquedaVersionesBDM').parent().appendTo("form");

            validationForm();
        }

        function MostrarVersionesBDM(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesBDM').dialog("open");
            }
        }

    </script>
    <style>
        .text-center {
            margin: 0px auto;
            text-align: center;
        }

        .btnVersion {
            float: right !important;
            color: #000 !important;
            font-size: 1.1em !important;
        }

        .error {
            color: #f00;
            display: block !important;
            font-size: 14px !important;
            font-weight: bold !important;
            margin-bottom: 5px !important;
        }

        #stylized label {
            text-align: left !important;
            width: auto !important;
        }

        .caja {
            display: block;
            float: left;
            margin: 10px 0;
            text-align: left;
        }

            .caja label {
                display: block;
                float: left;
                margin: 5px 0;
                text-align: left;
            }

        .mt-15 {
            margin-top: 15px;
        }

        .m-0 {
            margin: 0;
        } 
        .ui-widget-header {
            background-color: #3b9f9a;
            border: 1px solid #3b9f9a;
        }

        .text-center {
            margin: 0px auto;
            text-align: center;
        }

        .cambioVersion1 {
            background-color: #ea7b7e;
            color: white;
            border: 1px solid #000 !important;
        }

        .cambioVersion {
            /*background-color: #52bb69;*/
            background-color: transparent;
            color: black;
            border: 1px solid #000 !important;
        }

        .lblScroll {
            overflow-x: scroll;
            overflow-y: scroll;
            border: 1px solid;
        }

        .lblScrolllbl {
            overflow-x: scroll;
            padding: 10px;
            border: 1px solid;
        }

        .versionIgual {
            background-color: transparent;
            color: black;
            border: 1px solid;
        } 

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Brief de Diseño Muestral
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Consulte los briefs de diseño muestral requeridos para las propuestas
    <br />
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
            <div>
                <asp:Panel runat="server" ID="accordion1">
                    <h4 class="caja" style="margin-bottom: 0px;">
                        <a>Consulta</a>
                    </h4>
                    <div class="spacer">
                        <label>
                            Texto a Buscar</label>
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        <asp:Button ID="btnVolver" runat="server" Text="Volver" Visible="false" />
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                                    DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="Propuesta" HeaderText="Propuesta" />
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
                                <asp:TemplateField HeaderText="Capacidad">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcapacidad" runat="server" Text='<%# Eval("Capacidad") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Metodología">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmetodologia" runat="server" Text='<%# Eval("Metodologia") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="N. Desagregación">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesagregacion" runat="server" Text='<%# Eval("Desagregacion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posibles Marcos M">
                                    <ItemTemplate>
                                        <asp:Label ID="lblposiblesmarcos" runat="server" Text='<%# Eval("PosiblesMarcos") %>'></asp:Label>
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
                                <asp:TemplateField HeaderText="Diseño" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Diseno" ImageUrl="~/Images/list_16.png"
                                            Text="Diseño" ToolTip="Diseño Muestral" />
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
                    <div class="spacer"></div>
                </asp:Panel>
                <div class="spacer"></div>
                <asp:Panel runat="server" ID="accordion2" Visible="false">
                    <asp:HiddenField ID="hfidbrief" runat="server" />
                    <asp:HiddenField ID="hfidpropuesta" runat="server" />
                    <asp:HiddenField ID="hfFlag" runat="server" Value="0" />
                    <asp:HiddenField ID="hfVersion" runat="server" Value="0" />

                    <h4 class="caja" style="margin-bottom: 0px;">
                        <a>Información del Brief Diseños Muestrales</a>
                    </h4>
                    <br />
                    <br />
                    <br />
                    <div class="caja mt-15" style="margin-bottom: 0px;">
                        <label>Fecha:</label>
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                        <label style="margin-left: 20px;">Gerente:</label>
                        <asp:TextBox ID="txtGerente" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="caja mt-15" style="margin-bottom: 0px; float: right;">
                        <a href="#" id="lblVersionBriefMuestral" class="btnVersion" onclick="MostrarVersionesBDM(this)" runat="server">Sin Versiones</a>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="caja" style="margin: -5px;">
                            <label>
                                Objetivos de la Investigación</label>
                            <asp:TextBox ID="txtObjetivos" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Población Objetivo</label>
                            <asp:TextBox ID="txtPoblacion" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Capacidad de Pago</label>
                            <asp:TextBox ID="txtCapacidad" Width="100%" Height="150px" TextMode="MultiLine" runat="server"></asp:TextBox>
                            <label>
                                Metodología de análisis y de trabajo de campo</label>
                            <asp:TextBox ID="txtMetodologia" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Niveles de desagregación</label>
                            <asp:TextBox ID="txtDesagregacion" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Posibles marcos muestrales</label>
                            <asp:TextBox ID="txtMarcos" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Variables de ponderación o factores de expansión</label>
                            <asp:TextBox ID="txtVariables" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                            <label>
                                Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" Width="100%" Height="100px" runat="server" />
                        </div>
                        <div class="spacer"></div>
                        <div class="caja mt-15">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" CssClass="causesValidation" />
                            &nbsp;
                            <input id="Button1" type="submit" class="button" value="Cancelar" style="font-size: 11px;" onclick="location.href = 'BriefDisenoDeMuestra.aspx';" />
                            <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Documentos" />
                    </asp:Panel>
                </asp:Panel>
            </div>
            <div class="spacer"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaVersionesBDM">
        <asp:UpdatePanel ID="UPanelVersionesBDM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVersionesBDM" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVersionesBDM" runat="server" Width="95%" AutoGenerateColumns="false"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id,NoVersion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Codigo" Visible="false" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="NoVersion" HeaderText="Versión" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/application_view_detail.png" Text="Ver" ToolTip="Ver" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comparar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" OnClientClick="return true;"
                                                CommandName="Comparar" ImageUrl="~/Images/comparar_32.png" Text="Comparar con versión anterior" ToolTip="Comparar con versión anterior" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetalleVerBDM" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Número Versión</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetNoVersion" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fecha</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetFecha" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Objetivo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetObjetivo" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Poblacion</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetPoblacion" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Capacidad</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetCapacidad" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Metodologia</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetMetodologia" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>NivelesDesagregacion</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetNivelesDesagregacion" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>PosiblesMarcos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetPosiblesMarcos" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Variable</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetVariable" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDetObservaciones" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <a href="#" style="font-size: 12px;" id="volverListadoVersBDM" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararBDM" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVersionBDM" ForeColor="Red" />
                        <div style="width: 95%; clear: both">
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:Label runat="server" ID="lblVersionA" ForeColor="Red"></asp:Label>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:Label runat="server" ID="lblVersionB" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="spacer"></div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Fecha</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompFecha1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompFecha2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Objetivo</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompObjetivo1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompObjetivo2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Poblacion</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompPoblacion1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompPoblacion2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Capacidad</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompCapacidad1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompCapacidad2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Metodologia</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompMetodologia1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompMetodologia2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Niveles de Desagregacion</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompNivelesDesagregacion1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompNivelesDesagregacion2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Posibles Marcos</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompPosiblesMarcos1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompPosiblesMarcos2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Variables</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompVariable1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompVariable2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 95%; clear: both">
                            <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                <p>Observaciones</p>
                            </div>
                            <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompObservaciones1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompObservaciones2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersBDM2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
