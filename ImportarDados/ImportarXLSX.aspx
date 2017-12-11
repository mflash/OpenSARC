<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ImportarXLSX.aspx.cs" Inherits="ImportarDados_ImportarXLSX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <p>
    Importando dados de planilha_SARC.xlsx</p>
<p>
    <asp:Button ID="butImportXLSX" runat="server" OnClick="butImportXLSX_Click" Text="Iniciar" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="butClearDB" runat="server" OnClick="butClearDB_Click" Text="Limpar profs" />
</p>
    <div id="output" runat="server"></div>
<p>
    &nbsp;</p>
</asp:Content>

