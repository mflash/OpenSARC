<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelecaoRecursos.aspx.cs" Inherits="Docentes_SelecaoRecursos" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="controlToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Selecione os Recursos Desejados</title>
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>
<body  onunload="opener.location= opener.location;">


    <form id="form1" runat="server">
    <div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table style="width: 140px">
            <tr>
                <td class="ms-toolbar" style="width: 60px; height: 33px">
                    <asp:Label ID="lblOpcoes" runat="server" CssClass="ms-toolbar" Text="Opções:"></asp:Label></td>
                <td style="width: 251px; height: 33px">
        <asp:DropDownList ID="ddlPrioridadeRequisicao" runat="server" CssClass="ms-toolbar" OnSelectedIndexChanged="ddlRequisicoes_SelectedIndexChanged"
            Width="177px" AutoPostBack="True">
            <asp:ListItem Text="1&#170; Op&#231;&#227;o" Value="1" Selected="True"></asp:ListItem>
        </asp:DropDownList></td>
                <td style="width: 77px; height: 33px">
        <asp:Button ID="btnNovaOpcao" runat="server" CssClass="ms-toolbar"
            OnClick="btnNovaOpcao_Click" Text="Nova opção" Width="80px" /></td>
            </tr>
            <tr>
                <td class="ms-toolbar" style="width: 60px; height: 33px">
                    Categoria:</td>
                <td style="width: 251px; height: 33px">
                    <asp:DropDownList ID="ddlCategoriaRecurso" runat="server" CssClass="ms-toolbar" Width="177px" OnSelectedIndexChanged="ddlCategoriaRecurso_SelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True">
                        <asp:ListItem>Selecione</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 77px; height: 33px">
                    <asp:Button ID="btnRemover" runat="server" CssClass="ms-toolbar"
                        Text="Remover" OnClick="btnRemover_Click" Width="80px" /></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 33px; text-align: right">
                    <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" Text="Confirmar" OnClick="btnConfirmar_Click" Width="80px" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="ms-toolbar" OnClick="btnCancelar_Click" Width="80px"/><br />
        <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus"></asp:Label>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        &nbsp;&nbsp;
    </form>
</body>
</html>
