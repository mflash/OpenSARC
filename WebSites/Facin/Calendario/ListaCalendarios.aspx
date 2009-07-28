<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="ListaCalendarios.aspx.cs" Inherits="Pagina2" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="Label1" runat="server" CssClass="ms-WPTitle" Text="LISTA DE SEMESTRES"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<table style="WIDTH: 514px; HEIGHT: 71px" align="left">
<tbody>
<tr>
<td >
<asp:GridView id="grvListaCalendarios" runat="server" 
DataKeyNames="Id" Width="636px" AutoGenerateColumns="False" OnRowEditing="grvListaCalendarios_RowEditing" 
OnSelectedIndexChanging="grvListaCalendarios_SelectedIndexChanging"><Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Ano" HeaderText="Ano">
<ControlStyle Width="200px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Semestre" HeaderText="Semestre">
<ControlStyle Width="150px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:CommandField UpdateText="Alterar" SelectText="Visualizar" NewText="Novo" 
CancelText="Cancelar" InsertText="Inserir" ShowDeleteButton="True" DeleteText="" 
EditText="Editar" ShowEditButton="True" ShowSelectButton="True">
<HeaderStyle BorderStyle="None" CssClass="ms-wikieditthird"></HeaderStyle>

<ItemStyle CssClass="ms-wikieditthird"></ItemStyle>

<ControlStyle CssClass="ms-toolbar"></ControlStyle>
</asp:CommandField>
</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela">
</AlternatingRowStyle>
</asp:GridView> 
</td><td >
</td>
</tr>
<tr>
<td  colspan="1" rowspan="1">
<asp:Label id="lblListaDatas" runat="server" CssClass="lblstatus" Text="Label" Visible="False">
</asp:Label> 
<asp:GridView id="grvListaDatas" runat="server" 
DataKeyNames="Date" Width="636px" AutoGenerateColumns="False" 
OnRowDeleting="grvListaDatas_RowDeleting" >

<Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="PorExtenso" HeaderText="Data">
<ControlStyle Width="150px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
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
                Text=""></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
            OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Excluir"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela"></AlternatingRowStyle>
</asp:GridView>
</td>
<td style="WIDTH: 3px">
</td>
</tr>
<tr>
<td style="WIDTH: 292px" colspan="1" 
rowspan="1"><asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></td><td style="WIDTH: 3px">
</td></tr><tr><td style="WIDTH: 292px" colspan="1" rowspan="1">
<asp:LinkButton id="LinkButton1" runat="server" CssClass="ms-toolbar" OnClick="LinkButton1_Click">
Voltar</asp:LinkButton> </td>
<td style="WIDTH: 3px"></td></tr><tr><td style="WIDTH: 292px; HEIGHT: 6px">
</td>
<td style="WIDTH: 3px; HEIGHT: 6px">
</td>
</tr>
</tbody>
</table>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

