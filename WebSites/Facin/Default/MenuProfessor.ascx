<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin.ascx.cs" Inherits="Default_MenuAdmin" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<br /><div class="ms-quickLaunch">
    <table style="width: 150px">
        <tr>
            <td style="height: 20px;  text-align :center" class="ms-sitetitle a" >
                Menu</td>
        </tr>
        <tr>
            <td style="height: 34px;">
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td1" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> Professores
                        </td>
                    </tr>
                </table>
                 <a href="../Common/RelatorioDeAcessos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Relat�rio de Acessos</span></a>
                <a href="../AcessoProfessores/SelecionaTurma.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Turmas/Eventos</span></a>
                <a href="../AcessoProfessores/AterarSenha.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Alterar Senha</span></a>
                
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
                    <span style="color: #3966bf">Listar Futuros</span></a>
                    
                    
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td3" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> Aloca��es
                        </td>
                    </tr>
                </table>
                <a href="../Alocacoes/Default.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Visualizar Aloca��es</span></a>
               </td>
        </tr>
</table>
</div>