<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Cadastro.aspx.cs" Inherits="CategoriaData_Cadastro" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="UNLV.IAP.WebControls.HtmlColorDropDown" Namespace="UNLV.IAP.WebControls"
    TagPrefix="cc1" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="CADASTAR CATEGORIA DE DATAS">
        </asp:Label>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table style="width: 510px" align="left">
        <tr>
            <td colspan="2" style="height: 26px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
        <table style="WIDTH: 510px">
        <tbody>
        <tr>
        <td style="WIDTH: 20px; HEIGHT: 26px" class="ms-toolbar">Descrição:
        </td>
        <td style="WIDTH: 267px; HEIGHT: 26px"><asp:TextBox id="txtDescricao" runat="server" Width="223px">
        </asp:TextBox>
         <asp:RequiredFieldValidator id="rfvDescricao" runat="server" ControlToValidate="txtDescricao" 
                                    ErrorMessage="Campo Descrição não pode estar vazio.">*
         </asp:RequiredFieldValidator>
         </td>
         </tr>
            <tr>
                <td class="ms-toolbar" style="width: 20px; height: 11px">
                    Dia Letivo:
                    </td>
                <td style="width: 267px; height: 11px">
                    <asp:RadioButtonList ID="rbDiaLetivo" runat="server" CssClass="ms-toolbar" Width="198px" 
                                                BorderStyle="None" RepeatDirection="Horizontal" BackColor="White">
                        <asp:ListItem Value="N&#227;o">N&#227;o
                        </asp:ListItem>
                        <asp:ListItem Value="Sim">Sim
                        </asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="rbDiaLetivo"
                                                ErrorMessage="Selecione um Dia Letivo" CssClass="ms-toolbar">*
                        </asp:RequiredFieldValidator>
                        </td>
            </tr>
            <tr>
                <td class="ms-toolbar" style="width: 1px; height: 2px">
                    Cor:
                    </td>
                <td style="width: 267px; height: 2px">
                    &nbsp;<cc1:HtmlColorDropDown ID="ddlCor" runat="server" AutoPostBack="True">
                    </cc1:HtmlColorDropDown>
                </td>
            </tr>
            <tr>
            <td style="WIDTH: 20px; HEIGHT: 26px"><asp:Button id="btnConfirmar" 
            onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text=" Confirmar" 
            Width="79px" Height="20px" Font-Size="10px">
            </asp:Button>
            </td>
            <td style="WIDTH: 267px; HEIGHT: 26px">
            </td>
            </tr>
            <tr>
            <td style="HEIGHT: 12px" colspan="2">
            <asp:Label id="lblstatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label>
            </td>
            </tr>
            <tr>
            <td style="HEIGHT: 12px" colspan="2">
            <asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" 
                            OnClick="lbtnVoltar_Click">Voltar
            </asp:LinkButton>
            </td>
            </tr>
            <tr>
            <td style="HEIGHT: 26px" colspan="2">
            <asp:ValidationSummary id="vsSumario" runat="server" CssClass="ms-toolbar" DisplayMode="List">
            </asp:ValidationSummary>
            </td>
            </tr>
            </tbody>
            </table>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
