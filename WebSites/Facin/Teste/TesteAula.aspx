<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TesteAula.aspx.cs" Inherits="Teste_TesteAula" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Turma"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        <asp:TextBox ID="txt_id_turma" runat="server"></asp:TextBox>&nbsp;&nbsp; &nbsp;
        &nbsp; &nbsp;
        <asp:Label ID="Label6" runat="server" Text="AulaID"></asp:Label>
        <asp:TextBox ID="txt_aulaid" runat="server"></asp:TextBox>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <br />
        <asp:Label ID="Label2" runat="server" Text="Hora"></asp:Label>
        &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:TextBox ID="txt_hora" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label3" runat="server" Text="Data"></asp:Label>
        &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
        <asp:TextBox ID="txt_data" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label4" runat="server" Text="Descricao"></asp:Label>
        &nbsp; &nbsp;&nbsp;
        <asp:TextBox ID="txt_descricao" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label5" runat="server" Text="Categoria"></asp:Label>
        &nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txt_categoria" runat="server"></asp:TextBox><br />
        <br />
        &nbsp;<asp:Label ID="lbl_resultado" runat="server" Width="236px"></asp:Label>
        &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Label ID="lbl_resultado2" runat="server" Width="201px"></asp:Label><br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Inserir" />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Deletar" /><br />
        <br />
        <br />
        <br />
        <asp:Label ID="Label7" runat="server" Text="AulaID"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        <asp:TextBox ID="txt_aulaid_altera" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label8" runat="server" Text="Descricao"></asp:Label>
        &nbsp; &nbsp; &nbsp;
        <asp:TextBox ID="txt_descricao_altera" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label9" runat="server" Text="Categoria"></asp:Label>
        &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:TextBox ID="txt_categoria_altera" runat="server"></asp:TextBox><br />
        <br />
        <asp:Label ID="lbl_resultado3" runat="server"></asp:Label><br />
        <br />
        <asp:Button ID="btn_alterar" runat="server" OnClick="btn_alterar_Click" Text="Alterar" /><br />
        <br />
        <br />
        <asp:Button ID="btn_listar" runat="server" OnClick="btn_listar_Click" Text="Listar" /><br />
        <br />
        <asp:GridView ID="grd_aulas" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SARCFACINcs %>"
            SelectCommand="SELECT * FROM [Aulas]"></asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
