<%-- $Id$ --%>

<%@ Page Language="C#" MasterPageFile="~/Master/Login2.master" AutoEventWireup="true"
    CodeFile="Default2.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="BusinessData.Entities" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Scripts/tooltip.js" />
        </Scripts>
    </asp:ScriptManager>

    <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick">
    </asp:Timer>

    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />

    <style>
        /* =============================================
           Layout Full Width
        ============================================= */
        body {
            font-family: 'Inter', 'Segoe UI', Arial, sans-serif;
        }

        /* =============================================
           Login Compacto - UMA ÚNICA LINHA HORIZONTAL
        ============================================= */
        .login-compact {
            background: linear-gradient(135deg, #2563eb 0%, #1e40af 100%);
            padding: 0.5rem 1rem;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            margin: 0;
        }

            .login-compact table {
                width: 100%;
                margin: 0;
                padding: 0;
            }

            .login-compact label {
                color: white;
                font-weight: 500;
                font-size: 0.875rem;
                margin-right: 0.5rem;
                white-space: nowrap;
            }

            .login-compact .form-control {
                height: 32px;
                border-radius: 4px;
                border: 1px solid #e2e8f0;
                font-size: 0.875rem;
                width: 150px;
                display: inline-block;
                vertical-align: middle;
            }

                .login-compact .form-control:focus {
                    border-color: #60a5fa;
                    box-shadow: 0 0 0 0.2rem rgba(96, 165, 250, 0.25);
                }

            .login-compact .btn-light {
                height: 32px;
                font-weight: 600;
                font-size: 0.875rem;
                padding: 0 1.25rem;
                border: none;
                background: white;
                color: #1e40af;
                vertical-align: middle;
            }

                .login-compact .btn-light:hover {
                    background: #f1f5f9;
                    transform: translateY(-1px);
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15);
                }

            .login-compact .form-check {
                display: inline-flex;
                align-items: center;
                margin: 0;
                vertical-align: middle;
            }

            .login-compact .form-check-input {
                cursor: pointer;
                margin-top: 0;
                margin-right: 0.25rem;
                vertical-align: middle;
            }

            .login-compact .form-check-label {
                color: white;
                font-size: 0.875rem;
                font-weight: 400;
                margin-left: 0.25rem;
                cursor: pointer;
            }

            .login-compact a {
                color: #fef3c7;
                font-size: 0.875rem;
                font-weight: 500;
                text-decoration: none;
                vertical-align: middle;
            }

                .login-compact a:hover {
                    color: white;
                    text-decoration: underline;
                }

            .login-compact .text-danger {
                color: #fecaca !important;
                font-size: 0.75rem;
                margin-left: 0.25rem;
            }

            .login-compact .login-row {
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 1rem;
                flex-wrap: nowrap;
            }

            .login-compact .login-field {
                display: flex;
                align-items: center;
                gap: 0.5rem;
            }

        /* =============================================
           Dashboard de Recursos
        ============================================= */
        /*
        .container {
            display: flex;
            flex-direction: column;
            max-width: 1200px;
            width: 100%;
            margin: auto;
            padding: 0 1rem;
        }
            */

        /*
        .row {
            display: grid;
            grid-template-columns: 60px auto;
            gap: 5px;
            align-items: center;
            margin-bottom: 10px;
            padding: 5px;
        }

        .category {
            color: black;
            font-weight: bold;
            font-size: 10px;
            text-align: center;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 5px;
            width: 60px;
            flex-direction: column;
        }

            .category img {
                width: 24px;
                height: 24px;
            }

        .grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 5px;
            padding-right: 10px;
        }

        .block {
            padding: 2px;
            text-align: center;
            font-size: 18px;
            font-weight: bold;
            border-radius: 5px;
            transition: transform 0.2s, background-color 0.3s;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 80px;
            min-width: 280px;
            background: white;
        }

            .block span {
                display: block;
                font-size: 14px;
                font-weight: normal;
                font-family: 'Inter', 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
                opacity: 1.0;
            }

            /*

        /* Category Colors */
        .lab {
            background-color: #97c7a657;
        }

        .notebook {
            background-color: #6e90b057;
        }

        .cabo-hdmi {
            background-color: #e1c48e57;
        }

        .cabo-vga {
            background-color: #df9c7c57;
        }

        .auditorio {
            background-color: #d27c6a57;
        }

        .speaker {
            background-color: #b65c4657;
        }

        .emusoedisp {
            border: 5px solid #27b91c;
            color: black;
        }

        .emusoereserv {
            border: 5px solid #ff0000a9;
            color: black;
        }

        .dispereserv {
            border: 5px solid #ffd800ff;
            color: black;
        }

        .emusoedisp-legenda {
            background-color: #27b91c;
        }

        .emusoereserv-legenda {
            background-color: #ff0000a9;
        }

        .dispereserv-legenda {
            background-color: #ffd800ff;
        }

        .retirado {
            color: red;
        }

        .disponivel {
            color: black;
        }

        .recurso {
            color: black;
        }

        .legend {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: 20px;
            padding: 10px;
            flex-wrap: wrap;
        }

        .legend-item {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .legend-color {
            width: 20px;
            height: 20px;
            border-radius: 4px;
            border: 1px solid #333;
        }

        /*

        .block {
            position: relative;
            cursor: pointer;
        }

        .tooltip {
            content: attr(data-tooltip);
            position: absolute;
            bottom: 120%;
            left: 50%;
            transform: translateX(-50%);
            background-color: rgba(220, 220, 220, 1);
            color: black;
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            font-family: 'Inter', 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
            line-height: 19px;
            white-space: pre-line;
            width: max-content;
            max-width: 450px;
            opacity: 0;
            visibility: hidden;
            transition: opacity 0.1s;
        }

            .block:hover .tooltip,
            .block.active .tooltip,
            .tooltip.visible {
                opacity: 1;
                visibility: visible;
            }

            .tooltip.activeLeft {
                transform: translateX(-70%);
            }
            */

        /* Responsive Adjustments */
        /*
        @media (max-width: 768px) {
            .row {
                grid-template-columns: 100px auto;
            }

            .category {
                font-size: 12px;
                padding: 8px;
            }

            .login-compact .login-row {
                flex-wrap: wrap;
            }
        }
            */

        .schedule-card {
            border: none;
            border-radius: 12px;
        }

        .schedule-header {
            background-color: #f8f9fa;
            border-bottom: 2px solid #dee2e6;
            padding: 1rem;
            font-weight: 700;
            font-size: medium;
        }

        /* Card centralizado quando há apenas uma coluna */
        .schedule-col {
            max-width: 600px;
        }

        .resource-container {
            display: flex;
            align-items: center;
        }

        .resource-icon {
            font-size: 1.3rem;
            margin-right: 10px;
            color: #6c757d;
        }

        .resource-tag {
            font-size: 1rem !important;
            font-family: sans-serif;
            font-weight: 600;
        }

        /* Badge em uso: Cor vibrante e animação de sombra */
        .badge-active {
            background-color: #dc3545 !important; /* Vermelho para 'ocupado' */
            animation: pulse-red 2s infinite;
            border: 1px solid rgba(255, 255, 255, 0.4);
        }

        @keyframes pulse-red {
            0% {
                box-shadow: 0 0 0 0 rgba(220, 53, 69, 0.7);
            }

            70% {
                box-shadow: 0 0 0 10px rgba(220, 53, 69, 0);
            }

            100% {
                box-shadow: 0 0 0 0 rgba(220, 53, 69, 0);
            }
        }

        /* Indicador visual interno (ponto luminoso) */
        .status-indicator {
            height: 8px;
            width: 8px;
            background-color: #fff;
            border-radius: 50%;
            display: inline-block;
            margin-right: 6px;
            vertical-align: middle;
        }

        .resource-tag {
            font-size: 1.1rem !important; /* Fonte ampliada conforme solicitado */
            padding: 0.5rem 1rem !important;
            display: flex;
            align-items: center;
        }

        .resource-icon.text-danger {
            color: #dc3545 !important;
        }

        /* Estado: Disponível (Verde) */
        .badge-available {
            background-color: #198754 !important;
            border: 1px solid rgba(255, 255, 255, 0.2);
        }

        /* Estado: Manutenção (Amarelo) */
        .badge-maintenance {
            background-color: #ffc107 !important;
            color: #000 !important;
            border: 1px solid rgba(0, 0, 0, 0.1);
        }

        /* Ajuste para ícones de manutenção */
        .text-maintenance {
            color: #856404 !important;
        }

        .text-available {
            color: #198754 !important;
        }

        .nomedisc {
            font-size: medium;
        }

        /* Destaque ao passar o mouse - mesmo professor nas duas colunas */
        .list-group-item.highlight {
            background-color: #fff3cd;
            transition: background-color 0.2s;
        }

        .list-group-item.highlight-same {
            background-color: #d1e7dd;
            transition: background-color 0.2s;
        }

        /* Esmaece itens não relacionados ao hover */
        .list-group-item.dimmed {
            opacity: 0.25;
            transition: opacity 0.2s;
        }

        .list-group-item.highlight,
        .list-group-item.highlight-same {
            transition: background-color 0.2s, opacity 0.2s;
        }
        /* Indicador flutuante de busca por teclado */
        #indicadorBusca {
            position: fixed;
            bottom: 1.5rem;
            left: 50%;
            transform: translateX(-50%);
            background: rgba(0, 0, 0, 0.75);
            color: white;
            padding: 0.4rem 1rem;
            border-radius: 999px;
            font-size: 1rem;
            font-family: monospace;
            pointer-events: none;
            z-index: 9999;
            display: none;
        }
    </style>

    <script>
        // Destaque cruzado de linhas pelo responsável
        /*
                function bindRowHighlight() {
                    const items = document.querySelectorAll('.list-group-item[data-responsavel]');
                    items.forEach(function (item) {
                        item.addEventListener('mouseenter', function () {
                            const responsavel = this.dataset.responsavel;
                            document.querySelectorAll('.list-group-item[data-responsavel="' + responsavel + '"]')
                                .forEach(function (match) {
                                    match.classList.add(match === item ? 'highlight' : 'highlight-same');
                                });
                        });
                        item.addEventListener('mouseleave', function () {
                            const responsavel = this.dataset.responsavel;
                            document.querySelectorAll('.list-group-item[data-responsavel="' + responsavel + '"]')
                                .forEach(function (match) {
                                    match.classList.remove('highlight', 'highlight-same');
                                });
                        });
                    });
                }
        
                // Rebinda após cada UpdatePanel (Timer)
                if (typeof Sys !== 'undefined') {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindRowHighlight);
                }
                document.addEventListener('DOMContentLoaded', bindRowHighlight);
                */

        /*
        function bindRowHighlight() {
            const items = document.querySelectorAll('.list-group-item[data-responsavel]');

            items.forEach(function (item) {
                item.addEventListener('mouseenter', function () {
                    const responsavel = this.dataset.responsavel;

                    items.forEach(function (other) {
                        if (other.dataset.responsavel === responsavel) {
                            other.classList.add(other === item ? 'highlight' : 'highlight-same');
                            other.classList.remove('dimmed');
                        } else {
                            other.classList.add('dimmed');
                            other.classList.remove('highlight', 'highlight-same');
                        }
                    });
                });

                item.addEventListener('mouseleave', function () {
                    items.forEach(function (other) {
                        other.classList.remove('highlight', 'highlight-same', 'dimmed');
                    });
                });
            });
        }

        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindRowHighlight);
        }
        document.addEventListener('DOMContentLoaded', bindRowHighlight);
        */

        function bindRowHighlight() {
            const items = () => document.querySelectorAll('.list-group-item[data-responsavel]');

            // ── Hover cruzado ──────────────────────────────────────────
            items().forEach(function (item) {
                item.addEventListener('mouseenter', function () {
                    if (termoBusca !== '') return;

                    const responsavel = this.dataset.responsavel;
                    items().forEach(function (other) {
                        if (other.dataset.responsavel === responsavel) {
                            other.classList.add(other === item ? 'highlight' : 'highlight-same');
                            other.classList.remove('dimmed');
                        } else {
                            other.classList.add('dimmed');
                            other.classList.remove('highlight', 'highlight-same');
                        }
                    });
                });

                item.addEventListener('mouseleave', function () {
                    if (termoBusca !== '') return;
                    items().forEach(function (other) {
                        other.classList.remove('highlight', 'highlight-same', 'dimmed');
                    });
                });
            });
        }

        // ── Busca global por teclado ───────────────────────────────────
        let termoBusca = '';
        let timerLimpar = null;

        function atualizarIndicador() {
            let indicador = document.getElementById('indicadorBusca');
            if (!indicador) {
                indicador = document.createElement('div');
                indicador.id = 'indicadorBusca';
                document.body.appendChild(indicador);
            }
            if (termoBusca === '') {
                indicador.style.display = 'none';
            } else {
                indicador.textContent = '🔍 ' + termoBusca;
                indicador.style.display = 'block';
            }
        }

        function aplicarBusca() {
            const allItems = document.querySelectorAll('.list-group-item[data-responsavel]');
            if (termoBusca === '') {
                allItems.forEach(i => i.classList.remove('search-match', 'highlight', 'highlight-same', 'dimmed'));
                return;
            }
            const termo = termoBusca.toLowerCase();
            allItems.forEach(function (item) {
                const nome = item.dataset.responsavel.toLowerCase();
                if (nome.includes(termo)) {
                    item.classList.add('search-match');
                    item.classList.remove('dimmed', 'highlight', 'highlight-same');
                } else {
                    item.classList.add('dimmed');
                    item.classList.remove('search-match', 'highlight', 'highlight-same');
                }
            });
        }

        function limparBusca() {
            termoBusca = '';
            aplicarBusca();
            atualizarIndicador();
        }

        document.addEventListener('keydown', function (e) {
            // Ignora se o foco está num input/textarea
            const tag = document.activeElement.tagName.toLowerCase();
            if (tag === 'input' || tag === 'textarea' || tag === 'select') return;

            if (e.key === 'Escape') {
                limparBusca();
                return;
            }
            if (e.key === 'Backspace') {
                termoBusca = termoBusca.slice(0, -1);
            } else if (e.key.length === 1) {
                // Apenas caracteres imprimíveis
                termoBusca += e.key;
            } else {
                return;
            }

            aplicarBusca();
            atualizarIndicador();

            // Limpa automaticamente após 5s de inatividade
            clearTimeout(timerLimpar);
            timerLimpar = setTimeout(limparBusca, 5000);
        });

        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindRowHighlight);
        }
        document.addEventListener('DOMContentLoaded', bindRowHighlight);
    </script>

    <!-- ═══════════════════════════════════════
         LOGIN COMPACTO - UMA LINHA HORIZONTAL
    ═══════════════════════════════════════ -->
    <div class="login-compact">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Login ID="loginEntrada" runat="server"
                    FailureText="Usuário inválido ou senha inválida."
                    LoginButtonText="Entrar"
                    PasswordLabelText="Senha:"
                    PasswordRequiredErrorMessage="*"
                    RememberMeText="Lembrar-me"
                    UserNameLabelText="Usuário:"
                    UserNameRequiredErrorMessage="*"
                    DestinationPageUrl="~/Default/SelecionarCalendario2.aspx"
                    OnLoginError="loginEntrada_LoginError"
                    TitleText=""
                    AccessKey="M"
                    Orientation="Horizontal"
                    OnAuthenticate="loginEntrada_Authenticate"
                    PasswordRecoveryUrl="~/Default/ResetSenha.aspx"
                    PasswordRecoveryText="Esqueci"
                    DisplayRememberMe="true">

                    <LayoutTemplate>
                        <div class="login-row">
                            <!-- Usuário -->
                            <div class="login-field">
                                <label for="UserName">Usuário:</label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                    ControlToValidate="UserName"
                                    ErrorMessage="*"
                                    ValidationGroup="loginEntrada"
                                    CssClass="text-danger">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Senha -->
                            <div class="login-field">
                                <label for="Password">Senha:</label>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
                                    ControlToValidate="Password"
                                    ErrorMessage="*"
                                    ValidationGroup="loginEntrada"
                                    CssClass="text-danger">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Botão Entrar -->
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login"
                                Text="Entrar"
                                ValidationGroup="loginEntrada"
                                CssClass="btn btn-light" />

                            <!-- Lembrar-me -->
                            <div class="form-check">
                                <asp:CheckBox ID="RememberMe" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="RememberMe">
                                    Lembrar-me
                               
                                </label>
                            </div>

                            <!-- Link Esqueci -->
                            <%--                            <asp:HyperLink ID="PasswordRecoveryLink" runat="server" 
                                NavigateUrl="~/Default/ResetSenha.aspx">
                                <i class="bi bi-shield-lock"></i> Esqueci
                            </asp:HyperLink>--%>

                            <!-- Mensagem de erro inline -->
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </div>
                    </LayoutTemplate>
                </asp:Login>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- ═══════════════════════════════════════
         DATA E HORA + DASHBOARD
    ═══════════════════════════════════════ -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="text-center my-3">
                <asp:Label ID="lblDataHora" runat="server" CssClass="text-muted fw-semibold" Visible="false"></asp:Label>
            </div>

            <!-- Dashboard de Recursos -->
            <div class="container py-1" runat="server" id="container">
                <!-- Conteúdo dinâmico gerado pelo code-behind -->
            </div>

            <!-- Legenda -->
            <!--div class="legend">
                <div class="legend-item">
                    <div class="legend-color emusoereserv-legenda"></div>
                    <span>Reservado agora e no próximo horário</span>
                </div>
                <div class="legend-item">
                    <div class="legend-color dispereserv-legenda"></div>
                    <span>Livre agora e reservado no próximo horário</span>
                </div>
                <div class="legend-item">
                    <div class="legend-color emusoedisp-legenda"></div>
                    <span>Reservado agora e livre no próximo horário</span>
                </div>
            </div-->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- ═══════════════════════════════════════
         INFORMAÇÕES DO SISTEMA
    ═══════════════════════════════════════ -->
    <div class="container my-4">
        <div class="text-center">
            <p class="mb-2">
                O OpenSARC é um sistema para alocação de recursos computacionais. Além da solicitação
                de recursos durante o período de planejamento semestral,<br />
                o sistema permite agendar eventos, consultar datas de avaliações e trocar e transferir
                recursos durante todo o ano.
           
            </p>
            <p class="mb-2">
                O OpenSARC é <em>software</em> livre. Caso deseje participar, reclamar ou dar sugestões,
                visite <a href="https://github.com/mflash/OpenSARC" target="_blank" class="fw-semibold">https://github.com/mflash/OpenSARC
                </a>.
           
            </p>
            <p class="text-muted small">
                Em especial, aguardamos voluntários interessados em utilizar o sistema como estudo
                de caso para suas disciplinas de desenvolvimento de <em>software</em>.
           
            </p>
        </div>
    </div>

    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>
