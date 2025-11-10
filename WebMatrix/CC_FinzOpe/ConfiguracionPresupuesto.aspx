<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master"
    CodeBehind="ConfiguracionPresupuesto.aspx.vb" Inherits="WebMatrix.ConfiguracionPresupuesto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
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
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Configuracion de Presupuesto
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:HiddenField ID="hfidtrabajo" runat="server" />
                        <asp:HiddenField ID="hfidpresupuesto" runat="server" />
                        <asp:HiddenField ID="HfidTipo" runat="server" />
                        <div class="form_left">
                            <label>
                                Pregunta</label>
                            <asp:DropDownList ID="ddlpregunta" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                        <div class="form_left">
                        <label>
                            Respuestas</label>
                        <asp:DropDownList ID="ddlRespuestas" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                        </div> 
                        <div class="form_left">
                        <label>
                            Condicion</label>
                        <asp:DropDownList ID="ddlcondicion" runat="Server" AutoPostBack="True" Width="75px" >
                            <asp:ListItem Value="1">Igual</asp:ListItem>
                            <asp:ListItem Value="2">Diferente</asp:ListItem>
                        </asp:DropDownList>
                         <asp:Button ID="btnagregar" runat="server" Text="Agregar" />
                         <asp:Button ID="btnCrear" runat="server" Text="Crear Presupuesto" />
                        </div> 
                       
                   
                    <div class="form_left">
                        <asp:GridView ID="GvPreguntas" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Pr_Id" EmptyDataText="No existen registros para mostrar">
                            <Columns>
                                <asp:BoundField DataField="Pr_Id" HeaderText="Pr_Id" />
                                <asp:BoundField DataField="Pregunta" HeaderText="Preguntas" />
                                <asp:BoundField DataField="CodPregunta" HeaderText="CodPreguntas" />
                                <asp:BoundField DataField="Respuesta" HeaderText="Respuesta" />
                                <asp:BoundField DataField="CodRespuesta" HeaderText="CodRespuesta" />
                                <asp:BoundField DataField="Condicion" HeaderText="Condicion" />

                            </Columns>
                        </asp:GridView>
                        </div> 
                 
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
