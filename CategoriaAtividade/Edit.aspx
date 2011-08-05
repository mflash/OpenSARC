<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Edit.aspx.cs" Inherits="CategoriaAtividades_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="UNLV.IAP.WebControls.HtmlColorDropDown" Namespace="UNLV.IAP.WebControls"
    TagPrefix="cc1" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR CATEGORIA DE ATIVIDADE"></asp:Label>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>  
    <table align="left" style="width: 510px">
        <tr>
            <td colspan="2" style="height: 26px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
<TABLE style="WIDTH: 510px"><TBODY><TR><TD style="WIDTH: 1px; HEIGHT: 16px" class="ms-toolbar">Descrição:</TD><TD style="WIDTH: 267px; HEIGHT: 16px"><asp:TextBox id="txtDescricao" runat="server" Width="223px"></asp:TextBox> <asp:RequiredFieldValidator id="rfvDescricao" runat="server" ControlToValidate="txtDescricao" ErrorMessage="Digite uma Descrição">*</asp:RequiredFieldValidator></TD></TR>
    <tr>
        <td class="ms-toolbar" style="width: 1px; height: 2px">
            Cor:</td>
        <td style="width: 267px; height: 2px">
            <cc1:HtmlColorDropDown ID="ddlCor" runat="server" ></cc1:HtmlColorDropDown>
        </td>
    </tr>
    <TR><TD style="WIDTH: 1px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px"></asp:Button></TD><TD style="WIDTH: 267px; HEIGHT: 26px">&nbsp;</TD></TR><TR><TD style="HEIGHT: 16px" colSpan=2><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 17px" colSpan=2><asp:ValidationSummary id="vsSumario" runat="server" CssClass="ms-toolbar" DisplayMode="List"></asp:ValidationSummary></TD></TR></TBODY></TABLE>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
