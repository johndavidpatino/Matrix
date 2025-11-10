<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master" CodeBehind="SyncIssues.aspx.vb" Inherits="WebMatrix.SyncIssues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
          function (value, element) {
              return this.optional(element) ||
                (value != -1);
          }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
          function (value, element) {
              return this.optional(element) ||
                ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
          }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });


            loadPlugins();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Synchronization Issues
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Ajustar trabajos
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <h2>Trabajos</h2>
                        <label>Trabajo Id</label>
                        <b>
                            <asp:TextBox ID="txtNumeroTrabajo" runat="server"></asp:TextBox></b>
                        <br />
                        <asp:Button ID="btnQuitarEntrenamiento" runat="server" Text="Quitar Preguntas Entrenamiento" CssClass="button" Width="200px" OnClientClick="return confirm('Está seguro de quitar las encuestas de entrenamiento?')" />
                        <asp:Button ID="btnSupervision" runat="server" Text="Quitar Supervision Estudio Especializado" CssClass="button" Width="200px" OnClientClick="return confirm('Está seguro de quitar la supervisión a este trabajo?')" />
                        <asp:Button ID="btnSincronizacion" runat="server" Text="Habilitar sincronización" CssClass="button" Width="200px" OnClientClick="return confirm('Está seguro de habilitar la sincronización para este trabajo?')" />
                    </div>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Actualizar preguntas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <h2>Seleccionar Trabajo</h2>
                        <label>Trabajo Id</label>
                        <b>
                            <asp:TextBox ID="txtTrabajoId" runat="server"></asp:TextBox></b>
                        <asp:Button ID="btnSearch" runat="server" Text="Mostrar preguntas" />
                        <br />
                        <label>Preguntas</label>
                        <asp:DropDownList ID="ddlPreguntas" runat="server"></asp:DropDownList>
                        <label>SbjNum</label><b><asp:TextBox ID="txtSbjNum" runat="server" placeholder="Digite el SbjNum" Width="150px"></asp:TextBox></b>
                        <label>Nuevo valor</label><b><asp:TextBox ID="txtNewValor" runat="server" placeholder="Digite el nuevo valor" Width="150px"></asp:TextBox></b>
                        <asp:Button ID="btnActualizarValor" runat="server" Text="Actualizar respuesta" CssClass="button" Width="200px" OnClientClick="return confirm('Está seguro de realizar la actualización?')" />
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Habilitar encuesta piloto
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <label>SbjNum</label><b><asp:TextBox ID="txtSbjNumPiloto" runat="server" placeholder="Digite el SbjNum" Width="150px"></asp:TextBox>
                        <asp:Button ID="btnHabilitarPiloto" runat="server" CssClass="button" OnClientClick="return confirm('Está seguro de habilitar la encuesta nuevamente?')" Text="Habilitar" Width="200px" />
                        </b>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Encuesta piloto
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <label>SbjNum</label><b><asp:TextBox ID="txtSbjNumPiloto2" runat="server" placeholder="Digite el SbjNum" Width="150px"></asp:TextBox>
                        <asp:Button ID="btnEncuestaPiloto" runat="server" CssClass="button" Text="Encuesta Piloto" Width="200px" />
                        </b>
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
