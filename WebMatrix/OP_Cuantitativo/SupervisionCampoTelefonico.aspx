<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="SupervisionCampoTelefonico.aspx.vb" Inherits="WebMatrix.SupervisionCampoTelefonico" %>
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
<div class="actions">
<div class="form_left">
<label>Trabajo ID:</label>
    <asp:TextBox ID="txtTrabajoId" ReadOnly="true" runat="server"></asp:TextBox>    
</div>
<div class="form_left">
<label>Identificador:</label>
    <asp:TextBox ID="txtIdentificacionCT" runat="server"></asp:TextBox>
</div>
<div class="form_left">
<label>Operador:</label>
    <asp:DropDownList ID="ddlOperador" runat="server"></asp:DropDownList>
</div>
</div>
<div class="actions">
<h3><a href="#"><label>Intervención telefonica</label></a></h3><br />
<div class="form_left">
    <label>Lee introducción completa:</label>
    <asp:CheckBox ID="ChkCRI01" runat="server"/>
</div>
<div class="form_left">
    <label>Sigue el texto al pie de la letra:</label>
    <asp:CheckBox ID="ChkCRI02" runat="server"/>
</div>
<div class="form_left">
    <label>Ingada para buscar respuestas con contenido:</label>
    <asp:CheckBox ID="ChkCRI03" runat="server"/>
</div>
<div class="form_left">
    <label>Neutralidad:</label>
    <asp:CheckBox ID="ChkCRI04" runat="server"/>
</div>
<div class="form_left">
    <label>Sugiere, Interfiere, Interpreta o hace comentarios:</label>
    <asp:CheckBox ID="ChkCRI05" runat="server"/>
</div>
<div class="form_left">
    <label>Interpreta las instrucciones específicas :</label>
    <asp:CheckBox ID="ChkCRI06" runat="server"/>
</div>
<div class="form_left">
    <label>Reenfoca al entrevistado:</label>
    <asp:CheckBox ID="ChkCRI07" runat="server"/>
</div>
<div class="form_left">
    <label>Escucha sin interferir:</label>
    <asp:CheckBox ID="ChkCRI08" runat="server"/>
</div>
<div class="form_left">
    <label>Tono de voz claro y adecuado:</label>
    <asp:CheckBox ID="ChkCRI09" runat="server"/>
</div>
<div class="form_left">
    <label>Si hay respuesta "NO SE" vuelve a leer la pregunta:</label>
    <asp:CheckBox ID="ChkCRI10" runat="server"/>
</div>
<div class="form_left">
    <label>Lectura del cuestionario correcto:</label>
    <asp:CheckBox ID="ChkCRI11" runat="server"/>
</div>
<div class="form_left">
    <label>Cortesía con el entrevistado:</label>
    <asp:CheckBox ID="ChkCRI12" runat="server"/>
</div>
<div class="form_left">
    <label>Profundiza y aclara respuestas:</label>
    <asp:CheckBox ID="ChkCRI13" runat="server"/>
</div>
</div>
<div class="actions">
<h3><a href="#"><label>CATI</label></a></h3><br />
<div class="form_left">
    <label>Intervención del supervisor en CATI:</label>
    <asp:DropDownList ID="ddlCOM01" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Volver a entrenar al operador:</label>
    <asp:DropDownList ID="ddlCOM02" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Corregir el cuestionario:</label>
    <asp:DropDownList ID="ddlCOM03" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Anular el cuestionario:</label>
    <asp:DropDownList ID="ddlCOM04" runat="server"></asp:DropDownList>
</div>
</div>
<div class="actions">
<h3><a href="#"><label>Cuestionario</label></a></h3><br />
<div class="form_left">
    <label>Intervención del supervisor en entrevista:</label>
    <asp:DropDownList ID="ddlACC01" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Volver a entrenar al operador:</label>
    <asp:DropDownList ID="ddlACC02" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Corregir el cuestionario:</label>
    <asp:DropDownList ID="ddlACC03" runat="server"></asp:DropDownList>
</div>
<div class="form_left">
    <label>Anular el cuestionario:</label>
    <asp:DropDownList ID="ddlACC04" runat="server"></asp:DropDownList>
</div>
</div>
<div class="actions">
<label>Observaciones</label>
    <cc1:Editor ID="txtObservaciones" NoUnicode="true" Width="100%" Height="200px" runat="server"/>
<br />
</div>
<div class="form_right">
            <fieldset>
                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" 
                    CssClass="causesValidation buttonText buttonSave corner-all" 
                    Text="Guardar" />
                <input id="btnCancelar" type="button" value="Cancelar" class="buttonText buttonCancel corner-all" runat="server"
                                        style="font-size: 11px;" onclick="location.href='TrabajosProyectos.aspx';" />
            </fieldset>
</div>
<div class="actions">
    <center><asp:Label ID="lblResult" runat="server" Text=""></asp:Label></center>
    </div><br />
</asp:Content>
