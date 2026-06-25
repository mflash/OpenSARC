<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DashboardAtual.ascx.cs" Inherits="UserControls_DashboardAtual" %>

<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/UserControls/DashboardRecursos.css") %>" />

<style>

.user-initials {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 35px;
    height: 35px;
    padding: 0 6px;
    border-radius: 0.375rem;
    background-color: transparent;
    border: 1px solid #198754;
    color: #198754;
    font-weight: 700;
    font-size: 0.75rem;
    margin-right: 12px;
    flex-shrink: 0;
    text-transform: uppercase;
    white-space: nowrap;
}

/*
.user-initials {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 35px;
    height: 35px;
    padding: 0 6px;
    border-radius: 0.375rem;
    color: white;
    font-weight: 700;
    font-size: 0.85rem;
    margin-right: 12px;
    flex-shrink: 0;
    text-transform: uppercase;
    white-space: nowrap;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
*/

/* Reduz o tamanho da fonte para apelidos longos não estourarem o layout */
.user-initials.long-text {
    font-size: 0.65rem;
    padding: 0 8px;
}

.text-alternating {
    display: inline-block;
}
</style>

<asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick" Enabled="true" />

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

    // Listener para fazer busca na tela de login (filtragem por user)
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
        <div class="text-center my-1">
            <asp:Label ID="lblDataHora" runat="server" CssClass="text-muted fw-semibold" style="font-size: 0.65rem" Visible="false"></asp:Label>
        </div>

        <div class="container-fluid py-1" runat="server" id="container">
            <!-- Conteúdo dinâmico gerado pelo code-behind -->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
</asp:UpdatePanel>
