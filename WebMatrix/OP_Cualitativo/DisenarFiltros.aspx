<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_F.master"
    CodeBehind="DisenarFiltros.aspx.vb" Inherits="WebMatrix.DisenarFiltros" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../Styles/Filtros.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();

            $("#<%= txtFechaIni.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaIni.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFin.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientId %>").datepicker({
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

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });


        }
        function redireccion() {
            var identrevista= $('#<%= hfTipoFiltro.ClientID %>').val();
            document.location.href = 'Trabajos.aspx?TipoFiltro=' + identrevista;

        }
    </script>
    <style type="text/css">
        #accordion1 {
            margin-bottom: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Diseñador de Filtros</a>
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
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <br /><br />
    <asp:Label ID="lblTextoTrabajo" runat="server" Text="Trabajo:" ForeColor="White" Font-Bold="True"></asp:Label>
    <asp:Label ID="lblTrabajo" runat="server" ForeColor="White" Font-Bold="True"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <div style="width: 100%">
                <div id="container">

                    <div class="actions">
                        <asp:Panel ID="pnlCrearFiltro" runat="server" Visible="false">
                            <h3>
                                <label id="lblCrearFiltro" runat="server"></label>
                            </h3>
                            <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                            <asp:HiddenField ID="hfTipoFiltro" runat="server" />
                            <asp:HiddenField ID="hfPY" runat="server" />
                            <div class="form_left">
                                <fieldset>
                                    <label>Fecha Inicio:</label>
                                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>Fecha Final:</label>
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    <asp:Button ID="btnCrear" runat="server" Text="Crear Filtro"  />
                                </fieldset>
                            </div>
                            
                            
                        </asp:Panel>
                        <asp:Panel ID="pnlFiltros" runat="server" Visible="false">
                                <h4 style="text-align:center">Lista de Filtros</h4>
                                <asp:GridView ID="gvFiltros" runat="server" Width="70%" AutoGenerateColumns="False"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id, IdTipoFiltro" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="TipoFiltro" HeaderText="Tipo Filtro" />
                                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgGestionar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Gestionar" ImageUrl="~/Images/Select_16.png" Text="Gestionar" ToolTip="Gestionar" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Aprobaciones" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAprobaciones" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Aprobar" ImageUrl="~/Images/list_16_.png" Text="Aprobaciones" ToolTip="Aprobar" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                        </asp:Panel>    
                     </div>

                    <div style="min-height: 400px">
                    
                    <asp:Panel ID="pnlNewFiltro" runat="server" Visible="false">
                        <asp:HiddenField ID="hfIdFiltro" runat="server" value="0"/>
                        <asp:HiddenField ID="hfIdPregunta" runat="server" value="0"/>
                        <div id="campo-formulario1">
                        <h4>Preguntas Filtro</h4>
                        <label>Tipo Pregunta</label>
                        <asp:DropDownList ID="ddlTipoPregunta" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <label id="lblOrden" runat="server" visible="false">Orden Pregunta</label>
                        <asp:TextBox ID="txtOrden" runat="server" Visible="false"></asp:TextBox>
                        <label>Texto Pregunta</label>
                        <asp:TextBox ID="txtTextoPregunta" runat="server" CssClass="textMultiline"
                            Height="50px" TextMode="MultiLine"></asp:TextBox>

                        <asp:Panel ID="pnlRespuestas" CssClass="actions" runat="server" Visible="false">
                            <label>Respuestas</label>
                            <asp:TextBox ID="txtRespuestas" runat="server" CssClass="textMultiline"
                            Height="30px" TextMode="MultiLine"></asp:TextBox>
                            &nbsp;
                            <asp:Button ID="btnAddRespuestas" runat="server" Text="Añadir" />
                            <asp:ListBox ID="lstRespuestas" runat="server" Visible="false" Height="80px"></asp:ListBox>
                            <asp:Button ID="btnRemoveRespuestas" runat="server" Text="Eliminar" visible="false"/>
                        </asp:Panel>
                        <asp:CheckBox ID="chbObligatoria" text="Obligatoria" runat="server" />
                            <br />
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                        <br></br>
                        
                            <br>
                        
                        </br>

                        </div>
                    </asp:Panel>
                    
                   <asp:Panel ID="pnlVisualizar" runat="server" Visible="false">
                    <div id="campo-formulario3">
                        <asp:Panel ID="pnlGenerarlink" CssClass="actions" runat="server" BorderWidth="1px" BorderColor="White">
                        <label>Generar Link del Filtro para Diligenciar</label>
                        <asp:Button ID="btnGenerar" runat="server" Text="Generar Link" />
                        <label id="lblLink" runat="server">
                        </label>
                        </asp:Panel>
                        <h4 style="font-style:inherit">Visualizador Preguntas</h4>
                        <asp:Panel ID="pnlPreguntas" runat="server" CssClass="actions">

                        </asp:Panel>
                    </div>
                  </asp:Panel>
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
