<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="AlteraDisciplina.aspx.cs" Inherits="Recursos_Disciplina" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align  = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblDisciplina" runat="server" CssClass="ms-WPTitle" Text="EDITAR DISCIPLINA"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table style="width: 510px" align = left>
        <tr>
            <td class="ms-toolbar" style="width: 93px">
                Código:
            </td>
            <td>
                &nbsp;<asp:Label ID="lblCodigo" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="width: 93px; height: 24px;">
                Créditos:</td>
            <td style="height: 24px">
                <asp:TextBox ID="txtCreditos" runat="server" Height="20px" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCreditos"
                    ErrorMessage="Créditos devem ser inseridos">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCreditos"
                    ErrorMessage="Créditos deve ser um número maior que zero" MaximumValue="100"
                    MinimumValue="1" Type="Integer">*</asp:RangeValidator></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="width: 93px; height: 24px">
                Nome:</td>
            <td style="height: 24px">
                <asp:TextBox ID="txtNome" runat="server" Height="20px" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNome"
                    ErrorMessage="Nome deve ser inserido">*</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="width: 93px; height: 24px">
                G2:</td>
            <td style="height: 24px">
                <asp:RadioButtonList ID="rdbG2" runat="server" BackColor="White" CssClass="ms-toolbar"
                    RepeatDirection="Horizontal" Width="200px" BorderStyle="None">
                    <asp:ListItem>N&#227;o</asp:ListItem>
                    <asp:ListItem>Sim</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rdbG2"
                    ErrorMessage="G2 deve ser selecionado">*</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="width: 93px; height: 22px">
                Categoria:</td>
            <td style="height: 22px">
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="ms-toolbar" Height="20px"
                    Width="200px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="ms-toolbar" colspan="2">
                &nbsp;<asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" Height="20px"
                    OnClick="btnConfirmar_Click" Text="Confirmar" /></td>
        </tr>
        <tr>
            <td class="ms-toolbar" style="width: 93px; height: 22px">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
            <td style="height: 22px">
            </td>
        </tr>
    </table>
</asp:Content>
