<%@ Page Language="C#" MasterPageFile="~/Master/PainelPublico.master" AutoEventWireup="true"
    CodeFile="Painel.aspx.cs" Inherits="_Painel" %>

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

        <span class="fw-bold"><i class="bi bi-key-fill me-2"></i>OpenSARC | Painel</span>

        <!-- Filtros: matrícula e recurso -->
        <div class="d-flex align-items-center gap-2">
            <asp:Label ID="lblMatricula" runat="server" Text="Matrícula:"
                AssociatedControlID="txtMatricula"
                CssClass="col-form-label col-form-label-sm mb-0 text-white-50" />
            <div>
                <asp:TextBox ID="txtMatricula" runat="server"
                    CssClass="form-control form-control-sm"
                    MaxLength="10"
                    Style="width: 14ch;" />
                <asp:RegularExpressionValidator ID="revMatricula" runat="server"
                    ControlToValidate="txtMatricula"
                    ValidationExpression="^[a-zA-Z0-9]*$"
                    ErrorMessage="Apenas alfanuméricos."
                    CssClass="text-warning small"
                    Display="Dynamic" />
            </div>

            <asp:Label ID="lblRecurso" runat="server" Text="Recurso:"
                AssociatedControlID="txtRecurso"
                CssClass="col-form-label col-form-label-sm mb-0 text-white-50" />
            <div class="d-flex align-items-center gap-2">
                <asp:TextBox ID="txtRecurso" runat="server"
                    CssClass="form-control form-control-sm"
                    MaxLength="4"
                    Style="width: 8ch;" />
                <asp:RegularExpressionValidator ID="revRecurso" runat="server"
                    ControlToValidate="txtRecurso"
                    ValidationExpression="^[a-zA-Z0-9]*$"
                    ErrorMessage="Apenas alfanuméricos."
                    CssClass="text-warning small"
                    Display="Dynamic" />

                <!-- Painel de aviso atualizado de forma assíncrona -->
                <asp:UpdatePanel ID="upAviso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblAviso" runat="server"
                            CssClass="small text-white"
                            Style="min-width: 250px; display: inline-block;" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConsultaMatricula" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnConsultaRecurso" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Botões ocultos que disparam os postbacks assíncronos -->
        <asp:Button ID="btnConsultaMatricula" runat="server"
            OnClick="btnConsultaMatricula_Click"
            UseSubmitBehavior="false"
            Style="display: none;" />
        <asp:Button ID="btnConsultaRecurso" runat="server"
            OnClick="btnConsultaRecurso_Click"
            UseSubmitBehavior="false"
            Style="display: none;" />

        <asp:HyperLink ID="lnkLogin" runat="server"
            NavigateUrl="~/Default/Default2.aspx"
            CssClass="btn btn-sm btn-outline-light ms-auto">
            <i class="bi bi-box-arrow-in-right me-1"></i>Entrar
        </asp:HyperLink>

    </div>

    <uc:Dashboard ID="Dashboard1" ContainerCssClass="px-0" ExibeRecursosRetirados="true" runat="server" />

    <asp:Label ID="lblErro" runat="server" CssClass="text-danger small" visible="false"/>
    
    <script>
        (function () {
            var inputMatricula = document.getElementById('<%= txtMatricula.ClientID %>');
            var inputRecurso = document.getElementById('<%= txtRecurso.ClientID %>');

            // Foco inicial ao carregar a página
            if (inputMatricula) inputMatricula.focus();

            // Foco ao clicar em qualquer área não interativa
            document.addEventListener('click', function (e) {
                var el = e.target;
                while (el) {
                    var tag = el.tagName ? el.tagName.toUpperCase() : '';
                    if (tag === 'INPUT' || tag === 'BUTTON' || tag === 'A' ||
                        tag === 'TEXTAREA' || tag === 'SELECT') return;
                    el = el.parentElement;
                }
                if (inputMatricula) inputMatricula.focus();
            });

            // Limpa ambos os campos ao clicar em txtMatricula
            if (inputMatricula) {
                inputMatricula.addEventListener('click', function () {
                    limparCampo(inputMatricula);
                    if (inputRecurso) limparCampo(inputRecurso);
                });

                if (inputRecurso) {
                    inputRecurso.addEventListener('click', function () {
                        limparCampo(inputRecurso);
                    });
                }
            }

            // Dispara postback assíncrono ao atingir exatamente 10 caracteres em txtMatricula
            if (inputMatricula) {
                inputMatricula.addEventListener('input', function () {
                    if (this.value.length === 10) {
                        var aviso = document.getElementById('<%= lblAviso.ClientID %>');
                        if (aviso) aviso.innerHTML = "<i class='bi bi-search me-1'></i>Verificando usuário...";
                        __doPostBack('<%= btnConsultaMatricula.UniqueID %>', '');
                    }
                });
            }

            // Dispara postback assíncrono ao atingir exatamente 4 caracteres em txtRecurso
            if (inputRecurso) {
                inputRecurso.addEventListener('input', function () {
                    if (this.value.length === 4) {
                        __doPostBack('<%= btnConsultaRecurso.UniqueID %>', '');
                    }
                });
            }
        })();

        function limparCampo(input) {
            input.value = '';
        }
    </script>

</asp:Content>
