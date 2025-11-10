<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_.master" CodeBehind="HomeRecoleccion.aspx.vb" Inherits="WebMatrix._HomeRecoleccionC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior" title="Anterior"></a></div>
    <div id="slider">
        <div class="slidesContainer">
            <nav class="slide">

                <div class="menu-element1">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Coordinador</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a href="../OP_Cualitativo/TrabajosCoordinador.aspx">Trabajos</a></li>
                                <li><a href="../RE_GT/AsignacionCampo.aspx">Asignar Coordinador</a></li>
                                <li><a href="../OP_Cualitativo/Calendario.aspx">Calendario Coordinador</a></li>
                                <li><a href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element1-->

            </nav>
        </div>
    </div>
    <div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
