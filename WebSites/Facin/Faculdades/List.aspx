<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="List.aspx.cs" Inherits="Vinculos_List" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="LISTA DE FACULDADES"></asp:Label>
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
<table style="width: 514px; height: 71px" align="left">
    <tr>
            <td>
    
    <asp:GridView ID="grvListaVinculos" runat="server" Width="636px" 
    OnRowDeleting="grvListaVinculos_RowDeleting" OnRowEditing="grvListaVinculos_RowEditing"
     AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" ReadOnly="True" />
            <asp:BoundField DataField="Nome" HeaderText="Descri&#231;&#227;o">
                <ItemStyle CssClass="ms-toolbar" />
                <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
                <ControlStyle Width="200px" />
            </asp:BoundField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Alterar"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancelar"></asp:LinkButton>
                </EditItemTemplate>
                <ControlStyle CssClass="ms-toolbar" />
                <ItemStyle CssClass="ms-wikieditthird" />
                <HeaderStyle BorderStyle="None" CssClass="ms-wikieditthird" />
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Editar"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="Excluir" OnClientClick = "return confirm_delete();"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="cabecalhoTabela" />
        <AlternatingRowStyle CssClass="linhaAlternadaTabela" />
    </asp:GridView>
            </td>
    </tr>
    <tr>
            <td colspan="" rowspan="" >
                <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus" Visible="False">
                </asp:Label></td>
    </tr>
    <tr>
        <td colspan="1" rowspan="1" >
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar
                </asp:LinkButton></td>
    </tr>
    </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grvListaVinculos" EventName="RowDeleted" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
