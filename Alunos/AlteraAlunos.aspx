<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="AlteraAlunos.aspx.cs" Inherits="Alunos_AlteraAlunos" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="Editar Aluno"></asp:Label></div>
    <table style="width: 510px">
        <tr>
            <td style="width: 19px; height: 16px" class="ms-toolbar">
                Matrícula:</td>
            <td style="width: 267px; height: 16px">
                <asp:TextBox ID="txtMatricula" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 19px; height: 16px" class="ms-toolbar">
                Nome:</td>
            <td style="width: 267px; height: 16px">
                <asp:TextBox ID="txtNome" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 19px; height: 16px" class="ms-toolbar">
                E-mail: &nbsp; &nbsp;
            </td>
            <td style="width: 267px; height: 16px">
                <asp:TextBox ID="txtEmail" runat="server" Width="154px"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 19px; height: 16px" class="ms-toolbar">
                Senha:</td>
            <td style="width: 267px; height: 16px">
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" Width="153px"></asp:TextBox>
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 19px; height: 26px">
                <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" Height="20px"
                    Text="Confirmar" Width="75px" /></td>
            <td style="width: 267px; height: 26px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 19px; height: 26px">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar">Voltar</asp:LinkButton></td>
            <td style="width: 267px; height: 26px">
            </td>
        </tr>
    </table>
</asp:Content>
