<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true" CodeFile="EditarEventos2.aspx.cs" Inherits="Eventos_EditarEventos" Title="Editar Evento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!-- ═══════════════════════════════════════
         TÍTULO
    ═══════════════════════════════════════ -->
    <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
        <i class="bi bi-calendar-event me-2 text-primary"></i>
        <asp:Label ID="lblTitulo" runat="server"
            CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
            Text="EDITAR EVENTO" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- ═══════════════════════════════════════
                 FORMULÁRIO DE EVENTO
            ═══════════════════════════════════════ -->
            <div class="card shadow-sm border-0 mb-3">
                <div class="card-body">

                    <!-- Título -->
                    <div class="row mb-3">
                        <label class="col-sm-3 col-form-label fw-semibold">Título</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvTitulo" runat="server"
                                ControlToValidate="txtTitulo"
                                CssClass="text-danger small"
                                ErrorMessage="Digite um título."
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <!-- Responsável -->
                    <div class="row mb-3">
                        <label class="col-sm-3 col-form-label fw-semibold">Responsável</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtResponsavel" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                ControlToValidate="txtResponsavel"
                                CssClass="text-danger small"
                                ErrorMessage="Digite um Responsável pelo Evento."
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <!-- Unidade -->
                    <div class="row mb-3">
                        <label class="col-sm-3 col-form-label fw-semibold">Unidade</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                ControlToValidate="txtUnidade"
                                CssClass="text-danger small"
                                ErrorMessage="Digite uma Unidade."
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <!-- Descrição -->
                    <div class="row mb-3">
                        <label class="col-sm-3 col-form-label fw-semibold">Descrição</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtaDescricao" runat="server"
                                TextMode="MultiLine"
                                Rows="3"
                                CssClass="form-control" />
                        </div>
                    </div>

                </div>
            </div>

            <!-- ═══════════════════════════════════════
                 DATA E HORÁRIOS
            ═══════════════════════════════════════ -->
            <asp:Panel ID="Panel1" runat="server">
                <div class="card shadow-sm border-0 mb-3">
                    <div class="card-body">

                        <!-- Data -->
                        <div class="row mb-3">
                            <label class="col-sm-3 col-form-label fw-semibold">Data</label>
                            <div class="col-sm-9 col-md-6 col-lg-4">
                                <asp:TextBox ID="txtData" runat="server" TextMode="Date"
                                    CssClass="form-control" Style="max-width: 200px;" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtData"
                                    CssClass="text-danger small"
                                    ErrorMessage="Digite uma data de início."
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Horários -->
                        <div class="row mb-3">
                            <label class="col-sm-3 col-form-label fw-semibold">Horários</label>
                            <div class="col-sm-9">
                                <div class="row g-2">
                                    <div class="col-auto">
                                        <label class="form-label small mb-1">Início:</label>
                                        <asp:DropDownList ID="ddlInicio" runat="server"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlInicio_SelectedIndexChanged"
                                            CssClass="form-select">
                                            <asp:ListItem>AB</asp:ListItem>
                                            <asp:ListItem>CD</asp:ListItem>
                                            <asp:ListItem>EX</asp:ListItem>
                                            <asp:ListItem>FG</asp:ListItem>
                                            <asp:ListItem>HI</asp:ListItem>
                                            <asp:ListItem>JK</asp:ListItem>
                                            <asp:ListItem>LM</asp:ListItem>
                                            <asp:ListItem>NP</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-auto">
                                        <label class="form-label small mb-1">Até:</label>
                                        <asp:DropDownList ID="ddlFim" runat="server" CssClass="form-select" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

            <!-- ═══════════════════════════════════════
                 RECORRÊNCIA
            ═══════════════════════════════════════ -->
            <div class="card shadow-sm border-0 mb-3">
                <div class="card-body">

                    <div class="row mb-3">
                        <label class="col-sm-3 col-form-label fw-semibold">Recorrência</label>
                        <div class="col-sm-9">
                            <label class="d-flex align-items-center gap-2 mb-0 user-select-none" style="cursor: pointer;">
                                <asp:CheckBox ID="ckbEhRecorrente" runat="server"
                                    AutoPostBack="True"
                                    Enabled="False"
                                    OnCheckedChanged="ckbEhRecorrente_CheckedChanged" />
                                <span>Tornar Recorrente</span>
                            </label>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-sm-3"></div>
                        <div class="col-sm-9">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server"
                                Visible="false"
                                Enabled="False"
                                OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"
                                AutoPostBack="True"
                                CssClass="form-check">
                                <asp:ListItem Value="0">&nbsp;Diário</asp:ListItem>
                                <asp:ListItem Value="1">&nbsp;Selecionar Dias</asp:ListItem>
                                <asp:ListItem Value="2">&nbsp;Seleção Manual</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <!-- Dias da Semana -->
                    <asp:Panel ID="pnlDias" runat="server" Visible="false">
                        <div class="row mb-3">
                            <div class="col-sm-3"></div>
                            <div class="col-sm-9">
                                <div class="d-flex flex-wrap gap-3">
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="seg" runat="server" /><span>SEG</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="ter" runat="server" /><span>TER</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="qua" runat="server" /><span>QUA</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="qui" runat="server" /><span>QUI</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="sex" runat="server" /><span>SEX</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="sab" runat="server" /><span>SAB</span>
                                    </label>
                                    <label class="d-flex align-items-center gap-1 mb-0 user-select-none" style="cursor: pointer;">
                                        <asp:CheckBox ID="dom" runat="server" /><span>DOM</span>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Selecionar Datas -->
                    <asp:Panel ID="pnlSelecionarDatas" runat="server" Visible="false">
                        <div class="row mb-3">
                            <label class="col-sm-3 col-form-label fw-semibold">Data Final</label>
                            <div class="col-sm-9 col-md-6 col-lg-4">
                                <asp:TextBox ID="txtDataFinal" runat="server" TextMode="Date"
                                    CssClass="form-control" Style="max-width: 200px;" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtDataFinal"
                                    CssClass="text-danger small"
                                    ErrorMessage="Digite uma data de fim."
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Seleção Manual -->
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                        <div class="row mb-3">
                            <label class="col-sm-3 col-form-label fw-semibold">Data</label>
                            <div class="col-sm-9 col-md-6 col-lg-4">
                                <asp:TextBox ID="txtDataFim" runat="server" TextMode="Date"
                                    CssClass="form-control" Style="max-width: 200px;" />
                                <asp:CustomValidator ID="cvDataFim" runat="server"
                                    ControlToValidate="txtDataFim"
                                    ClientValidationFunction="validarDataFim"
                                    CssClass="text-danger small"
                                    ErrorMessage="Digite uma data."
                                    Display="Dynamic">*</asp:CustomValidator>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <label class="col-sm-3 col-form-label fw-semibold">Horários</label>
                            <div class="col-sm-9">
                                <div class="row g-2 mb-3">
                                    <div class="col-auto">
                                        <label class="form-label small mb-1">Início:</label>
                                        <asp:DropDownList ID="ddlHoraInicio" runat="server"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlHoraInicio_SelectedIndexChanged"
                                            CssClass="form-select">
                                            <asp:ListItem>AB</asp:ListItem>
                                            <asp:ListItem>CD</asp:ListItem>
                                            <asp:ListItem>EX</asp:ListItem>
                                            <asp:ListItem>FG</asp:ListItem>
                                            <asp:ListItem>HI</asp:ListItem>
                                            <asp:ListItem>JK</asp:ListItem>
                                            <asp:ListItem>LM</asp:ListItem>
                                            <asp:ListItem>NP</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-auto">
                                        <label class="form-label small mb-1">Até:</label>
                                        <asp:DropDownList ID="ddlHoraFim" runat="server" CssClass="form-select" />
                                    </div>
                                </div>

                                <asp:Button ID="btnAdicionar" runat="server"
                                    Text="Adicionar Horário"
                                    CssClass="btn btn-sm btn-primary"
                                    OnClick="btnAdicionar_Click1" />
                            </div>
                        </div>

                        <!-- Grid de Horários -->
                        <div class="row">
                            <div class="col-sm-3"></div>
                            <div class="col-sm-9">
                                <asp:GridView ID="grdHorarios" runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="table table-bordered table-hover table-sm"
                                    OnRowDeleting="grdHorarios_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Data">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server"
                                                    Text='<%# ((DateTime)Eval("data")).ToShortDateString() %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Horário de Início">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server"
                                                    Text='<%# Bind("horarioInicio") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Horário de Fim">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server"
                                                    Text='<%# Bind("horarioFim") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDeletar" runat="server"
                                                    CommandName="Delete"
                                                    CssClass="btn btn-sm btn-outline-danger"
                                                    CausesValidation="False">
                                                    <i class="bi bi-trash"></i> Deletar
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="table-primary" />
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>

            <!-- ═══════════════════════════════════════
                 MENSAGENS E AÇÕES
            ═══════════════════════════════════════ -->
            <div class="card shadow-sm border-0 mb-3">
                <div class="card-body">

                    <asp:Label ID="lblResultado" runat="server"
                        CssClass="d-block mb-2 text-success fw-semibold" />

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                        CssClass="alert alert-danger"
                        HeaderText="Por favor, corrija os seguintes erros:" />

                    <div class="d-flex gap-2 align-items-center">
                        <asp:Button ID="btnOk" runat="server"
                            OnClick="btnOk_Click"
                            Text="Salvar Evento"
                            CssClass="btn btn-primary" />
                        <asp:LinkButton ID="lbtnVoltar" runat="server"
                            CssClass="btn btn-outline-secondary"
                            OnClick="lbtnVoltar_Click1"
                            CausesValidation="False">
                            <i class="bi bi-arrow-left me-1"></i>Voltar
                        </asp:LinkButton>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function validarDataFim(sender, args) {
            args.IsValid = document.getElementById('<%= txtDataFim.ClientID %>').value.length > 0;
        }
    </script>

</asp:Content>
