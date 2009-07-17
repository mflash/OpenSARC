<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="List.aspx.cs" Inherits="Cursos_List" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="LISTA DE CURSOS"></asp:Label>
    </div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<uc1:Aguarde id="Aguarde1" runat="server"></uc1:Aguarde>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 198px" align="left"><TBODY><TR>
<TD colSpan=3>
<asp:GridView id="grvListaCursos" runat="server" 
OnRowEditing="grvListaCursos_RowEditing" OnRowDeleting="grvListaCursos_RowDeleting" 
OnSelectedIndexChanged="grvListaProfessores_SelectedIndexChanged" DataKeyNames="Codigo" 
AllowSorting="True" Width="636px" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="Codigo" HeaderText="Codigo">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="Nome" HeaderText="Nome">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="Vinculo" HeaderText="Faculdade">
<ItemStyle CssClass="ms-toolbar"></ItemStyle>
</asp:BoundField>
    <asp:TemplateField ShowHeader="False">
        <EditItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                Text="Alterar"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                Text=""></asp:LinkButton>
        </EditItemTemplate>
        <ControlStyle CssClass="ms-toolbar" />
        <ItemStyle CssClass="ms-wikieditthird" />
        <HeaderStyle CssClass="ms-wikieditthird" />
        <ItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
             CommandName="Edit"
                Text="Editar"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
             OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Excluir"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<RowStyle CssClass="ms-toolbar"></RowStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela" Wrap="True"></AlternatingRowStyle>
</asp:GridView>
</TD></TR><TR><TD colSpan="3"><asp:Label id="lblStatus" runat="server" 
CssClass="lblstatus" Visible="False"></asp:Label></TD></TR><TR><TD colSpan=3>
<asp:LinkButton
 id="LinkButton1" runat="server" CssClass="ms-toolbar" OnClick="LinkButton1_Click">Voltar
 </asp:LinkButton></TD></TR></TBODY></TABLE>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="grvListaCursos"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>
