<%@ page language="C#" debug="true" masterpagefile="~/Master/Master.master" autoeventwireup="true" Inherits="Docentes_SelecionaTurma" CodeFile="SelecionaTurma.aspx.cs" 
 title="Sistema de Alocação de Recursos Computacionais - FACIN" %>
         
<%@ Import Namespace="BusinessData.Entities" %>

    <asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
        <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="TURMAS/EVENTOS"></asp:Label>
    </div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <br />
        <div align="center"> 
            <asp:Label Visible="False" CssClass="ms-toolbar" ID="lblRotulo" runat="server" Text="Foram feitas propostas de troca de recursos para as seguintes aulas:">
            </asp:Label><br />
            <br />
            <asp:DataGrid ID="dgTroca" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="100%" 
                     HorizontalAlign="Center"  
                     DataKeyField="Id"
                     OnItemCommand="dgTroca_ItemCommand"
                     OnItemDataBound="dgTroca_ItemDataBound"
                     Visible="False" >
        
            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
            <Columns>
                
                <asp:TemplateColumn HeaderText="TrocaId" Visible="False"> 
                    <ItemTemplate>
                        <asp:Label ID="lblTrocaId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                           
                <asp:TemplateColumn HeaderText="Turma/Evento" > 
                    <ItemTemplate>
                        <asp:Label ID="lblTurmaEvento" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Data" > 
                    <ItemTemplate>
                        <asp:Label ID="lblData" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Horário" > 
                    <ItemTemplate>
                        <asp:Label ID="lblHorario" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Recurso Proposto" > 
                    <ItemTemplate>
                        <asp:Label ID="lblRecProposto" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Recurso Requerido" > 
                    <ItemTemplate>
                        <asp:Label ID="lblRecOferecido" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Responsável" > 
                    <ItemTemplate>
                        <asp:Label ID="lblResponsavel" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                                
                <asp:ButtonColumn CommandName="Aceitou" Text="Aceitar"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="Recusou" Text="Recusar"></asp:ButtonColumn>
                
            </Columns>
            </asp:DataGrid>
            <asp:Literal runat="server" id="htmlMOTD">
            </asp:Literal>	
            <asp:Literal runat="server" ID="htmlAniver">
            </asp:Literal>
        </div>
        <br />
        <div align="center">
        <asp:Label Visible="False" CssClass="ms-toolbar" ID="lblTransfencia" runat="server" Text="O seguintes recursos foram tranferidos para você:"></asp:Label>
        <asp:DataGrid ID="dgTransferencias" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="100%" 
                     HorizontalAlign="Center"  
                     DataKeyField="Id"
                     OnItemCommand="dgTransferencias_ItemCommand"
                     OnItemDataBound="dgTransferencias_ItemDataBound"
                     Visible="False" >
        
            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
            <Columns>
                
                <asp:TemplateColumn Visible="False"> 
                    <ItemTemplate>
                        <asp:Label ID="lblTransId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                           
                <asp:TemplateColumn HeaderText="Autor" > 
                    <ItemTemplate>
                        <asp:Label ID="lblAutor" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Recurso Recebido" > 
                    <ItemTemplate>
                        <asp:Label ID="lblRecurso" runat="server" Text='<%#((Recurso)Eval("Recurso")).Descricao %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Turma/Evento" > 
                    <ItemTemplate>
                        <asp:Label ID="lblTurmaEvento" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Data" > 
                    <ItemTemplate>
                        <asp:Label ID="lblData" runat="server" Text='<%#((DateTime)Eval("Data")).ToShortDateString() %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
               
                <asp:ButtonColumn CommandName="Viu" Text="OK"></asp:ButtonColumn>
                
            </Columns>
            </asp:DataGrid>
        
        </div>
        <br />
        <div align="left">
        <table>
            <tr>
                <td>
                    Minhas Turmas:    
                </td>
            </tr>
           
        </table>
        </div>
        <div align="center">
                <asp:Label ID="lblTurmas" runat="server" Visible="False" 
                    Text="Nenhuma turma cadastrada."></asp:Label>
            </div>
        <br />
        <div align="center">
        <asp:GridView ID="grvListaTurmas" runat="server"     
        AutoGenerateColumns="False" Width="636px" AllowSorting="True" 
        DataKeyNames="Id"      
        OnRowEditing="grvListaTurmas_RowEditing" >
          <Columns>
                <asp:BoundField HeaderText="Id" DataField="Id" Visible="false">
                </asp:BoundField>                                
                
                <asp:BoundField HeaderText="CodCred" DataField="Disciplina.CodCred">
                <ControlStyle Width="80px"></ControlStyle>
                <ItemStyle CssClass="ms-toolbar"></ItemStyle>
                <HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
                </asp:BoundField>

                <asp:BoundField HeaderText="Disciplina" DataField="Disciplina">
                <ControlStyle Width="120px"></ControlStyle>
                <ItemStyle CssClass="ms-toolbar"></ItemStyle>
                <HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Turma" DataField="Numero">
                <ControlStyle Width="100px"></ControlStyle>
                <ItemStyle CssClass="ms-toolbar"></ItemStyle>
                <HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
                </asp:BoundField>

                <asp:BoundField HeaderText="Curso" DataField="Curso" >
                <ControlStyle Width="100px"></ControlStyle>
                <ItemStyle CssClass="ms-toolbar"></ItemStyle>
                <HeaderStyle CssClass="ms-wikieditthird"></HeaderStyle>
                </asp:BoundField>
                
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Alterar"></asp:LinkButton>
                    </EditItemTemplate>
                    
                <ControlStyle CssClass="ms-toolbar" />
                <ItemStyle CssClass="ms-wikieditthird" />
                <HeaderStyle BorderStyle="None" CssClass="ms-wikieditthird" />
                    <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Selecionar"></asp:LinkButton> 
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            		
        </asp:GridView>
            <br />
			<p><strong>Obs: Conforme comunicado, serão criadas (vazias) as áreas Moodle para todas as disciplinas da Escola.<br/>
            Se houver algum erro ou inconsistência nas suas disciplinas, entre em contato com recursos.politecnica@pucrs.br</strong></p>
			<asp:Button Visible="false" cssclass="ms-toolbar" id="butMoodle" runat="server" OnClick="butMoodle_Click" Text="Clique aqui para solicitar a criação de áreas Moodle para as suas turmas"/>
            <br />
            
            <div align="left">
            
            <table>
                <tr>
                    <td>
                        Meus Eventos:
                    </td>
                </tr>
               
            </table>
            
            </div>
            <div align="center">
                <asp:Label ID="lblEventos" runat="server" Visible="False" 
                    Text="Nenhum evento cadastrado."></asp:Label>
                <br />
            </div>
            
         <asp:DataGrid ID="dgEventos" 
                     runat="server"       
                     AutoGenerateColumns="False" 
                     Width="66%" 
                     HorizontalAlign="Center"
                     OnItemCommand="dgEventos_ItemCommand"  >
        
            <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
            <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
            <Columns>
                
                <asp:TemplateColumn Visible="False"> 
                    <ItemTemplate>
                        <asp:Label ID="lblEventoId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EventoId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                           
                <asp:TemplateColumn HeaderText="Título" > 
                    <ItemTemplate>
                        <asp:Label ID="lblTitulo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Titulo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Unidade" > 
                    <ItemTemplate>
                        <asp:Label ID="lblUnidade" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Unidade") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                
                <asp:ButtonColumn CommandName="Horarios" Text="Selecionar"></asp:ButtonColumn>
                
            </Columns>
            </asp:DataGrid>
            
            </div>
            <br />
            <div align="center" >
            </div>
            
            </ContentTemplate>
            </asp:UpdatePanel>
    </asp:Content>
