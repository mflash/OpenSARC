<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferirRecurso.aspx.cs" Inherits="AcessoProfessores_TransferirRecurso" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Import Namespace="BusinessData.Entities" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Sistema de Alocação de Recursos</title>
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>

<body onunload="opener.location= opener.location;">

<script  type="text/javascript" language="javascript">
function confirm_trans()
{
  if (confirm("Tem certeza que deseja transferir este recurso?")==true)
    return true;
  else
    return false;
}
</script>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:Panel ID="pnlgeral" runat="server" Width="350" Height="300px" ScrollBars="Auto">
                <table>
                    <tr>
                        <td >
                        <asp:Label ID="lblDataHorario" runat="server" CssClass="ms-toolbar"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Label ID="lblRectrans" runat="server" CssClass="ms-toolbar" Text="Recursos para transferência:">
                        </asp:Label>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblRecursos" runat="server" BackColor="White" BorderColor="White"
                                CssClass="ms-toolbar">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Label ID="lblResponsavel" runat="server" CssClass="ms-toolbar" Text="Professores disponíveis:">
                        </asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:DropDownList AutoPostBack="true" ID="ddlResponsavel" runat="server" CssClass="ms-toolbar" OnSelectedIndexChanged="ddlResponsavel_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                    <td >
                    <asp:Label ID="lblTurmas" Visible="false" runat="server" CssClass="ms-toolbar" Text="Turmas disponíveis:">
                        </asp:Label>
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgTurmas" runat="server" 
                                          AutoGenerateColumns="false"
                                          OnItemDataBound="dgTurmas_ItemDataBound"
                                          OnItemCommand="dgTurmas_ItemCommand"
                                          Visible="false">
                            
                            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
                            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                            <Columns>
                            
                            <asp:TemplateColumn HeaderText="Curso" > 
                                <ItemTemplate>
                                    <asp:Label ID="lblCurso" runat="server"  Text='<%#((Curso)Eval("Curso")).Nome %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn HeaderText="Disciplina" >
                                <ItemTemplate>
                                    <asp:Label ID="lblDisciplina" runat="server" Text='<% #((Disciplina)Eval("Disciplina")).Nome %>' >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn Visible="false"> 
                                <ItemTemplate>
                                    <asp:Label ID="lblTurmaId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>' >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn HeaderText="Turma" > 
                                <ItemTemplate>
                                    <asp:Label ID="lblTurma" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem, "Numero") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn  Visible="false"> 
                                <ItemTemplate>
                                    <asp:Label ID="lblAulaId" runat="server"  >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:ButtonColumn CommandName="Transferir" Text="Transferir"/>
                            
                            
                            </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                    <td >
                    <asp:Label ID="lblEventos" Visible="false" runat="server" CssClass="ms-toolbar" Text="Eventos disponíveis:">
                        </asp:Label>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:DataGrid ID="dgEventos" runat="server" 
                                          AutoGenerateColumns="false"
                                          OnItemCommand="dgEventos_ItemCommand"
                                          Visible="false">
                            
                            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
                            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                            <Columns>
                            
                            <asp:TemplateColumn HeaderText="Título" > 
                                <ItemTemplate>
                                    <asp:Label ID="lblTitulo" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem, "Titulo") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn HeaderText="Unidade" >
                                <ItemTemplate>
                                    <asp:Label ID="lblDisciplina" runat="server" Text='<% #DataBinder.Eval(Container.DataItem, "Unidade") %>' >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn Visible="false"> 
                                <ItemTemplate>
                                    <asp:Label ID="lblEventoId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EventoId") %>' >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn > 
                                <ItemTemplate>
                                    <asp:Label ID="lblResponsavel" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem, "Responsavel") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:ButtonColumn CommandName="Transferir" Text="Transferir"/>
                            
                            
                            </Columns>
                            </asp:DataGrid>
                    
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus">
                            </asp:Label>
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                               
                         <asp:Button ID="btnFinalizar" runat="server" CssClass="ms-toolbar" OnClick="btnFinalizar_Click"
                                Text="Finalizar" />
                         </td>
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
    
    </div>
    </form>
</body>
</html>
