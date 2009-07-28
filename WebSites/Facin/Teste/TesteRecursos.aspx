<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TesteRecursos.aspx.cs" Inherits="Teste_TesteRecursos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="data"></asp:Label>
        <asp:TextBox ID="txtData" runat="server"></asp:TextBox>
        &nbsp;
        <asp:Label ID="Label2" runat="server" Text="Horario"></asp:Label>
        <asp:TextBox ID="txtHorario" runat="server"></asp:TextBox>
        <asp:Button ID="btn_get" runat="server" OnClick="btn_get_Click" Text="Get" /><br />
        <asp:Label ID="lblstatus" runat="server" Text="Label"></asp:Label></div>
    </form>
</body>
</html>
