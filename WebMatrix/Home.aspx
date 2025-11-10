<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="WebMatrix.Home" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home - Proyecto Matrix</title>
    <link rel="stylesheet" href="Styles/menu.css" />
    <link href='http://fonts.googleapis.com/css?family=Sanchez' rel='stylesheet' type='text/css'>
</head>
<body>
    <form id="form1" runat="server">
        <div id="content">
        <div id="logomenu"> <a href="#"><img src="images/logo-menu.png" width="395" height="85" alt="Proyecto Matrix"></a>
        </div>
        <div id="dobleflecha1">
          <img src="images/dobleflecha.png" width="35" height="35" alt=""/>
        </div>
        <div id="cajasarriba"> <a href="MBO/Default.aspx"><img src="images/proceso-gerencial.png" width="290" height="45" alt=""/></a>
        </div>
        <div id="cajasarriba"> <a href="GD_Documentos/Default.aspx"><img src="images/gestion-calidad.png" width="290" height="45" alt=""/></a>
        </div>
        <div id="dobleflecha2">
          <img src="images/dobleflecha2.png" width="35" height="35" alt=""/>
        </div>
        <div id="barraizq"> <a href="#"><img src="images/necesidades-cte.png" width="60" height="340" alt=""/></a>
        </div>
        <div id="content-centro">
          <div id="btnscentro"><a href="CU_Cuentas/Home.aspx"><img src="images/cuentas.png" width="600" height="60" alt=""/></a></div>
          <div id="btnscentro"><a href="PY_Proyectos/Home.aspx"><img src="images/proyectos.png" width="600" height="60" alt=""/></a></div>
          <div id="btnscentro"><a href="RE_GT/RecoleccionDeDatos.aspx"><img src="images/operaciones.png" width="600" height="60" alt=""/></a></div>
          <div id="btnscentro"><a href="RE_GT/GestionyTratamientoDeDatos.aspx"><img src="images/operacionesc.png" width="600" height="60" alt=""/></a></div>
        </div>
        <div id="barrader"> <a href="#"><img src="images/satisfaccion-cte.png" width="60" height="340" alt=""/></a>
        </div>
        <div id="abajo">
          <div id="cajasabajo"><a href="FI_AdministrativoFinanciero/Default.aspx"><img src="images/admin.png" width="151" height="45" alt=""/></a></div>
          <div id="cajasabajo"><a href="TH_TalentoHumano/Default.aspx"><img src="images/talento.png" width="151" height="45" alt=""/></a></div>
          <div id="cajasabajo"><a href="FI_AdministrativoFinanciero/Default-Compras.aspx"><img src="images/compras.png" width="151" height="45" alt=""/></a></div>
          <div id="cajasabajo"><a href="IT/Default.aspx"><img src="images/tecno.png" width="151" height="45" alt=""/></a></div>
          <div id="cajasabajo"><a href="ES_Estadistica/Home.aspx"><img src="images/estad.png" width="151" height="45" alt=""/></a></div>
        </div>
    
      </div>
      <footer><!-- espacio para elementos en el pie de la aplicación-->
        <nav>
           <ul>
              <li><a style="font-size:14px" title="Ir a inicio" href="Home.aspx">Ir a inicio</a></li>
               <li><a style="font-size:14px" title="Producto No Conforme" href="PNC/PNC_Productos.aspx">Producto No Conforme</a></li>
              <li><a style="font-size:14px" title="Reportes" href="RP_Reportes/DefaultMenu.aspx">Reportes</a></li>
              <li><a style="font-size:14px" title="Actualizar mi hoja de vida" href="TH_TalentoHumano/HojaVida.aspx">Mis datos</a></li>
              <li><a style="font-size:14px" title="Cambiar mi contraseña de ingreso" href="US_Usuarios/CambioContrasena.aspx">Cambiar mi password</a></li>
              <li><a style="font-size:14px" title="Listado de tareas y alertas" href="CORE/Gestion-Tareas-Trabajos.aspx">Mis tareas y asignaciones</a></li>
              <li><a style="font-size:14px" title="Sugerencias, errores, dudas y opiniones sobre el sistema" href="../US_Usuarios/Feedback.aspx">Mi retroalimentación a Matrix</a></li>
              <li><a style="font-size:14px" title="Salir del sistema de forma segura" href="#">
                  <asp:LoginStatus ID="LoginStatus1" runat="server" 
                      LogoutText="Cerrar mi sesión" Font-Size="14px" 
                      LogoutAction="RedirectToLoginPage"  /></a></li>
           </ul>
      </nav>
      <div id="sombra"><img src="../images/sombra-abajo.png" width="1000" height="35" alt="polea"></div>
      <div id="logo-abajo"><img src="../images/logo-abajo.png" width="31" height="29" alt="IPSOS"></div>
    </footer>
    </form>
</body>
</html>
