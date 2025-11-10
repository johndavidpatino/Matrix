<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ControlCostos.aspx.vb" Inherits="WebMatrix.ControlCostos" %>
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
        
         .RightAlign
        {
            text-align: Right;
        }
        
        .style2
        {
            width: 754px;
        }
        
        
        body
{
    font-family:verdana,tahoma,helvetica;
}

/* ajax__tab_red-theme theme (images/red.jpg) */
.ajax__tab_red-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    font-weight:bold;
    border-bottom:solid 5px #a50000;
}
.ajax__tab_red-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/red.jpg) #d8d8d8 repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:bottom;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_red-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#000;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_red-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/red.jpg) #bfdaff repeat-x left -1300px;
}
.ajax__tab_red-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#fff;
}
.ajax__tab_red-theme .ajax__tab_active .ajax__tab_outer
{
    background:url(images/red.jpg) #a50000 repeat-x left -1400px;
}
.ajax__tab_red-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#edf5ff;    
    border:solid 1px #808080;
    border-top-width:0px;
}

/* ajax__tab_lightblue-theme theme (images/lightblue.jpg) */
.ajax__tab_lightblue-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    border-bottom:solid 5px #c2e0fd;
}
.ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/lightblue.jpg) #d8d8d8 repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:bottom;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#000;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_lightblue-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/lightblue.jpg) #bfdaff repeat-x left -1300px;
}
.ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#000;
}
.ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_outer
{
    background:url(images/lightblue.jpg) #ffffff repeat-x left -1400px;
}
.ajax__tab_lightblue-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#ffffff;    
    border:solid 1px #808080;
    border-top-width:0px;
}

/* ajax__tab_green-theme theme (images/green.jpg) */
.ajax__tab_green-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    border-bottom:solid 5px #7cdb44;
}
.ajax__tab_green-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/green.jpg) #d8d8d8 repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:bottom;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_green-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#000;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_green-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/green.jpg) #bfdaff repeat-x left -1300px;
}
.ajax__tab_green-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#fff;
}
.ajax__tab_green-theme .ajax__tab_active .ajax__tab_outer
{
    background:url(images/green.jpg) #7cdb44 repeat-x left -1400px;
}
.ajax__tab_green-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#edf5ff;    
    border:solid 1px #808080;
    border-top-width:0px;
}
/* ajax__tab_orange-theme theme (images/orange.jpg) */
.ajax__tab_orange-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    border-bottom:solid 5px #84aeef;
}
.ajax__tab_orange-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/orange.jpg) #d8d8d8 repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:bottom;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_orange-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#000;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_orange-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/orange.jpg) #bfdaff repeat-x left -1300px;
}
.ajax__tab_orange-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#fff;
}
.ajax__tab_orange-theme .ajax__tab_active .ajax__tab_outer
{
    background:url(images/orange.jpg) #84aeef repeat-x left -1400px;
}
.ajax__tab_orange-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#edf5ff;    
    border:solid 1px #808080;
    border-top-width:0px;
}

/* ajax__tab_darkblue-theme theme (images/darkblue.jpg) */
.ajax__tab_darkblue-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    border-bottom:solid 5px #84aeef;
}
.ajax__tab_darkblue-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/darkblue.jpg) #d8d8d8 repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:bottom;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_darkblue-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#000;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_darkblue-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/darkblue.jpg) #bfdaff repeat-x left -1300px;
}
.ajax__tab_darkblue-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#fff;
}
.ajax__tab_darkblue-theme .ajax__tab_active .ajax__tab_outer
{
    background:url(images/darkblue.jpg) #84aeef repeat-x left -1400px;
}
.ajax__tab_darkblue-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#edf5ff;    
    border:solid 1px #808080;
    border-top-width:0px;
}
/* ajax__tab_blueGrad-theme theme (images/blueGrad.jpg) */
.ajax__tab_blueGrad-theme .ajax__tab_header 
{
    font-family:arial,helvetica,clean,sans-serif;
    font-size:small;
    color:#ffffff;
    border-bottom:solid 5px #84aeef;
}
.ajax__tab_blueGrad-theme .ajax__tab_header .ajax__tab_outer 
{
    background:url(images/blueGrad.jpg) repeat-x;
    margin:0px 0.16em 0px 0px;
    padding:1px 0px 1px 0px;
    vertical-align:middle;
    border:solid 1px #a3a3a3;
    border-bottom-width:0px;
}
.ajax__tab_blueGrad-theme .ajax__tab_header .ajax__tab_tab
{    
    color:#ffffff;
    padding:0.35em 0.75em;    
    margin-right:0.01em;
}
.ajax__tab_blueGrad-theme .ajax__tab_hover .ajax__tab_outer 
{
    background: url(images/blueGrad.jpg) #84aeef repeat-x left -1300px;
}
.ajax__tab_blueGrad-theme .ajax__tab_active .ajax__tab_tab 
{
    color:#fff;
}
.ajax__tab_blueGrad-theme .ajax__tab_active .ajax__tab_outer
{
     background:url(images/blueGrad.jpg) #84aeef repeat-x left -1400px;  
}
.ajax__tab_blueGrad-theme .ajax__tab_body 
{
    font-family:verdana,tahoma,helvetica;
    font-size:10pt;
    padding:0.25em 0.5em;
    background-color:#edf5ff;    
    border:solid 1px #808080;
    border-top-width:0px;
}
        
        .auto-style1 {
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

    <asp:Panel ID="mPanel" runat="server">
        <table style="width:100%;">
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
                            <td class="style2">Muestra:</td>
                            <td class="style2">
                                <asp:Label ID="lblMuestra" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right">
                                <asp:LinkButton ID="LKVOLVER" runat="server" Font-Bold="True" 
                                Font-Size="Medium">VOLVER</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width:100%;">
                        <tr>
                            <td class="auto-style1"><b>CERRADAS</b></td>
                            <td class="auto-style1">CERRADAS&nbsp; MULTIPLES</td>
                            <td class="auto-style1">ABIERTAS</td>
                            <td class="auto-style1">ABIERTAS MULTIPLES</td>
                            <td class="auto-style1">OTROS</td>
                            <td class="auto-style1">DEMOGRAFCOS</b></td>
                            <td class="auto-style1">&nbsp;</td>
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
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar excel" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
                    Width="100%" CssClass="ajax__tab_lightblue-theme">
                        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                            <HeaderTemplate>
                                Detalles costo de la unidad
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvControlCostos" runat="server" AutoGenerateColumns="False" 
                                CssClass="displayTable" ShowFooter="True" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                        <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                        <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUPUESTADO" >
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}" 
                                        HeaderText="AUTORIZADO" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS AUTO" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" 
                                        HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" 
                                        HeaderText="PRODUCCION" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS PROD" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" 
                                        HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="EJECUTADO" DataFormatString="{0:C0}" 
                                        HeaderText="EJECUTADO" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS EJEC" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" 
                                        HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="UNIDADES" DataFormatString="{0:N0}" 
                                        HeaderText="UNIDADES" />
                                        <asp:BoundField DataField="DESC_UNIDADES" HeaderText="DESCRIPCION" />
                                    </Columns>
                                    <FooterStyle Font-Bold="True" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                            <HeaderTemplate>
                                Detalles costo de operaciones
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvDetallesOperaciones" runat="server" 
                                AutoGenerateColumns="False" CssClass="displayTable" ShowFooter="True" 
                                Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                        <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                        <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUPUESTADO" />
                                        <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}" 
                                        HeaderText="AUTORIZADO" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS AUTO" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" HeaderText="%" 
                                        Visible="False" />
                                        <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}" 
                                        HeaderText="PRODUCCION" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS PROD" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}" 
                                        HeaderText="%" Visible="False" />
                                        <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}" 
                                        HeaderText="PRESUP VS EJEC" Visible="False" />
                                        <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%" 
                                        Visible="False" />
                                        <asp:BoundField DataField="UNIDADES" DataFormatString="{0:N0}" 
                                        HeaderText="UNIDADES" />
                                        <asp:BoundField DataField="DESC_UNIDADES" HeaderText="DESCRIPCION" />
                                        <asp:BoundField DataField="HORAS" HeaderText="HORAS" />
                                    </Columns>
                                    <FooterStyle Font-Bold="True" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                            <HeaderTemplate>
                                Viaticos presupuesto
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvViaticos" runat="server" AutoGenerateColumns="False" 
                                CssClass="displayTable" ShowFooter="True" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="CODIGO" HeaderText="CODIGO" />
                                        <asp:BoundField DataField="CIUDAD" HeaderText="CIUDAD" />
                                        <asp:BoundField DataField="HOTELES" DataFormatString="{0:C0}" 
                                        HeaderText="HOTELES" />
                                        <asp:BoundField DataField="TRANSPORTE" DataFormatString="{0:C0}" 
                                        HeaderText="TANSPORTE" />
                                        <asp:BoundField DataField="TOTAL" DataFormatString="{0:C0}" 
                                        HeaderText="TOTAL" />
                                    </Columns>
                                    <FooterStyle Font-Bold="True" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <br />
</asp:Content>
