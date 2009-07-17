<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="CadastroProfessores.aspx.cs" Inherits="Professores_CadastroProfessores" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR PROFESSOR"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 359px" align ="left">
        <tr>
            <td class="ms-toolbar" style="height: 23px">
                Matricula:</td>
            <td style="height: 23px">
                <asp:TextBox ID="txtMatricula" runat="server" CssClass="ms-toolbar" Height="20px"
                    Width="200px"></asp:TextBox>
            </td>
            <td style="width: 34px; height: 23px">
                <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ControlToValidate="txtMatricula"
                    ErrorMessage="Digite uma matricula.">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMatricula"
                    ErrorMessage="Matrícula deve conter apenas números." ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="height: 24px">
                Nome:</td>
            <td style="height: 24px">
                <asp:TextBox ID="txtNome" runat="server" CssClass="ms-toolbar" Height="20px" Width="200px"></asp:TextBox>
            </td>
            <td style="width: 34px; height: 24px">
                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
                    ErrorMessage="Digite um Nome">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revTxtNome" runat="server" ControlToValidate="txtNome"
                    ErrorMessage="Digite apenas caracteres para o nome." ValidationExpression="([A-Za-z]+|\s)+">*</asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td class="ms-toolbar">
                E-mail:
                </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="ms-toolbar" Height="20px" Width="200px"></asp:TextBox>
            </td>
            <td style="width: 34px">
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Digite um e-mail.">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Digite um e-mail válido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="height: 28px">
                Pergunta Secreta:</td>
            <td style="height: 28px">
                <asp:TextBox ID="txtPergunta" runat="server" CssClass="ms-toolbar" Height="20px"
                    Width="200px"></asp:TextBox>
            </td>
            <td style="width: 34px; height: 28px">
                <asp:RequiredFieldValidator ID="rfvPergunta" runat="server" ControlToValidate="txtPergunta"
                    ErrorMessage="Digite uma pergunta.">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="ms-toolbar">
                Resposta Secreta:</td>
            <td>
                <asp:TextBox ID="txtResposta" runat="server" CssClass="ms-toolbar" Height="20px"
                    Width="200px"></asp:TextBox>
            </td>
            <td style="width: 34px">
                <asp:RequiredFieldValidator ID="rfvResposta" runat="server" ControlToValidate="txtResposta"
                    ErrorMessage="Digite uma resposta.">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 46px">
                &nbsp;<asp:Label ID="lblStatus" runat="server" CssClass="lblStatus" Text="Label" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2" style="height: 22px">
                <asp:Button ID="btnCadastrar" runat="server" CssClass="ms-toolbar" Height="20px"
                    Text="Cadastrar" OnClick="btnCadastrar_Click" /></td>
            <td colspan="1" style="width: 34px; height: 22px;">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></td>
            <td colspan="1" style="width: 34px">
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    &nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
                <asp:ValidationSummary ID="vsSumario" runat="server" CssClass="ms-toolbar" Width="342px" DisplayMode="List" />
    <br />
    <br />
    <br />
    &nbsp;
</asp:Content>
