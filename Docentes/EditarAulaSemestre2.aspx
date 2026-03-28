<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="EditarAulaSemestre2.aspx.cs" Inherits="Docentes_EditarAula" 
    MaintainScrollPositionOnPostback="true"
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="BusinessData.Util" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <script type="text/javascript">
        function popitup(url, h, w) {
            var newwindow = window.open(url, 'name', 'width=' + (h || 400) + ', height=' + (w || 300) + ', menubar=no, resizable=no');
            if (window.focus) { newwindow.focus(); }
            return false;
        }

        var needToConfirm = false;

        function setDirtyFlag() {
            needToConfirm = true;
            var b = $get('ctl00_cphTitulo_btnSalvarTudo');
            if (b) {
                b.value = "Salvar Agora";
                b.disabled = false;
            }
            var b2 = $get('ctl00_cphTitulo_btnSalvarTudo2');
            if (b2) {
                b2.value = "Salvar Agora";
                b2.disabled = false;
            }
        }

        function testAlert(txt, num) {
            txt.style.color = '#FF0000';
            var c = $get('ctl00_cphTitulo_dgAulas_ctl' + num + '_cbChanged');
            if (c) c.checked = true;
            var b = $get('ctl00_cphTitulo_dgAulas_ctl' + num + '_butConfirm');
            if (b) {
                b.src = '../_layouts/images/STAR.gif';
                b.disabled = false;
            }
            setDirtyFlag();
        }

        function releaseDirtyFlag() {
            needToConfirm = false;
        }

        window.onbeforeunload = function () {
            if (needToConfirm)
                return "Suas alterações não foram salvas. Deseja descartar as alterações feitas?";
        };

        function autoResize(textarea) {
            textarea.style.height = 'auto';
            textarea.style.height = (textarea.scrollHeight) + 'px';
        }

        function initAutoResize() {
            var textareas = document.querySelectorAll('.auto-resize-textarea');
            textareas.forEach(function(textarea) {
                autoResize(textarea);
                textarea.addEventListener('input', function() {
                    autoResize(this);
                });
            });
        }

        // Executar ao carregar a página e após UpdatePanel refresh
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initAutoResize);
        window.addEventListener('load', initAutoResize);

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

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
                 TÍTULO + HORAS
            ═══════════════════════════════════════ -->
            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                <i class="bi bi-calendar3 me-2 text-primary"></i>
                <asp:Label ID="lblTitulo" runat="server"
                    CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                    Text="Aulas do Semestre" />
                <asp:Label ID="lblHoras" runat="server"
                    CssClass="ms-3 text-muted"
                    Text="Horas-relógio:" />
            </div>

            <!-- ═══════════════════════════════════════
                 TOOLBAR: EXPORTAR & COMPARTILHAR
            ═══════════════════════════════════════ -->
            <div class="card mb-3 border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="d-flex flex-wrap align-items-center gap-2">
                        <!-- Exportar -->
                        <span class="text-muted fw-semibold me-1">
                            <i class="bi bi-download me-1"></i>Exportar:
                        </span>
                        <asp:Button ID="Button1" runat="server"
                            OnClick="btnExportarHTML_Click"
                            ToolTip="Faz download de um arquivo HTML com o cronograma"
                            CssClass="btn btn-sm btn-outline-primary"
                            Text="HTML" />
                        <asp:Button ID="Button2" runat="server"
                            OnClick="btnExportarCSV_Click"
                            ToolTip="Faz download de um arquivo CSV com o cronograma para o sistema de atas"
                            CssClass="btn btn-sm btn-outline-secondary"
                            Text="CSV/Atas" />
                        <asp:HyperLink ID="Link1" runat="server"
                            NavigateUrl=""
                            ToolTip="Este link pode ser usado em qualquer lugar para visualizar o cronograma"
                            CssClass="btn btn-sm btn-outline-info"
                            Text="Link HTML" />

                        <!-- Separador visual -->
                        <div class="vr mx-1" style="height: 24px;"></div>

                        <!-- Compartilhar -->
                        <span class="text-muted fw-semibold me-1">
                            <i class="bi bi-share me-1"></i>Compartilhar:
                        </span>
                        <asp:HyperLink ID="Link2" runat="server"
                            NavigateUrl=""
                            Target="_blank"
                            ToolTip="Clique aqui para importar o cronograma como um calendário no Google Calendar"
                            CssClass="btn btn-sm btn-outline-success">
                            <i class="bi bi-google me-1"></i>Google
                        </asp:HyperLink>
                        <asp:HyperLink ID="Link3" runat="server"
                            NavigateUrl=""
                            ToolTip="Este link pode ser usado para importar o cronograma no Outlook/Apple Calendar"
                            CssClass="btn btn-sm btn-outline-primary">
                            <i class="bi bi-calendar-week me-1"></i>Webcal
                        </asp:HyperLink>
                        <asp:HyperLink ID="Link4" runat="server"
                            NavigateUrl=""
                            ToolTip="Download do arquivo .ics (formato iCal)"
                            CssClass="btn btn-sm btn-outline-secondary">
                            <i class="bi bi-download me-1"></i>.ics
                        </asp:HyperLink>

                        <!-- Separador visual -->
                        <div class="vr mx-1" style="height: 24px;"></div>

                        <!-- Botão Salvar -->
                        <asp:Button ID="btnSalvarTudo" runat="server"
                            CssClass="btn btn-primary btn-sm"
                            Text="Salvo"
                            OnClick="btnSalvarTudo_Click"
                            Enabled="False" />
                    </div>
                </div>
            </div>

            <!-- Mensagem de resultado -->
            <asp:Label ID="lblResultado" runat="server"
                CssClass="d-block mb-2 text-success fw-semibold"
                Text="" Visible="true" />

            <!-- CheckBox Auto Save (oculto) -->
            <asp:CheckBox ID="chbAutoSave" runat="server"
                CssClass="form-check-input d-none"
                Text="Auto Save"
                EnableViewState="true"
                Visible="false" />

            <!-- ═══════════════════════════════════════
                 GRID DE AULAS
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-2">
                <asp:DataGrid ID="dgAulas"
                    runat="server"
                    AutoGenerateColumns="False"
                    Width="100%"
                    HorizontalAlign="Center"
                    OnItemDataBound="dgAulas_ItemDataBound"
                    DataKeyField="Id"
                    CssClass="table table-bordered table-hover table-sm align-middle">

                    <ItemStyle CssClass="align-middle text-center" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <AlternatingItemStyle CssClass="table-light" />

                    <Columns>

                        <asp:TemplateColumn HeaderText="AulaId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAulaId" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="lblAula" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                            <ItemStyle VerticalAlign="Middle" Width="30px" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Dia" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblDia" runat="server"
                                    Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblDiaEdit" runat="server"
                                    Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>' />
                            </EditItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server"
                                    Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data/Hora">
                            <ItemTemplate>
                                        <div style="line-height: 1.4; text-align: center;">
            <div class="text-muted small"><%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yy")%>
            <%#(DataHelper.GetDiaPUCRS((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>
            <%#DataBinder.Eval(Container.DataItem, "Hora") %></div>
        </div>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Middle" Width="110px" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Hora" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                            <ItemStyle VerticalAlign="Middle" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Descrição">
                            <ItemTemplate>
                                <div class="descricao-container">
                                    <asp:TextBox ID="txtDescricao" runat="server"
                                        style="resize: none; overflow: hidden;"
                                        CssClass="form-control form-control-sm auto-resize-textarea"
                                        Rows="1"
                                        Width="100%"
                                        TextMode="MultiLine"
                                        Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'
                                        AutoPostBack="False" />
                                    <asp:ImageButton ID="butConfirm"
                                        Enabled="False"
                                        runat="server"
                                        OnClick="btnSalvarTudo_Click"
                                        ImageUrl="~/_layouts/images/STARgray.gif"
                                        CssClass="confirm-btn"
                                        ToolTip="Confirmar alteração" />
                                    <asp:CheckBox ID="cbChanged"
                                        style="display: none"
                                        runat="server" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Atividade">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlAtividade"
                                    AutoPostBack="true"
                                    runat="server"
                                    CssClass="form-select form-select-sm"
                                    OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                            <ItemStyle Width="210px" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos Disponíveis">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDisponiveis"
                                    runat="server"
                                    CssClass="form-select form-select-sm"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlDisponiveis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control form-control-sm" />
                            </EditItemTemplate>
                            <ItemStyle Width="200px" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos_Alocados_id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecursosAlocadosId" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos Selecionados" Visible="True">
                            <ItemTemplate>
                                <asp:Panel ID="pnRecursos" runat="server">
                                    <div class="recursos-container">
                                        <div class="recursos-list-wrapper">
                                            <asp:CheckBoxList ID="cbRecursos" runat="server"
                                                CssClass="recursos-list"
                                                RepeatLayout="UnorderedList">
                                            </asp:CheckBoxList>
                                        </div>
                                        <div class="recursos-actions">
                                            <asp:LinkButton ID="butDeletar" runat="server"
                                                onclick="butDeletar_Click"
                                                CssClass="btn btn-sm btn-outline-danger"
                                                ToolTip="Liberar recurso">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="butTransferir" runat="server"
                                                onclick="butTransferir_Click"
                                                CssClass="btn btn-sm btn-outline-primary"
                                                ToolTip="Transferir recurso">
                                                <i class="bi bi-arrow-right-circle"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="butTrocar" runat="server"
                                                onclick="butTrocar_Click"
                                                CssClass="btn btn-sm btn-outline-secondary"
                                                ToolTip="Trocar recurso">
                                                <i class="bi bi-arrow-left-right"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblRecursosAlocados" runat="server"
                                        Width="250px"
                                        Visible="false" />
                                </asp:Panel>
                            </ItemTemplate>
                            <ItemStyle Width="300px" VerticalAlign="Middle" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="CorDaData" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCorDaData" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="DescData" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblDescData" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                    </Columns>
                </asp:DataGrid>
            </div>

            <!-- Botão Salvar (rodapé) -->
            <div class="d-flex justify-content-end mb-3">
                <asp:Button ID="btnSalvarTudo2" runat="server"
                    CssClass="btn btn-primary btn-sm"
                    Text="Salvar Todos"
                    OnClick="btnSalvarTudo_Click"
                    Enabled="False" />
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <style>
        /* ═══════════════════════════════════════
           CONTAINER DE DESCRIÇÃO COM BOTÃO
        ═══════════════════════════════════════ */
        .descricao-container {
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .descricao-container .form-control {
            flex: 1;
        }

        .confirm-btn {
            flex-shrink: 0;
            width: 20px;
            height: 20px;
            cursor: pointer;
            transition: transform 0.2s;
            border: none;
            background: transparent;
        }

        .confirm-btn:hover:not([disabled]) {
            transform: scale(1.2);
        }

        .confirm-btn[disabled] {
            opacity: 0.5;
            cursor: not-allowed;
        }

        /* ═══════════════════════════════════════
           CONTAINER DE RECURSOS
        ═══════════════════════════════════════ */
        .recursos-container {
            display: flex;
            align-items: flex-start;
            gap: 0.75rem;
            padding: 0.25rem;
        }

        .recursos-list-wrapper {
            flex: 1;
        }

        /* Lista de recursos */
        .recursos-list {
            padding-left: 0;
            list-style: none;
            margin-bottom: 0;
        }

        /* Estiliza cada item da CheckBoxList */
        .recursos-list li {
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            padding: 0.5rem 0.75rem;
            margin-bottom: 0.5rem;
            transition: all 0.2s;
            display: flex;
            align-items: center;
        }

        .recursos-list li:last-child {
            margin-bottom: 0;
        }

        .recursos-list li:hover {
            background: #e9ecef;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        /* Estiliza o checkbox */
        .recursos-list input[type="checkbox"] {
            width: 1.2rem;
            height: 1.2rem;
            margin-right: 0.75rem;
            cursor: pointer;
            flex-shrink: 0;
        }

        /* Estiliza o label */
        .recursos-list label {
            cursor: pointer;
            font-weight: 500;
            color: #495057;
            margin-bottom: 0;
            flex-grow: 1;
            font-size: 0.875rem;
        }

/* ═══════════════════════════════════════
   BOTÕES DE AÇÕES DE RECURSOS (Bootstrap Modernos)
═══════════════════════════════════════ */
.recursos-actions {
    display: flex;
    flex-direction: row;
    gap: 0.375rem;
    flex-shrink: 0;
    align-items: center;
}

.recursos-actions .btn {
    padding: 0.25rem 0.5rem;
    font-size: 1rem;
    line-height: 1;
    border-radius: 0.25rem;
    transition: all 0.2s;
}

.recursos-actions .btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 2px 4px rgba(0,0,0,0.15);
}

.recursos-actions .btn i {
    display: block;
}

        /* ═══════════════════════════════════════
           TEXTAREAS - Estilos legados (mantidos)
        ═══════════════════════════════════════ */
        textarea.normal {
            font-family: verdana;
            font-size: 8pt;
            text-decoration: none;
            color: #003399;
        }

        textarea.changed {
            font-family: verdana;
            font-size: 8pt;
            text-decoration: none;
            color: #ff0000;
        }

        /* ═══════════════════════════════════════
           RESPONSIVE
        ═══════════════════════════════════════ */
        @media (max-width: 768px) {
            .recursos-container {
                flex-direction: column;
            }

            .recursos-actions {
                flex-direction: row;
                width: 100%;
                justify-content: center;
            }
        }
    </style>

</asp:Content>