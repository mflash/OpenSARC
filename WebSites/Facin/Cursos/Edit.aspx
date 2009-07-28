<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Edit.aspx.cs" Inherits="Cursos_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR CURSO"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<table style="WIDTH: 386px" align="left">
<tbody>
<tr>
<td style="WIDTH: 70px; HEIGHT: 16px" class="ms-toolbar">Código:
</td>
<td style="HEIGHT: 16px"><asp:TextBox id="txtCodigo" runat="server" CssClass="ms-toolbar" 
Width="200px" ReadOnly="True"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ControlToValidate="txtCodigo"
        ErrorMessage="Campo Código não pode estar vazio.">*
        </asp:RequiredFieldValidator>
        </td>
        </tr>
        <tr>
        <td style="WIDTH: 70px; HEIGHT: 16px" class="ms-toolbar">Nome:
        </td>
        <td style="HEIGHT: 16px">
        <asp:TextBox id="txtNome" runat="server" CssClass="ms-toolbar" Width="200px"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
        ErrorMessage="Campo Nome não pode estar vazio.">*
        </asp:RequiredFieldValidator>
        </td>
        </tr>
        <tr>
        <td style="WIDTH: 70px; HEIGHT: 21px" 
        class="ms-toolbar">Faculdade:&nbsp;
        </td>
        <td style="HEIGHT: 21px">
        <asp:DropDownList id="ddlFaculdade" runat="server" CssClass="ms-toolbar" Width="200px">
        </asp:DropDownList> &nbsp; 
        </td>
        </tr>
        <tr>
        <td colspan="2"><asp:Label id="lblStatus" 
        runat="server" CssClass="ms-toolbar" Text="Label" Visible="False">
        </asp:Label>
        <BR />
    <asp:LinkButton ID="lbtnVoltar" runat="server" CausesValidation="False" CssClass="ms-toolbar"
        OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton><BR />
        <asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" 
        CssClass="ms-toolbar" Text="Ok" Width="62px"></asp:Button>
        </td>
        </tr>
        </tbody>
        </table>
</ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar" />
</asp:Content>
