<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuAdmin.ascx.cs" Inherits="Default_MenuAdmin" %>
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<link href="../CORE.CSS" rel="stylesheet" type="text/css" />
<ul id="nav">
    <li><a accesskey="T" href="../Docentes/SelecionaTurma.aspx" >Resumo e Avisos<span>Turmas alocadas, eventos registrados, trocas e transferências de recursos</span></a></li>
    <asp:PlaceHolder ID="phClassListing" runat="server"></asp:PlaceHolder>
    <li><a href="../Eventos/Default.aspx" >Novo Evento<span>Cadastro de novo evento.</span></a></li>
    <li><a href="../Eventos/ListaEventos.aspx">Todos os Eventos<span>Consultas sobre eventos cadastrados.</span></a></li>
    <li><a href="../Eventos/ListaEventosFuturos.aspx" >Eventos Futuros<span>Consultas sobre eventos previstos a partir deste momento.</span></a>
    <li><a href="../Alocacoes/Default.aspx" >Todas as Alocações<span>Consultas sobre alocações de recursos.</span></a>
</ul>

