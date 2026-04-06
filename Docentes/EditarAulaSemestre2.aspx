<%@ page language="C#"
    masterpagefile="~/Master/MasterBootstrap.master"
    autoeventwireup="true"
    inherits="Docentes_EditarAula"
    codefile="~/Docentes/EditarAulaSemestre2.aspx.cs"
    maintainscrollpositiononpostback="true"
    enableeventvalidation="false"
    title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

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
            var badge = $get('ctl00_cphTitulo_dgAulas_ctl' + num + '_butConfirm');
            if (badge) {
                badge.classList.add('active');
                badge.classList.remove('saved');
                badge.title = 'Clique para salvar todas as alterações';
                
                // Adiciona o evento de clique se ainda não existir
                if (!badge.hasAttribute('data-click-attached')) {
                    badge.setAttribute('data-click-attached', 'true');
                    badge.style.cursor = 'pointer';
                    badge.onclick = function(e) {
                        e.preventDefault();
                        e.stopPropagation();
                        // Dispara o clique no botão salvar
                        var btnSalvar = $get('ctl00_cphTitulo_btnSalvarTudo');
                        if (btnSalvar && !btnSalvar.disabled) {
                            btnSalvar.click();
                        }
                    };
                }
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
            textareas.forEach(function (textarea) {
                autoResize(textarea);
                textarea.addEventListener('input', function () {
                    autoResize(this);
                });
            });
        }

        // Executar ao carregar a página e após UpdatePanel refresh
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initAutoResize);
        window.addEventListener('load', initAutoResize);

        // Controle de dropdowns já carregados
        var ddlsCarregados = {};

        function carregarRecursosDisponiveis(ddl) {

            console.log("Carregando recursos...");
            // Verifica se já foi carregado para evitar chamadas repetidas
            if (ddlsCarregados[ddl.id]) {
                console.log("Recurso já carregado: ", ddl.id);
                return;
            }

            // Verifica se já tem itens além do placeholder
            if (ddl.options.length > 1) {
                ddlsCarregados[ddl.id] = true;
                console.log("ddl já com itens");
               return;
            }

            // Dispara o postback com comando personalizado
            //__doPostBack(ddl.id.replace(/_/g, '$'), 'CARREGAR_RECURSOS');
            //ddlsCarregados[ddl.id] = true;

            // Obtém a linha TR mais próxima
            var row = ddl.closest('tr');

            if (!row) {
                console.error('Linha não encontrada para o dropdown:', ddl.id);
                return;
            }

            var divDataHora = row.querySelector('div.text-muted.small');

            if (!divDataHora) {
                console.error('Div de data/hora não encontrado');
                return;
            }

            var notebook = document.getElementById('ctl00_cphTitulo_lblNotebook').innerHTML;
            console.log(notebook);

            var textoCompleto = (divDataHora.innerText || divDataHora.textContent).trim();
            //console.log('Texto completo:', textoCompleto);
            const partes = textoCompleto.split(' ');
            const data = partes[0];
            const hora = partes[partes.length - 1];
            console.log("Data: ", data, " Hora: ", hora, "Note: ", notebook);

            // Mostra indicador de carregamento
            ddl.disabled = true;
            ddl.options[0].text = "Carregando...";

            // Chamada AJAX usando fetch
            fetch('EditarAulaSemestre2.aspx/ObterRecursosDisponiveis', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify({ data: data, hora: hora, note: notebook })
            })
                .then(function (response) {
                    console.log("Response:", response);
                    ddlsCarregados[ddl.id] = false;
                    return response.json();
                })
                .then(function (result) {
                    // Limpa o dropdown
                    ddl.options.length = 0;

                    // Adiciona opção padrão
                    ddl.options.add(new Option("-- Selecione um recurso --", ""));

                    // Adiciona os recursos retornados
                    var recursos = result.d || result;
                    if (recursos && recursos.length > 0) {
                        for (var i = 0; i < recursos.length; i++) {
                            ddl.options.add(new Option(recursos[i].Descricao, recursos[i].Id));
                        }
                    } else {
                        ddl.options.add(new Option("Nenhum recurso disponível", ""));
                    }

                    // Reabilita e marca como não carregado
                    ddl.disabled = false;
                    ddlsCarregados[ddl.id] = false;

                    // NOVO: Adiciona handler para salvar valor no HiddenField antes do postback
                    ddl.addEventListener('change', function () {
                        if (this.value) {
                            var hdnField = document.getElementById('ctl00_cphTitulo_hdnRecursoSelecionado');
                            console.log("hdnfield: ", hdnField);
                            if (hdnField) {
                                hdnField.value = this.value;
                            }
                        }
                    });

                    // Abre o dropdown automaticamente
                    ddl.focus();
                    if (ddl.showPicker) {
                        try {
                            ddl.showPicker();
                        } catch (e) {
                            ddl.click();
                        }
                    }
                })
                .catch(function (error) {
                    console.error("Erro ao carregar recursos:", error);
                    ddl.options.length = 0;
                    ddl.options.add(new Option("Erro ao carregar", ""));
                    ddl.disabled = false;
                });
        }

        function onChangeDDL(ddl) {
            if (ddl.value) {
                var hdnField = document.getElementById('ctl00_cphTitulo_hdnRecursoSelecionado');
                console.log("hdnField: ", hdnField);
                if (hdnField) {
                    hdnField.value = ddl.value;
                    __doPostBack(ddl.name, '');
                }
            }
        }

        // Configuração do menu dropdown para recursos
        function setupRecursosMenu() {
            // Fecha todos os dropdowns ao clicar fora
            document.addEventListener('click', function(e) {
                if (!e.target.closest('.recursos-list-simple li')) {
                    document.querySelectorAll('.recurso-dropdown').forEach(function(dropdown) {
                        dropdown.classList.remove('show');
                    });
                }
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

                // Para cada item da lista
                lista.querySelectorAll('li').forEach(function (li, index) {
                    // Remove menu existente se houver
                    var existingMenu = li.querySelector('.recurso-menu-btn');
                    if (existingMenu) existingMenu.remove();
                    var existingDropdown = li.querySelector('.recurso-dropdown');
                    if (existingDropdown) existingDropdown.remove();

                    // Cria botão de menu (⋮)
                    var menuBtn = document.createElement('button');
                    menuBtn.className = 'recurso-menu-btn';
                    menuBtn.innerHTML = '⋮';
                    menuBtn.type = 'button';
                    menuBtn.title = 'Ações';

                    // Cria dropdown menu
                    var dropdown = document.createElement('div');
                    dropdown.className = 'recurso-dropdown';

                    // Opções do menu
                    var opcoes = [
                        { 
                            btn: butDeletar, 
                            icon: 'bi-trash', 
                            texto: 'Liberar recurso',
                            classe: 'delete'
                        },
                        { 
                            btn: butTransferir, 
                            icon: 'bi-arrow-right-circle', 
                            texto: 'Transferir recurso',
                            classe: 'transfer'
                        },
                        { 
                            btn: butTrocar, 
                            icon: 'bi-arrow-left-right', 
                            texto: 'Trocar recurso',
                            classe: 'swap'
                        }
                    ];

                    opcoes.forEach(function(opcao, idx) {
                        var item = document.createElement('button');
                        item.className = 'recurso-dropdown-item ' + opcao.classe;
                        item.type = 'button';
                        item.innerHTML = '<i class="bi ' + opcao.icon + '"></i><span>' + opcao.texto + '</span>';
                        
                        item.addEventListener('click', function(e) {
                            e.preventDefault();
                            e.stopPropagation();
                            
                            // Fecha o dropdown
                            dropdown.classList.remove('show');
                            
                            // Marca o checkbox correspondente
                            var checkbox = li.querySelector('input[type="checkbox"]');
                            if (checkbox) {
                                // Desmarca todos primeiro
                                lista.querySelectorAll('input[type="checkbox"]').forEach(function(cb) {
                                    cb.checked = false;
                                });
                                // Marca apenas este
                                checkbox.checked = true;
                            }
                            
                            // Dispara o click no botão original
                            opcao.btn.click();
                        });

                        dropdown.appendChild(item);
                        
                        // Adiciona separador entre Transferir e Trocar
                        if (idx === 0) {
                            var divider = document.createElement('div');
                            divider.className = 'recurso-dropdown-divider';
                            dropdown.appendChild(divider);
                        }
                    });

                    // Toggle dropdown ao clicar no botão
                    menuBtn.addEventListener('click', function(e) {
                        e.preventDefault();
                        e.stopPropagation();
                        
                        // Fecha outros dropdowns
                        document.querySelectorAll('.recurso-dropdown').forEach(function(d) {
                            if (d !== dropdown) {
                                d.classList.remove('show');
                            }
                        });
                        
                        // Toggle este dropdown
                        dropdown.classList.toggle('show');
                    });

                    li.appendChild(menuBtn);
                    li.appendChild(dropdown);
                });
            });
        }

        // Executa após carregamento e após postbacks
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setupRecursosMenu);
        window.addEventListener('load', setupRecursosMenu);

        function resetConfirmBadges() {
            document.querySelectorAll('.confirm-badge.active').forEach(function(badge) {
                badge.classList.remove('active');
                badge.classList.add('saved');
                badge.title = 'Alteração salva';
                badge.style.cursor = 'default';
                badge.removeAttribute('data-click-attached');
                badge.onclick = null;
                
                // Remove o estado 'saved' após 2 segundos
                setTimeout(function() {
                    badge.classList.remove('saved');
                    badge.title = '';
                }, 2000);
            });
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
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
                            ToolTip="Faz download de um arquivo CSV com o cronograma para o sistsema de atas"
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

            <asp:Label ID="lblNotebook" runat="server"
                Style="display: none"
                Text=" "/>

            <asp:HiddenField ID="hdnRecursoSelecionado" runat="server" />

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
                                    <div class="text-muted small">
                                        <%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yy")%>
                                        <%#(DataHelper.GetDiaPUCRS((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>
                                        <%#DataBinder.Eval(Container.DataItem, "Hora") %>
                                    </div>
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
                                        Style="resize: none; overflow: hidden;"
                                        CssClass="form-control form-control-sm auto-resize-textarea"
                                        Rows="1"
                                        Width="100%"
                                        TextMode="MultiLine"
                                        Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'
                                        AutoPostBack="False" />
                                    <span ID="butConfirm"
                                        runat="server"
                                        class="confirm-badge">
                                        <i class="bi bi-exclamation-circle-fill"></i>
                                    </span>
                                    <asp:CheckBox ID="cbChanged"
                                        Style="display: none"
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
                                    OnSelectedIndexChanged="ddlDisponiveis_SelectedIndexChanged"
                                    onmousedown="carregarRecursosDisponiveis(this)"
                                    onchange="if(this.value) onChangeDDL(this);">
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
                                    <asp:CheckBoxList ID="cbRecursos" runat="server"
                                        CssClass="recursos-list-simple"
                                        RepeatLayout="UnorderedList">
                                    </asp:CheckBoxList>
                                    
                                    <div class="recursos-buttons-template" style="display: none;">
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
                                    
                                    <asp:Label ID="lblRecursosAlocados" runat="server"
                                        Width="250px"
                                        Visible="false" />
                                </asp:Panel>
                            </ItemTemplate>
                            <ItemStyle Width="350px" VerticalAlign="Middle" />
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
           CONTAINER DE DESCRIÇÃO COM BADGE
        ═══════════════════════════════════════ */
        .descricao-container {
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

            .descricao-container .form-control {
                flex: 1;
            }

        /* Badge de confirmação moderno */
        .confirm-badge {
            flex-shrink: 0;
            width: 24px;
            height: 24px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            background: #ffc107;
            color: white;
            font-size: 1rem;
            transition: all 0.3s;
            opacity: 0;
            transform: scale(0.8);
            cursor: default;
        }

            /* Estado ativo (visível e clicável) */
            .confirm-badge.active {
                opacity: 1;
                transform: scale(1);
                animation: pulse-warning 2s infinite;
                cursor: pointer;
            }

            .confirm-badge:hover.active {
                background: #e0a800;
                transform: scale(1.15);
                animation: none;
            }

            /* Estado desabilitado (salvo) */
            .confirm-badge.saved {
                background: #28a745;
                opacity: 0.6;
                cursor: default;
                animation: none;
            }

                .confirm-badge.saved i::before {
                    content: "\f26b"; /* bi-check-circle-fill */
                }

        /* Animação de pulso */
        @keyframes pulse-warning {
            0%, 100% {
                box-shadow: 0 0 0 0 rgba(255, 193, 7, 0.7);
            }
            50% {
                box-shadow: 0 0 0 8px rgba(255, 193, 7, 0);
            }
        }

        /* ═══════════════════════════════════════
           LISTA DE RECURSOS COM MENU DROPDOWN
        ═══════════════════════════════════════ */
        .recursos-list-simple {
            padding-left: 0;
            list-style: none;
            margin-bottom: 0;
            width: 100%;
        }

            /* Cada item da lista */
            .recursos-list-simple li {
                display: flex;
                align-items: center;
                justify-content: space-between;
                background: #f8f9fa;
                border: 1px solid #dee2e6;
                border-radius: 0.25rem;
                padding: 0.5rem 0.75rem;
                margin-bottom: 0.5rem;
                transition: all 0.2s;
                gap: 0.75rem;
                position: relative;
            }

                .recursos-list-simple li:last-child {
                    margin-bottom: 0;
                }

                .recursos-list-simple li:hover {
                    background: #e9ecef;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }

            /* Esconde os checkboxes */
            .recursos-list-simple input[type="checkbox"] {
                display: none;
            }

            /* Label do recurso - ALINHADO À ESQUERDA */
            .recursos-list-simple label {
                flex: 1;
                font-weight: 500;
                color: #495057;
                font-size: 0.875rem;
                margin-bottom: 0;
                cursor: default;
                display: block;
                text-align: left;
                padding-right: 0.5rem;
            }

        /* Esconde template de botões */
        .recursos-buttons-template {
            display: none !important;
        }

        /* ═══════════════════════════════════════
           BOTÃO DE MENU E DROPDOWN
        ═══════════════════════════════════════ */
        .recurso-menu-btn {
            padding: 0.25rem 0.5rem;
            background: white;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            cursor: pointer;
            transition: all 0.2s;
            color: #6c757d;
            font-size: 1.1rem;
            line-height: 1;
            flex-shrink: 0;
        }

            .recurso-menu-btn:hover {
                background: #e9ecef;
                color: #495057;
                border-color: #adb5bd;
            }

            .recurso-menu-btn:active {
                transform: scale(0.95);
            }

        /* Dropdown menu */
        .recurso-dropdown {
            position: absolute;
            right: 0;
            top: 100%;
            margin-top: 0.25rem;
            background: white;
            border: 1px solid #dee2e6;
            border-radius: 0.375rem;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            min-width: 160px;
            z-index: 1000;
            display: none;
            overflow: hidden;
        }

            .recurso-dropdown.show {
                display: block;
            }

        .recurso-dropdown-item {
            display: flex;
            align-items: center;
            gap: 0.75rem;
            padding: 0.5rem 1rem;
            color: #212529;
            text-decoration: none;
            cursor: pointer;
            transition: background 0.15s;
            border: none;
            background: none;
            width: 100%;
            text-align: left;
            font-size: 0.875rem;
        }

            .recurso-dropdown-item:hover {
                background: #f8f9fa;
            }

            .recurso-dropdown-item i {
                font-size: 1rem;
                width: 1.25rem;
                text-align: center;
            }

            .recurso-dropdown-item.delete {
                color: #dc3545;
            }

                .recurso-dropdown-item.delete:hover {
                    background: #fff5f5;
                }

            .recurso-dropdown-item.transfer {
                color: #0d6efd;
            }

                .recurso-dropdown-item.transfer:hover {
                    background: #f0f5ff;
                }

            .recurso-dropdown-item.swap {
                color: #6c757d;
            }

                .recurso-dropdown-item.swap:hover {
                    background: #f8f9fa;
                }

        /* Separador entre itens do menu */
        .recurso-dropdown-divider {
            height: 1px;
            background: #dee2e6;
            margin: 0.25rem 0;
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
            .recurso-dropdown {
                right: auto;
                left: 0;
            }
        }

        /* ═══════════════════════════════════════
           LOADING OVERLAY MODERNO
        ═══════════════════════════════════════ */
        #modernProgressOverlay {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.6);
            backdrop-filter: blur(4px);
            z-index: 9998;
            display: flex;
            align-items: center;
            justify-content: center;
            animation: fadeIn 0.2s ease-in;
        }

        #modernProgressMessage {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 9999;
            animation: slideIn 0.3s ease-out;
        }

        .modern-spinner-container {
            background: white;
            border-radius: 1rem;
            padding: 2.5rem 3rem;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            text-align: center;
            min-width: 280px;
        }

            .modern-spinner-container .spinner-border {
                width: 3.5rem;
                height: 3.5rem;
                border-width: 0.35rem;
            }

        @keyframes fadeIn {
            from {
                opacity: 0;
            }
            to {
                opacity: 1;
            }
        }

        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translate(-50%, -45%);
            }
            to {
                opacity: 1;
                transform: translate(-50%, -50%);
            }
        }

        /* Remove os estilos antigos se existirem */
        #progressBackgroundFilter,
        #processMessage {
            display: none !important;
        }
    </style>

</asp:Content>
