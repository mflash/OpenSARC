<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DefaultProfessores.aspx.cs" Inherits="ImportarDados_Default" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <asp:LinkButton ID="lbtnImportarProfessores" runat="server" OnClick="lbtnImportarProfessores_Click">Importar Professores</asp:LinkButton>
    <br />
    <asp:Label ID="lblSucesso" runat="server" CssClass="ms-toolbar"></asp:Label></asp:Content>
