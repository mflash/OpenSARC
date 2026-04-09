<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="ListaEventos2.aspx.cs" Inherits="Eventos_ListaEventos"
    Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div id="modernProgressOverlay"></div>
                    <div id="modernProgressMessage">
                        <div class="modern-spinner-container">
                            <div class="spinner-border text-primary" role="status">
                                <span class="visually-hidden">Carregando...</span>
                            </div>
                            <div class="mt-3 fw-semibold text-primary">
                                Processando...
                            </div>
                            <div class="mt-1 text-muted small">
                                Por favor, aguarde
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!-- Flags ocultas: atualizadas pelo server, lidas pelo JS após o postback -->
            <asp:HiddenField ID="hdnAbrirModal" runat="server" Value="false" />
            <asp:HiddenField ID="hdnMensagemAviso" runat="server" Value="" />
            <asp:Label ID="lblDatas" runat="server" Text="Datas do Evento" style="display:none;" />

            <!-- ═══════════════════════════════════════
                 TÍTULO
            ═══════════════════════════════════════ -->
            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                <i class="bi bi-calendar-event me-2 text-primary"></i>
                <asp:Label ID="lblTitulo" runat="server"
                    CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                    Text="Lista de Eventos" />
            </div>

            <!-- ═══════════════════════════════════════
                 TOOLBAR
            ═══════════════════════════════════════ -->
            <div class="card mb-3 border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="d-flex align-items-center gap-3">
                        <span class="text-muted fw-semibold">
                            <i class="bi bi-download me-1"></i>Exportar:
                        </span>
                        <asp:Button ID="btnExportarHtml" runat="server"
                            OnClick="btnExportarHtml_Click"
                            CssClass="btn btn-sm btn-outline-primary"
                            Text="HTML" />
                        <div class="vr"></div>
                        <label class="d-flex align-items-center gap-2 mb-0 user-select-none" style="cursor:pointer;">
                            <asp:CheckBox ID="ckbApenasEventosFuturos" runat="server"
                                AutoPostBack="True"
                                OnCheckedChanged="ckbApenasEventosFuturos_CheckedChanged" />
                            <span class="text-muted fw-semibold">Apenas eventos futuros</span>
                        </label>
                    </div>
                </div>
            </div>

            <!-- ═══════════════════════════════════════
                 MENSAGEM DE STATUS (sucesso)
            ═══════════════════════════════════════ -->
            <asp:Label ID="lblStatus" runat="server"
                CssClass="alert alert-info d-block mb-3"
                Visible="False" />

            <!-- ═══════════════════════════════════════
                 GRID DE EVENTOS
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-3">
                <asp:GridView ID="grvListaEventos" runat="server"
                    DataKeyNames="EventoId"
                    AutoGenerateColumns="False"
                    Width="100%"
                    OnRowEditing="grvListaEventos_RowEditing"
                    OnRowDeleting="grvListaEventos_RowDeleting"
                    OnSelectedIndexChanged="grvListaEventos_SelectedIndexChanged"
                    CssClass="table table-bordered table-hover table-sm align-middle"
                    HeaderStyle-CssClass="table-primary text-center fw-semibold"
                    RowStyle-CssClass="align-middle"
                    AlternatingRowStyle-CssClass="table-light">
                    <Columns>

                        <asp:TemplateField HeaderText="Título">
                            <ItemTemplate>
                                <asp:Label ID="lblTituloEvento" runat="server" Text='<%# Bind("Titulo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Responsável">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsavel" runat="server" Text='<%# Bind("Responsavel") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unidade">
                            <ItemTemplate>
                                <asp:Label ID="lblUnidade" runat="server" Text='<%# Bind("Unidade") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Autor">
                            <ItemTemplate>
                                <asp:Label ID="lblAutor" runat="server" Text='<%# ((PessoaBase)Eval("AutorId")).Nome %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:CommandField SelectText="Visualizar Datas" ShowSelectButton="True"
                            ButtonType="Button"
                            ControlStyle-CssClass="btn btn-sm btn-outline-info">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>

                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <div class="d-flex gap-1 justify-content-center">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True"
                                        CommandName="Update"
                                        CssClass="btn btn-sm btn-success"
                                        Text="Salvar"></asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                        CommandName="Cancel"
                                        CssClass="btn btn-sm btn-outline-secondary"
                                        Text="Cancelar"></asp:LinkButton>
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="d-flex gap-1 justify-content-center">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                        CommandName="Edit"
                                        CssClass="btn btn-sm btn-outline-primary">
                                        <i class="bi bi-pencil me-1"></i>Editar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                        OnClientClick="confirmarExclusao(this); return false;"
                                        CommandName="Delete"
                                        CssClass="btn btn-sm btn-outline-danger">
                                        <i class="bi bi-trash me-1"></i>Deletar
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>

            <!-- ═══════════════════════════════════════
                 BOTÃO VOLTAR
            ═══════════════════════════════════════ -->
            <div class="d-flex justify-content-start">
                <asp:LinkButton ID="lbtnVoltar" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>

            <!-- Grid de datas dentro do UpdatePanel (só os dados, não o modal) -->
            <div id="dadosDatasEvento" style="display:none;">
                <asp:GridView ID="grdDatas" runat="server"
                    AutoGenerateColumns="False"
                    Visible="True"
                    CssClass="table table-bordered table-hover table-sm align-middle mb-0"
                    HeaderStyle-CssClass="table-primary text-center fw-semibold"
                    RowStyle-CssClass="align-middle"
                    AlternatingRowStyle-CssClass="table-light">
                    <Columns>
                        <asp:TemplateField HeaderText="Data">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"
                                    Text='<%# ((DateTime)Eval("Data")).ToShortDateString() %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Horário de Início">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("HorarioInicio") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Horário de Fim">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("HorarioFim") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- ═══════════════════════════════════════
         MODAIS (fora do UpdatePanel — Bootstrap
         mantém as referências entre postbacks)
    ═══════════════════════════════════════ -->

    <!-- Modal: Datas do Evento -->
    <div class="modal fade" id="modalDatasEvento" tabindex="-1"
        aria-labelledby="modalDatasEventoLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalDatasEventoLabel">
                        <i class="bi bi-calendar3 me-2"></i>
                        <span id="spanTituloModal"></span>
                    </h5>
                    <button type="button" class="btn-close btn-close-white"
                        data-bs-dismiss="modal" aria-label="Fechar"></button>
                </div>
                <div class="modal-body" id="modalDatasBody">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        <i class="bi bi-x-lg me-1"></i>Fechar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal: Aviso -->
    <div class="modal fade" id="modalAviso" tabindex="-1" aria-labelledby="modalAvisoLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-warning text-dark">
                    <h5 class="modal-title fw-bold" id="modalAvisoLabel">
                        <i class="bi bi-exclamation-triangle-fill me-2"></i>Atenção
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                </div>
                <div class="modal-body fs-6" id="modalAvisoTexto"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal: Confirmação de Exclusão -->
    <div class="modal fade" id="modalConfirmarExclusao" tabindex="-1"
        aria-labelledby="modalConfirmarExclusaoLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title fw-bold" id="modalConfirmarExclusaoLabel">
                        <i class="bi bi-trash me-2"></i>Confirmar Exclusão
                    </h5>
                    <button type="button" class="btn-close btn-close-white"
                        data-bs-dismiss="modal" aria-label="Fechar"></button>
                </div>
                <div class="modal-body">
                    Tem certeza que deseja excluir este evento? Esta ação não pode ser desfeita.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        <i class="bi bi-x-lg me-1"></i>Cancelar
                    </button>
                    <button type="button" class="btn btn-danger" id="btnConfirmarExclusao">
                        <i class="bi bi-trash me-1"></i>Excluir
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var _btnExcluirPendente = null;

        function confirmarExclusao(btn) {
            _btnExcluirPendente = btn;
            bootstrap.Modal.getOrCreateInstance(document.getElementById('modalConfirmarExclusao')).show();
        }

        document.getElementById('btnConfirmarExclusao').addEventListener('click', function () {
            bootstrap.Modal.getOrCreateInstance(document.getElementById('modalConfirmarExclusao')).hide();
            if (_btnExcluirPendente) {
                _btnExcluirPendente.removeAttribute('onclick');
                _btnExcluirPendente.click();
                _btnExcluirPendente = null;
            }
        });

        function verificarModais() {
            // --- Modal de aviso ---
            var hdnAviso = document.getElementById('<%= hdnMensagemAviso.ClientID %>');
            if (hdnAviso && hdnAviso.value.length > 0) {
                var mensagem = hdnAviso.value;
                hdnAviso.value = '';
                requestAnimationFrame(function () {
                    document.getElementById('modalAvisoTexto').innerText = mensagem;
                    bootstrap.Modal.getOrCreateInstance(document.getElementById('modalAviso')).show();
                });
                return;
            }

            // --- Modal de datas ---
            var hdnAbrir = document.getElementById('<%= hdnAbrirModal.ClientID %>');
            if (hdnAbrir && hdnAbrir.value === 'true') {
                hdnAbrir.value = 'false';
                var origem = document.getElementById('dadosDatasEvento');
                var htmlDados = origem ? origem.innerHTML : '';
                var lblDatas = document.getElementById('<%= lblDatas.ClientID %>');
                var titulo = lblDatas ? lblDatas.innerText : 'Datas do Evento';
                requestAnimationFrame(function () {
                    document.getElementById('modalDatasBody').innerHTML = htmlDados;
                    document.getElementById('spanTituloModal').innerText = titulo;
                    bootstrap.Modal.getOrCreateInstance(document.getElementById('modalDatasEvento')).show();
                });
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(verificarModais);
        document.addEventListener('DOMContentLoaded', verificarModais);
    </script>

</asp:Content>
