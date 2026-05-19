<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master"
    AutoEventWireup="true" CodeFile="DetalhesEvento2.aspx.cs" Inherits="DetalhesEvento"
    Title="Detalhes do Evento" EnableEventValidation="false" %>

<%@ Import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="Server">

    <script type="text/javascript">
        function popitup(url, w, h) {
            var win = window.open(url, 'name', 'width=' + (w || 400) + ',height=' + (h || 300) + ',menubar=no,resizable=no');
            if (window.focus) { win.focus(); }
            return false;
        }

        var ddlsCarregados = {};

        function carregarRecursosDisponiveis(ddl) {
            if (ddlsCarregados[ddl.id]) return;
            if (ddl.options.length > 1) { ddlsCarregados[ddl.id] = true; return; }

            var row = ddl.closest('tr');
            if (!row) return;

            var divDataHora = row.querySelector('div.text-muted.small');
            if (!divDataHora) return;

            var textoCompleto = (divDataHora.innerText || divDataHora.textContent).trim();
            var partes = textoCompleto.split(' ');
            var data = partes[0];
            var hora = partes[partes.length - 1];

            ddl.disabled = true;
            ddl.options[0].text = "Carregando...";

            fetch('DetalhesEvento2.aspx/ObterRecursosDisponiveis', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({ data: data, hora: hora })
            })
            .then(function (r) { ddlsCarregados[ddl.id] = false; return r.json(); })
            .then(function (result) {
                ddl.options.length = 0;
                ddl.options.add(new Option("-- Selecione um recurso --", ""));
                var recursos = result.d || result;
                if (recursos && recursos.length > 0)
                    for (var i = 0; i < recursos.length; i++)
                        ddl.options.add(new Option(recursos[i].Descricao, recursos[i].Id));
                else
                    ddl.options.add(new Option("Nenhum recurso disponível", ""));
                ddl.disabled = false;
                ddlsCarregados[ddl.id] = false;
                ddl.focus();
            })
            .catch(function (err) {
                ddl.options.length = 0;
                ddl.options.add(new Option("Erro ao carregar", ""));
                ddl.disabled = false;
            });
        }

        function onChangeDDL(ddl) {
            if (ddl.value) {
                var hdnField = document.getElementById('ctl00_cphTitulo_hdnRecursoSelecionado');
                if (hdnField) {
                    hdnField.value = ddl.value;
                    __doPostBack(ddl.name, '');
                }
            }
        }

        function setupRecursosMenu() {
            document.addEventListener('click', function (e) {
                if (!e.target.closest('.recursos-list-simple li'))
                    document.querySelectorAll('.recurso-dropdown').forEach(function (d) { d.classList.remove('show'); });
            });

            document.querySelectorAll('.recursos-list-simple').forEach(function (lista) {
                var panel = lista.closest('[id*="pnRecursos"]');
                if (!panel) return;
                var templateDiv = panel.querySelector('.recursos-buttons-template');
                if (!templateDiv) return;

                var butDeletar = templateDiv.querySelector('.btn-action-delete');
                var butTransferir = templateDiv.querySelector('.btn-action-transfer');
                var butTrocar = templateDiv.querySelector('.btn-action-swap');
                if (!butDeletar || !butTransferir || !butTrocar) return;

                lista.querySelectorAll('li').forEach(function (li) {
                    if (li.querySelector('.recurso-menu-btn')) return;

                    var menuBtn = document.createElement('button');
                    menuBtn.className = 'recurso-menu-btn';
                    menuBtn.innerHTML = '⋮';
                    menuBtn.type = 'button';
                    menuBtn.title = 'Ações';

                    var dropdown = document.createElement('div');
                    dropdown.className = 'recurso-dropdown';

                    [
                        { btn: butDeletar,    icon: 'bi-trash',              texto: 'Liberar recurso',   classe: 'delete'   },
                        { btn: butTransferir, icon: 'bi-arrow-right-circle', texto: 'Transferir recurso', classe: 'transfer' },
                        { btn: butTrocar,     icon: 'bi-arrow-left-right',   texto: 'Trocar recurso',    classe: 'swap'     }
                    ].forEach(function (opcao, idx) {
                        var item = document.createElement('button');
                        item.className = 'recurso-dropdown-item ' + opcao.classe;
                        item.type = 'button';
                        item.innerHTML = '<i class="bi ' + opcao.icon + '"></i><span>' + opcao.texto + '</span>';
                        item.addEventListener('click', function (e) {
                            e.preventDefault(); e.stopPropagation();
                            dropdown.classList.remove('show');
                            var cb = li.querySelector('input[type="checkbox"]');
                            if (cb) {
                                lista.querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = false; });
                                cb.checked = true;
                            }
                            opcao.btn.click();
                        });
                        dropdown.appendChild(item);
                        if (idx === 0) {
                            var div = document.createElement('div');
                            div.className = 'recurso-dropdown-divider';
                            dropdown.appendChild(div);
                        }
                    });

                    menuBtn.addEventListener('click', function (e) {
                        e.preventDefault(); e.stopPropagation();
                        document.querySelectorAll('.recurso-dropdown').forEach(function (d) { if (d !== dropdown) d.classList.remove('show'); });
                        dropdown.classList.toggle('show');
                    });

                    li.appendChild(menuBtn);
                    li.appendChild(dropdown);
                });
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setupRecursosMenu);
        window.addEventListener('load', setupRecursosMenu);
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- ═══════════════════════════════════════
                 TÍTULO
            ═══════════════════════════════════════ -->
            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                <i class="bi bi-calendar-event me-2 text-primary"></i>
                <span class="fw-bold text-uppercase text-primary fs-6 mb-0">Detalhes do Evento</span>
                <asp:Label ID="lblTituloEvento" runat="server"
                    CssClass="ms-2 text-muted fw-normal" />
            </div>

            <asp:Label ID="lblResultado" runat="server"
                CssClass="d-block mb-3 fw-semibold" />

            <asp:HiddenField ID="hdnRecursoSelecionado" runat="server" />

            <!-- ═══════════════════════════════════════
                 GRID DE HORÁRIOS
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-3">
                <asp:DataGrid ID="dgHorariosEvento" runat="server"
                    AutoGenerateColumns="False"
                    Width="100%"
                    HorizontalAlign="Center"
                    OnItemDataBound="dgHorariosEvento_ItemDataBound"
                    DataKeyField="HorariosEventoId"
                    CssClass="table table-bordered table-hover table-sm align-middle">
                    <ItemStyle CssClass="align-middle text-center" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <AlternatingItemStyle CssClass="table-light" />
                    <Columns>

                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblHorariosEventoId" runat="server"
                                    Text='<%# Eval("HorariosEventoId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEventoId" runat="server"
                                    Text='<%# ((Evento)Eval("EventoId")).EventoId %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server"
                                    Text='<%# ((DateTime)Eval("Data")).ToShortDateString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblHorario" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Data / Horário">
                            <ItemTemplate>
                                <div class="text-muted small text-center" style="line-height:1.4;">
                                    <%# ((DateTime)Eval("Data")).ToString("dd/MM/yy") %>
                                    <%# Eval("HorarioInicio") %>
                                    —
                                    <%# Eval("HorarioFim") %>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="160px" VerticalAlign="Middle" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos Disponíveis">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDisponiveis" runat="server"
                                    CssClass="form-select form-select-sm"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlDisponiveis_SelectedIndexChanged"
                                    onmousedown="carregarRecursosDisponiveis(this)"
                                    onchange="if(this.value) onChangeDDL(this);">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Width="220px" VerticalAlign="Middle" />
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos Alocados">
                            <ItemTemplate>
                                <asp:Panel ID="pnRecursos" runat="server">
                                    <asp:CheckBoxList ID="cbRecursos" runat="server"
                                        CssClass="recursos-list-simple"
                                        RepeatLayout="UnorderedList">
                                    </asp:CheckBoxList>

                                    <div class="recursos-buttons-template" style="display:none;">
                                        <asp:LinkButton ID="butDeletar" runat="server"
                                            OnClick="butDeletar_Click"
                                            CssClass="btn-action-delete">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="butTransferir" runat="server"
                                            OnClick="butTransferir_Click"
                                            CssClass="btn-action-transfer">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="butTrocar" runat="server"
                                            OnClick="butTrocar_Click"
                                            CssClass="btn-action-swap">
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                            <ItemStyle Width="300px" VerticalAlign="Middle" />
                        </asp:TemplateColumn>

                    </Columns>
                </asp:DataGrid>
            </div>

            <!-- Voltar -->
            <div class="d-flex justify-content-start">
                <asp:LinkButton ID="lbtnVoltar" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    OnClick="lbtnVoltar_Click"
                    CausesValidation="False">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        .recursos-list-simple {
            padding-left: 0;
            list-style: none;
            margin-bottom: 0;
            width: 100%;
        }
        .recursos-list-simple li {
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            padding: 0.4rem 0.65rem;
            margin-bottom: 0.35rem;
            position: relative;
            gap: 0.5rem;
        }
        .recursos-list-simple li:last-child { margin-bottom: 0; }
        .recursos-list-simple li:hover { background: #e9ecef; }
        .recursos-list-simple input[type="checkbox"] { display: none; }
        .recursos-list-simple label { flex: 1; font-weight: 500; margin: 0; font-size: .85rem; }
        .recurso-menu-btn {
            background: none; border: none; cursor: pointer;
            padding: 0 4px; font-size: 1rem; color: #6c757d; line-height: 1;
        }
        .recurso-menu-btn:hover { color: #343a40; }
        .recurso-dropdown {
            display: none; position: absolute; right: 0; top: 100%;
            background: #fff; border: 1px solid #dee2e6; border-radius: .25rem;
            box-shadow: 0 4px 12px rgba(0,0,0,.15); z-index: 1000; min-width: 170px;
        }
        .recurso-dropdown.show { display: block; }
        .recurso-dropdown-item {
            display: flex; align-items: center; gap: .5rem;
            width: 100%; padding: .45rem .75rem; background: none;
            border: none; cursor: pointer; font-size: .875rem; text-align: left;
        }
        .recurso-dropdown-item:hover { background: #f8f9fa; }
        .recurso-dropdown-item.delete:hover { color: #dc3545; }
        .recurso-dropdown-item.transfer:hover { color: #0d6efd; }
        .recurso-dropdown-item.swap:hover { color: #198754; }
        .recurso-dropdown-divider { height: 1px; background: #dee2e6; margin: .25rem 0; }
    </style>

</asp:Content>
