<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="Admin_Results" MasterPageFile="~/Master/Master.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

<asp:Label ID="lblStatus" runat="server" CssClass="lblstatus" Text="Label" Visible="False"></asp:Label><br />
<asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" PostBackUrl="~/Admin/ControleEstados.aspx" Visible="False">Voltar</asp:LinkButton>
 
<table style="WIDTH: 806px; HEIGHT: 71px" align="left"><tbody><tr><td style="WIDTH: 292px">
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

<asp:BoundField DataField="Atendidos" HeaderText="Atendidos">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Pedidos" HeaderText="Pedidos">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Satisfacao" HeaderText="Satisfação">
<ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

</Columns>

<HeaderStyle CssClass="cabecalhoTabela"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela"></AlternatingRowStyle>
</asp:GridView> </td><td style="WIDTH: 3px">
</td>
</tr>
            </table>
</asp:Content>
