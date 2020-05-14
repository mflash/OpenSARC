<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="CadastroTurma2.aspx.cs" Inherits="Pagina6" Title="Sistema de Aloca��o de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR TURMA"></asp:Label></div>
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
<TABLE style="WIDTH: 510px" align=left><TBODY><TR><TD style="WIDTH: 20px; HEIGHT: 26px" class="ms-toolbar">Disciplina:</TD><TD style="WIDTH: 267px; HEIGHT: 26px">            
    <ajaxToolkit:ComboBox ID="cbDisciplina" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="500px">
    </ajaxToolkit:ComboBox>
    </TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 16px" class="ms-toolbar">N�mero:</TD><TD style="WIDTH: 267px; HEIGHT: 16px"><asp:TextBox id="txtNumero" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNumero"
                        CssClass="ms-toolbar" ErrorMessage="Digite um n�mero.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumero"
                        CssClass="ms-toolbar" ErrorMessage="Digite apenas n�meros." ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 16px" class="ms-toolbar">Data&nbsp;&amp;&nbsp;Hora:</TD><TD style="WIDTH: 267px; HEIGHT: 16px"><asp:TextBox id="txtDataHora" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataHora" CssClass="ms-toolbar"
                        ErrorMessage="Digite uma Data & Hora.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator id="revDataHora" runat="server" ControlToValidate="txtDataHora" ValidationExpression="([2-7][A-N|P|X]{1,2}[\s]{0,1})+" ErrorMessage="Formato de Data & Hora inv�lido!" CssClass="ms-toolbar">*</asp:RegularExpressionValidator></TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 16px" class="ms-toolbar">Professor:</TD><TD style="WIDTH: 267px; HEIGHT: 16px">
        <ajaxToolkit:ComboBox ID="cbProfessor" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="300px">
        </ajaxToolkit:ComboBox>
        </TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 16px" class="ms-toolbar">Curso:</TD><TD style="WIDTH: 267px; HEIGHT: 16px">
        <ajaxToolkit:ComboBox ID="cbCurso" runat="server" AutoCompleteMode="SuggestAppend" CssClass="ms-toolbar" Width="300px">
        </ajaxToolkit:ComboBox>
        </TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px"></asp:Button></TD><TD style="WIDTH: 267px; HEIGHT: 26px">&nbsp;</TD></TR><TR><TD style="HEIGHT: 26px" colSpan=2><asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></TD></TR></TBODY></TABLE>
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
            <td style="width: 250px; height: 26px">
            </td>
        </tr>
    </table>
</asp:Content>
