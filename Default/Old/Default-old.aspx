<%-- $Id$ --%>

<%@ Page Language="C#" MasterPageFile="~/Master/Login.master" AutoEventWireup="true"
    CodeFile="Default-old.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="BusinessData.Entities" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="center" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick">
        </asp:Timer>
    </div>
    <table style="width: 100%">
        <tr>
            <td align="center" class="ms-toolbar" style="text-align: center;" valign="top">
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Login ID="loginEntrada" runat="server" CssClass="ms-toolbar" FailureText="Usuário inválido ou senha inválida."
                                Font-Bold="False" LoginButtonText="Entrar" PasswordLabelText="Senha:" PasswordRequiredErrorMessage="Senha não pode ser nula."
                                RememberMeText="Lembrar de mim da próxima vez." UserNameLabelText="Usuário:"
                                UserNameRequiredErrorMessage="Informe usuário." Width="862px" DestinationPageUrl="~/Default/SelecionarCalendario.aspx"
                                OnLoginError="loginEntrada_LoginError" TitleText="Informe seu usuário e senha no OpenSARC:"
                                AccessKey="M" BorderPadding="3" EnableTheming="True" Orientation="Horizontal"
                                OnAuthenticate="loginEntrada_Authenticate" PasswordRecoveryIconUrl="~/_layouts/images/attention16by16.gif" PasswordRecoveryUrl="~/Default/ResetSenha.aspx" PasswordRecoveryText="Esqueci">
                                <TitleTextStyle CssClass="ms-toolbar" Font-Bold="False" Font-Names="Verdana" Font-Size="12px" />
                                <CheckBoxStyle CssClass="ms-toolbar" />
                                <HyperLinkStyle CssClass="ms-toolbar" Font-Bold="True" ForeColor="Red" />
                                <InstructionTextStyle CssClass="ms-toolbar" />
                                <TextBoxStyle CssClass="ms-toolbar" Width="200px" />
                                <LoginButtonStyle CssClass="ms-toolbar" />
                                <LabelStyle CssClass="ms-toolbar" />
                            </asp:Login>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblDataHora" runat="server" CssClass="ms-blogrss"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAtual" runat="server" CssClass="ms-bigcaption"></asp:Label>
                        </td>
                        <td>
                            <asp:DataGrid ID="dgAlocacoes" runat="server" AutoGenerateColumns="False" Width="100%"
                                HorizontalAlign="Center" OnItemDataBound="dgAlocacoes_ItemDataBound" CellPadding="4"
                                Visible="False">
                                <ItemStyle CssClass="ms-btoolbar" HorizontalAlign="Center" />
                                <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemTemplate>
                                            <asp:Image ID="imgIcon" runat="server" ImageUrl="~/SRRC/img/lab.png" AlternateText="Ícone" Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Recurso" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Disciplina/Evento" ItemStyle-Width="400px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDisc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Responsável" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResponsavel" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Estado" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblProximo" runat="server" CssClass="ms-bigcaption"></asp:Label>
                        </td>
                        <td>
                            <asp:DataGrid ID="dgAlocacoes2" runat="server" AutoGenerateColumns="False" Width="100%"
                                HorizontalAlign="Center" OnItemDataBound="dgAlocacoes_ItemDataBound" CellPadding="4"
                                Visible="False">
                                <ItemStyle CssClass="ms-btoolbar" HorizontalAlign="Center" />
                                <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="">
                                        <ItemTemplate>
                                            <asp:Image ID="imgIcon" runat="server" ImageUrl="~/SRRC/img/lab.png" AlternateText="Ícone" Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Recurso" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Disciplina/Evento" ItemStyle-Width="400px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDisc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Responsável" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResponsavel" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Estado" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblStatus" runat="server" CssClass="ms-blogrss"></asp:Label>
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <div align="center">
        <p style="padding: 4px; margin: 4px; text-align: center">
            O OpenSARC é um sistema para alocação de recursos computacionais. Além da solicitação
            de recursos durante o período de planejamento semestral,<br />
            o sistema permite agendar eventos, consultar datas de avaliações e trocar e transferir
            recursos durante todo o ano.<br />
            <br />
            O OpenSARC é <i>software</i> livre. Caso deseje participar, reclamar ou dar sugestões,
            visite <a href="https://github.com/mflash/OpenSARC">https://github.com/mflash/OpenSARC</a>.<br />
            <br />
            Em especial, aguardamos voluntários interessados em utilizar o sistema como estudo
            de caso para suas disciplinas de desenvolvimento de <i>software</i>.</p>
    </div>
</asp:Content>
