<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="CadastrarAdmin.aspx.cs" Inherits="Default_CadastrarAdmin"  %>

<%@ Register Src="../UserControls/CreateUser.ascx" TagName="CreateUser" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <uc1:CreateUser ID="cuUsuarios" runat="server" OnLoad="cuUsuarios_Load1" />
    <br />
    <br />
    <br />
    <br />
    &nbsp;
</asp:Content>

