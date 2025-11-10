<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPC_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._Default4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior"  title="Anterior"></a></div>
       <div id="slider">
       <div class="slidesContainer">
       <nav class="slide">
       
<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Sesiones</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Opcion 1" href="FichaSesion.aspx">Definición</a></li>
        <li><a title="Opcion 2" href="Sesion.aspx">Planeación</a></li>
        <li><a title="Opcion 3" href="Sesion.aspx">Ejecución</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Entrevistas</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Opcion 1" href="FichaEntrevista.aspx">Definición</a></li>
        <li><a title="Opcion 2" href="Entrevista.aspx">Planeación</a></li>
        <li><a title="Opcion 2" href="Entrevista.aspx">Ejecución</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Observaciones - In Visit</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href=""></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Opcion 1" href="FichaObservacion.aspx">Definición</a></li>
        <li><a title="Opcion 2" href="Observacion.aspx">Planeación</a></li>
        <li><a title="Opcion 2" href="Observacion.aspx">Ejecución</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->

 
       </nav>
       
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>