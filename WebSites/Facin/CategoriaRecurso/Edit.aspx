<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Edit.aspx.cs" Inherits="CategoriaRecurso_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = left class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR CATEGORIA DE RECURSOS"></asp:Label>
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
<TABLE style="WIDTH: 510px"><TBODY><TR><TD style="WIDTH: 20px; HEIGHT: 26px"><TABLE style="WIDTH: 510px" align=left><TBODY><TR><TD style="WIDTH: 20px; HEIGHT: 2px" class="ms-toolbar">Descrição:</TD><TD style="WIDTH: 267px; HEIGHT: 2px"><asp:TextBox id="txtDescricao" runat="server" CssClass="ms-toolbar" Width="223px"></asp:TextBox> <asp:RequiredFieldValidator id="rfvDescricao" runat="server" CssClass="ms-toolbar" ErrorMessage="Digite uma Descrição" ControlToValidate="txtDescricao">*</asp:RequiredFieldValidator></TD></TR></TBODY></TABLE></TD><TD style="WIDTH: 267px; HEIGHT: 26px"></TD></TR><TR><TD style="WIDTH: 20px; HEIGHT: 26px"><asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px"></asp:Button></TD><TD style="WIDTH: 267px; HEIGHT: 26px">&nbsp;</TD></TR><TR><TD style="HEIGHT: 3px" colSpan=2><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD></TR><TR><TD style="HEIGHT: 29px" colSpan=2><asp:ValidationSummary id="vsSumario" runat="server" CssClass="ms-toolbar" Width="147px" DisplayMode="List"></asp:ValidationSummary></TD></TR></TBODY></TABLE>
</ContentTemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
