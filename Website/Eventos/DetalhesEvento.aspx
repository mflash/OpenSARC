
<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
         CodeFile="DetalhesEvento.aspx.cs" Inherits="Eventos_DetalhesEvento"
         Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>
         
<%@ Import Namespace="BusinessData.Entities" %>

    <asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
        <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="DETALHES DO EVENTO"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
        </asp:UpdatePanel>
        
    </asp:Content>
