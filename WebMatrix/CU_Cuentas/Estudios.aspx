<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterCuentas.master"
    CodeBehind="Estudios.aspx.vb" Inherits="WebMatrix.Estudios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });


            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });
            $("#<%= txtJobBook.ClientId %>").mask("99-999999-99");
            $("#<%= txtJobBookInt.ClientId %>").mask("99-999999-99-99");
            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
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

            $("#<%= txtFechaTerminacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTerminacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            validationForm();

            //            $('#<%= btnGrabar.ClientID %>').click(function (evt) {
            //                if ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() == "") {
            //                    validarSelect('<%= btnGrabar.ClientID %>', "Debe seleccionar por lo menos un presupuesto");
            //                }
            //            });

            $('#PresupuestosAsignadosXEstudio').parent().appendTo("form");

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#PresupuestosAsignadosXEstudio').dialog(
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

            loadPlugins();
        });
        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function CheckOtherIsCheckedByGVID(spanChk) {

            var IsChecked = spanChk.checked;
            if (IsChecked) {
            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=gvPresupuestos.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Estudios
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Aquí puede consultar los anuncios de aprobación realizados, así como la creación de nuevos proyectos
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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel runat="server" id="accordion0">
                    <h3 style="text-align:left;">
                        <a>
                                Consultar Estudios
                        </a>
                    </h3>
                                <label>
                                    Titulo</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" Visible="false" />
                        <asp:UpdatePanel ID="upEstudios" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvEstudios" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="No. Estudio" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                        <asp:BoundField DataField="PropuestaId" HeaderText="PropuestaId" />
                                        <asp:BoundField DataField="GerenteCuentas" HeaderText="GerenteCuentas" Visible="false" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                        <asp:BoundField DataField="FechaInicioCampo" HeaderText="F. Inicio Campo" DataFormatString="{0:dd/MM/yyyy}"
                                            HtmlEncode="False" />
                                        <asp:TemplateField HeaderText="Trabajos" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgTrabajos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Trabajos" ImageUrl="~/Images/list_16.png" Text="Seleccionar"
                                                    ToolTip="Trabajos" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Presupuestos" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="PresupuestosAsignados" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                    ToolTip="Presupuestos asignados" OnClientClick="MostrarPresupuestosAsignadosXEstudio()" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Actualizar" ImageUrl="~/Images/list_16_.png" Text="Actualizar"
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
                                                            Enabled='<%# IIF(gvEstudios.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                            Enabled='<%# IIF(gvEstudios.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <span class="pagingLinks">[<%= gvEstudios.PageIndex + 1%>-
                                                            <%= gvEstudios.PageCount%>]</span>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                            Enabled='<%# IIF((gvEstudios.PageIndex +1) = gvEstudios.PageCount, "false", "true") %>'
                                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                            Enabled='<%# IIF((gvEstudios.PageIndex +1) = gvEstudios.PageCount, "false", "true") %>'
                                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel runat="server" id="accordion1" Visible="false">
                    <h3 style="text-align:left;">
                        <a>
                                Información del estudio
                        </a>
                    </h3>
                    <div>
                                <asp:Panel ID="pnlEstudio" runat="server" Visible="true">
                                    <asp:UpdatePanel ID="upDetalleEstudios" runat="server" ChildrenAsTriggers="false"
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hfIdEstudio" runat="server" />
                                                    <label>
                                                        No. Propuesta
                                                    </label>
                                                    <asp:TextBox ID="txtPropuesta" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="hfPropuestaOriginal" runat="server" />
                                                </fieldset>
                                                    <label>
                                                        Nombre estudio:
                                                    </label>
                                                    <asp:TextBox ID="txtNombreEstudio" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                                    <label>
                                                        JobBook
                                                    </label>
                                                    <asp:TextBox ID="txtJobBook" runat="server"></asp:TextBox>
                                                    <asp:TextBox ID="txtJobBookInt" runat="server" Visible="false"></asp:TextBox>
                                                    <label>
                                                        Valor
                                                    </label>
                                                    <asp:TextBox ID="txtValor" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fteTxtValor" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtValor">
                                                    </asp:FilteredTextBoxExtender>
                                                </fieldset>
                                                    <label>
                                                        Observaciones
                                                    </label>
                                                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                                    <label>
                                                        Fecha Inicio:
                                                    </label>
                                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                                    <label>
                                                        Fecha Terminación:
                                                    </label>
                                                    <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
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
                                                    <label>
                                                        Fecha Estimada Inicio Campo</label>
                                                    <asp:TextBox ID="txtFechaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                            <label>
                                            <asp:Label runat="server" Text="Documento soporte" ToolTip="Debe seleccionar cuál es el documento que se manejará con el cliente para legalizar el estudio"></asp:Label></label>
                                        <asp:DropDownList ID="ddlDocumentoSoporte" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                            <label>
                                            <asp:Label runat="server" Text="Tiempo retención (años)" ToolTip="El tiempo de retención es máximo un año para estudios adhoc y hasta 3 años para tipo tracking. Para casos diferentes consulte primero con Legal y IT"></asp:Label></label>
                                            <asp:TextBox ID="txtTiempoRetencion" Text="1" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtTiempoRetencion">
                                                    </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div style="clear: both">
                                        Presupuestos asociados a la propuesta
                                    </div>
                                    <asp:UpdatePanel ID="upPresupuestos" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False" Enabled="false"
                                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                                DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                                <SelectedRowStyle CssClass="SelectedRow" />
                                                <AlternatingRowStyle CssClass="odd" />
                                                <Columns>
                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                                    <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" />
                                                    <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                                    <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                                                    <asp:TemplateField HeaderText="JobBook" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtJobBook" runat="server" Text='<%#Eval("JobBook") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Custom, Numbers"
                                                                TargetControlID="txtJobBook" ValidChars="-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asignado">
                                                        <ItemTemplate>
                                                            <asp:RadioButton GroupName="groupPresupuestos" ID="chkAsignar" runat="server" AutoPostBack="true" OnCheckedChanged="chkAsignar_CheckedChanged"
                                                                Checked='<%#Eval("Asignado") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <div class="pagingButtons">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                                        Enabled='<%# IIF(gvPresupuestos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                                        Enabled='<%# IIF(gvPresupuestos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <span class="pagingLinks">[<%= gvPresupuestos.PageIndex + 1%>-
                                                                    <%= gvPresupuestos.PageCount%>]</span>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                                        Enabled='<%# IIf((gvPresupuestos.PageIndex + 1) = gvPresupuestos.PageCount, "false", "true") %>'
                                                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                                        Enabled='<%# IIf((gvPresupuestos.PageIndex + 1) = gvPresupuestos.PageCount, "false", "true") %>'
                                                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </PagerTemplate>
                                            </asp:GridView>
                                            <asp:HiddenField ID="hfIndicesFilasPresupuestosAsignados" runat="server" />
                                            <asp:HiddenField ID="hfIndicesFilasPresupuestosOriginales" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
<div class="spacer"></div>
                                    <asp:Button ID="btnGrabar" runat="server" Text="Guardar" CssClass="mySpecificClass2 causesValidation" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                                <asp:Button ID="btnCrearEstudio" runat="server" Text="Crear Proyecto" />
                                                <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Correo Aprobacion" />
                                                <asp:Button ID="btnCorreccionEstudio" runat="server" Text="Corregir Alternativa Aprobada" OnClientClick="return confirm('Debe cambiar primero el valor del estudio. Haga clic en Aceptar si ya lo hizo o en Cancelar para hacerlo antes de continuar.')" />
                                    
                                </asp:Panel>
                                <asp:Panel ID="pnlCambiosPresupuestos" runat="server" Visible="false">
                                    Seleccione la nueva alternativa a asignar a este estudio. 
                                <asp:DropDownList ID="ddlAlternativas" runat="server"></asp:DropDownList><br />
                                    Se actualizará toda la información y se volverá a enviar el anuncio de aprobación.
<div class="spacer"></div>
                                    <asp:Button ID="btnActualizarCambio" runat="server" Text="Confirmar" OnClientClick="return confirm('Está seguro de realizar el cambio?')" />
                                            <asp:Button ID="btnCancelarCambio" runat="server" Text="Cancelar" />
                                </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="PresupuestosAsignadosXEstudio">
        <asp:UpdatePanel ID="upPresupuestosAsignadosXEstudio" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvPresupuestosAsignadosXEstudio" runat="server" Width="100%" AutoGenerateColumns="False"
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
                        <asp:TemplateField HeaderText="Consultar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrConsultarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="ConsultarPresupuesto" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
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
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXEstudio.PageIndex = 0, "false", "true") %>'
                                            SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXEstudio.PageIndex = 0, "false", "true") %>'
                                            SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPresupuestosAsignadosXEstudio.PageIndex + 1%>-
                                            <%= gvPresupuestosAsignadosXEstudio.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXEstudio.PageIndex +1) = gvPresupuestosAsignadosXEstudio.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXEstudio.PageIndex +1) = gvPresupuestosAsignadosXEstudio.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                Por favor espere un momento
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
