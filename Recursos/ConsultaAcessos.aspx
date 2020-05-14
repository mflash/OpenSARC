<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ConsultaAcessos.aspx.cs" Inherits="Recursos_ConsultaAcessos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <h3><asp:Label ID="lblSala" Text=""  runat ="server"></asp:Label></h3>
    <table class="ms-form">
                    <tr>
                        <td>    
    <asp:GridView ID="grvAccessStats" AutoGenerateColumns="False" DataKeyNames="Sala" 
                    AllowSorting="True" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">                    
                            <Columns>

<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ItemStyle CssClass="ms-btoolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:HyperLinkField
            DataNavigateUrlFields="Sala"
            DataNavigateUrlFormatString="~\Recursos/ConsultaAcessos.aspx?Sala={0}"
            DataTextField="Sala"
            HeaderText="Sala"
            SortExpression="Sala">
<ControlStyle Width="50px"></ControlStyle>
<ItemStyle Width="50px"></ItemStyle>
</asp:HyperLinkField>

<asp:Boundfield
            DataField="Hosts"
            HeaderText="Hosts"
            SortExpression="Hosts" />

<asp:BoundField DataField="Fail" HeaderText="Falhas" SortExpression="Fail">
<ItemStyle CssClass="ms-btoolbar"></ItemStyle>
</asp:BoundField>

</Columns>

                            <FooterStyle BackColor="#CCCCCC" />

<HeaderStyle CssClass="cabecalhoTabela" BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela" BackColor="#CCCCCC"></AlternatingRowStyle>
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
</asp:GridView>
                            </td>
    <td>            
            <asp:GridView ID="grvDetails" AutoGenerateColumns="False" DataKeyNames="Host" 
                    AllowSorting="True" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">                    
                            <Columns>

<asp:BoundField DataField="Id" Visible="False" HeaderText="ID">
<ControlStyle Width="100px"></ControlStyle>
<ItemStyle CssClass="ms-btoolbar"></ItemStyle>
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Sala" HeaderText="Sala">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Pos" HeaderText="Pos">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:HyperLinkField
            DataNavigateUrlFields="Host"
            DataNavigateUrlFormatString="~\Recursos/ConsultaAcessos.aspx?Host={0}"
            DataTextField="Host"
            HeaderText="Host"
            SortExpression="Host" />

<asp:HyperLinkField
            DataNavigateUrlFields="User"
            DataNavigateUrlFormatString="~\Recursos/ConsultaAcessos.aspx?User={0}"
            DataTextField="User"
            HeaderText="Usuário"
            SortExpression="User" />

<asp:BoundField DataField="Datahora" HeaderText="Data e hora">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="OkUser" HeaderText="Usuário OK?">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="OkHost" HeaderText="Host OK?">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Mac" HeaderText="MAC">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="Obs" HeaderText="Obs">
<HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
</asp:BoundField>
</Columns>

                            <FooterStyle BackColor="#CCCCCC" />

<HeaderStyle CssClass="cabecalhoTabela" BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="linhaAlternadaTabela" BackColor="#CCCCCC"></AlternatingRowStyle>
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
</asp:GridView>
        </td>
                                <td>
        <asp:Image ID="imgMapa" ImageUrl="" runat="server" /></td>
    </tr>
    </table>

</asp:Content>

