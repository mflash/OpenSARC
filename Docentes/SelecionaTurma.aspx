<%@ page language="C#" debug="true" masterpagefile="~/Master/Master.master" autoeventwireup="true" Inherits="Docentes_SelecionaTurma" CodeFile="SelecionaTurma.aspx.cs" 
 title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <!-- ═══════════════════════════════════════
             TÍTULO DA PÁGINA
        ═══════════════════════════════════════ -->
        <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
            <i class="bi bi-grid-1x2-fill me-2 text-primary"></i>
            <asp:Label ID="lblTitulo" runat="server"
                CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                Text="TURMAS / EVENTOS" />
        </div>

        <!-- ═══════════════════════════════════════
             SEÇÃO: PROPOSTAS DE TROCA
        ═══════════════════════════════════════ -->
        <div class="mb-4">
            <asp:Label Visible="False" ID="lblRotulo" runat="server"
                CssClass="fw-semibold text-warning d-block mb-2"
                Text="Foram feitas propostas de troca de recursos para as seguintes aulas:" />

            <asp:Literal runat="server" ID="htmlMOTD" />
            <asp:Literal runat="server" ID="htmlAniver" />

            <div class="table-responsive">
                <asp:DataGrid ID="dgTroca"
                    runat="server"
                    AutoGenerateColumns="False"
                    Width="100%"
                    DataKeyField="Id"
                    OnItemCommand="dgTroca_ItemCommand"
                    OnItemDataBound="dgTroca_ItemDataBound"
                    Visible="False"
                    CssClass="table table-bordered table-hover table-sm text-center align-middle">

                    <ItemStyle CssClass="align-middle" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <AlternatingItemStyle CssClass="table-light" />

                    <Columns>
                        <asp:TemplateColumn HeaderText="TrocaId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblTrocaId" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Turma/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblTurmaEvento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Horário">
                            <ItemTemplate>
                                <asp:Label ID="lblHorario" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recurso Proposto">
                            <ItemTemplate>
                                <asp:Label ID="lblRecProposto" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recurso Requerido">
                            <ItemTemplate>
                                <asp:Label ID="lblRecOferecido" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Responsável">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsavel" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:ButtonColumn CommandName="Aceitou" Text="Aceitar"
                            ButtonType="PushButton"
                            ItemStyle-CssClass="text-center" />
                        <asp:ButtonColumn CommandName="Recusou" Text="Recusar"
                            ButtonType="PushButton"
                            ItemStyle-CssClass="text-center" />
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>

        <!-- ═══════════════════════════════════════
             SEÇÃO: TRANSFERÊNCIAS RECEBIDAS
        ═══════════════════════════════════════ -->
        <div class="mb-4">
            <asp:Label Visible="False" ID="lblTransfencia" runat="server"
                CssClass="fw-semibold text-info d-block mb-2"
                Text="Os seguintes recursos foram transferidos para você:" />

            <div class="table-responsive">
                <asp:DataGrid ID="dgTransferencias"
                    runat="server"
                    AutoGenerateColumns="False"
                    Width="100%"
                    DataKeyField="Id"
                    OnItemCommand="dgTransferencias_ItemCommand"
                    OnItemDataBound="dgTransferencias_ItemDataBound"
                    Visible="False"
                    CssClass="table table-bordered table-hover table-sm text-center align-middle">

                    <ItemStyle CssClass="align-middle" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <AlternatingItemStyle CssClass="table-light" />

                    <Columns>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblTransId" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Autor">
                            <ItemTemplate>
                                <asp:Label ID="lblAutor" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recurso Recebido">
                            <ItemTemplate>
                                <asp:Label ID="lblRecurso" runat="server"
                                    Text='<%#((Recurso)Eval("Recurso")).Descricao %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Turma/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblTurmaEvento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server"
                                    Text='<%#((DateTime)Eval("Data")).ToShortDateString() %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:ButtonColumn CommandName="Viu" Text="OK"
                            ButtonType="PushButton"
                            ItemStyle-CssClass="text-center" />
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>

        <!-- ═══════════════════════════════════════
             SEÇÃO: MINHAS TURMAS
        ═══════════════════════════════════════ -->
        <div class="d-flex align-items-center mb-2">
            <i class="bi bi-book-half me-2 text-primary"></i>
            <span class="fw-semibold text-primary">Minhas Turmas</span>
        </div>

        <asp:Label ID="lblTurmas" runat="server" Visible="False"
            CssClass="text-muted fst-italic d-block mb-2"
            Text="Nenhuma turma cadastrada." />

        <div class="table-responsive mb-2">
            <asp:GridView ID="grvListaTurmas" runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                AllowSorting="True"
                DataKeyNames="Id"
                OnRowEditing="grvListaTurmas_RowEditing"
                CssClass="table table-bordered table-hover table-sm align-middle"
                HeaderStyle-CssClass="table-primary fw-semibold"
                RowStyle-CssClass="align-middle"
                AlternatingRowStyle-CssClass="table-light">
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" />

                    <asp:BoundField HeaderText="Cód/Cred" DataField="Disciplina.CodCred">
                        <ItemStyle CssClass="text-center" />
                        <HeaderStyle CssClass="text-center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Disciplina" DataField="Disciplina">
                        <ItemStyle CssClass="text-start" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Turma" DataField="Numero">
                        <ItemStyle CssClass="text-center" />
                        <HeaderStyle CssClass="text-center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Curso" DataField="Curso">
                        <ItemStyle CssClass="text-center" />
                        <HeaderStyle CssClass="text-center" />
                    </asp:BoundField>

                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server"
                                CausesValidation="True"
                                CommandName="Update"
                                CssClass="btn btn-sm btn-success"
                                Text="Alterar" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server"
                                CausesValidation="False"
                                CommandName="Edit"
                                CssClass="btn btn-sm btn-primary"
                                Text="Selecionar" />
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="alert alert-warning d-flex align-items-start gap-2 mb-3" role="alert">
            <i class="bi bi-exclamation-triangle-fill mt-1"></i>
            <div>
                <strong>Obs:</strong> Conforme comunicado, serão criadas (vazias) as áreas Moodle para todas as disciplinas da Escola.<br />
                Se houver algum erro ou inconsistência nas suas disciplinas, entre em contato com
                <a href="mailto:recursos.politecnica@pucrs.br" class="alert-link">recursos.politecnica@pucrs.br</a>
            </div>
        </div>

        <asp:Button Visible="false" ID="butMoodle" runat="server"
            OnClick="butMoodle_Click"
            CssClass="btn btn-outline-primary mb-3"
            Text="Clique aqui para solicitar a criação de áreas Moodle para as suas turmas" />

        <!-- ═══════════════════════════════════════
             SEÇÃO: MEUS EVENTOS
        ═══════════════════════════════════════ -->
        <div class="d-flex align-items-center mb-2">
            <i class="bi bi-calendar-event me-2 text-primary"></i>
            <span class="fw-semibold text-primary">Meus Eventos</span>
        </div>

        <asp:Label ID="lblEventos" runat="server" Visible="False"
            CssClass="text-muted fst-italic d-block mb-2"
            Text="Nenhum evento cadastrado." />

        <div class="table-responsive mb-4">
            <asp:DataGrid ID="dgEventos"
                runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                OnItemCommand="dgEventos_ItemCommand"
                CssClass="table table-bordered table-hover table-sm text-center align-middle">

                <ItemStyle CssClass="align-middle" />
                <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                <AlternatingItemStyle CssClass="table-light" />

                <Columns>
                    <asp:TemplateColumn Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblEventoId" runat="server"
                                Text='<%#DataBinder.Eval(Container.DataItem, "EventoId") %>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Título">
                        <ItemTemplate>
                            <asp:Label ID="lblTituloEvento" runat="server"
                                Text='<%#DataBinder.Eval(Container.DataItem, "Titulo") %>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Unidade">
                        <ItemTemplate>
                            <asp:Label ID="lblUnidade" runat="server"
                                Text='<%#DataBinder.Eval(Container.DataItem, "Unidade") %>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:ButtonColumn CommandName="Horarios" Text="Selecionar"
                        ButtonType="PushButton"
                        ItemStyle-CssClass="text-center" />
                </Columns>
            </asp:DataGrid>
        </div>

    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
