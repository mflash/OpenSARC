<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
    CodeFile="CadastroTurma2.aspx.cs" Inherits="Pagina6" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR TURMA"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table style="width: 510px" align="left">
        <tr>
            <td colspan="2" style="height: 14px" valign="middle">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 510px" align="left">
                            <tbody>
                                <tr>
                                    <td style="width: 20px; height: 26px" class="ms-toolbar">Disciplina:</td>
                                    <td style="width: 267px; height: 26px">
                                        <ajaxToolkit:ComboBox ID="cbDisciplina" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="500px">
                                        </ajaxToolkit:ComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Número:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:TextBox ID="txtNumero" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNumero"
                                            CssClass="ms-toolbar" ErrorMessage="Digite um número.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumero"
                                            CssClass="ms-toolbar" ErrorMessage="Digite apenas números." ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Data&nbsp;&amp;&nbsp;Hora:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:TextBox ID="txtDataHora" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataHora" CssClass="ms-toolbar"
                                            ErrorMessage="Digite uma Data & Hora.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revDataHora" runat="server" ControlToValidate="txtDataHora" ValidationExpression="([2-7][A-N|P|X]{1,2}[\s]{0,1})+" ErrorMessage="Formato de Data & Hora inválido!" CssClass="ms-toolbar">*</asp:RegularExpressionValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Professor:</td>
                                    <td style="width: 267px; height: 16px">
                                        <ajaxToolkit:ComboBox ID="cbProfessor" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="300px">
                                        </ajaxToolkit:ComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Curso:</td>
                                    <td style="width: 267px; height: 16px">
                                        <ajaxToolkit:ComboBox ID="cbCurso" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="300px">
                                        </ajaxToolkit:ComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Sala:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:TextBox ID="txtSala" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSala" runat="server" ControlToValidate="txtSala"
                                            CssClass="ms-toolbar" ErrorMessage="Digite o numero da sala(s)">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 26px">
                                        <asp:Button ID="btnConfirmar" OnClick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px"></asp:Button></td>
                                    <td style="width: 267px; height: 26px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 26px" colspan="2">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 55px; height: 26px">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
                    DisplayMode="List" Width="232px" />
            </td>
            <td style="width: 250px; height: 26px"></td>
        </tr>
    </table>
</asp:Content>
