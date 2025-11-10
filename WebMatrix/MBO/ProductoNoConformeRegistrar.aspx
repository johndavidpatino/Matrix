<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="ProductoNoConformeRegistrar.aspx.vb" Inherits="WebMatrix.ProductoNoConformeRegistrar" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
      
        $(document).ready(function () {
            loadpluggins();
        });

        function pageLoad() {
            $('#FechaIncidente').unbind();
            $("#<%= FechaReclamo.ClientID%>").mask("99/99/9999");
            $("#<%= FechaReclamo.ClientID%>").datepicker("setDate", new Date());
            $("#<%= FechaReclamo.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#FechaAccion').unbind();
            $("#<%= FechaPlanAccion.ClientID%>").mask("99/99/9999");
            $("#<%= FechaPlanAccion.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#FechaCierre').unbind();
            $("#<%= FechaEjecAccion.ClientID%>").mask("99/99/9999");
            $("#<%= FechaEjecAccion.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function loadpluggins() {

            $('#FechaIncidente').unbind();
            $("#<%= FechaReclamo.ClientID%>").mask("99/99/9999");
            $("#<%= FechaReclamo.ClientID%>").datepicker("setDate", new Date());
            $("#<%= FechaReclamo.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#FechaAccion').unbind();
            $("#<%= FechaPlanAccion.ClientID%>").mask("99/99/9999");
            $("#<%= FechaPlanAccion.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $('#FechaCierre').unbind();
            $("#<%= FechaEjecAccion.ClientID%>").mask("99/99/9999");
            $("#<%= FechaEjecAccion.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

    </script>

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
        .auto-style1 {
            height: 19px;
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
            <asp:HiddenField ID="hfId" runat="server" Value="0" />
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
        
       </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Filtro1" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <table style="width:100%;">
        <tr>
            <td width="5%">
                <asp:RadioButton ID="rdTrabajo" runat="server" Text="TRABAJO" AutoPostBack="True" />
            </td>
            <td width="5%">
                <asp:RadioButton ID="rdEstudio" runat="server" Text="ESTUDIO" AutoPostBack="True" />
            </td>
            <td width="10%"><a>JOB BOOK</a></td>
            <td width="20%"><asp:TextBox ID="lblJobBook" runat="server" Width="200px"> </asp:TextBox></td>
            <td width="10%"><asp:Button ID="btnBuscar" runat="server" Text="Buscar JB" Width="80px" BackColor="#CC3300" Font-Bold="True" ForeColor="White" /></td>
            <td width="50%"><asp:DropDownList ID="NEstudio" runat="server" Width="500px" AutoPostBack="True"></asp:DropDownList></td>
        </tr>
    </table>
    <table style="width:100%;"> 
        <tr>   
            <td width="100%"><a>TRABAJOS </a></td>
        </tr>
        <tr> 
            <td width="100%"> 
                <asp:GridView ID="GVTrabajos" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False"  CssClass="displayTable" PageSize="10" Width="100%" EmptyDataText="No existen datos" AutoGenerateSelectButton="True">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="IdTrabajo" />
                        <asp:BoundField DataField="JobBook" HeaderText="Job Book" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />                       
                    </Columns>
                    <SelectedRowStyle foreColor="red" font-bold="true"/>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td width="100%"><a> </a>></td>
        </tr>
        <tr>  
            <td width="100%"><a>PRODUCTO NO CONFORME</a></td>
        </tr>
        <tr> 
            <td width="100%">
                <asp:GridView ID="GVPNC" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False"  CssClass="displayTable" PageSize="5" Width="100%" EmptyDataText="No existen datos" AutoGenerateSelectButton="True">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="IdPNC" />
                        <asp:BoundField DataField="JobBook" HeaderText="Job Book" />
                        <asp:BoundField DataField="IdTrabajo" HeaderText="IdTrabajo" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" /> 
                        <asp:BoundField DataField="Fuente" HeaderText="Fuente" />
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                        <asp:BoundField DataField="Reporta" HeaderText="Quien Reporta" />
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                        <asp:TemplateField HeaderText="Descripcion">
                            <ItemTemplate>
                                <asp:TextBox ID="Descripcion" runat="server" TextMode="MultiLine" Width="80px" Text='<%# Bind("Descripcion")%>' Height="30px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>                  
                    </Columns>
                    <SelectedRowStyle foreColor="red" font-bold="true"/>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td width="100%"><a> </a>></td>
        </tr>
        <tr>  
            <td width="100%"><a>ACCIONES</a></td>
        </tr>
        <tr> 
            <td width="100%">
                <asp:GridView ID="GVPNCDetalle" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False"  CssClass="displayTable" PageSize="6" Width="100%" EmptyDataText="No existen datos" AutoGenerateSelectButton="True">
                    <Columns>
                        <asp:BoundField DataField="IdPNC" HeaderText="IdPNC" />
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:TemplateField HeaderText="CausaRaiz">
                            <ItemTemplate>
                                <asp:TextBox ID="CausaRaiz" runat="server" TextMode="MultiLine" Width="80px" Text='<%# Bind("CausaRaiz")%>' Height="50px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TipoAccion" HeaderText="TipoAccion" /> 
                        <asp:TemplateField HeaderText="Accion">
                            <ItemTemplate>
                                <asp:TextBox ID="DescripcionAccion" runat="server" TextMode="MultiLine" Width="80px" Text='<%# Bind("DescripcionAccion")%>' Height="50px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="FechaPlaneada" HeaderText="Fec.Planeada" DataFormatString="{0:d}"/>
                        <asp:BoundField DataField="FechaEjecucion" HeaderText="Fec.Ejecucion" DataFormatString="{0:d}"/>
                        <asp:BoundField DataField="ResponsableAccion" HeaderText="Resp.Accion" />
                        <asp:BoundField DataField="ResponsableSeguimiento" HeaderText="Resp.Seguimiento" />
                        
                        <asp:TemplateField HeaderText="EvidenciaCierre">
                            <ItemTemplate>
                                <asp:TextBox ID="EvidenciaCierre" runat="server" TextMode="MultiLine" Width="80px" Text='<%# Bind("EvidenciaCierre")%>' Height="50px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle foreColor="red" font-bold="true"/>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>


     <asp:UpdatePanel ID="Filtro2" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <table style="width:100%;">
        <tr>
            <td width="100%"><a> </a>></td>
        </tr>
         <tr>  
            <td width="100%"><a>DATOS PRODUCTO NO CONFORME</a></td>
        </tr>
    </table>
    <table style="width:100%;">     
        <tr>
            <td width="10%"><a>Job Book</a></td>
            <td width="15%"><asp:TextBox ID="JobBook" runat="server" Width="120px" ReadOnly="true"> </asp:TextBox></td>
            <td width="10%"><a>Trabajo</a></td>
            <td width="10%"><asp:TextBox ID="IdTrabajo" runat="server" Width="120px" ReadOnly="true"> </asp:TextBox></td>
            <td width="10%"><a>Nombre trabajo</a></td>
            <td width="25%"><asp:TextBox ID="NomTrabajo" runat="server" Width="250px" ReadOnly="true"> </asp:TextBox></td>
            <td width="10%"><a>Unidad</a></td>
            <td width="10%"><asp:TextBox ID="Unidad" runat="server" Width="120px" ReadOnly="true"></asp:TextBox></td>
        </tr>
     </table>
    </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Datos1" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <table style="width:100%;">
        <tr>
            <td width="15%"><a>Quien reporta</a></td>
            <td width="15%"><a>Cliente</a></td>
            <td width="10%"><a>Fecha reclamo</a></td>
            <td width="20%"><a>Fuente del reclamo</a></td>
            <td width="20%"><a>Categoria</a></td> 
            <td width="20%"><a>Tarea</a></td>    
        </tr>
        <tr>
            <td><asp:TextBox ID="Reporta" runat="server" Width="180px" ReadOnly="true"></asp:TextBox></td>
            <td><asp:TextBox ID="Cliente" runat="server" Width="180px" ReadOnly="true"></asp:TextBox></td>
            <td><asp:TextBox ID="FechaReclamo" runat="server" Width="100px" CssClass="bgCalendar textCalendarStyle"> </asp:TextBox></td>
            <td><asp:DropDownList ID="Fuente" runat="server" Width="170px" AutoPostBack="True"></asp:DropDownList></td>
            <td><asp:DropDownList ID="Categoria" runat="server" Width="170px" AutoPostBack="True"></asp:DropDownList></td>
            <td><asp:DropDownList ID="Tarea" runat="server" Width="170px" AutoPostBack="True"></asp:DropDownList></td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Datos2" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <Table style="width:100%;">
        <tr>
            <td width="10%"><a>Descripcion problema</a></td>
            <td width="90%"><asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Width="100%" MaxLength="200"></asp:TextBox></td>
        </tr>
    </Table>
    <table style="width:100%;"> 
        <tr>
            <td width="100%"><a> </a>></td>
        </tr>
        <tr>  
            <td width="100%"><a>CAUSA Y ACCIONES</a></td>
        </tr> 
    </table>
    <Table style="width:100%;">
        <tr>
            <td width="10%"><a>Causa raiz</a></td>
            <td width="90%"><asp:TextBox ID="txtCausa" runat="server" TextMode="MultiLine" Width="100%" MaxLength="200"></asp:TextBox></td>
        </tr>
    </table>
    <Table style="width:100%;">
        <tr>
            <td width="10%"><a>TipoAccion</a></td>
            <td width="15%"><asp:DropDownList ID="TipoAccion" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList></td>
            <td width="5%"><a>Accion</a></td>
            <td width="70%"><asp:TextBox ID="txtAccion" runat="server" TextMode="MultiLine" Width="750px" MaxLength="200"></asp:TextBox></td>
        </tr>
    </table>
    <Table style="width:100%;">
        <tr>
            <td width="20%"><a>Responsable accion</a></td>
            <td width="20%"><a>Responsable seguimiento</a></td>
            <td width="10%"><a>Fecha planeacion accion</a></td>
            <td width="10%"><a>Fecha Ejecucion accion</a></td>
        </tr>
        <tr>       
            <td><asp:DropDownList ID="ResponsableAccion" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList></td>
            <td><asp:DropDownList ID="ResponsableSeguir" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList></td>
            <td><asp:TextBox ID="FechaPlanAccion" runat="server" Width="100px" CssClass="bgCalendar textCalendarStyle"> </asp:TextBox></td>
            <td><asp:TextBox ID="FechaEjecAccion" runat="server" Width="100px" CssClass="bgCalendar textCalendarStyle"> </asp:TextBox></td>
        </tr>
    </table>
    <table style="width:100%;">
         <tr>
            <td width="10%"><a>Evidencia de cierre</a></td>
            <td width="90%"><asp:TextBox ID="txtEvidencia" runat="server" TextMode="MultiLine" Width="100%" MaxLength="200"></asp:TextBox></td>
        </tr>
    </table>
    
    </ContentTemplate>
    </asp:UpdatePanel>

     <asp:UpdatePanel ID="Datos3" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <table style="width:100%;">
        <tr>
            <td><asp:Button ID="btnGrabarPNCDetalle" runat="server" Text="AGREGAR ACCIONES" Width="150px"  BackColor="#CC3300" Font-Bold="True" ForeColor="White" /></td>
            <td><asp:Button ID="btnActualizarFecha" runat="server" Text="ACTUALIZAR FECHA y EVIDENCIA" Width="230px" BackColor="#CC3300" Font-Bold="True" ForeColor="White" /></td>
            <td><asp:Button ID="btnGrabarPNC" runat="server" Text="GRABAR NUEVO PNC" Width="150px" BackColor="#CC3300" Font-Bold="True" ForeColor="White" /></td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
