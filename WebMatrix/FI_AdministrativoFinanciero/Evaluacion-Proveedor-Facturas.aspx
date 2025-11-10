<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_.master" CodeBehind="Evaluacion-Proveedor-Facturas.aspx.vb" Inherits="WebMatrix.Evaluacion_Proveedor_Facturas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        function loadPlugins() {
            $("td").css("padding", "0");
        }

        $(document).ready(function () {
            loadPlugins();
        });

        function validarP1_5(oSrc, args) {
            if ($("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_5']:checked").length == 0 && $("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_5_96']:checked").length == 0) {
                args.IsValid = false;
                alert("Pregunta 5 Obligatoria!!!")
            }
            else {
                args.IsValid = true;
            }
        }
        function validarP1_6(oSrc, args) {
            if ($("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_6']:checked").length == 0 && $("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_6_96']:checked").length == 0) {
                args.IsValid = false;
                alert("Pregunta 6 Obligatoria!!!")
            }
            else {
                args.IsValid = true;
            }
        }
        function validarP1_7(oSrc, args) {
            if ($("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_7']:checked").length == 0 && $("input[name='ctl00$ctl00$CPH_Section$CPH_Section$RBP1_7_96']:checked").length == 0) {
                args.IsValid = false;
                alert("Pregunta 7 Obligatoria!!!")
            }
            else {
                args.IsValid = true;
            }
        }



    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <div style="width: 100%">
                        <h1><a>Gestión de </a>Facturas</h1>


                        <asp:Panel ID="pnlCronograma" runat="server" Visible="true">
                            <asp:HiddenField ID="hfEstado" runat="server" Value="1" />
                            <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                            <asp:HiddenField ID="hfFactura" runat="server" Value="-1" />
                            <div class="clear"></div>
                            <a>
                                <asp:Label ID="lblTitle" runat="server">Evaluación de Proveedores de la compra o servicio</asp:Label></a>
                            <div class="clear"></div>
                            <div class="block">
                                <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false" PageSize="5"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="id" AllowPaging="true" EmptyDataText="No se encuentran facturas para los filtros seleccionados">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnEvaluar" runat="server" Text="Evaluar" Width="60px" CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="id" HeaderText="Id" />
                                        <asp:BoundField DataField="FechaRadicacion" HeaderText="FechaRadicacion" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="NoFactura" HeaderText="NoFactura" />
                                        <asp:BoundField DataField="Consecutivo" HeaderText="NoRadicado" />
                                        <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                                        <asp:BoundField DataField="NIT_CC" HeaderText="NIT/CC" />
                                        <asp:BoundField DataField="ValorFactura" HeaderText="Valor"
                                            DataFormatString="{0:C0}" />
                                        <asp:TemplateField HeaderText="Escáner" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Load" ImageUrl="~/Images/list_16.png" Text="Escáner" ToolTip="Imagen de la factura" OnClientClick="MostrarLoadFiles()" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <div class="pagingButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td><span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-<%= gvDatos.PageCount%>]</span> </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>

                        </asp:Panel>
                    </div>
                </div>
                <div class="clear"></div>
                <asp:Panel ID="pnlAprobacion" runat="server" Visible="false">
                    <label>Usted esta Evaluando al Proveedor de la factura</label>
                    Factura No:
                    <asp:Label ID="lblFacturaNo" runat="server" />
                    <br />
                    Radicado No:
                    <asp:Label ID="lblRadicadoNo" runat="server" />
                    <br />
                    Proveedor:
                    <asp:Label ID="lblProveedor" runat="server" />
                    <br />
                    NIT/CC:
                    <asp:Label ID="lblNitCC" runat="server" />
                    <br />
                    Valor:
                    <asp:Label ID="lblValor" runat="server" />
                    <br />

                    <asp:Button ID="btnAprobarSinEvaluar" Text="Guardar sin Evaluar" runat="server" />
                    <asp:Button ID="btnEvaluarYAprobar" Text="Evaluar" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlEvaluacion" runat="server" Visible="false">

                    <div id="campo-formulario3" style="min-width: 600px; margin-bottom: 10px">
                        <a style="color: white;">Usando una escala una escala de 1 a 10 donde 1 es calificación más baja y 10 calificación más alta, como evaluaría el servicio prestado en el último MES por :  </a>
                        <asp:Label ID="lblProveedor1" runat="server" Style="color: white"></asp:Label>
                        <a style="color: white;">en cuanto a:</a>

                        <br></br>
                        <br></br> 
                        <br></br>
                        <div style="width: 100%">
                            <div style="width: 100%; float: none">
                                <label>
                                    1. Calidad del producto servicio en general:
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RBP1_1" ErrorMessage="*" ForeColor="Red">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left">
                                    <asp:RadioButtonList ID="RBP1_1" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1." Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10." Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_1" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a calidad del producto servicio en general?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtP1_1" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_1" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                2. La eficiencia, entendida como las actividades realizadas para obtener el resultado:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="RBP1_2" ErrorMessage="*" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left">
                                    <asp:RadioButtonList ID="RBP1_2" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_2" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a la eficiencia, entendida como las actividades realizadas para obtener el resultado?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtP1_2" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_2" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                3. Cumplimiento de tiempos pactados (cronograma).
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="RBP1_3" ErrorMessage="*" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left">
                                    <asp:RadioButtonList ID="RBP1_3" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_3" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de tiempos pactados (cronograma)?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtP1_3" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_3" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                4.&nbsp;Cumplimiento de los objetivos.:
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="RBP1_4" ErrorMessage="*" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left">

                                    <asp:RadioButtonList ID="RBP1_4" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_4" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de los objetivos.?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtP1_4" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_4" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                5.&nbsp;Cumplimiento de privacidad de datos y confidencialidad.</label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left; padding-top: 10px">
                                    <asp:RadioButtonList ID="RBP1_5" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                                <div style="float: left; padding-top: 10px">
                                    <asp:RadioButtonList ID="RBP1_5_96" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="NoAplica" Value="96"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 30px">
                                    <asp:CustomValidator ID="RequiredFieldValidator13" runat="server" ClientValidationFunction="validarP1_5" ControlToValidate="RBP1_5_96" ErrorMessage="*" ForeColor="Red" ValidateEmptyText="true" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_5" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de privacidad de datos y confidencialidad.?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtP1_5" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_5" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                6.&nbsp;Cumplimiento de estándares ISO 9001 – 20252 requeridos.</label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left; padding-top: 10px">
                                    <asp:RadioButtonList ID="RBP1_6" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                                <div style="float: left; padding-top: 10px">
                                    <asp:RadioButtonList ID="RBP1_6_96" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="NoAplica" Value="96"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 30px">
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validarP1_6" ControlToValidate="RBP1_6_96" ErrorMessage="*" ForeColor="Red" ValidateEmptyText="true" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_6" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a cumplimiento de estándares ISO 9001 – 20252 requeridos.?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtP1_6" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_6" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="width: 100%">
                            <label>
                                7. Resultados Sesiones Operaciones Cualitativas (Aplica para proveedores de Operaciones Cualitativas).</label>
                            <div style="margin: auto; width: 80%">
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más baja
                                    </label>
                                </div>
                                <div style="float: left">
                                    <asp:RadioButtonList ID="RBP1_7" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 40px">
                                    <label>
                                        Calificación más alta
                                    </label>
                                </div>
                                <div style="float: left; padding-top: 10px">
                                    <asp:RadioButtonList ID="RBP1_7_96" runat="server" AutoPostBack="true" RepeatColumns="11" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="NoAplica" Value="96"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left; padding-top: 30px">
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="validarP1_7" ControlToValidate="RBP1_7_96" ErrorMessage="*" ForeColor="Red" ValidateEmptyText="true" />
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                        </div>
                        <div style="margin: auto; width: 80%">
                            <asp:Panel ID="pnlTxtP1_7" runat="server" Visible="false">
                                <label>
                                    ¿Podría decirme por qué ha dado usted esa calificación a resultados Sesiones Operaciones Cualitativas (Aplica para proveedores de Operaciones Cualitativas).?
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtP1_7" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtP1_7" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </asp:Panel>
                        </div>

                        <br></br>
                        <br></br>
                        <br></br>
                        <div style="clear: both">
                            <div style="width: 100%">
                                <label>
                                    P2. Teniendo en cuenta las calificaciones asignadas, Usted considera que la acción a seguir con
                                <asp:Label ID="lblProveedor2" runat="server" Style="color: white"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="RBP2" ErrorMessage="*" ForeColor="Red">
                                    </asp:RequiredFieldValidator>
                                </label>
                                <div style="margin: auto; width: 80%">
                                    <asp:RadioButtonList ID="RBP2" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="Continuar contratando sus servicios" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Continua con plan de acción" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="No continuar contratando sus servicios" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div>
                                <asp:Panel ID="pnlTxtP3" runat="server" Visible="false">
                                    <label>
                                        P3. Explique las razones por las cuales no continúa contratando los servicios de
                                        <asp:Label ID="lblProveedor3" runat="server" Style="color: white"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtP3" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP3" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                </asp:Panel>
                            </div>

                        </div>

                        <asp:Button ID="btnEnviar" Text="Guardar Evaluación" runat="server" CssClass="button" Width="150px" />
                </asp:Panel>
            </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="LoadFiles">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
