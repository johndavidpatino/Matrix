<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterREP.master" CodeBehind="ReportesCumplimientoTareas.aspx.vb" Inherits="WebMatrix.ReportesCumplimientoTareas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://blacklabel.github.io/grouped_categories/css/styles.css" type="text/css" />
    <script src="https://code.highcharts.com/10.0/highcharts.js" type="text/javascript" language="javascript"></script>
    <script src="https://blacklabel.github.io/grouped_categories/grouped-categories.js" type="text/javascript" language="javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
    <%--<script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordionDataDetGP').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });

            $('#accordionDataGP').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });
            $('#accordionDataDetCOE').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });

            $('#accordionDataCOE').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });
        });
        function bloquearUI() {
            $.blockUI({ message: "Procesando ...." });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Indicadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Indicador de cumplimiento
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    Esta gráfica muestra el indicador de cumplimiento, de acuerdo a las estimaciones y ejecuciones en el modulo de tareas
    <br />
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#tabs').tabs();

            $('#accordionDataDetCOE').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });

            $('#accordionDataCOE').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });
            $('#accordionDataDetGP').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });

            $('#accordionDataGP').accordion({
                autoHeight: false,
                header: "h2",
                active: 0
            });
        }
    </script>
    <style>
        .text-center {
            margin: 0px auto;
            text-align: center;
        }
    </style>
    <asp:Label ID="lblAno" Text="Año" runat="server" AssociatedControlID="ddlAno"></asp:Label>
    <asp:DropDownList ID="ddlAno" runat="server" CssClass="form-control">
    </asp:DropDownList>
    <asp:Label ID="lblMes" Text="Mes" runat="server" AssociatedControlID="ddlMes"></asp:Label>
    <asp:DropDownList ID="ddlMes" runat="server" CssClass="form-control">
        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
        <asp:ListItem Text="Enero" Value="1"></asp:ListItem>
        <asp:ListItem Text="Febrero" Value="2"></asp:ListItem>
        <asp:ListItem Text="Marzo" Value="3"></asp:ListItem>
        <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
        <asp:ListItem Text="Mayo" Value="5"></asp:ListItem>
        <asp:ListItem Text="Junio" Value="6"></asp:ListItem>
        <asp:ListItem Text="Julio" Value="7"></asp:ListItem>
        <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
        <asp:ListItem Text="Septiembre" Value="9"></asp:ListItem>
        <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
        <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
        <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
    </asp:DropDownList>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label ID="lblProcesos" Text="Procesos" runat="server" AssociatedControlID="ddlProcesos"></asp:Label>
            <asp:DropDownList ID="ddlProcesos" runat="server" AutoPostBack="true" CssClass="form-control">
            </asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upTareas" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblTareas" Text="Tareas" runat="server" AssociatedControlID="ddlTareas"></asp:Label>
            <asp:DropDownList ID="ddlTareas" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblVerPor" Text="Ver por:" runat="server" AssociatedControlID="ddlVerPor"></asp:Label>
    <asp:DropDownList ID="ddlVerPor" runat="server" CssClass="form-control">
        <asp:ListItem Text="Proceso" Value="1"></asp:ListItem>
        <asp:ListItem Text="Tarea" Value="3"></asp:ListItem>
        <asp:ListItem Text="Persona asignada" Value="4"></asp:ListItem>
    </asp:DropDownList>
    <asp:UpdatePanel ID="upActualizar" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnAcualizar" runat="server" Text="Actualizar" CssClass="form-control" />
        </ContentTemplate>
    </asp:UpdatePanel> 
    <div style="clear: both;"></div>

    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel ID="upGraficaDatos" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="contenedorGrafica" runat="server">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-md-12">
            <asp:UpdatePanel runat="server" ID="updDatos" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlData" Visible="false">
                        <div style="margin-top: 30px">
                            <div id="accordionDataGP">
                                <h2>Datos Agrupados Persona Asignada</h2>
                                <div style="max-width: 100%; min-height: 300px; max-height: 300px;">
                                    <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="true"></asp:GridView>
                                </div>
                            </div>

                            <br />
                            <div id="accordionDataDetGP">
                                <h2>Detalles Por Usuario Persona Asignada</h2>
                                <div style="max-width: 100%; min-height: 300px; max-height: 300px;">
                                    <asp:Button Text="Exportar a Excel" runat="server" ID="btnExcelDetalle" />
                                    <br />
                                    <asp:GridView ID="gvDatosDetalle" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="id" AllowPaging="false" ShowHeader="true" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="Código" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="HiloId" HeaderText="Hilo" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Tarea" HeaderText="Tarea" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="ProyectoId" HeaderText="Proyecto" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Cumplimiento" HeaderText="Cumplimiento" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Ano" HeaderText="Año" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Mes" HeaderText="Mes" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="FIniP" HeaderText="Inicio Planeación" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FIniR" HeaderText="Inicio Ejecución" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FFinP" HeaderText="Fin Planeación" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FFinR" HeaderText="Fin Ejecución" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </div>

                        </div>
                        <div style="margin-top: 30px">
                            <div id="accordionDataCOE">
                                <h2>Datos Agrupados OMP</h2>
                                <div style="max-width: 100%; min-height: 300px; max-height: 300px;">
                                    <asp:GridView ID="gvDatosCOE" runat="server" AutoGenerateColumns="true"></asp:GridView>
                                </div>
                            </div>

                            <br />
                            <div id="accordionDataDetCOE">
                                <h2>Detalles Por Usuario OMP</h2>
                                <div style="max-width: 100%; min-height: 300px; max-height: 300px;">
                                    <asp:Button Text="Exportar a Excel" runat="server" ID="btnExcelDetalleCOE" />
                                    <br />
                                    <asp:GridView ID="gvDatosDetalleCOE" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="id" AllowPaging="false" ShowHeader="true" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="Código" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="HiloId" HeaderText="Hilo" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Tarea" HeaderText="Tarea" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="ProyectoId" HeaderText="Proyecto" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Cumplimiento" HeaderText="Cumplimiento" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Ano" HeaderText="Año" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Mes" HeaderText="Mes" Visible="true" ItemStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="FIniP" HeaderText="Inicio Planeación" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FIniR" HeaderText="Inicio Ejecución" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FFinP" HeaderText="Fin Planeación" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FFinR" HeaderText="Fin Ejecución" Visible="true" ItemStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </div>

                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>
