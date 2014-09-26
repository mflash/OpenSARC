<%@ Page Language="C#" 
         AutoEventWireup="true" 
         CodeFile="Importar.aspx.cs" 
         Inherits="ImportarDados_Importar" 
         MasterPageFile="~/Master/Master.master"
		 Debug="true" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

<div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="IMPORTAÇÃO DE DADOS"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
            <table align="left">
                <tr>
                    <td style="width: 194px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 194px">
                        <asp:LinkButton ID="lbImportar" runat="server" CssClass="ms-toolbar" OnClick="lbImportar_Click" Width="98px" >Importar dados</asp:LinkButton></td>
                </tr>
                <tr>
                    <td style="width: 194px">
                        <asp:Label ID="lblStatus" runat="server" CssClass="ms-toolbar">test</asp:Label></td>
                </tr>
            </table>
    
</asp:Content>

