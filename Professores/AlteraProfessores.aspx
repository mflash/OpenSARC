<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="AlteraProfessores.aspx.cs" Inherits="Professores_AlteraProfessores" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="EDITAR PROFESSOR"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 510px" align="left">
<TBODY>
<TR>
<TD style="WIDTH: 19px; HEIGHT: 21px" class="ms-toolbar">Matrícula:</TD>
<TD style="WIDTH: 273px; HEIGHT: 21px">
    <asp:TextBox ID="txtMatricula" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
    </TD>
    </TR>
    <TR>
    <TD style="WIDTH: 19px; HEIGHT: 26px" class="ms-toolbar">Nome:
    </TD>
    <TD style="WIDTH: 273px; HEIGHT: 26px">
    <asp:TextBox ID="txtNome" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
    </TD>
    </TR>
    <TR>
    <TD style="WIDTH: 19px; HEIGHT: 25px" class="ms-toolbar">E-mail:&nbsp;</TD>
    <TD style="WIDTH: 273px; HEIGHT: 25px"><asp:TextBox id="txtEmail" runat="server" Width="200px">
    </asp:TextBox> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" Width="17px" ControlToValidate="txtEmail" ErrorMessage="RegularExpressionValidator" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Visible="False">*</asp:RegularExpressionValidator></TD></TR><TR><TD style="WIDTH: 19px; HEIGHT: 18px" class="ms-toolbar">Senha:</TD><TD style="WIDTH: 273px; HEIGHT: 18px"><asp:Button id="btnResetaSenha" onclick="btnResetaSenha_Click" runat="server" CssClass="ms-toolbar" Text="Resetar Senha" Width="126px"></asp:Button>&nbsp; </TD></TR>
    <tr>
        <td class="ms-toolbar" style="width: 19px; height: 18px">
            Status:</td>
        <td style="width: 273px; height: 18px">
            <asp:Button ID="btnLockUnlock" runat="server" CssClass="ms-toolbar" OnClick="btnLockUnlock_Click"
                Text="Button" Width="126px" /></td>
    </tr>
    <TR><TD style="HEIGHT: 7px" colSpan=2><asp:ValidationSummary id="vsSumario" runat="server" CssClass="ms-toolbar" DisplayMode="List"></asp:ValidationSummary> </TD></TR><TR><TD style="HEIGHT: 7px" colSpan=2><asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 19px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="75px" Height="20px"></asp:Button></TD><TD style="WIDTH: 273px; HEIGHT: 26px">&nbsp;</TD></TR><TR><TD style="WIDTH: 19px; HEIGHT: 12px"><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD><TD style="WIDTH: 273px; HEIGHT: 12px"></TD></TR></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
