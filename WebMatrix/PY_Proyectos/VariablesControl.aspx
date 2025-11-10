<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="VariablesControl.aspx.vb" Inherits="WebMatrix.VariablesControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <style>
        .no-height {
            max-height: 26px;
            min-height: 26px;
            max-width: 50%;
            min-width: 50%;
        }

        .divNuevaLinea {
            width: 100%;
            float: left;
        }

        .div4Form {
            width: 24%;
            float: left;
        }

        .div3Form {
            width: 33%;
            float: left;
        }

        .div2Form {
            width: 48%;
            float: left;
        }

        #stylized label {
            text-align: left;
            margin: 0px;
            margin-left: 5px;
            width: 100px;
        }

        #stylized input[type="radio"] {
            width: auto;
            float: left;
            padding: 0px;
            margin: 5px 0 0 10px;
        }

        #stylized input, select {
            margin: 0;
        }

        .text70 {
            width: 70% !important;
        }

        .textFull {
            width: 100% !important;
        }

        .wAuto {
            width: auto;
        }

        .textTabla {
            margin: 0px 5px !important;
        }

        .mt-10 {
            margin-top: 30px;
        }

        .der {
            float: right;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../Home/Default.aspx">IR AL INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Variables de Control
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Variables de Control
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdProyecto" />
            <asp:HiddenField runat="server" ID="hfIdTrabajo" />
            <asp:HiddenField runat="server" ID="hfmodalidad" />
            <asp:HiddenField runat="server" ID="hfIdCOE" />
            <asp:HiddenField runat="server" ID="hfIdGerente" />
            <div class="col-md-12 cajaFiltro">
                <asp:Panel runat="server" ID="pnlTrabajoId" CssClass="div4Form">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Id Trabajo</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtTrabajoId" ReadOnly="true" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlJobBook" CssClass="div4Form">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>JobBook</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtJobBook" ReadOnly="true" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTrabajoNombre" CssClass="div4Form">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Nombre Trabajo/Estudio</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtTrabajoNombre" ReadOnly="true" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlCoe" Visible="false" CssClass="div4Form">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>OMP</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtCoe" ReadOnly="true" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlGerente" Visible="false" CssClass="div4Form">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Gerente de Proyecto</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtGerente" ReadOnly="true" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="col-md-12 cajaFiltro mt-10">
                <asp:Panel runat="server" ID="pnlSeguridad" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Seguridad y confidencialidad de la información</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblSeguridad" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtSeguridad" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlObtencion" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Forma de Obtención de los entrevistados</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblObtencion" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtObtencion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlObjetivo" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Grupo objetivo</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblObjetivo" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtObjetivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlAplicacion" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Aplicación de instrumentos</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblAplicacion" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtAplicacion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDistribucion" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Distribución de Cuotas</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblDistribucion" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtDistribucion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlCumplimiento" Visible="true">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <h4>Cumplimiento de Metodología y otras instrucciones</h4>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:RadioButtonList ID="rblCumplimiento" runat="server" Width="30%" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Si</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Observaciones</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtCumplimiento" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="col-md-12 cajaFiltro mt-10">
                <asp:Button Text="Guardar" runat="server" ID="btnGuardar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
