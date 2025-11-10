<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master" CodeBehind="GD_Maestro.aspx.vb" Inherits="WebMatrix.GD_Maestro" %>
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
                    <h3><a href="#"><label>Gestión de documentos</label></a></h3>
                    <div class="form_left">

                 <!--   <label>Descripción:</label>
                    <asp:TextBox ID="txtNoSolicitud" runat="server" TextMode="SingleLine" 
                            style=" max-height:31px; max-width:118px;" ></asp:TextBox>
                    </div>
                    <div class="form_left">
                    <label>Fecha:</label>
                    <asp:DropDownList ID="ddUsuario" runat="server"></asp:DropDownList>
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
                    </div>-->
                    </div>
            </div>
            <div class="actions" id ="Construcción">

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
             <div class="actions" id ="Construcción2" >
                    
                   <div class="form_left">
                    <label runat="server" id="lblUbiArchivo" >Ubicacion Archivo: </label> 
                        <asp:TextBox ID="txtUbiArchivo" runat="server"></asp:TextBox>                      
                        
                    </div>
                    <div class="form_left">
                    <label runat="server" id="lblMetRec" >Metodo Recuperación: </label> 
                        <asp:TextBox ID="txtMetRec" runat="server"></asp:TextBox>                      
                        
                    </div>
                    <div class="form_left">
                    <label runat="server" id="lblTiempoRet" >Tiempo retención: </label> 
                        <asp:TextBox ID="txtTiempoRet" runat="server"></asp:TextBox>                      
                        
                    </div>
                    <div class="form_left">
                    <label runat="server" id="LblDisFin" >Disposición Final: </label> 
                        <asp:TextBox ID="txtDisFin" runat="server"></asp:TextBox>
                    </div>                              
            </div>
             <div class="actions" id ="Responsables Revisiones" >
                 <asp:GridView ID="gvUsuCorreos" runat="server">
                 </asp:GridView>
             </div>
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
                                     <asp:GridView ID="GridView1" runat="server">
                                     </asp:GridView>
                                </div>    
               </div>-->

            <div class="actions">
                        <div class="form_right">
                <fieldset>
                    <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Visible="false" 
                         
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
