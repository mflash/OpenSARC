<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="ListaTurmas2.aspx.cs" Inherits="Pagina2" 
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Src="../UserControls/SelecionaCalendario.ascx" TagName="SelecionaCalendario" TagPrefix="uc2" %>
<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <script type="text/javascript">
        function confirm_delete() {
            return confirm('Tem certeza que deseja excluir esta turma?');
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                <i class="bi bi-list-ul me-2 text-primary"></i>
                <asp:Label ID="Label1" runat="server"
                    CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                    Text="Lista de Turmas" />
            </div>

            <!-- ═══════════════════════════════════════
                 FILTRO
            ═══════════════════════════════════════ -->
            <div class="card mb-3 shadow-sm">
                <div class="card-body">
                    <asp:CheckBox runat="server" ID="chkMostrarNotes"
                        CssClass="form-check-input me-2"
                        Text="Apenas turmas com notebooks"
                        AutoPostBack="true" />
                </div>
            </div>

            <!-- ═══════════════════════════════════════
                 MENSAGEM DE STATUS
            ═══════════════════════════════════════ -->
            <asp:Label ID="lblStatus" runat="server"
                CssClass="alert alert-success d-block mb-3"
                Visible="False" />

            <!-- ═══════════════════════════════════════
                 GRID DE TURMAS
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-3">
                <asp:GridView ID="grvListaTurmas" runat="server"
                    Width="100%"
                    AutoGenerateColumns="False"
                    OnRowDeleting="grvListaTurmas_RowDeleting"
                    OnRowEditing="grvListaTurmas_RowEditing"
                    DataKeyNames="Id"
                    AllowSorting="True"
                    CssClass="table table-bordered table-hover table-sm align-middle"
                    HeaderStyle-CssClass="table-primary text-center fw-semibold"
                    RowStyle-CssClass="align-middle"
                    AlternatingRowStyle-CssClass="table-light">

                    <Columns>

                        <asp:BoundField DataField="Id" Visible="False" HeaderText="ID" />

                        <asp:BoundField DataField="Numero" HeaderText="Número">
                            <ItemStyle CssClass="text-center" Width="80px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Disciplina.CodCred" HeaderText="CodCred">
                            <ItemStyle CssClass="text-center" Width="80px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Infra" HeaderText="💻" HtmlEncode="False">
                            <ItemStyle CssClass="text-center" Width="50px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Disciplina" HeaderText="Disciplina">
                            <ItemStyle Width="280px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="DataHora" HeaderText="Data & Hora">
                            <ItemStyle Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Professor" HeaderText="Professor">
                            <ItemStyle Width="180px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Curso" HeaderText="Curso">
                            <ItemStyle Width="120px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Sala" HeaderText="Sala" SortExpression="Sala">
                            <ItemStyle CssClass="text-center" Width="100px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Ações">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:HyperLink ID="LinkVer" runat="server"
                                        NavigateUrl='<%#"~/Default/ExportPlan.aspx?id="+Eval("Id")+"&ano="+Eval("Calendario.Ano")+"&sem="+Eval("Calendario.Semestre")%>'
                                        Target="_blank"
                                        CssClass="btn btn-outline-info btn-sm"
                                        ToolTip="Visualizar">
                                        <i class="bi bi-eye"></i>
                                    </asp:HyperLink>
                                    <asp:LinkButton ID="LinkButton1" runat="server"
                                        CausesValidation="False"
                                        CommandName="Edit"
                                        CssClass="btn btn-outline-primary btn-sm"
                                        ToolTip="Editar">
                                        <i class="bi bi-pencil"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server"
                                        CausesValidation="False"
                                        OnClientClick="return confirm_delete();"
                                        CommandName="Delete"
                                        CssClass="btn btn-outline-danger btn-sm"
                                        ToolTip="Excluir">
                                        <i class="bi bi-trash"></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="140px" CssClass="text-center" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>

            <!-- ═══════════════════════════════════════
                 BOTÃO VOLTAR
            ═══════════════════════════════════════ -->
            <div class="d-flex justify-content-start">
                <asp:LinkButton ID="LinkButton1" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        /* Fix para checkbox do ASP.NET */
        .card-body .form-check-input {
            vertical-align: middle;
            margin-right: 0.5rem;
        }

        .card-body label {
            vertical-align: middle;
            margin-bottom: 0;
            cursor: pointer;
        }

        /* Botões de ação compactos */
        .btn-group-sm .btn {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
        }

        .btn-group-sm .btn i {
            font-size: 1rem;
        }
    </style>

</asp:Content>

