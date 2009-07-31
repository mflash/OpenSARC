<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recursos.aspx.cs"
 Inherits="Docentes_Recursos" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="controlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sistema de Alocação de Recursos - FACIN</title>
    <link href="../../CORE.CSS" rel="stylesheet" type="text/css" />
    <link href="../../CORE.CSS" rel="stylesheet" type="text/css" />
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>
<body onunload="opener.location= opener.location;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table >
            <tr>
                <td class="ms-toolbar" >
                    Categoria de Recurso:</td>
                <td >
                    <asp:DropDownList ID="ddlCategoriaRecurso" runat="server" CssClass="ms-toolbar" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCategoriaRecurso_SelectedIndexChanged" Width="177px">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="ms-toolbar"  >
                    <asp:Label ID="lblRecurso" runat="server" CssClass="ms-toolbar" Text="Recursos Disponíveis:"></asp:Label>
                </td>
                <td >
                    <asp:DropDownList ID="ddlRecurso" runat="server" CssClass="ms-toolbar" Enabled="False" Width="177px">
                    </asp:DropDownList>
                 </td>
                <td align="right">
                    <asp:Button ID="btnAdicionar" runat="server" CssClass="ms-toolbar" Text="Adicionar"
                     OnClick="btnAdicionar_Click" Width="80px" />
                  </td>
            </tr>
            <tr>
                <td >
                    </td>
                <td >
                    &nbsp;</td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ListBox ID="LBoxAlocados" runat="server" CssClass="ms-toolbar" Width="340px">
                    </asp:ListBox>
                    </td>
            </tr>
            <tr>
                <td  >
                    </td>
                <td style="text-align:right">
                    <asp:Button ID="btnLiberar" runat="server" Text="Liberar" CssClass="ms-toolbar" OnClick="btnLiberar_Click" Width="80px" /></td>
                <td align="right">
                    <asp:Button ID="btnConfirmar" runat="server" Text="Finalizar" CssClass="ms-toolbar" OnClick="btnConfirmar_Click" Width="80px"  /></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus"></asp:Label>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <uc1:Aguarde ID="Aguarde1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
