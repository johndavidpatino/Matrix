<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="CargarErrores.aspx.vb" Inherits="WebMatrix.CargarErrores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
        
       </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width:100%;">  
                <tr>
                    <td class="style1">Seleccione el archivo:</td>
                    <td class="style2"><asp:FileUpload ID="FileUpload1" runat="server" Height="30px" Width="772px" /></td>
                    <td><asp:Button ID="btnPasarServidor" runat="server" Text="Pasar a servidor" /></td>
                </tr>        
                <tr>
                    <td class="style1">Seleccione la hoja:</td>
                    <td class="style2"><asp:DropDownList ID="lstHoja" runat="server"></asp:DropDownList></td>
                    <td><asp:Button ID="btnCargarDatos" runat="server" Text="Cargar a la base" /></td>
                </tr>
 
     </table>
</asp:Content>
