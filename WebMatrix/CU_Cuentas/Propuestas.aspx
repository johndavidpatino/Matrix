<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterCuentas.master"
    CodeBehind="Propuestas.aspx.vb" Inherits="WebMatrix.Propuestas" Culture="es-CO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
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
            $("#<%= txtJobBook.ClientId %>").mask("99-999999");
            $("#<%= txtJobBookInt.ClientId %>").mask("99-999999-99");
            $("#<%= txtFechaEnvio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaEnvio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaAprobacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaAprobacion.ClientId %>").datepicker({
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

            $('#PresupuestosAsignadosXPropuesta').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Presupuestos asignados",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            }); 


            $('#PresupuestosAsignadosXPropuesta').parent().appendTo("form");

            $('#DuplicarAlternativaAPropuesta').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Duplicar alternativa a otra propuesta",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            $('#DuplicarAlternativaAPropuesta').parent().appendTo("form");

            $('#DuplicarPresupuesto').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Duplicar un presupuesto dentro de la alternativa",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            $('#DuplicarPresupuesto').parent().appendTo("form");

        }

        function MostrarDuplicarAlternativaAPropuesta() {
            $('#DuplicarAlternativaAPropuesta').dialog("open");
        }

        function MostrarDuplicarPresupuesto() {
            $('#DuplicarPresupuesto').dialog("open");
        }

        function MostrarPresupuestosAsignadosXPropuesta() {
            $('#PresupuestosAsignadosXPropuesta').dialog("open");
        }

        function CerrarDuplicarAlternativaAPropuesta() {
            $('#DuplicarAlternativaAPropuesta').dialog("close");
        }

        function CerrarDuplicarPresupuesto() {
            $('#DuplicarPresupuesto').dialog("close");
        }

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Propuestas
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Registre la información general de la propuesta para poder realizar propuestos
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
            <div>
                <asp:Panel runat="server" id="accordion1">
                    <h3 style="float:left; text-align:left;">
                        <a>
                                Consulta de propuestas
                        </a>
                    </h3>
<div class="spacer"></div>
                    <div>
                                <label>
                                    Titulo</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
<div class="spacer"></div>                        
                                <label>
                                    Estados de Propuesta</label>
                                
                                    <asp:DropDownList ID="ddEstadosPropuesta" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                <asp:Button ID="btnQuitarFiltro" runat="server" Text="Quitar Filtro" />
                        <div class="spacer"></div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="No." />
                                <asp:BoundField DataField="RazonSocial" HeaderText="Cliente" />
                                <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                                <asp:BoundField DataField="Probabilidad" HeaderText="Probabilidad" />
                                <asp:BoundField DataField="FechaEnvio" HeaderText="F. Envio" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="FechaInicioCampo" HeaderText="F. Inicio Campo" DataFormatString="{0:dd/MM/yyyy}"
                                    HtmlEncode="False" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/list_16_.png" Text="Seleccionar"
                                            ToolTip="Abrir la propuesta" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detalles" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgdetalles" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Detalles" ImageUrl="~/Images/application_view_detail.png" Text="Seleccionar"
                                            ToolTip="Detalles" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Presupuestos"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/Select_16.png"
                                            Text="Seleccionar" OnClientClick="MostrarPresupuestosAsignadosXPropuesta()" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Revisión Presupuestos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgrevgross" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Envio" ImageUrl="~/Images/application_view_detail.png" Text="Enviar"
                                            ToolTip="Enviar presupuestos para revisión por parte de los gerentes de operaciones" />
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
                </asp:Panel>
                <div class="spacer"></div>
                <asp:Panel runat="server" id="accordion2" Visible="false">
                    <h3 style="float:left; text-align:left;">
                        <a>
                                Información de la Propuesta
                        </a>
                    </h3>
                    <div class="spacer"></div>
                                        <label>
                                            Título</label>
                                        <asp:HiddenField ID="hfidpropuesta" runat="server" />
                                        <asp:TextBox ID="txtTitulo" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                        <label>
                                            Estado Propuesta</label>
                                        <asp:DropDownList ID="ddlestadopropuesta" runat="server" CssClass="dropdowntext"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <label>
                                            Probabilidad Aprobación</label>
                                        <asp:DropDownList ID="ddlprobabilidadaprob" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                    <div class="spacer"></div>
                                        <label>
                                            Fecha Envío</label>
                                        <asp:TextBox ID="txtFechaEnvio" runat="server" CssClass="bgCalendar textCalendarStyle"
                                            Enabled="False"></asp:TextBox>
                                        <label>
                                            Fecha Aprobación/No Aprobación</label>
                                        <asp:TextBox ID="txtFechaAprobacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        <label>
                                            Fecha Estimada Inicio Campo</label>
                                        <asp:TextBox ID="txtFechaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                    <div class="spacer"></div>
                                        <div style="float:left">
                                        <label>
                                            Contratado por otro Ipsos</label>
                                        <asp:CheckBox ID="chkInternacional" runat="server" AutoPostBack="true" />
</div>
                    <div style="float:left">
                    <label>
                                            Tracking</label>
                                        <asp:CheckBox ID="chkTracking" runat="server" />
                        </div>
                    <div class="spacer"></div>
                                        <label>
                                            Job Book</label>
                                        <asp:TextBox ID="txtJobBook" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtJobBookInt" runat="server" Visible="false"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtJobBook" ValidChars="-">
                                        </asp:FilteredTextBoxExtender>--%>
                                        <label>
                                            Plazo de pago (días)</label>
                                        <asp:TextBox ID="txtPlazo" runat="server" Text="30" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                            TargetControlID="txtPlazo">
                                        </asp:FilteredTextBoxExtender>
                                        <label>
                                            Anticipo (%)</label>
                                        <asp:TextBox ID="txtAnticipo" Text="70" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                            TargetControlID="txtAnticipo">
                                        </asp:FilteredTextBoxExtender>
                                        <label>
                                            Saldo (%)</label>
                                        <asp:TextBox ID="txtSaldo" runat="server" Text="30" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                            TargetControlID="txtSaldo">
                                        </asp:FilteredTextBoxExtender>
                                        <label style="width:90%; text-align:left;">
                                            Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</label>
                                        <asp:TextBox ID="txtHabeasData" runat="server" TextMode="MultiLine" Width="100%" Height="30px"></asp:TextBox>
                                        <label>
                                            Razones No Aprobación</label>
                                        <asp:DropDownList ID="ddlrazonesnoaprob" runat="server" CssClass="dropdowntext" Enabled="false">
                                        </asp:DropDownList>

                                        <label>
                                            No. Brief asociado</label>
                                        <asp:TextBox ID="txtBrief" runat="server" ReadOnly="true"></asp:TextBox>
<div class="spacer"></div>
                    
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                                            <asp:Button ID="btnDisenoMuestral" runat="server" Text="Diseño Muestral" />
                                            <asp:Button ID="btnAddIquote" runat="server" Text="Agregar Presupuesto" CommandName="Guardar" />
                    <input id="Button3" type="submit" class="button" value="Cancelar" style="font-size: 11px;"
                                                onclick="location.href = 'Propuestas.aspx';" />
                                            <asp:Button ID="btnEstudio" runat="server" Text="Crear Estudio" CommandName="Estudio" />
                                            <asp:Button ID="btnDocumentos" runat="server" Text="Cargar propuesta" CommandName="Estudio" />
                </asp:Panel>
                <div class="spacer"></div>
                <asp:Panel runat="server" id="accordion3" Visible="false">
                    <asp:Panel ID="PnlDetalles" runat="server" Visible="false">
                    <h3 style="float:left; text-align:left;">
                        <a>
                                Detalles del registro
                        </a>
                    </h3>
                                        <label>
                                            Título</label>
                                        <asp:TextBox ID="txtTituloDetalle" runat="server" CssClass="textEntry" ReadOnly="True"></asp:TextBox>
                                        <label>
                                            Tipo Propuesta</label>
                                        <asp:TextBox ID="txttipopropuestaDetalle" runat="server" CssClass="textEntry" ReadOnly="True"></asp:TextBox>
                                        <label>
                                            Probabilidad Aprobación</label>
                                        <asp:TextBox ID="txtprobabilidadaprobdetalle" runat="server" CssClass="textEntry"
                                            ReadOnly="True"></asp:TextBox>
                                        <label>
                                            Estado Propuesta</label>
                                        <asp:TextBox ID="txtEstadoPropuestadetalle" runat="server" CssClass="textEntry" ReadOnly="True"></asp:TextBox>
                                        <asp:Label ID="lbltituloobservaciones" runat="server" Text=" Observaciones Anteriores:"></asp:Label>
                                        <asp:Label ID="lblobservacionesant" runat="server" Text="" Font-Size="Small" Visible="False"></asp:Label>
<div class="spacer"></div>
                    <label>
                                            Nuevas Observaciones:</label>
                                        <asp:TextBox TextMode="MultiLine" ID="txtObservaciones" Width="100%" Height="120px"
                                            runat="server" />
<div class="spacer"></div>
                    &nbsp;
                                            <asp:Button ID="btnGuardarObservacion" runat="server" Text="Guardar" CommandName="GuardarObs" />
                                            &nbsp;
                                            
                        </asp:Panel>
                </asp:Panel>
                <div class="spacer"></div>
                <asp:Panel runat="server" id="accordion4" Visible="false">
                    <h3 style="float:left; text-align:left;">
                        <a>
                                Gestión de presupuestos
                        </a>
                    </h3>
                    <div >
                            <asp:Panel ID="pnlAlternativas" Visible="false" runat="server">
                                <div>
                                    <br />
                                    <br />
                                    <h4>Alternativas asociadas a la propuesta</h4>
                                    <asp:GridView ID="gvAlternativas" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id,Alternativa" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                            <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" />
                                            <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                            <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                                            <asp:TemplateField HeaderText="Duplicar a propuesta" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgrevduplicar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Duplicar" ImageUrl="~/Images/New.Gif" Text="Enviar" OnClientClick="MostrarDuplicarAlternativaAPropuesta()"
                                                        ToolTip="Duplicar esta alternativa en otra propuesta" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ver presupuestos" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgIrEditarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="VerlPresupuestos" ImageUrl="~/Images/List_16.png" Text="Ver presupuestos asociados"
                                                        ToolTip="Ver presupuestos asociados" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlPresupuestos" Visible="false" runat="server">
                                <div>
                                    <asp:HiddenField ID="hfAlternativa" runat="server" />
                                    <br />
                                    <br />
                                    <h4>Presupuestos asignados a la alternativa</h4>
                                    <asp:GridView ID="gvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Alternativa,MetCodigo,ParNacional" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="Alternativa" HeaderText="Alt" />
                                            <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                                            <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                            <asp:BoundField DataField="ValorVenta" HeaderText="ValorVenta" DataFormatString="{0:C0}" />
                                            <asp:BoundField DataField="GM" HeaderText="GM" DataFormatString="{0:P2}" />
                                            <asp:BoundField DataField="Revisado" HeaderText="Revisado" />
                                            <asp:TemplateField HeaderText="Copiar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDuplicarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Duplicar" ImageUrl="~/Images/New.gif" Text="Copiar a otra alternativa" OnClientClick="MostrarDuplicarPresupuesto()"
                                                        ToolTip="Copiar presupuesto a otra alternativa" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="DuplicarAlternativaAPropuesta">
        <asp:UpdatePanel ID="uPanelDuplicacion" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfpD" runat="server" />
                <asp:HiddenField ID="hfaD" runat="server" />
                <asp:TextBox ID="txtNewPropuesta" placeholder="Escriba el número de propuesta destino" runat="server"></asp:TextBox>
                <asp:Button ID="btnDuplicarAlternativa" runat="server" Text="Duplicar" onclientclick="CerrarDuplicarAlternativaAPropuesta()" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="DuplicarPresupuesto">
        <asp:UpdatePanel ID="UPanelDuplicacionPresupuesto" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfPPD" runat="server" />
                <asp:HiddenField ID="hfPAD" runat="server" />
                <asp:HiddenField ID="hfPMD" runat="server" />
                <asp:HiddenField ID="hfPFD" runat="server" />
                <label>Seleccione la alternativa</label>
                <asp:DropDownList ID="ddlAlternativa" runat="server" AutoPostBack="true"></asp:DropDownList>
                <label>Seleccione la fase</label>
                <asp:DropDownList ID="ddlFase" runat="server"></asp:DropDownList>
                <asp:Button ID="btnCopiarPresupuesto" runat="server" Text="Copiar" onclientclick="CerrarDuplicarPresupuesto()" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="PresupuestosAsignadosXPropuesta">
        <asp:UpdatePanel ID="upPresupuestosAsignadosXPropuesta" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvPresupuestosAsignadosXPropuesta" runat="server" Width="100%" AutoGenerateColumns="False"
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" />
                        <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                        <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                        <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrEditarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="EditarPresupuesto" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                    ToolTip="Editar presupuestos" />
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
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXPropuesta.PageIndex = 0, "false", "true") %>'
                                            SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXPropuesta.PageIndex = 0, "false", "true") %>'
                                            SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPresupuestosAsignadosXPropuesta.PageIndex + 1%>-
                                            <%= gvPresupuestosAsignadosXPropuesta.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXPropuesta.PageIndex +1) = gvPresupuestosAsignadosXPropuesta.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXPropuesta.PageIndex +1) = gvPresupuestosAsignadosXPropuesta.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
