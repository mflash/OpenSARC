<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ImportarXLSX.aspx.cs" Inherits="ImportarDados_ImportarXLSX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <p>
        Importando dados de planilha_SARC.xlsx</p>
    <p>
        <asp:Button ID="butImportXLSX" runat="server" OnClick="butImportXLSX_Click" Text="Iniciar" />
    </p>
    <p>
        <asp:ListBox ID="ListBox1" runat="server" Height="442px" Width="777px"></asp:ListBox>
    </p>
</asp:Content>

