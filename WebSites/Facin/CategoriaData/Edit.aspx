<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="Edit.aspx.cs" Inherits="CategoriaData_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="UNLV.IAP.WebControls.HtmlColorDropDown" Namespace="UNLV.IAP.WebControls"
    TagPrefix="cc1" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR CATEGORIA DE DATAS"></asp:Label>
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
<TABLE style="WIDTH: 510px"><TBODY><TR><TD style="WIDTH: 20px; HEIGHT: 26px"><asp:Label id="lblDescricao" runat="server" CssClass="ms-toolbar" Text="Descrição :"></asp:Label></TD><TD style="WIDTH: 267px; HEIGHT: 26px"><asp:TextBox id="txtDescricao" runat="server" Width="223px"></asp:TextBox></TD></TR>
    <tr>
        <td class="ms-toolbar" style="width: 20px; height: 11px">
            Dia Letivo:</td>
        <td style="width: 267px; height: 11px">
            <asp:RadioButtonList ID="rbDiaLetivo" runat="server" BackColor="White" BorderStyle="None"
                CssClass="ms-toolbar" RepeatDirection="Horizontal" Width="198px">
                <asp:ListItem Value="N&#227;o">N&#227;o</asp:ListItem>
                <asp:ListItem Value="Sim">Sim</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rbDiaLetivo"
                CssClass="ms-toolbar" ErrorMessage="Selecione um Dia Letivo">*</asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td style="width: 20px; height: 26px">
            <asp:Label ID="Label1" runat="server" CssClass="ms-toolbar" Text="Cor :"></asp:Label></td>
        <td style="width: 267px; height: 26px">
            <cc1:HtmlColorDropDown ID="ddlCor" runat="server" AutoPostBack="true"></cc1:HtmlColorDropDown>
        </td>
    </tr>
    <TR><TD style="WIDTH: 20px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Font-Size="10px" Height="20px"></asp:Button></TD><TD style="WIDTH: 267px; HEIGHT: 26px">&nbsp;</TD></TR><TR><TD style="HEIGHT: 26px" colSpan=2><asp:Label id="lblstatus" runat="server" CssClass="ms-toolbar" Text="Label" Visible="False"></asp:Label></TD></TR><TR><TD style="HEIGHT: 26px" colSpan=2><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD></TR></TBODY></TABLE>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
