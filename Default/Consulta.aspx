<%@ Page Language="C#" MasterPageFile="~/Master/PainelPublico.master" AutoEventWireup="true"
    CodeFile="Consulta.aspx.cs" Inherits="_Painel" %>

<%@ Register Src="~/UserControls/DashboardAtual.ascx" TagName="Dashboard" TagPrefix="uc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphConteudo">

    <!-- Recarrega a página inteira a cada 120 segundos -->
    <!--meta http-equiv="refresh" content="120" /-->

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />

    <style>
        .text-success-bright {
            color: #2ecc71 !important; /* A bright, vivid emerald green */
        }
    </style>

    <%-- Se PainelPublico.master já tiver ScriptManager, substituir por ScriptManagerProxy --%>
    <asp:ScriptManagerProxy ID="ScriptManager1" runat="server" />

    <!-- Cabeçalho público -->
    <div class="bg-dark text-white py-2 px-3 d-flex align-items-center gap-3">

        <span class="fw-bold"><i class="bi bi-key-fill me-2"></i>OpenSARC | Consulta</span>

        <!-- Filtros: matrícula e recurso -->
        <div class="d-flex align-items-center gap-2">
        </div>

        <asp:HyperLink ID="lnkLogin" runat="server"
            NavigateUrl="~/Default/Default2.aspx"
            CssClass="btn btn-sm btn-outline-light ms-auto">
            <i class="bi bi-box-arrow-in-right me-1"></i>Entrar
        </asp:HyperLink>

    </div>

    <uc:Dashboard ID="Dashboard1" ContainerCssClass="px-0" PainelConsulta="true" runat="server" />

    <asp:Label ID="lblErro" runat="server" CssClass="text-danger small" visible="false"/>
    
    <script>
        (function () {
            // Foco ao clicar em qualquer área não interativa
            document.addEventListener('click', function (e) {
                var el = e.target;
                while (el) {
                    var tag = el.tagName ? el.tagName.toUpperCase() : '';
                    if (tag === 'INPUT' || tag === 'BUTTON' || tag === 'A' ||
                        tag === 'TEXTAREA' || tag === 'SELECT') return;
                    el = el.parentElement;
                }
            });
        })();

        function limparCampo(input) {
            input.value = '';
        }
    </script>

</asp:Content>
