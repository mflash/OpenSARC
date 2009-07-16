<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="Admin_Results" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" id="pnlAguarde">
            <img id="aguarde" src="../_layouts/images/working.gif" alt="" />
            <span style="color:black; font-family: Arial;">Aguarde... Distribuindo Recursos</span>
        </asp:Panel>
        <br />
        <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label><br />
        <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" PostBackUrl="~/Admin/ControleEstados.aspx" Visible="False">Voltar</asp:LinkButton>
    </form>
</body>
</html>
