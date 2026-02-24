<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="ListaTurmas.aspx.cs" Inherits="Pagina2" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../UserControls/SelecionaCalendario.ascx" TagName="SelecionaCalendario"
    TagPrefix="uc2" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="Label1" runat="server" CssClass="ms-WPTitle" Text="LISTA DE TURMAS"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
&nbsp;<table style="WIDTH: 806px; HEIGHT: 71px" align="left"><tbody><tr><td style="WIDTH: 292px">
    <asp:CheckBox runat="server" ID="chkMostrarNotes" CssClass="ms-WPTitle" Text="Apenas turmas com notebooks" AutoPostBack="true" />
<asp:GridView id="grvListaTurmas" runat="server" Width="900px" 
AutoGenerateColumns="False" OnRowDeleting="grvListaTurmas_RowDeleting" 
                    OnRowEditing="grvListaTurmas_RowEditing" DataKeyNames="Id" 
                    AllowSorting="True" AlternatingRowStyle-BackColor="#E0E0E0">
                    <Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Numero" HeaderText="N&#250;mero">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Disciplina.CodCred" HeaderText="CodCred">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" Width="60px"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Infra" HeaderText="Infra">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" Width="30px"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Disciplina" HeaderText="Disciplina" ControlStyle-Width="280px">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird" Width="280px"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="DataHora" HeaderText="Data &amp; Hora">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Professor" HeaderText="Professor">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Curso" HeaderText="Curso">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Sala" HeaderText="Sala" SortExpression="Sala">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
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
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
            OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Excluir"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela"></AlternatingRowStyle>
</asp:GridView> </td><td style="WIDTH: 3px">
</td>
</tr>
<tr>
<td style="WIDTH: 292px" colspan="1" rowspan="1">
<asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></td><td style="WIDTH: 3px">
</td>
</tr>
<tr>
<td style="WIDTH: 292px" colspan="1" rowspan="1">
<asp:LinkButton id="LinkButton1" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">
Voltar
</asp:LinkButton> 
</td>
<td style="WIDTH: 3px">
</td>
</tr>
<tr>
<td style="WIDTH: 292px; HEIGHT: 2px">
</td>
<td style="WIDTH: 3px; HEIGHT: 2px">
</td>
</tr>
</tbody>
</table>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

