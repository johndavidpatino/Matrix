<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Header_Presupuesto.ascx.vb" Inherits="WebMatrix.UC_Header_Presupuesto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<div class="form-row">
    <div class="col-md-12">
        <div class="position-relative form-group">
            <label for="txtGrupoObjetivo" class="">Grupo Objetivo</label><asp:TextBox runat="server" ID="txtGrupoObjetivo" TextMode="MultiLine" class="form-control-min form-control" />
        </div>
    </div>
</div>
<h5 class="card-title">Preguntas y Procesos</h5>
<div class="form-row">
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtCerradas" class="">Cerradas</label><asp:TextBox runat="server" ID="txtCerradas" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtCerradasMultiples" class="">Cerradas múltiples</label><asp:TextBox runat="server" ID="txtCerradasMultiples" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtAbiertas" class="">Abiertas</label><asp:TextBox runat="server" ID="txtAbiertas" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtAbiertasMultiples" class="">Abiertas múltiples</label><asp:TextBox runat="server" ID="txtAbiertasMultiples" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtOtros" class="">Otros</label><asp:TextBox runat="server" ID="txtOtros" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
    <div class="col-md-2">
        <div class="position-relative form-group">
            <label for="txtDemograficos" class="">Demográficos</label><asp:TextBox runat="server" Text="15" ID="txtDemograficos" TextMode="Number" class="form-control-min form-control" />
        </div>
    </div>
</div>

<div class="form-inline">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnHistorial" runat="server" Visible="false" Text="Histórico de preguntas" class="mb-2 mr-2 btn btn-info"></asp:Button>
            <asp:ModalPopupExtender ID="btnHitorial_ModalPopupExtender" CancelControlID="btnCancelarHistorico" runat="server" Enabled="True" PopupControlID="PnlHistoricoPreguntas" TargetControlID="btnHistorial" BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <label for="ddlIncidencia" class="mr-sm-2">Incidencia</label><asp:DropDownList runat="server" ID="ddlIncidencia" class="form-control-min-select form-control" />
    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <label for="txtProductividad" class="mr-sm-2">Productividad</label><asp:TextBox runat="server" Width="50px" ID="txtProductividad" class="form-control-min form-control" />
    </div>
    <div class="position-relative form-check form-check-inline">
        <label class="form-check-label">
            <asp:CheckBox runat="server" ID="chbProbabilistico" class="form-check-input" />
            Probabilístico</label>
    </div>
    <div class="position-relative form-check form-check-inline">
        <label class="form-check-label" style="font-weight: bold">
            <asp:CheckBox runat="server" ID="chbF2fVirtual" class="form-check-input" />
            F2F Virtual</label>
    </div>
            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
            <label for="ddlCodificación" class="mr-sm-2">Complejidad Cuestionario</label><asp:DropDownList runat="server" ID="ddlComplejidadCuestionario" class="form-control-min-select form-control">
                <asp:ListItem Value="1" Text="Básica"></asp:ListItem>
                <asp:ListItem Value="2" Text="Estándar" Selected="True"></asp:ListItem>
                <asp:ListItem Value="3" Text="Compleja"></asp:ListItem>
            </asp:DropDownList>
            </div>

</div>

<asp:Panel ID="PnlHistoricoPreguntas" runat="server" Visible="false" BackColor="#1C9993" ScrollBars="Horizontal">
    <asp:UpdatePanel ID="upHistorico" runat="server">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td class="auto-style1">Unidad:</td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="lstUnidad_hist" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style1">Jobbook:</td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txtJobbook_Hist" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Oferta:</td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="lstOferta_Hist" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style1">Producto:</td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="lstProducto_Hist" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Nombre:</td>
                                <td>
                                    <asp:TextBox ID="txtNombres_Hist" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnBuscarHist" runat="server" Text="Buscar" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvHistPreg" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="displayTable" AllowPaging="True" EmptyDataText="No existen datos">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="CerradasRU" HeaderText="Cerradas" />
                                <asp:BoundField DataField="CerradasRM" HeaderText="Cerradas Mult" />
                                <asp:BoundField DataField="Abiertas" HeaderText="Abiertas" />
                                <asp:BoundField DataField="AbiertasMul" HeaderText="Abiertas Mult" />
                                <asp:BoundField DataField="Otros" HeaderText="Otros" />
                                <asp:BoundField DataField="Demograficos" HeaderText="Demo" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook">
                                    <ItemStyle Width="110px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:ButtonField ButtonType="Button" CommandName="SEL" Text="Seleccionar" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td class="auto-style2">&nbsp;</td>
                                <td style="text-align: left">
                                    <asp:Button ID="btnCancelarHistorico" CssClass="btn btn-light" runat="server" Text="Cancelar" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Panel>
<div class="form-inline">
    <div class="position-relative form-check form-check-inline">
        <div class="position-relative form-check form-check-inline">
            <label class="form-check-label">
                <asp:CheckBox runat="server" ID="chbProcessScripting" Checked="true" class="form-check-input" />
                Scripting</label>
        </div>
        <div class="position-relative form-check form-check-inline">
            <label class="form-check-label">
                <asp:CheckBox runat="server" ID="chbProcessCampo" Checked="true" class="form-check-input" />
                Campo</label>
        </div>
        <div class="position-relative form-check form-check-inline">
            <label class="form-check-label">
                <asp:CheckBox runat="server" ID="chbProcessVerificacion" Checked="true" class="form-check-input" />
                Verificación</label>
        </div>
        <div class="position-relative form-check form-check-inline">
            <label class="form-check-label">
                <asp:CheckBox runat="server" ID="chbProcessCritica" Checked="true" class="form-check-input" />
                Crítica</label>
        </div>
        <div class="position-relative form-check form-check-inline">
            <label class="form-check-label">
                <asp:CheckBox runat="server" ID="chbProcessCodificacion" Checked="true" class="form-check-input" />
                Codificación</label>
        </div>
        <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
            <asp:CheckBox runat="server" ID="chbProcessDataClean" Visible="false" class="form-check-input" />
            <asp:CheckBox runat="server" ID="chbProcessTopLines" Visible="false" class="form-check-input" />
            <asp:CheckBox runat="server" ID="chbProcessProceso" Visible="false" class="form-check-input" />
            <asp:CheckBox runat="server" ID="chbProcessArchivos" Visible="false" class="form-check-input" />
            <label for="ddlCodificación" class="mr-sm-2">Complejidad Codificación</label><asp:DropDownList runat="server" ID="ddlComplejidadCodificación" class="form-control-min-select form-control">
                <asp:ListItem Value="1" Text="Básica"></asp:ListItem>
                <asp:ListItem Value="2" Text="Estándar" Selected="True"></asp:ListItem>
                <asp:ListItem Value="3" Text="Compleja"></asp:ListItem>
            </asp:DropDownList>
        </div>



    </div>
</div>
<div class="form-inline">
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <div class="input-group-prepend">
            <span class="input-group-text">Especificaciones</span>
        </div>
    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <asp:UpdatePanel ID="UPanelBtnDP" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnDP" runat="server" Text="Procesamiento" class="btn btn-info"></asp:Button>
                <asp:ModalPopupExtender ID="ModalPopupExtenderDP" CancelControlID="btnCancelDP" runat="server" Enabled="True" PopupControlID="pnlEspecificacionesDP" TargetControlID="btnDP" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <asp:UpdatePanel ID="UPanelBtnEstadistica" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnEstadistica" runat="server" Text="Estadística" class="btn btn-info"></asp:Button>
                <asp:ModalPopupExtender ID="ModalPopupExtender1" CancelControlID="btnCancelEstadistica" runat="server" Enabled="True" PopupControlID="pnlEspecificacionEstadistica" TargetControlID="btnEstadistica" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <asp:UpdatePanel ID="UPanelBtnPruebaProducto" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnPruebaProducto" runat="server" Enabled="false" Text="Prueba de Producto" class="btn btn-info"></asp:Button>
                <asp:ModalPopupExtender ID="ModalPopupExtender2" CancelControlID="btnCancelPruebaProducto" runat="server" Enabled="True" PopupControlID="pnlEspecificacionPruebaProducto" TargetControlID="btnPruebaProducto" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <asp:UpdatePanel ID="UPanelBtnProfessionalTimes" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnProfessionalTimes" runat="server" Text="Professional Times" class="btn btn-info"></asp:Button>
                <asp:ModalPopupExtender ID="ModalPopupExtender4" CancelControlID="btnCancelProfessionalTimes" runat="server" Enabled="True" PopupControlID="pnlEspecificacionesProfessionalTime" TargetControlID="btnProfessionalTimes" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
        <asp:UpdatePanel ID="UPanelActividadesSubcontratadas" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnSubcontratadas" runat="server" Text="Sub EXT" class="btn btn-info"></asp:Button>
                <asp:ModalPopupExtender ID="ModalPopupExtender3" CancelControlID="btnCancelSubcontratadas" runat="server" Enabled="True" PopupControlID="pnlActividadesSubContratadas" TargetControlID="btnSubcontratadas" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<asp:Panel ID="pnlEspecificacionPruebaProducto" runat="server">
    <asp:UpdatePanel ID="UPanelEspecificacionesProducto" runat="server">
        <ContentTemplate>
            <div class="bd-example-modal" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="main-card mb-3 card">
                                <div class="card-body">
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Personas requeridas</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtPorcInterceptacion" class="">% Interceptación</label><asp:TextBox runat="server" ID="txtPorcInterceptacion" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtPorcReclutamiento" class="">% Reclutamiento</label><asp:TextBox runat="server" ID="txtPorcReclutamiento" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtEncuestadoresPunto" class=""># Encuestadores</label><asp:TextBox runat="server" ID="txtEncuestadoresPunto" ToolTip="Número de encuestadores requeridos por punto" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtApoyosLogisticos" class=""># Apoyos</label><asp:TextBox runat="server" ToolTip="Número de apoyos logísticos requeridos" ID="txtApoyosLogisticos" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="form-inline"></div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Producto Requerido</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Active si se requiere compra de producto" ID="chbPTRequierecompra" class="form-check-input" />
                                                    Requiere compra</label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Active si se requiere compra de producto" ID="chbPTNeutralizador" class="form-check-input" />
                                                    Neutralizado de paladar</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                <label for="ddlTipoProducto" class="mr-sm-2">Tipo de Producto</label><asp:DropDownList runat="server" ID="ddlTipoProducto" class="form-control-min-select form-control">
                                                    <asp:ListItem Value="1" Text="Pequeño, liviano (ej: gomas, chiclets, dulces)"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Mediano, medio (ej: champú, jabón)"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Mediano, pesado (ej: detergentes, bebidas enlatadas)"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Grande, pesado (ej: toallas de papel, pañales)"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtNumLotes" class=""># Lotes</label><asp:TextBox runat="server" ID="txtNumLotes" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtNumUnidadesLote" class=""># Unidades</label><asp:TextBox runat="server" ID="txtNumUnidadesLote" ToolTip="Número de unidades por lote" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtNumValorProducto" class="">Valor producto</label><asp:TextBox runat="server" ToolTip="Valor unitario del producto" ID="txtValorProducto" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-inline"></div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Evaluación</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtVisitasRequeridas" class=""># Visitas</label><asp:TextBox runat="server" ToolTip="Número de visitas requeridas" ID="txtVisitasRequeridas" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtCeldasEvaluadas" class=""># Celdas</label><asp:TextBox runat="server" ToolTip="Número de celdas a evaluar" ID="txtCeldasEvaluadas" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtNumeroProductosPersona" class=""># Productos</label><asp:TextBox runat="server" ID="txtNumeroProductosPersona" ToolTip="Número de productos que evalúa la persona" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-inline"></div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">CLT</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Active si se requiere acceso a Internet en el CLT" ID="chbPTAccesoInternet" class="form-check-input" />
                                                    Acceso a Internet</label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                <label for="ddlTipoCLT" class="mr-sm-2">Tipo CLT</label><asp:DropDownList runat="server" ID="ddlTipoCLT" class="form-control-min-select form-control">
                                                    <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Café Internet"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Nivel C (Bajo)"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Nivel B (Medio)"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Nivel A (Alto)"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtValorAlquilerEquipos" class="">Alquiler equipos</label><asp:TextBox runat="server" ID="txtValorAlquilerEquipos" ToolTip="Valor de alquiler de equipos" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>


                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkPruebaProducto" type="button" class="btn btn-secondary" runat="server" Text="Continuar"></asp:Button>
                            <asp:Button ID="btnCancelPruebaProducto" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlEspecificacionesDP" runat="server">
    <asp:UpdatePanel ID="UpanelEspecifDP" runat="server">
        <ContentTemplate>
            <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="main-card mb-3 card">
                                <div class="card-body">
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Cantidad de Procesos</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtProcesosDataClean" class="">DataClean</label><asp:TextBox runat="server" ID="txtProcesosDataClean" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtProcesosToplines" class="">TopLines</label><asp:TextBox runat="server" ID="txtProcesosToplines" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtProcesosTablas" class="">Tablas</label><asp:TextBox runat="server" ID="txtProcesosTablas" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group">
                                                <label for="txtProcesosArchivos" class="">Bases</label><asp:TextBox runat="server" ID="txtProcesosArchivos" TextMode="Number" class="form-control-min form-control" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-3">
                                            <div class="position-relative form-group form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Active para casos donde la base viene de proveedor externo (ej: Genius Lab)" ID="chbDPTransformacion" class="form-check-input" />
                                                    Requiere Transformación</label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="position-relative form-group form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Active para casos donde debe unir bases, como cuando hay mediciones anteriores" ID="chbDPUnificacion" class="form-check-input" />
                                                    Requiere Unificación</label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                <label for="ddlComplejidadProcesamiento" class="mr-sm-2">Complejidad</label><asp:DropDownList runat="server" ID="ddlComplejidadProcesamiento" ToolTip="Complejidad del Procesamiento" class="form-control-min-select form-control">
                                                    <asp:ListItem Value="1" Text="Básico"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Estándar"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Complejo"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                <label for="ddlPonderacion" class="mr-sm-2">Ponderación</label><asp:DropDownList runat="server" ID="ddlPonderacion" class="form-control-min-select form-control">
                                                    <asp:ListItem Value="1" Text="Ninguna"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Simple"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Moderada"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Compleja"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="form-inline"></div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Origenes de Bases de datos</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-inline">
                                        <div class="position-relative form-check form-check-inline">
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPInInterna" Checked="true" class="form-check-input" />
                                                    Interna</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPInCliente" class="form-check-input" />
                                                    Cliente</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPInPanel" class="form-check-input" />
                                                    Panel</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Proveedor externo" ID="chbDPInExterno" class="form-check-input" />
                                                    Externo</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPInGMU" class="form-check-input" />
                                                    GMU</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPInOtro" class="form-check-input" />
                                                    Otro</label>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="position-relative form-group">
                                                <label class="">Destinos de Bases de datos</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-inline">
                                        <div class="position-relative form-check form-check-inline">
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPOutCliente" class="form-check-input" />
                                                    Cliente</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPOutWebDelivery" class="form-check-input" />
                                                    WebDelivery</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ToolTip="Proveedor externo" ID="chbDPOutExterno" class="form-check-input" />
                                                    Externo</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPOutGMU" class="form-check-input" />
                                                    GMU</label>
                                            </div>
                                            <div class="position-relative form-check form-check-inline">
                                                <label class="form-check-label">
                                                    <asp:CheckBox runat="server" ID="chbDPOutOtro" class="form-check-input" />
                                                    Otro</label>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOKDP" type="button" class="btn btn-secondary" runat="server" OnClick="btnOKDP_Click1" Text="Continuar"></asp:Button>
                            <asp:Button ID="btnCancelDP" Visible="true" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlEspecificacionEstadistica" runat="server">
    <asp:UpdatePanel ID="UPEstadistica" runat="server">
        <ContentTemplate>
            <div class="bd-example-modal-xl" tabindex="-1" role="dialog" aria-labelledby="myBasicModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="main-card mb-3 card">
                                <div class="card-body">
                                    <div>
                                        <asp:GridView ID="gvAnalisisEstadisticos" runat="server" AutoGenerateColumns="false" DataKeyNames="IDAnalisis" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay presupuestos Online creados" Width="100%" >
                                            <Columns>
                                                <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                                                <asp:BoundField DataField="AnalisisServicio" HeaderText="Analisis / Servicio" HeaderStyle-Wrap="true" />
                                                <asp:BoundField DataField="Precio" HeaderText="Precio sin margen" DataFormatString="{0:C0}" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" />
                                                <asp:TemplateField HeaderText="Cantidad" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCantidad" runat="server" Text='<%# Bind("Cantidad") %>' CssClass="nopadding form-control-min form-control" TextMode="Number" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EspecificacionesEntregable" HeaderText="Entregable" ItemStyle-Wrap="true" />
                                                <asp:ImageField DataImageUrlField="IDAnalisis" DataImageUrlFormatString="~\Images\cotizadorEstadistica\Imagen{0}.png" NullDisplayText="N/D" ControlStyle-Width="150px" ControlStyle-Height="150px" ItemStyle-Width="100px" ItemStyle-Height="100px"  ></asp:ImageField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOKEstadistica" type="button" class="btn btn-secondary" runat="server" Text="Continuar"></asp:Button>
                            <asp:Button ID="btnCancelEstadistica" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlActividadesSubContratadas" runat="server">
    <asp:UpdatePanel ID="UPanelSubContratadas" runat="server">
        <ContentTemplate>
            <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myBasicModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="main-card mb-3 card">
                                <div class="card-body">
                                    <div>
                                        <asp:GridView ID="gvActividadesSubcontratadas" runat="server" AutoGenerateColumns="false" DataKeyNames="ASID" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay presupuestos Online creados" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="Actividad" HeaderText="Analisis / Servicio" ItemStyle-Wrap="true" />
                                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" />
                                                <asp:TemplateField HeaderText="Valor" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtValorAct" runat="server" Text='<%# Bind("VALOR", "{0:C0}") %>' Width="100px" CssClass="form-control-min form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkSubContratadas" type="button" class="btn btn-secondary" runat="server" Text="Continuar"></asp:Button>
                            <asp:Button ID="btnCancelSubcontratadas" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlEspecificacionesProfessionalTime" runat="server">
    <asp:UpdatePanel ID="UPanelProfessionalTime" runat="server">
        <ContentTemplate>
            <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="main-card mb-3 card">
                                <div class="card-body">
                                    <div>
                                        <asp:GridView ID="gvProfessionalTime" runat="server" AutoGenerateColumns="false" DataKeyNames="PgrCodigo" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay presupuestos Online creados" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="PgrDescripcion" HeaderText="Level" HeaderStyle-Wrap="true" />
                                                <asp:TemplateField HeaderText="PreField" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPreField" runat="server" Text='<%# Bind("PreField") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FieldWork" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFieldWork" runat="server" Text='<%# Bind("FieldWork") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DP/Coding" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDPandCoding" runat="server" Text='<%# Bind("DPandCoding") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analysis" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAnalysis" runat="server" Text='<%# Bind("Analysis") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PM" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPM" runat="server" Text='<%# Bind("PM") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Meetings" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMeetings" runat="server" Text='<%# Bind("Meetings") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Presentation" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPresentation" runat="server" Text='<%# Bind("Presentation") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ClientTravel" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtClientTravel" runat="server" Text='<%# Bind("ClientTravel") %>' TextMode="Number" CssClass="nopadding form-control-min form-control" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TotalHoras" HeaderText="Total" HeaderStyle-Wrap="true" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkProfessionalTime" type="button" class="btn btn-secondary" runat="server" Text="Continuar"></asp:Button>
                            <asp:Button ID="btnCancelProfessionalTimes" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
