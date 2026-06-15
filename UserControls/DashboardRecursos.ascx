<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DashboardRecursos.ascx.cs" Inherits="UserControls_DashboardRecursos" %>
<asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick" Enabled="false" />

<style>
    .schedule-card {
        border: none;
        border-radius: 12px;
    }

    .schedule-header {
        background-color: #f8f9fa;
        border-bottom: 2px solid #dee2e6;
        padding: 1rem;
        font-weight: 700;
        font-size: medium;
    }

    .schedule-col {
        max-width: 100%;
    }

    .resource-container {
        display: flex;
        align-items: center;
    }

    .resource-icon {
        font-size: 1.3rem;
        margin-right: 10px;
        color: #6c757d;
    }

    .resource-tag {
        font-size: 1.7rem !important;
        padding: 0.3rem 1rem !important;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        font-family: sans-serif;
        min-width: 9vw;
    }

    .badge-active {
        background-color: #dc3545 !important;
        animation: pulse-red 2s infinite;
        border: 1px solid rgba(255, 255, 255, 0.4);
    }

    @keyframes pulse-red {
        0% {
            box-shadow: 0 0 0 0 rgba(220, 53, 69, 0.7);
        }

        70% {
            box-shadow: 0 0 0 10px rgba(220, 53, 69, 0);
        }

        100% {
            box-shadow: 0 0 0 0 rgba(220, 53, 69, 0);
        }
    }

    .status-indicator {
        height: 8px;
        width: 8px;
        background-color: #fff;
        border-radius: 50%;
        display: inline-block;
        margin-right: 6px;
        vertical-align: middle;
    }

    .resource-icon.text-danger {
        color: #dc3545 !important;
    }

    .badge-available {
        background-color: #198754 !important;
        border: 1px solid rgba(255, 255, 255, 0.2);
    }

    .badge-maintenance {
        background-color: #ffc107 !important;
        color: #000 !important;
        border: 1px solid rgba(0, 0, 0, 0.1);
    }

    .text-maintenance {
        color: #856404 !important;
    }

    .text-available {
        color: #198754 !important;
    }

    .nomedisc {
        font-size: medium;
    }

    .list-group-item.highlight {
        background-color: #fff3cd;
        transition: background-color 0.2s;
    }

    .list-group-item.highlight-same {
        background-color: #d1e7dd;
        transition: background-color 0.2s;
    }

    .list-group-item.dimmed {
        opacity: 0.25;
        transition: opacity 0.2s;
    }

    .list-group-item.highlight,
    .list-group-item.highlight-same {
        transition: background-color 0.2s, opacity 0.2s;
    }

    #indicadorBusca {
        position: fixed;
        bottom: 1.5rem;
        left: 50%;
        transform: translateX(-50%);
        background: rgba(0, 0, 0, 0.75);
        color: white;
        padding: 0.4rem 1rem;
        border-radius: 999px;
        font-size: 1rem;
        font-family: monospace;
        pointer-events: none;
        z-index: 9999;
        display: none;
    }

    /* Category Colors */
    .lab {
        background-color: #97c7a6;
    }

    .notebook {
        background-color: #6e90b0;
    }

    .cabo-hdmi {
        background-color: #e1c48e;
    }

    .cabo-vga {
        background-color: #df9c7c;
    }

    .auditorio {
        background-color: #d27c6a;
    }

    .speaker {
        background-color: #b65c46;
    }

    .emusoedisp {
        border: 5px solid #27b91c;
        color: black;
    }

    .emusoereserv {
        border: 5px solid #ff0000a9;
        color: black;
    }

    .dispereserv {
        border: 5px solid #ffd800ff;
        color: black;
    }

    .emusoedisp-legenda {
        background-color: #27b91c;
    }

    .emusoereserv-legenda {
        background-color: #ff0000a9;
    }

    .dispereserv-legenda {
        background-color: #ffd800ff;
    }

    .retirado {
        color: red;
    }

    .disponivel {
        color: black;
    }

    .recurso {
        color: black;
    }

    .legend {
        display: flex;
        justify-content: center;
        gap: 20px;
        margin-top: 20px;
        padding: 10px;
        flex-wrap: wrap;
    }

    .legend-item {
        display: flex;
        align-items: center;
        gap: 8px;
    }

    .legend-color {
        width: 20px;
        height: 20px;
        border-radius: 4px;
        border: 1px solid #333;
    }

    @media (max-width: 1024px) {
        .nomedisc {
            font-size: 0.9rem !important;
        }

        .resource-tag {
            font-size: 1.5rem !important;
            padding: 0.25rem 0.5rem !important;
        }

        .resource-icon {
            font-size: 1.5rem;
        }

        .schedule-header {
            font-size: 1.0rem;
        }
    }

    @media (max-width: 500px) {
        .nomedisc {
            font-size: 1rem !important;
        }
    }
</style>

<script>
    function bindRowHighlight() {
        const items = () => document.querySelectorAll('.list-group-item[data-responsavel]');

        items().forEach(function (item) {
            item.addEventListener('mouseenter', function () {
                if (termoBusca !== '') return;

                const responsavel = this.dataset.responsavel;
                items().forEach(function (other) {
                    if (other.dataset.responsavel === responsavel) {
                        other.classList.add(other === item ? 'highlight' : 'highlight-same');
                        other.classList.remove('dimmed');
                    } else {
                        other.classList.add('dimmed');
                        other.classList.remove('highlight', 'highlight-same');
                    }
                });
            });

            item.addEventListener('mouseleave', function () {
                if (termoBusca !== '') return;
                items().forEach(function (other) {
                    other.classList.remove('highlight', 'highlight-same', 'dimmed');
                });
            });
        });
    }

    // ── Busca global por teclado ───────────────────────────────────
    let termoBusca = '';
    let timerLimpar = null;

    function atualizarIndicador() {
        let indicador = document.getElementById('indicadorBusca');
        if (!indicador) {
            indicador = document.createElement('div');
            indicador.id = 'indicadorBusca';
            document.body.appendChild(indicador);
        }
        indicador.style.display = termoBusca === '' ? 'none' : 'block';
        if (termoBusca !== '')
            indicador.textContent = '🔍 ' + termoBusca;
    }

    function aplicarBusca() {
        const allItems = document.querySelectorAll('.list-group-item[data-responsavel]');
        if (termoBusca === '') {
            allItems.forEach(i => i.classList.remove('search-match', 'highlight', 'highlight-same', 'dimmed'));
            return;
        }
        const termo = termoBusca.toLowerCase();
        allItems.forEach(function (item) {
            const nome = item.dataset.responsavel.toLowerCase();
            if (nome.includes(termo)) {
                item.classList.add('search-match');
                item.classList.remove('dimmed', 'highlight', 'highlight-same');
            } else {
                item.classList.add('dimmed');
                item.classList.remove('search-match', 'highlight', 'highlight-same');
            }
        });
    }

    function limparBusca() {
        termoBusca = '';
        aplicarBusca();
        atualizarIndicador();
    }

    document.addEventListener('keydown', function (e) {
        const tag = document.activeElement.tagName.toLowerCase();
        if (tag === 'input' || tag === 'textarea' || tag === 'select') return;

        if (e.key === 'Escape') { limparBusca(); return; }
        if (e.key === 'Backspace') { termoBusca = termoBusca.slice(0, -1); }
        else if (e.key.length === 1) { termoBusca += e.key; }
        else { return; }

        aplicarBusca();
        atualizarIndicador();
        clearTimeout(timerLimpar);
        timerLimpar = setTimeout(limparBusca, 5000);
    });

    let _alternatingTimer = null;

    function bindTextAlternating() {
        if (_alternatingTimer !== null) {
            clearInterval(_alternatingTimer);
            _alternatingTimer = null;
        }
        document.querySelectorAll('.text-alternating').forEach(function (el) {
            el.textContent = el.dataset.textPrimary;
            el._showingAlt = false;
        });
        _alternatingTimer = setInterval(function () {
            document.querySelectorAll('.text-alternating').forEach(function (el) {
                el.textContent = el._showingAlt ? el.dataset.textPrimary : el.dataset.textAlt;
                el._showingAlt = !el._showingAlt;
            });
        }, 3000);
    }

    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            bindRowHighlight();
            bindTextAlternating();
        });
    }
    document.addEventListener('DOMContentLoaded', function () {
        bindRowHighlight();
        bindTextAlternating();
    });
</script>

<!-- ═══════════════════════════════════════
     DATA E HORA + DASHBOARD
═══════════════════════════════════════ -->
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="text-center my-3">
            <asp:Label ID="lblDataHora" runat="server" CssClass="text-muted fw-semibold" Visible="true"></asp:Label>
        </div>

        <div class="container-fluid px-3 py-1" runat="server" id="container">
            <!-- Conteúdo dinâmico gerado pelo code-behind -->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
</asp:UpdatePanel>
