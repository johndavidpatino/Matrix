<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_F.master" CodeBehind="NominaDistribucionCostos.aspx.vb" Inherits="WebMatrix.NominaDistribucionCostos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type='text/javascript'>
        function Forzar() {
            __doPostBack('', '');
        }
    </script>
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
                    <td class="style1" >Seleccione el archivo:</td>
                    <td class="style2" ><asp:FileUpload ID="FileUpload1" runat="server" Height="30px" Width="800px" /></td>
                    <td ><asp:Button ID="btnPasarServidor" runat="server" Text="1-Pasar a servidor" /></td>
                </tr>
   </table>
   <table style="width:100%;">
                <tr>
                    <td>Seleccione la hoja</td>
                    <td>Desde</td>
                    <td>Hasta</td>
                    <td>Tipo contrato</td>
                    <td></td>
                </tr>        
                <tr>
                    <td Width="40%"><asp:DropDownList ID="lstHoja" runat="server" Width="300px" AutoPostBack="True"></asp:DropDownList> </td>
                    <td Width="10%"><asp:TextBox ID="txtFechaInicial" runat="server" Width="100px"></asp:TextBox></td>
                    <td Width="10%"><asp:TextBox ID="txtFechaFinal" runat="server" Width="100px" ></asp:TextBox></td>
                    <td Width="20%"><asp:TextBox ID="txtTipoContratacion" runat="server" Width="250px"></asp:TextBox></td>
                    <td class="style2" Width="10%"><asp:Button ID="btnCargarDatos" runat="server"  Text="2-Cargar a la base" /></td>
                </tr>
     </table>
    <table style="width:100%;">
        <tr>
            <td>
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GVErrores" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="True" CssClass="displayTable" PageSize="10" Width="100%" EmptyDataText="NO HAY ERRORES">
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIf(GVErrores.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(GVErrores.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GVErrores.PageIndex + 1%>-<%= GVErrores.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((GVErrores.PageIndex + 1) = GVErrores.PageCount, "false", "true")%>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((GVErrores.PageIndex + 1) = GVErrores.PageCount, "false", "true")%>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
    </table>

    <table style="width:100%;">
        <tr>
            <td class="style2" Width="10%"><asp:Button ID="btnDistribucion" runat="server"  Text="3-Distribuir" /></td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GVDistribucion" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="displayTable" PageSize="20" Width="100%" AlternatingRowStyle-CssClass="odd" 
                            PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="NO EXISTEN DATOS">
                            <Columns>
                                <asp:BoundField DataField="JobBook" HeaderText="JOB BOOK" />
                                <asp:BoundField DataField="CtaSymphony" HeaderText="CUENTA" />
                                <asp:BoundField DataField="JB_Descripcion" HeaderText="DESCRIPCION" />
                                <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" />
                                <asp:BoundField DataField="VrCosto" HeaderText="VALOR COSTO" DataFormatString="{0:C2}" HtmlEncode="False" />
                            </Columns>
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIf(GVDistribucion.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(GVDistribucion.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GVDistribucion.PageIndex + 1%>-<%= GVDistribucion.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((GVDistribucion.PageIndex + 1) = GVDistribucion.PageCount, "false", "true")%>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((GVDistribucion.PageIndex + 1) = GVDistribucion.PageCount, "false", "true")%>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </td>
            <td class="style2" Width="10%"><asp:Button ID="btnExportar" runat="server"  Text="4-Exportar" /></td>
        </tr>
    </table>
    <table>
        <tr Width="100%">
            <td Width="50%">
                TOTAL COSTO A DISTRIBUIR :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtCostoADistribuir" runat="server" Width="200px" DataFormatString="{0:C2}" HtmlEncode="False"></asp:TextBox></td>
            <td Width="50%">
                TOTAL COSTO DISTRIBUIDO  :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtVrDistribuido" runat="server" Width="200px" DataFormatString="{0:C2}" HtmlEncode="False"></asp:TextBox>
            </td>
        </tr>
    </table>

     <script type="text/javascript">
         var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
         pageReqManger.add_initializeRequest(InitializeRequest);
         pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>

