<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="Edit.aspx.cs" Inherits="CategoriaDisciplina_Edit" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle"
         Text="EDITAR CATEGORIA DE DISCIPLINAS">
         </asp:Label>
        </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>  
    <table align="left" style="width: 510px">
        <tr>
            <td colspan="2" style="height: 26px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
<table style="WIDTH: 510px">
<tbody>
<tr>
<td style="HEIGHT: 26px">
<table style="WIDTH: 510px" align="left">
<tbody>
<tr>
<td style="WIDTH: 20px; HEIGHT: 2px" class="ms-toolbar">Descrição:
</td>
<td style="WIDTH: 267px; HEIGHT: 2px">
<asp:TextBox id="txtDescricao" runat="server" CssClass="ms-toolbar" Width="223px">
</asp:TextBox>
<asp:RequiredFieldValidator id="rfvDescricao" runat="server" CssClass="ms-toolbar" 
ErrorMessage="Digite uma Descrição" ControlToValidate="txtDescricao">*
</asp:RequiredFieldValidator>
</td>
</tr>
    <tr>
        <td class="ms-toolbar" colspan="2" style="height: 2px">
            <asp:GridView ID="grvListaCatDisciplina" runat="server" AutoGenerateColumns="False"
                DataKeyNames="Id" Width="636px">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                    <asp:BoundField DataField="Descricao" HeaderText="Categoria Recurso">
                        <ControlStyle Width="200px" />
                        <ItemStyle CssClass="ms-toolbar" />
                        <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Prioridade">
                    <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Left" />
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server">
                            </asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtPrioridade" runat="server">
                            </asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                            ControlToValidate="txtPrioridade"
                                ErrorMessage="Valor deve estar entre 0 e 1" MaximumValue="1" 
                                MinimumValue="0"
                                Type="Double">*</asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtPrioridade"
                                ErrorMessage="Favor completar o campo Prioridade">*
                                </asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="cabecalhoTabela" />
                <AlternatingRowStyle CssClass="linhaAlternadaTabela" />
            </asp:GridView>
        </td>
    </tr>
</tbody>
</table>
</td>
<td style="WIDTH: 267px; HEIGHT: 26px">
</TD>
</TR>
    <tr>
        <td style="height: 26px" class="ms-toolbar">
            Observação:<br />
            <br />
            0 - somente recebe o recurso quando o número de recursos disponível for maior que
            o número de requisições.<br />
            0,5 - valor mediano.<br />
            &gt; 0,5 - têm prioridade sobre os recursos.</td>
        <td style="width: 267px; height: 26px">
        </td>
    </tr>
    <tr>
    <td style="HEIGHT: 26px"><asp:Button id="btnConfirmar" 
    onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" 
    Text="Confirmar" Width="79px" Height="20px" Font-Size="10px">
    </asp:Button>
    </td>
    <td style="WIDTH: 267px; HEIGHT: 26px">&nbsp;
    </td>
    </tr>
    <tr>
    <td style="HEIGHT: 1px" colspan="2">
    <asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" 
    OnClick="lbtnVoltar_Click">Voltar
    </asp:LinkButton>
    </td>
    </tr>
    <tr>
    <td style="HEIGHT: 29px" colspan="2">
    <asp:ValidationSummary id="vsSumario" runat="server" 
    CssClass="ms-toolbar" Width="147px" DisplayMode="List">
    </asp:ValidationSummary>
    </td>
    </tr>
    </tbody>
    </table>
</ContentTemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
