<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_F.master"
    CodeBehind="FichaEntrevista.aspx.vb" Inherits="WebMatrix.FichaEntrevista" %>

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
    </script>
    <style type="text/css">
        .style1
        {
            width: 93px;
        }
        .style2
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Acta de entrega de Proyecto - Entrevistas en Profundidad</a>
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
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Información Entrevistas<asp:HiddenField ID="hfidfichaentrevista" runat="server" /><asp:HiddenField ID="hfidtrabajo" runat="server" />
                            </label>
                            &nbsp;</a></h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Nombre Trabajo</label>
                                            <asp:TextBox ID="txtNombreProyecto" runat="server" CssClass="textEntry" ReadOnly="True"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Job Book</label>
                                            <asp:TextBox ID="txtJobBook" runat="server" CssClass="textEntry" ReadOnly="True"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    </div>
                                    <div class="actions">
                                        <h4>Distribución Muestral</h4>
                                        <asp:GridView ID="gvDistribucion" runat="server" AllowPaging="False" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false" CssClass="displayTable" 
                                            DataKeyNames="Id,CiudadId,IdModerador" EmptyDataText="No se encuentran opciones. Posiblemente no ha sido asignado el JBI. Comuníquese con Cuentas y/o Gerentes de Operaciones" PagerStyle-CssClass="headerfooter ui-toolbar">
                                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                            <SelectedRowStyle CssClass="SelectedRow" />
                                            <AlternatingRowStyle CssClass="odd" />
                                            <Columns>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Grupo Objetivo"/>
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad"/>
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="Moderador" HeaderText="Moderador" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="text-align: right"></div>
                                    <p style="color: White;">Backups Necesarios</p>
                                    Backups:
                                    <asp:TextBox ID="txtBackups" runat="server" CssClass="textEntry"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                TargetControlID="txtBackups">
                                            </asp:FilteredTextBoxExtender>
                                    <asp:CompareValidator ID="cvbackups" runat="server" ErrorMessage="[sólo números]"
                                        ControlToValidate="txtBackups" Type="Double" Operator="DataTypeCheck"
                                        ValidationGroup="Guardar"></asp:CompareValidator>
                                    <p style="color: White; ">Incentivos a utilizar</p>
                                    <table style="font-size: 14px">
                                        
                                        <tr>
                                            <td>
                                                Incentivos Económicos:
                                            </td>
                                            <td class="style2">
                                                <asp:RadioButtonList ID="rblIncentivos" runat="server" 
                                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                Presupuesto:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPresupuestoIncentivo" runat="server" CssClass="textEntry" Enabled="false"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtPresupuestoIncentivo">
                                                        </asp:FilteredTextBoxExtender>
                                                <asp:CompareValidator ID="cvpresupuestoincentivo" runat="server" ErrorMessage="[sólo números]"
                                                    ControlToValidate="txtPresupuestoIncentivo" Type="Double" Operator="DataTypeCheck"
                                                    ValidationGroup="Guardar"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Distribución Incentivos Económicos:</label>
                                            <asp:TextBox ID="txtDistribucionIncentivo" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                     <table style="font-size: 14px;">
                                        <tr>
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
                                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="style2">
                                                Presupuesto
                                            </td>
                                            <td class="style2">
                                                <asp:TextBox ID="txtPresupuesto" runat="server" CssClass="textEntry" Enabled="false"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtPresupuesto">
                                                        </asp:FilteredTextBoxExtender>
                                                <asp:CompareValidator ID="cvPresupuesto" runat="server" ErrorMessage="[sólo números]"
                                                    ControlToValidate="txtPresupuesto" Type="Double" Operator="DataTypeCheck" ValidationGroup="Guardar"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Distribución Compra Ipsos:</label>
                                            <asp:TextBox ID="txtDistribucionCompra" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div>
                                        <fieldset>
                                            <label>
                                                Ayudas Adicionales</label>
                                            <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" >
                                            </asp:CheckBoxList>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    
                                        <fieldset>
                                            <label>
                                                Método Aceptable de Reclutamiento</label>
                                             <asp:CheckBoxList ID="chbReclutamiento" runat="server" RepeatDirection="Horizontal" >
                                            </asp:CheckBoxList>
                                        </fieldset>
                                        </div>
                                    <div class="actions">
                                        <fieldset>
                                            <label>
                                                Observaciones, Exclusiones y Restricciones Específicas</label>
                                            <asp:TextBox ID="txtExclusionesyRestricciones" runat="server" CssClass="textMultiline"
                                                Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="actions">
                                    
                                        <fieldset>
                                            <label>
                                                Recursos Propiedad del Cliente</label>
                                            <asp:TextBox ID="txtRecursosPropiedadesCliente" runat="server" CssClass="textMultiline"
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
                                </asp:Panel>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                                            &nbsp;
                                            <asp:Button ID="btnCancelar" runat="server" Text="Volver" />
                                            &nbsp;
                                            <asp:Button ID="btnEntrega" runat="server" Text="Entrega" ValidationGroup="Guardar" />
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
