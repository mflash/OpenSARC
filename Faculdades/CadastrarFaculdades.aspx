<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="CadastrarFaculdades.aspx.cs" Inherits="Faculdades_CadastrarFaculdades" 
Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitulo" Runat="Server">
<div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR FACULDADE"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table align="left" style="width: 386px">
                <tbody>
                    <tr>
                        <td class="ms-toolbar" style="width: 70px; height: 16px">
                            Descrição:</td>
                        <td style="height: 16px">
                        <asp:TextBox ID="txtDescricao" runat="server" CssClass="ms-toolbar"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Campo Descricao não pode estar vazio." 
                        ControlToValidate="txtDescricao">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" Text="Confirmar"  
                        OnClick="btnConfirmar_Click"/><br />
                            <asp:Label ID="lblStatus" runat="server" CssClass="lblStatus" Visible="False"></asp:Label><br />
                            <asp:LinkButton ID="lbtnVoltar" runat="server" CausesValidation="False" 
                            CssClass="ms-toolbar"
                                OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></td>
                    </tr>
                </tbody>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table align="left">
    <tr>
    <td>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
        DisplayMode="List" />
        </td>
        </tr>
        </table>
        
</asp:Content>


