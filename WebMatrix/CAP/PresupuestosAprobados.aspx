<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master" CodeBehind="PresupuestosAprobados.aspx.vb" Inherits="WebMatrix.PresupuestosAprobados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <style type="text/css">
  table { text-align : center
        }
        tr {text-align : left}
        th {text-align : left}
        td{text-align : left       }

        
           .modalBackground 
        {
            background-color:Black ;
            filter:alpha(opacity=70);
            opacity:0.7;
        }
        .PanelPop {
            color: #FFFFFF;
        }
        .auto-style2 {
            width: 160px;
        }
        .auto-style3 {
            font-size: 11px;
            color: #FFFFFF;
        }
        .auto-style5 {
            width: 169px;
        }

        #stylized input,select{
            width:inherit;
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

    <table style="width:100%;">
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>Id Propuesta:</td>
                        <td>
                            <asp:TextBox ID="txtIdPropuesta" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="txtIdPropuesta">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>JobBook:</td>
                        <td>
                            <asp:TextBox ID="txtJobBook" runat="server"></asp:TextBox>
                        </td>
                        <td>Id Trabajo:</td>
                        <td>
                            <asp:TextBox ID="txtIdTrabajo" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtIdTrabajo_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="txtIdTrabajo">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>Tecnica:</td>
                        <td>
                            <asp:DropDownList ID="lstTecnica" runat="server">
                            </asp:DropDownList>
                        </td>
                         <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnBuscar" runat="server" Text="BUSCAR" BackColor="#CC3300" Font-Bold="True" ForeColor="White" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvPresupAprobados" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" PageSize="20" Width="100%" EmptyDataText="No existen datos">
                            <Columns>
                                <asp:TemplateField HeaderText="PROPUESTA" Visible="False">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("IDPROPUESTA") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("IDPROPUESTA") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TECODIGO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("TECCODIGO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("TECCODIGO") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TecNombre" HeaderText="TECNICA" />
                                <asp:TemplateField HeaderText="METCODIGO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("METCODIGO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("METCODIGO") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MetNombre" HeaderText="METODOLOGIA" />
                                <asp:TemplateField HeaderText="ALT">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ParAlternativa") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ParAlternativa") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PARNACIONAL" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("PARNACIONAL") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("PARNACIONAL") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DescFase" HeaderText="FASE" />
                                <asp:BoundField DataField="ParNumJobBook" HeaderText="--JOBBOOK--" />
                                <asp:TemplateField HeaderText="OBSERVACIONES">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("ParNomPresupuesto") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObs" runat="server" Height="50px" TextMode="MultiLine" Width="200px" Text='<%# Bind("ParNomPresupuesto") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdTrabajo" HeaderText="IdTrabajo" />
                                <asp:BoundField DataField="NomTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:TemplateField ShowHeader="False" HeaderText="PRESUP.">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" 
                                            CommandName="VER" ImageUrl="~/Images/Select_16.png" Text="VER" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ButtonType="Button"  CommandName="ACT" Text="Ajustar" />
                                <asp:ButtonField ButtonType="Button" CommandName="VERP" HeaderText="Presup." Text="Ver" />
                                <asp:CheckBoxfield DataField="ParAprobado" HeaderText="Aprobado" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Height="420px" Width="650px" BackColor="#188B86">
                    <asp:UpdatePanel ID="upCambio" runat="server">
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td>Tecnica:</td>
                                                <td>
                                                    <asp:Label ID="lbltecnica" runat="server"></asp:Label>
                                                </td>
                                                <td>Metodologia:</td>
                                                <td>
                                                    <asp:Label ID="lblmetodologia" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Alternativa:</td>
                                                <td>
                                                    <asp:Label ID="lblalternativa" runat="server"></asp:Label>
                                                </td>
                                                <td>Fase:</td>
                                                <td>
                                                    <asp:Label ID="lblFase" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>JobBook:</td>
                                                <td>
                                                    <asp:Label ID="lbljob" runat="server"></asp:Label>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td class="auto-style2">Actividad:</td>
                                                <td>
                                                    <asp:DropDownList ID="lstActividad" runat="server" AutoPostBack="True" Width="100%">
                                                    </asp:DropDownList>
                                                    <asp:ListSearchExtender ID="lstActividad_ListSearchExtender" runat="server" PromptText="Digite el texto a buscar:" QueryPattern="Contains" TargetControlID="lstActividad">
                                                    </asp:ListSearchExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td>Costo Actividad Actual</td>
                                                <td>Venta Operacion Actual</td>
                                                <td>Margen Operaciones Actual</td>
                                                <td>% GM Actual</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCostoActual" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCostoOperaconActual" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMargenOperacionesActual" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPorcActual" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="PanelPop"><strong>**Utilice solo valor o solo porcentaje</strong></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="auto-style5">Ajuste al costo:</td>
                                                <td>
                                                    <asp:TextBox ID="txtNuevoCosto" runat="server" Width="100px"></asp:TextBox>
                                                    &nbsp;<span class="auto-style3">*Valor para aumentar(positivo) / para disminuir(negativo)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style5">Porcentaje:</td>
                                                <td>
                                                    <asp:TextBox ID="txtPorcentaje" runat="server" Width="100px"></asp:TextBox>
                                                    &nbsp;<span class="auto-style3">*Utilice % en decimales usando COMA</span>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="auto-style5">% Que asume operaciones:</td>
                                                <td>
                                                    <asp:TextBox ID="txtPorcOperaciones" runat="server" Width="100px"></asp:TextBox>
                                                    &nbsp;<span class="auto-style3">*Utilice % en decimales usando COMA - 1 para 100%</span>
                                                    <asp:FilteredTextBoxExtender ID="txtPorcOperaciones_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtPorcOperaciones" ValidChars=",">
                                                    </asp:FilteredTextBoxExtender>
                                                    </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAplicar" runat="server" Text="    Aplicar    " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td>Costo Actividad Nuevo</td>
                                                <td>Venta Operacion Nuevo </td>
                                                <td>Margen Operaciones Nuevo </td>
                                                <td>% GM Nuevo</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNuevoValor" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCostoOperacon" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMargenOperaciones" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPorcentajeOperaciones" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancelar" runat="server" Text="    Cerrar    " />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="LinkButton1_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="Panel1" TargetControlID="LinkButton1" CancelControlID="btnCancelar">
                </asp:ModalPopupExtender>
            </td>
        </tr>
    </table>
</asp:Content>
