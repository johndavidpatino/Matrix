<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="RevisionPlanillas.aspx.vb" Inherits="WebMatrix.RevisionPlanillas" %>

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
    Revisión de Planillas para Liquidación de Producción
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    En este paso requerimos que seleccione cada uno de los trabajos para ir aprobando o rechazando lo cargado
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
            <label id="lbltextError">
            </label>
        </p>
    </div>
    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
    <asp:HiddenField ID="hfidMetodologia" runat="server" />
    <asp:HiddenField ID="hfNew" runat="server" />
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Trabajos sin presupuestos</h5>
            <p class="card-subtitle">Estos son los trabajos para los que no hay presupuesto autorizado</p>
            <div>
                <asp:GridView ID="gvSinPresupuestos" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="Todos los trabajos cargados en las planillas tienen presupuesto">
                    <Columns>
                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" Visible="false" />
                        <asp:BoundField DataField="TipoActividad" HeaderText="Actividad" />
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
                <h5 class="card-title">Registros</h5>
                <p class="card-subtitle">Estos son todos los registros que han sido cargados</p>
                <div>
                    <asp:GridView ID="gvDataSearch" runat="server" AutoGenerateColumns="false" DataKeyNames="id" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                            <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" Visible="false" />
                            <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" Visible="false" />
                            <asp:BoundField DataField="NombrePersona" HeaderText="Nombre Persona" HeaderStyle-Wrap="true" />
                            <asp:BoundField DataField="PersonaId" HeaderText="Cedula" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Res_Fecha" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy}" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <asp:BoundField DataField="TipoActividadDescripcion" HeaderText="Actividad" />
                            <asp:BoundField DataField="UsuarioCarga" HeaderText="Subido Por" />
                            <asp:BoundField DataField="FechaCarga" HeaderText="FechaCarga" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnAprobar" class="btn btn-primary" Text="Aprobar registros" OnClick="btnAprobar_Click" OnClientClick="return confirm('Confirmas la aprobación de los registros mostrados?')"></asp:Button>
                    <asp:Button runat="server" ID="btnRechazar" class="btn btn-danger" Text="Rechazar registros" OnClick="btnRechazar_Click" OnClientClick="return confirm('Confirmas que rechazas estos registros? Esta acción no se puede deshacer y la planilla deberá volver a subirse')"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
