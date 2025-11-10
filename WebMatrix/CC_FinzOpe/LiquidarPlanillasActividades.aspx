<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="LiquidarPlanillasActividades.aspx.vb" Inherits="WebMatrix.LiquidarPlanillasActividades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
        <li>
        <a href="../FI_AdministrativoFinanciero/Default.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Volver al menú
        </a>
    </li>
    <li>
    <a href="../CC_FinzOpe/LiquidarPlanillasActividades.aspx" class="nav-link text-primary">
        <i class="metismenu-icon fa fa-table"></i>
        Liquidar Planillas
    </a>
</li>
        <li>
    <a href="../CC_FinzOpe/LiquidarProductividadPST.aspx" class="nav-link text-success">
        <i class="metismenu-icon fa fa-table"></i>
        Liquidar Productividad
    </a>
</li>
        <li>
    <a href="../Home/Home.aspx" class="nav-link">
        <i class="metismenu-icon fa fa-home"></i>
        Ir a inicio
    </a>
</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Planillas aprobadas para Liquidación de Producción
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    Aquí puede ver las planillas que han sido aprobadas previamente
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
                    <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" />
                    <asp:BoundField DataField="TipoActividad" HeaderText="Actividad" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

    <asp:Panel ID="pnlResultados" runat="server">
     <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Planillas Pendientes Revision PMO</h5>
            <p class="card-subtitle">Estas son las planillas que tienen pendiente una revisión por parte del PMO</p>
            <div>
                <asp:GridView ID="gvDataPendientes" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                    <Columns>
                        <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" Visible="false"/>
                        <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" Visible="false"/>
                        <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" />
                        <asp:BoundField DataField="PeriodoInicial" HeaderText="Periodo Inicial" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:BoundField DataField="PeriodoFinal" HeaderText="Periodo Final" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:BoundField DataField="Registros" HeaderText="Registros" />
                    </Columns>
                </asp:GridView>
                <asp:Button runat="server" ID="btnRechazar" class="btn btn-danger" Text="Rechazar registros" Visible="false"></asp:Button>
            </div>
        </div>
    </div>
         <div class="main-card mb-3 card">
    <div class="card-body">
        <h5 class="card-title">Planillas Para Liquidar</h5>
        <p class="card-subtitle">Estas son las planillas que fueron aprobadas por el PMO y que están listas para ser liquidadas</p>
        <div>
            <asp:GridView ID="gvDataToLiquidar" runat="server" AutoGenerateColumns="false" DataKeyNames="TrabajoId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay registros">
                <Columns>
                    <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" Visible="false"/>
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="Trabajo" HeaderStyle-Wrap="true" Visible="false"/>
                    <asp:BoundField DataField="PMO" HeaderText="PMO" HeaderStyle-Wrap="true" />
                    <asp:BoundField DataField="PeriodoInicial" HeaderText="Periodo Inicial" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:BoundField DataField="PeriodoFinal" HeaderText="Periodo Final" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:BoundField DataField="Registros" HeaderText="Registros" />
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" ID="btnLiquidar" class="btn btn-primary" Text="Pasar registros a producción" OnClick="btnLiquidar_Click" OnClientClick="return confirm('Confirmas que vas a liquidar estos registros y pasarlos a producción?')"></asp:Button>
        </div>
    </div>
</div>
        </asp:Panel>
</asp:Content>
