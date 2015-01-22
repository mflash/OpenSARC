<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="VerificaTurmas.aspx.cs" Inherits="Pagina2" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../UserControls/SelecionaCalendario.ascx" TagName="SelecionaCalendario"
    TagPrefix="uc2" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="Label1" runat="server" CssClass="ms-WPTitle" Text="VERIFICAÇÃO DO PREENCHIMENTO DE TURMAS"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
<asp:Label ID="lblPercentual" runat="server" CssClass="lblstatus" Text="Percentual de preenchimento: "></asp:Label><br />
&nbsp;<table style="WIDTH: 806px; HEIGHT: 71px" align="left"><tbody><tr><td style="WIDTH: 292px">
<asp:GridView id="grvListaTurmas" runat="server" Width="781px" 
AutoGenerateColumns="False" DataKeyNames="Id" 
                    AllowSorting="True" >
                    <Columns>
<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle CssClass="ms-toolbar"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Numero" HeaderText="N&#250;mero">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Disciplina" HeaderText="Disciplina">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="DataHora" HeaderText="Data &amp; Hora">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Professor" HeaderText="Professor">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Curso" HeaderText="Curso">
<ControlStyle Width="300px"></ControlStyle>
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>   

<asp:BoundField DataField="RecursosOK" HeaderText="Recursos">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="AulasOK" HeaderText="Aulas">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="ProvasOK" HeaderText="Provas">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="TrabalhosOK" HeaderText="Trab">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="G2OK" HeaderText="G2">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

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

