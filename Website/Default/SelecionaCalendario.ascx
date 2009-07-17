<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelecionaCalendario.ascx.cs" 
Inherits="Default_SelecionaCalendario_" %>
<asp:DropDownList ID="ddlCalendarios" runat="server" Width="300px" CssClass="ms-toolbar">
</asp:DropDownList>
<asp:Button ID="Button1" runat="server" Text="Selecionar" OnClick="Selecionar_Click" CssClass="ms-toolbar" CausesValidation="False" />
