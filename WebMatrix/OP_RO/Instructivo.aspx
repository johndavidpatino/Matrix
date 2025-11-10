<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master" CodeBehind="Instructivo.aspx.vb" Inherits="WebMatrix.Instructivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {

            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#tabs").tabs();
            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });

        }
        function redireccion() {
            var idtrabajo = $('#<%= hfidtrabajo.ClientID %>').val();
            document.location.href = 'Instructivo.aspx?idtrabajo=' + idtrabajo;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>REGISTRO OBSERVACIONES A INSTRUMENTOS DE ENTREGA (INSTRUCTIVO)</a>
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
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Anotaciones Durante la Revisión del Instructivo</label>
                        </a>
                        <asp:HiddenField ID="hfidtrabajo" runat="server" />
                        <asp:HiddenField ID="hfGerentePY" runat="server" />
                    </h3>
                    <div class="block">
                        <div id="tabs">
                            <ul>
                                <li><a href="#tab-1">Revisión</a></li>
                                <li><a href="#tab-2">Ejecución</a></li>
                            </ul>
                            <div id="tab-1">
                                <asp:Panel ID="pnllistarevision" runat="server" Height="350px" ScrollBars="Auto">
                                    <asp:GridView ID="gvRevision" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="25"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" Caption="ANOTACIONES DURANTE LA REVISIÓN DEL INSTRUCTIVO"
                                        ShowFooter="True">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <EmptyDataRowStyle VerticalAlign="Top" />
                                        <EmptyDataTemplate>
                                            <table style="vertical-align: top">
                                                <tr>
                                                    <td>
                                                        <b>Observación</b>
                                                    </td>
                                                    <td>
                                                        <b>Descripción Observación</b>
                                                    </td>
                                                    <td>
                                                        <b>Espacio Para Gerentes</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    
                                                    <td>
                                                        <asp:TextBox ID="txtObservacionEmpty" MaxLength="1" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="fteTxtObservacion" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionEmpty" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescripcionEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEspacioEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="ctrl_btnaddempty" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                            OnClick="AgregarRevision" Font-Size="11px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ShowHeader="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observación">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Observacion") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtObservacionedit" MaxLength="1" runat="server" Text='<%# Bind("Observacion") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fteTxtObservacion4" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionedit" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtObservacionfooter" MaxLength="1" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fteTxtObservacion4" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionfooter" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción Observación">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("DescripcionObservacion") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDescripcionedit" runat="server" Text='<%# Bind("DescripcionObservacion") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtDescripcionfooter" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Espacio Para Gerentes">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("RespuestaGerente") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEspacioedit" runat="server" Text='<%# Bind("RespuestaGerente") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtEspaciofooter" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" HeaderText="Modificar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton32223" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ImageUrl="~/Images/Select_16.png" Text="Edit" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ImageButton322" runat="server" CausesValidation="True" CommandName="Update"
                                                        ImageUrl="~/Images/save_16.png" Text="Update" ToolTip="Guardar" />
                                                    &nbsp;<asp:ImageButton ID="ImageButton2211" runat="server" CausesValidation="False"
                                                        CommandName="Cancel" ImageUrl="~/Images/no.jpg" Text="Cancel" ToolTip="Cancelar" />
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:Button ID="ctrl_btnaddfooter" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                        OnClick="AgregarRevision" Font-Size="11px" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                        CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                                        OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
                                                </ItemTemplate>
                                                  </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rechazar" ShowHeader="False" Visible="true" >
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chbRechazarRev" Checked="False" runat="server" Visible='<%# IIF(Eval("Observacion")="E","TRUE","FALSE") %>' />
                                                </ItemTemplate>
                                                
                                                <ItemStyle HorizontalAlign="Center" />
                                          
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                            <div id="tab-2">
                                <asp:Panel ID="pnllistaejecucion" runat="server" Height="350px" ScrollBars="Auto">
                                    <asp:GridView ID="gvEjecucion" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="25"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id" EmptyDataText="No existen registros para mostrar" Caption="ANOTACIONES DURANTE LA EJECUCIÓN DEL INSTRUCTIVO"
                                        ShowFooter="True">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <EmptyDataRowStyle VerticalAlign="Top" />
                                        <EmptyDataTemplate>
                                            <table style="vertical-align: top">
                                                <tr>
                                                    <td>
                                                        <b>Observación</b>
                                                    </td>
                                                    <td>
                                                        <b>Descripción Observación</b>
                                                    </td>
                                                    <td>
                                                        <b>Espacio Para Gerentes</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtObservacionEmpty" MaxLength="1" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="fteTxtObservacion" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionEmpty" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescripcionEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEspacioEmpty" runat="server" CssClass="textEntry" Width="140px"
                                                            BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="ctrl_btnaddempty" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                            OnClick="AgregarEjecucion" Font-Size="11px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ShowHeader="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observación">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Observacion") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtObservacionedit" MaxLength="1" runat="server" Text='<%# Bind("Observacion") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fteTxtObservacion3" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionedit" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtObservacionfooter" MaxLength="1" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fteTxtObservacion" runat="server" FilterType="Custom"
                                                    TargetControlID="txtObservacionfooter" ValidChars="ESes"></asp:FilteredTextBoxExtender>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción Observación">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("DescripcionObservacion") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDescripcionedit" runat="server" Text='<%# Bind("DescripcionObservacion") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtDescripcionfooter" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Espacio Para Gerentes">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("RespuestaGerente") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEspacioedit" runat="server" Text='<%# Bind("RespuestaGerente") %>'
                                                        CssClass="textEntry" Width="140px" BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtEspaciofooter" runat="server" CssClass="textEntry" Width="140px"
                                                        BorderColor="Silver" BorderStyle="Solid"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False" HeaderText="Modificar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton32223" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ImageUrl="~/Images/Select_16.png" Text="Edit" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ImageButton322" runat="server" CausesValidation="True" CommandName="Update"
                                                        ImageUrl="~/Images/save_16.png" Text="Update" ToolTip="Guardar" />
                                                    &nbsp;<asp:ImageButton ID="ImageButton2211" runat="server" CausesValidation="False"
                                                        CommandName="Cancel" ImageUrl="~/Images/no.jpg" Text="Cancel" ToolTip="Cancelar" />
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:Button ID="ctrl_btnaddfooter" runat="server" Text="Agregar" CssClass="buttonText buttonSave corner-all"
                                                        OnClick="AgregarEjecucion" Font-Size="11px" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                        CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                                        OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
                                                </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Rechazar" ShowHeader="False" Visible="TRUE">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chbRechazarEje" Checked="False" runat="server" Visible='<%# IIF(Eval("Observacion")="E","TRUE","FALSE") %>' />
                                                </ItemTemplate>
                                                
                                                <ItemStyle HorizontalAlign="Center" />
                                          
                                            </asp:TemplateField>
                                           
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="actions">
                        </div>
                        <div class="actions">
                            <div class="form_right">
                                <fieldset>
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" CssClass="causesValidation buttonText buttonSave corner-all" />
                                    &nbsp;
                                    <input id="Button1" type="button" value="Cancelar" class="buttonText buttonCancel corner-all"
                                        style="font-size: 11px;" onclick="redireccion();" />
                                    &nbsp;
                                    <asp:Button ID="btnNotificar" runat="server" Text="Notificar observaciones" />
                                     &nbsp;
                                    <asp:Button ID="btnRechazar" runat="server" Text="Rechazar Error" />
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles del registro</label>
                        </a>
                    </h3>
                    <div class="block">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
