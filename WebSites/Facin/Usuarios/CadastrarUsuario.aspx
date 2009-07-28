<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="CadastrarUsuario.aspx.cs" Inherits="Usuarios_Default" Title="Cadastrar Evento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="Server">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR USUÁRIO"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 415px">
                <tr>
                    <td class="ms-toolbar" style="width: 113px; height: 26px">
                        Tipo de usuário</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True" CssClass="ms-toolbar" Width="124px">
                            <asp:ListItem Value="0">Admin</asp:ListItem>
                            <asp:ListItem Value="1">Funcion&#225;rio</asp:ListItem>
                            <asp:ListItem Value="2">Professor</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 113px; height: 26px">
                        Nome</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtNome" runat="server" Enabled="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNome"
                            CssClass="ms-toolbar" ErrorMessage="Digite um Nome.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 113px; height: 26px;" class="ms-toolbar">
                        Login</td>
                    <td colspan="2" style=" width: 317px; height: 26px" class="ms-toolbar">
                        <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtLogin"
                            CssClass="ms-toolbar" ErrorMessage="Digite um login.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 113px; height: 26px">
                        Senha</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtSenha" runat="server"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSenha"
                            CssClass="ms-toolbar" ErrorMessage="Digite uma Senha.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 113px; height: 26px">
                        Confirmar Senha<br />
                    </td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtConfirmaSenha" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtConfirmaSenha"
                            CssClass="ms-toolbar" ErrorMessage="Digite uma Unidade.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 113px; height: 40px">
                        Email</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                            CssClass="ms-toolbar" ErrorMessage="Digite uma Unidade.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 146px">
                        <asp:CreateUserWizard ID="CreateUserWizard2" runat="server" CssClass="ms-toolbar">
                            <WizardSteps>
                                <asp:CreateUserWizardStep runat="server">
                                </asp:CreateUserWizardStep>
                                <asp:CompleteWizardStep runat="server">
                                </asp:CompleteWizardStep>
                            </WizardSteps>
                        </asp:CreateUserWizard>
                        &nbsp;<br />
                        <asp:Label ID="lblResultado" runat="server" CssClass="lblstatus"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar" Width="238px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 26px">
                        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click"
                         Text="OK" Width="95px" CssClass="ms-toolbar" /><br />
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 26px">
                        <br />
                        <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click1" CausesValidation="False">Voltar</asp:LinkButton></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

