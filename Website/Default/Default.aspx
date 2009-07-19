<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Default.aspx.cs" Inherits="_Default" Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblLogin" runat="server" CssClass="ms-WPTitle" Text="LOGIN"></asp:Label></div>
    <table style="width: 100%">
        <tr>
            <td align="center" class="ms-toolbar" style="font-size: 42px;" valign="top">
                Benvindo</td>
        </tr>
        <tr>
            <td align="center" class="ms-toolbar" style="font-size: 42px" valign="top">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td align="center" class="ms-toolbar" style="font-size: 42px" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <asp:Login ID="loginEntrada" runat="server" CssClass="ms-toolbar" FailureText="Matrícula ou senha inválida. Por favor, tente novamente."
                    Font-Bold="False" LoginButtonText="Entrar" PasswordLabelText="Senha:" PasswordRequiredErrorMessage="Senha não pode ser nula."
                    RememberMeText="Lembrar de mim da próxima vez?" UserNameLabelText="Matrícula:"
                    UserNameRequiredErrorMessage="Matrícula não pode ser nula." Width="360px" DestinationPageUrl="~/Default/SelecionarCalendario.aspx" OnLoginError="loginEntrada_LoginError">
                    <TitleTextStyle CssClass="ms-wikieditthird" Font-Bold="False" Font-Names="Verdana"
                        Font-Size="12px" />
                    <CheckBoxStyle CssClass="ms-toolbar" />
                    <InstructionTextStyle CssClass="ms-toolbar" />
                    <TextBoxStyle CssClass="ms-toolbar" Width="200px" />
                    <LoginButtonStyle CssClass="ms-toolbar" />
                    <LabelStyle CssClass="ms-toolbar" />
                </asp:Login>
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
