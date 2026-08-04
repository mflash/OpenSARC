<%@ Page Language="C#"
    MasterPageFile="~/Master/MasterBootstrap.master"
    AutoEventWireup="true"
    Inherits="Docentes_EditarAula"
    CodeFile="~/Docentes/EditarAulaSemestre2.aspx.cs"
    MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false"
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../UserControls/ModernProgress.ascx" TagName="ModernProgress" TagPrefix="uc" %>
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
            var b = document.getElementById('ctl00_cphTitulo_btnSalvarTudo');
            if (b) {
                b.value = "Salvar Agora";
                b.disabled = false;
            }
            var b2 = document.getElementById('ctl00_cphTitulo_btnSalvarTudo2');
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
                    badge.onclick = function (e) {
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
            //var textareas = Array.prototype.slice.call(document.querySelectorAll('.auto-resize-textarea'));
            var textareas = document.querySelectorAll('.auto-resize-textarea');
            var taArray = Array.prototype.slice.call(textareas);

            taArray.forEach(function (ta) {
                ta.style.height = 'auto';
            });

            var heights = taArray.map(function (ta) {
                return ta.scrollHeight;
            });

            taArray.forEach(function (ta, i) {
                ta.style.height = heights[i] + 'px';
            });
            /*
            // Read phase: collect all scrollHeights in one pass (single reflow).
            var heights = textareas.map(function (ta) {
                ta.style.height = 'auto';
                return ta.scrollHeight;
            });
            // Write phase: apply all heights without interleaving reads.
            textareas.forEach(function (ta, i) {
                ta.style.height = heights[i] + 'px';
                if (!ta.hasAttribute('data-autoresize')) {
                    ta.setAttribute('data-autoresize', 'true');
                    ta.addEventListener('input', function () { autoResize(this); });
                }
            });
            */
        }

        document.addEventListener('input', function (e) {
            if (e.target && e.target.classList.contains('auto-resize-textarea')) {
                e.target.style.height = 'auto';
                e.target.style.height = e.target.scrollHeight + 'px';
            }
        });

        function debounce(func, wait) {
            var timeout;
            return function () {
                var context = this, args = arguments;
                clearTimeout(timeout);
                timeout = setTimeout(function () {
                    func.apply(context, args);
                }, wait);
            };
        }

        /*
        document.addEventListener('keyup', debounce(function (e) {
            if (e.target && e.target.classList.contains('auto-resize-textarea')) {
                // Extract row identifier index from element ID
                var match = e.target.id.match(/_ctl(\d+)_txtDescricao/);
                console.log("keyup: " + e.target.id);
                if (match && match[1]) {
                    console.log("match: " + match);
                    testAlert(e.target, match[1]);
                }
            }
        }, 300));
        */
        document.addEventListener('keyup', debounce(function (e) {
            if (e.target && e.target.classList.contains('auto-resize-textarea')) {
                var txt = e.target;
                txt.style.color = '#FF0000';

                var row = txt.closest('tr');
                if (row) {
                    var cb = row.querySelector('input[type="checkbox"][id*="cbChanged"]');
                    if (cb) cb.checked = true;

                    var badge = row.querySelector('.confirm-badge');
                    if (badge) {
                        badge.classList.add('active');
                        badge.classList.remove('saved');
                        badge.title = 'Clique para salvar todas as alterações';

                        if (!badge.hasAttribute('data-click-attached')) {
                            badge.setAttribute('data-click-attached', 'true');
                            badge.style.cursor = 'pointer';
                            badge.addEventListener('click', function (evt) {
                                evt.preventDefault();
                                evt.stopPropagation();
                                var btnSalvar = document.getElementById('ctl00_cphTitulo_btnSalvarTudo');
                                if (btnSalvar && !btnSalvar.disabled) {
                                    btnSalvar.click();
                                }
                            });
                        }
                    }
                }
                setDirtyFlag();
            }
        }, 300));

        // Executar ao carregar a página e após UpdatePanel refresh
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initAutoResize);
        //window.addEventListener('load', initAutoResize);

        // Controle de dropdowns já carregados
        var ddlsCarregados = {};

        // Controle de dropdowns em carregamento (evita chamadas simultâneas, mas permite recarregar)
        var ddlsCarregando = {};

        function carregarRecursosDisponiveis(ddl) {

            // Already loaded — let the browser open natively with cached options
            if (ddlsCarregados[ddl.id]) {
                return;
            }

            // Already fetching — don't start another request
            if (ddlsCarregando[ddl.id]) {
                return;
            }

            var row = ddl.closest('tr');
            if (!row) return;

            var divDataHora = row.querySelector('div.text-muted.small');
            if (!divDataHora) return;

            var notebook = document.getElementById('ctl00_cphTitulo_lblNotebook').innerHTML;
            var textoCompleto = (divDataHora.innerText || divDataHora.textContent).trim();
            const partes = textoCompleto.split(' ');
            const data = partes[0];
            const hora = partes[partes.length - 1];
            // Mark as loading
            // cancels the native dropdown open in Firefox, and showPicker() on <select>
            // is unsupported there so it cannot be recovered programmatically.
            // The user will see "Carregando..." on the first click; the second click
            // (after the fetch resolves) will open with the real options.
            ddlsCarregando[ddl.id] = true;
            ddl.options.length = 0;
            ddl.options.add(new Option("Carregando...", ""));

            fetch('EditarAulaSemestre.aspx/ObterRecursosDisponiveis', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify({ data: data, hora: hora, note: notebook })
            })
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    ddl.options.length = 0;
                    ddl.options.add(new Option("-- Selecione um recurso --", ""));

                    var row = ddl.closest('tr');
                    var temRetirarNotebook = false;
                    if (row) {
                        var labels = row.querySelectorAll('.recursos-list-simple label');
                        labels.forEach(function (lbl) {
                            if ((lbl.innerText || lbl.textContent).trim().startsWith("Retirar")) {
                                temRetirarNotebook = true;
                            }
                        });
                    }

                    var recursos = result.d || result;
                    if (recursos && recursos.length > 0) {
                        for (var i = 0; i < recursos.length; i++) {
                            if (temRetirarNotebook && recursos[i].Descricao.startsWith("Retirar")) {
                                continue;
                            }
                            ddl.options.add(new Option(recursos[i].Descricao, recursos[i].Id));
                        }
                    } else {
                        ddl.options.add(new Option("Nenhum recurso disponível", ""));
                    }

                    ddlsCarregando[ddl.id] = false;
                    ddlsCarregados[ddl.id] = true;
                })
                .catch(function (error) {
                    ddl.options.length = 0;
                    ddl.options.add(new Option("Erro ao carregar", ""));
                    ddlsCarregando[ddl.id] = false;
                    // Do not set ddlsCarregados — allow retry on next mousedown
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

        // Listener para chamada async à atualização apenas deste dropdown
        /*
        document.addEventListener('change', function(e) {
            if (e.target && e.target.classList.contains('form-select')) {
                const dropdown = e.target;
                const selectedValue = dropdown.value;
                const rowId = dropdown.id; 

                if (!selectedValue) return;

                dropdown.disabled = true;

                fetch('EditarAulaSemestre.aspx/UpdateDropdownState', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    },
                    body: JSON.stringify({ rowId: rowId, selectedValue: selectedValue })
                })
                .then(response => response.json())
                .then(data => {
                    dropdown.disabled = false;
                })
                .catch(error => {
                    dropdown.disabled = false;
                    dropdown.style.border = "1px solid red";
                });
            }
        });
        */

        document.addEventListener('change', function (e) {
            if (!e.target || !e.target.classList.contains('form-select')) return;

            var dropdown = e.target;
            var selectedValue = dropdown.value;
            if (!selectedValue) return;

            var isAtividade = dropdown.id.indexOf('ddlAtividade') > -1;
            var tipo = isAtividade ? 'atividade' : 'recurso';

            var prefixMatch = dropdown.id.match(/(.*_ctl\d+_)/);
            if (!prefixMatch) return;
            var prefix = prefixMatch[1];

            // Extract embedded data attributes
            var aulaId = dropdown.getAttribute('data-aula-id');
            var dataAula = dropdown.getAttribute('data-data');
            var horaAula = dropdown.getAttribute('data-hora');

            dropdown.disabled = true;

            fetch('EditarAulaSemestre.aspx/UpdateDropdownState', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({
                    tipo: tipo,
                    aulaId: aulaId,
                    selectedValue: selectedValue,
                    dataString: dataAula,
                    horario: horaAula
                })
            })
                .then(function (response) { return response.json(); })
                .then(function (data) {
                    dropdown.disabled = false;

                    if (data.d && data.d.status === 'success') {
                        if (tipo === 'recurso') {
                            var panel = document.getElementById(prefix + 'pnRecursos');
                            var ul = panel.querySelector('.recursos-list-simple');

                            if (!ul) {
                                ul = document.createElement('ul');
                                ul.className = 'recursos-list-simple';
                                ul.id = prefix + 'cbRecursos';
                                panel.insertBefore(ul, panel.querySelector('.recursos-buttons-template'));
                            }

                            var li = document.createElement('li');
                            li.setAttribute('data-value', data.d.value);
                            li.innerHTML = '<input type="checkbox" value="' + data.d.value + '"><label>' + data.d.text + '</label>';
                            ul.appendChild(li);

                            var template = panel.querySelector('.recursos-buttons-template');
                            if (template) template.style.display = 'block';

                            if (typeof setupRecursosMenu === 'function') setupRecursosMenu();
                            dropdown.selectedIndex = 0;
                        } else {
                            var badge = document.getElementById(prefix + 'butConfirm');
                            if (badge) {
                                badge.classList.add('saved');
                                badge.classList.remove('active');
                                setTimeout(function () { badge.classList.remove('saved'); }, 2000);
                            }
                        }
                    } else {
                        dropdown.style.border = '1px solid red';
                    }
                })
                .catch(function (error) {
                    dropdown.disabled = false;
                    dropdown.style.border = '1px solid red';
                });
        });

        var deleteBtnTemplate = document.createElement('button');
        deleteBtnTemplate.className = 'recurso-delete-btn';
        deleteBtnTemplate.type = 'button';
        deleteBtnTemplate.title = 'Liberar recurso';
        deleteBtnTemplate.innerHTML = '<i class="bi bi-trash"></i>';


        function setupRecursosMenu() {
            var lists = document.querySelectorAll('.recursos-list-simple');
            for (var i = 0; i < lists.length; i++) {
                var items = lists[i].querySelectorAll('li');
                for (var j = 0; j < items.length; j++) {
                    if (!items[j].hasAttribute('data-btn-initialized')) {
                        items[j].appendChild(deleteBtnTemplate.cloneNode(true));
                        items[j].setAttribute('data-btn-initialized', 'true');
                    }
                }
            }
        }

        document.addEventListener('click', function (e) {
            var btn = e.target.closest('.recurso-delete-btn');
            if (!btn) return;

            e.preventDefault();
            e.stopPropagation();

            var li = btn.closest('li');
            var lista = li.closest('.recursos-list-simple');
            var panel = lista.closest('[id*="pnRecursos"]');

            if (!li || !panel) return;

            // Traverse to locate the ASP.NET injected span containing the attribute
            var targetNode = li.hasAttribute('data-value') ? li : li.querySelector('[data-value]');
            var recursoId = targetNode ? targetNode.getAttribute('data-value') : null;

            // Fallback for dynamically injected nodes where value might be directly on the checkbox
            if (!recursoId || recursoId === 'on') {
                var checkbox = li.querySelector('input[type="checkbox"]');
                if (checkbox && checkbox.value !== 'on') {
                    recursoId = checkbox.value;
                }
            }
            //console.log("Recursoid: " + recursoId);

            var prefixMatch = panel.id.match(/(.*_ctl\d+_)/);
            if (!prefixMatch) return;
            var prefix = prefixMatch[1];

            // Read attributes from the adjacent initialized dropdown
            var ddl = document.getElementById(prefix + 'ddlDisponiveis');
            if (!ddl) return;

            var aulaId = ddl.getAttribute('data-aula-id');
            var dataAula = ddl.getAttribute('data-data');
            var horaAula = ddl.getAttribute('data-hora');

            btn.disabled = true;
            btn.innerHTML = '<i class="bi bi-hourglass"></i>';

            fetch('EditarAulaSemestre.aspx/UpdateDropdownState', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({
                    tipo: 'delete',
                    aulaId: aulaId,
                    selectedValue: recursoId,
                    dataString: dataAula,
                    horario: horaAula
                })
            })
                .then(function (response) { return response.json(); })
                .then(function (data) {
                    if (data.d && data.d.status === 'success') {
                        li.remove();

                        if (lista.children.length === 0) {
                            var template = panel.querySelector('.recursos-buttons-template');
                            if (template) template.style.display = 'none';
                        }
                    } else {
                        btn.disabled = false;
                        btn.innerHTML = '<i class="bi bi-trash text-danger"></i>';
                    }
                })
                .catch(function (error) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="bi bi-trash text-danger"></i>';
                });
        });

        /*
        function setupRecursosMenu() {
            document.querySelectorAll('.recursos-list-simple').forEach(function (lista) {
                var panel = lista.closest('[id*="pnRecursos"]');
                if (!panel) return;

                var templateDiv = panel.querySelector('.recursos-buttons-template');
                if (!templateDiv) return;

                var butDeletar = templateDiv.querySelector('.btn-action-delete');
                if (!butDeletar) return;

                lista.querySelectorAll('li').forEach(function (li) {
                    // Remove botão existente se houver
                    var existingBtn = li.querySelector('.recurso-delete-btn');
                    if (existingBtn) existingBtn.remove();

                    // Cria botão de lixeira
                    var deleteBtn = document.createElement('button');
                    deleteBtn.className = 'recurso-delete-btn';
                    deleteBtn.type = 'button';
                    deleteBtn.title = 'Liberar recurso';
                    deleteBtn.innerHTML = '<i class="bi bi-trash"></i>';

                    deleteBtn.addEventListener('click', function (e) {
                        e.preventDefault();
                        e.stopPropagation();

                        // Marca o checkbox correspondente
                        var checkbox = li.querySelector('input[type="checkbox"]');
                        if (checkbox) {
                            lista.querySelectorAll('input[type="checkbox"]').forEach(function (cb) {
                                cb.checked = false;
                            });
                            checkbox.checked = true;
                        }

                        // Dispara o click no botão deletar original
                        butDeletar.click();
                    });

                    li.appendChild(deleteBtn);
                });
            });
        }
        */

        // Tratamento do Salvar Todos — envia todas as alterações de uma vez, sem precisar clicar em cada badge individualmente
        document.addEventListener('DOMContentLoaded', function () {
            var btn1 = document.getElementById('ctl00_cphTitulo_btnSalvarTudo');
            var btn2 = document.getElementById('ctl00_cphTitulo_btnSalvarTudo2');

            function executeBulkSave(e) {
                e.preventDefault();

                var modifiedRows = document.querySelectorAll('.confirm-badge.active');
                if (modifiedRows.length === 0) return;

                var payload = [];

                for (var i = 0; i < modifiedRows.length; i++) {
                    var row = modifiedRows[i].closest('tr');
                    var ddlAtividade = row.querySelector('select[id*="ddlAtividade"]');
                    var txtDescricao = row.querySelector('.auto-resize-textarea');

                    if (!ddlAtividade || !txtDescricao) continue;

                    var rawText = txtDescricao.value;
                    var splitIndex = rawText.indexOf('\n');
                    var cleanDesc = splitIndex !== -1 ? rawText.substring(splitIndex + 1) : rawText;

                    payload.push({
                        AulaId: ddlAtividade.getAttribute('data-aula-id'),
                        Data: ddlAtividade.getAttribute('data-data'),
                        Hora: ddlAtividade.getAttribute('data-hora'),
                        AtividadeId: ddlAtividade.value,
                        Descricao: cleanDesc
                    });
                }

                if (btn1) btn1.disabled = true;
                if (btn2) btn2.disabled = true;

                fetch('EditarAulaSemestre.aspx/SalvarAulas', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify({ aulas: payload })
                })
                    .then(function (res) { return res.json(); })
                    .then(function (data) {
                        if (data.d && data.d.status === 'success') {
                            for (var j = 0; j < modifiedRows.length; j++) {
                                var txtNode = modifiedRows[j].closest('tr').querySelector('.auto-resize-textarea');
                                if (txtNode) txtNode.style.color = '';
                            }

                            if (typeof resetConfirmBadges === 'function') resetConfirmBadges();
                            if (btn1) btn1.value = "Salvo";
                            if (btn2) btn2.value = "Salvar Todos";
                            if (typeof releaseDirtyFlag === 'function') releaseDirtyFlag();
                        } else {
                            showBootstrapAlert('Falha na sincronização: ' + (data.d ? data.d.message : 'Unknown error'));
                            if (btn1) btn1.disabled = false;
                            if (btn2) btn2.disabled = false;
                        }
                    })
                    .catch(function (err) {
                        showBootstrapAlert('Erro de rede.');
                        if (btn1) btn1.disabled = false;
                        if (btn2) btn2.disabled = false;
                    });
            }

            if (btn1) btn1.addEventListener('click', executeBulkSave);
            if (btn2) btn2.addEventListener('click', executeBulkSave);
        });

        // Pre-load all resource dropdowns on page load and after every UpdatePanel refresh,
        // so the first click is always instant.
        function preCarregarTodosDropdowns() {
            document.querySelectorAll('select[id*="ddlDisponiveis"]').forEach(function (ddl) {
                carregarRecursosDisponiveis(ddl);
            });
        }

        window.addEventListener('load', function () {
            initAutoResize();
            setupRecursosMenu();
            //preCarregarTodosDropdowns();
        });

        // Executa após carregamento e após postbacks
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setupRecursosMenu);
        //window.addEventListener('load', setupRecursosMenu);

        function resetConfirmBadges() {
            document.querySelectorAll('.confirm-badge.active').forEach(function (badge) {
                badge.classList.remove('active');
                badge.classList.add('saved');
                badge.title = 'Alteração salva';
                badge.style.cursor = 'default';
                badge.removeAttribute('data-click-attached');
                badge.onclick = null;

                // Remove o estado 'saved' após 2 segundos
                setTimeout(function () {
                    badge.classList.remove('saved');
                    badge.title = '';
                }, 2000);
            });
        }

        // Quando o usuário seleciona um arquivo, dispara o submit automaticamente
        document.addEventListener('DOMContentLoaded', function () {
            var fileInput = document.getElementById('ctl00_cphTitulo_csvUpload');
            if (fileInput) {
                fileInput.addEventListener('change', function () {
                    if (this.files.length > 0) {
                        // Show the same progress overlay used by async postbacks
                        var overlay = document.getElementById('modernProgressOverlay');
                        var message = document.getElementById('modernProgressMessage');
                        if (overlay) overlay.style.display = 'flex';
                        if (message) message.style.display = 'block';
                        document.getElementById('ctl00_cphTitulo_btnImportarCSVSubmit').click();
                    }
                });
            }
        });

        // Alerta usando bootstrap
        function showBootstrapAlert(message) {
            var html = '<div class="alert alert-danger alert-dismissible fade show" role="alert">'
                + message
                + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>'
                + '</div>';
            document.getElementById('alertContainer').innerHTML = html;
        }

    </script>

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

                        <span class="text-muted fw-semibold me-1">
                            <i class="bi bi-download me-1"></i>Importar:
                        </span>

                        <asp:Button ID="btnImportarCSV" runat="server"
                            OnClick="btnImportarCSV_Click"
                            ToolTip="Importa cronograma a partir do CSV do sistema de atas"
                            CssClass="btn btn-sm btn-outline-secondary"
                            Text="CSV/Atas"
                            OnClientClick="document.getElementById('ctl00_cphTitulo_csvUpload').click(); return false;" />

                        <asp:FileUpload ID="csvUpload" runat="server" Style="display: none" />

                        <%-- Botão oculto que dispara o postback completo após a seleção do ficheiro --%>
                        <asp:Button ID="btnImportarCSVSubmit" runat="server"
                            OnClick="btnImportarCSV_Click"
                            Style="display: none;" />

                        <!-- Separador visual -->
                        <div class="vr mx-1" style="height: 24px;"></div>

                        <!-- Botão Salvar -->
                        <asp:Button ID="btnSalvarTudo" runat="server"
                            CssClass="btn btn-primary btn-sm"
                            Text="Salvo"
                            UseSubmitBehavior="false"
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
                Text=" " />

            <asp:HiddenField ID="hdnRecursoSelecionado" runat="server" />

            <div id="alertContainer"></div>

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
                    EnableViewState="true"
                    CssClass="table table-bordered table-hover table-sm align-middle">

                    <ItemStyle CssClass="align-middle text-center" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <AlternatingItemStyle CssClass="table-light align-middle text-center" />

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
                                    <span id="butConfirm"
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
                            <ItemStyle Width="250px" />
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
                                    AutoPostBack="False"
                                    OnSelectedIndexChanged="ddlDisponiveis_SelectedIndexChanged">
                                </asp:DropDownList>
                                <!--onchange="if(this.value) onChangeDDL(this);"-->
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

                        <asp:TemplateColumn HeaderText="CorDaDataExport" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCorExport" runat="server" />
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
                    UseSubmitBehavior="false"
                    Enabled="False" />
            </div>


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

        /*
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

        .recurso-dropdown-divider {
            height: 1px;
            background: #dee2e6;
            margin: 0.25rem 0;
        }
        */

        /* ═══════════════════════════════════════
           BOTÃO DE LIXEIRA DO RECURSO
        ═══════════════════════════════════════ */
        .recurso-delete-btn {
            padding: 0.2rem 0.45rem;
            background: white;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            cursor: pointer;
            transition: all 0.2s;
            color: #dc3545;
            font-size: 0.875rem;
            line-height: 1;
            flex-shrink: 0;
        }

            .recurso-delete-btn:hover {
                background: #fff5f5;
                border-color: #dc3545;
                color: #b02a37;
            }

            .recurso-delete-btn:active {
                transform: scale(0.95);
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
