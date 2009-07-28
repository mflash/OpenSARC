<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TesteEvento.aspx.cs" Inherits="Teste_TesteEvento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="ProfessorId"></asp:Label>
        <asp:TextBox ID="txtProfessorId" runat="server"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="Descrição"></asp:Label>
        <asp:TextBox ID="txtDescricao" runat="server"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="DataHora"></asp:Label>
        <asp:TextBox ID="txtDataHora" runat="server"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="CalendarioId"></asp:Label>
        <asp:TextBox ID="txtCalendarioId" runat="server"></asp:TextBox>
        <asp:Button ID="btnInserir" runat="server" OnClick="btnInserir_Click" Text="INSERIR" />
        <asp:Label ID="lblResultadoInserir" runat="server"></asp:Label><br />
        <asp:Label ID="Label5" runat="server" Text="EventoId"></asp:Label>
        <asp:TextBox ID="txtEventoId" runat="server"></asp:TextBox>
        <asp:Button ID="btnDeletar" runat="server" OnClick="btnDeletar_Click" Text="DELETAR" />
        <asp:Label ID="lblResultadoDeletar" runat="server"></asp:Label><br />
        <asp:Label ID="Label10" runat="server" Text="EventoId"></asp:Label>
        <asp:TextBox ID="txtEventoId2" runat="server"></asp:TextBox>
        <asp:Label ID="lblProfessorId" runat="server" Text="ProfessorId"></asp:Label>
        <asp:TextBox ID="txtProfessorId2" runat="server"></asp:TextBox><asp:Label ID="Label6"
            runat="server" Text="Descrição"></asp:Label>
        <asp:TextBox ID="txtDescricao2" runat="server"></asp:TextBox>
        <asp:Button ID="btnAlterar" runat="server" OnClick="btnAlterar_Click" Text="ALTERAR" />
        <asp:Label ID="lblResultadoAlterar" runat="server"></asp:Label><br />
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#DEDFDE"
            BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1"
            ForeColor="Black" GridLines="Vertical">
            <FooterStyle BackColor="#CCCC99" />
            <RowStyle BackColor="#F7F7DE" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SARCFACINcs %>"
            SelectCommand="SELECT * FROM [Eventos]"></asp:SqlDataSource>
        &nbsp;
    </div>
    </form>
</body>
</html>
