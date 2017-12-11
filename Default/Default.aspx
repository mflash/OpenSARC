<%-- $Id$ --%>

<%@ Page Language="C#" MasterPageFile="~/Master/Login.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
                            <asp:Login ID="loginEntrada" runat="server" CssClass="ms-toolbar" FailureText="Usu�rio inv�lido ou senha inv�lida."
                                Font-Bold="False" LoginButtonText="Entrar" PasswordLabelText="Senha:" PasswordRequiredErrorMessage="Senha n�o pode ser nula."
                                RememberMeText="Lembrar de mim da pr�xima vez." UserNameLabelText="Usu�rio:"
                                UserNameRequiredErrorMessage="Informe usu�rio." Width="852px" DestinationPageUrl="~/Default/SelecionarCalendario.aspx"
                                OnLoginError="loginEntrada_LoginError" TitleText="Informe seu usu�rio e senha no OpenSARC:"
                                AccessKey="M" BorderPadding="3" EnableTheming="True" Orientation="Horizontal"
                                OnAuthenticate="loginEntrada_Authenticate">
                                <TitleTextStyle CssClass="ms-toolbar" Font-Bold="False" Font-Names="Verdana" Font-Size="12px" />
                                <CheckBoxStyle CssClass="ms-toolbar" />
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
                                    <asp:TemplateColumn HeaderText="Recurso">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Disciplina/Evento">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDisc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Respons�vel">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResponsavel" runat="server"></asp:Label>
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
                                    <asp:TemplateColumn HeaderText="Recurso">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Disciplina/Evento">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDisc" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Respons�vel">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResponsavel" runat="server"></asp:Label>
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
            O OpenSARC � um sistema para aloca��o de recursos computacionais. Al�m da solicita��o
            de recursos durante o per�odo de planejamento semestral,<br />
            o sistema permite agendar eventos, consultar datas de avalia��es e trocar e transferir
            recursos durante todo o ano.<br />
            <br />
            O OpenSARC � <i>software</i> livre. Caso deseje participar, reclamar ou dar sugest�es,
            visite <a href="https://github.com/mflash/OpenSARC">https://github.com/mflash/OpenSARC</a>.<br />
            <br />
            Em especial, aguardamos volunt�rios interessados em utilizar o sistema como estudo
            de caso para suas disciplinas de desenvolvimento de <i>software</i>.</p>
    </div>
</asp:Content>
