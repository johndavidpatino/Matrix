<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterCuentas.master"
    CodeBehind="Briefs.aspx.vb" Inherits="WebMatrix.Briefs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

                        

    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function MostrarRazonNoViabilidad() {
            $('#RazonNoViabilidad').dialog("open");
        }


        function loadPlugins() {

            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {

            });

            
            $('#tabs').tabs();

            $('#RazonNoViabilidad').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Escriba la razón de no viabilidad",
                    width: "600px",
                    closeOnEscape: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");

                    }
                });


        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Brief / Frame
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Este espacio ha sido diseñado para almacenar la información del brief / frame obtenido de las reuniones con el cliente.
    La información que aquí se plasme será tenida en cuenta en otros sitios de Matrix para ayudar a entender a los equipos
    las necesidades de los clientes: Operaciones lo tendrá en cuenta para los presupuestos, los Gerentes de Proyectos
    podrán recurrir a él para obtener información específica de la necesidad.
    <br />
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
            <asp:Panel ID="accordion0" runat="server">
                <h3>
                    <a>
                        <label>
                            Consulta de Briefs</label>
                    </a>
                </h3>
                <label>
                    Palabra a Buscar</label>
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                <div class="spacer"></div>
                <label>
                    Filtros</label>
                <asp:Button ID="btnfiltrar" runat="server" Text="Sin Propuesta" />
                <asp:Button ID="btnFiltroSinViabilidad" runat="server" Text="Por definir viabilidad" />
                <asp:Button ID="btnQuitarFiltro" runat="server" Text="Quitar Filtro" />
                <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="No. Brief" />
                        <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                        <asp:BoundField DataField="RazonSocial" HeaderText="Cliente" />
                        <asp:BoundField DataField="Nombre" HeaderText="Contacto" />
                        <asp:BoundField DataField="NombreBrief" HeaderText="Tipo Brief" />
                        <asp:TemplateField HeaderText="Viabilidad" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Eval("Id") %>' Visible="False"></asp:Label>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Viabilidad") %>' Enabled="false"
                                    Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")), False, True) %>' />
                                <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="SI" ImageUrl="~/Images/si.jpg" Text="SI" OnClientClick="return confirm('Está seguro de Elegir la Viabilidad?')"
                                    ToolTip="SI" Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")), True, False) %>' />
                                &nbsp;
                                        <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="NO" ImageUrl="~/Images/no.jpg" Text="NO" OnClientClick="MostrarRazonNoViabilidad()"
                                            Visible='<%# IIF(String.IsNullOrEmpty(Eval("Viabilidad")), True, False) %>' ToolTip="NO" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Propuesta" HeaderText="Propuesta" />
                        <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Modificar" ImageUrl="~/Images/list_16_.png" Text="Seleccionar"
                                    ToolTip="Abrir el Brief" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalles" ShowHeader="False" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgdetalles" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Detalles" ImageUrl="~/Images/application_view_detail.png" Text="Seleccionar"
                                    ToolTip="Detalles" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                            Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                    <%= gvDatos.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel runat="server" ID="accordion1" Visible="false">
                <h3>
                    <a>
                        <label>
                            Información del Brief</label>
                    </a>
                </h3>
                <div class="spacer" />
                <label>
                    Nombre Cliente</label>
                <asp:TextBox ID="txtNombreCliente" runat="server"></asp:TextBox>
                <label>
                    Nombre Contacto</label>
                <asp:HiddenField ID="hfidbrief" runat="server" />
                <asp:HiddenField ID="hfidbriefnoviab" runat="server" />
                <asp:TextBox ID="txtNombreContacto" runat="server"></asp:TextBox>
                <label>
                    Tipo de Brief</label>
                <asp:DropDownList ID="ddlTipoBrief" runat="server" CssClass="dropdowntext">
                </asp:DropDownList>
                <div style="clear: both;">
                    <label id="lblTextoViabilidad" visible="false" runat="server">Fecha de Marcado de Viabilidad:</label>
                    <label id="lblFechaViabilidad" visible="false" runat="server"></label>
                </div>
                <div style="clear: both;">
                    <label>
                        0. Titulo:</label>
                    <asp:TextBox ID="txtTitulo" Width="80%"
                        runat="server" />
                    <div class="spacer"></div>
                    <asp:Panel ID="pnlFrame" runat="server">
                        <div id="tabs">
                            <ul>
                                <li><a href="#tabs-1">Objetivos de Negocio</a></li>
                                <li><a href="#tabs-2">Decisiones</a></li>
                                <li><a href="#tabs-3">Competencia</a></li>
                                <li><a href="#tabs-4">Metodología</a></li>
                                <li><a href="#tabs-5">Datos de Diseño</a></li>
                                <li><a href="#tabs-6">Tiempos y Presupuesto</a></li>
                                <li><a href="#tabs-7">Entregables</a></li>
                            </ul>
                            <div id="tabs-1">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 100%; float: left; clear:both; padding: 2px 2px 2px 2px">
                                        <p>¿Qué necesidad de negocios está tratando de resolver?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuáles son sus objetivos estratégicos para este estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Hay algún factor importante a considerar para el diseño del estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿El problema planteado está relacionado a alguna área en específico de la empresa?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO4" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Por qué está haciendo este estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO5" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Qué hipótesis tiene sobre el tema (s) está estudiando?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO6" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuáles son los indicadores clave de rendimiento requerido?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtO7" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="tabs-2">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Qué decisiones va a tomar como resultado de este estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtD1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Quién va a utilizar esta investigación y qué van a hacer con él?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtD2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Hay alguna política de su empresa /reglas que se tienen que considerar ir, sin cambio en el precio, empaquetado, etc. protocolo global?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtD3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="tabs-3">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuáles son los desafíos que enfrenta su  negocio en el mercado? ¿Cómo es el desempeño de la industria?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtC1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Quiénes son sus competidores? ¿Necesita alguna evaluación comparativa del estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtC2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuál es el posicionamiento de su marca y cuáles son las diferencias vs. su competencia?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtC3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Qué actividades recientes hay en el mercado que podrían haber influido en el mercado?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtC4" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Dónde ve su oportunidad y las amenazas en su caso?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtC5" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <div id="tabs-4">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Necesita comparar esta investigación contra alguna investigación previa?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtM1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuál fue el diseño de esa investigación previa?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtM2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuándo se realizó?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtM3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="tabs-5">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿En dónde tiene pensado hacer el levantamiento?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI1" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuál es la definición de su target?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI2" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cómo segmenta su mercado? Por productos, por usuario, por zonas geográficas?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI3" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Tiene algún requisito de tamaño de la muestra?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI4" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Espera que la muestra debe sea representativa del mercado?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI5" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Tiene alguna metodología específica en mente, por ejemplo, cualitativa o cuantitativa?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI6" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Hay alguna información clave que necesite este en el reporte final?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI7" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="tabs-6">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Para cuándo necesita los resultados. ¿Hay alguna limitante de tiempo?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI8" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Para cuándo necesita la propuesta?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI9" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>¿Cuándo cree que se tomará para tomar una decisión para comisionar el proyecto?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI10" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Con la idea de dimensionar el alcance del estudio para usted, podría compartir con nosotros cuál es el presupuesto que ha destinado a este estudio?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI11" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="tabs-7">
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Por último, sólo quiero ver si usted tiene algún requerimiento especial en cuanto a formatos de entrega?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI12" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Topline de  resultados?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI13" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Presentación en ppt?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI14" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Transcripciones?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI15" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Presentaciones verbales?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI16" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Workshop?</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI17" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="width: 100%; clear: both">
                                    <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                        <p>Otros, especifique:</p>
                                    </div>
                                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtDI18" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <label>
                            Unidad
                        </label>
                        <asp:DropDownList ID="ddlUnidades" runat="server">
                        </asp:DropDownList>
                    </asp:Panel>
                    <div style="clear: both;"></div>
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                        <legend>Brief</legend>

                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>1. Antecedentes y Problema de Marketing:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtAntecedentes" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>2. Objetivos de la Investigación:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtObjetivos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>3. Action Standards:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtActionStandard" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>4. Metodología:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtMetodologia" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>5. Target group de la Investigación de Mercado:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtTargetGroup" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>6. Tiempos:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtTiempos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>7. Presupuestos:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtPresupuesto" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>8. Materiales disponibles:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtMateriales" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>9. Resultados de estudios anteriores:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtEstudiosAnteriores" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>10. Formato requerido por el cliente para el informe:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtFormatos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>11. Aprobaciones:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtAprobaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        <div style="width: 100%; clear: both">
                            <div style="width: 29%; float: left; padding: 2px 2px 2px 2px">
                                <label>12. Competencia:</label>
                            </div>
                            <div style="width: 70%; float: left; padding: 2px 2px 2px 2px">
                                <asp:TextBox ID="txtCompetencia" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </div>
                        </div>
                        
                        
                    </asp:Panel>
                    <div style="clear: both;"></div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar" />
                    &nbsp;
                                            <asp:Button ID="btnGuardarYCrear" runat="server" Text="Guardar y Crear Propuesta" CommandName="Guardar y Crear Propuesta" />
                    &nbsp;
                                            <input id="Button1" type="submit" value="Cancelar" style="font-size: 11px;"
                                                onclick="location.href = 'Briefs.aspx';" />
                    <div style="clear: both;"></div>
                </div>
                <asp:Panel runat="server" ID="accordion2" Visible="false">
                    <h3>
                        <a>
                            <label>
                                Detalles del registro</label>
                        </a>
                    </h3>
                    <asp:Button ID="btnDocumentos" runat="server" Text="Cargar Documentos" />
                    <asp:Label ID="lbldetalleregistro" runat="server" Text=""></asp:Label>
                    <div style="clear: both;"></div>
                    <label>Propuestas creadas</label>
                    <asp:GridView ID="gvPropuestas" runat="server" Width="100%" AutoGenerateColumns="False"
                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="RazonSocial" HeaderText="Cliente" />
                            <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                            <asp:BoundField DataField="TipoPropuesta" HeaderText="Tipo Propuesta" />
                            <asp:BoundField DataField="Probabilidad" HeaderText="Probabilidad Aprob" />
                            <asp:BoundField DataField="FechaEnvio" HeaderText="FechaEnvio" DataFormatString="{0:dd/MM/yyyy}"
                                HtmlEncode="False" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                        ToolTip="Modificar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                Enabled='<%# IIf(gvPropuestas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIf(gvPropuestas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvPropuestas.PageIndex + 1%>-
                                                            <%= gvPropuestas.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIf((gvPropuestas.PageIndex + 1) = gvPropuestas.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIf((gvPropuestas.PageIndex + 1) = gvPropuestas.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                    <div style="clear: both;"></div>
                    <asp:Panel ID="pnlbotones" runat="server">
                        <asp:Button ID="bntcrearpropuesta" runat="server" Text="Crear Propuesta" />
                        &nbsp;
                                                <input id="Button3" type="submit" value="Cancelar" style="font-size: 11px;"
                                                    onclick="location.href = 'Briefs.aspx';" />
                    </asp:Panel>
                    </div>
                            </fieldset>
                        </div>
                </asp:Panel>
            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="RazonNoViabilidad">
        <asp:UpdatePanel ID="upGerenteAsignar" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <label>Escriba la razón de no viabilidad</label>
                    <asp:TextBox ID="txtRazonNoViabilidad" Width="300px" MaxLength="250" TextMode="MultiLine" Height="100px" runat="server" CssClass="required text textEntry"></asp:TextBox>
                </div>
                <div>
                    <div>
                        <asp:Button ID="btnGuardarRazonViabilidad" runat="server" Text="Guardar" OnClientClick="$('#RazonNoViabilidad').dialog('close');" />
                        <asp:Button ID="btnCancelarRazonViabilidad" runat="server" Text="Cancelar" OnClientClick="$('#RazonNoViabilidad').dialog('close');" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
