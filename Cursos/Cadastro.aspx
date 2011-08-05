<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Cadastro.aspx.cs" Inherits="Cursos_Cadastro" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR CURSO"></asp:Label>
    </div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<uc1:Aguarde id="Aguarde1" runat="server"></uc1:Aguarde>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 386px" align="left">
<TBODY>
<TR>
<TD style="WIDTH: 70px; HEIGHT: 16px" class="ms-toolbar">Código:
</TD>
<TD style="HEIGHT: 16px">
<asp:TextBox id="txtCodigo" runat="server" Width="200px"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo"
        CssClass="ms-toolbar" ErrorMessage="Digite um código.">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
    runat="server" ControlToValidate="txtCodigo"
        CssClass="ms-toolbar" 
        ErrorMessage="O código deve ser preenchido apenas com números, no formato 12/1." 
        ValidationExpression="[0-9]+/[0-9]+">*</asp:RegularExpressionValidator>
        </TD>
        </TR>
        <TR>
        <TD style="WIDTH: 70px; HEIGHT: 16px" class="ms-toolbar">Nome:</TD>
        <TD style="HEIGHT: 16px"><asp:TextBox id="txtNome" runat="server" Width="200px">
        </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNome"
        CssClass="ms-toolbar" ErrorMessage="Digite um nome.">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtNome"
        CssClass="ms-toolbar" ErrorMessage="Digite apenas caracteres." ValidationExpression="[a-z-A-Z| |ç|ã|õ|ê|ô]*">*
        </asp:RegularExpressionValidator>
        </td>
        </tr>
        <tr>
        <td style="WIDTH: 70px; HEIGHT: 16px" class="ms-toolbar">Faculdade:
        </td>
        <td style="HEIGHT: 16px"><asp:DropDownList id="ddlFaculdade" runat="server" CssClass="ms-toolbar" 
       Width="200px"></asp:DropDownList>&nbsp;&nbsp; </TD></TR><TR><TD colSpan=2><asp:Label id="lblStatus" 
       runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label><BR />
    <asp:LinkButton ID="lbtnVoltar" runat="server" CausesValidation="False" CssClass="ms-toolbar"
        OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>&nbsp;<br />
    <BR /><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar"
     Text="Cadastrar"></asp:Button></TD></TR></TBODY></TABLE><BR />
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
        DisplayMode="List" />
</asp:Content>
