<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ImportarAcad.aspx.cs" Inherits="ImportarDados_ImportarAcad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <p>
    Importando dados do Planejamento Acadêmico da Escola Politécnica</p>
<p>
    <asp:Button ID="butImportAcad" runat="server" Text="Iniciar" OnClick="butImportAcad_Click" />
</p>
    <asp:CheckBox ID="checkSimul" runat="server" Text="Simular apenas" />&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtPredio" runat="server">32</asp:TextBox>
    <div id="output" runat="server" style="font-size: 14px;">
    </div>
<p>
    &nbsp;</p>
</asp:Content>
