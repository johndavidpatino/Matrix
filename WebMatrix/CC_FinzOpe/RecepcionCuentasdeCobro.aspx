<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_F.master" CodeBehind="RecepcionCuentasdeCobro.aspx.vb" Inherits="WebMatrix.RecepcionCuentasdeCobro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            validationForm();

        }
        $(document).ready(function () {
            loadPlugins();
        });

        function calcularTotal()
        {
            var valCantidad = parseFloat($('#CPH_Section_CPH_Section_CPH_ContentForm_txtcantidad').val().replace(",", "."));
            var valUnitario = parseFloat($('#CPH_Section_CPH_Section_CPH_ContentForm_txtvrunitario').val().replace(",", "."));
            $('#CPH_Section_CPH_Section_CPH_ContentForm_txttotal').val(valCantidad * valUnitario);
        }
        
    </script>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
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
                <div id="accordion0">
                    <asp:HiddenField ID="hfidtrabajo" runat="server" />
                    <asp:HiddenField ID="hfidOrden" runat="server" />
                    <asp:HiddenField ID="HfId" runat="server" />
                    <h3><a href="#">Listado de Ordenes de Servicio</a></h3>
                    <div class="block">
                         <div class="form_left">
                            <fieldset>
                                <label>
                                    Buscar por</label>
                                <asp:TextBox ID="txtbuscar" placeholder="ID Orden" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                        </div>
                        <asp:GridView ID="GvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="VrTotal" HeaderText="VrTotal" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="PersonaEvalua" HeaderText="Persona Evalua" />
                                <asp:TemplateField HeaderText="Grabar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgGrabar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Grabar" ImageUrl="~/Images/Select_16.png" Text="Grabar"
                                            ToolTip="Grabar" />
                                    </ItemTemplate>

                      
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>


                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">Radicar Cuenta
                        </a>
                    </h3>
                    <div class="block">
                       <div class="form_left">
                            <fieldset>
                                <label>Número de Radicado</label>
                                <asp:TextBox ID="txtconsecutivo" runat="server" TabIndex="3"></asp:TextBox>

                            </fieldset>
                            <fieldset>
                                <label>Observacion</label>
                                <asp:TextBox ID="txtobservacion" runat="server" TabIndex="7"></asp:TextBox>

                            </fieldset>
                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Cantidad</label>
                                <asp:TextBox ID="txtcantidad" runat="server" TabIndex="4"  onblur="calcularTotal()"></asp:TextBox>
                                <label >
                                <br />
                                Tipo Documento</label>
                                <asp:DropDownList ID="ddltipodocumento" runat ="server" TabIndex="8"></asp:DropDownList>
                            </fieldset>
                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>VrUnitario</label>
                                <asp:TextBox ID="txtvrunitario" runat="server" TabIndex="5" onblur="calcularTotal()"></asp:TextBox>

                            </fieldset>
                        </div>

                        <div class="form_left">
                            <fieldset>
                                <label>Valor Total</label>
                                <asp:TextBox ID="txttotal" runat="server" TabIndex="6" Enabled="false"></asp:TextBox>

                            </fieldset>
                        </div>
                         <div class="actions">
                             <asp:Button ID ="btnAgregar" runat ="server"  Text ="Agregar" TabIndex="9"/>
                        <asp:Button ID ="btnGuardar" runat ="server"  Text ="Guardar"/>
                        <asp:Button ID="btnCancelar" runat="server" Text ="Cancelar" />
                             </div>
                    <fieldset >
                        <asp:GridView ID="GvDetalle" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Consecutivo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Consecutivo" HeaderText="Número Radicado" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="VrTotal" HeaderText="VrTotal" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="TipoDocumento" HeaderText="TipoDocumento" />


                            </Columns>
                        </asp:GridView>
                        </fieldset>
                    </div>

                </div>
                 <div id="accordion2">
                     <h3>
                        <a href="#">Consulta de Radicados
                        </a>
                    </h3>
                    <div class="block">
                     <div class="form_left">
                            <fieldset>
                                <label>Ingrese Número de Radicado a buscar</label>
                                <asp:TextBox ID="TxtConBus" runat="server" TabIndex="3"></asp:TextBox>
                                <asp:Button ID="btbuscar" runat="server" Text="buscar" />


                                <br />
                                <br />


                                <asp:GridView ID="GvRadicados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id,Consecutivo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="Número Radicado" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="VrTotal" HeaderText="VrTotal" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="TipoDocumento" HeaderText="TipoDocumento" />
                                <asp:BoundField DataField="OrdenId" HeaderText="Orden" />
                                <asp:BoundField DataField="UsuarioId" HeaderText="Usuario" Visible ="false"  />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                 <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgEliminar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Eliminar" ImageUrl="~/Images/Delete_16.png" Text="Eliminar"
                                            ToolTip="Eliminar" OnClientClick="javascript:if(!confirm('¿Confirma que desea Eliminar Radicado?'))return false"/>
                                    </ItemTemplate>                     
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
                                    </ItemTemplate>                                                           
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                            </fieldset>
                  </div>
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
