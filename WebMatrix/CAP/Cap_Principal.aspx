<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="Cap_Principal.aspx.vb" Inherits="WebMatrix.Cap_Principal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/CAP/UserControls/UC_Producto.ascx" TagName="UC_Producto" TagPrefix="uc1" %>

<%@ Register Src="~/CAP/UserControls/UC_Preguntas.ascx" TagName="UC_Preguntas" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Styles/TabsStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .mt-15 > input {
            margin-top: 15px;
        }

        .style2 {
            width: 59px;
        }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .style3 {
            width: 127px;
        }

        .style6 {
            width: 202px;
        }

        .style7 {
            height: 224px;
        }

        .style8 {
            width: 210px;
        }

        .style9 {
            height: 23px;
        }

        table {
            text-align: center
        }

        tr {
            text-align: left
        }

        th {
            text-align: left
        }

        td {
            text-align: left
        }

        .centrar {
            text-align: center
        }

        .style11 {
            width: 144px;
        }

        .style12 {
            width: 152px;
        }

        .style13 {
            width: 91px;
        }



        .style14 {
            height: 17px;
        }



        .style15 {
            text-align: center;
            color: #FFFFFF;
            font-size: large;
        }



        .style16 {
            width: 99px;
        }



        .style17 {
            width: 94px;
        }



        .style18 {
            width: 110px;
        }

        .style19 {
            width: 130px;
        }

        .style20 {
            width: 132px;
        }

        .style21 {
            width: 946px;
        }

        .style22 {
            width: 227px;
        }



        .style23 {
            color: #FFFFFF;
        }



        .style24 {
            font-size: medium;
        }



        .style25 {
            width: 551px;
        }



        .style26 {
            width: 190px;
        }

        .style27 {
            width: 80px;
        }

        .style28 {
            width: 81px;
        }



        .style29 {
            width: 143px;
        }

        .style30 {
            width: 136px;
        }

        .field_set {
            border: 1px solid #FFFFFF;
        }


        /* ajax__tab_lightblue-theme theme (images/lightblue.jpg) */
        .ajax__tab_lightblue-theme .ajax__tab_header {
            font-family: arial,helvetica,clean,sans-serif;
            font-size: small;
            border-bottom: solid 5px #c2e0fd;
        }

            .ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_outer {
                background: url(images/lightblue.jpg) #d8d8d8 repeat-x;
                margin: 0px 0.16em 0px 0px;
                padding: 1px 0px 1px 0px;
                vertical-align: bottom;
                border: solid 1px #a3a3a3;
                border-bottom-width: 0px;
            }

            .ajax__tab_lightblue-theme .ajax__tab_header .ajax__tab_tab {
                color: #000;
                padding: 0.35em 0.75em;
                margin-right: 0.01em;
            }

        .ajax__tab_lightblue-theme .ajax__tab_hover .ajax__tab_outer {
            background: url(images/lightblue.jpg) #bfdaff repeat-x left -1300px;
        }

        .ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_tab {
            color: #000;
        }

        .ajax__tab_lightblue-theme .ajax__tab_active .ajax__tab_outer {
            background: url(images/lightblue.jpg) #ffffff repeat-x left -1400px;
        }

        .ajax__tab_lightblue-theme .ajax__tab_body {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            padding: 0.25em 0.5em;
            background-color: #ffffff;
            border: solid 1px #808080;
            border-top-width: 0px;
        }

        .auto-style1 {
            font-size: 10px;
        }

        .auto-style2 {
            font-size: small;
        }
    </style>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>




    <script type="text/javascript">

        function FilterGrid() {

            $(document).ready(function () {

                //
                // Client Side Search (Autocomplete)
                // Get the search Key from the TextBox
                // Iterate through the 1st Column.
                // td:nth-child(1) - Filters only the 1st Column
                // If there is a match show the row [$(this).parent() gives the Row]
                // Else hide the row [$(this).parent() gives the Row]
                $('#txtNomCiudad').keyup(function (event) {
                    var searchKey = $(this).val().toLowerCase();

                    $("#gvMuestraNueva tr td:nth-child(2)").each(function () {
                        var cellText = $(this).text().toLowerCase();
                        if (cellText.indexOf(searchKey) >= 0) {
                            $(this).parent().show();
                        }
                        else {
                            $(this).parent().hide();
                        }
                    });
                });
            });

        }
    </script>

    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999' >" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "Imagenes/minus.png");
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "Imagenes/plus.png");
            $(this).closest("tr").next().remove();
        });


        function Sumar() {

            document.getElementById('<%= CCtxtTotalPorcentaje.ClientID %>').value = Number(document.getElementById('CCPorIntercep').value) + Number(document.getElementById('CCPorRecluta').value);


        }

        function Multiplicar() {
            document.getElementById('<%= CCtxtTotalProd.ClientID %>').value = document.getElementById('CCTxtUniProductos').value * document.getElementById('CCTxtValorUniProd').value;
        }

        function TotalDias() {

            document.getElementById('<%= txtDiasTotal.ClientID %>').value = Number(document.getElementById('<%= txtDiasDiseno.ClientID %>').value) + Number(document.getElementById('<%= txtDiasCampo.ClientID %>').value) + Number(document.getElementById('<%= txtDiasProceso.ClientID %>').value) + Number(document.getElementById('<%= txtDiasInformes.ClientID %>').value);

        }



        function clearContents() {
            var AsyncFileUpload = $get("<%=AsyncFileUpload1.ClientID%>");
            var txts = AsyncFileUpload.getElementsByTagName("input");
            for (var i = 0; i < txts.length; i++) {
                if (txts[i].type == "file") {
                    txts[i].value = "";
                    txts[i].style.backgroundColor = "transparent";
                }
            }
        }

        function uploadComplete(sender) {
            clearContents()
        }

        function HideModalPopup() {
            $find("lkgm_ModalPopupExtender").hide();
        }

        function ConfirmOnSubmit(item) {
            if (confirm("Realmente dese marcar la propuesta como REVISADA o NO REVISADA?") == true)
                return true;
            else
                return false;
        }



        function moverDiv() {

            $('#accordion').appendTo(("#ContenedorAccordion"));
        }

        function CreateAccordion() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

        }

        function CreateAccordionEnt() {
            $('#accordionEnt').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

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
    <asp:HiddenField ID="hfOperaciones" runat="server" />
    <table style="width: 100%;">
        <tr>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td>PROPUESTA:</td>
                        <td>
                            <asp:Label ID="lblPropuesta" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>ALTERNATIVA:
                        </td>
                        <td>
                            <asp:Label ID="lblAlternativa" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>JOBBOOK:</td>
                        <td>
                            <asp:Label ID="iqLbljobBook" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:LinkButton ID="lkSalir" runat="server" Font-Bold="True">VOLVER</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>PLAZO:</td>
                        <td>
                            <asp:Label ID="lblplazo" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>ANTICIPO:</td>
                        <td>
                            <asp:Label ID="lblAnticipo" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>SALDO:</td>
                        <td>
                            <asp:Label ID="lblSaldo" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="text-align: right">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>TASA&nbsp; CAMBIO:</td>
                        <td>
                            <asp:Label ID="lblTasa" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="text-align: right">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td class="style22">NOMBRE PROPUESTA:</td>
                        <td class="style21">
                            <asp:Label ID="lblNomPropuesta" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:HiddenField ID="hfTope" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style22">NOBRE CLIENTE:</td>
                        <td class="style21">
                            <asp:Label ID="lblNomCliente" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="style22">DESCRIPCION:</td>
                        <td class="style21">
                            <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine"
                                Width="700px" Height="30px" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style22">NUMERO MEDICIONES:</td>
                        <td class="style21">
                            <asp:TextBox ID="txtNumMediciones" runat="server" Width="80px">1</asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtNumMediciones_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Numbers"
                                TargetControlID="txtNumMediciones">
                            </asp:FilteredTextBoxExtender>
                            &nbsp; SALE CADA&nbsp;&nbsp;
                            <asp:TextBox ID="txtMesesMediciones" runat="server" Width="80px">1</asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtMesesMediciones_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Numbers"
                                TargetControlID="txtMesesMediciones">
                            </asp:FilteredTextBoxExtender>
                            &nbsp; MESES</td>
                        <td class="style21">
                            <a style="font-style: normal; font-weight: normal; color: black;">
                                <asp:CheckBox ID="chbObserver" Enabled="true" runat="server" Text="Observer" /></a></td>
                    </tr>
                    <tr>
                        <td class="style22">DIAS 
                            HABILES ESTIMADOS:</td>
                        <td class="style21">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    &nbsp;DISENO:<asp:TextBox ID="txtDiasDiseno"
                                        runat="server"
                                        Width="80px" ClientIDMode="Static" onchange="Javascript:TotalDias();">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                        TargetControlID="txtDiasDiseno">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CAMPO :<asp:TextBox ID="txtDiasCampo" runat="server"
                                        Width="80px" ClientIDMode="Static" onchange="Javascript:TotalDias();">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                        TargetControlID="txtDiasCampo">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; PROCESO:<asp:TextBox
                                        ID="txtDiasProceso" runat="server" Width="80px" ClientIDMode="Static" onchange="Javascript:TotalDias();">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                        TargetControlID="txtDiasProceso">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; INFORMES:<asp:TextBox ID="txtDiasInformes"
                                        runat="server" Width="80px" ClientIDMode="Static" onchange="Javascript:TotalDias();">0</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                                        TargetControlID="txtDiasInformes">
                                    </asp:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; TOTAL:<asp:TextBox ID="txtDiasTotal"
                                        runat="server" Enabled="False"
                                        Width="80px" ClientIDMode="Static"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnGuardarDatosGenerales" runat="server" Text="Guardar"
                                BackColor="#CC3300" Font-Bold="True" ForeColor="White" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td class="style7">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td class="style12">ALTERNATIVAS:</td>
                                <td class="style11">
                                    <asp:Button ID="btnAlternativas" runat="server" Text="Nueva alternativa" />
                                </td>
                                <td class="style13">
                                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" />
                                </td>
                                <td class="style13">
                                    <asp:Label ID="lblalternativaNum" runat="server" Font-Bold="True"
                                        ForeColor="White"></asp:Label>
                                </td>
                                <td class="style16">
                                    <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnDuplicarAlternativa" runat="server" BackColor="#CC3300"
                                                Font-Bold="True" ForeColor="White" Text="Duplicar Alternativa" Width="143px" OnClientClick="return confirm('Esta seguro que desea duplicar la alternativa actual?');" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="btnJobBookExt" runat="server" Text="JBE"
                                        BackColor="#CC3300" Font-Bold="True" ForeColor="White" Width="80px"
                                        Visible="False" />
                                </td>
                                <td>&nbsp;</td>
                                <td style="text-align: right">
                                    <asp:Button ID="btnJobBookInt" runat="server" Text="JBI"
                                        BackColor="#CC3300" Font-Bold="True" ForeColor="White" Width="80px"
                                        Visible="False" />
                                </td>
                                <td>&nbsp;</td>
                                <td style="text-align: right">
                                    <asp:Button ID="btnCambioGeneralGM" runat="server" Text="CAMBIAR GM"
                                        BackColor="#CC3300" Font-Bold="True" ForeColor="White" Visible="False" />
                                </td>
                                <td style="text-align: right">&nbsp;</td>
                            </tr>
                        </table>
                        &nbsp;<asp:HiddenField ID="hfTecnica" runat="server" />

                        <asp:GridView ID="gvCuanti" runat="server" AutoGenerateColumns="False"
                            Width="100%" CssClass="displayTable" EmptyDataText="Sin datos"
                            ShowFooter="True">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <img alt="Click to show/hide presupuestos" style="cursor: pointer" src="Imagenes/plus.png" />
                                        <%--  Style="display: none"   --%>
                                        <asp:Panel ID="pnlNacionaless" runat="server" Style="display: none">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server"
                                                ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvNacionales" runat="server" AutoGenerateColumns="False"
                                                        Width="90%" OnRowCommand="gvNacionales_RowCommand"
                                                        OnRowCreated="gvNacionales_RowCreated"
                                                        OnRowDataBound="gvNacionales_RowDataBound">
                                                        <Columns>
                                                            <asp:ButtonField DataTextField="IDTECNICA" HeaderText="Tec"
                                                                CommandName="TEC" />
                                                            <asp:BoundField DataField="NACIONAL" HeaderText="Id"
                                                                ItemStyle-Width="150px">
                                                                <ControlStyle Width="30px" />
                                                                <FooterStyle Width="80px" />
                                                                <HeaderStyle Width="30px" />
                                                                <ItemStyle Width="30px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FASE" HeaderText="Fase" />
                                                            <asp:BoundField DataField="METCODIGO" HeaderText="MET" />
                                                            <asp:BoundField DataField="METNOMBRE" HeaderText="Metodologia" />
                                                            <asp:ButtonField DataTextField="ActSubGasto" HeaderText="Sub. Ext"
                                                                CommandName="SUB" DataTextFormatString="{0:c0}" />
                                                            <asp:ButtonField DataTextField="ACTSUBCOSTO" DataTextFormatString="{0:c0}"
                                                                HeaderText="Sub. Int" Text="Button" CommandName="SUB" Visible="False" />
                                                            <asp:BoundField DataField="COSTODIRECTO" DataFormatString="{0:C0}"
                                                                HeaderText="Venta Operaciones" />
                                                            <asp:BoundField DataField="VALORVENTA" DataFormatString="{0:C0}"
                                                                HeaderText="Valor Venta" />
                                                            <asp:BoundField DataField="MUESTRA" HeaderText="Muestra" />
                                                            <asp:BoundField DataField="VALORUNITARIO" HeaderText="V.Unitario"
                                                                DataFormatString="{0:c0}" />
                                                            <asp:ButtonField DataTextField="GROSSMARGIN" DataTextFormatString="{0:n}"
                                                                HeaderText="GM%" CommandName="GM">
                                                                <ItemStyle Font-Bold="True" Font-Size="Medium" />
                                                            </asp:ButtonField>
                                                            <asp:TemplateField ShowHeader="False" HeaderText="Rev.">
                                                                <ItemTemplate>


                                                                    <asp:ImageButton ID="ImageButton1" runat="server"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="REVISADO"
                                                                        ImageUrl="~/CAP/Imagenes/checkbox-checked.png" Text="Rev" />


                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Aprob.">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Eval("APROBADO")%>'
                                                                        Enabled="False" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ejecu." ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="Button2" runat="server" CausesValidation="false"
                                                                        CommandName="CONTROL" Text="Ver" CommandArgument='<%# Container.DataItemIndex %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                        CommandName="BORRAR" Text="DEL"
                                                                        OnClientClick="return confirm('Realmente desea borrar este presupuesto?')"
                                                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="Button3" runat="server" CausesValidation="false"
                                                                        CommandName="JBEXT" Text="JBE"
                                                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="Button4" runat="server" CausesValidation="false"
                                                                        CommandName="JBINT" Text="JBI"
                                                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="TEC" DataTextField="IDTECNICA"
                                    HeaderText="ID. TECNICA" />
                                <asp:HyperLinkField DataTextField="Tecnica" HeaderText="TECNICA" />
                                <asp:BoundField DataField="ACTSUBGASTO" DataFormatString="{0:c0}"
                                    HeaderText="SUB. EXT" />
                                <asp:BoundField DataField="ActSubCosto" DataFormatString="{0:C0}"
                                    HeaderText="SUB. INT" Visible="False" />
                                <asp:BoundField DataField="CostoDirecto" HeaderText="VENTA OPERACIONES"
                                    DataFormatString="{0:C0}" ApplyFormatInEditMode="True" />
                                <asp:BoundField DataField="ValorVenta" HeaderText="VALOR VENTA"
                                    DataFormatString="{0:C0}" />
                                <asp:BoundField DataField="GrossMargin" DataFormatString="{0:n}"
                                    HeaderText="%GM" />

                            </Columns>
                            <FooterStyle Font-Bold="True" />
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td class="style8">OBSERVACIONES::</td>
                        <td>
                            <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine"
                                Width="100%" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style8">&nbsp;</td>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style25">&nbsp;</td>
                                    <td>VALOR FINANCIACION:</td>
                                    <td>
                                        <asp:Label ID="lblValorFinanciacion" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lkb1" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="lkb1_ModalPopupExtender" runat="server"
                    BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
                    DropShadow="True" DynamicServicePath="" Enabled="True"
                    PopupControlID="Panel1" TargetControlID="lkb1">
                </asp:ModalPopupExtender>
                <asp:LinkButton ID="lkgm" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="lkgm_ModalPopupExtender" runat="server"
                    BackgroundCssClass="modalBackground" PopupControlID="PanelGM"
                    TargetControlID="lkgm" CancelControlID="btnCancelarAjusteGM"
                    DropShadow="True" ClientIDMode="Static">
                </asp:ModalPopupExtender>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="Button7" runat="server" Text="Button" Visible="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="PanelGM" runat="server" BackColor="Black" Height="450px"
                    Width="560px" CssClass="style23">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%; height: 350px;">
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="style23">Valor venta:</td>
                                                <td>
                                                    <asp:TextBox ID="txtValorVentaSimular" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style23">
                                                    <asp:Button ID="btnSimular" runat="server" Text="Simular GM" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGMsimulado" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnModificarGM_1" runat="server" Text="Modificar" OnClientClick="return confirm('Realmente desea efectuar la modificacion?');" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>!No olvide digitar los valores decimales separados por coma(,)<asp:HiddenField
                                        ID="hfTipoCalculo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>GM Unidad:</td>
                                                <td>
                                                    <asp:TextBox ID="txtNuevoGM" runat="server"
                                                        ForeColor="#003300" Width="80px" Font-Bold="True"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtNuevoGM_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=","
                                                        TargetControlID="txtNuevoGM">
                                                    </asp:FilteredTextBoxExtender>
                                                    *<span class="auto-style1">En blanco para mantener el actual</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblgmActual" runat="server" ForeColor="Black"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>GM Operaciones:</td>
                                                <td>
                                                    <asp:TextBox ID="txtGMOpera" runat="server" Font-Bold="True"
                                                        ForeColor="#003300" Width="80px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtGMOpera_FilteredTextBoxExtender" runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Custom, Numbers" TargetControlID="txtGMOpera" ValidChars=",">
                                                    </asp:FilteredTextBoxExtender>
                                                    *E<span class="auto-style1">n blanco para mantener el actual </span>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSimValorVenta" runat="server" Text="Simular venta" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblValorVentaSimulado" runat="server" Font-Bold="True"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnModificarGM_2" runat="server" Text="Modificar" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center" class="style23">
                                        <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%" Height="140px" ScrollBars="Both" CssClass="ajax__tab_lightblue-theme" VerticalStripWidth="140px">
                                            <asp:TabPanel ID="TabPanelG1" runat="server" HeaderText="TabPanel1">
                                                <HeaderTemplate>
                                                    JOBBOOK INTERNO
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:GridView ID="GVJBI" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="displayTable">
                                                        <Columns>
                                                            <asp:BoundField DataField="JBDESCRIPCION" HeaderText="DESCRIPCION" />
                                                            <asp:BoundField DataField="TOTALCOSTO" HeaderText="COSTO" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanelG2" runat="server" HeaderText="TabPanel2">
                                                <HeaderTemplate>
                                                    JOBBOOK EXTERNO
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:GridView ID="GVJBE" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="displayTable">
                                                        <Columns>
                                                            <asp:BoundField DataField="JBDESCRIPCION" HeaderText="DESCRIPCION" />
                                                            <asp:BoundField DataField="TOTALCOSTO" HeaderText="COSTOS" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                        </asp:TabContainer>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style23" style="text-align: center">
                                        <asp:Panel ID="pnNotificacion" runat="server" Visible="False">
                                            <strong><span class="style24">Si usted no es un usuario autorizado presione el boton de enviar notificacion<br />
                                            </span></strong>&nbsp;<asp:Button ID="EnviarNotificacion0" runat="server" BackColor="#CC3300" OnClientClick="HideModalPopup();" Text="Enviar notificacion" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center" class="style15">
                                        <asp:Label ID="lblContrasena" runat="server" Text="Contrasena:" Visible="False"></asp:Label>
                                        <asp:TextBox ID="gmTxtContrasena" runat="server" TextMode="Password"
                                            Visible="False" Width="100px"></asp:TextBox>
                                        &nbsp;<span class="auto-style2">*Presione nuevamente el boton modificar respectivo</span></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">&nbsp;<asp:Button ID="btnCancelarAjusteGM" runat="server" Text="Cerrar" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BackColor="#1F9E97" Height="580px"
                    Width="840px">

                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upTabContainer" runat="server" ChildrenAsTriggers="False"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>


                                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1"
                                            CssClass="" Height="550px" ScrollBars="Auto" Width="100%">
                                            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                                                <HeaderTemplate>CATI</HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="panel100" runat="server">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>Fase:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="CatiLstFase" runat="server"></asp:DropDownList></td>
                                                                                    <td>
                                                                                        <p style="width: 60px; float: left;" id="anoSiguiente" runat="server"></p>
                                                                                        <asp:CheckBox ID="CatichkAñoSiguiente" CssClass="mt-15" runat="server" Checked="true" /></td>
                                                                                    <td>
                                                                                        <p style="width: 60px; float: left;" id="anoActual" runat="server"></p>
                                                                                        <asp:CheckBox ID="CatichkAñoActual" CssClass="mt-15" runat="server" Checked="false" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style3">Metodologia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="CatiLstMetodologia" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style3">Grupo objetivo:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtGrupoObjetivo" runat="server" MaxLength="250"
                                                                                            TextMode="MultiLine" Width="500px"></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <uc1:UC_Producto ID="UC_Producto1" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="chkProcesos" runat="server" RepeatColumns="10" RepeatDirection="Horizontal" Style="margin-left: 1px" TextAlign="Left"
                                                                                Width="100%">
                                                                            </asp:CheckBoxList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>N.Procesos DataClean:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CatiTxtNProcesosDC" runat="server" Width="30px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CatiTxtNProcesosDC">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos TopLines:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CatiTxtNProcesosTL" runat="server" Width="30px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CatiTxtNProcesosTL">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos Tablas:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CatiTxtNProcesosTablas" runat="server" Width="30px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CatiTxtNProcesosTablas">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos Archivos:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CatiTxtNProcesosBases" runat="server" Width="30px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CatiTxtNProcesosBases">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Incidencia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="lstIncidencia_101" runat="server" Width="70px"></asp:DropDownList>
                                                                                    </td>
                                                                                    <td>Productividad:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProductividad" runat="server" Font-Bold="True" ForeColor="#FF3300" Width="60px"> </asp:TextBox>
                                                                                    </td>
                                                                                    <td>Marcaciones no efectivas:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtMarcNoEfectivas" runat="server" Enabled="False" Width="60px"></asp:TextBox></td>
                                                                                    <td>Num encuestadores:</td>
                                                                                    <td>
                                                                                        <asp:Label ID="CatiLblNumEncuestadores" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td>&nbsp;</td>
                                                                                    <td>&nbsp;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style17">Tipo muestra:</td>
                                                                                    <td>
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td class="style6">
                                                                                                    <asp:DropDownList ID="CatiLstTipoMuestra" runat="server" Width="200px"></asp:DropDownList></td>
                                                                                                <td class="style2">Cantidad:</td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="CatiTxtCantidad" runat="server" Width="80px"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                                                                        runat="server" FilterType="Numbers" TargetControlID="CatiTxtCantidad">
                                                                                                    </asp:FilteredTextBoxExtender>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnAgregarMueCati" runat="server" Text="Agregar" /></td>
                                                                                                <td>&#160;</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style17">Total muestra:</td>
                                                                                    <td>
                                                                                        <asp:Label ID="CatiLblTotalMuestra" runat="server"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upGVMUestraCati" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="gvMuestracati" runat="server" AutoGenerateColumns="False"
                                                                                        CssClass="displayTable" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField ShowHeader="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="DEL"
                                                                                                        OnClientClick="return confirm('Realmente desea borrar la muestra ?')"
                                                                                                        Text="Borrar" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="IDENTIFICADOR" HeaderText="ID" />
                                                                                            <asp:BoundField DataField="TIPO_MUESTRA" HeaderText="TIPO MUESTRA" />
                                                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style18">OBSERVACIONES:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CatiTxtObservaciones" runat="server" MaxLength="300"
                                                                                            TextMode="MultiLine" Width="550px">NO TENGO OBSERVACIONES</asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                                                <HeaderTemplate>CARA A CARA&nbsp;</HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Panel ID="PanelCaraCara" runat="server">
                                                        <asp:UpdatePanel ID="upCaraCara" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>Fase:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="CCLstFases" runat="server" AutoPostBack="True"></asp:DropDownList></td>

                                                                                    <td>AÑO 2023:
                                                                                        <asp:CheckBox ID="chkAñoSiguiente" runat="server" Checked="true" /></td>
                                                                                    <td>AÑO 2022:
                                                                                        <asp:CheckBox ID="chkAñoActual" runat="server" Checked="false" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Metodologia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="CCLstMetodologia" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Grupo objetivo:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CCTxtGrupoObj" runat="server" MaxLength="250"
                                                                                            TextMode="MultiLine" Width="500px"></asp:TextBox></td>
                                                                                    <td>&#160;</td>
                                                                                    <td>&#160;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <uc1:UC_Producto ID="UC_Producto2" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="CCchkProcesos" runat="server" RepeatColumns="10" RepeatDirection="Horizontal" Style="margin-left: 1px" TextAlign="Left" Width="100%" Font-Italic="False"></asp:CheckBoxList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="PanelLocalizacion" runat="server" Visible="False">
                                                                                <table style="width: 100%;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table style="width: 100%;">
                                                                                                <tr>
                                                                                                    <td>Porcentaje por interceptacion:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCPorIntercep" runat="server" ClientIDMode="Static"
                                                                                                            onchange="Javascript:Sumar();" Width="80px"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                                                ID="CCPorIntercep_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                                                FilterType="Numbers" TargetControlID="CCPorIntercep">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td>Porcentaje por reclutamiento:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCPorRecluta" runat="server" ClientIDMode="Static"
                                                                                                            onchange="Javascript:Sumar();" Width="80px"></asp:TextBox><asp:FilteredTextBoxExtender ID="CCPorRecluta_FilteredTextBoxExtender"
                                                                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                                                                TargetControlID="CCPorRecluta">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td>Total:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCtxtTotalPorcentaje" runat="server" Enabled="False"
                                                                                                            Width="80px"></asp:TextBox></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Unidades producto:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCTxtUniProductos" runat="server" ClientIDMode="Static"
                                                                                                            onchange="Javascript:Multiplicar();" Width="80px"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                                                ID="CCTxtUniProductos_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                                                FilterType="Numbers" TargetControlID="CCTxtUniProductos">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td>Valor unitario producto:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCTxtValorUniProd" runat="server" ClientIDMode="Static"
                                                                                                            onchange="Javascript:Multiplicar();" Width="80px"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                                                ID="CCTxtValorUniProd_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                                                FilterType="Numbers" TargetControlID="CCTxtValorUniProd">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td>Total:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCtxtTotalProd" runat="server" Enabled="False" Width="80px"></asp:TextBox></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Tipos de CLT:</td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="CCLstTipoCLT" runat="server" Width="150px">
                                                                                                            <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                                                                            <asp:ListItem Value="1">Cafe internet</asp:ListItem>
                                                                                                            <asp:ListItem Value="2">NIVEL C (BAJO)</asp:ListItem>
                                                                                                            <asp:ListItem Value="3">NIVEL B (MEDIO)</asp:ListItem>
                                                                                                            <asp:ListItem Value="4">NIVEL A (ALTO)</asp:ListItem>
                                                                                                        </asp:DropDownList></td>
                                                                                                    <td>Alquiler de equipos:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCtxtAlqEquip" runat="server" Width="100px"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                                            ID="CCtxtAlqEquip_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                                            FilterType="Numbers" TargetControlID="CCtxtAlqEquip">
                                                                                                        </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                    <td>Encuestadores por punto:</td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="CCEncXPunto" runat="server"
                                                                                                            ToolTip="Impacto en costo de supervision" Width="80px">2</asp:TextBox><asp:FilteredTextBoxExtender ID="CCEncXPunto_FilteredTextBoxExtender"
                                                                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                                                                TargetControlID="CCEncXPunto">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Requiere apoyo logistico:</td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="CCchkApoyoLogis" runat="server" /></td>
                                                                                                    <td>Acceso internet:</td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="CCchkAccesoInter" runat="server" /></td>
                                                                                                    <td>&nbsp;</td>
                                                                                                    <td>&nbsp;</td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style29">N.Procesos DataClean:</td>
                                                                                    <td class="style30">
                                                                                        <asp:TextBox ID="CCTxtNumProcDC" runat="server" Width="30px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CCTxtNumProcDC">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td class="style29">N.Procesos TopLines:</td>
                                                                                    <td class="style30">
                                                                                        <asp:TextBox ID="CCTxtNumProcTL" runat="server" Width="30px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CCTxtNumProcTL">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td class="style29">N.Procesos Tablas:</td>
                                                                                    <td class="style30">
                                                                                        <asp:TextBox ID="CCTxtNumProcTablas" runat="server" Width="30px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CCTxtNumProcTablas">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td class="style29">N.Procesos Archivos:</td>
                                                                                    <td class="style30">
                                                                                        <asp:TextBox ID="CCTxtNumProcBases" runat="server" Width="30px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="CCTxtNumProcBases">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>

                                                                                    <td>Tablet 100%:
                                                                                        <asp:CheckBox ID="chkTablet" runat="server" />
                                                                                    </td>
                                                                                    <td>Papel 100%:
                                                                                        <asp:CheckBox ID="ChkPapel" runat="server" />
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:CheckBox ID="CCChkDispPropio" Visible="false" runat="server" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="PanelIncidencia" runat="server">
                                                                                <table style="width: 100%;">
                                                                                    <tr>
                                                                                        <td class="style29">Incidencia:</td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="CCLstIncidencia" runat="server">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td>Probabilistico:</td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="CCCHKProbabilistico" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="style29">Productividad:</td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="CCTxtProductividad" runat="server"
                                                                                                Font-Bold="True" ForeColor="#FF3300" Width="80px"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>Num encuestadores:</td>
                                                                                        <td>
                                                                                            <asp:Label ID="CCLblNumEncuetadores" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="UpMistery" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel ID="PanelMistery" runat="server" Visible="False">
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table style="width: 100%;">
                                                                                                        <tr>
                                                                                                            <td>Tipo servicio:</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="lstTipoServicio" runat="server" AutoPostBack="True">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>Evidencia:</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="lstEvidencia" runat="server" AutoPostBack="True"
                                                                                                                    Width="100px">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>Tipo evidencia:</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="lstTipoEvidencia" runat="server" AutoPostBack="True">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>Tiempo critica:</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="CCTxtTiempoCritica" runat="server" Width="100px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>Contactos:</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="lstNumContactos" runat="server" AutoPostBack="True"
                                                                                                                    Width="100px">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>Valor:</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtValor" runat="server" Width="100px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Button ID="btnAgergarParamMistery" runat="server" Text="Agregar" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:GridView ID="gvTarifasMistery" runat="server" CssClass="displayTable"
                                                                                                        Width="100%" AutoGenerateColumns="False">
                                                                                                        <Columns>
                                                                                                            <asp:ButtonField ButtonType="Button" CommandName="Borrar" Text="Borrar" />
                                                                                                            <asp:TemplateField HeaderText="TM_Id">
                                                                                                                <EditItemTemplate>
                                                                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TM_Id") %>'></asp:TextBox>
                                                                                                                </EditItemTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("TM_Id") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="TS_Descripcion" HeaderText="Tipo Servicio" />
                                                                                                            <asp:BoundField DataField="EV_Descripcion" HeaderText="Evidencia" />
                                                                                                            <asp:BoundField DataField="TE_Descripcion" HeaderText="Tipo evidencia" />
                                                                                                            <asp:BoundField DataField="CC_Cantidad" HeaderText="Contactos" />
                                                                                                            <asp:BoundField DataField="TM_Valor" HeaderText="Valor" />
                                                                                                            <asp:BoundField DataField="TE_TiempoCritica" HeaderText="Tiemp Critica" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td>Departamento:</td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="CCLstDepartamento" runat="server" AutoPostBack="True">
                                                                                                </asp:DropDownList>
                                                                                                <asp:ListSearchExtender ID="CCLstDepartamento_ListSearchExtender"
                                                                                                    runat="server" Enabled="True" QueryPattern="Contains"
                                                                                                    TargetControlID="CCLstDepartamento">
                                                                                                </asp:ListSearchExtender>
                                                                                            </td>
                                                                                            <td>Ciudad:</td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="CCLstCiudad" runat="server">
                                                                                                </asp:DropDownList>
                                                                                                <asp:ListSearchExtender ID="CCLstCiudad_ListSearchExtender" runat="server"
                                                                                                    Enabled="True" QueryPattern="Contains" TargetControlID="CCLstCiudad">
                                                                                                </asp:ListSearchExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>DIFICULTAD&nbsp; MUESTRA:</td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="CCLstTipoMuestra" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>Cantidad:</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="CCTxtCantidad" runat="server" Height="16px" Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                                                                                    FilterType="Numbers" TargetControlID="CCTxtCantidad">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    <asp:Button ID="CCBtnAgregar" runat="server" Text="Agregar" />
                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>
                                                                                                <asp:Button ID="btnAddMuestra" runat="server" Text="Agregar muestra"
                                                                                                    Visible="False" />
                                                                                            </td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Cargar desde excel:</td>
                                                                                            <td>
                                                                                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server"
                                                                                                    OnClientUploadComplete="uploadComplete" />
                                                                                            </td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>
                                                                                                <asp:Button ID="btnCargarEx" runat="server" Text="Cargar Archivo" />
                                                                                                <asp:LinkButton ID="lkmu" runat="server"></asp:LinkButton>
                                                                                                <asp:ModalPopupExtender ID="lkmu_ModalPopupExtender" runat="server"
                                                                                                    BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True"
                                                                                                    PopupControlID="PanelMuestra" TargetControlID="lkmu">
                                                                                                </asp:ModalPopupExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td>Total muestra:</td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblTotalAlta" runat="server" Font-Bold="True" ForeColor="White"
                                                                                                    Width="100px"></asp:Label>
                                                                                            </td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:GridView ID="CCGvMuestra" runat="server" AllowPaging="True"
                                                                                        CssClass="displayTable" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField ShowHeader="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                                                        CommandName="DEL"
                                                                                                        OnClientClick="return confirm('Realmente desea borrar la muestra de esta ciudad ?')"
                                                                                                        Text="Borrar" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style3">OBSERVACIONES:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="CCTxtObservaciones" runat="server" MaxLength="300"
                                                                                            TextMode="MultiLine" Width="500px">NO TENGO OBSERVACIONES</asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="PanelMuestra" runat="server" BackColor="#178E88" Height="500px"
                                                                                Width="800px">
                                                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td style="color: #FFFFFF">&nbsp;Digite el nombre de la ciudad&nbsp; :
                                                                                        <asp:TextBox ID="txtNomCiudad" runat="server" ClientIDMode="Static"
                                                                                            Width="200px"></asp:TextBox>
                                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Dificultad muestra :<asp:DropDownList ID="lstDificultadNuevaMuestra"
                                                                                                        runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Panel ID="pnlCities" runat="server" Height="220px" ScrollBars="Auto">
                                                                                                        <asp:GridView ID="gvMuestraNueva" runat="server" AutoGenerateColumns="False"
                                                                                                            ClientIDMode="Static" CssClass="displayTable" EmptyDataText="No existen datos"
                                                                                                            Width="100%">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="DEPTID" Visible="False">
                                                                                                                    <EditItemTemplate>
                                                                                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CIUDEP") %>'></asp:TextBox>
                                                                                                                    </EditItemTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("CIUDEP") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="CiuDeptoNombre" HeaderText="DEPARTAMENTO" />
                                                                                                                <asp:TemplateField HeaderText="CIUID" Visible="False">
                                                                                                                    <EditItemTemplate>
                                                                                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CIUCIUDAD") %>'></asp:TextBox>
                                                                                                                    </EditItemTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CIUCIUDAD") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="CIUNOMBRE" HeaderText="CIUDAD" />
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtCantMuestra" runat="server" BackColor="#178E8A"
                                                                                                                            Width="80px" Font-Bold="True" ForeColor="White"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle Width="85px" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="ImageButton2" runat="server"
                                                                                                                            ImageUrl="~/CAP/Imagenes/add.png" Width="20px" CommandName="ADD" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle Width="60px" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="color: #FFFFFF">MUESTRA&nbsp; INGRESADA TOTAL:&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                        <asp:Label ID="lblTotalMuestra1" runat="server"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <asp:Panel ID="pnlCities2" runat="server" Height="200px">
                                                                                                        <asp:GridView ID="gvMuestraIngresada" runat="server" CssClass="displayTable"
                                                                                                            Width="100%" AllowPaging="True" PageSize="5">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField ShowHeader="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                                                                            CommandName="DEL" Text="Borrar" OnClientClick="return confirm('Realmente desea borrar la muestra de esta ciudad ?')" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="btnSalirMu" runat="server" Text="Salir" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                                                <HeaderTemplate>ACTIVIDADES SUBCONTRATADAS</HeaderTemplate>
                                                <ContentTemplate>
                                                    <table style="width: 100%;">
                                                        <caption>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <tr>
                                                                <td style="text-align: right">VALOR TOTAL DE ACTIVIDAES SUBCONTRATADAS:<strong style="text-align: right">&nbsp; 
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="SubLblTotal" runat="server" Font-Bold="True" ForeColor="White"></asp:Label>
                                                                </strong>
                                                                </td>
                                                            </tr>
                                                            <caption>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <caption>
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <tr>
                                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:UpdatePanel ID="upActSub" runat="server" ChildrenAsTriggers="False"
                                                                                UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:Panel ID="PanelSub" runat="server">
                                                                                        <asp:GridView ID="gvActSubCont" runat="server" AutoGenerateColumns="False"
                                                                                            ClientIDMode="Static" CssClass="displayTable" Width="100%">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                                <asp:BoundField DataField="ACTIVIDAD" HeaderText="ACTIVIDAD" />
                                                                                                <asp:BoundField DataField="TIPO" HeaderText="TIPO" />
                                                                                                <asp:TemplateField HeaderText="VALOR">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtValorAct" runat="server" AutoPostBack="True"
                                                                                                                    OnTextChanged="txtValorAct_TextChanged" Text='<%# Bind("VALOR", "{0:C}")%>'
                                                                                                                    Width="100px"></asp:TextBox>
                                                                                                                <asp:FilteredTextBoxExtender ID="txtValorAct_FilteredTextBoxExtender"
                                                                                                                    runat="server" Enabled="True" FilterType="Numbers"
                                                                                                                    TargetControlID="txtValorAct">
                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <EditRowStyle HorizontalAlign="Left" />
                                                                                            <RowStyle HorizontalAlign="Left" />
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>

                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                </caption>
                                                            </caption>
                                                        </caption>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
                                                <HeaderTemplate>SESIONES</HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Panel ID="PanelSesiones" runat="server">
                                                        <asp:UpdatePanel ID="upSesiones" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>




                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>Fase:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="sesLstFase" runat="server" AutoPostBack="True"></asp:DropDownList></td>

                                                                                    <td>AÑO 2023:
                                                                                        <asp:CheckBox ID="seschkAñoSiguiente" runat="server" Checked="TRUE" /></td>
                                                                                    <td>AÑO 2022:
                                                                                        <asp:CheckBox ID="seschkAñoActual" runat="server" Checked="FALSE" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Metodologia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="SesLstMetodologia" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Grupo objetivo:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="SesGrupoObjetivo" runat="server" MaxLength="250"
                                                                                            TextMode="MultiLine" Width="500px"></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upSesProductos" runat="server"
                                                                                UpdateMode="Conditional">
                                                                                <ContentTemplate>

                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td class="style9">Oferta:</td>
                                                                                            <td class="style9">
                                                                                                <asp:DropDownList ID="sesLstOferta" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                            <td class="style9">Producto:</td>
                                                                                            <td class="style9">
                                                                                                <asp:DropDownList ID="sesLstProducto" runat="server"></asp:DropDownList></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Cantidad participantes:</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="SesTxtCantPart" runat="server" Height="16px" Width="100px"></asp:TextBox><asp:FilteredTextBoxExtender ID="SesTxtCantPart_FilteredTextBoxExtender"
                                                                                                    runat="server" Enabled="True" FilterType="Numbers"
                                                                                                    TargetControlID="SesTxtCantPart">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                            <td>:</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="sesChkOpciones" runat="server" RepeatColumns="4"
                                                                                Width="100%">
                                                                            </asp:CheckBoxList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upSessubcontratar" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td class="style27">Subcontratar:</td>
                                                                                            <td class="style26">
                                                                                                <asp:CheckBox ID="SesChkSubcontratar" runat="server" AutoPostBack="True" />
                                                                                            </td>
                                                                                            <td class="style28">Porcentaje:</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="sestxtSubcontratar" runat="server" Visible="False"
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="sestxtSubcontratar_FilteredTextBoxExtender"
                                                                                                    runat="server" Enabled="True" FilterType="Numbers"
                                                                                                    TargetControlID="sestxtSubcontratar">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="SesChkProcesos" runat="server"
                                                                                RepeatDirection="Horizontal" Width="100%">
                                                                            </asp:CheckBoxList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="text-align: left">

                                                                                        <div id="ContenedorAccordion">
                                                                                            <div id="accordion">
                                                                                                <div id="accordion0">
                                                                                                    <h3>
                                                                                                        <a href="#">
                                                                                                            <label>
                                                                                                                Muestra</label></a></h3>
                                                                                                    <asp:UpdatePanel ID="upotr1" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <div class="block">
                                                                                                                <asp:UpdatePanel ID="upSesDeptos" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <table style="width: 100%;">
                                                                                                                            <tr>
                                                                                                                                <td>Departamento:</td>
                                                                                                                                <td>
                                                                                                                                    <asp:DropDownList ID="SesLstDepto" runat="server" AutoPostBack="True">
                                                                                                                                    </asp:DropDownList>
                                                                                                                                </td>
                                                                                                                                <td>Ciudad:</td>
                                                                                                                                <td>
                                                                                                                                    <asp:DropDownList ID="SesLstCiudad" runat="server">
                                                                                                                                    </asp:DropDownList>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                                <table style="width: 100%;">
                                                                                                                    <tr>
                                                                                                                        <td>Dificultad muestra:</td>
                                                                                                                        <td>
                                                                                                                            <asp:DropDownList ID="sesLstTipoMuestra" runat="server" Width="200px">
                                                                                                                                <asp:ListItem Selected="True" Value="0">Seleccione...</asp:ListItem>
                                                                                                                                <asp:ListItem Value="A">ALTA</asp:ListItem>
                                                                                                                                <asp:ListItem Value="M">MEDIA</asp:ListItem>
                                                                                                                                <asp:ListItem Value="B">BAJA</asp:ListItem>
                                                                                                                            </asp:DropDownList>
                                                                                                                        </td>
                                                                                                                        <td>Cantidad:</td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="SesTxtCantidad" runat="server" Width="80px"></asp:TextBox>
                                                                                                                            <asp:FilteredTextBoxExtender ID="SesTxtCantidad_FilteredTextBoxExtender"
                                                                                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                                                                                TargetControlID="SesTxtCantidad">
                                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="upsesBetnAgregar" runat="server">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <asp:Button ID="sesBetnAgregar" runat="server" Text="Agregar" />
                                                                                                                                </ContentTemplate>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>Total&nbsp; muestra:</td>
                                                                                                                        <td>
                                                                                                                            <asp:Label ID="sesLblTotalMuestra" runat="server" Font-Bold="True"
                                                                                                                                ForeColor="White" Text="Label"></asp:Label>
                                                                                                                        </td>
                                                                                                                        <td>&nbsp;</td>
                                                                                                                        <td>&nbsp;</td>
                                                                                                                        <td>&nbsp;</td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                                <asp:UpdatePanel ID="upGVSesMuestra" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:GridView ID="gvSesionesMuestra" runat="server" AllowPaging="True"
                                                                                                                            CssClass="displayTable" PageSize="5" Width="100%">
                                                                                                                            <Columns>
                                                                                                                                <asp:TemplateField ShowHeader="False">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                                                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="DEL"
                                                                                                                                            OnClientClick="return confirm('Realmente desea borrar la muestra de esta ciudad ?')"
                                                                                                                                            Text="Borrar" />
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                            </Columns>
                                                                                                                        </asp:GridView>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </div>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </div>
                                                                                                <div id="accordion1">
                                                                                                    <h3>
                                                                                                        <a href="#">
                                                                                                            <label>
                                                                                                                Horas profesionales
                                                                                                            </label>
                                                                                                        </a>
                                                                                                    </h3>
                                                                                                    <asp:UpdatePanel ID="upHoras" runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <div class="block">
                                                                                                                &nbsp;<asp:GridView ID="gvCargosSesiones" runat="server" AutoGenerateColumns="False"
                                                                                                                    CssClass="displayTable" EmptyDataText="No existen datos para mostrar"
                                                                                                                    Width="100%">
                                                                                                                    <Columns>
                                                                                                                        <asp:BoundField DataField="PGRCODIGO" HeaderText="CODIGO" />
                                                                                                                        <asp:BoundField DataField="PGRDESCRIPCION" HeaderText="DESCRIPCION" />
                                                                                                                        <asp:TemplateField HeaderText="No. HORAS">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtHoras" runat="server" BackColor="#16938D" ForeColor="White"
                                                                                                                                    Width="80px" Text='<%# Bind("HORAS")%>'></asp:TextBox>
                                                                                                                                <asp:FilteredTextBoxExtender ID="txtHoras_FilteredTextBoxExtender"
                                                                                                                                    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtHoras">
                                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                                        </asp:TemplateField>
                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
                                                                                                            </div>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>


                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style19">OBSERVACIONES:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="SesTxtObservaciones" runat="server" MaxLength="300"
                                                                                            TextMode="MultiLine" Width="500px">NO TENGO OBSERVACIONES</asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&#160;</td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                                                <HeaderTemplate>ENTREVISTAS</HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Panel ID="PanelEntrevistas" runat="server">
                                                        <asp:UpdatePanel ID="UPeNTREVISTAS" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>Fase:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="EntLstFase" runat="server" AutoPostBack="True"></asp:DropDownList></td>

                                                                                    <td>AÑO 2023:
                                                                                        <asp:CheckBox ID="EntchkAñoSiguiente" runat="server" Checked="TRUE" /></td>
                                                                                    <td>AÑO 2022:
                                                                                        <asp:CheckBox ID="EntchkAñoActual" runat="server" Checked="FALSE" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Metodologia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="EntLstMetodologia" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Grupo objetivo:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="EntGrupoObjetivo" runat="server" MaxLength="250" TextMode="MultiLine" Width="500px"></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upSesProductos0" runat="server"
                                                                                UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td class="style9">Oferta:</td>
                                                                                            <td class="style9">
                                                                                                <asp:DropDownList ID="EntLstOferta" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                            <td class="style9">Producto:</td>
                                                                                            <td class="style9">
                                                                                                <asp:DropDownList ID="EntLstProducto" runat="server"></asp:DropDownList></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:CheckBoxList ID="EntChkOpciones" runat="server" RepeatColumns="2"
                                                                                            Width="100%">
                                                                                        </asp:CheckBoxList></td>
                                                                                    <td>
                                                                                        <table style="width: 100%;">
                                                                                            <tr>
                                                                                                <td class="style14">Duracion horas:</td>
                                                                                                <td class="style14">
                                                                                                    <asp:RadioButtonList ID="EntRbHoras" runat="server"
                                                                                                        RepeatDirection="Horizontal">
                                                                                                        <asp:ListItem Value="1"></asp:ListItem>
                                                                                                        <asp:ListItem Value="2"></asp:ListItem>
                                                                                                        <asp:ListItem Value="3"></asp:ListItem>
                                                                                                    </asp:RadioButtonList></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upSessubcontratar0" runat="server"
                                                                                UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td class="style27">Subcontratar:</td>
                                                                                            <td class="style26">
                                                                                                <asp:CheckBox ID="EntChkSubcontratar" runat="server" AutoPostBack="True" />
                                                                                            </td>
                                                                                            <td class="style28">Porcentaje:</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="EnttxtSubcontratar" runat="server" Visible="False"
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="EnttxtSubcontratar_FilteredTextBoxExtender"
                                                                                                    runat="server" Enabled="True" FilterType="Numbers"
                                                                                                    TargetControlID="EnttxtSubcontratar">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="EntChkProcesos" runat="server" RepeatDirection="Horizontal" Width="100%">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="accordionEnt">
                                                                                <div id="accordionEnt0">
                                                                                    <h3>
                                                                                        <a href="#">
                                                                                            <label>
                                                                                                Muestra
                                                                                            </label>
                                                                                        </a>
                                                                                    </h3>
                                                                                    <div class="block">

                                                                                        <asp:UpdatePanel ID="upEntDeptos" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <table style="width: 100%;">
                                                                                                    <tr>
                                                                                                        <td>Departamento:</td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="EntLstDepto" runat="server" AutoPostBack="True">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td>Ciudad:</td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="EntLstCiudad" runat="server">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <table style="width: 100%;">
                                                                                                    <tr>
                                                                                                        <td>DIFICULTAD MUESTRA:&nbsp;</td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="EntLstTipoMuestra" runat="server" Width="200px">
                                                                                                                <asp:ListItem Selected="True" Value="0">Seleccione...</asp:ListItem>
                                                                                                                <asp:ListItem Value="A">ALTA</asp:ListItem>
                                                                                                                <asp:ListItem Value="M">MEDIA</asp:ListItem>
                                                                                                                <asp:ListItem Value="B">BAJA</asp:ListItem>
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td>Cantidad:</td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="EntTxtCantidad" runat="server" Width="80px"></asp:TextBox>
                                                                                                            <asp:FilteredTextBoxExtender ID="EntTxtCantidad_FilteredTextBoxExtender"
                                                                                                                runat="server" Enabled="True" FilterType="Numbers"
                                                                                                                TargetControlID="EntTxtCantidad">
                                                                                                            </asp:FilteredTextBoxExtender>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="upEntBtnAgregar" runat="server">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="EntBtnAgregar" runat="server" Text="Agregar" />
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Total de la muestra:</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="entLblTotalMuestra" runat="server" Font-Bold="True"
                                                                                                                ForeColor="White"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>&nbsp;</td>
                                                                                                        <td>&nbsp;</td>
                                                                                                        <td>&nbsp;</td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <asp:UpdatePanel ID="upGvEntrevistasMuestra" runat="server"
                                                                                                    UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:GridView ID="gvEntrevistasMuestra" runat="server" AllowPaging="True"
                                                                                                            CssClass="displayTable" PageSize="5" Width="100%">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField ShowHeader="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Button ID="Button1" runat="server" CausesValidation="false"
                                                                                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="DEL"
                                                                                                                            OnClientClick="return confirm('Realmente desea borrar  al muestra de esta ciudad ?')"
                                                                                                                            Text="Borrar" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                                <br />
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>

                                                                                    </div>
                                                                                </div>
                                                                                <div id="accordionEnt1">
                                                                                    <h3>
                                                                                        <a href="#">
                                                                                            <label>
                                                                                                Horas profesionales
                                                                                            </label>
                                                                                        </a>
                                                                                    </h3>
                                                                                    <asp:UpdatePanel ID="upHorasEntrevistas" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <div class="block">
                                                                                                <asp:GridView ID="gvCargosEntrevistas" runat="server"
                                                                                                    AutoGenerateColumns="False" CssClass="displayTable"
                                                                                                    EmptyDataText="No existen datos para mostrar" Width="100%">
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="PGRCODIGO" HeaderText="CODIGO" />
                                                                                                        <asp:BoundField DataField="PGRDESCRIPCION" HeaderText="DESCRIPCION" />
                                                                                                        <asp:TemplateField HeaderText="No. HORAS">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:TextBox ID="txtHoras0" runat="server" BackColor="#16938D"
                                                                                                                    ForeColor="White" Text='<%# Bind("HORAS")%>' Width="80px"></asp:TextBox>
                                                                                                                <asp:FilteredTextBoxExtender ID="txtHoras0_FilteredTextBoxExtender"
                                                                                                                    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtHoras0">
                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style19">OBSERVACIONES:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="EntTxtObservaciones" runat="server" MaxLength="300"
                                                                                            TextMode="MultiLine" Width="500px">NO TENGO OBSERVACIONES</asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&#160;</td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="TabPanel6">
                                                <HeaderTemplate>ON LINE</HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Panel ID="PanelOnline" runat="server">
                                                        <asp:UpdatePanel ID="upOnline" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>Fase:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="onLstFase" runat="server"></asp:DropDownList></td>

                                                                                    <td>AÑO 2023:
                                                                                        <asp:CheckBox ID="OnchkAñoSiguiente" runat="server" Checked="TRUE" /></td>
                                                                                    <td>AÑO 2022:
                                                                                        <asp:CheckBox ID="OnchkAñoActual" runat="server" Checked="FALSE" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Metodologia:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="onLstMetodologia" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>Grupo objetivo:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtGrupoObjetivo" runat="server" MaxLength="250"
                                                                                            TextMode="MultiLine"></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <uc1:UC_Producto ID="UC_Producto3" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="onChkProcesos" runat="server" RepeatDirection="Horizontal" Width="100%"></asp:CheckBoxList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>N.Procesos DataClean:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtNumProcesosDC" runat="server" Width="40px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="onTxtNumProcesos_FilteredTextBoxExtender" runat="server" FilterType="Numbers" InvalidChars="0"
                                                                                            TargetControlID="onTxtNumProcesosDC">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos TopLines:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtNumProcesosTL" runat="server" Width="40px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" FilterType="Numbers" InvalidChars="0"
                                                                                            TargetControlID="onTxtNumProcesosTL">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos Tablas:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtNumProcesosTablas" runat="server" Width="40px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" FilterType="Numbers" InvalidChars="0"
                                                                                            TargetControlID="onTxtNumProcesosTablas">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>N.Procesos Archivos:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtNumProcesosBases" runat="server" Width="40px">1</asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" FilterType="Numbers" InvalidChars="0"
                                                                                            TargetControlID="onTxtNumProcesosBases">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td>Dificultad:</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="onLstTipoMuestra" runat="server" Width="50px"></asp:DropDownList></td>
                                                                                    <td>Cantidad:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtCantidad" runat="server" Width="60px"></asp:TextBox><asp:FilteredTextBoxExtender ID="onTxtCantidad_FilteredTextBoxExtender"
                                                                                            runat="server" Enabled="True" FilterType="Numbers"
                                                                                            TargetControlID="onTxtCantidad">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="BtnOnLineAgregar" runat="server" Text="AGREGAR" /></td>
                                                                                    <td>Productividad:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="onTxtProductividad" runat="server" Width="60px"></asp:TextBox></td>
                                                                                    <td>&#160;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="gvOnLineMuestra" runat="server" CssClass="displayTable"
                                                                                Width="100%">
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td class="style20">OBSERVACIONES:</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="OnTxtObservaciones" runat="server" MaxLength="300"
                                                                                            TextMode="MultiLine" Width="500px">NO TENGO OBSERVACIONES</asp:TextBox></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style20">&#160;</td>
                                                                                    <td>&#160;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                        </asp:TabContainer>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="text-align: center;">
                                <asp:Button ID="btnOk" runat="server" Text="Calcular" />
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>

                    <br />
                    <br />
                </asp:Panel>
            </td>
        </tr>
    </table>



</asp:Content>
