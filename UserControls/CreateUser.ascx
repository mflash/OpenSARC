<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateUser.ascx.cs" Inherits="Default_CreateUser" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<asp:Wizard ID="wzCreateUser" runat="server" ActiveStepIndex="0" DisplaySideBar="False"
    FinishCompleteButtonText="Criar Usuário" FinishPreviousButtonText="Voltar" StartNextButtonText="Preencher dados"
    StepNextButtonText="Próximo" StepPreviousButtonText="Voltar" OnFinishButtonClick="Wizard1_FinishButtonClick" OnNextButtonClick="Wizard1_NextButtonClick" OnPreviousButtonClick="btnRedirecionar_Click" Width="320px">
    <WizardSteps>
        <asp:WizardStep runat="server" StepType="Start" Title="Sele&#231;&#227;o pap&#233;is">
            <asp:RadioButtonList ID="rblRoles" runat="server" BackColor="White" BorderColor="White"
                CssClass="ms-toolbar">
            </asp:RadioButtonList>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="rblRoles"
                CssClass="ms-toolbar">Selecione uma opção.</asp:RequiredFieldValidator>
        </asp:WizardStep>
        <asp:WizardStep runat="server" StepType="Finish" Title="Preencher dados" OnDeactivate="cuwCriarUsuario">
            <table style="width: 500px">
                <tr>
                    <td class="ms-toolbar" style="width: 50px; text-align: right; height: 23px;">
                        <asp:Label ID="lblNome" runat="server" CssClass="ms-toolbar" Text="Nome:"></asp:Label>
                    </td>
                    <td style="width: 100px; height: 23px;">
                        <asp:TextBox ID="txtNome" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                    </td>
                    <td style="height: 23px; width: 270px;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNome"
                            ErrorMessage="Digite um nome." EnableClientScript="False">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 50px; text-align: right; height: 23px;">
                        Login:</td>
                    <td style="width: 100px; height: 23px;">
                        <asp:TextBox ID="txtLogin" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                    </td>
                    <td style="height: 23px; width: 270px;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLogin"
                            ErrorMessage="Digite um login." EnableClientScript="False">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 50px; text-align: right; height: 23px;">
                        <asp:Label ID="lblSenha1" runat="server" CssClass="ms-toolbar" Text="Senha:"></asp:Label>
                    </td>
                    <td style="width: 100px; height: 23px;">
                        <asp:TextBox ID="txtSenha" runat="server" CssClass="ms-toolbar" TextMode="Password"
                            Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 270px; height: 23px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSenha"
                            ErrorMessage="Digite uma senha." EnableClientScript="False" Height="18px">*</asp:RequiredFieldValidator>
                        &nbsp;<asp:Label ID="lblsenha" runat="server" CssClass="ms-toolbar"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 50px; text-align: right; height: 23px;">
                        <asp:Label ID="lblConfirmaSenha" runat="server" CssClass="ms-toolbar" Text="Confirma senha:"></asp:Label>
                    </td>
                    <td style="width: 100px; height: 28px;">
                        <asp:TextBox ID="txtConfirmarSenha" runat="server" CssClass="ms-toolbar" TextMode="Password"
                            Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 270px; height: 28px;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmarSenha"
                            ErrorMessage="Digite a confirma&#231;&#227;o de sua senha." EnableClientScript="False">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 50px; text-align: right; height: 23px;">
                        E-mail:</td>
                    <td style="width: 100px; height: 23px">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                    </td>
                    <td style="height: 23px; width: 270px;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Digite um endere&#231;o de e-mail." EnableClientScript="False">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Digite um endere&#231;o de e-mail v&#225;lido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="False">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
            <asp:CheckBox ID="ckbSenha" runat="server" AutoPostBack="True" CssClass="ms-toolbar"
                OnCheckedChanged="ckbSenha_CheckedChanged" Text="Gerar senha automaticamente." />
            <br />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtSenha"
                            ControlToValidate="txtConfirmarSenha" CssClass="ms-toolbar" Display="Dynamic">*Senha e confirmação diferentes! Por favor, digite novamente sua senha,</asp:CompareValidator>
            <br />
            <asp:ValidationSummary ID="vsCreateUser" runat="server" CssClass="ms-toolbar" />
        </asp:WizardStep>
        <asp:WizardStep runat="server" StepType="Complete" Title="Ok">
            <div style="text-align:center;">
            Usuário criado com sucesso!<br />
            <br />
            <asp:Button ID="btnRedirecionar" runat="server" Text="OK" CssClass="mst-toolbar" Width="80px" OnClick="btnRedirecionar_Click" />
            </div>
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>
            <asp:Label ID="lblstatus" runat="server"></asp:Label>
