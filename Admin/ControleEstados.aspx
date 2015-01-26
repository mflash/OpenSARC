<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ControleEstados.aspx.cs" Inherits="Admin_ControleEstados" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="Label1" runat="server" CssClass="ms-WPTitle" Text="CONFIGURAÇÕES DO SEMESTRE"></asp:Label></div>
    <div align="center" style="vertical-align:middle;">        
                <table style="width: 237px">
                    <tr>
                        <td style="width: 250px; height: 26px">
        <asp:Button ID="btnAbrirSolicitacaoRecursos" runat="server" Text="Abrir Solicitação de Recursos" OnClick="btnAbrirSolicitacaoRecursos_Click" Width="250px" /></td>
                        <td style="width: 355px; height: 26px">
        <asp:Button ID="btnFecharAcesso" runat="server" Text="Fechar Acesso de Professores" Width="250px" OnClick="btnFecharAcesso_Click" /></td>
                        <td style="width: 355px; height: 26px">
        <asp:Button ID="btnIniciarSemestre" runat="server" Text="Iniciar Semestre" Width="250px" OnClick="btnIniciarSemestre_Click" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 24px; text-align: center">
                            <asp:CheckBox ID="checkSimula" runat="server" CssClass="ms-WPTitle" Text="Apenas simulação" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; text-align:center" colspan="3">
                <asp:Label ID="lblResultado" runat="server" CssClass="lblstatus"></asp:Label>                           
                        </td>                        
                    </tr>
                </table>                                          
    </div>
        </asp:Content>
    