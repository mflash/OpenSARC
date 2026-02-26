<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="ListaEventosFuturos.aspx.cs" Inherits="Eventos_ListaEventosFuturos" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    
    <!-- SweetAlert2 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    
    <!-- Título da página -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="h4 mb-0 fw-semibold text-primary">
            <i class="bi bi-calendar-event me-2"></i>Lista de Eventos Futuros
        </h1>
        <asp:Button 
            ID="btnExportarHtml" 
            runat="server" 
            CssClass="btn btn-outline-success btn-sm btn-with-icon" 
            OnClick="btnExportarHtml_Click"
            Text="Exportar HTML">
        </asp:Button>
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <!-- Card principal com a lista de eventos -->
            <div class="card shadow-sm border-0 mb-4">
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView 
                            id="grvListaEventos" 
                            runat="server" 
                            DataKeyNames="EventoId" 
                            OnRowEditing="grvListaEventos_RowEditing" 
                            OnRowDeleting="grvListaEventos_RowDeleting" 
                            OnSelectedIndexChanged="grvListaEventos_SelectedIndexChanged"
                            Width="100%"
                            AutoGenerateColumns="False"
                            CssClass="table table-hover table-sm mb-0"
                            HeaderStyle-CssClass="table-light text-muted text-uppercase small fw-semibold"
                            RowStyle-CssClass="align-middle"
                            GridLines="None">
                            
                            <Columns>
                                
                                <asp:TemplateField HeaderText="Título">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitulo" runat="server" Text='<%# Bind("Titulo") %>' CssClass="fw-semibold"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-start" />
                                    <HeaderStyle CssClass="text-start" Width="200px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Responsável">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResponsavel" runat="server" Text='<%# Bind("Responsavel") %>' CssClass="small"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-start" />
                                    <HeaderStyle CssClass="text-start" Width="150px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Descrição">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("Descricao") %>' CssClass="small text-muted"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-start" />
                                    <HeaderStyle CssClass="text-start" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Unidade">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnidade" runat="server" Text='<%# Bind("Unidade") %>' CssClass="badge bg-secondary"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                    <HeaderStyle CssClass="text-center" Width="100px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Autor">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAutor" runat="server" Text='<%#((PessoaBase)Eval("AutorId")).Nome %>' CssClass="small"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-start" />
                                    <HeaderStyle CssClass="text-start" Width="150px" />
                                </asp:TemplateField>
                                   
                                <asp:TemplateField HeaderText="Datas">
                                    <ItemTemplate>
                                        <asp:LinkButton 
                                            ID="btnVisualizarDatas" 
                                            runat="server" 
                                            CommandName="Select"
                                            CssClass="btn btn-sm btn-outline-info"
                                            ToolTip="Visualizar datas do evento">
                                            <i class="bi bi-calendar-check me-1"></i>Datas
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                    <HeaderStyle CssClass="text-center" Width="120px" />
                                </asp:TemplateField>
                                
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
                                            ToolTip="Editar evento">
                                            <i class="bi bi-pencil-square"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton 
                                            ID="LinkButton2" 
                                            runat="server" 
                                            CausesValidation="False" 
                                            OnClientClick="return confirm_delete();" 
                                            CommandName="Delete"
                                            CssClass="btn btn-sm btn-outline-danger"
                                            ToolTip="Excluir evento">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            
            <!-- Card com as datas do evento (visível quando selecionado) -->
            <div class="card shadow-sm border-0 mb-4" id="cardDatas" runat="server" visible="false">
                <div class="card-header bg-light border-bottom">
                    <h6 class="mb-0 fw-semibold text-dark">
                        <i class="bi bi-calendar3 me-2"></i>
                        <asp:Label ID="lblDatas" runat="server" Text="Datas do Evento"></asp:Label>
                    </h6>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView 
                            ID="grdDatas" 
                            runat="server" 
                            AutoGenerateColumns="False" 
                            Width="100%"
                            CssClass="table table-sm table-striped mb-0"
                            HeaderStyle-CssClass="table-light text-muted text-uppercase small fw-semibold"
                            RowStyle-CssClass="align-middle"
                            GridLines="None">
                            
                            <Columns>
                                <asp:TemplateField HeaderText="Data">
                                    <ItemTemplate>
                                        <asp:Label 
                                            ID="Label1" 
                                            runat="server" 
                                            Text='<%# ((DateTime)Eval("Data")).ToShortDateString()%>'
                                            CssClass="fw-semibold text-primary"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                    <HeaderStyle CssClass="text-center" Width="150px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Horário de Início">
                                    <ItemTemplate>
                                        <asp:Label 
                                            ID="Label2" 
                                            runat="server" 
                                            Text='<%# Bind("HorarioInicio") %>'
                                            CssClass="font-monospace small"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                    <HeaderStyle CssClass="text-center" Width="150px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Horário de Fim">
                                    <ItemTemplate>
                                        <asp:Label 
                                            ID="Label3" 
                                            runat="server" 
                                            Text='<%# Bind("HorarioFim") %>'
                                            CssClass="font-monospace small"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                    <HeaderStyle CssClass="text-center" Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            
            <!-- Mensagem de status (mantida para compatibilidade, mas oculta) -->
            <asp:Label 
                id="lblStatus" 
                runat="server" 
                CssClass="alert alert-info d-block mb-3" 
                Visible="False"
                role="alert">
            </asp:Label>
            
            <!-- Botão voltar -->
            <div class="mt-3">
                <asp:LinkButton 
                    ID="lbtnVoltar" 
                    runat="server" 
                    CssClass="btn btn-outline-secondary"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <!-- SweetAlert2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
.btn-with-icon::before {
    content: "\f0ab";
    font-family: "bootstrap-icons";
    margin-right: 0.5rem;
}
</style>

</asp:Content>

