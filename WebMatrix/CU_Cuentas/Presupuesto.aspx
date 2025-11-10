<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="Presupuesto.aspx.vb" Inherits="WebMatrix.PresupuestosForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/AppUsersControls/UC_Header_Presupuesto.ascx" TagName="HeaderPresupuesto" TagPrefix="uch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function TotalDias() {

            document.getElementById('<%= txtDiasTotal.ClientID %>').value = Number(document.getElementById('<%= txtDiasDiseno.ClientID %>').value) + Number(document.getElementById('<%= txtDiasCampo.ClientID %>').value) + Number(document.getElementById('<%= txtDiasProceso.ClientID %>').value) + Number(document.getElementById('<%= txtDiasInformes.ClientID %>').value);

        }
        function HideModalPopup() {
            $find("ModalPopupExtenderGM").hide();
        }
    </script>
    <style type="text/css">
        /* ajax__tab_lightblue-theme theme (images/lightblue.jpg) */
        .ajax__tab_lightblue-theme .ajax__tab_header {
            font-family: arial,helvetica,clean,sans-serif;
            font-size: small;
            border-bottom: solid 5px #c2e0fd;
        }

            .ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_outer {
                background: url(images/lightblue.jpg) #d8d8d8 repeat-x;
                margin: 0px 0.16em 0px 0px;
                padding: 1px 0px 1px 0px;
                vertical-align: bottom;
                border: solid 1px #a3a3a3;
                border-bottom-width: 0px;
            }

            .ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_tab {
                color: #000;
                padding: 0.35em 0.75em;
                margin-right: 0.01em;
            }

        .ajax__tab_lightblue-theme .ajax__tab_hover .ajax__tab_outer {
            background: url(../images/lightblue.jpg) #bfdaff repeat-x left -1300px;
        }

        .ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_tab {
            color: #000;
        }

        .ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_outer {
            background: url(../images/lightblue.jpg) #ffffff repeat-x left -1400px;
        }

        .ajax__tab_lightblue-theme .ajax__tab_body {
            padding: 0.25em 0.5em;
            background-color: #ffffff;
            border: solid 1px #808080;
            border-top-width: 0px;
        }
    </style>
    <style type="text/css">
        .chosen-container {
            display: block !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <asp:Panel ID="pnlSideBarCuentas" runat="server">
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
    </asp:Panel>
    <asp:Panel ID="pnlSidebarOPS" runat="server" Visible="false">
        <li class="app-sidebar__heading">Opciones</li>
        <li>
            <a href="RevisionPresupuestos.aspx" class="nav-link">
                <i class="metismenu-icon fa fa-list"></i>
                Volver a la lista de revisión
            </a>
        </li>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Gestión de Presupuestos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    <asp:HiddenField ID="hfPropuesta" runat="server" />
    <asp:HiddenField ID="hfOPS" runat="server" Value="0" />
    <asp:HiddenField ID="hfNewAlternativa" runat="server" Value="true" />
    Cree presupuestos, duplique e importe presupuestos desde este formulario
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Actions" runat="server">
    <div id="divActions" class="btn-shadow mr-3 btn btn-dark">
        <asp:Panel ID="pnlAlternativa" runat="server" Visible="false">
            Alternativa
        <asp:DropDownList ID="ddlAlternativa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlternativa_SelectedIndexChanged"></asp:DropDownList>
            de
        <asp:Label ID="lblNumAlternativas" runat="server" Text="1"></asp:Label>
        </asp:Panel>
        <asp:Button ID="btnNewAlternativa" runat="server" CssClass="btn btn-primary" Visible="true" Text="Nueva alternativa" OnClick="btnNewAlternativa_Click" />
        <asp:Button ID="btnImportar" runat="server" CssClass="btn btn-primary" Visible="true" Text="Importar" OnClick="btnImportar_Click" />
        <asp:Button ID="btnDuplicarAlternativa" runat="server" CssClass="btn btn-primary" Visible="false" Text="Duplicar" OnClick="btnDuplicarAlternativa_Click" />
        <asp:Button ID="btnRevision" runat="server" CssClass="btn btn-primary" Visible="false" Text="Enviar a revisión" OnClick="btnRevision_Click" />
        <asp:Button ID="btnExportIQuote" runat="server" CssClass="btn btn-primary" Visible="false" Text="Enviar a IQuote" OnClick="btnExportIQuote_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <asp:Panel ID="pnlGeneral" runat="server" Visible="false">
                <asp:UpdatePanel ID="UPanelGeneral" runat="server">
                    <ContentTemplate>

                        <div class="form-row">
                            <div class="input-group col-md-5 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Descripción</button>
                                </div>
                                <asp:TextBox ID="txtDescripcionAlternativa" ClientIDMode="Static" CssClass="form-control" runat="server" MaxLength="300"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">No. mediciones</button>
                                </div>
                                <asp:TextBox ID="txtNoMediciones" CssClass="form-control" TextMode="Number" runat="server" Text="1"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Periodicidad meses</button>
                                </div>
                                <asp:TextBox ID="txtPeriodicidad" CssClass="form-control" TextMode="Number" runat="server" Text="1"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-1 mb-3">
                                <asp:CheckBox ID="chbObserver" Text="Observer" runat="server" CssClass="form-check-inline" />
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary"># IQuote</button>
                                </div>
                                <asp:Label ID="lblNumIQuote" CssClass="form-control" runat="server"></asp:Label>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Días hábiles estimados</span>
                                </div>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Diseño</button>
                                </div>
                                <asp:TextBox ID="txtDiasDiseno" CssClass="form-control" TextMode="Number" runat="server" Text="1" onchange="Javascript:TotalDias();"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Campo</button>
                                </div>
                                <asp:TextBox ID="txtDiasCampo" CssClass="form-control" TextMode="Number" runat="server" Text="1" onchange="Javascript:TotalDias();"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3" data-tooltip="Esta es la fecha en que se aprobó o se canceló la propuesta">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Proceso</button>
                                </div>
                                <asp:TextBox ID="txtDiasProceso" CssClass="form-control" TextMode="Number" runat="server" Text="1" onchange="Javascript:TotalDias();"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Informes</button>
                                </div>
                                <asp:TextBox ID="txtDiasInformes" CssClass="form-control" TextMode="Number" runat="server" Text="1" onchange="Javascript:TotalDias();"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-2 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Total</button>
                                </div>
                                <asp:TextBox ID="txtDiasTotal" CssClass="form-control" ReadOnly="true" TextMode="Number" Text="4" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-12 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Observaciones</button>
                                </div>
                                <asp:TextBox ID="txtObservacionesGeneral" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:Button runat="server" ID="btnSaveGeneral" class="btn btn-primary" Text="Guardar" OnClick="btnSaveGeneral_Click"></asp:Button>
                <asp:Button runat="server" ID="btnCancelGeneral" class="btn btn-primary" Text="Cancelar" OnClick="btnCancelGeneral_Click"></asp:Button>
            </asp:Panel>
        </div>
        <asp:UpdatePanel ID="UPanelForMessages" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lkbModalWarning" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="ModalPopupExtenderWarning" CancelControlID="btnCloseAlert" PopupControlID="pnlMessageInfo" TargetControlID="lkbModalWarning" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                </asp:ModalPopupExtender>
                <asp:LinkButton ID="lkbModalImport" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="ModalPopupExtenderImport" CancelControlID="btnCloseImport" PopupControlID="pnlImport" TargetControlID="lkbModalImport" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                </asp:ModalPopupExtender>
                <asp:LinkButton ID="lbkModalExportIQuote" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="ModalPopupExtenderExpiQuote" CancelControlID="btnCancelExportiQuote" PopupControlID="pnlExportToiQuote" TargetControlID="lbkModalExportIQuote" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:Panel ID="pnlPresupuestos" runat="server" Visible="true">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-header">
                        <asp:Label ID="lblShowPopup" runat="server"></asp:Label>
                        <asp:Label ID="lblShowGM" runat="server"></asp:Label>
                        <asp:Label ID="lblShowJBI" runat="server"></asp:Label>
                        <asp:Label ID="lblShowJBE" runat="server"></asp:Label>
                        <asp:Label ID="lblShowCopiarPresupuesto" runat="server"></asp:Label>
                        <asp:Label ID="lblShowExecution" runat="server"></asp:Label>
                        <asp:Label ID="lblShowExcelMuestra" runat="server"></asp:Label>
                        <asp:Label ID="lblShowSimulator" runat="server"></asp:Label>
                        <asp:Label ID="lblShowGMSimulator" runat="server"></asp:Label>
                        <asp:Button ID="btnAddPresupuestos" runat="server" class="btn btn-info" Text="Agregar presupuestos" Visible="false" OnClick="btnAddPresupuestos_Click"></asp:Button>

                        <asp:ModalPopupExtender ID="lkb1_ModalPopupExtender" runat="server"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancel"
                            DropShadow="True" Enabled="True"
                            PopupControlID="pnlPresupuestoModal" TargetControlID="lblShowPopup">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderGM" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelGM"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelGM"
                            PopupControlID="pnlGM" TargetControlID="lblShowGM">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderSimulator" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelSimulator"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelSimulator"
                            PopupControlID="pnlGMSimulator" TargetControlID="lblShowGMSimulator">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderJBI" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelJBI"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelJBI"
                            PopupControlID="pnlJBI" TargetControlID="lblShowJBI">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderJBE" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelJBE"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelJBE"
                            PopupControlID="pnlJBE" TargetControlID="lblShowJBE">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderCopiarPresupuesto" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelCopiar"
                            DropShadow="True" Enabled="True"
                            PopupControlID="pnlCopiarPresupuesto" TargetControlID="lblShowCopiarPresupuesto">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderExecution" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelExecution"
                            DropShadow="True" Enabled="True" OkControlID="btnCancelExecution"
                            PopupControlID="pnlExecution" TargetControlID="lblShowExecution">
                        </asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderExcelMuestra" runat="server" ClientIDMode="Static"
                            BackgroundCssClass="modal-backdrop fade show" CancelControlID="btnCancelExcelMuestra"
                            DropShadow="True" Enabled="True"
                            PopupControlID="pnlExcelMuestra" TargetControlID="lblShowExcelMuestra">
                        </asp:ModalPopupExtender>
                    </div>
                    <div class="tab-content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="main-card mb-3 card">
                                    <div class="card-body">
                                        <div>
                                            <asp:GridView ID="gvPresupuestos" runat="server" AutoGenerateColumns="false" DataKeyNames="NACIONAL,MetCodigo,Revisado,IdTecnica" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" OnRowCommand="gvPresupuestos_RowCommand" OnRowDataBound="gvPresupuestos_RowDataBound" EmptyDataText="No hay presupuestos F2F creados" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbReview" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ReviewP" ToolTip="Marcar como revisado este presupuesto" Visible='<%# IIF(CBool(Eval("Revisado")) = True, False, True) %>' OnClientClick="return confirm('¿Está seguro marcar como revisado este presupuesto?')"><i class="metismenu-icon fa fa-check-square"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lbUndoReview" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="UndoReviewP" ToolTip="Desmarcar la revisión de este presupuesto" Visible='<%# IIF(CBool(Eval("Revisado")) = True, True, False) %>' OnClientClick="return confirm('¿Está seguro de quitar la marca de revisión este presupuesto?')"><i class="metismenu-icon fa fa-undo"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbEdit" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditP" ToolTip="Editar"><i class="metismenu-icon fa fa-edit"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbCopy" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="CopyP" ToolTip="Copiar este presupuesto dentro de esta alternativa u otra alternativa"><i class="metismenu-icon fa fa-copy"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteP" OnClientClick="return confirm('¿Está seguro de borrar esta fase?')" ToolTip="Borrar"><i class="metismenu-icon fa fa-trash"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDetails" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DetailsP" ToolTip="Ajustes de venta y gross margin"><i class="metismenu-icon fa fa-money-check-alt"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbSimulator" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="SimulatorP" ToolTip="Simular"><i class="metismenu-icon fa fa-calculator"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbExec" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ExecP" ToolTip="Ver ejecución"><i class="metismenu-icon fa fa-info"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False" Visible="True">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbProfessionalTimeCalc" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="CalcProfessionalTimeP" OnClientClick="return confirm('Esta opción calculará automáticamente las horas profesionales para este presupuesto reemplazando las horas introducidas previamente en caso de haberlas asignado. ¿Desea continuar?')" ToolTip="Calcular horas profesionales para este presupuesto"><i class="metismenu-icon fa fa-clock"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbJBE" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="JBEP" ToolTip="Ver JBE">JBE</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbJBI" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="JBIP" ToolTip="JBI">JBI</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:ImageField DataImageUrlField="IdTecnica" DataImageUrlFormatString="~/Images/{0}.png" ReadOnly="true"></asp:ImageField>
                                                    <asp:BoundField DataField="MetNombre" HeaderText="Metodología" />
                                                    <asp:BoundField DataField="FASE" HeaderText="Fase" />
                                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                                    <asp:BoundField DataField="CostoDirecto" DataFormatString="{0:C0}" HeaderText="Venta OPS" HeaderStyle-Wrap="true" />
                                                    <asp:BoundField DataField="ValorVenta" DataFormatString="{0:C0}" HeaderText="Venta Total" HeaderStyle-Wrap="true" />
                                                    <asp:BoundField DataField="ActSubGasto" DataFormatString="{0:C0}" HeaderText="Sub Ext" HeaderStyle-Wrap="true" />
                                                    <asp:BoundField DataField="GrossMargin" DataFormatString="{0:F2}" HeaderText="%GM" />
                                                    <asp:BoundField DataField="OP" DataFormatString="{0:F2}" HeaderText="%OP" />
                                                    <asp:CheckBoxField DataField="Revisado" HeaderText="Revisado" />
                                                    <asp:CheckBoxField DataField="Aprobado" HeaderText="Aprobado" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <asp:Button ID="btnCalcProfessionalTime" runat="server" CssClass="btn btn-primary" OnClientClick="return confirm('Esta opción calculará automáticamente las horas profesionales para TODA LA ALTERNATIVA reemplazando las horas introducidas previamente en caso de haberlas asignado. ¿Desea continuar?')" Text="Recalcular las horas profesionales para esta alternativa" OnClick="btnCalcProfessionalTime_Click" />
                                        <asp:Button ID="btnImportarMuestraExcel" runat="server" CssClass="btn btn-primary" Text="Importar muestra desde Excel" Visible="false" OnClick="btnImportarMuestraExcel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlPresupuestoModal" runat="server">
                        <asp:UpdatePanel ID="UpanelPresupuesto" runat="server">
                            <ContentTemplate>
                                <div class="bd-example-modal-lg show" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">General</h5>
                                                        <div class="form-row">
                                                            <div class="col-md-3">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlTecnica" class="">Técnica</label><asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTecnica" OnSelectedIndexChanged="ddlTecnica_SelectedIndexChanged" class="form-control-min-select form-control">
                                                                        <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                                                        <asp:ListItem Value="100" Text="Cara a Cara"></asp:ListItem>
                                                                        <asp:ListItem Value="200" Text="CATI"></asp:ListItem>
                                                                        <asp:ListItem Value="300" Text="Online"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlMetodologia" class="">Metodología</label><asp:DropDownList runat="server" AutoPostBack="true" ID="ddlMetodologia" OnSelectedIndexChanged="ddlMetodologia_SelectedIndexChanged" class="form-control-min-select form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlFase" class="">Fase</label><asp:DropDownList runat="server" ID="ddlFase" class="form-control-min-select form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlComplejidad" class="">Complejidad</label><asp:DropDownList runat="server" ID="ddlComplejidad" class="form-control-min-select form-control">
                                                                        <asp:ListItem Value="1" Text="1 (Demasiado fácil)"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                        <asp:ListItem Value="5" Text="5 (Estándar)" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                                        <asp:ListItem Value="10" Text="10 (Muy complejo)"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="position-relative form-group">
                                                                    <label for="txtDuracion" class="">Duración</label><asp:TextBox runat="server" ID="txtDuracionMinutos" TextMode="Number" ToolTip="Duración en minutos" class="form-control-min form-control" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <uch:HeaderPresupuesto ID="UCHeader" runat="server" />
                                                        <div class="clearfix"></div>
                                                        <h5 class="card-title">Agregar Muestra</h5>
                                                        <div class="form-row">
                                                            <div class="col-md-4">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlDificultadMuestra" class="">Dificultad / Tipo</label><asp:DropDownList ID="ddlDificultadMuestra" runat="server" CssClass="form-control-min-select form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="position-relative form-group">
                                                                    <label for="ddlCiudad" class="">Ciudad</label><asp:DropDownListChosen runat="server" ID="ddlCiudad" DataPlaceHolder="Seleccione" CssClass="form-control-min-select form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="position-relative form-group">
                                                                    <label for="txtCantidad" class="">Cantidad</label><asp:TextBox runat="server" ID="txtCantidadMuestra" TextMode="Number" ToolTip="" class="form-control-min form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="position-relative form-group">
                                                                    <label for="btnAddMuestra" class="">&nbsp;</label><asp:Button ID="btnAddMuestra" runat="server" Text="Agregar" class="form-control-min form-control mb-2 mr-2 btn btn-info" OnClick="btnAddMuestra_Click"></asp:Button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="position-relative form-group">
                                                                <label>
                                                                    Total Muestra:
                                                            <asp:Label ID="lblTotalMuestra" runat="server"></asp:Label></label>
                                                            </div>
                                                        </div>
                                                        <div>
                                                            <asp:GridView ID="gvMuestraF2F" Visible="false" runat="server" AutoGenerateColumns="false" DataKeyNames="Codigo" CssClass="mb-0 table table-hover" OnRowCommand="gvMuestraF2F_RowCommand" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay muestra agregada">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbDelete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DelMuestra" OnClientClick="return confirm('¿Está seguro de borrar esta muestra?')" ToolTip="Eliminar esta muestra de la propuesta"><i class="metismenu-icon fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Codigo" HeaderText="CODANE" />
                                                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                                    <asp:BoundField DataField="NSE5Y6" HeaderText="NSE 5 y 6" />
                                                                    <asp:BoundField DataField="NSE4" HeaderText="NSE 4" />
                                                                    <asp:BoundField DataField="NSE123" HeaderText="NSE 1,2,3" />
                                                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:GridView ID="gvMuestraCATI" Visible="false" runat="server" AutoGenerateColumns="false" DataKeyNames="IDENTIFICADOR" CssClass="mb-0 table table-hover" OnRowCommand="gvMuestraCATI_RowCommand" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay muestra agregada">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbDelete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DelMuestra" OnClientClick="return confirm('¿Está seguro de borrar esta muestra?')" ToolTip="Eliminar esta muestra de la propuesta"><i class="metismenu-icon fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="TIPO_MUESTRA" HeaderText="Tipo Muestra" />
                                                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:GridView ID="gvMuestraOnline" Visible="false" runat="server" AutoGenerateColumns="false" DataKeyNames="Codigo" CssClass="mb-0 table table-hover" OnRowCommand="gvMuestraOnline_RowCommand" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay muestra agregada">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbDelete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DelMuestra" OnClientClick="return confirm('¿Está seguro de borrar esta muestra?')" ToolTip="Eliminar esta muestra de la propuesta"><i class="metismenu-icon fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Codigo" HeaderText="CODANE" />
                                                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                                    <asp:BoundField DataField="ALTADIFICULTAD" HeaderText="Alta Dificultad" />
                                                                    <asp:BoundField DataField="BAJADIFICULTAD" HeaderText="Baja Dificultad" />
                                                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="col-md-12">
                                                                <div class="position-relative form-group">
                                                                    <label for="txtObservacionesPresupuesto" class="">Observaciones</label><asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" class="form-control-min form-control" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnGuardar" type="button" class="btn btn-primary" runat="server" Text="Guardar"></asp:Button>
                                                <asp:Button ID="btnCancel" type="button" class="btn btn-primary" runat="server" Text="Cerrar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCancel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlGM" runat="server">
                        <asp:UpdatePanel ID="UPanelGM" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMetCodigo" runat="server" />
                                <asp:HiddenField ID="hfFase" runat="server" />
                                <asp:HiddenField ID="hfTope" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myBasicModalLabelGM" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">

                                                        <div class="form-row">
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="txtValorVentaSimular">Valor Venta</label>
                                                                <asp:TextBox ID="txtValorVentaSimular" runat="server" CssClass="form-control form-control-min" Width="100px"></asp:TextBox>
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="btnSimular">&nbsp</label>
                                                                <asp:Button ID="btnSimular" CssClass="btn btn-info" runat="server" Text="Simular GM" OnClick="btnSimular_Click" />
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <asp:Label ID="lblGMsimulado" runat="server" Font-Bold="True"></asp:Label>
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="btnSimular">&nbsp</label>
                                                                
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <asp:Button ID="btnModificarGM_1" CssClass="btn btn-info" runat="server" Text="Modificar venta" OnClientClick="return confirm('Realmente desea efectuar la modificacion?');" OnClick="btnModificarGM_1_Click" />
                                                            </div>
                                                        </div>
                                                        <div class="clearfix form-inline"></div>
                                                        <div class="form-row">
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="txtNuevoGM">GM Unidad *</label>
                                                                <asp:TextBox ID="txtNuevoGM" runat="server" CssClass="form-control form-control-min"
                                                                    ForeColor="#003300" Width="80px" Font-Bold="True"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtNuevoGM_FilteredTextBoxExtender"
                                                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=","
                                                                    TargetControlID="txtNuevoGM">
                                                                </asp:FilteredTextBoxExtender>
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="txtGMOpera">GM OPS *</label>
                                                                <asp:TextBox ID="txtGMOpera" runat="server" Font-Bold="True" CssClass="form-control form-control-min"
                                                                    ForeColor="#003300" Width="80px"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtGMOpera_FilteredTextBoxExtender" runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom, Numbers" TargetControlID="txtGMOpera" ValidChars=",">
                                                                </asp:FilteredTextBoxExtender>
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <asp:Label ID="lblgmActual" runat="server" ForeColor="Black"></asp:Label>
                                                                <asp:HiddenField ID="hfTipoCalculo" runat="server" Value="1" />
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <asp:Button ID="btnSimValorVenta" CssClass="btn btn-info" runat="server" Text="Simular venta" OnClick="btnSimValorVenta_Click" />
                                                                <asp:Button ID="btnModificarGM_2" CssClass="btn btn-info" runat="server" Text="Modificar GM" OnClick="btnModificarGM_2_Click" />
                                                            </div>

                                                        </div>
                                                        <div class="form-row">
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <asp:Label ID="lblValorVentaSimulado" runat="server" Font-Bold="True"></asp:Label>
                                                            </div>
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                            </div>
                                                        </div>
                                                        <div class="form-inline">
                                                            <label>
                                                                * Deje en blanco los datos que no requiere modificar para mantener el actual.
                                                            </label>
                                                        </div>
                                                        <div class="form-inline">
                                                            <label>
                                                                Los valores decimales se deben separar por coma (,)
                                                            </label>
                                                        </div>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td align="center" style="text-align: center" class="style23">
                                                                    <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%" Height="150px" CssClass="ajax__tab_lightblue-theme" VerticalStripWidth="140px">
                                                                        <asp:TabPanel ID="TabPanelG1" runat="server" HeaderText="TabPanel1">
                                                                            <HeaderTemplate>
                                                                                JobBook Interno
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="GVJBI" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="mb-0 table table-hover">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="JBDESCRIPCION" HeaderText="DESCRIPCION" DataFormatString="{0:C0}" />
                                                                                        <asp:BoundField DataField="TOTALCOSTO" HeaderText="COSTO" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </ContentTemplate>
                                                                        </asp:TabPanel>
                                                                        <asp:TabPanel ID="TabPanelG2" runat="server" HeaderText="TabPanel2">
                                                                            <HeaderTemplate>
                                                                                JobBook Externo
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="GVJBE" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="mb-0 table table-hover">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="JBDESCRIPCION" HeaderText="DESCRIPCION" />
                                                                                        <asp:BoundField DataField="TOTALCOSTO" HeaderText="COSTOS" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </ContentTemplate>
                                                                        </asp:TabPanel>
                                                                    </asp:TabContainer>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" class="style23" style="text-align: center">
                                                                    <asp:Panel ID="pnNotificacion" runat="server" Visible="False">
                                                                        <strong><span class="style24">Si usted no es un usuario autorizado presione el boton de enviar notificacion<br />
                                                                        </span></strong>&nbsp;<asp:Button ID="EnviarNotificacion0" runat="server" OnClick="EnviarNotificacion0_Click" BackColor="#CC3300" Text="Enviar notificacion" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" style="text-align: center" class="style15">
                                                                    <asp:Label ID="lblContrasena" runat="server" Text="Contrasena:" Visible="False"></asp:Label>
                                                                    <asp:TextBox ID="gmTxtContrasena" runat="server" TextMode="Password" AutoCompleteType="None"
                                                                        Visible="False" Width="100px"></asp:TextBox>
                                                                    &nbsp;<span class="auto-style2">*Presione nuevamente el boton modificar respectivo</span></td>
                                                            </tr>
                                                        </table>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelGM" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCancelGM" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlGMSimulator" runat="server">
                        <asp:UpdatePanel ID="UPanelSimulator" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfSimPropuesta" runat="server" />
                                <asp:HiddenField ID="hfSimAlternativa" runat="server" />
                                <asp:HiddenField ID="hfSimMetodologia" runat="server" />
                                <asp:HiddenField ID="hfSimFase" runat="server" />
                                <asp:HiddenField ID="hfGMMin" runat="server" />
                                <asp:HiddenField ID="hfVrVenta" runat="server" />
                                <asp:HiddenField ID="hfMargenBruto" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myBasicModalLabelGMSimulator" aria-hidden="true">
                                    <div class="modal-dialog modal-xl show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Valor Venta</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-success">
                                                                                            <asp:Label ID="lblSIMVrVenta" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>


                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Otros Costos</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-secondary">
                                                                                            <asp:Label ID="lblSIMOtrosCostos" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">GM %</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-success">
                                                                                            <asp:Label ID="lblSIMGM" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Cargos del grupo</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-secondary">
                                                                                            <asp:Label ID="lblSIMCargosGrupo" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Costo OPS</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-primary">
                                                                                            <asp:Label ID="lblSIMCostoOPS" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Total Costos Directos</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-info">
                                                                                            <asp:Label ID="lblSIMCostosDirectos" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Target Professional Time</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-primary">
                                                                                            <asp:Label ID="lblSIMTargetProfessionalTime" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>


                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">OP</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-info">
                                                                                            <asp:Label ID="lblSIMOP" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="main-card mb-3 card">
                                                            <div class="card-body">
                                                                <ul class="list-group">
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">% GM OPS</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-warning">
                                                                                            <asp:Label ID="lblSIMGMOPS" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Margen Bruto</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-alt">
                                                                                            <asp:Label ID="lblSIMMargenBruto" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>

                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">Professional Time</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-warning">
                                                                                            <asp:Label ID="lblSIMProfessionalTime" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>

                                                                    <li class="list-group-item">
                                                                        <div class="widget-content p-0">
                                                                            <div class="widget-content-outer">
                                                                                <div class="widget-content-wrapper-simulator">
                                                                                    <div class="widget-content-left">
                                                                                        <div class="widget-heading">OP %</div>

                                                                                    </div>
                                                                                    <div class="widget-content-right">
                                                                                        <div class="widget-numbers text-danger">
                                                                                            <asp:Label ID="lblSIMOPPercent" runat="server"></asp:Label></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">Criterio</button>
                                                        </div>
                                                        <asp:RadioButtonList ID="rbSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbSearch_SelectedIndexChanged" CssClass="form-check-inline form-control form-control-sm" CellPadding="10" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="1" Text="Venta"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="GM"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="OP"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="input-group col-md-3 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">$ Venta</button>
                                                        </div>
                                                        <asp:TextBox ID="txtVentaSimular" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                    runat="server" Enabled="True" FilterType="Numbers" 
                                                                    TargetControlID="txtVentaSimular">
                                                                </asp:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="input-group col-md-2 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">% GM</button>
                                                        </div>
                                                        <asp:TextBox ID="txtGMSimular" Enabled="false" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=","
                                                                    TargetControlID="txtGMSimular">
                                                                </asp:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="input-group col-md-2 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">% OP</button>
                                                        </div>
                                                        <asp:TextBox ID="txtOPSimular" Enabled="false" MaxLength="5" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=","
                                                                    TargetControlID="txtOPSimular">
                                                                </asp:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="input-group col-md-2 mb-3">
                                                        <div class="input-group-prepend">
                                                            <button class="btn btn-secondary">% GM OPS</button>
                                                        </div>
                                                        <asp:TextBox ID="txtGMOPSSimular" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=","
                                                                    TargetControlID="txtGMOPSSimular">
                                                                </asp:FilteredTextBoxExtender>
                                                    </div>

                                                </div>
                                                <div style="width: 100%; margin: 0 auto; text-align: center">
                                                    <asp:Button runat="server" ID="btnSimularExec" class="btn btn-primary" OnClick="btnSimularExec_Click" Text="Simular"></asp:Button>
                                                    <asp:Button ID="btnRecalcularHoras" CssClass="btn btn-secondary" runat="server" Text="Recalcular Horas" OnClick="btnRecalcularHoras_Click" />
                                                    <asp:Button runat="server" ID="btnAdjustment" OnClick="btnAdjustment_Click" class="btn btn-success" Text="Realizar el ajuste"></asp:Button>
                                                    <asp:Button runat="server" ID="btnRequestAdjustment" OnClick="btnRequestAdjustment_Click" class="btn btn-warning" Text="Solicitar aprobación del ajuste"></asp:Button>
                                                    <asp:TextBox CssClass="form-text" ID="txtComentariosSolicitud" placeholder="Si desea opcionalmente puede agregar comentarios cuando vaya a hacer una solicitud de ajuste en este espacio" runat="server" Width="100%"></asp:TextBox>
                                                    <asp:GridView ID="gvSolicitudes" runat="server" CssClass="mb-0 table" AutoGenerateColumns="false" EmptyDataText="No se han hecho solicitudes para autorización de cambio de gross">
                                                    <Columns>
                                                        <asp:BoundField DataField="EstadoSolicitud" HeaderText="Estado" />
                                                        <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha" />
                                                        <asp:BoundField DataField="Aprobador" HeaderText="Aprobador" />
                                                        <asp:BoundField DataField="FechaAprobacion" HeaderText="Aprobación" />
                                                        <asp:BoundField DataField="ComentariosAprobacion" HeaderText="Comentarios" />
                                                    </Columns>
                                                </asp:GridView>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                
                                                <asp:Button ID="btnCancelSimulator" type="button" class="btn btn-secondary" runat="server" Text="Cerrar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCancelSimulator" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlImport" runat="server">
                        <asp:UpdatePanel ID="UPanelImportar" runat="server">
                            <ContentTemplate>
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">Importar alternativas</h5>
                                                        <div>
                                                            <div class="form-row">
                                                                <div class="col-md-3">
                                                                    <div class="position-relative form-group">
                                                                        <label class="">Criterio</label>
                                                                        <asp:DropDownList ID="ddlSearch" runat="server" CssClass="form-control-min-select form-control">
                                                                            <asp:ListItem Value="1" Text="Mis jobs" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Value="2" Text="Los de mi unidad"></asp:ListItem>
                                                                            <asp:ListItem Value="3" Text="Todos"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="position-relative form-group">
                                                                        <label class="">Título</label>

                                                                        <asp:TextBox ID="txtTituloSearch" runat="server" CssClass="form-control-min form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="position-relative form-group">
                                                                        <label class="">JobBook</label>

                                                                        <asp:TextBox ID="txtJobBookSearch" runat="server" CssClass="form-control-min form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="position-relative form-group">
                                                                        <label class="">No Propuesta</label>

                                                                        <asp:TextBox ID="txtIdPropuestaSearch" runat="server" CssClass="form-control-min form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:Button runat="server" ID="btnSearchImport" class="btn btn-primary" Text="Buscar" OnClick="btnSearchImport_Click"></asp:Button>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <asp:UpdatePanel ID="UPanelSearch" runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="lkbModals" runat="server"></asp:LinkButton>
                                                                <asp:ModalPopupExtender ID="ModalPopupExtenderClonar" CancelControlID="btnCancelClone" PopupControlID="pnlDuplicar" TargetControlID="lkbModals" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                                                                </asp:ModalPopupExtender>
                                                                <asp:GridView ID="gvDataSearchImport" runat="server" AutoGenerateColumns="false" DataKeyNames="IdBrief,IdPropuesta,IdEstudio" OnRowCommand="gvDataSearchImport_RowCommand" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbSelect" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="SelectProp" ToolTip="Seleccionar presupuestos de esta alternativa"><i class="metismenu-icon fa fa-share-square"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="IdPropuesta" HeaderText="Propuesta" />
                                                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                                        <asp:BoundField DataField="Titulo" HeaderText="Nombre" />
                                                                        <asp:BoundField DataField="MarcaCategoria" HeaderText="Marca o Categoría" HeaderStyle-Wrap="true" />
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                        <asp:BoundField DataField="GerenteCuentas" HeaderText="Gerente Cuentas" HeaderStyle-Wrap="true" />
                                                                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCloseImport" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlExportToiQuote" runat="server" DefaultButton="btnExportToiQuote">
                        <asp:UpdatePanel ID="UPanelExportiQuote" runat="server">
                            <ContentTemplate>
                                <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-sm show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">Exportación a iQuote</h5>
                                                        <div>
                                                            <div class="form-row">
                                                                <asp:Panel ID="pnlOldExportToIQuoteNotVisible" runat="server" Visible="false">
                                                                    <div class="col-md-6">
                                                                        <div class="position-relative form-group">
                                                                            <label class="">Usuario iQuote</label>
                                                                            <asp:TextBox ID="txtUsuarioiQuote" runat="server" placeholder="Usuario" CssClass="form-control-min form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="position-relative form-group">
                                                                            <label class="">Password iQuote</label>

                                                                            <asp:TextBox ID="txtPasswordiQuote" placeholder="Contraseña" TextMode="Password" runat="server" CssClass="form-control-min form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <label>
                                                                            <asp:CheckBox ID="chbVPNIQuote" runat="server" />Conectado a VPN</label>
                                                                    </div>
                                                                </asp:Panel>
                                                                <div class="col-md-12">
                                                                    <label>
                                                                        Al hacer clic en continuar, se marcará esta alternativa para ser enviada a IQuote. Deberá usar el programa de escritorio para esto. Si requiere guía para usarlo
                                                        o no lo tiene instalado por favor contacte a IT Local.
                                                                    </label>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:Button runat="server" ID="btnExportToiQuote" class="btn btn-primary" Text="Continuar" OnClick="btnExportToiQuote_Click"></asp:Button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelExportiQuote" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExportToiQuote" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlDuplicar" runat="server">
                        <asp:UpdatePanel ID="UPanelClonar" runat="server">
                            <ContentTemplate>
                                <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-sm show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <div class="form-row">
                                                            <div class="form-group" id="divPresupuestos">
                                                                <asp:GridView ID="gvPresupuestosImport" runat="server" Enabled="true" AutoGenerateColumns="false" DataKeyNames="Id,Alternativa,PropuestaId" OnRowCommand="gvPresupuestosImport_RowCommand" Width="100%" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                                                                        <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                                                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                                                                        <asp:CheckBoxField DataField="Aprobado" HeaderText="Revisado" />
                                                                        <asp:TemplateField HeaderText="Importar" ShowHeader="False">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbImportAlternativa" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ImportAlternativa" ToolTip="Importar esta alternativa" OnClientClick="return confirm('¿Está seguro de importar esta alternativa?')"><i class="metismenu-icon fa fa-download"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelClone" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="gvPresupuestosImport" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlJBI" runat="server">
                        <asp:UpdatePanel ID="UpdatePanelJBI" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMetCodigoJBI" runat="server" />
                                <asp:HiddenField ID="hfFaseJBI" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">JobBook Interno</h5>
                                                        <div class="form-inline">
                                                            <asp:ImageButton runat="server" ImageUrl="~/Images/excel.jpg"
                                                                Height="36px" Width="39px" ID="ExpToExcelJBI"></asp:ImageButton>
                                                        </div>
                                                        <div>
                                                            <asp:GridView runat="server" EmptyDataText="No existen datos"
                                                                CssClass="mb-0 table" Width="100%" ID="gvJBInterno" ShowFooter="True">
                                                                <FooterStyle Font-Bold="True" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelJBI" type="button" class="btn btn-secondary" runat="server" Text="Close"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ExpToExcelJBI" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlJBE" runat="server">
                        <asp:UpdatePanel ID="UpdatePanelJBE" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMetCodigoJBE" runat="server" />
                                <asp:HiddenField ID="hfFaseJBE" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">JobBook Externo</h5>
                                                        <div class="form-inline">
                                                            <asp:ImageButton runat="server" ImageUrl="~/Images/excel.jpg"
                                                                Height="36px" Width="39px" ID="ExpToExcelJBE"></asp:ImageButton>
                                                        </div>
                                                        <div>
                                                            <asp:GridView runat="server" EmptyDataText="No existen datos"
                                                                CssClass="mb-0 table" Width="100%" ID="gvJBExterno" ShowFooter="True">
                                                                <FooterStyle Font-Bold="True" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelJBE" type="button" class="btn btn-secondary" runat="server" Text="Close"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ExpToExcelJBE" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlExecution" runat="server">
                        <asp:UpdatePanel ID="UpdatePanelExecution" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMetCodigoCostos" runat="server" />
                                <asp:HiddenField ID="hfFaseCostos" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">Detalle de Costos</h5>
                                                        <div class="form-inline">
                                                            <asp:ImageButton runat="server" ImageUrl="~/Images/excel.jpg"
                                                                Height="36px" Width="39px" ID="ExpToExcelCostos"></asp:ImageButton>
                                                        </div>
                                                        <div>
                                                            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1"
                                                                Width="100%" CssClass="ajax__tab_lightblue-theme">
                                                                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                                                                    <HeaderTemplate>
                                                                        <div class="badge badge-info">
                                                                            Detalles costo de la unidad
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvControlCostos" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="displayTable" ShowFooter="True" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                                                                <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUPUESTADO">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="AUTORIZADO" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS AUTO" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}"
                                                                                    HeaderText="%" Visible="False" />
                                                                                <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRODUCCION" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS PROD" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}"
                                                                                    HeaderText="%" Visible="False" />
                                                                                <asp:BoundField DataField="EJECUTADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="EJECUTADO" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS EJEC" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}"
                                                                                    HeaderText="%" Visible="False" />
                                                                                <asp:BoundField DataField="UNIDADES" DataFormatString="{0:N0}"
                                                                                    HeaderText="UNIDADES" />
                                                                                <asp:BoundField DataField="DESC_UNIDADES" HeaderText="DESCRIPCION" />
                                                                            </Columns>
                                                                            <FooterStyle Font-Bold="True" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:TabPanel>
                                                                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                                                                    <HeaderTemplate>
                                                                        <div class="badge badge-info">
                                                                            Detalles costo de operaciones
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvDetallesOperaciones" runat="server"
                                                                            AutoGenerateColumns="False" CssClass="displayTable" ShowFooter="True"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                                                                <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUPUESTADO" />
                                                                                <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="AUTORIZADO" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS AUTO" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" HeaderText="%"
                                                                                    Visible="False" />
                                                                                <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRODUCCION" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS PROD" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}"
                                                                                    HeaderText="%" Visible="False" />
                                                                                <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}"
                                                                                    HeaderText="PRESUP VS EJEC" Visible="False" />
                                                                                <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%"
                                                                                    Visible="False" />
                                                                                <asp:BoundField DataField="UNIDADES" DataFormatString="{0:N0}"
                                                                                    HeaderText="UNIDADES" />
                                                                                <asp:BoundField DataField="DESC_UNIDADES" HeaderText="DESCRIPCION" />
                                                                                <asp:BoundField DataField="HORAS" HeaderText="HORAS" />
                                                                            </Columns>
                                                                            <FooterStyle Font-Bold="True" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:TabPanel>
                                                                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                                                                    <HeaderTemplate>
                                                                        <div class="badge badge-info">
                                                                            Viaticos presupuesto
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvViaticos" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="displayTable" ShowFooter="True" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CODIGO" HeaderText="CODIGO" />
                                                                                <asp:BoundField DataField="CIUDAD" HeaderText="CIUDAD" />
                                                                                <asp:BoundField DataField="HOTELES" DataFormatString="{0:C0}"
                                                                                    HeaderText="HOTELES" />
                                                                                <asp:BoundField DataField="TRANSPORTE" DataFormatString="{0:C0}"
                                                                                    HeaderText="TRANSPORTE" />
                                                                                <asp:BoundField DataField="TOTAL" DataFormatString="{0:C0}"
                                                                                    HeaderText="TOTAL" />
                                                                            </Columns>
                                                                            <FooterStyle Font-Bold="True" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:TabPanel>
                                                                <asp:TabPanel ID="TabPanel4" runat="server" CssClass="table table-striped" HeaderText="TabPanel4" Visible="False">
                                                                    <HeaderTemplate>
                                                                        <div class="badge badge-info">
                                                                            P&L  - Presupuesto
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvPYGPresupuesto" DataKeyNames="ID" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="displayTable" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto" />
                                                                                <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:TabPanel>
                                                                <asp:TabPanel ID="TabPanel5" runat="server" CssClass="table table-striped" HeaderText="TabPanel5" Visible="False">
                                                                    <HeaderTemplate>
                                                                        <div class="badge badge-info">
                                                                            P&L - Alternativa
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvPYGAlternativa" DataKeyNames="ID" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="displayTable" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto" />
                                                                                <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:TabPanel>
                                                            </asp:TabContainer>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelExecution" type="button" class="btn btn-secondary" runat="server" Text="Close"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ExpToExcelCostos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlCopiarPresupuesto" runat="server">
                        <asp:UpdatePanel ID="uPanelCopiarPresupuesto" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMetCodigoCopiar" runat="server" />
                                <asp:HiddenField ID="hfFaseCopiar" runat="server" />
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">Copiar o duplicar presupuesto</h5>
                                                        <div class="form-inline">
                                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                <label for="ddlAlternativaToCopy" class="mr-sm-2">Alternativa</label><asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlternativaToCopy_SelectedIndexChanged" ID="ddlAlternativaToCopy" class="form-control-min-select form-control">
                                                                </asp:DropDownList>
                                                                <label for="ddlFaseToCopy" class="mr-sm-2">Fase</label><asp:DropDownList runat="server" ID="ddlFaseToCopy" class="form-control-min-select form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnOkCopiarAlternativa" type="button" class="btn btn-secondary" runat="server" Text="Copiar" OnClick="btnOkCopiarAlternativa_Click"></asp:Button>
                                                <asp:Button ID="btnCancelCopiar" type="button" class="btn btn-secondary" runat="server" Text="Cerrar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCancelCopiar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="pnlExcelMuestra" runat="server">
                        <asp:UpdatePanel ID="UPanelExcelMuestra" runat="server">
                            <ContentTemplate>
                                <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg show">
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="main-card mb-3 card">
                                                    <div class="card-body">
                                                        <h5 class="card-title">Importar muestra desde Excel</h5>
                                                        <div class="form-inline">
                                                            <label>Descargue el formato en Excel desde este icono para cargar los datos. No cambie la estructura ni inserte o elimine filas</label>
                                                            <a style="margin-left: 10px;" href="../Files/FormatoImportarMuestraMatrix20220922.xlsx" target="_blank">
                                                                <img src="../Images/excel.jpg" alt="Excel de importar" style="width: 36px; height: 36px" /></a>
                                                        </div>
                                                        <div>
                                                            <div class="form-inline">
                                                                <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                                    <label for="FUploadExcelMuestra" class="mr-sm-2">Seleccione el archivo a subir</label>
                                                                    <asp:FileUpload ID="FUploadExcelMuestra" runat="server" CssClass="form-control-min-select form-control" />
                                                                </div>
                                                                <div class="form-inline">
                                                                    <label for="ddlHojaArchivo" class="mr-sm-2">Qué hoja va a importar</label><asp:DropDownList runat="server" ID="ddlHojaArchivo" class="form-control-min-select form-control">
                                                                        <asp:ListItem Value="Muestra_x_NSE_Poblacional" Text="Muestra x NSE poblacional"></asp:ListItem>
                                                                        <asp:ListItem Value="Muestra_x_NSE" Text="Muestra x NSE"></asp:ListItem>
                                                                        <asp:ListItem Value="Muestra_x_Dificultad" Text="Muestra x dificultad"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <br />
                                                                <div class="form-inline">
                                                                    <asp:Button ID="btnLoadDataExcel" runat="server" Text="Importar la información" CssClass="btn btn-primary" OnClick="btnLoadDataExcel_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnCancelExcelMuestra" type="button" class="btn btn-secondary" runat="server" Text="Cerrar"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnLoadDataExcel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

    </div>

    <asp:Panel ID="pnlMessageInfo" runat="server" DefaultButton="btnCloseAlert">
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
</asp:Content>
