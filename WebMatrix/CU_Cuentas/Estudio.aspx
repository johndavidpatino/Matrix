<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="Estudio.aspx.vb" Inherits="WebMatrix.EstudioForm" %>

<%@ Register Src="~/AppUsersControls/UC_LoadFiles.ascx" TagName="LoadFiles" TagPrefix="uclf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/theme.light.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= txtJobBook.ClientId %>").mask("99-999999-99");
            $("#<%= txtJobBookProyecto.ClientId %>").mask("99-999999-99");
            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
            $("#<%= txtFechaFin.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
            $("#<%= txtFechaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
        });
        function DibabledBtnSaveWhenClick() {
            let btn = document.getElementById('<%= btnSave.ClientID %>');
            btn.classList.add('Disabled')
        }
    </script>
    <style>
        .Disabled {
            pointer-events: none;
            cursor: not-allowed;
            opacity: 0.65;
            filter: alpha(opacity=65);
            -webkit-box-shadow: none;
            box-shadow: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="Default.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Buscar o crear
        </a>
    </li>
    <li>
        <a href="Frame.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-book-reader"></i>
            Brief / Frame
        </a>
    </li>
    <li>
        <a href="Propuesta.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-file-alt"></i>
            Información de la Propuesta
        </a>
    </li>
    <li>
        <a href="Presupuesto.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-calculator"></i>
            Presupuestos
        </a>
    </li>
    <li>
        <a href="Estudio.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-tag"></i>
            Estudios aprobados
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Información de los Estudios aprobados
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    Esta información es relacionada con el estudio aprobado. Es la actualización y confirmación de los datos con que fue aprobada la propuesta presentada al cliente
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <asp:HiddenField ID="hfPropuesta" runat="server" Value="0" />
    <asp:HiddenField ID="hfEstudio" runat="server" Value="0" />
    <asp:HiddenField ID="hfProyecto" runat="server" Value="0" />
    <asp:UpdatePanel ID="UPanelForMessages" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lkbModalWarning" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="ModalPopupExtenderWarning" CancelControlID="btnCloseAlert" PopupControlID="pnlMessageInfo" TargetControlID="lkbModalWarning" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlListadoEstudios" runat="server" Visible="true">
        <div class="main-card mb-3 card">
            <div class="card-body">
                <h5 class="card-title">Listado de estudios</h5>
                <asp:Button runat="server" ID="btnNew" class="btn btn-primary" Visible="false" Text="Crear Nuevo" OnClick="btnNew_Click"></asp:Button>
                <div>
                    <asp:GridView ID="gvEstudios" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0"
                        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No hay estudios creados aún">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="No. Estudio" />
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="PropuestaId" HeaderText="PropuestaId" />
                            <asp:BoundField DataField="GerenteCuentas" HeaderText="GerenteCuentas" Visible="false" />
                            <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                            <asp:BoundField DataField="FechaInicioCampo" HeaderText="F. Inicio Campo" DataFormatString="{0:dd/MM/yyyy}"
                                HtmlEncode="False" />
                            <asp:TemplateField HeaderText="" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditP" ToolTip="Actualizar"><i class="metismenu-icon fa fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlNew" runat="server" Visible="false">
        <div class="main-card mb-3 card">
            <div class="card-body">
                <div>
                    <div class="form-row">
                        <div class="input-group col-md-3 mb-3" id="ModalDatePicker" data-tooltip="El jobbook se crea en Symphony. Registre aquí el número de Job antes de agregar los presupuestos">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">JobBook</button>
                            </div>
                            <asp:TextBox ID="txtJobBook" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-3 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Valor</button>
                            </div>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3" data-tooltip="Defina la probabilidad de aprobación de esta propuesta">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Soporte</button>
                            </div>
                            <asp:DropDownList ID="ddlDocumentoSoporte" runat="server" CssClass="form-control">
                                <asp:ListItem Value="1" Text="Contrato específico"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Contrato marco local"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Contrato marco global"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Orden de compra / SOW / Declaración de trabajo"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Correo aprobación* (Autorizado Finanzas)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="input-group col-md-2 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Años retención</button>
                            </div>
                            <asp:TextBox ID="txtRetencion" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Fecha Inicio</button>
                            </div>
                            <asp:TextBox ID="txtFechaInicio" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Fecha Terminación</button>
                            </div>
                            <asp:TextBox ID="txtFechaFin" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend" data-tooltip="Escriba la fecha en que estima que iniciaría el campo. Esta información es importante para planeación">
                                <button class="btn btn-secondary">F Inicio Campo</button>
                            </div>
                            <asp:TextBox ID="txtFechaInicioCampo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Plazo de pago (días)</button>
                            </div>
                            <asp:TextBox ID="txtPlazoPago" Text="30" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Anticipo (%)</button>
                            </div>
                            <asp:TextBox ID="txtAnticipo" Text="70" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Saldo (%)</button>
                            </div>
                            <asp:TextBox ID="txtSaldo" Text="30" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-12 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Observaciones</button>
                            </div>
                            <asp:TextBox ID="txtObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <asp:Panel ID="pnlPresupuestosAsociados" runat="server" Visible="false">
                        <h5 class="card-title">Presupuesto asociado</h5>
                        <div>
                            <asp:GridView ID="gvPresupuestosAsignadosXEstudio" runat="server" Width="70%" HorizontalAlign="Center" AutoGenerateColumns="False" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0"
                                DataKeyNames="Id" AllowPaging="True" EmptyDataText="No hay presupuestos asociados">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                    <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" />
                                    <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="divider"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlPresupuestosPropuesta" runat="server" Visible="false">
                        <h5 class="card-title">Presupuestos disponibles</h5>
                        <div>
                            <asp:GridView ID="gvPresupuestos" runat="server" Width="70%" HorizontalAlign="Center" AutoGenerateColumns="False" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0"
                                DataKeyNames="Id" EmptyDataText="No hay presupuestos aprobados">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                    <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" />
                                    <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                    <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                                    <asp:TemplateField HeaderText="Asignar">
                                        <ItemTemplate>
                                            <asp:RadioButton GroupName="groupPresupuestos" ID="chkAsignar" runat="server" AutoPostBack="true" OnCheckedChanged="chkAsignar_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="divider"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCambiosPresupuestos" runat="server" Visible="false">
                        <div class="divider"></div>
                        <h5 class="card-title">Corrección de alternativa aprobada</h5>
                        <div class="form-inline">
                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                <label>Seleccione la nueva alternativa a asignar a este estudio. </label>
                                <asp:DropDownList ID="ddlAlternativas" runat="server"></asp:DropDownList>
                            </div>
                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                <label>Se actualizará toda la información y se volverá a enviar el anuncio de aprobación</label>
                                <div class="spacer"></div>
                                <asp:Button ID="btnActualizarCambio" CssClass="btn btn-secondary custom-control-inline" runat="server" Text="Confirmar" OnClick="btnActualizarCambio_Click" OnClientClick="return confirm('Está seguro de realizar el cambio?')" />
                                <asp:Button ID="btnCancelarCambio" CssClass="btn btn-secondary custom-control-inline" runat="server" Text="Cancelar" OnClick="btnCancelarCambio_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewProyects" runat="server" Visible="false">
                        <div class="form-inline">
                            <div class="position-relative form-check form-check-inline">
                                <label class="form-check-label">
                                    <asp:CheckBox ID="chbProjectCuanti" Enabled="false" runat="server" />
                                    Crear Proyecto Cuantitativo</label>
                            </div>
                            <div class="position-relative form-check form-check-inline">
                                <label class="form-check-label">
                                    <asp:CheckBox ID="chbProjectCuali" runat="server" Enabled="false" />
                                    Crear Proyecto Cualitativo</label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlListadoProyectos" runat="server" Visible="false">
                        <h5 class="card-title">Listado de Proyectos</h5>
                        <asp:Button runat="server" ID="btnNewProject" class="btn btn-primary" Visible="true" Text="Nuevo Proyecto" OnClick="btnNewProject_Click"></asp:Button>
                        <asp:GridView ID="gvProyectos" runat="server" Width="100%" AutoGenerateColumns="False" OnRowCommand="gvProyectos_RowCommand"
                            DataKeyNames="Id, EstudioId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <Columns>
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="GP_Nombres" HeaderText="Gerente Proyecto" />
                                <asp:BoundField DataField="TipoProyecto" HeaderText="Tipo proyecto" />
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEdit" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditP" ToolTip="Editar"><i class="metismenu-icon fa fa-edit"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="pnlDatosProyectos" runat="server" Visible="false">
                        <div class="divider"></div>
                        <div class="form-row">
                            <div class="input-group col-md-3 mb-2">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">JobBook Proyecto</button>
                                </div>
                                <asp:TextBox ID="txtJobBookProyecto" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-4 mb-2">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Nombre Proyecto</button>
                                </div>
                                <asp:TextBox ID="txtNombreProyecto" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-2">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Tipo</button>
                                </div>
                                <asp:DropDownList ID="ddlTipoProyecto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoProyecto_SelectedIndexChanged" CssClass="form-control">
                                    <asp:ListItem Value="1" Text="Cuanti"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Cuali"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-3 mb-2">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Unidad Ejecuta</button>
                                </div>
                                <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </asp:Panel>
                    <asp:Panel ID="pnlEsquemaAnalisis" runat="server" Visible="false">
                        <div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Cruces requeridos para el informe</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA1" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Comparativos (años anteriores u otros informes)</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA2" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Orden de la presentación y su contenido (Capítulos) -  Vs. Cubrimiento de objetivos</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA3" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Cómo se quiere la presentación de los datos (decimales, enteros o con o sin símbolo de porcentaje)</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA4" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Graficación sugerida (círculos, líneas, columnas, etc y por pregunta o por bloques o tipos de preguntas)</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA5" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Diseño (Definición de logos, colores, preguntas, soporte de matrices, lineamientos especiales del cliente ejemplo plantilla, etc), Complementos a los datos, por ejemplo: información secundaria, información del cliente, Orden de los datos históricos, de izquierda a derecha o de derecha a izquierda, etc</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA6" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">Formato de gráficas para presentar los análisis estadísticos</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA7" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                            <div class="position-relative row form-group">
                                <label for="exampleEmail" class="col-sm-3 col-form-label">¿Ponderación, entregas adicionales, traducción?</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtA8" runat="server" TextMode="MultiLine" CssClass="form-control form-control-min"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <asp:Button runat="server" ID="btnSaveProyecto" class="btn btn-primary" Visible="false" Text="Guardar Proyecto" OnClick="btnSaveProyecto_Click"></asp:Button>
                        <asp:Button runat="server" ID="btnCancelProject" class="btn btn-primary" Visible="false" Text="Cancelar" OnClick="btnCancelProject_Click"></asp:Button>

                        <div class="divider"></div>
                    </asp:Panel>
                </div>

                <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Visible="true" Text="Guardar" OnClick="btnSave_Click" OnClientClick="DibabledBtnSaveWhenClick()"></asp:Button>
                <asp:Button runat="server" ID="btnCancel" class="btn btn-primary" Visible="true" Text="Cancelar" OnClick="btnCancel_Click"></asp:Button>
                <asp:Button runat="server" ID="btnChangeAlternativa" class="btn btn-primary" Visible="true" Text="Cambiar alternativa" OnClick="btnChangeAlternativa_Click"></asp:Button>
                <asp:Button runat="server" ID="btnViewEsquemaAnalisis" class="btn btn-primary" Visible="false" Text="Ver Esquema Análisis" OnClick="btnViewEsquemaAnalisis_Click"></asp:Button>
            </div>
        </div>
    </asp:Panel>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <asp:Button runat="server" ID="btnLoadFiles" CssClass="btn btn-secondary" Visible="false" Text="Ver / Cargar Archivos" OnClick="LoadFiles_Click" />
            <asp:Panel ID="pnlLoadFiles" runat="server" Visible="false">
                <div>
                    <uclf:LoadFiles ID="UCFiles" runat="server" />
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:Panel ID="pnlMessageInfo" runat="server">
        <asp:UpdatePanel ID="UPanelMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="bd-example-modal-sm" tabindex="-2" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-sm show">
                        <div class="modal-content">
                            <div class="modal-header">
                                <p class="modal-title" id="exampleModalLabel">
                                    <asp:Label ID="lblTitleWarning" runat="server"></asp:Label>
                                </p>
                                <asp:Button ID="btnCloseAlert" runat="server" class="icon" data-dismiss="modal" aria-label="Close" Text="x"></asp:Button>
                            </div>
                            <div class="modal-body">
                                <div class="main-card mb-3 card">
                                    <div class="card-body">
                                        <asp:Panel ID="pnlMsgTextWarning" runat="server" Visible="false" class="alert alert-warning fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextWarning" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextError" runat="server" Visible="false" class="alert alert-danger fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextError" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextInfo" runat="server" Visible="false" class="alert alert-info fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextInfo" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
