<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master"
    CodeBehind="IPS.aspx.vb" Inherits="WebMatrix.IPS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();
           <%-- $('#<%= btnGuardar.ClientID %>').click(function (evt) {

            });--%>


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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>REPORTE DE OBSERVACIONES</a>
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
    <div id="accordion">
        <div id="accordion1">
            <h3>
                <a href="#">
                    <label>
                        Manejo de Observaciones</label>
                </a>
                <asp:HiddenField ID="hfidtrabajo" runat="server" />
                <asp:HiddenField ID="hfidwf" runat="server" />
                <asp:HiddenField ID="hfGerentePY" runat="server" />
                <asp:HiddenField ID="hfidtarea" runat="server" />
            </h3>
            <div class="block">
                <div id="tabs">
                    <ul>
                        <li>
                            <a href="#tab-1">Observaciones</a>
                        </li>
                    </ul>
                    <div id="tab-1">
                        <asp:Panel ID="pnllistarevision" runat="server" Height="350px" ScrollBars="Auto">
                            Tarea:
                            <asp:Label ID="lblNombreTarea" runat="server"></asp:Label>
                            <asp:GridView ID="gvRevision" runat="server" AutoGenerateColumns="False" Width="110%" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" Caption="OBSERVACIONES"
                                ShowFooter="True" AllowSorting="True" OnSorting="gvRevision_Sorting">
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
                                                <b>Descripción Observación</b>
                                            </td>
                                            <td runat="server" id="colVersion">
                                                <b>Versión</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" id="colInstrumento2">
                                                <asp:DropDownList ID="txtInstrumentoEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                    <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                    <asp:ListItem Value="9">Cuestionario</asp:ListItem>
                                                    <asp:ListItem Value="10">Instructivo</asp:ListItem>
                                                    <asp:ListItem Value="11">Metodología</asp:ListItem>
                                                    <asp:ListItem Value="12">Tarjetas</asp:ListItem>
                                                    <asp:ListItem Value="13">Circular</asp:ListItem>
                                                    <asp:ListItem Value="70">Tracking-Archivo de cambios</asp:ListItem>
                                                    <asp:ListItem Value="80">Guion de verificación</asp:ListItem>
                                                    <asp:ListItem Value="106">LLCC</asp:ListItem>
                                                    <asp:ListItem Value="74">Otros documentos</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colPregunta2">
                                                <asp:TextBox ID="txtPreguntaEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                            </td>
                                            <td runat="server" id="colAplicativo2">
                                                <asp:DropDownList ID="ddlAplicativoEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Gandia" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Dimensions" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Survey" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="SPSS Statistics" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Other system" Value="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colProceso2">
                                                <asp:DropDownList ID="ddlProcesoEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                    BorderColor="Silver" BorderStyle="Solid">
                                                </asp:DropDownList>
                                            </td>
                                            <td runat="server" id="colSolicitud2">
                                                <asp:DropDownList ID="txtObservacionEmpty" runat="server" CssClass="textEntry" Width="140px"
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
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFechaRegfooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario que Registra" SortExpression="UsuarioRegistra">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUsuarioReg" runat="server" Text='<%# Eval("UsuarioRegistra")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUsuarioRegedit" runat="server" Text='<%# Bind("UsuarioRegistra")%>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtUsuarioRegfooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Instrumento" SortExpression="Instrumento">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumento" runat="server" Text='<%# Eval("Instrumento")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtInstrumentoedit" runat="server" Text='<%# If(Not (Eval("CodInstrumento") Is Nothing), Eval("CodInstrumento"), 0)%>'
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                <asp:ListItem Value="9">Cuestionario</asp:ListItem>
                                                <asp:ListItem Value="10">Especificaciones Técnicas del Trabajo</asp:ListItem>
                                                <asp:ListItem Value="11">Metodología</asp:ListItem>
                                                <asp:ListItem Value="12">Tarjetas</asp:ListItem>
                                                <asp:ListItem Value="13">Circular</asp:ListItem>
                                                <asp:ListItem Value="70">Tracking-Archivo de cambios</asp:ListItem>
                                                <asp:ListItem Value="80">Guion de verificación</asp:ListItem>
                                                <asp:ListItem Value="106">LLCC</asp:ListItem>
                                                <asp:ListItem Value="74">Otros documentos</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtInstrumentofooter" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">---Seleccione---</asp:ListItem>
                                                <asp:ListItem Value="9">Cuestionario</asp:ListItem>
                                                <asp:ListItem Value="10">Especificaciones Técnicas del Trabajo</asp:ListItem>
                                                <asp:ListItem Value="11">Metodología</asp:ListItem>
                                                <asp:ListItem Value="12">Tarjetas</asp:ListItem>
                                                <asp:ListItem Value="13">Circular</asp:ListItem>
                                                <asp:ListItem Value="70">Tracking-Archivo de cambios</asp:ListItem>
                                                <asp:ListItem Value="80">Guion de verificación</asp:ListItem>
                                                <asp:ListItem Value="106">LLCC</asp:ListItem>
                                                <asp:ListItem Value="74">Otros documentos</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pregunta" SortExpression="Pregunta">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pregunta") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPreguntaedit" runat="server" Text='<%# Bind("Pregunta") %>' CssClass="textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPreguntafooter" runat="server" CssClass="textEntry" Width="140px"
                                                BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aplicativo" SortExpression="Aplicativo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAplicativo" runat="server" Text='<%# Eval("Aplicativo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlAplicativoedit" runat="server" Text='<%# If(Not (Eval("CodAplicativo") Is Nothing), Eval("CodAplicativo"), 0)%>'
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Text="--Seleccione--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Gandia" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Dimensions" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Survey" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="SPSS Statistics" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Other system" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlAplicativofooter" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Text="--Seleccione--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Gandia" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Dimensions" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Survey" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="SPSS Statistics" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Other system" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proceso" SortExpression="Proceso">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProceso" runat="server" Text='<%# Eval("Proceso")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlProcesoedit" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlProcesofooter" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Solicitud" SortExpression="Observacion">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Observacion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="txtObservacionedit" runat="server" Text='<%# Bind("CodObservacion")%>'
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="1">ADICIONAL</asp:ListItem>
                                                <asp:ListItem Value="2">CAMBIO</asp:ListItem>
                                                <asp:ListItem Value="3">ERROR</asp:ListItem>
                                                <asp:ListItem Value="4">SUGERENCIA</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtObservacionfooter" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="1">ADICIONAL</asp:ListItem>
                                                <asp:ListItem Value="2">CAMBIO</asp:ListItem>
                                                <asp:ListItem Value="3">ERROR</asp:ListItem>
                                                <asp:ListItem Value="4">SUGERENCIA</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción Observación" SortExpression="DescripcionObservacion">
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
                                                BorderColor="Silver" BorderStyle="Solid" Width="140px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtVersionfooter" runat="server" CssClass="textEntry" Width="140px"
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
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">RECHAZAR</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTAR</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="txtRechazarfooter" runat="server"
                                                CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid">
                                                <asp:ListItem Value="0">RECHAZAR</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTAR</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Espacio para Revisión" SortExpression="RespuestaProgramador">
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
                                    <asp:TemplateField ShowHeader="False" HeaderText="Modificar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton32223" runat="server" CausesValidation="False" CommandName="Edit"
                                                ImageUrl="~/Images/Select_16.png" Text="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ImageButton322" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/Images/save_16.png" Text="Update" ToolTip="Guardar" />
                                            &nbsp;&nbsp;<asp:ImageButton ID="ImageButton2211" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/Images/no.jpg" Text="Cancel" ToolTip="Cancelar" />
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Button ID="ctrl_btnaddfooter" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                OnClick="AgregarRevision" Font-Size="11px" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete"
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
