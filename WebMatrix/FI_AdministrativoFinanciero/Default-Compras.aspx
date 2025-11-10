<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Default-Compras.aspx.vb" Inherits="WebMatrix._DefaultCO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior" title="Anterior"></a></div>
    <div id="slider">
        <div class="slidesContainer">
            <nav class="slide">
       
<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Gestión de Ordenes</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión Ordenes" href="Gestion-Ordenes.aspx">Ordenes Compra y Servicio</a></li>            
        <li><a title="Busqueda / Consulta" href="ReporteOrdenesFacturas.aspx">Busqueda / Consulta</a></li>
        <li><a title="Gestión Ordenes" href="Gestion-Ordenes1.aspx">Ordenes Compra, Servicio y Requerimientos</a></li>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->
<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Facturas</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/ejecutivo.png" width="65" height="65" alt="polea"/></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a></a></li>
        <li><a title="Recepción" href="Recepcion-Facturas.aspx">Recepcion Facturas</a></li>
        <li><a title="Reporte Órdenes" href="ReporteOrdenesFacturas.aspx">Reporte Órdenes</a></li>
        <li><a></a></li>
        <li><a title="Reporte Facturas" href="ReporteFacturasRadicadas.aspx">Reporte Facturas</a></li>
        <li><a title="Detalle de Requerimientos" href="DetalleRequerimientos.aspx">Detalle de Requerimientos</a></li>
        <li><a></a></li>            
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Aprobaciones</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión Aprobaciones" href="Gestion-Ordenes-Aprobacion.aspx">Ordenes</a></li>
        <li><a title="Aprobar Facturas" href="Aprobacion-Evaluacion-Facturas.aspx">Facturas</a></li>
        <li><a title="Evaluar Proveedores" href="Evaluacion-Proveedor-Facturas.aspx">Evaluar Proveedores</a></li>
        </ul>
        </div> 
    </div>
</div>
  
<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Trazabilidad Facturas</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/polea.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="RadicarCuentas" href="Gestion-Traza-Facturas.aspx">Envíos de facturas</a></li>
         <li><a></a></li>
        </ul>
        </div> 
    </div>
</div>              

               
                <!-- menu-element3-->
       </nav>

        </div>

    </div>
    <div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
