<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuSecretario.ascx.cs" Inherits="Default_MenuSecretario" %>
<!-- Menu Secretário com Bootstrap 5 -->
<nav class="nav flex-column">

    <!-- Título do Menu -->
    <div class="px-3 py-2 mb-3 border-bottom border-secondary">
        <h6 class="text-uppercase fw-bold mb-0 text-white-50 small">
            <i class="bi bi-person-lines-fill me-2"></i>Menu Secretaria
        </h6>
    </div>

    <!-- Secretaria -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-briefcase-fill me-2"></i>Secretaria
            </span>
        </div>
        <a href="../Secretarios/ListaEvento.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-calendar-check me-2"></i>Meus Eventos
        </a>
        <a href="../Alocacoes/Default.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-eye me-2"></i>Visualizar Alocações
        </a>
        <a href="../Alocacoes/GerenciarRecursos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-sliders me-2"></i>Gerenciar Recursos
        </a>
        <a href="../Docentes/AterarSenha.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-key me-2"></i>Alterar Senha
        </a>
        <a href="../Secretarios/VisualizarAtividades.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-clipboard-check me-2"></i>Visualizar Atividades
        </a>
        <a href="../Usuarios/GerenciarUsuarios.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-people-fill me-2"></i>Gerenciar Usuários
        </a>
        <a href="../Secretarios/PersonificarProfessor.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-person-badge me-2"></i>Personificar Professor
        </a>
    </div>

    <!-- Eventos -->
    <div class="mb-3">
        <div class="px-3 py-1 mb-1">
            <span class="text-white-50 text-uppercase small fw-semibold">
                <i class="bi bi-calendar3 me-2"></i>Eventos
            </span>
        </div>
        <a href="../Eventos/Default.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-plus-circle me-2"></i>Cadastrar
        </a>
        <a href="../Eventos/ListaEventos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-list-ul me-2"></i>Listar Todos
        </a>
        <a href="../Eventos/ListaEventosFuturos.aspx" class="nav-link px-4 py-1">
            <i class="bi bi-calendar-event me-2"></i>Listar Futuros
        </a>
    </div>

</nav>