<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master"
    CodeBehind="FichaCuantitativa.aspx.vb" Inherits="WebMatrix.FichaCuantitativa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
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


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });
        }
        function redireccion() {
            var idtrabajo = $('#<%= hfidtrabajo.ClientID %>').val();
            document.location.href = 'FichaCuantitativa.aspx?idtrabajo=' + idtrabajo;

        }
    </script>
    <style type="text/css">
        .style2
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Instructivo General del Trabajo</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Complete las diferentes especificaciones del trabajo
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
                <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
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
                <h3 style="float: left; text-align: left;">
                        <a>
                           
                                Información Ficha Cuantitativa<asp:HiddenField ID="hfidfichacuantitativa" runat="server" /><asp:HiddenField ID="hfidtrabajo" runat="server" />
                        </a></h3>
                         <div class="block">
                           <fieldset class="validationGroup">
                            <div>
                             <asp:Panel ID="Panel1" runat="server">
                             <div class="form_left">
                                        <fieldset>
                                            <label>
                                              Nombre Trabajo</label>
                                                <asp:TextBox ID="txtNombreProyecto" runat="server" CssClass="textEntry" 
                                                ReadOnly="True"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                <div class="form_left">
                                        <fieldset>
                                            <label>
                                              </label>
                                            <asp:TextBox ID="txtJobBook" Visible="false" runat="server" CssClass="textEntry" 
                                                ReadOnly="True"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                   
                                    <div class="actions">
                                    
                                      <table style="font-size: 14px; width: 805px;">
                                        <tr>
                                        <td>
                                                Incentivos:
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblIncentivos" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1" Text="Si">Si</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                Regalos Cliente
                                            </td>
                                            <td class="style2">
                                                <asp:RadioButtonList ID="rblRegaloClientes" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                Compra Ipsos
                                            </td>
                                            <td class="style2">
                                                <asp:RadioButtonList ID="rblCompraIpsos" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Grupo Objetivo</label>
                                            <asp:TextBox ID="txtGrupoObjetivo" runat="server" CssClass="textMultiline" Height="100px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Cubrimiento Geográfico</label>
                                            <asp:TextBox ID="txtCubrimientoGeografico" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Marco Muestral</label>
                                            <asp:TextBox ID="txtMarcoMuestral" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                    
                                        <fieldset>
                                            <label>
                                                Distribución Muestral</label>
                                            <cc1:Editor ID="txtDistribucionMuestral" NoUnicode="true" Width="100%" Height="200px" runat="server" />
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Variables para control en el RMC</label>
                                            <asp:TextBox ID="txtCuotas" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Nivel Desagregación de Resultados</label>
                                            <asp:TextBox ID="txtNivelDesagregacionResultados" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                               Ponderación</label>
                                            <asp:TextBox ID="txtPonderacion" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                               Requerimientos Especiales</label>
                                            <asp:TextBox ID="txtRequerimientosEspeciales" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                 <div class="actions">
                                        <fieldset>
                                            <label>
                                               Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</label>
                                            <asp:TextBox ID="txtHabeasData" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine" ></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                               Otras Observaciones</label>
                                            <asp:TextBox ID="txtOtrasObservaciones" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                        </div>
                                    
                             </asp:Panel>
                              
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" ValidationGroup="Guardar" />
                                            &nbsp;
                                            <asp:Button ID="btnCancelar" runat="server" Text="Volver" />
                                            &nbsp;
                                            <asp:Button ID="btnEntrega" runat="server" Text="Entrega" ValidationGroup="Guardar" Visible="false" />
                                            &nbsp;
                                            <asp:Button ID="btnVolverOP" runat="server" Text="Volver" Visible="false" />

                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                            </fieldset>
                         </div>
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
