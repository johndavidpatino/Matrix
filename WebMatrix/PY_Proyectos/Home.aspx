<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="../MasterPage/NewSite.master" CodeBehind="Home.aspx.vb" Inherits="WebMatrix.Home3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu">
    <li>
        <a href="#">PROYECTOS</a>
        <ul>
            <li><a title="Lista de proyectos asignados" href="../PY_Proyectos/PY_Proyectos.aspx">Listado de Proyectos</a></li>
            <li><a title="Tráfico de Tareas" href="../CORE/ListaTareas-Trafico.aspx?FiltrarTareasPor=2&Permiso=38">Tráfico de Tareas</a></li>
            <li><a title="Cuali - Registro planilla" href="../PY_Proyectos/RegistroPlanillasCualitativo.aspx">Cuali - Registro planilla</a></li>
        </ul>
    </li>
    <li>
        <a href="#">GESTIÓN</a>
        <ul>
            <li><a title="Asignación de gerente de proyectos" href="../PY_Proyectos/AsignacionProyectos.aspx">Asignar Gerente de Proyectos</a></li>
            <li><a title="Asignación de gerente de proyectos" href="../PY_Proyectos/REAsignacionProyectos.aspx">Cambiar Gerente de Proyectos</a></li>
            <li><a title="Ver listado de proyectos" href="../RP_Reportes/TrabajosPorGrupoBU.aspx">Listado de trabajos de la unidad</a></li>
            <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=35">Trafico tareas</a></li>
            <li><a href="../RP_Reportes/ListadoPlaneacionUnidades.aspx" target="_blank">Planeación OPS</a></li>
        </ul>
    </li>
    <li>
        <a href="../Home/Default.aspx">IR AL INICIO</a>
    </li>
</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Proyectos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Content" runat="server">
    En esta sección puede gestionar sus proyectos. Se incluye el Brief de Cuentas a Proyectos, el Instructivo 
    General del Trabajo, Asignaciones, entre otras tareas para la ejecución y seguimiento del proyecto
    <br />
</asp:Content>
