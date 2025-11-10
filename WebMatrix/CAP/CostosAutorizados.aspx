<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="CostosAutorizados.aspx.vb" Inherits="WebMatrix.AgregarCostosEjecutados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 146px;
            
        }
        
             tr {text-align : left}
        th {text-align : left}
        td{text-align : left       }
        .style2
        {
            width: 754px;
        }
        .style3
        {
            text-align: center;
            color: #FFFFFF;
        }
        .style4
        {
            font-weight: bold;
            text-align: center;
        }
          .modalBackground 
        {
            background-color:Black ;
            filter:alpha(opacity=70);
            opacity:0.7;
        }
        
       /*Para creacion de formulario sin usar tablas*/
     .formLayout
    {
       /* background-color: #f3f3f3;
        border: solid 1px #a1a1a1; 
        padding: 10px;*/
        width: 800px;
        height :400px;
        position:relative ;

    }
    
    .formLayout label, .formLayout input
    {
        display: block;
        width: 120px;
        float: left;
        margin-bottom: 10px;
    }
 
    .formLayout label
    {
        text-align: right;
        padding-right: 20px;
    }
 
    br
    {
        clear: left;
    }
        
    .FormFooter
    {
        
            /* Position at the bottom */
        position: absolute;
        top: 90%;
        width :100%;
        height :20%;
        /* Center */
        margin: 0 auto;
        text-align:center ;
          
    
    } */

        .style5
        {
            height: 106px;
        }
        .style6
        {
            height: 258px;
        }

        .style7
        {
            width: 756px;
        }
        .style8
        {
            width: 80px;
        }

        .style10
        {
            width: 217px;
            color: #FFFFFF;
        }
        .style11
        {
            width: 192px;
            color: #FFFFFF;
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
    <table style="width:100%;">
        <tr>
            <td>
    <table style="width:100%;">
        <tr>
            <td class="style8">
                Nombre:</td>
            <td class="style7">
                <asp:Label ID="lblNombre" runat="server"></asp:Label>
            </td>
            <td style="text-align: right">
                <asp:LinkButton ID="lkVolver" runat="server" Font-Bold="True" 
                    Font-Size="Medium" ForeColor="White">VOLVER</asp:LinkButton>
            </td>
        </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style1">
                            IdPropuesta:</td>
                        <td class="style2">
                            <asp:Label ID="lblIdPropuesta" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Tecnica:</td>
                        <td class="style2">
                            <asp:Label ID="lblTecnica" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Metodologia:</td>
                        <td class="style2">
                            <asp:Label ID="lblMetodologia" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Fase:</td>
                        <td class="style2">
                            <asp:Label ID="lblFase" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Muestra:</td>
                        <td class="style2">
                            <asp:Label ID="LblMuestra" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Productvidad:</td>
                        <td class="style2">
                            <asp:Label ID="lblProductividad" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Contactos no efectivos:</td>
                        <td class="style2">
                            <asp:Label ID="lblContactosNo" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            JobBook:</td>
                        <td class="style2">
                            <asp:Label ID="lblJobBook" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Dias campo:</td>
                        <td class="style2">
                            <asp:Label ID="lblDiasCampo" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Duracion:</td>
                        <td class="style2">
                            <asp:Label ID="lblDuracion" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            Num encuestadores:</td>
                        <td class="style2">
                            <asp:Label ID="lblNumEncuestadores" runat="server"></asp:Label>
                        </td>
                        <td class="style2">
                            &nbsp;</td>
                        <td class="style2">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style10">
                            <strong>OBSERVACIONES PRESUPUESTO:</strong></td>
                        <td>
                            <asp:Label ID="lblObserPresup" runat="server" Width="100%"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style3">
                            <b>CERRADAS</td>
                        <td class="style3">
                            CERRADAS&nbsp; MULTIPLES</td>
                        <td class="style3">
                            ABIERTAS</td>
                        <td class="style3">
                            ABIERTAS MULTIPLES</td>
                        <td class="style3">
                            OTROS</td>
                        <td class="style3">
                            DEMOGRAFCOS</b></td>
                        <td class="style3">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style4">
                            <asp:Label ID="lblCerradas" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="lblCerradasMult" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="lblAbiertas" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="lblAbiertasMult" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="lblOtros" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="lblDemograficos" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Button ID="btnExportar" runat="server" Text="Exportar a excel" />
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvControlCostos" runat="server" AutoGenerateColumns="False" 
                            CssClass="displayTable" ShowFooter="True" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}" 
                                    HeaderText="PRESUPUESTADO" />
                                <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}" 
                                    HeaderText="AUTORIZADO" />
                                <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}" 
                                    HeaderText="PRESUP VS AUTO" />
                                <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" 
                                    HeaderText="%" />
                                <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" 
                                    HeaderText="PRODUCCION" />
                                <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" 
                                    HeaderText="PRESUP VS PROD" />
                                <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" 
                                    HeaderText="%" />
                                <asp:BoundField DataField="EJECUTADO" DataFormatString="{0:C0}" 
                                    HeaderText="EJECUTADO" />
                                <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" 
                                    HeaderText="PRESUP VS EJEC" />
                                <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" 
                                    HeaderText="%" />
                                <asp:BoundField DataField="ActTipoValor" HeaderText="TIPO VALOR" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="false" 
                                            CommandName="ADD" Text="Agregar" CommandArgument="<%# Container.DataItemIndex %>"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField />
                            </Columns>
                            <FooterStyle Font-Bold="True" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Height="400px" Width="800px" 
                    BackColor="#17908B" ScrollBars="Vertical">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div  >
                            
                                                         <table style="width:100%; height: 100%;">
                                                              <tr>
                                                                  <td class="style5">
                                                                      <table style="width:100%;">
                                                                          <tr>
                                                                              <td>
                                                                                  Actividad:</td>
                                                                              <td>
                                                                                  <asp:DropDownList ID="lstActividades" runat="server" AutoPostBack="True">
                                                                                  </asp:DropDownList>
                                                                              </td>
                                                                          </tr>
                                                                          <tr>
                                                                              <td>
                                                                                  Valor:</td>
                                                                              <td>
                                                                                  <asp:TextBox ID="txtValorAutorizado" runat="server" Width="100px"></asp:TextBox>
                                                                              </td>
                                                                          </tr>
                                                                          <tr>
                                                                              <td>
                                                                                  Observaciones:</td>
                                                                              <td>
                                                                                  <asp:TextBox ID="txtObservacion" runat="server" Width="400px"></asp:TextBox>
                                                                              </td>
                                                                          </tr>
                                                                          <tr>
                                                                              <td>
                                                                                  &nbsp;</td>
                                                                              <td>
                                                                                  <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                                                                              </td>
                                                                          </tr>
                                                                      </table>
                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td class="style6" valign="top">
                                                                      <asp:GridView ID="gvdetalle" runat="server" CssClass="displayTable" 
                                                                          Width="100%" AutoGenerateColumns="False" 
                                                                          EmptyDataText="No existe registros" ShowFooter="True">
                                                                          <Columns>
                                                                              <asp:BoundField DataField="Consecutivo" HeaderText="No" >
                                                                              </asp:BoundField>
                                                                              <asp:BoundField DataField="OBSERVACION" HeaderText="OBSERVACION">
                                                                              <ItemStyle Width="60%" />
                                                                              </asp:BoundField>
                                                                              <asp:BoundField DataField="ValorAutorizado" HeaderText="VALOR" 
                                                                                  DataFormatString="{0:c0}" />
                                                                              <asp:TemplateField ShowHeader="False">
                                                                                  <ItemTemplate>
                                                                                      <asp:Button ID="Button1" runat="server" CausesValidation="false" CommandName="DEL" CommandArgument="<%# Container.DataItemIndex %>"  
                                                                                          Text="Eliminar" OnClientClick="return confirm('Realmente el registro seleccionado ?')"  />
                                                                                  </ItemTemplate>
                                                                                  <ItemStyle HorizontalAlign="Center" />
                                                                              </asp:TemplateField>
                                                                          </Columns>
                                                                          <FooterStyle Font-Bold="True" Font-Size="Medium" />
                                                                      </asp:GridView>
                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td style="text-align: center">
                                                                      <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="100px" />
                                                                  </td>
                                                              </tr>
                                                          </table>
                                                         
                                                       
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" 
                    Enabled="True" TargetControlID="Panel1">
                </asp:RoundedCornersExtender>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style11">
                            <strong>OBSERVACIONES GENERALES:</strong></td>
                        <td>
                            <asp:Label ID="lblObserGenerales" runat="server" Width="100%"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#17908B">LinkButton</asp:LinkButton>
                <asp:ModalPopupExtender ID="LinkButton1_ModalPopupExtender" runat="server" 
                    BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" 
                    PopupControlID="Panel1" TargetControlID="LinkButton1">
                </asp:ModalPopupExtender>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
