<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ListaAlunos.aspx.cs" Inherits="Alunos_ListaAlunos" Title="Untitled Page" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblListaAlunos" runat="server" CssClass="ms-WPTitle" Text="Lista de Alunos"></asp:Label></div>
    <table style="width: 514px; height: 71px">
        <tr>
            <td style="width: 258px">
                <asp:GridView ID="grvListaAlunos" runat="server" AutoGenerateColumns="False"
                    Width="636px">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Matr&#237;cula">
                            <ControlStyle Width="100px" />
                            <ItemStyle CssClass="ms-toolbar" />
                            <HeaderStyle CssClass="ms-wikieditthird" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Categoria" HeaderText="Nome">
                            <ControlStyle Width="150px" />
                            <ItemStyle CssClass="ms-toolbar" />
                            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="E-mail">
                            <ControlStyle Width="200px" />
                            <ItemStyle CssClass="ms-toolbar" />
                            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:CommandField CancelText="Cancelar" DeleteText="Excluir" EditText="Editar" InsertText="Inserir"
                            NewText="Novo" SelectText="Selecionar" ShowDeleteButton="True" ShowEditButton="True"
                            UpdateText="Alterar">
                            <HeaderStyle BorderStyle="None" CssClass="ms-wikieditthird" />
                            <ItemStyle CssClass="ms-wikieditthird" />
                            <ControlStyle CssClass="ms-toolbar" />
                        </asp:CommandField>
                    </Columns>
                    <HeaderStyle CssClass="cabecalhoTabela" />
                    <AlternatingRowStyle CssClass="linhaAlternadaTabela" />
                </asp:GridView>
            </td>
            <td style="width: 5px">
            </td>
        </tr>
        <tr>
            <td colspan="1" rowspan="1" style="width: 258px">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ms-toolbar">Voltar</asp:LinkButton>
            </td>
            <td style="width: 5px">
            </td>
        </tr>
    </table>
</asp:Content>
