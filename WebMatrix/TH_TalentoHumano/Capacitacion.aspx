<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master"
    CodeBehind="Capacitacion.aspx.vb" Inherits="WebMatrix.Capacitacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/new/forms.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/new/tables.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/uploadFile.js" type="text/javascript"></script>
    <script src="../Scripts/js/capacitacion/MxContainer.js?v3" type="module" defer></script>
    <script src="../Scripts/js/capacitacion/MxTable.js" type="module" defer></script>
    <script src="../Scripts/js/capacitacion/personsLoad.js" type="module" defer></script>
    <script src="../Scripts/js/capacitacion/capacitacionPersonasSearch.js" type="module" defer></script>
    <script src="../Scripts/js/capacitacion/capacitacionParticipantesGet.js?v3" type="module" defer></script>
    <script type="text/javascript">
        let fullParticipantList = []

        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {
                var idresponsable = $('#<%= ddlresponsable.ClientID %>').val();
                if (idresponsable == '-1') {
                    validarSelect('<%= ddlresponsable.ClientID %>', "Debe seleccionar un responsable");
                }
            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            $("#tabs").tabs();

            $("#<%= txtFecha.ClientId %>").mask("99/99/9999");
            $("#<%= txtFecha.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });


        }

        function redireccion() {
            var idtrabajo = $('#<%= hfidtrabajo.ClientID %>').val();
            document.location.href = 'Capacitacion.aspx?idtrabajo=' + idtrabajo;

        }

        function onChangeActivityId(event){
            console.log(event.target)
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>ASISTENCIA A ACTIVIDADES DE FORMACIÓN</a>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
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
                                Lista de Información Técnica a Equipos de Recogida de Datos</label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfidtrabajo" runat="server" />
                         <div class="form_left">
                            <fieldset>
                                <label>
                                    Ubicación, Actividad, Responsable o Evaluado Por
                                </label>
                                &nbsp;<asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  />
                                <asp:Button ID="btnVolver" runat="server" Text="Volver"  />
                            </fieldset>
                        </div>
                         <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID Info Técnica"/>
                                <asp:BoundField DataField="TrabId" HeaderText="ID Trabajo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JBI" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="Horas" HeaderText="Duracion (Horas)" Visible="false" />
                                <asp:BoundField DataField="Actividad" HeaderText="Actividad" Visible="false" />
                                <asp:BoundField DataField="Responsable" HeaderText="Instructor" />
                                <asp:BoundField DataField="ModoEvaluacion" HeaderText="Modo Evaluación" Visible="false" />
                                <asp:BoundField DataField="EvaluadoPor" HeaderText="Evaluado Por" Visible="false" />
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                            OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                    <%= gvDatos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Información de la Información Técnica a Equipos</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Ubicación</label>
                                        <asp:HiddenField ID="hfidCapacitacion" runat="server"/>
                                        <asp:HiddenField ID="hfidCapacitacionRef" runat="server" />
                                        <asp:TextBox ID="txtUbicacion" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Fecha:</label>
                                        <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Duración (Horas)</label>
                                        <asp:TextBox ID="txtHoras" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtHoras" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Instructor</label>
                                        <asp:DropDownList ID="ddlresponsable" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Actividad</label>
                                        <asp:TextBox ID="txtActividad" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Modo Evaluación</label>
                                        <asp:TextBox ID="txtModoEvaluacion" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Evaluado Por</label>
                                       <asp:TextBox ID="txtEvaluadorPor" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                    <fieldset>
                                        <label>
                                            Objetivo de la Actividad de Formación</label>
                                        <asp:TextBox ID="txtObjetivoActividad" runat="server" CssClass="textMultiline" Height="100px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </fieldset>
                                </div>
                                
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar"  />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar" 
                                                style="font-size: 11px;" onclick="redireccion();"  />
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        
                    </div>
 
                </div>
                <div id="accordion3" ableEventValidation="false">
                    <h3>
                        <a href="#">
                            <label>
                                Resultados de la Información Técnica a Equipos</label>
                        </a>
                    </h3>
                    <div class="block">
                        <h4>Participantes que no aprobaron esta Información Técnica</h4>
                         <div class="actions">
                            <asp:GridView ID="gvResultado" runat="server" Width="50%" AutoGenerateColumns="False" DataKeyNames="id" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Participante" />
                                    <asp:BoundField DataField="CargoTexto" HeaderText="Cargo" />
                                    <asp:BoundField DataField="Eficacia" HeaderText="Calificación" />
                                </Columns>
                            </asp:GridView>
                            <asp:Panel ID="pnlAcciones" runat="server" Visible="false">
                                <asp:Button ID="btnRetro" runat="server" Text="Generar retroalimentación (Asistir de nuevo)" />
                            </asp:Panel>
                            <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Planillas"/>
                             <asp:Button ID="btnPlanilla" runat="server" Text="Generar Planillas"/>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <mx-container id="registerParticipantsModule">
        <div>
            <h2>Registrar personas a la actividad</h2>
            <section class="ip-container">
                <h4 class="ip-tableInner-title">Filtrar personas:</h4>
                <div  class="ip-formFilter">

                    <input id="inputPersonsSearchNombre" type="text" class="ip-inputField ip-inputField-text" placeholder="Nombre">
                    <input id="inputPersonsSearchId" type="text" class="ip-inputField ip-inputField-text" placeholder="Identificación">
                    <input id="inputPersonsSearchContratista" type="text" class="ip-inputField ip-inputField-text" placeholder="Nombre del Contratista">
                    <button id="btnPersonsSeatch" class="ip-btn" type="button">Buscar</button>
                </div>
                <div class="ip-tableInner">
                    <h4 class="ip-tableInner-title">Personas disponibles para agregar</h4>
                    <mx-table id="mxPersonsTable"></mx-table>
                </div>                                                                                
            </section>
            <section class="ip-container">
                <div class="ip-tableInner">
                    <h4 class="ip-tableInner-title">Participantes de la capacitación</h4>
                    <mx-table id="mxCapacitacionTable"></mx-table>
                </div>                                                                                
            </section>
        </div>

    </mx-container>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
