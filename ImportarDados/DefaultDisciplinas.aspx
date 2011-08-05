<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DefaultDisciplinas.aspx.cs" Inherits="ImportarDados_DefaultDisciplinas" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    &nbsp;<asp:DropDownList ID="ddlCalendario" runat="server" CssClass = "ms-toolbar">
    </asp:DropDownList>
    <asp:LinkButton ID="lbtnImportarDisciplinas" runat="server" OnClick="lbtnImportarDisciplinas_Click">LinkButton</asp:LinkButton></asp:Content>
