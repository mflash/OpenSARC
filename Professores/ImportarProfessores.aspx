<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ImportarProfessores.aspx.cs" Inherits="Professores_ImportarProfessores" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblListaProfs" runat="server" CssClass="ms-WPTitle" Text="IMPORTAR PROFESSORES"></asp:Label></div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<uc1:Aguarde id="Aguarde1" runat="server"></uc1:Aguarde>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
 <asp:Button ID="btnImportar" runat="server" CssClass="ms-toolbar" Height="20px"
                    Text="Importar do acadêmico" OnClick="btnImportar_Click" />
<TABLE style="WIDTH: 514px; HEIGHT: 71px" align="left">
<TBODY>
<tr><td>
<asp:Label id="lblOnline"
 runat="server" CssClass="lblstatus" Visible="True"></asp:Label>
</td></tr>
<tr>
<TD>
<asp:GridView id="grvListaProfessores" 
runat="server" AutoGenerateColumns="False" Width="636px" AllowSorting="True" DataKeyNames="nome"
 AlternatingRowStyle-BackColor="#E0E0E0">
 <Columns>
<asp:BoundField DataField="matricula" HeaderText="Matr&#237;cula">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="nome" HeaderText="Nome">
<ControlStyle Width="150px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Unidade" HeaderText="Unidade">
<ControlStyle Width="200px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
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
