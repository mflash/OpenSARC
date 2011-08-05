<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DefaultCursos.aspx.cs" Inherits="ImportarDados_DefaultCursos" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <asp:LinkButton ID="lbtnImportarCursos" runat="server" OnClick="lbtnImportarCursos_Click">Importar Cursos</asp:LinkButton>
    <br />
    <asp:Label ID="lblSucesso" runat="server"></asp:Label></asp:Content>
