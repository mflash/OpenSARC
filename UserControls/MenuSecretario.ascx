<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuSecretario.ascx.cs" Inherits="Default_MenuSecretario" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<br /><div class="ms-quickLaunch">
    <table style="width: 150px" cellpadding="4" cellspacing="4">
        <tr>
            <td style="height: 20px;  text-align :center" class="ms-sitetitle a" >
                Menu</td>
        </tr>
        <tr>
            <td style="height: 18px;">
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td1" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> Secretaria</td>
                    </tr>
                </table>
                <a href="../Secretarios/ListaEvento.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Meus Eventos</span></a>
                <a href="../Alocacoes/Default.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Visualizar Aloca��es</span></a>
                    <a href="../Alocacoes/GerenciarRecursos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Gerenciar Recursos</span></a><br />
                <a href="../Docentes/AterarSenha.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Alterar Senha</span></a><br />
                    <a href="../Secretarios/VisualizarAtividades.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Visualizar Atividades</span></a><br />
                    <a href="../Usuarios/GerenciarUsuarios.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Gerenciar Usu�rios</span></a><br />
                    <a href="../Secretarios/PersonificarProfessor.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Personificar Professor</span></a><br />
                    <!--a href="../Common/RelatorioDeAcessos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Relat�rio de Acessos</span></!-->
                    <!--a href="../Turmas/VerificaTurmas.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Verificar preenchimento</span></a><br /!-->

    
        <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td2" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> Eventos
                        </td>
                    </tr>
                </table>
                <a href="../Eventos/Default.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Cadastrar</span></a>
                <a href="../Eventos/ListaEventos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Todos</span></a>
                <a href="../Eventos/ListaEventosFuturos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Futuros</span></a></td>
     </tr>
</table>
</div>
