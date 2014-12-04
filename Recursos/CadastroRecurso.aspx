<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="CadastroRecurso.aspx.cs" Inherits="Pagina6" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="~/Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>

<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="CADASTAR RECURSO"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table style="width: 510px" align="left">
        <tr>
            <td colspan="2" style="height: 16px; width: 517px;" valign="middle">
                &nbsp;<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <table style="width: 510px" align="left">
                    <tbody>
                        <tr>
                            <td style="WIDTH: 48px; HEIGHT: 16px" 
                                class="ms-toolbar">
                                Categoria:
                            </td>
                            <td style="WIDTH: 267px; HEIGHT: 16px">
                            <asp:DropDownList id="ddlCategoria" 
                                              runat="server" 
                                              CssClass="ms-toolbar" 
                                              Width="160px" 
                                              Height="20px">
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 48px; HEIGHT: 16px" class="ms-toolbar">
                            Descrição:
                            </td>
                            <td style="WIDTH: 267px; HEIGHT: 16px">
                            <asp:TextBox id="txtDescricao" 
                                         runat="server" 
                                         CssClass="ms-toobar" 
                                         Width="160px" 
                                         Height="20px" ValidationGroup="Confirmar"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                                        runat="server" 
                                                        ControlToValidate="txtDescricao"
                                                        CssClass="ms-toolbar" 
                                                        ErrorMessage="Digite uma descrição." ValidationGroup="Confirmar">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 48px; HEIGHT: 16px" class="ms-toolbar">
                            Faculdade:
                            </td>
                            <td style="WIDTH: 267px; HEIGHT: 16px">
                            <asp:DropDownList id="ddlVinculo" 
                                              runat="server" 
                                              CssClass="ms-toolbar" 
                                              Width="160px" 
                                              Height="20px">
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="WIDTH: 48px; HEIGHT: 16px" 
                                class="ms-toolbar">
                                Disponível:
                            </td>
                            <td style="WIDTH: 267px; HEIGHT: 16px">
                            <asp:RadioButtonList id="rblDisponivel" 
                                                 runat="server"
                                                 CssClass="teste" 
                                                 Width="160px" 
                                                 Height="5px" 
                                                 RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Selected="True">Sim</asp:ListItem>
                            <asp:ListItem Value="0">N&#227;o</asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="2">
                        <asp:LinkButton ID="lbCadastrarHorarios" 
                                        runat="server" 
                                        CssClass="ms-toolbar" 
                                        OnClick="lbCadastrarHorarios_Click">
                                        Cadastrar Horarios Bloqueados
                        </asp:LinkButton>
                        </td>
                        </tr>
                        <tr>
                        <td colspan="2">
                        <asp:Panel ID="pnlHorarios" runat="server" Visible="false">
                        <table style="width: 400px">
                        <tr>
                        <td class="ms-toolbar">
                        Horário De Início:
                        </td>
                        <td class="ms-toolbar">
                            &nbsp;
                            <asp:DropDownList ID="ddlHorarioInicio" runat="server" CssClass="ms-toolbar" AutoPostBack="True">
                                <asp:ListItem Value="1">AB</asp:ListItem>
                                <asp:ListItem Value="2">CD</asp:ListItem>
                                <asp:ListItem Value="3">EX</asp:ListItem>
                                <asp:ListItem Value="4">FG</asp:ListItem>
                                <asp:ListItem Value="5">HI</asp:ListItem>
                                <asp:ListItem Value="6">JK</asp:ListItem>
                                <asp:ListItem Value="7">LM</asp:ListItem>
                                <asp:ListItem Value="8">NP</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        <td>
                        </td>
                        </tr>
                        <tr>
                        <td class="ms-toolbar" style="height: 23px">Horário De Fim:
                        </td>
                        <td class="ms-toolbar" style="height: 23px">
                            &nbsp;
                            <asp:DropDownList ID="ddlHorarioFim" 
                                              runat="server" 
                                              CssClass="ms-toolbar" 
                                              AutoPostBack="True">
                                <asp:ListItem Value="1">AB</asp:ListItem>
                                <asp:ListItem Value="2">CD</asp:ListItem>
                                <asp:ListItem Value="3">EX</asp:ListItem>
                                <asp:ListItem Value="4">FG</asp:ListItem>
                                <asp:ListItem Value="5">HI</asp:ListItem>
                                <asp:ListItem Value="6">JK</asp:ListItem>
                                <asp:ListItem Value="7">LM</asp:ListItem>
                                <asp:ListItem Value="8">NP</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        <td style="height: 23px">
                        <asp:Button ID="btnAdicionar" 
                                    runat="server" 
                                    Text="Adicionar" 
                                    CssClass="ms-toolbar" OnClick="btnAdicionar_Click" >
                        </asp:Button>
                        </td>
                        </tr>
                        <tr>
                        <td class="ms-toolbar" colspan="3" style="height: 20px">
                        <asp:DataGrid ID="dgHorarios" 
                                      runat="server"       
                                      AutoGenerateColumns="False" 
                                      Width="100%" 
                                      HorizontalAlign="Center"
                                      OnItemCommand="dgHorarios_ItemCommand" >
                                      <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
                                      <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                                <Columns>
            
                                    <asp:TemplateColumn HeaderText="Hora Início"> 
                                        <ItemTemplate>
                                            <asp:Label ID="lblInicio" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HoraInicio") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Hora Fim"> 
                                        <ItemTemplate>
                                            <asp:Label ID="lblFim" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HoraFim") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:ButtonColumn CommandName="Excluir" Text="Excluir"></asp:ButtonColumn>
                                    
                                </Columns>
                        </asp:DataGrid>
                        </td>
                        </tr>
                        </table>
                        </asp:Panel>
                        </td>
                        </tr>
                        <tr>
                        <td style="width: 48px; height: 21px;">
                            <asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" Text="Confirmar" Width="79px" Height="20px" Font-Size="10px" ValidationGroup="Confirmar"></asp:Button>
                        </td>
                        <td style="height: 21px">
                        </td>
                        </tr>
                        <tr>
                        <td style="height: 20px;" colspan="2">
                            <asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False"></asp:Label></td>
                    </tr>
                        
                    </tbody>
                </table>
                    </ContentTemplate>
                        <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click" />
                        </Triggers> 
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
        <td>
        <asp:LinkButton ID="lbtnVoltar" runat="server" 
                        CssClass="ms-toolbar" 
                        OnClick="lbtnVoltar_Click">
                        Voltar
        </asp:LinkButton>    
        </td>
        </tr>
        <tr>
        <td>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
                    DisplayMode="List" Height="1px" Width="220px" ValidationGroup="Confirmar" />
            &nbsp;
        </td>
        </tr>
    </table>
    
</asp:Content>
