<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrocarRecurso.aspx.cs"
 Inherits="Docentes_TrocarRecurso" 
 Title="Sistema de Alocação de Recursos - FACIN" %>
<%@ import Namespace="BusinessData.Entities" %>
<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="controlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sistema de Alocação de Recursos - FACIN</title>
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>
<body onunload="opener.location= opener.location;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:Panel ID="pnlGeral" runat="server" ScrollBars="Auto" Height="300px" Width="400px">
        <table >
        <tr>
        <td >   
                <asp:Label id="lblDataHorario" runat="server" CssClass="ms-toolbar"></asp:Label>
                </td>
        </tr>
            <tr>
                <td style="height: 21px" >
                <asp:Label id="lblStatus" runat="server" CssClass="lblstatus">
                </asp:Label>
                    </td>
            </tr>
        <tr>
                <td class="ms-toolbar">
                    Meus recursos disponíveis para troca:
                </td>
                </tr>
                <tr>
                <td>
                    <asp:RadioButtonList ID="rblRecursos" runat="server" BackColor="White" BorderColor="White"
                        CssClass="ms-toolbar">
                    </asp:RadioButtonList></td>
                </tr>       
         <tr>
                <td class="ms-toolbar" > Relação dos recursos alocados:
                
                </td>
                </tr>
            <tr>
                
                <td >
                <asp:DataGrid ID="dgRecusrosAlocados" 
                 runat="server"       
                 AutoGenerateColumns="False" 
                 Width="100%" 
                 HorizontalAlign="Center" 
                 OnItemDataBound="dgRecusrosAlocados_ItemDataBound"
                 OnItemCommand="dgRecusrosAlocados_ItemCommand" >
    
        <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
        <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
        <Columns>
        
                <asp:TemplateColumn HeaderText="Recurso"> 
                    <ItemTemplate>
                        <asp:Label ID="lblRecurso" runat="server"  Text='<%#((Recurso)Eval("Recurso")).Descricao %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="RecursoId" Visible="false"> 
                    <ItemTemplate>
                        <asp:Label ID="lblRecursoId" runat="server"  Text='<%#((Recurso)Eval("Recurso")).Id %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Turma/Evento" Visible="false"> 
                    <ItemTemplate>
                        <asp:Label ID="lblTurmaEventoId" runat="server" >
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Turma/Evento"> 
                    <ItemTemplate>
                        <asp:Label ID="lblTurmaEvento" runat="server" >
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Responsável"> 
                    <ItemTemplate>
                        <asp:Label ID="lblResponsavel" runat="server" >
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:ButtonColumn CommandName="Troca" Text="Propor Troca">
                </asp:ButtonColumn>
        </Columns>
        </asp:DataGrid>
                <asp:Label ID="lblDgRecursos" runat="server" CssClass="lblstatus" Visible="False"></asp:Label>
                </td>
                
            <tr>
                <td align="right">
                    <asp:Button ID="btnFinalizar" runat="server" CssClass="ms-toolbar" 
                    Text="Finalizar" OnClick="btnFinalizar_Click" /></td>
            </tr>
        </table>
        </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <uc1:Aguarde ID="Aguarde1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
