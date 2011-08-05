<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Edit.aspx.cs" Inherits="Vinculos_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>


<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR FACULDADE"></asp:Label>
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
                        <table style="width: 510px">
                            <tr>
                                <td style="width: 20px; height: 2px">
                                    &nbsp;</td>
                                <td style="width: 267px; height: 2px">
                                    &nbsp;</td>
                            </tr>
        <tr>
            <td style="width: 20px; height: 2px">
                <asp:Label ID="lblDescricao" runat="server" CssClass="ms-toolbar" Text="Descrição :"></asp:Label></td>
            <td style="width: 267px; height: 2px">
                <asp:TextBox ID="txtDescricao" runat="server" Width="223px"></asp:TextBox></td>
        </tr>

                            <tr>
                                <td style="width: 20px; height: 26px">
                                    <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" Font-Size="10px"
                                        Height="20px" Text="Confirmar" Width="79px" OnClick="btnConfirmar_Click" /></td>
                                <td style="width: 267px; height: 26px">
                &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 26px">
                                    <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 20px; height: 26px">
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar
                </asp:LinkButton>
                </td>
            <td style="width: 267px; height: 26px">
            </td>
        </tr>
    </table>
</asp:Content>
