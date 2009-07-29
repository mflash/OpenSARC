<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="Erro.aspx.cs" Inherits="Erro" %>

<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px; background-color: #ff1841">
        <asp:Label ID="lblErro" runat="server" CssClass="ms-WPTitle" ForeColor="White" Text="ERRO"></asp:Label></div>
    <table style="width: 430px; height: 150px" align="left">
        <tr>
            <td class="ms-toolbar" style="width: 48px; height: 53px;" valign="top">
        <img src="../_layouts/images/error70by70.gif" alt="" style="height: 62px" width="62" /></td>
            <td class="ms-toolbar" style="height: 53px" valign="middle">
                <asp:Label ID="lblErroMensagem" runat="server" Text="Ocorreu um erro na aplicação. Os dados foram armazenados e enviados ao administrador do sistema."></asp:Label></td>
            <td style="height: 53px; width: 3px;">
            </td>
            <td style="height: 53px">
            </td>
        </tr>
        <tr>
            <td style="width: 48px">
            </td>
            <td>
            </td>
            <td style="width: 3px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 48px">
            </td>
            <td>
                <a onclick="history.go(-1)" id="linkVoltar" class="ms-toolbar">Voltar</a>
                </td>
            <td style="width: 3px">
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

