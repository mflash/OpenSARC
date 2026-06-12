<%@ Page Language="C#" MasterPageFile="~/Master/PainelPublico.master" AutoEventWireup="true"
    CodeFile="Painel.aspx.cs" Inherits="_Painel" %>

<%@ Register Src="~/UserControls/DashboardRecursos.ascx" TagName="Dashboard" TagPrefix="uc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphConteudo">

    <!-- Recarrega a página inteira a cada 60 segundos -->
    <meta http-equiv="refresh" content="60">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />

    <!-- Cabeçalho público -->
    <div class="bg-dark text-white py-2 px-3 d-flex align-items-center justify-content-between">
        <span class="fw-bold"><i class="bi bi-display me-2"></i>OpenSARC — Painel de Recursos</span>
        <asp:HyperLink ID="lnkLogin" runat="server"
            NavigateUrl="~/Default/Default2.aspx"
            CssClass="btn btn-sm btn-outline-light">
            <i class="bi bi-box-arrow-in-right me-1"></i>Entrar
        </asp:HyperLink>
    </div>

    <uc:Dashboard ID="Dashboard1" runat="server" />

</asp:Content>
