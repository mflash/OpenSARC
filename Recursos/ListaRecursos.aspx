<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ListaRecursos.aspx.cs" Inherits="Pagina2" Title="Sistema de Aloca��o de Recursos - FACIN" %>

<%@ Register Src="~/Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="Label1" runat="server" CssClass="ms-WPTitle" Text="LISTA DE RECURSOS"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 514px; HEIGHT: 71px" align="left">
<TBODY>
<TR>
<TD style="WIDTH: 292px"><asp:GridView id="grvListaRecursos" 
runat="server" DataKeyNames="Id" AllowSorting="True" 
AutoGenerateColumns="False" Width="636px" OnRowDeleting="grvListaRecursos_RowDeleting" 
OnRowEditing="grvListaRecursos_RowEditing"><Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="Id">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Categoria" HeaderText="Categoria">
<ControlStyle Width="150px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Descricao" HeaderText="Descri&#231;&#227;o">
<ControlStyle Width="200px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Abrev" HeaderText="Abrevia��o">
    <ItemStyle CssClass="ms-toolbar" />
</asp:BoundField>
        <asp:BoundField DataField="Tipo" HeaderText="Tipo">
            <ItemStyle CssClass="ms-toolbar" />
        </asp:BoundField>
        <asp:BoundField DataField="Vinculo" HeaderText="V�nculo">
            <controlstyle width="150px" />
            <ItemStyle CssClass="ms-toolbar" />
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
        </asp:BoundField>
<asp:CheckBoxField DataField="EstaDisponivel" HeaderText="Disponivel">
<ControlStyle Width="25px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:CheckBoxField>
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
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Excluir"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela"></AlternatingRowStyle>
</asp:GridView> </TD><TD style="WIDTH: 3px"></TD></TR><TR><TD style="WIDTH: 292px" colSpan=1 rowSpan=1><asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></TD><TD style="WIDTH: 3px"></TD></TR><TR><TD style="WIDTH: 292px" colSpan=1 rowSpan=1><asp:LinkButton id="LinkButton1" runat="server" CssClass="ms-toolbar" OnClick="LinkButton1_Click">Voltar</asp:LinkButton> </TD><TD style="WIDTH: 3px"></TD></TR><TR><TD style="WIDTH: 292px"></TD><TD style="WIDTH: 3px"></TD></TR></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

