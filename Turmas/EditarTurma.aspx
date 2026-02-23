<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
    CodeFile="EditarTurma.aspx.cs" Inherits="Pagina6" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="EDITAR TURMA"></asp:Label>
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
            <td colspan="2" style="height: 15px" valign="middle">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 510px" align="left">
                            <tbody>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Disciplina:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:DropDownList ID="ddlDisciplina" runat="server" CssClass="ms-toolbar" Width="200px" Enabled="False">
                                        </asp:DropDownList></td>
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
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Data &amp; Hora:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:TextBox ID="txtDataHora" runat="server" CssClass="ms-toolbar" Width="200px" Enabled="False"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataHora" CssClass="ms-toolbar"
                                            ErrorMessage="Digite uma Data & Hora.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revDataHora" runat="server" ControlToValidate="txtDataHora"
                                            CssClass="ms-toolbar" ErrorMessage="Formato de Data & Hora inválido!" ValidationExpression="([2-7][A-N|P|X]{1,2}[\s]{0,1})+">*</asp:RegularExpressionValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Professor:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:DropDownList ID="ddlProfessor" runat="server" CssClass="ms-toolbar" Width="200px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Curso:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="ms-toolbar" Width="200px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; height: 16px" class="ms-toolbar">Uso de Notebook:</td>
                                    <td style="width: 267px; height: 16px">
                                        <asp:CheckBox ID="chkNotebook" runat="server" CssClass="ms-toolbar">
                                        </asp:CheckBox></td>
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
                                    <td style="width: 20px; height: 26px" class="ms-toolbar">
                                        <asp:Button ID="btnConfirmar" OnClick="btnConfirmar_Click1" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Font-Size="10px" Height="20px"></asp:Button></td>
                                    <td style="width: 267px; height: 26px" class="ms-toolbar">&nbsp;</td>
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
            <td style="width: 50px; height: 26px">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton><br />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
                    DisplayMode="List" Width="229px" />
            </td>
            <td style="width: 267px; height: 26px"></td>
        </tr>
    </table>
</asp:Content>
