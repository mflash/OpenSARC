<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="GerenciarRecursos.aspx.cs" Title="Gerenciar Recursos" Inherits="Alocacoes_GerenciarRecursos" %>

<%@ Import Namespace ="BusinessData.Util" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
<div align="left" style="width: 100%; height: 14px" class="ms-menutoolbar">
            <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="GERENCIAR RECURSOS">
            </asp:Label>
            </div>
            
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                        <table id="Table1" runat="server" width="300">
                            <tr>
                                <td>
                                    <asp:Label Width="150px" CssClass="ms-toolbar" runat="server" Text="Professor/Secretário:" ID="Label1" BorderStyle="None"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="150px" CssClass="ms-toolbar" ID="ddlProfAutor" runat="server" OnSelectedIndexChanged="ddlProfAutor_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Width="150px" CssClass="ms-toolbar" runat="server" Text="Eventos:" ID="Label2" BorderStyle="None">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="150px" CssClass="ms-toolbar" ID="ddlEventos" runat="server" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlEventos_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Width="150px" CssClass="ms-toolbar" runat="server" Text="Turmas:" ID="Label3" BorderStyle="None">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="150px" CssClass="ms-toolbar" ID="ddlTurmas" runat="server" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        
                        <asp:DataGrid ID="dgAulas_HorariosEvento" 
                            runat="server"       
                            AutoGenerateColumns="False" 
                            Width="100%" 
                            HorizontalAlign="Center"
                            Visible="false"
                            OnItemDataBound="dgAulas_HorariosEvento_ItemDataBound"
                            OnItemCommand="dgAulas_HorariosEvento_ItemCommand"  >
    
                            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center" BorderColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px"/>
                            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                            <Columns>
            
                            <asp:TemplateColumn HeaderText="AulaId/EventoId" Visible="False"> 
                                <ItemTemplate>
                                    <asp:Label ID="lblAulaIdEventoId" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                                                                         
                            <asp:TemplateColumn HeaderText="Data" >
                                <ItemTemplate>
                                    <asp:Label ID="lblData" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                     
                            <asp:TemplateColumn HeaderText="Dia">
                                <ItemTemplate>
                                    <asp:Label ID="lblDia" runat="server" Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                     
                            <asp:TemplateColumn HeaderText="Horario">
                                <ItemTemplate>
                                    <asp:Label ID="lblHorario" runat="server" ></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Middle" />
                            </asp:TemplateColumn>

                            <asp:TemplateColumn Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecurosAlocadosId" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                                   
                            <asp:TemplateColumn HeaderText="Recursos Alocados" >
                                <ItemTemplate>
                                    <asp:Label ID="lblRecurosAlocados" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn> 
                            
                            <asp:TemplateColumn Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipo" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:ButtonColumn CommandName="AlocarLiberar" Text="Alocar/Liberar" >
                            </asp:ButtonColumn>
                            
                        </Columns>
                            
                        </asp:DataGrid>
                       
                    </div>
            </ContentTemplate>
         </asp:UpdatePanel>
        
</asp:Content>
