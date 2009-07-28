<%@ Page MasterPageFile="~/Master/Master.master" 
            Language="C#" 
            AutoEventWireup="true" 
            CodeFile="SelecionarCalendario.aspx.cs" 
            Inherits="Default_SelecionarCalendario"
            Title="Sistema de Alocação de Recursos - FACIN"
             %>

<%@ Register Src="SelecionaCalendario.ascx" TagName="SelecionaCalendario"
 TagPrefix="uc2" %>

<%@ Register Src="MenuAdmin.ascx"  
            TagName="MenuAdmin" 
            TagPrefix="uc1" %>
    <asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
            <tr>
                <td align="center" class="ms-toolbar" style="font-size: 42px" valign="top">
                    Bem-Vindo</td>
            </tr>
            <tr>
                <td class="ms-toolbar"> Selecione o calendário que deseja utilizar:
                </td>
            </tr>
            <tr>
                <td class="ms-toolbar" style="height: 22px" align="center"> 
                    <uc2:SelecionaCalendario ID="selecionaCalendario" runat="server" EnableViewState="true" />
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Content>
