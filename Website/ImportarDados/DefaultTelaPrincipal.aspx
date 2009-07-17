<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DefaultTelaPrincipal.aspx.cs" Inherits="ImportarDados_DefaultTelaPrincipal" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <table align="left">
    <a href="DefaultCursos.aspx" class="UserToolbarTextArea">Importar Cursos</a><br />
    <a href="Default.aspx" class="UserToolbarTextArea">Importar Faculdades</a>
    <br />
    <a href="DefaultProfessores.aspx" class="UserToolbarTextArea">Importar Professores</a>
    <br />
    <a href="ImportarTurmas.aspx" class="UserToolbarTextArea">Importar Turmas</a>
    </table>
    &nbsp;</asp:Content>
