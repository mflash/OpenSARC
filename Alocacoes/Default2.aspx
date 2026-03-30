<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true" 
    CodeFile="Default2.aspx.cs" Inherits="Alocacoes_Default" 
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter"></div>
                    <div id="processMessage">
                        <uc1:Aguarde ID="Aguarde1" runat="server" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!-- ═══════════════════════════════════════
                 TÍTULO
            ═══════════════════════════════════════ -->
            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                <i class="bi bi-calendar-check me-2 text-primary"></i>
                <asp:Label ID="lblDisciplina" runat="server"
                    CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                    Text="Visualizar Recursos Alocados Por:" />
            </div>

            <!-- ═══════════════════════════════════════
                 FILTROS
            ═══════════════════════════════════════ -->
            <div class="card mb-3 shadow-sm">
                <div class="card-body">

                    <!-- Botões de seleção (substituindo RadioButtonList) -->
                    <div class="mb-3">
                        <div class="btn-group" role="group" aria-label="Tipo de visualização">
                            <asp:RadioButtonList ID="rblAlocacoes" runat="server"
                                OnSelectedIndexChanged="rblAlocacoes_SelectedIndexChanged"
                                AutoPostBack="True"
                                RepeatLayout="Flow"
                                RepeatDirection="Horizontal"
                                CssClass="btn-group-modern">
                                <asp:ListItem Selected="True" Value="Data">
                                    <i class="bi bi-calendar3 me-1"></i>Data
                                </asp:ListItem>
                                <asp:ListItem Value="Recurso">
                                    <i class="bi bi-pc-display me-1"></i>Recurso
                                </asp:ListItem>
                                <asp:ListItem Value="Professor">
                                    <i class="bi bi-person me-1"></i>Professor
                                </asp:ListItem>
                                <asp:ListItem Value="Secretário">
                                    <i class="bi bi-briefcase me-1"></i>Secretário
                                </asp:ListItem>
                                <asp:ListItem Value="Turma">
                                    <i class="bi bi-people me-1"></i>Turma
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <!-- Campo Data -->
                    <div class="row mb-3 align-items-center">
                        <div class="col-auto">
                            <asp:Label runat="server" ID="lblData" Text="Data" CssClass="form-label mb-0" />
                        </div>
                        <div class="col-auto">
                            <asp:TextBox ID="txtData" runat="server" CssClass="form-control form-control-sm" Style="width: 150px;" />
                            <cc1:CalendarExtender ID="txtData_CalendarExtender" runat="server"
                                Enabled="True" TargetControlID="txtData" />
                        </div>
                        <div class="col-auto">
                            <asp:Label ID="lblOpcional" runat="server"
                                CssClass="text-danger small fst-italic mb-0"
                                Text="(Opcional)"
                                Visible="False" />
                        </div>
                    </div>

                    <!-- Panel: Visualizar por Categoria -->
                    <asp:Panel ID="pnlVisualizarPorCategoria" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Categoria</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlCategorias" runat="server"
                                    AppendDataBoundItems="True"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged"
                                    CssClass="form-select form-select-sm"
                                    Style="width: 250px;">
                                    <asp:ListItem>Selecione</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Panel: Visualizar por Recurso -->
                    <asp:Panel ID="pnlVisualizarPorRecurso" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Recurso</label>
                            </div>
                            <div class="col-auto" style="position: relative;">
                                <ajaxToolkit:ComboBox ID="cbRecurso" runat="server"
                                    AutoCompleteMode="SuggestAppend"
                                    CssClass="form-control form-control-sm combobox-fix"
                                    Width="500px"
                                    DropDownStyle="DropDownList" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Panel: Visualizar por Horário -->
                    <asp:Panel ID="pnlVisualizarPorHorario" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Categoria</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlCategorias2" runat="server"
                                    AppendDataBoundItems="True"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCategorias2_SelectedIndexChanged"
                                    CssClass="form-select form-select-sm"
                                    Style="width: 250px;">
                                    <asp:ListItem>Selecione</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Horário</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlHorarios" runat="server"
                                    AppendDataBoundItems="True"
                                    CssClass="form-select form-select-sm"
                                    Style="width: 250px;">
                                    <asp:ListItem>Selecione</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Panel: Visualizar por Professor -->
                    <asp:Panel ID="pnlVisualizarPorProfessor" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Professor</label>
                            </div>
                            <div class="col-auto" style="position: relative;">
                                <ajaxToolkit:ComboBox ID="cbProfessor" runat="server"
                                    AutoCompleteMode="SuggestAppend"
                                    CssClass="form-control form-control-sm combobox-fix"
                                    Width="500px"
                                    DropDownStyle="DropDownList" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Panel: Visualizar por Secretário -->
                    <asp:Panel ID="pnlVisualizarPorSecretario" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Secretário</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlSecretario" runat="server"
                                    AppendDataBoundItems="True"
                                    CssClass="form-select form-select-sm"
                                    Style="width: 250px;">
                                    <asp:ListItem>Selecione</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Panel: Visualizar por Turma -->
                    <asp:Panel ID="pnlVisualizarPorTurma" runat="server" Visible="False">
                        <div class="row mb-3 align-items-center">
                            <div class="col-auto">
                                <label class="form-label mb-0">Turma</label>
                            </div>
                            <div class="col-auto" style="position: relative;">
                                <ajaxToolkit:ComboBox ID="cbTurma" runat="server"
                                    AutoCompleteMode="SuggestAppend"
                                    CssClass="form-control form-control-sm combobox-fix"
                                    Width="500px"
                                    DropDownStyle="DropDownList" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Botão Visualizar -->
                    <div class="row">
                        <div class="col-auto">
                            <asp:Button ID="btnVisualizarAlocacoes" runat="server"
                                OnClick="btnVisualizarAlocacoes_Click"
                                Text="Visualizar Alocações"
                                CssClass="btn btn-primary btn-sm" />
                        </div>
                    </div>

                </div>
            </div>

            <!-- ═══════════════════════════════════════
                 MENSAGEM DE STATUS
            ═══════════════════════════════════════ -->
            <asp:Label ID="lblStatus" runat="server"
                CssClass="alert alert-info d-block mb-3"
                Visible="False" />

            <!-- ═══════════════════════════════════════
                 GRID DE ALOCAÇÕES
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-3">
                <asp:DataGrid ID="dgAlocacoes" runat="server"
                    AutoGenerateColumns="False"
                    Width="100%"
                    OnItemDataBound="dgAlocacoes_ItemDataBound"
                    Visible="True"
                    CssClass="table table-bordered table-hover table-sm align-middle"
                    HeaderStyle-CssClass="table-primary text-center fw-semibold"
                    ItemStyle-CssClass="align-middle"
                    AlternatingItemStyle-CssClass="table-light">

                    <Columns>

                        <asp:TemplateColumn HeaderText="Recurso">
                            <ItemTemplate>
                                <asp:Label ID="lblRecurso" runat="server"
                                    Text='<%# ((Recurso)Eval("Recurso")).Descricao%>' />
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server"
                                    Text='<%# ((DateTime)Eval("Data")).ToShortDateString()%>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="text-center" Width="100px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:TemplateColumn>

                        <asp:BoundColumn DataField="Horario" HeaderText="Horário">
                            <ItemStyle CssClass="text-center" Width="80px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundColumn>

                        <asp:TemplateColumn HeaderText="CodCred">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscCod" runat="server" />
                            </ItemTemplate>
                            <ItemStyle CssClass="text-center" Width="80px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Disciplina/Evento">
                            <ItemTemplate>
                                <asp:Label ID="lblDisc" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="250px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>

<%--                        <asp:TemplateColumn HeaderText="Curso">
                            <ItemTemplate>
                                <asp:Label ID="lblCurso" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>--%>

                        <asp:TemplateColumn HeaderText="Responsável">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsavel" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="180px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>

                    </Columns>

                </asp:DataGrid>
            </div>

            <!-- ═══════════════════════════════════════
                 BOTÃO VOLTAR
            ═══════════════════════════════════════ -->
            <div class="d-flex justify-content-start">
                <asp:LinkButton ID="LinkButton1" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    CausesValidation="False"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        /* ═══════════════════════════════════════
           BOTÕES MODERNOS TIPO TOGGLE
        ═══════════════════════════════════════ */
        .btn-group-modern {
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
        }

        .btn-group-modern input[type="radio"] {
            display: none;
        }

        .btn-group-modern label {
            padding: 0.5rem 1rem;
            background-color: #fff;
            border: 2px solid #dee2e6;
            border-radius: 0.375rem;
            cursor: pointer;
            transition: all 0.3s ease;
            font-weight: 500;
            color: #495057;
            display: inline-flex;
            align-items: center;
            margin: 0;
        }

        .btn-group-modern label:hover {
            background-color: #f8f9fa;
            border-color: #0d6efd;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        .btn-group-modern input[type="radio"]:checked + label {
            background-color: #0d6efd;
            border-color: #0d6efd;
            color: #fff;
            box-shadow: 0 4px 12px rgba(13, 110, 253, 0.3);
        }

        .btn-group-modern input[type="radio"]:checked + label i {
            color: #fff;
        }

        .btn-group-modern label i {
            font-size: 1rem;
            transition: color 0.3s ease;
        }

        /* ═══════════════════════════════════════
           FIX PARA COMBOBOX DO AJAXTOOLKIT
        ═══════════════════════════════════════ */
        
        /* Container do ComboBox */
        .ajax__combobox_inputcontainer {
            position: relative !important;
            display: inline-block;
            width: 100%;
        }

        /* TextBox do ComboBox */
        .ajax__combobox_textboxcontainer input {
            width: 100% !important;
            padding: 0.375rem 0.75rem;
            font-size: 0.875rem;
            border: 1px solid #ced4da;
            border-radius: 0.25rem;
        }

        /* Botão dropdown do ComboBox */
        .ajax__combobox_buttoncontainer button {
            height: 100% !important;
            border: 1px solid #ced4da;
            background-color: #e9ecef;
            padding: 0 0.5rem;
        }

        .ajax__combobox_buttoncontainer button:hover {
            background-color: #dee2e6;
        }

        /* Lista dropdown do ComboBox - CORREÇÃO PRINCIPAL */
        .ajax__combobox_itemlist {
            position: absolute !important;
            left: 0 !important;
            top: 100% !important;
            width: 100% !important;
            max-height: 200px !important;
            overflow-y: auto !important;
            background-color: white !important;
            border: 1px solid #ced4da !important;
            border-radius: 0.25rem !important;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
            z-index: 1050 !important;
            margin-top: 2px !important;
        }

        /* Itens da lista */
        .ajax__combobox_itemlist li {
            padding: 0.5rem 0.75rem !important;
            cursor: pointer !important;
            list-style: none !important;
        }

        .ajax__combobox_itemlist li:hover {
            background-color: #f8f9fa !important;
        }

        .ajax__combobox_itemlist .ajax__combobox_highlighteditem {
            background-color: #0d6efd !important;
            color: white !important;
        }

        /* CalendarExtender z-index fix */
        .ajax__calendar_container {
            z-index: 1060 !important;
        }

        /* Responsivo */
        @media (max-width: 768px) {
            .btn-group-modern {
                flex-direction: column;
            }

            .btn-group-modern label {
                width: 100%;
                justify-content: center;
            }

            .ajax__combobox_itemlist {
                max-width: 100vw !important;
            }
        }
    </style>

</asp:Content>

