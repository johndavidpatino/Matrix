<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/VisualizadorSite.master"
    CodeBehind="VisualizadorFiltros.aspx.vb" Inherits="WebMatrix.VisualizadorFiltros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../Styles/Filtros.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
            });


        }
        function redireccion() {
            var identrevista = $('#<%= hfIdFiltro.ClientID %>').val();
            document.location.href = 'Trabajos.aspx?TipoFiltro=' + identrevista;

        }

        function verifyCheckboxList(source, arguments) {
            var ctr_Txt = source.id.replace('cv', 'Ctr');
            var val = document.getElementById(ctr_Txt);
            var col = val.getElementsByTagName("*");
            if (col != null) {
                for (i = 0; i < col.length; i++) {
                    if (col.item(i).tagName == "INPUT") {
                        if (col.item(i).checked) {
                            arguments.IsValid = true;
                            return;
                        }
                    }
                }
            }
            arguments.IsValid = false;
        }

    </script>
    <style type="text/css">
        #accordion1 {
            margin-bottom: 0px;
        }
    </style>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>--%>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Diligenciar Filtros</a>
</asp:Content>--%>
<%--<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>--%>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div style="width: 100%">
                <div id="container">


                    <div style="min-height: 400px">
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfIdFiltro" runat="server" Value="0" />
                        <asp:HiddenField ID="hfIdRespuesta" runat="server" Value="0" />

                        <asp:Panel ID="pnlVisualizar" runat="server">
                            <div class="actions">
                                <label id="lblFiltro" runat="server" style="font-size: 22px"></label>
                                <asp:Panel ID="pnlBusqueda" runat="server" CssClass="actions" Visible="false">
                                    <div>
                                        <label>Cédula:</label>
                                        <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
                                        &nbsp;
                                        <asp:Button ID="btnBuscar" Text="Buscar" runat="server" />
                                        <br />
                                        <label style="color: red" id="lblErrorMessage" runat="server" visible="false">El Usuario no se encuentra registrado en el Filtro de Reclutamiento</label>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlPreguntas" runat="server" CssClass="actions" Visible="false">
                                </asp:Panel>
                                <asp:Panel ID="pnlGuardar" runat="server" CssClass="actions">
                                    <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CausesValidation="true" />
                                </asp:Panel>
                                <asp:Panel ID="pnlGracias" runat="server" CssClass="actions" Visible="false">
                                    <label style="font-size: 24px">Muchas Gracias por sus respuestas!</label>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
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
