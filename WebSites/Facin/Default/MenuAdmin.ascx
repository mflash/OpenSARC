<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin.ascx.cs" Inherits="Default_MenuAdmin" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<br />
<div class="ms-quickLaunch">
<table style="width: 150px" cellpadding="4" cellspacing="4">
    <tr >
        <td align="center" style="height: 28px; width: 341px;" class="ms-sitetitle a">
            Menu</td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td14" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                        Usuários</td>
                    </tr>
            </table>
            <a href="../Default/CadastrarAdmin.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Admin/ListaAdmin.aspx"class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a>
            <a href="../AcessoProfessores/AterarSenha.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Alterar senha do admin</span></a>
            <a href="../Common/RelatorioDeAcessos.aspx" class="ms-SPLink">
                 <span style="color: #3966bf">Relatório de Acessos</span></a>   
                
                </td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td6" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Calendários</td>
                    </tr>
            </table>
            <a href="../Calendario/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Calendario/ListaCalendarios.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td3" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Categoria de Atividades
                    </td>
                </tr>
            </table>
            <a href="../CategoriaAtividade/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../CategoriaAtividade/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td4" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                            Categoria de Datas
                    </td>
                </tr>
            </table>
            <a href="../CategoriaData/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../CategoriaData/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td13" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Faculdades</td>
                </tr>
            </table>
            <a href="../Faculdades/CadastrarFaculdades.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Faculdades/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td7" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Professores</td>
                </tr>
            </table>
            <a href="../Professores/CadastroProfessores.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Professores/ListaProfessores.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td11" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Cursos</td>
                </tr>
            </table>
            <a href="../Cursos/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Cursos/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td5" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Categoria de Recursos</td>
                </tr>
            </table>
            <a href="../CategoriaRecurso/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../CategoriaRecurso/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
             <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td1" class="ms-sitetitle a" nowrap="nowrap" 
                        style="width: 135px; height: 10px;"> Recursos
                    </td>
                </tr>
         </table>
            <a href="../Recursos/CadastroRecurso.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Recursos/ListaRecursos.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a>
                <a href="../Alocacoes/GerenciarRecursos.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Gerenciar Recursos</span>
                </a></td>
                
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td12" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Categoria de Disciplinas</td>
                </tr>
            </table>
            <a href="../CategoriaDisciplina/Cadastro.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../CategoriaDisciplina/List.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td10" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Disciplinas</td>
                    </tr>
            </table>
            <a href="../Disciplina/CadastraDisciplina.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Disciplina/ListaDisciplinas.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 18px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td8" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Turmas</td>
                    </tr>
            </table>
            <a href="../Turmas/CadastroTurma2.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Cadastrar</span></a>
            <a href="../Turmas/ListaTurmas.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Listar</span></a></td>
    </tr>
    <tr>
        <td style="height: 19px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td9" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Importação</td>
                </tr>
            </table>
            <a href="../ImportarDados/Importar.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Importar</span></a></td>
    </tr>
    <tr>
        <td style="height: 19px; width: 341px;">
            <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                <tr>
                    <td id="Td2" class="ms-sitetitle a" nowrap="nowrap" style="width: 135px; height: 10px;">
                                        Controle de Acessos</td>
                </tr>
            </table>
            <a href="../Admin/ControleEstados.aspx" class="ms-SPLink">
                <span style="color: #3966bf">Configurar</span></a></td>
    </tr>
</table>
</div>