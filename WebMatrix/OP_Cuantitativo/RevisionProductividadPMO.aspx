<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="RevisionProductividadPMO.aspx.vb" Inherits="WebMatrix.RevisionProductividadPMO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="../op_cuantitativo/Trabajos.aspx" class="nav-link text-primary">
            <i class="metismenu-icon fa fa-search"></i>
            Ir a Trabajos
        </a>
    </li>
    <li>
        <a href="../op_cuantitativo/RevisionPlanillas.aspx" class="nav-link text-secondary">
            <i class="metismenu-icon fa fa-table"></i>
            Revisar planillas
        </a>
    </li>
    <li>
        <a href="../op_cuantitativo/PlanillasRevisadas.aspx" class="nav-link text-success">
            <i class="metismenu-icon fa fa-table"></i>
            Ver planillas aprobadas
        </a>
    </li>
    <li>
        <a href="../op_cuantitativo/RevisionProductividadPMO.aspx" class="nav-link text-secondary">
            <i class="metismenu-icon fa fa-list"></i>
            Revisar Productividad
        </a>
    </li>
    <li>
        <a href="../op_cuantitativo/ProductividadRevisadaPMO.aspx" class="nav-link text-success">
            <i class="metismenu-icon fa fa-list"></i>
            Productividad Revisada
        </a>
    </li>
    <li>
        <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link text-primary">
            <i class="metismenu-icon fa fa-bookmark"></i>
            Ir al menu
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Revisión productividad para el corte actual - PMO
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    Aquí se pueden ver las los registros de productividad sincronizados para el presente corte del <strong>
        <asp:Label ID="lblIni" runat="server"></asp:Label>
        al
        <asp:Label ID="lblFin" runat="server"></asp:Label></strong>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Content" runat="server">
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
            <h2>
                <label id="lbltextError">
                </label>
            </h2>
        </p>
    </div>
    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
    <asp:HiddenField ID="hfidMetodologia" runat="server" />
    <asp:HiddenField ID="hfNew" runat="server" />
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Estatus de trabajos</h5>
            <p class="card-subtitle">Verifique los presupuestos</p>
            <div>
                <asp:GridView ID="gvEstatus" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No tiene trabajos en este corte">
                    <Columns>
                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                        <asp:BoundField DataField="StatusPresupuesto" HeaderText="Presupuesto" />
                        <asp:BoundField DataField="VrUnitario" HeaderText="Vr. Encuesta" DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="Cargo" HeaderText="Actividad" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Trabajos</h5>
            <p class="card-subtitle">Seleccione un trabajo para ver los registros cargados</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-4 mb-2">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Trabajo</button>
                        </div>
                        <asp:DropDownList ID="ddlTrabajoSeleccionado" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTrabajoSeleccionado_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>

            </div>

        </div>
    </div>
    <asp:Panel ID="pnlResultados" runat="server" Visible="false">
        <div class="main-card mb-3 card">
            <div class="card-body">
                <h5 class="card-title">Montos</h5>
                <div>
                    <div class="form-row">
                        <div class="col-md-4 mb-2">
                            <h3>Monto Previo</h3>
                            <asp:GridView ID="gvMontoPrevio" runat="server" AutoGenerateColumns="false" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0">
                                <Columns>
                                    <asp:BoundField DataField="RowNum" HeaderText="" Visible="false" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="#" />
                                    <asp:BoundField DataField="Monto" HeaderText="$" DataFormatString="{0:C0}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-1 mb-2">
                        </div>
                        <div class="col-md-4 mb-2">
                            <h3>Monto Actual</h3>
                            <asp:GridView ID="gvMontoActual" runat="server" AutoGenerateColumns="false" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0">
                                <Columns>
                                    <asp:BoundField DataField="RowNum" HeaderText="" Visible="false" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="#" />
                                    <asp:BoundField DataField="Monto" HeaderText="$" DataFormatString="{0:C0}" />
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>

                </div>
            </div>
            <div class="card-body">
                <h5 class="card-title">Registros</h5>
                <p class="card-subtitle">Estos son todos los registros que han sido cargados</p>
                <div>
                    <asp:GridView ID="gvDataSearch" runat="server" AutoGenerateColumns="false" DataKeyNames="id" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                            <asp:BoundField DataField="Nombre" HeaderText="Persona" ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="Cedula" HeaderText="Cedula" HeaderStyle-Wrap="true" />
                            <asp:BoundField DataField="FechaEjecucion" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <asp:BoundField DataField="CantidadCoordinador" HeaderText="# Coord." />
                            <asp:BoundField DataField="ObservacionesCoordinador" HeaderText="Obs Coord." ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="CantidadJefe" HeaderText="# Jefe" />
                            <asp:BoundField DataField="ObservacionesJefe" HeaderText="Obs Jefe" ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />

                            <asp:BoundField DataField="Coordinador" HeaderText="Coord" />
                            <asp:BoundField DataField="StatusPresupuesto" HeaderText="Presupuesto" />
                            <asp:TemplateField HeaderText="# Encuestas">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCantidad" Text='<%# IIf(IsNumeric(Eval("CantidadJefe")), Eval("CantidadJefe"), Eval("Cantidad")) %>' runat="server" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Observaciones">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="100"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnGuardar" class="btn btn-primary" Text="Guardar cambios" OnClick="btnGuardar_Click" OnClientClick="return confirm('Confirmas que vas a guardar los cambios? Esta acción no se puede deshacer y los registros pasarán al siguiente nivel de aprobación')"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
