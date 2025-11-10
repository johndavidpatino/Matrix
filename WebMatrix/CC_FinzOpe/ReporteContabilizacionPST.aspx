<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ReporteContabilizacionPST.aspx.vb" Inherits="WebMatrix.ReporteContabilizacionPST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
     <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtfechainicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechainicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtfechafin.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechafin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });



            validationForm();

        }
        $(document).ready(function () {
            loadPlugins();
        });
    </script>
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
            <asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
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
                <div id="accordion0">
                    
                    <h3><a href="#">Reporte Contabilizacion PST</a></h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Inicio</label>
                                <asp:TextBox ID="txtfechainicio" runat="server" CssClass="textEntry"></asp:TextBox>
                                 <br />
                                 <asp:Button ID="btnconsultar" runat ="server" Text ="Consultar" />

                                                             
                                
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>Fecha Fin</label>
                                <asp:TextBox ID="txtfechafin" runat="server" CssClass="textEntry"></asp:TextBox>
                                <br />
                                <asp:Button ID="btngenerar" runat ="server" Text ="Generar" />
                            </fieldset>

                        </div>
                         <asp:GridView ID="GvContabilizacion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="100"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="PersonaId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="PersonaId" HeaderText="Cedula" />
                                 <asp:BoundField DataField="TrabajoId" HeaderText="Trabajo" />
                                <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                <asp:BoundField DataField="CantidadEntrevistas" HeaderText="CantidadEntrevistas" />
                                <asp:BoundField DataField="ValoraPagar" DataFormatString="{0:C0}" HeaderText="ValoraPagar" />
                                <asp:BoundField DataField="DiasTrabajados" HeaderText="DiasTrabajados" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />                                                                                       
                                <asp:BoundField DataField="CuentaContable" HeaderText="CuentaContable" />
                                <asp:BoundField DataField="CCJOB" HeaderText="CCJOB" />
                                <asp:BoundField DataField="CiudadId" HeaderText="CiudadId" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="PorcentajeTrabajoPersona" DataFormatString="{0:0%}" HeaderText="PorcentajeTrabajoPersona" />
                                <asp:BoundField DataField="PresupuestoId" HeaderText="PresupuestoId" Visible ="false"  />
                                <asp:BoundField DataField="TarifaTransporte" DataFormatString="{0:C0}" HeaderText="TarifaTransporte" />
                                <asp:BoundField DataField="Transporte" DataFormatString="{0:C0}" HeaderText="Transporte" />
                                <asp:BoundField DataField="CCTRANSPORTE" HeaderText="CCTRANSPORTE" />
                                <asp:BoundField DataField="Provision" DataFormatString="{0:C0}" HeaderText="Provision" />
                                
                            </Columns>
                        </asp:GridView>

                        </div>
                     
                      
                       
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
