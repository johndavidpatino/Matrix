<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPPS_F.master"
    CodeBehind="RegistroProduccionOP.aspx.vb" Inherits="WebMatrix.RegistroProduccionOP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            function obtenerHora(Hora) {
                var hora = parseInt(Hora.substring(0, 2), 10);
                var minutos = parseInt(Hora.substring(3, 5), 10);

                var Hora = new Date(0, 0, 0, hora, minutos, 0);
                return Hora
            }


            loadPlugins();

            $('#BusquedaJBEJBICC').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "JBEJBICC",
                width: "600px"
            });

            $('#BusquedaJBEJBICC').parent().appendTo("form");

        });

        function loadPlugins() {

            function horaFinalMenorHoraInicial(HoraInicial, HoraFinal) {
                if (HoraFinal <= HoraInicial) {
                    return true
                }
                else {
                    return false
                }
            }

            $.validator.addMethod('selectNone',
          function (value, element) {
              return (this.optional(element) == true) || ((value != -1) && (value != ''));
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('validPerson',
          function (value, element) {
              return this.optional(element) == true || $('#CPH_Section_CPH_Section_CPH_ContentForm_lblNombresApellidos').text() != 'Identificación no valida';
          }, "*Identificación no valida");
            $.validator.addClassRules("mySpecificClass2", { validPerson: true });

            $.validator.addMethod('validateDateInitEnd',
         function (value, element) {
             return this.optional(element) == true || horaFinalMenorHoraInicial($('#CPH_Section_CPH_Section_CPH_ContentForm_txtHoraInicial').val(), $('#CPH_Section_CPH_Section_CPH_ContentForm_txtHoraFinal').val()) == false;
         }, "*La hora fin no puede ser menor a la hora inicial");
            $.validator.addClassRules("mySpecificClass3", { validateDateInitEnd: true });

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFecha.ClientId%>").attr('readonly', 'true').datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                maxDate: 0,
                minDate: -25
            });

            $('#<%= txtHoraInicial.ClientID%>').timeEntry({ show24Hours: true, spinnerImage: '', defaultTime: '00:00:00', showSeconds: true });
            $('#<%= txtHoraFinal.ClientID%>').timeEntry({ show24Hours: true, spinnerImage: '', defaultTime: '00:00:00', showSeconds: true });

            validationForm();
        }

        function MostrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("open");
        }

        function CerrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("close");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Registro de producción
                            </label>
                        </a>
                    </h3>
                    <div class="block">

                        <fieldset class="validationGroup">
                            <div>
                                <asp:Label ID="lblActualizar" runat="server" Text="Se esta actualizando el Id:"></asp:Label>
                                <asp:Label ID="lblIdActualizar" runat="server"></asp:Label>
                            </div>
                            <div>
                                <label>Identificación</label>
                                <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="required mySpecificClass2 text textEntry" TextMode="Number" AutoPostBack="True"></asp:TextBox>
                                <asp:Label ID="lblNombresApellidos" runat="server"></asp:Label>
                            </div>
                            <div class="form_left">
                                <label>Fecha</label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="required text textEntry" AutoPostBack="True"></asp:TextBox>
                                <label>Área</label>
                                <asp:DropDownList ID="ddlAreas" runat="server" AutoPostBack="true" CssClass="mySpecificClass"></asp:DropDownList>
                                <label>Actividad</label>
                                <asp:DropDownList ID="ddlActividad" runat="server" AutoPostBack="true" CssClass="mySpecificClass"></asp:DropDownList>
                                <label>SubActividad</label>
                                <asp:DropDownList ID="ddlSubActividad" runat="server"></asp:DropDownList>
                                <label runat="server" id="lblTipoAplicativoProceso">Aplicativo</label>
                                <asp:DropDownList ID="ddlTipoAplicativoProceso" runat="server" CssClass="mySpecificClass" Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="form_left">
                                <asp:RadioButtonList ID="rbTipoJB" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="JBE" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="JBI" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Ninguno" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <label>Estudio/Trabajo</label>
                                <asp:DropDownList ID="ddlJB" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                <asp:Button ID="btnSearchJBEJBICC" Text="..." runat="server" Width="25px" OnClientClick="MostrarJBEJBICC()" Visible="false" />
                                <br />
                                <br />
                                <label runat="server" id="lblReproceso">Reproceso</label>
                                <asp:DropDownList ID="ddlReproceso" runat="server" CssClass="mySpecificClass" AutoPostBack="true">
                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <label runat="server" id="lblTipoReproceso" visible="false">Tipo Reproceso</label>
                                <asp:DropDownList ID="ddlTipoReproceso" runat="server" CssClass="mySpecificClass" Visible="false">
                                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Area" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Operaciones" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Proyectos" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form_left">
                                <label>Hora Inicial</label>
                                <asp:TextBox ID="txtHoraInicial" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                <label>Hora Final</label>
                                <asp:TextBox ID="txtHoraFinal" runat="server" CssClass="required text textEntry mySpecificClass3"></asp:TextBox>
                                <asp:label ID="lblCantidadGeneral" runat="server">Cantidad general</asp:label>
                                <asp:TextBox ID="txtCantidadGeneral" runat="server" CssClass="required text textEntry" TextMode="Number"></asp:TextBox>
                                <asp:label ID="lblCantidadEfectivas" runat="server">Cantidad efectivas</asp:label>
                                <asp:TextBox ID="txtCantidadEfectivas" runat="server" CssClass="required text textEntry" TextMode="Number"></asp:TextBox>
                                <asp:label ID="lblCantVarsScript" runat="server">Cantidad variables Script</asp:label>
                                <asp:TextBox ID="txtCantVarsScript" runat="server" CssClass="required text textEntry" TextMode="Number"></asp:TextBox>
                                <label>Observaciones</label>
                                <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Rows="10" Columns="40"></asp:TextBox>
                            </div>
                            <div class="form_left">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="causesValidation" />
                                &nbsp;
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                            </div>
                        </fieldset>
                        <asp:Label ID="lblHorasReg" runat="server" Text="Horas registradas:"></asp:Label>
                        <asp:Label ID="lblCantHorasReg" runat="server"></asp:Label>
                    </div>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Producción registrada</label></a></h3>
                    <div class="block">
                        &nbsp;<asp:Button ID="btnVolver" runat="server" Text="Volver" />
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:g}" />
                                <asp:BoundField DataField="PersonaNombre" HeaderText="Persona" />
                                <asp:BoundField DataField="AreaNombre" HeaderText="Area" />
                                <asp:BoundField DataField="ActividadNombre" HeaderText="Actividad" />
                                <asp:BoundField DataField="SubActividadNombre" HeaderText="SubActividad" />
                                <asp:BoundField DataField="TipoNombre" HeaderText="Tipo" />
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Actividad" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="HoraInicio" HeaderText="HoraInicio" />
                                <asp:BoundField DataField="HoraFin" HeaderText="HoraFin" />
                                <asp:BoundField DataField="CantidadGeneral" HeaderText="Total" />
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
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
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaJBEJBICC">
        <asp:UpdatePanel ID="upJBEJBICC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Búsqueda" Width="176px"></asp:TextBox>
                <asp:Button ID="btnBuscarJBEJBICC" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvJBEJBICC" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="Id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarJBEJBICC()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
