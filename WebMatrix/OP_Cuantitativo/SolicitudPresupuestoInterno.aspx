<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="SolicitudPresupuestoInterno.aspx.vb" Inherits="WebMatrix.SolicitudPresupuestoInterno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="../op_cuantitativo/Trabajos.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Regresar al trabajo
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Solicitud de presupuesto interno
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubTitle" runat="server">
    Por favor describa los elementos que requiere para la solicitud de presupuesto interno
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Content" runat="server">
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
    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                     <asp:HiddenField ID="hfidMetodologia" runat="server" />
    <asp:HiddenField ID="hfNew" runat="server" />
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Configuración</h5>
            <p class="card-subtitle">Diligencie las especificaciones para el presupuesto</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary" >Muestra</button>
                        </div>
                        <asp:TextBox ID="txtMuestra" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Vr Sugerido Ext</button>
                        </div>
                        <asp:TextBox ID="txtVrSugerido" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group">
                            <div class="input-group-prepend"><span class="input-group-text">
                                <asp:CheckBox ID="chbCampo" runat="server" />
                                </span></div>
                            <label placeholder="Campo" type="text" class="form-control">Campo</label>
                        </div>
                    </div>

                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group">
                            <div class="input-group-prepend"><span class="input-group-text">
                                <asp:CheckBox ID="chbAgendamiento" runat="server" />
                                </span></div>
                            <label placeholder="Agendamiento" type="text" class="form-control">Agendamiento</label>
                        </div>
                    </div>

                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group">
                            <div class="input-group-prepend"><span class="input-group-text">
                                <asp:CheckBox ID="chbJornadas" runat="server" />
                                </span></div>
                            <label placeholder="Jornadas" type="text" class="form-control">Jornadas</label>
                        </div>
                    </div>

                    <div class="input-group col-md-2 mb-2">
                        <div class="input-group">
                            <div class="input-group-prepend"><span class="input-group-text">
                                <asp:CheckBox ID="chbReclutamiento" runat="server" AutoPostBack="true" OnCheckedChanged="chbReclutamiento_CheckedChanged" />
                                </span></div>
                            <label placeholder="Reclutamiento" type="text" class="form-control">Reclutamiento</label>
                        </div>
                    </div>
                </div>
                <div class="form-row" runat="server" id="rowReclutamiento" visible="false">
                    <div class="input-group col-md-3 mb-2">
                            <div class="input-group-prepend"><button class="btn btn-secondary">Distribución General</button>
                                </div>
                            <asp:TextBox ID="txtGeneral" TextMode="Number" runat="server" />
                    </div>
                    <div class="input-group col-md-3 mb-2">
                            <div class="input-group-prepend"><button class="btn btn-secondary">Distribución NSE 1 y 2</button>
                                </div>
                            <asp:TextBox ID="txtNSE1y2" TextMode="Number" runat="server" />
                    </div>
                    <div class="input-group col-md-3 mb-2">
                            <div class="input-group-prepend"><button class="btn btn-secondary">Distribución NSE 3 y 4</button>
                                </div>
                            <asp:TextBox ID="txtNSE3y4" TextMode="Number" runat="server" />
                    </div>
                    <div class="input-group col-md-3 mb-2">
                            <div class="input-group-prepend"><button class="btn btn-secondary">Distribución NSE 5 y 6</button>
                                </div>
                            <asp:TextBox ID="txtNSE5y6" TextMode="Number" runat="server" />
                    </div>
                </div>
                <div class="input-group col-md-12 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Observaciones</button>
                    </div>
                    <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button runat="server" ID="btnSolicitar" class="btn btn-primary" Text="Solicitar" OnClick="btnSolicitar_Click"></asp:Button>
                
            </div>

        </div>
    </div>
    </asp:Content>