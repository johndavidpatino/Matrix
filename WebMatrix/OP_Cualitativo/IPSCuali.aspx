<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="IPSCuali.aspx.vb" Inherits="WebMatrix.IPSCuali1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
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

            $("#tabs").tabs();
            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });
        }
    </script>
    <style>
        .ml-10 {
            margin: 10px;
        }

        .obsText {
            margin: 2px 5px;
        }

        .text-center {
            margin: 0px auto;
            text-align: center;
            float: none !important;
        }

            .text-center input[type=image] {
                outline: none !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu">
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    REPORTE DE OBSERVACIONES
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
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
    <div id="accordion">
        <div id="accordion0" style="margin: 5px 10px; background-color: #fff;">
            <h3><a href="#">Manejo de Observaciones</a></h3>
            <asp:HiddenField ID="hfidtrabajo" runat="server" />
            <asp:HiddenField ID="hfidwf" runat="server" />
            <asp:HiddenField ID="hfGerentePY" runat="server" />
            <asp:HiddenField ID="hfidtarea" runat="server" />
            <div class="block">
                <div id="tabs">
                    <div id="tab-1">
                        <asp:Panel ID="pnllistarevision" runat="server" Height="350px" ScrollBars="Auto" CssClass="ml-10">
                            <h4 id="lblNombreTarea" runat="server"></h4>
                            <asp:GridView ID="gvRevision" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" ShowFooter="True" AllowSorting="True" OnSorting="gvRevision_Sorting">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <EmptyDataRowStyle VerticalAlign="Top" />
                                <EmptyDataTemplate>
                                    <table style="vertical-align: top" runat="server" id="tabla">
                                        <tr>
                                            <td runat="server" id="colInstrumento">
                                                <b>Tipo Instrumento</b>
                                            </td>
                                            <td runat="server" id="colPregunta">
                                                <b>Pregunta</b>
                                            </td>
                                            <td runat="server" id="colAplicativo">
                                                <b>Aplicativo</b>
                                            </td>
                                            <td runat="server" id="colProceso">
                                                <b>Proceso</b>
                                            </td>
                                            <td runat="server" id="colSolicitud">
                                                <b>Tipo Solicitud</b>
                                            </td>
                                            <td runat="server" id="colDescripcion">
                                                <b>Observación</b>
                                            </td>
                                            <td runat="server" id="colVersion">
                                                <b>Versión</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="colInstrumento2">
                                                <asp:DropDownList ID="txtInstrumentoEmpty" runat="server" CssClass="textEntry" Width="100px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                    <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                    <asp:ListItem Value="17">Filtro</asp:ListItem>
                                                    <asp:ListItem Value="93">Acta</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colPregunta2">
                                                <asp:TextBox ID="txtPreguntaEmpty" runat="server" CssClass="textEntry" Width="50px"
                                                    BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                            </td>
                                            <td runat="server" id="colAplicativo2">
                                                <asp:DropDownList ID="ddlAplicativoEmpty" runat="server" CssClass="textEntry" Width="50px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colProceso2">
                                                <asp:DropDownList ID="ddlProcesoEmpty" runat="server" CssClass="textEntry" Width="100px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colSolicitud2">
                                                <asp:DropDownList ID="txtObservacionEmpty" runat="server" CssClass="textEntry" Width="100px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                    <asp:ListItem Value="1">ADICIONAL</asp:ListItem>
                                                    <asp:ListItem Value="2">CAMBIO</asp:ListItem>
                                                    <asp:ListItem Value="3">ERROR</asp:ListItem>
                                                    <asp:ListItem Value="4">SUGERENCIA</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colDescripcion2">
                                                <asp:TextBox ID="txtDescripcionEmpty" TextMode="MultiLine" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                            </td>
                                            <td runat="server" id="colVersion2">
                                                <asp:TextBox ID="txtVersionEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="ctrl_btnaddempty" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                    OnClick="AgregarRevision" Font-Size="11px" />
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField ShowHeader="false" Visible="false" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha de Registro" SortExpression="FechaHoraObservacion">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFechaReg" runat="server" Text='<%# Eval("FechaHoraObservacion")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFechaRegedit" runat="server" Text='<%# Bind("FechaHoraObservacion")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="100px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFechaRegfooter" runat="server" CssClass="textEntry" Width="100px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario que Registra" SortExpression="UsuarioRegistra">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUsuarioReg" runat="server" Text='<%# Eval("UsuarioRegistra")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUsuarioRegedit" runat="server" Text='<%# Bind("UsuarioRegistra")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="100px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtUsuarioRegfooter" runat="server" CssClass="textEntry" Width="100px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Instrumento" SortExpression="Instrumento">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumento" runat="server" Text='<%# Eval("Instrumento")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtInstrumentoedit" runat="server" Text='<%# If(Not (Eval("CodInstrumento") Is Nothing), Eval("CodInstrumento"), 0)%>'
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                <asp:ListItem Value="17">Filtro</asp:ListItem>
                                                <asp:ListItem Value="93">Acta</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtInstrumentofooter" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                <asp:ListItem Value="17">Filtro</asp:ListItem>
                                                <asp:ListItem Value="93">Acta</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pregunta" SortExpression="Pregunta">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pregunta") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPreguntaedit" runat="server" Text='<%# Bind("Pregunta") %>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="50px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPreguntafooter" runat="server" CssClass="textEntry" Width="50px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aplicativo" SortExpression="Aplicativo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAplicativo" runat="server" Text='<%# Eval("Aplicativo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlAplicativoedit" runat="server" Text='<%# If(Not (Eval("CodAplicativo") Is Nothing), Eval("CodAplicativo"), 0)%>'
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Text="--Seleccione--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlAplicativofooter" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Text="--Seleccione--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proceso" SortExpression="Proceso">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProceso" runat="server" Text='<%# Eval("Proceso")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlProcesoedit" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlProcesofooter" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Solicitud" SortExpression="Observacion">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Observacion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtObservacionedit" runat="server" Text='<%# Bind("CodObservacion")%>'
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="1">ADICIONAL</asp:ListItem>
                                                <asp:ListItem Value="2">CAMBIO</asp:ListItem>
                                                <asp:ListItem Value="3">ERROR</asp:ListItem>
                                                <asp:ListItem Value="4">SUGERENCIA</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtObservacionfooter" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="1">ADICIONAL</asp:ListItem>
                                                <asp:ListItem Value="2">CAMBIO</asp:ListItem>
                                                <asp:ListItem Value="3">ERROR</asp:ListItem>
                                                <asp:ListItem Value="4">SUGERENCIA</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Observación" SortExpression="DescripcionObservacion">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("DescripcionObservacion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescripcionedit" TextMode="MultiLine" runat="server" Text='<%# Bind("DescripcionObservacion") %>'
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDescripcionfooter" TextMode="MultiLine" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Versión" SortExpression="VersionScript">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("VersionScript")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVersionedit" runat="server" Text='<%# Bind("VersionScript")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="50px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtVersionfooter" runat="server" CssClass="textEntry" Width="50px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha de Revisión" SortExpression="FechaHoraRespuesta">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFechaProg" runat="server" Text='<%# Eval("FechaHoraRespuesta")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFechaProgedit" runat="server" Text='<%# Bind("FechaHoraRespuesta")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFechaProgfooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario Revisor" SortExpression="UsuarioProgramador">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUsuarioProg" runat="server" Text='<%# Eval("UsuarioProgramador")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUsuarioProgedit" runat="server" Text='<%# Bind("UsuarioProgramador")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtUsuarioProgfooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rechazar" SortExpression="Rechazar">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRechazar" runat="server" Text='<%# Eval("Rechazar")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtRechazaredit" runat="server" Text='<%# If(Not (Eval("CodRechazar") Is Nothing), Eval("CodRechazar"), 1)%>'
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">RECHAZAR</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTAR</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtRechazarfooter" runat="server"
                                                CssClass="textEntry" Width="100px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">RECHAZAR</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTAR</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revisión" SortExpression="RespuestaProgramador">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("RespuestaProgramador")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEspacioedit" runat="server" TextMode="MultiLine" Text='<%# Bind("RespuestaProgramador")%>'
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEspaciofooter" runat="server" TextMode="MultiLine" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("Estado")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEstadoedit" runat="server" Text='<%# Bind("Estado")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEstadofooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" HeaderText="Modificar" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton32223" runat="server" CausesValidation="False" CssClass="text-center" CommandName="Edit"
                                                ImageUrl="~/Images/Select_16.png" Text="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ImageButton322" runat="server" CausesValidation="True" CssClass="text-center" CommandName="Update"
                                                ImageUrl="~/Images/save_16.png" Text="Update" ToolTip="Guardar" />
                                            &nbsp;&nbsp;<asp:ImageButton ID="ImageButton2211" runat="server" CausesValidation="False" CssClass="text-center"
                                                CommandName="Cancel" ImageUrl="~/Images/no.jpg" Text="Cancel" ToolTip="Cancelar" />
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Button ID="ctrl_btnaddfooter" runat="server" Text="Agregar" CssClass="text-center buttonText buttonSave corner-all"
                                                OnClick="AgregarRevision" Font-Size="11px" Width="100px" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CssClass="text-center" CommandName="Delete"
                                                CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png" />

                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" SelectCommand="SELECT [Id], [Proceso] FROM [OP_IPS_Procesos] WHERE ([IdTarea] = @IdTarea)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfidtarea" Name="IdTarea" PropertyName="Value" Type="Int64" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </asp:Panel>
                    </div>

                </div>
                <div class="actions">
                </div>
                <br />
                <div class="actions">
                    <div class="form_right">
                        <fieldset>
                            <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                            &nbsp;
                            <asp:Button ID="btnCancelar" runat="server" Text="Volver al Trabajo" />
                            &nbsp;
                            <asp:Button ID="btnNotificar" runat="server" Text="Notificar observaciones" />
                            &nbsp;
                            <asp:Button ID="btnRechazar" runat="server" Text="Rechazar Error" />
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
