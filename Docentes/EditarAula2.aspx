<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="EditarAula2.aspx.cs" Inherits="Docentes_EditarAula"
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" Trace="false" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="BusinessData.Util" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <script type="text/javascript">
        function popitup(url) {
            var newwindow = window.open(url, 'name', 'height=140,width=333');
            if (window.focus) { newwindow.focus(); }
            return false;
        }

        var needToConfirm = false;

        function setDirtyFlag() {
            needToConfirm = true;
            var b = $get('ctl00_cphTitulo_btnSalvarTudo');
            b.value = "Salvar Agora"; b.disabled = false;
            b = $get('ctl00_cphTitulo_btnSalvarTudo2');
            b.value = "Salvar Agora"; b.disabled = false;
        }

        function releaseDirtyFlag() { needToConfirm = false; }

        window.onbeforeunload = function () {
            if (needToConfirm)
                return "Suas alterações não foram salvas. Deseja descartar as alterações feitas?";
        };
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
            <i class="bi bi-pencil-square me-2 text-primary"></i>
            <asp:Label ID="lblTitulo" runat="server"
                CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                Text="Aulas" />
            <asp:Label ID="lblHoras" runat="server"
                CssClass="ms-3 text-muted"
                Text="Horas-relógio:" />
        </div>

        <!-- ═══════════════════════════════════════
             TOOLBAR: exportar / importar
        ═══════════════════════════════════════ -->
        <div class="d-flex flex-wrap align-items-center gap-2 mb-3">
            <asp:Button ID="btnExportarHTML" runat="server"
                OnClick="btnExportarHTML_Click"
                CssClass="btn btn-sm btn-outline-primary"
                Text="Exportar HTML" />
            <asp:Button ID="btnExportarCSV" runat="server"
                OnClick="btnExportarCSV_Click"
                ToolTip="Faz download de um arquivo CSV com o cronograma para o sistema de atas"
                CssClass="btn btn-sm btn-outline-secondary"
                Text="Exportar CSV/Atas" />
            <asp:Button ID="btnImportarCSV" runat="server"
                OnClick="btnImportarCSV_Click"
                ToolTip="Importa cronograma a partir do CSV do sistema de atas"
                CssClass="btn btn-sm btn-outline-secondary"
                Text="Importar CSV/Atas" />
            <asp:FileUpload ID="csvUpload" runat="server" />
        </div>

        <!-- Mensagem de resultado -->
        <asp:Label ID="lblResultado" runat="server"
            CssClass="d-block mb-2 text-success fw-semibold"
            Text="" Visible="true" />

        <!-- Botão Salvar (topo) -->
        <div class="mb-2">
            <asp:Button ID="btnSalvarTudo" runat="server"
                CssClass="btn btn-primary btn-sm"
                Text="Salvar Todos"
                OnClick="btnSalvarTudo_Click" />
        </div>

        <!-- ═══════════════════════════════════════
             GRID DE AULAS
        ═══════════════════════════════════════ -->
        <div class="table-responsive mb-2">
            <asp:DataGrid ID="dgAulas"
                runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                OnItemDataBound="dgAulas_ItemDataBound"
                DataKeyField="Id"
                OnItemCommand="dgAulas_ItemCommand"
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
                        <ItemStyle VerticalAlign="Middle" Width="30px"/>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Dia" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDia" runat="server"
                                Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblDiaEdit" runat="server"
                                Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>' />
                        </EditItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Data" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblData" runat="server"
                                Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Hora" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblHora" runat="server"
                                Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Data/Hora">
                        <ItemTemplate>
                            <asp:Label ID="lblData2" runat="server"
                                Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yy")%>' />
                            <asp:Label ID="lblDia2" runat="server"
                                Text='<%#(DataHelper.GetDiaPUCRS((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>' />
                            <asp:Label ID="lblHora2" runat="server"
                                Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox14" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                        <ItemStyle VerticalAlign="Middle" Width="110px" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Descrição">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescricao" runat="server"
                                style="resize: none;"
                                CssClass="form-control form-control-sm"
                                Height="38px" Width="100%"
                                TextMode="MultiLine"
                                Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Atividade">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAtividade" runat="server"
                                CssClass="form-select form-select-sm"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                        <ItemStyle Width="210px" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Tipo de Recurso">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlRecurso" runat="server"
                                CssClass="form-select form-select-sm"
                                OnSelectedIndexChanged="ddlRecurso_SelectedIndexChanged"
                                AutoPostBack="true" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox17" runat="server" CssClass="form-control form-control-sm" />
                        </EditItemTemplate>
                        <ItemStyle Width="170px" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Selecionados" Visible="true">
                        <ItemTemplate>
                            <asp:Panel ID="pnRecursos" runat="server">
                                <div class="recursos-container">
                                    <asp:CheckBoxList ID="cbRecursos" runat="server" 
                                        CssClass="recursos-list" 
                                        RepeatLayout="UnorderedList">
                                    </asp:CheckBoxList>
                                    <asp:LinkButton ID="butDeletar" runat="server"
                                        OnClick="butDeletar_Click"
                                        CssClass="btn btn-sm btn-outline-danger recursos-delete-btn"
                                        ToolTip="Remover recurso(s) selecionado(s)">
                                        <i class="bi bi-trash"></i>
                                    </asp:LinkButton>
                                </div>
                                <asp:Label ID="lblRecursosAlocados" runat="server"
                                    Width="250px" Visible="false" />
                            </asp:Panel>
                        </ItemTemplate>
                        <ItemStyle Width="250px" VerticalAlign="Middle" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Selecionados" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblRecursosSelecionados" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control form-control-sm" />
    </EditItemTemplate>
                    </asp:TemplateColumn>

                    <asp:ButtonColumn CommandName="Select" Text="Editar..." Visible="false" />

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
                OnClick="btnSalvarTudo_Click" />
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
    </Triggers>
    </asp:UpdatePanel>

    <style>
        /* Container principal dos recursos */
        .recursos-container {
            display: flex;
            align-items: center;
            gap: 0.75rem;
            justify-content: space-between;
            padding: 0.25rem;
        }

        /* Lista de recursos */
        .recursos-list {
            padding-left: 0;
            list-style: none;
            margin-bottom: 0;
            flex: 1;
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

        /* Botão de deletar (LinkButton) */
        .recursos-delete-btn {
            flex-shrink: 0;
            align-self: flex-start;
            padding: 0.375rem 0.5rem;
            line-height: 1;
            border-radius: 0.25rem;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            justify-content: center;
        }

        .recursos-delete-btn:hover {
            background-color: #dc3545 !important;
            color: white !important;
            border-color: #dc3545 !important;
            text-decoration: none;
        }

        /* Ícone dentro do botão */
        .recursos-delete-btn i {
            font-size: 1rem;
            pointer-events: none;
        }
    </style>
</asp:Content>


