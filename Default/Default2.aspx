<%-- $Id$ --%>

<%@ Page Language="C#" MasterPageFile="~/Master/Login2.master" AutoEventWireup="true"
    CodeFile="Default2.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/UserControls/DashboardRecursos.ascx" TagName="Dashboard" TagPrefix="uc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <!-- Recarrega a página inteira a cada 60 segundos -->
    <!--meta http-equiv="refresh" content="60"-->

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/Scripts/tooltip.js" />
        </Scripts>
    </asp:ScriptManager>

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
                justify-content: flex-start;
                gap: 1rem;
                flex-wrap: nowrap;
            }

            .login-compact .login-field {
                display: flex;
                align-items: center;
                gap: 0.5rem;
            }

    </style>

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
                            <!-- Título à esquerda -->
                            <span class="fw-bold text-white">
                                <i class="bi bi-grid-1x2-fill me-2"></i>OpenSARC
                            </span>

                            <!-- Usuário -->
                            <div class="login-field">
                                <label for="UserName">Usuário:</label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" Style="width: 14ch;" />
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

                            <!-- Mensagem de erro inline -->
                            <asp:Label ID="FailureText" runat="server" EnableViewState="False" CssClass="me-auto text-warning fs-6"></asp:Label>
                            <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Default/Painel.aspx" CssClass="btn btn-sm btn-outline-light">
                                <i class="bi bi-box-arrow-in-right me-1"></i>Painel</asp:HyperLink>
                        </div>
                    </LayoutTemplate>
                </asp:Login>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- ═══════════════════════════════════════
         DASHBOARD DE RECURSOS
    ═══════════════════════════════════════ -->
    <uc:Dashboard ID="Dashboard1" ContainerCssClass="px-5" ExibeRecursosRetirados="false" runat="server" />

    <!-- ═══════════════════════════════════════
         INFORMAÇÕES DO SISTEMA
    ═══════════════════════════════════════ -->
    <div class="container my-4">
        <div class="text-center">
            <p class="text-muted small">
                O OpenSARC é um sistema para alocação de recursos computacionais. Além da solicitação
                de recursos durante o período de planejamento semestral,<br />
                o sistema permite agendar eventos, consultar datas de avaliações e trocar e transferir
                recursos durante todo o ano.
            </p>
            <p class="text-muted small">
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
