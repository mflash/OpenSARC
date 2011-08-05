<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="AterarSenha.aspx.cs" Inherits="Docentes_AterarSenhaaspx" Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div align="center">
            <asp:ChangePassword ID="ChangePassword1" runat="server" CancelButtonText="Cancelar"
                ChangePasswordButtonText="Alterar Senha" ChangePasswordFailureText="Senha incorreta ou Nova senha inválida. Tamando mínimo da nova senha: {0}. Caracteres alfa-numéricos requeridos: {1}."
                ChangePasswordTitleText="Alterar Senha" ConfirmNewPasswordLabelText="Confirmar Nova Senha"
                CssClass="ms-toolbar" NewPasswordLabelText="Nova Senha:" PasswordLabelText="Senha:"
                SuccessText="Senha alterada com sucesso!" SuccessTitleText="Troca de senha completa."
                UserNameLabelText="Usuário:" CancelDestinationPageUrl="~/Default/PaginaInicial.aspx" ContinueDestinationPageUrl="~/Default/PaginaInicial.aspx" OnChangedPassword="ChangePassword1_ChangedPassword">
                <LabelStyle CssClass="ms-toolbar" />
                <CancelButtonStyle CssClass="ms-toolbar" />
                <ChangePasswordButtonStyle CssClass="ms-toolbar" />
                <ValidatorTextStyle CssClass="ms-toolbar" />
                <TextBoxStyle CssClass="ms-toolbar" />
            </asp:ChangePassword>
         </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

