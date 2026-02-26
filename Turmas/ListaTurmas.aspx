<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="ListaTurmas.aspx.cs" Inherits="Pagina2" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../UserControls/SelecionaCalendario.ascx" TagName="SelecionaCalendario"
    TagPrefix="uc2" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    
    <!-- Título da página -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="h4 mb-0 fw-semibold text-primary">
            <i class="bi bi-list-ul me-2"></i>Lista de Turmas
        </h1>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <!-- Card container -->
            <div class="card shadow-sm border-0 mb-4">
                <div class="card-header bg-white border-bottom py-3">
                    <asp:CheckBox 
                        runat="server" 
                        ID="chkMostrarNotes" 
                        Text="&nbsp;Apenas turmas com notebooks" 
                        AutoPostBack="true" 
                        CssClass="form-check-label" />
                </div>
                
                <div class="card-body p-0">
                    <!-- GridView com Bootstrap -->
                    <div class="table-responsive">
                        <asp:GridView 
                            id="grvListaTurmas" 
                            runat="server" 
                            Width="100%" 
                            AutoGenerateColumns="False" 
                            OnRowDeleting="grvListaTurmas_RowDeleting" 
                            OnRowEditing="grvListaTurmas_RowEditing" 
                            DataKeyNames="Id" 
                            AllowSorting="True"
                            CssClass="table table-hover table-sm mb-0"
                            HeaderStyle-CssClass="table-light text-muted text-uppercase small fw-semibold"
                            RowStyle-CssClass="align-middle"
                            AlternatingRowStyle-CssClass="table-light align-middle"
                            GridLines="None">
                            
                            <Columns>
                                <asp:BoundField DataField="Id" Visible="False" HeaderText="ID" />
                                
                                <asp:BoundField 
                                    DataField="Numero" 
                                    HeaderText="Número"
                                    ItemStyle-CssClass="text-center fw-semibold"
                                    HeaderStyle-CssClass="text-center" />
                                
                                <asp:BoundField 
                                    DataField="Disciplina.CodCred" 
                                    HeaderText="CodCred"
                                    ItemStyle-CssClass="font-monospace small"
                                    HeaderStyle-Width="80px" />
                                
                                <asp:BoundField 
                                    DataField="Infra" 
                                    HeaderText="Infra"
                                    ItemStyle-CssClass="text-center fs-5"
                                    HeaderStyle-CssClass="text-center"
                                    HeaderStyle-Width="60px" />
                                
                                <asp:BoundField 
                                    DataField="Disciplina" 
                                    HeaderText="Disciplina"
                                    ItemStyle-CssClass="text-truncate"
                                    HeaderStyle-Width="280px" />
                                
                                <asp:BoundField 
                                    DataField="DataHora" 
                                    HeaderText="Data & Hora"
                                    ItemStyle-CssClass="small text-muted" />
                                
                                <asp:BoundField 
                                    DataField="Professor" 
                                    HeaderText="Professor"
                                    ItemStyle-CssClass="small" />
                                
                                <asp:BoundField 
                                    DataField="Curso" 
                                    HeaderText="Curso"
                                    ItemStyle-CssClass="small" />
                                
                                <asp:BoundField 
                                    DataField="Sala" 
                                    HeaderText="Sala" 
                                    SortExpression="Sala"
                                    ItemStyle-CssClass="font-monospace small fw-semibold text-primary"
                                    HeaderStyle-Width="100px" />
                                
                                <asp:TemplateField ShowHeader="False" HeaderText="Ações">
                                    <ItemStyle CssClass="text-end text-nowrap" Width="120px" />
                                    <HeaderStyle CssClass="text-end" Width="120px" />
                                    <ItemTemplate>
                                        <asp:LinkButton 
                                            ID="LinkButton1" 
                                            runat="server" 
                                            CausesValidation="False" 
                                            CommandName="Edit"
                                            CssClass="btn btn-sm btn-outline-primary me-1"
                                            ToolTip="Editar turma">
                                            <i class="bi bi-pencil-square"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton 
                                            ID="LinkButton2" 
                                            runat="server" 
                                            CausesValidation="False" 
                                            OnClientClick="return confirm_delete();" 
                                            CommandName="Delete"
                                            CssClass="btn btn-sm btn-outline-danger"
                                            ToolTip="Excluir turma">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            
            <!-- Mensagem de status -->
            <asp:Label 
                id="lblStatus" 
                runat="server" 
                CssClass="alert alert-info d-block" 
                Visible="False"
                role="alert">
            </asp:Label>
            
            <!-- Botão voltar -->
            <div class="mt-3">
                <asp:LinkButton 
                    id="LinkButton1" 
                    runat="server" 
                    CssClass="btn btn-outline-secondary"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

