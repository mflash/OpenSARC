<%@ page language="C#" masterpagefile="~/Master/Master.master" autoeventwireup="true" CodeFile="DownloadHTML.aspx.cs" Inherits="Docentes_DownloadHTML" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <asp:DataGrid ID="dgAulas" runat="server" AutoGenerateColumns="False" 
        HorizontalAlign="Center"
        Width="100%" BorderColor="#E0E0E0">
        <ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center" BorderColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px" Height="40px" />
        <HeaderStyle CssClass="ms-toolbar" HorizontalAlign="Center" BackColor="AliceBlue" Font-Bold="True" Height="25px" />
        <Columns>
            
            <asp:TemplateColumn HeaderText="#">
                <ItemTemplate>
                    <asp:Label ID="lblAula" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"#") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
                <HeaderStyle CssClass=".ms-wikieditthird" />
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Dia">
                <ItemTemplate>
                    <asp:Label ID="lblDia" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem, "Dia"))%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
			
			<asp:TemplateColumn HeaderText="Data">
                <ItemTemplate>
                    <asp:Label ID="lblData" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem, "Data"))%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>

			<asp:TemplateColumn HeaderText="Hora">
                <ItemTemplate>
                    <asp:Label ID="lblHora" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem, "Hora"))%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>			
			         
            <asp:TemplateColumn HeaderText="Descri&#231;&#227;o">
                <ItemTemplate>
                    <asp:Label ID="lblDescricao" runat="server" CssClass="ms-toolbar"
                        Text='<%#DataBinder.Eval(Container.DataItem, "Descrição") %>'
                        Width="400px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Atividade">
                <ItemTemplate>
                    <asp:Label ID="lblAtividade" runat="server" CssClass="ms-toolbar" Text='<%#DataBinder.Eval(Container.DataItem, "Atividade") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            
             <asp:TemplateColumn HeaderText="Recursos">
                <ItemTemplate>
                    <asp:Label ID="lblRecursos" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Recursos") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="CorDaData" Visible="False">
                <ItemTemplate>
                    <asp:Label ID="lblCorDaData" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CorDaData") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>

