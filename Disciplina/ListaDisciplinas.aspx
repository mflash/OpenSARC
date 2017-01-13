<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ListaDisciplinas.aspx.cs" Inherits="Disciplina_ListaDisciplinas" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="LISTA DE DISCIPLINAS">
        </asp:Label>
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
<table align="left">
<tbody>
<tr>
<td > 
<asp:GridView id="grvListaDisciplinas" 
runat="server" DataKeyNames="cod" OnRowEditing="grvListaDisciplinas_RowEditing" Width="600px"
 AutoGenerateColumns="False" OnRowDeleting="grvListaDisciplinas_RowDeleting" AlternatingRowStyle-BackColor="#E0E0E0">
 <Columns>
<asp:BoundField DataField="cod" HeaderText="C&#243;digo">
</asp:BoundField>
<asp:BoundField DataField="cred" HeaderText="Cr&#233;ditos">
</asp:BoundField>
<asp:BoundField DataField="nome" HeaderText="Nome"><ItemStyle width="200px" />
</asp:BoundField>
<asp:BoundField DataField="G2PorExtenso" HeaderText="G2">
</asp:BoundField>
    <asp:BoundField DataField="categoria" HeaderText="Categoria Disciplina" />
    <asp:TemplateField ShowHeader="False">
        <EditItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                Text="Update"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                Text="Cancelar"></asp:LinkButton>
        </EditItemTemplate>
        <ItemStyle CssClass="ms-wikieditthird" />
        <ItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                Text="Editar"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
            OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Deletar"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<RowStyle CssClass="ms-toolbar">
</RowStyle>

<HeaderStyle CssClass="ms-wikieditthird">
</HeaderStyle>
</asp:GridView>
</td>
</tr>
<tr>
<td>
<asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False">
</asp:Label>
</tr>
</td>
</tr>
<td>
        <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
        </tbody>
</table>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
