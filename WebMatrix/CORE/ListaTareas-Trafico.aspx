<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master"
    CodeBehind="ListaTareas-Trafico.aspx.vb" Inherits="WebMatrix.ListaTareas_Trafico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
    <style type="text/css">
        .contenedorBuscar {
            width: 24%;
            float: left;
        }

        .pnlwidth {
            width: 70% !important;
        }

        #pnlBusqueda {
            margin: 20px 0px;
            width: 100%;
        }

            #pnlBusqueda label {
                display: block;
                font-weight: lighter;
                text-align: right;
                padding: 5px;
                width: 50px;
                height: 26px;
                float: left;
                font-family: 'Roboto', sans-serif;
                font-size: 13px;
            }

            #pnlBusqueda input {
                float: left;
                font-size: 12px;
                margin: 0px 5px;
                padding: 0;
                color: #666666;
                height: 26px;
                background: #fff;
                border: 1px solid;
                border-color: #c4c4c4 #d1d1d1 #d4d4d4;
                border-radius: 0px;
                outline: 3px solid #f4f4f4;
                width: 150px;
            }


            #pnlBusqueda select {
                float: left;
                font-size: 12px;
                margin: 0px 5px;
                padding: 0;
                border: solid 1px #D7D7D7;
                color: #666666;
                width: 150px;
            }
    </style>
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
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <div>
        <div id="pnlBusqueda">
            <div class="contenedorBuscar">
                <label>
                    Nombre JobBook:
                </label>
                <asp:TextBox ID="txtNombreTrabajo" runat="server" CssClass="pnlwidth"></asp:TextBox>
            </div>
            <div class="contenedorBuscar">
                <label>
                    Proceso:
                </label>
                <asp:DropDownList ID="ddlProceso" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="contenedorBuscar">
                <label>
                    Gerente Proyecto:
                </label>
                <asp:DropDownList ID="ddlGerente" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="contenedorBuscar">
                <label>
                    OMP:
                </label>
                <asp:DropDownList ID="ddlCoe" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="spacer" style="width: 100%; padding: 5px;"></div>
            <div class="contenedorBuscar">
                <label>
                    Año:
                </label>
                <asp:DropDownList ID="ddlAno" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                    <asp:ListItem Text="2019" Value="2019" Selected="True" />
                    <asp:ListItem Text="2018" Value="2018" />
                    <asp:ListItem Text="2017" Value="2017" />
                </asp:DropDownList>
            </div>
            <div class="contenedorBuscar">
                <label>
                    Mes:
                </label>
                <asp:DropDownList ID="ddlMes" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                    <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Enero" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Febrero" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Marzo" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Mayo" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Junio" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Julio" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Septiembre" Value="9"></asp:ListItem>
                    <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
                    <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="contenedorBuscar">
                <label>
                    Unidad:
                </label>
                <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="contenedorBuscar">
                <label>
                    Estado:
                </label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="pnlwidth mySpecificClass dropdowntext" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div class="spacer" style="width: 100%; padding: 5px;"></div>
        <div style="float: right;">
            <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" />
            <asp:Button ID="btnExportar" runat="server" Text="Descargar Excel" />
            <asp:Button ID="btnCancelar" runat="server" Text="Quitar Filtro" Visible="false" />
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvLista" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="30"
                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                    <asp:BoundField DataField="GerenteProyectos" HeaderText="GerenteProyectos" />
                    <asp:BoundField DataField="COE" HeaderText="OMP" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                    <asp:BoundField DataField="FIniP" HeaderText="FIniP" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinP" HeaderText="FFinP" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FIniR" HeaderText="FIniR" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinR" HeaderText="FFinR" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="txtEstadoActual" HeaderText="EstadoActual" />
                </Columns>
                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" /> 
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlProceso" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlGerente" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlCoe" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlMes" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlUnidad" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtNombreTrabajo" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
