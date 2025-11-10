<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_.master" CodeBehind="Evaluacion-Facturas-Operaciones.aspx.vb" Inherits="WebMatrix.Evaluacion_Facturas_Operaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

            loadPlugins();

            $('#UsuariosAsignados').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios Asignados",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");

                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                    }
                }
            });

        });


        function loadPlugins() {

            $('#LoadFiles').dialog(
{
    modal: true,
    autoOpen: false,
    title: "Cargar archivo",
    width: "600px",
    closeOnEscape: true,
    open: function (type, data) {
        $(this).parent().appendTo("form");
    },
    buttons: {
        Cerrar: function () {
            $(this).dialog("close");
        }
    }
});


        }

        function MostrarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("open");
        }

        function MostrarLoadFiles() {
            $('#LoadFiles').dialog("open");
        }

        function CerrarLoadFiles() {
            $('#LoadFiles').dialog("close");
        }



    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <div id="container">
                    <h1><a>Gestión de </a>Facturas</h1>
                    <div id="menu-form">
                        <nav>
           <ul>
            </ul>
           </nav>
                    </div>
                    <asp:HiddenField ID="hfEstado" runat="server" Value="1" />
                    <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                    <asp:HiddenField ID="hfFactura" runat="server" Value="-1" />

                    <asp:Panel ID="pnlEvaluacion" runat="server" Visible="false">
                        <div id="campo-formulario3" style="min-width: 600px; margin-bottom: 10px">

                            <a style="color: white;">Lo invitamos a participar en la encuesta de evaluación de proveedores de Ipsos, la encuesta tomara menos de 2 minutos de su tiempo, la información que proporcione será usada solo como parte de nuestro programa de mejora continua, participando en esta encuesta usted acepta que la información que proporcione pueda ser compartida con el personal de Ipsos involucrado en el Sistema de Gestión de Calidad, quien la mantendrá bajo estricta confidencialidad</a>
                            <br></br>
                            <a style="color: white;">Usando una escala una escala de 1 a 10 donde 1 es completamente insatisfecho y 10 completamente satisfecho, como evaluaría el servicio prestado por </a>
                            <asp:Label ID="lblNombreProveedor" runat="server" Style="color: white"></asp:Label><a style="color: white;"> en el proyecto </a>
                            <asp:Label ID="lblIdTrabajo" runat="server" Style="color: white"></asp:Label><a style="color: white;"> - </a>
                            <asp:Label ID="lblNombreTrabajo" runat="server" Style="color: white"></asp:Label>
                            <a style="color: white;">en cuanto a:</a>
                            <br></br>
                            <br></br>
                            <br></br>
                            <div style="width: 100%">
                                <div style="width: 100%; float: none">
                                    <label>
                                        1. Cumplimiento de tiempos:
                                    </label>
                                </div>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="RBP1"></asp:RequiredFieldValidator></label>
                            <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP1" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">

                                <asp:Panel ID="pnlTxtP1" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de tiempos?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP1"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP1" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>
                            <div style="width: 100%">
                                <label>
                                    2. Calidad del servicio:

                                </label>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">

                                        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP2" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">

                                <asp:Panel ID="pnlTxtP2" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Calidad del servicio?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP2"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP2" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>

                            <div style="width: 100%">

                                <label>3. Cumplimiento de las especificaciones e instrucciones del proyecto:</label>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP3" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>
                                </div>

                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">
                                <asp:Panel ID="pnlTxtP3" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de las especificaciones e instrucciones del proyecto?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP3"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP3" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>

                            <div style="width: 100%">

                                <label>4. Proactividad y resolución de problemas:</label>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP4" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">
                                <asp:Panel ID="pnlTxtP4" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Proactividad y resolución de problemas?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP4"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP4" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>

                            <div style="width: 100%">

                                <label>5. Cumplimiento de estándares ISO 9001 – 20252 requeridos:</label>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP5" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>

                                </div>

                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">

                                <asp:Panel ID="pnlTxtP5" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de estándares ISO 9001 – 20252 requeridos?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP5"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP5" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>

                            <div style="width: 100%">

                                <label>6. Cumplimiento de privacidad de datos y confidencialidad:</label>
                                <div style="margin: auto; width: 80%">
                                    <div style="float: left; padding-top: 40px">
                                        <label>
                                            Completamente insatisfecho
                                        </label>
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="11" runat="server" ID="RBP6" AutoPostBack="true">
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
                                            Completamente satisfecho
                                        </label>
                                    </div>
                                </div>

                            </div>
                            <div style="clear: both"></div>
                            <div style="margin: auto; width: 80%">
                                <asp:Panel ID="pnlTxtP6" runat="server" Visible="false">
                                    <label>
                                        ¿Podría decirme por qué ha dado usted esa calificación a Cumplimiento de privacidad de datos y confidencialidad?
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP6"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP6" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <br></br>
                            <br></br>
                            <br></br>

                            <div>

                                <asp:Panel ID="pnlP7" runat="server">
                                    <label>
                                        7.	Si tiene otros comentarios o recomendaciones que puedan aportar a que el proveedor mejore su servicio, por favor regístrelos a continuación:
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtP7"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtP7" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <asp:Button ID="btnEnviar" Text="Enviar Evaluación" runat="server" CssClass="button" Width="150px" />
                    </asp:Panel>

                    <asp:Panel ID="pnlAvisoFactura" runat="server" Visible="false" Style="width: 60%; height: 250px; margin-left: auto; margin-right: auto; padding-top: 10%">
                        <div id="campo-formulario2" style="min-width: 600px;">
                            <label>Esta intentando acceder a una factura que no es valida y/o a una factura para la cual ya se diligencio la evaluación</label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlGracias" runat="server" Visible="false" Style="width: 60%; height: 250px; margin-left: auto; margin-right: auto; padding-top: 10%">
                        <div id="campo-formulario1" style="min-width: 600px;">

                            <label>Gracias por participar</label>
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="LoadFiles">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Button ID="btnViewFile" runat="server" Text="Ver archivo" />
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
