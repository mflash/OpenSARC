<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="AlteraRecurso.aspx.cs" Inherits="Recursos_AlteraRecurso" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<%-- Add content controls here --%>
<asp:Content ID="phTitulo" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="Label6" runat="server" CssClass="ms-WPTitle" Text="EDITAR RECURSO"></asp:Label>
    </div>
               <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                &nbsp;<uc1:Aguarde ID="Aguarde2" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    
    <div align = "left">
    <table>
        <tr>
            <td style="width: 151px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
    
    <table style="width: 510px" align="left">
        <tr>
            <td style="width: 36px; height: 21px" class="ms-toolbar">
                Categoria:</td>
            <td style="width: 267px; height: 21px">
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="ms-toolbar" Height="20px" Width="200px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 36px; height: 26px;" class="ms-toolbar">
                Descrição:</td>
            <td style="width: 267px; height: 26px;">
                <asp:TextBox ID="txtDescricao" runat="server" Width="200px" CssClass="ms-toolbar" Height="20px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescricao"
                    CssClass="ms-toolbar" ErrorMessage="Digite uma descrição.">*</asp:RequiredFieldValidator></td>
        </tr>
        <tr style="color: #003399">
            <td style="width: 36px; height: 25px" class="ms-toolbar">
                Faculdade:</td>
            <td style="width: 267px; height: 25px">
                <asp:DropDownList ID="ddlVinculo" runat="server" CssClass="ms-toolbar" Height="20px" Width="200px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 36px;" class="ms-toolbar">
                Disponível:</td>
            <td style="width: 267px; vertical-align:middle ">
                <asp:RadioButtonList ID="rblDisponivel" runat="server" CssClass="teste" RepeatDirection="Horizontal" Width="200px">
                    <asp:ListItem>Sim</asp:ListItem>
                    <asp:ListItem>N&#227;o</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td class="ms-toolbar" colspan="2">
                <table style="width: 400px">
                    <tr>
                        <td class="ms-toolbar">
                            Horário De Início:
                        </td>
                        <td class="ms-toolbar">
                            &nbsp;
                            <asp:DropDownList ID="ddlHorarioInicio" runat="server" AutoPostBack="True" CssClass="ms-toolbar">
                                <asp:ListItem Value="1">AB</asp:ListItem>
                                <asp:ListItem Value="2">CD</asp:ListItem>
                                <asp:ListItem Value="3">E</asp:ListItem>
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
                        <td class="ms-toolbar" style="height: 23px">
                            Horário De Fim:
                        </td>
                        <td class="ms-toolbar" style="height: 23px">
                            &nbsp;
                            <asp:DropDownList ID="ddlHorarioFim" runat="server" AutoPostBack="True" CssClass="ms-toolbar">
                                <asp:ListItem Value="1">AB</asp:ListItem>
                                <asp:ListItem Value="2">CD</asp:ListItem>
                                <asp:ListItem Value="3">E</asp:ListItem>
                                <asp:ListItem Value="4">FG</asp:ListItem>
                                <asp:ListItem Value="5">HI</asp:ListItem>
                                <asp:ListItem Value="6">JK</asp:ListItem>
                                <asp:ListItem Value="7">LM</asp:ListItem>
                                <asp:ListItem Value="8">NP</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="height: 23px">
                            <asp:Button ID="btnAdicionar" runat="server" CssClass="ms-toolbar" OnClick="btnAdicionar_Click"
                                Text="Adicionar" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-toolbar" colspan="3" style="height: 20px">
                            <asp:DataGrid ID="dgHorarios" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                OnItemCommand="dgHorarios_ItemCommand" Width="100%">
                                <ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center" />
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
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 26px">
                    &nbsp;<table style="WIDTH: 510px" align="left">
                <tbody>
                <tr>
                <td style="WIDTH: 19px; HEIGHT: 26px">
                <asp:Button id="btnConfirmar" onclick="btnConfirmar_Click" runat="server" CssClass="ms-toolbar" 
                Text="Confirmar" Width="75px" Height="20px">
                </asp:Button></td>
                <td style="WIDTH: 267px; HEIGHT: 26px">&nbsp;
                </td>
                </tr>
                <tr>
                <td style="HEIGHT: 26px" colspan="2">
                <asp:Label id="lblStatus" runat="server" CssClass="lblstatus" Visible="False">Label
                </asp:Label>
                </td>
                </tr>
                </tbody>
                </table>
            </td>
        </tr>
        
    </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 151px">
    <table>
    <tr>
    <td>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar"
                    DisplayMode="List" Height="1px" Width="208px" />
                <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" 
                OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
    </td>
    </tr>
    </table>
            </td>
        </tr>
    </table>
    </div>
    
    

</asp:Content>