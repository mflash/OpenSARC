<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="DownloadHTML.aspx.cs" Inherits="Eventos_DownloadHTML" Title="Untitled Page"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <asp:DataGrid ID="dgEvento" runat="server" AutoGenerateColumns="False" 
        onselectedindexchanged="dgEvento_SelectedIndexChanged">
        <ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center" />
        <HeaderStyle BackColor="AliceBlue" CssClass="ms-toolbar" Font-Bold="True" Height="25px"
            HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateColumn HeaderText="T&#237;tulo">
                <ItemTemplate>
                    <asp:Label ID="lblTitulo" runat="server" Text='<%# Bind("Título") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Respons&#225;vel">
                <ItemTemplate>
                    <asp:Label ID="lblResponsavel" runat="server" Text='<%# Bind("Responsável") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Descri&#231;&#227;o">
                <ItemTemplate>
                    <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("Descrição") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Unidade">
                <ItemTemplate>
                    <asp:Label ID="lblUnidade" runat="server" Text='<%# Bind("Unidade") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Autor">
                <ItemTemplate>
                    <asp:Label ID="lblAutor" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Autor") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        
    </asp:DataGrid>
    
</asp:Content>
