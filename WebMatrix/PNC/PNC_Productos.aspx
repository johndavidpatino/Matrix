<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master" CodeBehind="PNC_Productos.aspx.vb" Inherits="WebMatrix.PNC_Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Styles/formulario.css" rel="stylesheet" type="text/css" />--%>
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
    <script src="../Scripts/filter.js" type="text/javascript"></script>
    <style type="text/css">
        .itemWizard {
            float: left;
        }

        .itemWizardActivo {
            border: 0.5px solid #eee;
            border-left-color: white;
            border-left-width: 5px;
        }

        .itemWizardInactivo {
            border: none;
        }
    </style>
    <style type="text/css">
        .ui-tabs-vertical {
            /*width: 55em;*/
        }

            .ui-tabs-vertical .ui-tabs-nav {
                padding: .2em .1em .2em .2em;
                float: left;
                /*width: 12em;*/
            }

                .ui-tabs-vertical .ui-tabs-nav li {
                    clear: left;
                    width: 100%;
                    border-bottom-width: 1px !important;
                    border-right-width: 0 !important;
                    margin: 0 -1px .2em 0;
                }

                    .ui-tabs-vertical .ui-tabs-nav li a {
                        display: block;
                    }

                    .ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active {
                        padding-bottom: 0;
                        padding-right: .1em;
                        border-right-width: 1px;
                    }

            .ui-tabs-vertical .ui-tabs-panel {
                padding: 1em;
                float: right;
                width: 48em;
            }

                .ui-tabs-vertical .ui-tabs-panel span {
                    font-size: 13px;
                }

        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            background-color: #777;
            border-radius: 10px;
            margin-left: 5px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });
        function loadPlugins() {
            $("#<%= txtFechaEstimadaCierre.ClientId%>").attr('readonly', 'true').datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                minDate: 0
            });
            $('#tabs').tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#<%= txtFechaReclamo.ClientId%>").attr('readonly', 'true').datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                maxDate: 0
            });
            $.validator.addMethod('selectNone',
          function (value, element) {
              return (this.optional(element) == true) || ((value != -1) && (value != ''));
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });
            $('#BusquedaJBEJBICC').dialog({
                modal: true,
                autoOpen: false,
                title: "JBEJBICC",
                width: "600px"
            });
            $('#BusquedaJBEJBICC').parent().appendTo("form");
            $('#nuevoProducto').click(mostrarRegistroNuevoProducto);
            validationForm();
            $('select').listaConFiltro();
        }
        function mostrarProducto() {
            $('#visualizadorDivs').animate({ scrollLeft: '0px' });
            $($('#wizardContainer .itemWizard')[0]).removeClass('itemWizardInactivo');
            $($('#wizardContainer .itemWizard')[0]).addClass('itemWizardActivo');
            $($('#wizardContainer .itemWizard img')[0]).remove();
            $($('#wizardContainer .itemWizard')[1]).remove();
        }
        function mostrarCausa() {
            $('#visualizadorDivs').animate({ scrollLeft: '880px' });
            $('#wizardContainer').append("<div class='itemWizard itemWizardActivo'><label>Causa</label></div>")
            $($('#wizardContainer .itemWizard label')[0]).append("<img src='../Images/Select_16.png'/>");
            $($('#wizardContainer .itemWizard')[0]).removeClass('itemWizardActivo');
            $($('#wizardContainer .itemWizard')[0]).addClass('itemWizardInactivo');
        }
        function mostrarRechazarProducto() {
            $('#visualizadorDivs').animate({ scrollLeft: '1760px' });
            $($('#wizardContainer .itemWizard')[0]).removeClass('itemWizardActivo');
            $($('#wizardContainer .itemWizard')[0]).addClass('itemWizardInactivo');
            $($('#wizardContainer .itemWizard label')[0]).append("<img src='../Images/Select_16.png'/>");
            $('#wizardContainer').append("<div class='itemWizard itemWizardActivo''><label>Rechaza</label></div>")
        }
        function mostrarMensajeRechaza() {
            $('#visualizadorDivs').animate({ scrollLeft: '2640px' });
            $($('#wizardContainer .itemWizard')[1]).removeClass('itemWizardActivo');
            $($('#wizardContainer .itemWizard')[1]).addClass('itemWizardInactivo');
            $($('#wizardContainer .itemWizard label')[1]).append("<img src='../Images/Select_16.png'/>");
            $('#wizardContainer').append("<div class='itemWizard'><label>Confirmación<img src='../Images/Select_16.png'/></label></div>")
        }
        function mostrarMensajeGrabacionCausa() {
            $('#visualizadorDivs').animate({ scrollLeft: '3520px' });
            $($('#wizardContainer .itemWizard')[1]).removeClass('itemWizardActivo');
            $($('#wizardContainer .itemWizard')[1]).addClass('itemWizardInactivo');
            $($('#wizardContainer .itemWizard label')[1]).append("<img src='../Images/Select_16.png'/>");
            $('#wizardContainer').append("<div class='itemWizard'><label>Confirmación<img src='../Images/Select_16.png'/></label></div>")
        }
        function mostrarMensajeNuevoProducto() {
            $('#visualizadorDivs2').animate({ scrollLeft: '870px' });
        }
        function mostrarRegistroNuevoProducto() {
            $('#visualizadorDivs2').animate({ scrollLeft: '0px' });
        }
        function mostrarRespuestaProductoEnviado() {
            $('#visualizadorDivs3').animate({ scrollLeft: '870px' });
        }
        function mostrarProductoEnviado() {
            $('#visualizadorDivs3').animate({ scrollLeft: '0px' });
        }
        function MostrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("open");
        }

        function CerrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("close");
        }
        function actualizarCantRecibidos(cant) {
            $("#<%= cantProdRecibibos.ClientId%>").text(cant);
        }
        function actualizarCantEnviados(cant) {
            $("#<%= cantProdEnviados.ClientId%>").text(cant);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Producto no conforme</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="tabs">
        <ul>
            <li><a id="nuevoProducto" href="#tabs-1">Nuevo</a></li>
            <li><a href="#tabs-2">Recibidos<span class="badge" runat="server" id="cantProdRecibibos"></span></a></li>
            <li><a href="#tabs-3">Enviados<span class="badge" runat="server" id="cantProdEnviados"></span></a></li>
        </ul>
        <div id="tabs-2" style="overflow-y: auto">
            <asp:Panel ID="pnlProductos" runat="server">
                <div style="width: 90%; margin-left: auto; margin-right: auto">
                    <asp:UpdatePanel ID="upProductos" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false" PageSize="2"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id" AllowPaging="true" EmptyDataText="No se encuentran registros">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ShowHeader="false">
                                        <ItemTemplate>
                                            <asp:Button ID="btnVer" runat="server" Text="Ver" Width="60px" Height="25px" CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Registro" />
                                    <asp:BoundField DataField="asociadoA" HeaderText="Asociado A" />
                                    <asp:BoundField DataField="proyectoTrabajo" HeaderText="Nombre proyecto / trabajo" />
                                    <asp:BoundField DataField="proceso" HeaderText="proceso" />
                                    <asp:BoundField DataField="procedimiento" HeaderText="procedimiento" />
                                    <asp:BoundField DataField="fuente" HeaderText="fuente" />
                                    <asp:BoundField DataField="personaIdentifica" HeaderText="personaIdentifica" />

                                </Columns>
                                <PagerTemplate>
                                    <div class="pagingButtons">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                </td>
                                                <td><span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span> </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>

            <asp:UpdatePanel ID="upWizard" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlWizard" runat="server" Visible="false">
                        <div style="margin-top: 10px; margin-left: auto; margin-right: auto;">
                            <hr />
                            <div id="wizardContainer">
                                <div class="itemWizard">
                                    <label>Producto</label>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                            <hr />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="upProductosCausas" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlProductosCausas" runat="server" Visible="false">
                        <div id="visualizadorDivs" style="position: relative; margin: auto; overflow: hidden; margin-top: 5px">
                            <div id="contenedorDivs" style="width: 4400px; position: relative">
                                <div id="producto" style="float: left; width: 870px; margin-right: 10px">
                                    <div style="margin-top: 10px; margin-left: auto; margin-right: auto">
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Asociado A:
                                                </label>
                                                <asp:Label ID="lblAsociadoA" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Nombre:
                                                </label>
                                                <asp:Label ID="lblNombreProyectoTrabajo" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Proceso:
                                                </label>
                                                <asp:Label ID="lblProceso" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Procedimiento:
                                                </label>
                                                <asp:Label ID="lblProcedimiento" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Fecha reclamo:
                                                </label>
                                                <asp:Label ID="lblFechaReclamo" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Fuente reclamo:
                                                </label>
                                                <asp:Label ID="lblFuente" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Categoría:
                                                </label>
                                                <asp:Label ID="lblCategoria" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Tarea:
                                                </label>
                                                <asp:Label ID="lblTarea" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Unidad:
                                                </label>
                                                <asp:Label ID="lblUnidad" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Quien identifica el PNC:
                                                </label>
                                                <asp:Label ID="lblIdentifica" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Responsable:
                                                </label>
                                                <asp:Label ID="lblResponsable" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Quien debe ser informado:
                                                </label>
                                                <asp:Label ID="lblInformar" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div class="form_left" style="margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Descripción:
                                                </label>
                                                <asp:Label ID="lblDescripcion" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div style="margin-left: auto; margin-right: auto; width: 30%">
                                            <div style="width: 40%; float: left">
                                                <asp:Button ID="btnAceptar" Text="Aceptar" runat="server" CssClass="button" OnClientClick="mostrarCausa();return false;" />
                                            </div>
                                            <div style="width: 40%; margin-left: 10px; float: left">
                                                <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" CssClass="button" OnClientClick="mostrarRechazarProducto();return false;" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="causa" style="float: left; width: 870px; margin-right: 10px">
                                    <fieldset class="validationGroup">
                                        <div>
                                            <label for="txtCausa">Causa</label>
                                            <asp:TextBox ID="txtCausa" runat="server" Rows="5" TextMode="MultiLine" Width="98%" CssClass="required"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label for="txtCorreccion">Corrección de la causa</label>
                                            <asp:TextBox ID="txtCorreccion" runat="server" Rows="5" TextMode="MultiLine" Width="98%" CssClass="required"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label for="txtFechaEstimadaCierre">Fecha estimada de cierre</label>
                                            <asp:TextBox ID="txtFechaEstimadaCierre" runat="server" CssClass="required"></asp:TextBox>
                                        </div>
                                        <div style="margin-left: auto; margin-right: auto; width: 25%">
                                            <div style="margin-right: 10px; float: left; width: 40%">
                                                <asp:Button ID="btnRegresarProducto" runat="server" Text="Regresar" OnClientClick="mostrarProducto();return false;" />
                                            </div>
                                            <div style="margin-right: 10px; float: left; width: 40%">
                                                <asp:Button ID="btnGrabarCausa" runat="server" Text="Grabar" CssClass="causesValidation" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <div id="rechazaProducto" style="float: left; width: 870px; margin-right: 10px">
                                    <fieldset class="validationGroup">
                                        <div>
                                            <label for="txtRechazo">Razones de rechazo</label>
                                            <asp:TextBox ID="txtRechazo" runat="server" Rows="5" TextMode="MultiLine" Width="98%" CssClass="required"></asp:TextBox>
                                        </div>
                                        <div style="margin-left: auto; margin-right: auto; width: 25%">
                                            <div style="margin-right: 10px; float: left; width: 40%">
                                                <asp:Button ID="btnRegresarDesdeRechazo" runat="server" Text="Regresar" OnClientClick="mostrarProducto();return false;" />
                                            </div>
                                            <div style="margin-right: 10px; float: left; width: 40%">
                                                <asp:Button ID="btnGrabarRechazo" runat="server" Text="Grabar" CssClass="causesValidation" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <div id="infoRechazaProducto" style="float: left; width: 870px; margin-right: 10px">
                                    <div style="margin-left: auto; margin-right: auto; width: 60%">
                                        <img src="../Images/1490154973_checkmark-24.png" alt="Producto rechazado" />
                                        Se ha registrado y notificado el rechazo
                                    </div>
                                </div>
                                <div id="infoGrabacionCausa" style="float: left; width: 870px; margin-right: 10px">
                                    <div style="margin-left: auto; margin-right: auto; width: 60%">
                                        <img src="../Images/1490154973_checkmark-24.png" alt="Causa registrada con exito" />
                                        Causa registrada con exito
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="tabs-1" style="overflow-y: auto">
            <div id="visualizadorDivs2" style="position: relative; margin: auto; overflow: hidden; margin-top: 5px">
                <div id="contenedorDivs2" style="width: 1760px; position: relative">
                    <asp:UpdatePanel ID="upDiligenciarProducto" runat="server">
                        <ContentTemplate>
                            <div id="productoDiligenciar" style="float: left; width: 870px; margin-right: 10px">
                                <fieldset class="validationGroup">
                                    <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                        <fieldset>
                                            <label>Asociado a:</label>
                                            <asp:DropDownList ID="ddlAsociadoA" runat="server" AutoPostBack="true" CssClass="mySpecificClass">
                                                <asp:ListItem Text="--Seleccione--" Value="-1" />
                                                <asp:ListItem Text="Actividad" Value="3" />
                                                <asp:ListItem Text="JBE" Value="1" />
                                                <asp:ListItem Text="JBI" Value="2" />
                                            </asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Procedimiento</label>
                                            <asp:DropDownList ID="ddlProcedimientos" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Fuente del reclamo</label>
                                            <asp:DropDownList ID="ddlFuente" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Responsable</label>
                                            <asp:DropDownList ID="ddlResponsable" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                    </div>
                                    <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                        <fieldset>
                                            <asp:Panel ID="pnlBuscarJBIJBE" runat="server" Visible="false">
                                                <asp:Button ID="btnBuscarProyectoEstudio" runat="server" Text="Buscar" OnClientClick="MostrarJBEJBICC();return false;" />
                                                <br />
                                                <asp:Label ID="lblId_PT" runat="server" Visible="false"></asp:Label>
                                                <br />
                                                <asp:Label ID="lblNombre_PT" runat="server"></asp:Label>
                                            </asp:Panel>
                                        </fieldset>
                                        <fieldset>
                                            <label>Unidad</label>
                                            <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Categoría</label>
                                            <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Quien debe ser informado</label>
                                            <asp:DropDownList ID="ddlInformar" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                    </div>
                                    <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                        <fieldset>
                                            <asp:Label ID="Label3" runat="server" Text="Proceso" AssociatedControlID="ddlProcesos"></asp:Label>
                                            <asp:DropDownList ID="ddlProcesos" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Quien identifica el PNC</label>
                                            <asp:DropDownList ID="ddlIdentifica" runat="server" CssClass="mySpecificClass"></asp:DropDownList>
                                        </fieldset>
                                        <fieldset>
                                            <label>Tarea</label>
                                            <asp:DropDownList ID="ddlTarea" runat="server"></asp:DropDownList>
                                        </fieldset>
                                        <div class="formOrder">
                                            <asp:Label ID="Label7" runat="server" Text="Fecha del reclamo" AssociatedControlID="txtFechaReclamo"></asp:Label>
                                            <asp:TextBox ID="txtFechaReclamo" runat="server" CssClass="required"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="form_left" style="width: 100%; margin-left: 10px; margin-right: 10px;">
                                        <fieldset>
                                            <label>Descripción</label>
                                            <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="8" CssClass="required" Width="98%"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div style="margin-left: auto; margin-right: auto; width: 10%">
                                        <asp:Button ID="btnEnviar" Text="Enviar" runat="server" CssClass="causesValidation" />
                                    </div>
                                </fieldset>
                            </div>
                            <div id="infoGrabacionNuevoProducto" style="float: left; width: 870px; margin-right: 10px">
                                <div style="margin-left: auto; margin-right: auto; width: 60%">
                                    <img src="../Images/1490154973_checkmark-24.png" alt="Causa registrada con exito" />
                                    Producto no conforme, registrado y notificado con exito
                                </div>
                                <div style="margin-left: auto; margin-right: auto; width: 18%">
                                    <asp:Button ID="btnRegistrarNuevoProducto" runat="server" Text="Registrar nuevo" OnClientClick="mostrarRegistroNuevoProducto();return false;" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="BusquedaJBEJBICC">
                <asp:UpdatePanel ID="upJBEJBICC" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Búsqueda" Width="176px"></asp:TextBox>
                        <asp:Button ID="btnBuscarJBEJBICC" Text="Buscar" runat="server" />
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
        </div>
        <div id="tabs-3" style="overflow-y: auto">
            <asp:UpdatePanel ID="upProductosEnviados" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvProductosEnviados" runat="server" AutoGenerateColumns="false" PageSize="2"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="true" EmptyDataText="No se encuentran registros">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:TemplateField HeaderText="" ShowHeader="false">
                                <ItemTemplate>
                                    <asp:Button ID="btnVer" runat="server" Text="Ver" Width="60px" Height="25px" CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Registro" />
                            <asp:BoundField DataField="asociadoA" HeaderText="Asociado A" />
                            <asp:BoundField DataField="proyectoTrabajo" HeaderText="Nombre proyecto / trabajo" />
                            <asp:BoundField DataField="proceso" HeaderText="proceso" />
                            <asp:BoundField DataField="procedimiento" HeaderText="procedimiento" />
                            <asp:BoundField DataField="fuente" HeaderText="fuente" />
                            <asp:BoundField DataField="personaIdentifica" HeaderText="personaIdentifica" />

                        </Columns>
                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvProductosEnviados.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvProductosEnviados.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td><span class="pagingLinks">[<%= gvProductosEnviados.PageIndex + 1%>-<%= gvProductosEnviados.PageCount%>]</span> </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvProductosEnviados.PageIndex + 1) = gvProductosEnviados.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvProductosEnviados.PageIndex + 1) = gvProductosEnviados.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:Panel ID="pnlProductoEnviado" runat="server" Visible="false">
                        <div id="visualizadorDivs3" style="position: relative; margin: auto; overflow: hidden; margin-top: 5px">
                            <div id="contenedorDivs3" style="width: 4400px; position: relative">
                                <div id="productoEnviado" style="float: left; width: 850px; margin-right: 10px">
                                    <div style="margin-top: 10px; margin-left: auto; margin-right: auto">
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Asociado A:
                                                </label>
                                                <asp:Label ID="lbl_PE_AsociadoA" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Nombre:
                                                </label>
                                                <asp:Label ID="lbl_PE_Nombre" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Proceso:
                                                </label>
                                                <asp:Label ID="lbl_PE_Proceso" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Procedimiento:
                                                </label>
                                                <asp:Label ID="lbl_PE_Procedimiento" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Fecha reclamo:
                                                </label>
                                                <asp:Label ID="lbl_PE_FechaReclamo" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Fuente reclamo:
                                                </label>
                                                <asp:Label ID="lbl_PE_Fuente" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Categoría:
                                                </label>
                                                <asp:Label ID="lbl_PE_Categoria" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Tarea:
                                                </label>
                                                <asp:Label ID="lbl_PE_Tarea" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div class="form_left" style="width: 30%; margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Unidad:
                                                </label>
                                                <asp:Label ID="lbl_PE_Unidad" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Quien identifica el PNC:
                                                </label>
                                                <asp:Label ID="lbl_PE_Identifica" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Responsable:
                                                </label>
                                                <asp:Label ID="lbl_PE_Responsable" runat="server" />
                                            </fieldset>
                                            <fieldset>
                                                <label>
                                                    Quien debe ser informado:
                                                </label>
                                                <asp:Label ID="lbl_PE_Informar" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div class="form_left" style="margin-left: 10px; margin-right: 10px;">
                                            <fieldset>
                                                <label>
                                                    Descripción:
                                                </label>
                                                <asp:Label ID="lbl_PE_Descripcion" runat="server" />
                                            </fieldset>
                                        </div>
                                        <div style="margin-left: auto; margin-right: auto; width: 25%">
                                            <div style="margin-right: 10px; float: left; width: 40%">
                                                <asp:Button ID="btnMostrarRespuestaProductoEnviado" runat="server" Text="Ver respuesta" OnClientClick="mostrarRespuestaProductoEnviado();return false;" />
                                            </div>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div style="margin-left: auto; margin-right: auto; width: 30%; color: red;">
                                            <asp:Label ID="lblRegistroCausa" runat="server" Text="Aún no se ha registrado respuesta!"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div id="respuestaProductoEnviado" style="float: left; width: 870px; margin-right: 10px">
                                    <div id="divRespuesta" style="float: left; width: 870px; margin-right: 10px" runat="server" visible="false">
                                        <div style="margin-top: 10px; margin-left: auto; margin-right: auto">
                                            <div class="form_left" style="margin-left: 10px; margin-right: 10px;">  
                                                <fieldset>
                                                    <label>
                                                        Causa:
                                                    </label>
                                                    <asp:Label ID="lblCausa" runat="server" />
                                                </fieldset>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="form_left" style="margin-left: 10px; margin-right: 10px;">
                                                <fieldset>
                                                    <label>
                                                        Corrección:
                                                    </label>
                                                    <asp:Label ID="lblCorreccion" runat="server" />
                                                </fieldset>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="form_left" style="margin-left: 10px; margin-right: 10px;">
                                                <fieldset>
                                                    <label>
                                                        Fecha estimada de cierre:
                                                    </label>
                                                    <asp:Label ID="lblFechaEstimadaCierre" runat="server" />
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divRechazo" style="float: left; width: 870px; margin-right: 10px" runat="server" visible="false">
                                        <div style="margin-top: 10px; margin-left: auto; margin-right: auto">
                                            <div class="form_left" style="margin-left: 10px; margin-right: 10px;">
                                                <fieldset>
                                                    <label>
                                                        Rechazo:
                                                    </label>
                                                    <asp:Label ID="lblRechazo" runat="server" />
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div style="margin-left: auto; margin-right: auto; width: 25%">
                                        <div style="margin-right: 10px; float: left; width: 40%">
                                            <asp:Button ID="btnMostrarProductoEnviado" runat="server" Text="Volver al producto" OnClientClick="mostrarProductoEnviado();return false;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
