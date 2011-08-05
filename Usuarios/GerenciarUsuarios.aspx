<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="GerenciarUsuarios.aspx.cs" Inherits="Usuarios_GerenciarUsuarios" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="GERENCIAR USUÁRIOS"></asp:Label></div>
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
<TD style="HEIGHT: 21px" class="ms-toolbar">Selecione a categoria de usuário que deseja alterar:</TD></tr>
<tr><td style="width: 95px"><asp:DropDownList ID="ddlCategoriasUsuario" runat="server" ></asp:DropDownList></td>
</TR>
<TR><TD style="WIDTH: 95px; HEIGHT: 12px"> <asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="75px" Height="20px"></asp:Button></td></tr></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>