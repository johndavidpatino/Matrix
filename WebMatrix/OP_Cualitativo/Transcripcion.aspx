<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_F.master"
    CodeBehind="Transcripcion.aspx.vb" Inherits="WebMatrix.Transcripcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {

            });


            $("#<%= txtFechaEntrega.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaEntrega.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaRequerida.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaRequerida.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTranscripcion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTranscripcion.ClientId %>").datepicker({
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
            var idtrabajo = $('#<%= hfidtrabajo.ClientID %>').val();
            document.location.href = 'Transcripcion.aspx?idtrabajo=' + idtrabajo;

        }
    </script>
    <style type="text/css">
        .fueraTiempo {
            background-color: red;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>TRANSCRIPCIONES</a>
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
                                Lista de Transcripciones</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Responsable o Transcriptor</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="causesValidation buttonText buttonSave corner-all" />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="causesValidation buttonText buttonSave corner-all" />
                                <asp:HiddenField ID="hfidtrabajo" runat="server" />
                            </fieldset>
                        </div>
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" />
                                <asp:BoundField DataField="JobBook" HeaderText="Job Book" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="NombreResponsable" HeaderText="Responsable" />
                                <asp:BoundField DataField="NombreTranscriptor" HeaderText="Transcriptor" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="FechaRequerida" HeaderText="Fecha Requerida" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="FechaTranscripcion" HeaderText="Fecha Real Transcripción" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
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
                                Información de Transcripción</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Responsable</label>
                                        <asp:HiddenField ID="hfTranscripcionId" runat="server" />
                                        <asp:DropDownList ID="ddlresponsable" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Fecha Entrega:</label>
                                        <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Transcriptor</label>
                                        <asp:DropDownList ID="ddltranscriptor" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Fecha Requerida:</label>
                                        <asp:TextBox ID="txtFechaRequerida" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Cantidad</label>
                                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Fecha Transcripción:</label>
                                        <asp:TextBox ID="txtFechaTranscripcion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" CssClass="causesValidation buttonText buttonSave corner-all" />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar"
                                                style="font-size: 11px;" onclick="redireccion();" />
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles del registro</label>
                        </a>
                    </h3>
                    <div class="block">
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
