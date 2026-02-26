<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin.ascx.cs" Inherits="Default_MenuAdmin" %>

<nav class="sarc-sidebar-nav d-flex flex-column py-2 px-0" style="min-height: 100%;">

    <!-- Título + botão colapsar -->
    <div class="sarc-nav-header px-2 pb-2 mb-1 border-bottom border-secondary d-flex align-items-center justify-content-between">
        <span class="sarc-menu-title">
            <i class="bi bi-person-workspace me-1"></i>Professores
        </span>
        <button id="sarcToggleBtn"
                class="btn btn-sm p-0 border-0 sarc-toggle-btn"
                title="Recolher menu"
                onclick="sarcToggleSidebar(); return false;">
            <i class="bi bi-layout-sidebar-reverse" id="sarcToggleIcon"></i>
        </button>
    </div>

    <!-- Conteúdo do menu (ocultado ao colapsar) -->
    <div id="sarcMenuContent" class="d-flex flex-column">

        <!-- ── Turmas ───────────────────────────── -->
        <div class="sarc-section px-2 mt-2 mb-1">
            <span class="sarc-section-label">
                <i class="bi bi-book me-1"></i>Turmas
            </span>
            <ul class="nav flex-column ms-1 mt-1">
                <li class="nav-item">
                    <a accesskey="T" href="../Docentes/SelecionaTurma.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-grid me-1"></i>Turmas/Eventos
                    </a>
                </li>
                <li class="nav-item">
                    <asp:PlaceHolder ID="phClassListing" runat="server" />
                </li>
                <li class="nav-item">
                    <a href="../Docentes/AterarSenha.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-key me-1"></i>Alterar Senha
                    </a>
                </li>
            </ul>
        </div>

        <!-- ── Eventos ──────────────────────────── -->
        <div class="sarc-section px-2 mt-2 mb-1">
            <span class="sarc-section-label">
                <i class="bi bi-calendar2-event me-1"></i>Eventos
            </span>
            <ul class="nav flex-column ms-1 mt-1">
                <li class="nav-item">
                    <a href="../Eventos/Default.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-plus-circle me-1"></i>Cadastrar
                    </a>
                </li>
                <li class="nav-item">
                    <a href="../Eventos/ListaEventos.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-list-ul me-1"></i>Listar Todos
                    </a>
                </li>
                <li class="nav-item">
                    <a href="../Eventos/ListaEventosFuturos.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-calendar-check me-1"></i>Listar Futuros
                    </a>
                </li>
            </ul>
        </div>

        <!-- ── Consultas ────────────────────────── -->
        <div class="sarc-section px-2 mt-2 mb-1">
            <span class="sarc-section-label">
                <i class="bi bi-search me-1"></i>Consultas
            </span>
            <ul class="nav flex-column ms-1 mt-1">
                <li class="nav-item">
                    <a href="../Alocacoes/Default.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-eye me-1"></i>Visualizar Alocações
                    </a>
                </li>
                <li class="nav-item">
                    <a href="../Common/ListaTurmas.aspx"
                       class="nav-link sarc-nav-link py-1 px-2">
                        <i class="bi bi-table me-1"></i>Listar Turmas
                    </a>
                </li>
                <li class="nav-item">
                    <a href="../Alocacoes/nova_grade_2020.png"
                       class="nav-link sarc-nav-link py-1 px-2"
                       target="_blank">
                        <i class="bi bi-calendar3 me-1"></i>Nova Grade
                    </a>
                </li>
            </ul>
        </div>

    </div><!-- /sarcMenuContent -->

</nav>

<style>
    .sarc-sidebar-nav {
        font-size: 0.75rem;
    }

    .sarc-menu-title {
        font-size: 0.7rem;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 0.07em;
        color: #94a3b8;
        white-space: nowrap;
        overflow: hidden;
    }

    .sarc-toggle-btn {
        font-size: 0.9rem;
        color: #64748b !important;
        line-height: 1;
        flex-shrink: 0;
    }

    .sarc-toggle-btn:hover {
        color: #ffffff !important;
    }

    .sarc-section-label {
        display: block;
        font-size: 0.68rem;
        font-weight: 600;
        text-transform: uppercase;
        letter-spacing: 0.06em;
        color: #64748b;
        padding: 0 0.5rem;
    }

    .sarc-nav-link {
        font-size: 0.75rem;
        border-radius: 0.3rem;
        color: #cbd5e1 !important;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        display: flex;
        align-items: center;
        transition: background-color 0.15s ease, color 0.15s ease;
    }

    .sarc-nav-link:hover {
        background-color: var(--sarc-sidebar-hover, #2563eb);
        color: #ffffff !important;
    }

    /* ── Modo colapsado ── */
    .sarc-sidebar-nav.sarc-collapsed .sarc-menu-title,
    .sarc-sidebar-nav.sarc-collapsed #sarcMenuContent {
        display: none !important;
    }

    .sarc-sidebar-nav.sarc-collapsed .sarc-nav-header {
        justify-content: center !important;
        border-bottom: none !important;
    }

    .sarc-sidebar.sarc-collapsed {
        min-width: 36px !important;
        max-width: 36px !important;
        width: 36px !important;
    }
</style>

<script type="text/javascript">
    function sarcToggleSidebar() {
        var nav     = document.querySelector('.sarc-sidebar-nav');
        var sidebar = document.querySelector('.sarc-sidebar');
        var icon    = document.getElementById('sarcToggleIcon');
        var btn     = document.getElementById('sarcToggleBtn');

        if (nav.classList.contains('sarc-collapsed')) {
            nav.classList.remove('sarc-collapsed');
            if (sidebar) sidebar.classList.remove('sarc-collapsed');
            icon.className = 'bi bi-layout-sidebar-reverse';
            btn.title = 'Recolher menu';
        } else {
            nav.classList.add('sarc-collapsed');
            if (sidebar) sidebar.classList.add('sarc-collapsed');
            icon.className = 'bi bi-layout-sidebar';
            btn.title = 'Expandir menu';
        }
    }
</script>
