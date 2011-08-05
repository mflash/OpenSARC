<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="PersonificarProfessor.aspx.cs" Inherits="Secretarios_PersonificarProfessor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    
    
    Professor:
    <asp:DropDownList ID="DropDownList1" runat="server" 
        DataSourceID="ODSProfessores" DataTextField="Nome" DataValueField="Matricula" 
        Height="26px" Width="400px">
    </asp:DropDownList>
    <asp:ObjectDataSource ID="ODSProfessores" runat="server" 
        SelectMethod="GetProfessores" 
        TypeName="BusinessData.BusinessLogic.ProfessoresBO"></asp:ObjectDataSource>
    <br />
    <asp:Button ID="btnPersonificar" runat="server" onclick="Button1_Click" Text="Personificar Professor" />
</asp:Content>

