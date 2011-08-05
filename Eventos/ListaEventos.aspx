<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ListaEventos.aspx.cs" Inherits="Eventos_ListaEventos" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ import Namespace="BusinessData.Entities" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="LISTA DE EVENTOS"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<table align="left">
<tbody>
<tr>
<td > 
    <asp:Button ID="btnExportarHtml" runat="server" CssClass="ms-toolbar" OnClick="btnExportarHtml_Click"
        Text="Exportar HTML" /><br />
<asp:GridView id="grvListaEventos" 
runat="server" DataKeyNames="EventoId" OnRowEditing="grvListaEventos_RowEditing" Width="600px"
 AutoGenerateColumns="False" OnRowDeleting="grvListaEventos_RowDeleting" OnSelectedIndexChanged="grvListaEventos_SelectedIndexChanged">
 <Columns>
     
     <asp:TemplateField HeaderText="T&#237;tulo">
         <ItemTemplate>
             <asp:Label ID="lblTitulo" runat="server" Text='<%# Bind("Titulo") %>'></asp:Label>
         </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" />
         <HeaderStyle HorizontalAlign="Center" />
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Respons&#225;vel">
         <ItemTemplate>
             <asp:Label ID="lblResponsavel" runat="server" Text='<%# Bind("Responsavel") %>'></asp:Label>
         </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" />
         <HeaderStyle HorizontalAlign="Center" />
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Descri&#231;&#227;o">
         <ItemTemplate>
             <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
         </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" />
         <HeaderStyle HorizontalAlign="Center" />
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Unidade">
         <ItemTemplate>
             <asp:Label ID="lblUnidade" runat="server" Text='<%# Bind("Unidade") %>'></asp:Label>
         </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" />
         <HeaderStyle HorizontalAlign="Center" />
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Autor">
         <ItemTemplate>
             <asp:Label ID="lblAutor" runat="server" Text='<%#((PessoaBase)Eval("AutorId")).Nome %>'></asp:Label>
         </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" />
         <HeaderStyle HorizontalAlign="Center" />
     </asp:TemplateField>
     <asp:CommandField SelectText="Visualizar Datas" ShowSelectButton="True" >
         <ItemStyle HorizontalAlign="Center" />
     </asp:CommandField>
     
    <asp:TemplateField ShowHeader="False">
        <EditItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                Text="Update"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                Text="Cancelar"></asp:LinkButton>
        </EditItemTemplate>
        <ItemStyle CssClass="ms-wikieditthird" />
        <ItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                Text="Editar"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
            OnClientClick = "return confirm_delete();" CommandName="Delete"
                Text="Deletar"></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

<RowStyle CssClass="ms-toolbar">
</RowStyle>

<HeaderStyle CssClass="ms-wikieditthird">
</HeaderStyle>
</asp:GridView>
</td>
</tr>
    <tr>
        <td>
            <asp:Label ID="lblDatas" runat="server" Text="Datas do Evento:" Visible="False"></asp:Label></td>
    </tr>
    <tr>
        <td style="height: 20px">
            <asp:GridView ID="grdDatas" runat="server" AutoGenerateColumns="False" Width="229px" Visible="False">
                <Columns>
                    <asp:TemplateField HeaderText="Data">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# ((DateTime)Eval("Data")).ToShortDateString()%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hor&#225;rio de In&#237;cio">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("HorarioInicio") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hor&#225;rio de Fim">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("HorarioFim") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="ms-toolbar" />
                <HeaderStyle CssClass="ms-wikieditthird" />
            </asp:GridView>
        </td>
    </tr>
<tr>
<td>
<asp:Label id="lblStatus" runat="server" CssClass="lblstatus"></asp:Label>
</tr>
    <tr>
<td>
        <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
    </tr>
        </tbody>
</table>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
