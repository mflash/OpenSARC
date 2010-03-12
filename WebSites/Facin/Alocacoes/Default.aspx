<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Alocacoes_Default" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BusinessData.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div align="left" style="width: 100%; height: 14px">
                <asp:Label ID="lblDisciplina" runat="server" CssClass="ms-WPTitle" Text="VISUALIZAR RECURSOS ALOCADOS POR:"></asp:Label></div>
            <asp:RadioButtonList ID="rblAlocacoes" runat="server" OnSelectedIndexChanged="rblAlocacoes_SelectedIndexChanged" AutoPostBack="True" BackColor="White" BorderColor="White" CssClass="ms-toolbar">
                <asp:ListItem Selected="True" Value="Data">Data</asp:ListItem>
				<asp:ListItem Value="Categoria">Categoria recurso + horário</asp:ListItem>
                <asp:ListItem Value="Recurso">Recurso</asp:ListItem>
                <asp:ListItem Value="Professor">Professor</asp:ListItem>
                <asp:ListItem>Secret&#225;rio</asp:ListItem>
            </asp:RadioButtonList>
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
            <asp:Panel ID="pnlVisualizarPorRecurso" runat="server" Height="50px"
                Width="125px" Visible="False">
            <table id="Table2">
                <tr>
                    <td style="width: 88px; height: 26px" class="ms-toolbar">
                        Categoria</td>
                    <td style="height: 26px; width: 173px;">
                        <asp:DropDownList ID="ddlCategorias" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged" CssClass="ms-toolbar" Width="170px">
                            <asp:ListItem>Selecione</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 88px; height: 26px" class="ms-toolbar">
                        Recurso</td>
                    <td style="height: 26px; width: 173px;">
                        <asp:DropDownList ID="ddlRecursos" runat="server" AppendDataBoundItems="True" CssClass="ms-toolbar" Width="170px">
                            <asp:ListItem>Selecione</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                </tr>
            </table>			
                </asp:Panel>
			<asp:Panel ID="pnlVisualizarPorCategoria" runat="server" Height="50px"
                Width="125px" Visible="False">
            <table id="Table2">
                <tr>
                    <td style="width: 88px; height: 26px" class="ms-toolbar">
                        Categoria</td>
                    <td style="height: 26px; width: 173px;">
                        <asp:DropDownList ID="ddlCategorias2" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCategorias2_SelectedIndexChanged" CssClass="ms-toolbar" Width="170px">
                            <asp:ListItem>Selecione</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 88px; height: 26px" class="ms-toolbar">
                        Horário</td>
                    <td style="height: 26px; width: 173px;">
                        <asp:DropDownList ID="ddlHorarios" runat="server" AppendDataBoundItems="True" CssClass="ms-toolbar" Width="170px">
                            <asp:ListItem>Selecione</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                </tr>
            </table>			
                </asp:Panel>
				<asp:Panel ID="pnlVisualizarPorProfessor" runat="server" Height="50px"
                Width="125px" Visible="False">
                    <table id="Table1">
                        <tr>
                            <td style="width: 84px; height: 26px" class="ms-toolbar">
                                Professor</td>
                            <td style="height: 26px">
                                <asp:DropDownList ID="ddlProfessores" runat="server" AppendDataBoundItems="True" CssClass="ms-toolbar" Width="170px">
                                    <asp:ListItem>Selecione</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                        </tr>
                    </table>
                </asp:Panel>
            <asp:Panel ID="pnlVisualizarPorSecretario" runat="server" Height="50px"
                Width="125px" Visible="False">
                <table id="Table4">
                    <tr>
                        <td style="width: 84px; height: 26px" class="ms-toolbar">
                            Secretário</td>
                        <td style="height: 26px">
                            <asp:DropDownList ID="ddlSecretario" runat="server" AppendDataBoundItems="True" CssClass="ms-toolbar" Width="170px">
                                <asp:ListItem>Selecione</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
                        <asp:Button ID="btnVisualizarAlocacoes" runat="server" OnClick="btnVisualizarAlocacoes_Click"
                            Text="Visualizar Alocações" CssClass="ms-toolbar" Width="170px" /><br />
            <br />
                
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
                        
                        <asp:TemplateColumn HeaderText="Data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" Text='<%# ((DateTime)Eval("Data")).ToShortDateString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:BoundColumn DataField="Horario" HeaderText="Horario" />
                        
                        <asp:TemplateColumn HeaderText="DisciplinaCod">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscCod" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    
                        <asp:TemplateColumn HeaderText="Disciplina">
                            <ItemTemplate>
                                <asp:Label ID="lblDisc" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Turma/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblTurmaEvento" runat="server" ></asp:Label>
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
                <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus"></asp:Label><br />
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

