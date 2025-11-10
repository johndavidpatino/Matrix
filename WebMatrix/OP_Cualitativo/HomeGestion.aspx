<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_.master" CodeBehind="HomeGestion.aspx.vb" Inherits="WebMatrix._HomeGestionC" %>
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
    <div class="name-menu">Control de Calidad</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Calidad Campo" href="../PY_ControlCalidad/ControlCalidadCampo.aspx">Calidad Campo</a></li>
        <li><a title="Calidad Transcripción" href="../PY_ControlCalidad/ControlTranscripcion.aspx">Calidad Transcripciones</a></li>
        <li><a title="Calidad Informe" href="../PY_ControlCalidad/ControlCalidadInforme.aspx">Calidad Informe</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Evaluación de Calidad</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Evaluación Entrevistadora" href="../PY_ControlCalidad/EvaluacionEntrevistadora.aspx">Evaluación Entrevistadora</a></li>
        <li><a title="Evaluación Moderadora" href="../PY_ControlCalidad/EvaluacionModeradora.aspx">Evaluación Moderadora</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

       </nav>
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>
