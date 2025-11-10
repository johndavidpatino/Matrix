<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="PlaneacionEstudiosPorSalir.aspx.vb" Inherits="WebMatrix.PlaneacionEstudiosPorSalir" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
            
        });

        function loadPlugins() {

            $('#accordion').accordion({
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
    <a>Planeacion Trabajos Estudios y Propuestas</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <a>Formulario que muestra la cantidad de trabajos y encuestas a realizar</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Datos generales<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Gerencias OP</label>
                                    <asp:DropDownList ID="ddlGerencias" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Unidades</label>
                                    <asp:DropDownList ID="ddlUnidades" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Metodología</label>
                                    <asp:DropDownList ID="ddlMetodologia" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudad" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="form_left">
                            <fieldset>
                                <label> </label>
                            </fieldset>
                            <asp:Button ID="btnBuscar" runat="server" Text="Mostrar información" />
                        </div>
                            <div class="actions"></div>
                                <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar" DataKeyNames="Año,Semana" >
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="AÑO" HeaderText="Año" />
                                    <asp:BoundField DataField="Mes" HeaderText="Mes" />
                                    <asp:BoundField DataField="Semana" HeaderText="Semana" />
                                    <asp:BoundField DataField="CantidadEstudios" HeaderText="Estudios" />
                                    <asp:BoundField DataField="DiasCampoPromedio" HeaderText="Dias de Campo"/>
                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                    <asp:BoundField DataField="Encuestadores" HeaderText="Encuestadores" />
                                    <asp:TemplateField HeaderText="Detalles">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrProject" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Ver detalles"  />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                            <div style="text-align:center">
                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                    </div>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles<asp:HiddenField ID="hfdSemana" runat="server" /><asp:HiddenField ID="hfdAno" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                            <div class="actions"></div>
                                <asp:GridView ID="gvDetalle" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar" DataKeyNames="Id" >
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Propuesta_Estudio" HeaderText="Propuesta / Estudio" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                    <asp:BoundField DataField="CiudadReco" HeaderText="CiudadReco" />
                                    <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                    <asp:BoundField DataField="DiasCampo" HeaderText="Dias de Campo"/>
                                    <asp:BoundField DataField="Productividad" HeaderText="Productividad" />

                                </Columns>
                            </asp:GridView>
                            </div>
                            <div style="text-align:center">
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
