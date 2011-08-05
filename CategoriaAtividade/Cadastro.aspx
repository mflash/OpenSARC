<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Cadastro.aspx.cs" Inherits="CategoriaAtividades_Cadastro" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="UNLV.IAP.WebControls.HtmlColorDropDown" Namespace="UNLV.IAP.WebControls"
    TagPrefix="cc1" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR CATEGORIA DE ATIVIDADE"></asp:Label>
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
<table style="WIDTH: 510px"><tbody>
<tr><td style="WIDTH: 1px; HEIGHT: 2px" class="ms-toolbar">Descrição:</TD>
<td style="WIDTH: 267px; HEIGHT: 2px">
<asp:TextBox id="txtDescricao" runat="server" Width="223px">
</asp:TextBox> 
<asp:RequiredFieldValidator id="rfvDescricao" runat="server" CssClass="ms-toolbar" Width="1px" 
Height="15px" ErrorMessage="Digite uma Descrição" ControlToValidate="txtDescricao">*
</asp:RequiredFieldValidator></td></tr>
    <tr>
        <td class="ms-toolbar" style="width: 1px; height: 2px">
            Cor:</td>
        <td style="width: 267px; height: 2px">
            <cc1:HtmlColorDropDown ID="ddlCor" runat="server" AutoPostBack="True" >
            </cc1:HtmlColorDropDown>
        </td>
    </tr>
    <tr><td style="WIDTH: 1px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click1" 
    runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px">
    </asp:Button></TD><TD style="WIDTH: 267px; HEIGHT: 26px">&nbsp;
    </TD></TR><TR><TD style="HEIGHT: 9px" colSpan=2><asp:Label id="lblstatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label></TD></TR><TR><TD style="HEIGHT: 1px" colSpan=2><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click" CausesValidation="False">Voltar</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 1px" colSpan=2><asp:ValidationSummary id="vsSumario" runat="server" CssClass="ms-toolbar" Width="144px" Height="1px" DisplayMode="List"></asp:ValidationSummary></TD></TR></TBODY></TABLE>
</ContentTemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
