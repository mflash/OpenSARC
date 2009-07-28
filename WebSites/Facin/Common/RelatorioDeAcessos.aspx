<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="RelatorioDeAcessos.aspx.cs" Inherits="Default_RelatorioDeAcessos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
     <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="Relatório de Acessos"></asp:Label>
    </div> 
    <p />
    <center>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="RelatorioDataSource">
            <RowStyle CssClass="ms-toolbar" />
            <HeaderStyle CssClass="ms-wikieditthird" />
        </asp:GridView>
    </center>  
    <asp:ObjectDataSource ID="RelatorioDataSource" runat="server" 
        SelectMethod="CriarRelatorioDeAcessos" 
        TypeName="BusinessData.BusinessLogic.AcessosBO"></asp:ObjectDataSource>
</asp:Content>

