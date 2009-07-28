<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
         CodeFile="ListaEvento.aspx.cs" Inherits="Secretarios_ListaEvento" 
         Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>
         
<%@ Import Namespace="BusinessData.Entities" %>

    <asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
        <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EVENTOS"></asp:Label>
    </div>
        
        <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
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
                           
                <asp:TemplateColumn HeaderText="Evento" > 
                    <ItemTemplate>
                        <asp:Label ID="lblEvento" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Data" > 
                    <ItemTemplate>
                        <asp:Label ID="lblData" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn HeaderText="Horario" > 
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
                
                <asp:TemplateColumn HeaderText="Autor" > 
                    <ItemTemplate>
                        <asp:Label ID="lblAutor" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                
                
                <asp:ButtonColumn CommandName="Aceitou" Text="Aceitar"></asp:ButtonColumn>
                <asp:ButtonColumn CommandName="Recusou" Text="Recusar"></asp:ButtonColumn>
                
            </Columns>
            </asp:DataGrid>
        </div>
        <br />
        <div align="center">
        <asp:Label Visible="False" CssClass="ms-toolbar" ID="lblTransfencia" runat="server" Text="O seguintes recursos foram tranferidos para você:"></asp:Label>
        <br />
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
                
                <asp:TemplateColumn HeaderText="Evento" > 
                    <ItemTemplate>
                        <asp:Label ID="lblEvento" runat="server" Text='<%#((Evento)Eval("EventoRecebeu")).Titulo %>'></asp:Label>
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
        <div align="left">
            </div>
        <div align="center">
            <div align="left">
            
            <table>
                <tr>
                    <td>
                        Eventos Ministrados:
                    </td>
                </tr>
                <tr>
                    <td class="ms-toolbar" >
                        Selecione um evento para ver os detalhes:</td>
                </tr>
            </table>
            </div>
            <br />
            <div>
                <asp:Label ID="lblEventos" runat="server" Visible="false" Text="Você não tem eventos cadastrados."></asp:Label>
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
                           
                <asp:TemplateColumn HeaderText="Titulo" > 
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
