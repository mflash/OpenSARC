<<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin2.ascx.cs" Inherits="Default_MenuAdmin" %>

<!-- Menu Admin com Bootstrap 5 -->
<nav class="nav flex-column">
    
    <!-- Título do Menu -->
    <div class="px-3 py-2 mb-3 border-bottom border-secondary">
        <h6 class="text-uppercase fw-bold mb-0 text-white-50 small">
            <i class="bi bi-gear-fill me-2"></i>Menu Administrativo
        </h6>
    </div>

    <!-- Usuários -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-people-fill me-2"></i>Usuários
            </span>
        </div>
        <a href="../Default/CadastrarAdmin.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-person-plus me-2"></i>Cadastrar
        </a>
        <a href="../Admin/ListaAdmin.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
        <a href="../Docentes/AterarSenha.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-key me-2"></i>Alterar senha do admin
        </a>
        <a href="../Common/RelatorioDeAcessos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-file-text me-2"></i>Relatório de Acessos
        </a>
    </div>

    <!-- Calendários -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-calendar3 me-2"></i>Calendários
            </span>
        </div>
        <a href="../Calendario/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Calendario/ListaCalendarios.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Categoria de Atividades -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-tag-fill me-2"></i>Categoria de Atividades
            </span>
        </div>
        <a href="../CategoriaAtividade/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../CategoriaAtividade/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Categoria de Datas -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-calendar-event me-2"></i>Categoria de Datas
            </span>
        </div>
        <a href="../CategoriaData/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../CategoriaData/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Faculdades -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-building me-2"></i>Faculdades
            </span>
        </div>
        <a href="../Faculdades/CadastrarFaculdades.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Faculdades/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Professores -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-person-badge me-2"></i>Professores
            </span>
        </div>
        <a href="../Professores/CadastroProfessores.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Professores/ListaProfessores.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
        <a href="../Professores/ImportarProfessores.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-upload me-2"></i>Importar
        </a>
    </div>

    <!-- Cursos -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-mortarboard-fill me-2"></i>Cursos
            </span>
        </div>
        <a href="../Cursos/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Cursos/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Categoria de Recursos -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-tags-fill me-2"></i>Categoria de Recursos
            </span>
        </div>
        <a href="../CategoriaRecurso/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../CategoriaRecurso/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Recursos -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-laptop me-2"></i>Recursos
            </span>
        </div>
        <a href="../Recursos/CadastroRecurso.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Recursos/ListaRecursos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
        <a href="../Alocacoes/GerenciarRecursos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-sliders me-2"></i>Gerenciar Recursos
        </a>
        <a href="../Recursos/ConsultaAcessos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-search me-2"></i>Consultar Acessos (Linux)
        </a>
    </div>

    <!-- Categoria de Disciplinas -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-bookmark-fill me-2"></i>Categoria de Disciplinas
            </span>
        </div>
        <a href="../CategoriaDisciplina/Cadastro.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../CategoriaDisciplina/List.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Disciplinas -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-book me-2"></i>Disciplinas
            </span>
        </div>
        <a href="../Disciplina/CadastraDisciplina.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Disciplina/ListaDisciplinas.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
    </div>

    <!-- Turmas -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-people me-2"></i>Turmas
            </span>
        </div>
        <a href="../Turmas/CadastroTurma2.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Turmas/ListaTurmas.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar
        </a>
        <a href="../Turmas/VerificaTurmas.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-check-circle me-2"></i>Verificar preenchimento
        </a>
    </div>

    <!-- Importação -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-cloud-upload me-2"></i>Importação
            </span>
        </div>
        <a href="../ImportarDados/Importar.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-file-earmark-arrow-up me-2"></i>Importar
        </a>
        <a href="../ImportarDados/ImportarXLSX.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-file-earmark-excel me-2"></i>Importar XLSX
        </a>
        <a href="../ImportarDados/ImportarAcad.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-database-add me-2"></i>Importar Acadêmico
        </a>
    </div>

    <!-- Controle de Acessos -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-shield-lock me-2"></i>Controle de Acessos
            </span>
        </div>
        <a href="../Admin/ControleEstados.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-gear me-2"></i>Configurar
        </a>
    </div>

</nav>

<style>
    /* Estilos customizados para o menu admin */
    .nav-link {
        color: var(--sarc-sidebar-text, #cbd5e1);
        font-size: 0.85rem;
        transition: all 0.2s ease;
        border-radius: 0.25rem;
        margin: 0 0.5rem;
    }

    .nav-link:hover {
        color: #ffffff;
        background-color: rgba(37, 99, 235, 0.2);
        text-decoration: none;
        padding-left: 1.25rem !important;
    }

    .nav-link i {
        opacity: 0.7;
        font-size: 0.85rem;
    }

    .nav-link:hover i {
        opacity: 1;
    }
</style>