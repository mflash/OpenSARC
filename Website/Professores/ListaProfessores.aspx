<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ListaProfessores.aspx.cs" Inherits="Professores_ListaProfessores" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblListaProfs" runat="server" CssClass="ms-WPTitle" Text="LISTA DE PROFESSORES"></asp:Label></div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<uc1:Aguarde id="Aguarde1" runat="server"></uc1:Aguarde>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 514px; HEIGHT: 71px" align="left">
<TBODY>
<TR>
<TD>
<asp:GridView id="grvListaProfessores" 
runat="server" AutoGenerateColumns="False" Width="636px" AllowSorting="True" DataKeyNames="Id"
 OnRowDeleting="grvListaProfessores_RowDeleting" OnRowEditing="grvListaProfessores_RowEditing">
 <Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="Id"></asp:BoundField>
<asp:BoundField DataField="Matricula" HeaderText="Matr&#237;cula">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Nome" HeaderText="Nome">
<ControlStyle Width="150px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Email" HeaderText="E-mail">
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
</asp:GridView> 
</TD><TD ></TD></TR>
<TR><TD ><asp:Label id="lblStatus"
 runat="server" CssClass="lblstatus" Visible="False"></asp:Label></TD><TD >
 </TD></TR>
 <TR><TD colSpan=1 rowSpan=1>
 <asp:LinkButton id="LinkButton1" runat="server" CssClass="ms-toolbar"
  OnClick="LinkButton1_Click">Voltar</asp:LinkButton> </TD><TD ></TD></TR>
  </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
