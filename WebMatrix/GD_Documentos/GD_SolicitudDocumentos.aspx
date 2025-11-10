<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master" CodeBehind="GD_SolicitudDocumentos.aspx.vb" Inherits="WebMatrix.GD_SolicitudDocumentos" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
            <div id ="gestion_d" runat="server" visible="true">
                    <h3><a href="#"><label>Solicitud de Documentos</label></a></h3>
                    <div class="form_left">
                  
                    <div class="form_left">
                    <label>Fecha:</label>
                    <asp:TextBox ID="txtfecha" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                    <div class="form_left">
                    <label>Solicitante:</label>
                    <asp:DropDownList ID="ddlSolicitante" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form_left">
                    <label>Area:</label>
                        <asp:TextBox ID="txtArea" runat="server"></asp:TextBox>
                    </div>
                    <div class="form_left">
                    <label>Cargo:</label>
                        <asp:TextBox ID="txtCargo" runat="server"></asp:TextBox>
                    </div>
                    </div>
            </div>

            <div class="actions" id ="Construcción" >

                    <div class="form_left">
                    <label>Tipo de Solicitud:</label>
                    <asp:DropDownList ID="ddlTipoSolicitud" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </div>

                    <div class="form_left">
                    <label runat="server" id="lblNomDoc" >Nombre documento: </label> 
                        <asp:TextBox ID="txtNomDoc" runat="server"></asp:TextBox>                      
                        
                    </div>

                     <div class="form_left">
                    <label runat="server" id="lblNomDoc2" visible="False" >Nombre documento: </label>                                          
                      <asp:DropDownList ID="ddlNomDocumento" runat="server" Visible="False" 
                            AutoPostBack="True"></asp:DropDownList>
                    </div>
                   

                    <div class="form_left">
                     <label runat="server" id="lblCodDoc">Codigo Documento:</label>
                        <asp:TextBox ID="txtCodDoc" runat="server"></asp:TextBox>
                    </div>

                     <div class="form_left">
                    <label runat="server" id ="lblResponsable">Responsable:</label>                       
                        <asp:DropDownList ID="ddlResponsable" runat="server" Visible="true" 
                             AutoPostBack="True"></asp:DropDownList>
                    </div>

                    <div class="form_left">
                    <label  runat="server" id="lblProceso"> Proceso:</label>                    
                    <asp:DropDownList ID="ddlProceso" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </div>     
            </div>

            <div class="actions" id="SolicitudDocumentos">
                    <div class="form_left">
                     <label runat="server" id="lblAreaUso">Area Uso:</label>
                        <asp:TextBox ID="txtAreaUso" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form_left">
                     <label runat="server" id="lblSitAcce">Sitio Acceso:</label>
                        <asp:TextBox ID="txtSitAcce" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form_left">
                     <label runat="server" id="lblRazSol">RazonSolicitud:</label>
                        <asp:TextBox ID="txtRazSol" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form_left">
                     <label runat="server" id="lblDescSol">Descripción Solicitud:</label>
                        <asp:TextBox ID="txtDescSol" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
            </div>
 
            <div class="actions" id="SolicitudDocumentos2">
                    <div class="form_left">
                     <label runat="server" id="lblEstId">Estado Id:</label>
                        <asp:DropDownList ID="ddlEstadoId" runat="server" AutoPostBack="True"></asp:DropDownList>
                        <asp:TextBox ID="txtfechaestado" runat="server" Visible ="False"></asp:TextBox>
                    </div>
                    <div class="form_left">
                     <label runat="server" id="lblComentarios">Comentarios:</label>
                        <asp:TextBox ID="txtComent" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="form_left">
                     <label runat="server" id="lblMods">Modificaciones:</label>
                        <asp:TextBox ID="txtMods" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
            </div>

             <div class="actions" id="DvControlUsuarios" runat="server">
                <label>Selecciona Responsables Revisiones</label><asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                <Control:Usuarios id="SelUser" runat="server"/>
                <label>Asunto:</label> <asp:TextBox ID="txtAsunto" runat="server" Width="483px"></asp:TextBox>
                <label>Contenido:</label><cc1:Editor ID="txtContenido" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
        </div><br />
    
           <!-- <div class="actions" id="Revision Solicitud" visible = "false">
                     <div class="form_left">
                    <label runat="server" id="lblEstSol" >Estado Solicitud: </label> 
                         <asp:RadioButtonList ID="rblEstadoSolicitud" runat="server">
                         </asp:RadioButtonList>
                    </div>        
                     <div class="form_left">
                    <label>Fecha Aprobación o Rechazo:</label>
                         <asp:TextBox ID="txtAprobRec" runat="server"></asp:TextBox>
                    </div>        
                     <div class="form_left">
                    <label>Version:</label>
                         <asp:TextBox ID="txtVersion" runat="server"></asp:TextBox>
                    </div>  
                <br />      
                <div class="actions">
                    <label>Comentarios:</label>
                         <asp:TextBox ID="txtComentarios" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>  

            </div>  -->

               <!-- <div class="actions" id="Actualizaciones Realizadas">
                                 <div class="form_left">
                                <label>Estado Solicitud:</label>
                                     <asp:GridView ID="GridView2" runat="server">
                                     </asp:GridView>
                                </div>    
               </div>-->

            <div class="actions">
                        <div class="form_right">
                <fieldset>
                    <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Visible="True" 
                         
                        Text="Guardar" />
                    <asp:Button ID="btnEditar" runat="server" CommandName="Editar" Visible="false" 
                         Text="Editar" />
                    &nbsp;
                    <input id="btnCancelar" type="button" class="button" value="Cancelar"  runat="server"
                                            style="font-size: 11px;" onclick="location.href='Actas.aspx';" />
                </fieldset></div>
            </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
