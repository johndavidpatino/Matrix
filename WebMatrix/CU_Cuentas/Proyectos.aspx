<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterCuentas.master"
    CodeBehind="Proyectos.aspx.vb" Inherits="WebMatrix.Proyectos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
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
            $("#<%= txtJobBook.ClientId %>").mask("99-999999-99");
            $("#<%= txtJobBookInt.ClientId %>").mask("99-999999-99-99");
            $.validator.addMethod('selectNone2',
                function (value, element) {
                    return this.optional(element) ||
                        ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
                }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();

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
    Proyectos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    En este formulario se debe configurar el proyecto para que sea ejecutado por el Gerente de Proyectos.
    <br />
    Asegúrese de completar la sección Esquema de Análisis para contar con toda la información requerida en los entregables del proyecto.
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
            <div>
                <asp:Panel runat="server" ID="accordion0">
                    <h3>
                        <a>
                            <label>
                                Proyectos
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Titulo</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" Visible="false" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="gvProyectos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id, EstudioId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="GP_Nombres" HeaderText="GerenteProyectos" />
                                <asp:BoundField DataField="TipoProyecto" HeaderText="Tipos proyectos" />
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
                                                    Enabled='<%# IIF(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvProyectos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvProyectos.PageIndex + 1%>-<%= gvProyectos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvProyectos.PageIndex +1) = gvProyectos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvProyectos.PageIndex +1) = gvProyectos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="accordion1" Visible="false">
                    <h3 style="text-align: left;">
                        <a>Información del proyecto
                        </a>
                    </h3>
                    <div>
                        <label>
                            Estudio:
                        </label>
                        <asp:DropDownList ID="ddlEstudios" runat="server" CssClass="mySpecificClass dropdowntext" Enabled="false"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfEstudioOriginal" runat="server" />
                        <asp:HiddenField ID="hfIdProyecto" runat="server" />
                        <label>
                            Nombre
                        </label>
                        <asp:TextBox ID="txtNombreProyecto" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        <label>
                            JobBook
                        </label>
                        <asp:TextBox ID="txtJobBook" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtJobBookInt" runat="server" Visible="false"></asp:TextBox>
                        <label>
                            Tipo proyecto
                        </label>
                        <asp:DropDownList ID="ddlTiposProyectos" runat="server"
                            CssClass="mySpecificClass dropdowntext" AutoPostBack="True">
                        </asp:DropDownList>
                        <label>
                            Unidad que ejecuta
                        </label>
                        <asp:DropDownList ID="ddlUnidades" runat="server" CssClass="mySpecificClass dropdowntext">
                        </asp:DropDownList>
                    </div>
                    <div style="clear: both"></div>
                    <asp:Panel ID="pnlEsquemaAnalisis" runat="server" Visible="false">
                    <h3 style="text-align: left;">
                        <a>Esquema de Análisis
                        </a>
                    </h3>
                    <div style="width: 100%; clear: both">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <p>Cruces requeridos para el informe.</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Comparativos (años anteriores u otros informes) </p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Orden de la presentación y su contenido (Capítulos) -  Vs. Cubrimiento de objetivos</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Cómo se quiere la presentación de los datos (decimales, enteros o con o sin símbolo de porcentaje)</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA4" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Graficación sugerida (círculos, líneas, columnas, etc y por pregunta o por bloques o tipos de preguntas)</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA5" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Diseño (Definición de logos, colores, preguntas, soporte de matrices, lineamientos especiales del cliente ejemplo plantilla, etc), Complementos a los datos, por ejemplo: información secundaria, información del cliente, Orden de los datos históricos, de izquierda a derecha o de derecha a izquierda, etc </p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA6" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Formato de gráficas para presentar los análisis estadísticos</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA7" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Ponderación, entregas adicionales, traducción?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtA8" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                        </asp:Panel>
                    <div style="clear: both">
                        Presupuestos asociados al estudio
                    </div>
                    <asp:GridView ID="gvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False"
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
                            <asp:TemplateField HeaderText="Asignar" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAsignar" runat="server" OnClick='<%# "javascript:ActualizarPresupuestosAsignados("  + Eval("Id").ToString() + " ,this.checked);"   %>'
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
                    <div class="spacer"></div>
                    <asp:Button ID="btnGrabar" runat="server" Text="Guardar" CssClass="causesValidation" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                    <asp:Button ID="btnDocumentos" runat="server" Text="Brief de Cuentas a Proyectos" Visible="false" />
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="PresupuestosAsignadosXEstudio">
        <asp:UpdatePanel ID="upPresupuestosAsignadosXProyecto" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvPresupuestosAsignadosXProyecto" runat="server" Width="100%" AutoGenerateColumns="False"
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
                        <asp:CheckBoxField DataField="Aprobado" HeaderText="Aprobado" />
                        <asp:TemplateField HeaderText="Consultar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrConsultarPresupuestos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="ConsultarPresupuesto" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                    ToolTip="Consultar presupuestos" />
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
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXProyecto.PageIndex = 0, "false", "true") %>'
                                            SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPresupuestosAsignadosXProyecto.PageIndex = 0, "false", "true") %>'
                                            SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPresupuestosAsignadosXProyecto.PageIndex + 1%>-
                                            <%= gvPresupuestosAsignadosXProyecto.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXProyecto.PageIndex +1) = gvPresupuestosAsignadosXProyecto.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestosAsignadosXProyecto.PageIndex +1) = gvPresupuestosAsignadosXProyecto.PageCount, "false", "true") %>'
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
