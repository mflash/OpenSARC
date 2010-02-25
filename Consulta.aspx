<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="Consulta.aspx.cs" Inherits="Alocacoes_Default" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BusinessData.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div align="left" style="width: 100%; height: 14px">
                <asp:Label ID="lblDataHora" runat="server" CssClass="ms-WPTitle" Text="Data: "></asp:Label></div>
           
            <table id="Table3">
                <tr>
                    <td style="width: 56px; height: 26px" class="ms-toolbar">
                        Data
                        </td>
                    <td style="height: 26px; width: 166px;">
                        <asp:TextBox ID="txtData" runat="server" CssClass="ms-toolbar"></asp:TextBox>
                        <asp:ImageButton ID="ibtnAbrirCalendario" runat="server" ImageUrl="~/_layouts/images/CALENDAR.GIF" />
                        <asp:Label ID="lblOpcional" runat="server" CssClass="ms-toolbar" Text="(Opcional)" ForeColor="Red" Visible="False"></asp:Label>
                        <cc1:CalendarExtender ID="ceData" 
                        runat="server" 
                        PopupButtonID="ibtnAbrirCalendario"
                        TargetControlID="txtData"
                        Format="dd/MM/yyyy"
                        >
                        </cc1:CalendarExtender>
                        &nbsp;
                    </td>
                </tr>
            </table>
                        <asp:Button ID="btnVisualizarAlocacoes" runat="server" OnClick="btnVisualizarAlocacoes_Click"
                            Text="Visualizar Alocações" CssClass="ms-toolbar" Width="170px" />
            <br />
            <br />
			
			<table><tr>
			<td>
            <div align=center><asp:Label ID="lblAtual" runat="server" CssClass="ms-pagecaption"></asp:Label></div>
			</td>
			<td>                     
            <asp:DataGrid ID="dgAlocacoes" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="100%" 
                     HorizontalAlign="Center"  
                     OnItemDataBound="dgAlocacoes_ItemDataBound"
                     Visible="False" >
        
            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                    <Columns>
                    
                        <asp:TemplateColumn HeaderText="Recurso">
                         <ItemTemplate>
                                <asp:Label Id="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                              
                    
                        <asp:TemplateColumn HeaderText="Disciplina/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblDisc" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Curso">
                            <ItemTemplate>
                                <asp:Label ID="lblCurso" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Responsavel">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsavel" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                              
                   </Columns>
                    </asp:DataGrid>
					<br />
			</td>
			</tr>
			<tr>
			<td>
            <div align=center><asp:Label ID="lblProximo" runat="server" CssClass="ms-pagecaption"></asp:Label></div>
			</td>
			<td>
            <asp:DataGrid ID="dgAlocacoes2" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="100%" 
                     HorizontalAlign="Center"  
                     OnItemDataBound="dgAlocacoes_ItemDataBound"
                     Visible="False" >
        
            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
                    <Columns>
                    
                        <asp:TemplateColumn HeaderText="Recurso">
                         <ItemTemplate>
                                <asp:Label Id="lblRecurso" runat="server" Text='<%# ((Recurso)Eval("Recurso")).Descricao%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                             
                    
                        <asp:TemplateColumn HeaderText="Disciplina/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblDisc" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Curso">
                            <ItemTemplate>
                                <asp:Label ID="lblCurso" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Responsavel">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsavel" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                              
                   </Columns>
                    </asp:DataGrid>
					</td></tr></table>
					<br/>										
                <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus"></asp:Label><br />                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

