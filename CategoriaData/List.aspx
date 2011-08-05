<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="List.aspx.cs" Inherits="CategoriaData_List" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">


    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="LISTA DE CATEGORIAS DE DATAS"></asp:Label></div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 514px; HEIGHT: 71px" align=left><TBODY>
<TR><TD><asp:GridView id="grvListaCategoriaData" runat="server" 
Width="636px" AutoGenerateColumns="False" OnRowEditing="grvListaCategoriaData_RowEditing" OnRowDeleting="grvListaCategoriaData_RowDeleting" DataKeyNames="Id"><Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="Id"></asp:BoundField>
<asp:BoundField DataField="Descricao" HeaderText="Descri&#231;&#227;o">
<ControlStyle Width="200px"></ControlStyle>

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
                Text="Editar"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Excluir"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela"></AlternatingRowStyle>
</asp:GridView> </TD></TR><TR><TD style="WIDTH: 292px" colSpan=1 rowSpan=1><asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 292px" colSpan=1 rowSpan=1><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD></TR></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

