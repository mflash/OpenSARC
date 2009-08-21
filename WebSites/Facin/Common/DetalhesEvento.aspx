<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DetalhesEvento.aspx.cs" Inherits="Secretarios_DetalhesEvento" %>
<%@ Import Namespace="BusinessData.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="Detalhes do Evento"></asp:Label>
    </div>
    
    <div style="font-size: larger; font-style: italic">
        Evento: <asp:Label ID="lblTituloEvento" runat="server" ></asp:Label>
   </div>
    <br />
    <asp:DataGrid ID="dgHorariosEvento" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="100%" 
                     HorizontalAlign="Center"
                     OnItemCommand="dgHorariosEvento_ItemCommand"
                     OnItemDataBound="dgHorariosEvento_ItemDataBound"  >
        <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
        <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateColumn HeaderText="Data" >
                <ItemTemplate>
                    <asp:Label ID="lblData" runat="server" 
                            Text='<%#((DateTime)Eval("Data")).ToShortDateString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Horario" >
                <ItemTemplate>
                    <asp:Label ID="lblHorario" runat="server" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblEventoId" runat="server" 
                            Text='<%# ((Evento)Eval("EventoId")).EventoId %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Recursos_Alocados_id"  Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblRecursosAlocadosId" runat="server" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Recursos Alocados" >
                <ItemTemplate>
                    <asp:Label ID="lblRecursosAlocados" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:ButtonColumn CommandName="Selecionar" Text="Selecionar" 
                    HeaderText="Recursos Disponíveis"></asp:ButtonColumn>
            <asp:ButtonColumn CommandName="Trocar" Text="Trocar Recurso"></asp:ButtonColumn>
            <asp:ButtonColumn CommandName="Transferir" Text="Transferir Recurso">
            </asp:ButtonColumn>
        </Columns>
    </asp:DataGrid>
    <br />
  
</asp:Content>

