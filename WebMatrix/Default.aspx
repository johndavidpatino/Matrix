<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>..:::IPSOS Game Changers - Matrix:::...</title>
    <link rel="stylesheet" href="css/newLogin.css" />
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css' />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
</head>
<body class="align">
    <div class="site__container">


        <div class="grid__container">
            <form id="form1" runat="server" class="form form--login">

                <div id="logoLogin">
                    <img src="images/Newlogo.png" alt="Matrix" />
                </div>

                <div class="form__field">
                    <label class="fontawesome-user" for="login__username"><span class="hidden">Usuario</span></label>
                    <asp:TextBox ID="user" CssClass="form__input" runat="server" placeholder="Usuario"></asp:TextBox>
                </div>

                <div class="form__field">
                    <label class="fontawesome-lock" for="login__password"><span class="hidden">Contraseña</span></label>
                    <asp:TextBox ID="pass" TextMode="Password" CssClass="form__input" runat="server" placeholder="Contraseña"></asp:TextBox>
                </div>

                <div class="form__field">
                    <asp:Button ID="btnEntrar" Text="Entrar" runat="server" />
                </div>

            </form>
            <!-- espacio para elementos en el pie de la aplicación-->
            <div class="footer" style="text-align: center; margin: 40px auto 20px; ">
                <a style="font-size: 14px;" title="Registre Easy Work" href="../TH_TalentoHumano/HWH.aspx">Easy Work</a>
                <label style="font-size: 14px; text-decoration: none; color: #00ada8;">&nbsp;-&nbsp;</label>
                <a style="font-size: 14px;" title="Registro de Producción" href="OP_Cuantitativo/RegistroProduccionOP.aspx">Registro de Producción</a>
                <div id="sombra" style="margin: 10px;">
                    <img src="../images/sombra-abajo.png" height="35" alt="polea" />
                </div>
            </div>
        </div>

    </div>
	<%
        Response.Write("<script>console.log('ApplicationPath:" & Request.ApplicationPath & ",Url.Scheme:" & Request.Url.Scheme & ", Url.SchemeDelimiter:" & Request.Url.SchemeDelimiter.ToString & ", Url.Authority:" & Request.Url.Authority & "')</script>")
    %>
</body>
</html>
