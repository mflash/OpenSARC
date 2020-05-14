<%@ control language="C#" autoeventwireup="true" CodeFile="MenuAdmin.ascx.cs" Inherits="Default_MenuAdmin" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<br /><div class="ms-quickLaunch">
    <table style="width: 150px" cellpadding="4" cellspacing="4">
        <tr>
            <td style="height: 20px;  text-align :center" class="ms-sitetitle a" >
                Professores</td>
        </tr>
        <tr>
            <td style="height: 34px;">
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td1" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> TURMAS
                        </td><br/>
                    </tr>
                </table>
                <a accesskey="T" href="../Docentes/SelecionaTurma.aspx" class="ms-SPLink">
                    <span style="color: #3966bf" >Turmas/Eventos<br/></span></a><br/>
<span style="color: #3966bf"><asp:PlaceHolder ID="phClassListing" runat="server"></asp:PlaceHolder></span>
                <br/><a href="../Docentes/AterarSenha.aspx" class="ms-SPLink"><span style="color: #3966bf">Alterar Senha<br/></span></a>
                
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td2" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> EVENTOS
                        </td><br/>
                    </tr>
                </table>
                <a href="../Eventos/Default.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Cadastrar</span></a><br/>
                <a href="../Eventos/ListaEventos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Todos</span></a><br/>
                <a href="../Eventos/ListaEventosFuturos.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Futuros</span></a><br/>
                    
                    
                <table border="0" cellpadding="0" cellspacing="0" class="ms-navheader" width="100%">
                    <tr>
                        <td id="Td3" class="ms-sitetitle a" nowrap="nowrap" 
                            style="height: 10px; width:135px;"> CONSULTAS
                        </td><br/>
                    </tr>
                </table>
                <a href="../Alocacoes/Default.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Visualizar Alocações</span></a>
                <a href="../Common/ListaTurmas.aspx" class="ms-SPLink">
                    <span style="color: #3966bf">Listar Turmas</span></a><br />
                <a href="../Alocacoes/nova_grade_2020.png" class="ms-SPLink" target="_blank">
                    <span style="color: #3966bf">Nova grade de horários</span></a>
               </td>
        </tr>
</table>
</div>
