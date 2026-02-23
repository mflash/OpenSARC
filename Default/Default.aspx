<%-- $Id$ --%>

<%@ Page Language="C#" MasterPageFile="~/Master/Login.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="BusinessData.Entities" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="center" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/tooltip.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick">
        </asp:Timer>
    </div>
        <style>
        /* General Styles */
        /*body {
            font-family: Arial, sans-serif;
            background-color: #ddd;
            margin: 0;
            padding: 20px;
            display: flex;
            justify-content: center;
        }*/

        /*body {
            display: flex;
            justify-content: center;
            align-items: center;
        }*/

        .container {
            display: flex;
            flex-direction: column;
            max-width: 1024px;
            width: 100%;
            margin: auto;
        }

        /* Each row contains category + blocks */
        .row {
            display: grid;
            grid-template-columns: 60px auto;
            gap: 5px;
            align-items: center;
            margin-bottom: 10px;
            padding: 5px;
        }

        /* Fixed Category Column */
        .category {
            /*background-color: #777;*/
            color: black;
            font-weight: bold;
            font-size: 10px;
            /*padding: 5px;*/
            text-align: center;
            /*border: none;*/
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

        /* Blocks Grid */
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

        /* Two-Line Text */
        .block span {
            display: block;
            font-size: 14px;
            font-weight: normal;
            font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
            opacity: 1.0;
        }

        /* Hover Effect
        .block:hover {
            transform: scale(1.05);
            filter: brightness(90%);
        }*/

        /* Category Colors */
        .lab { background-color: #97c7a657;}
        .notebook { background-color: #6e90b057;}
        .cabo-hdmi { background-color: #e1c48e57;}
        .cabo-vga { background-color: #df9c7c57;}
        .auditorio { background-color: #d27c6a57;}
        .speaker { background-color: #b65c4657;}

        .emusoedisp { border-color: #ffd800ff; border: 5px solid #27b91c; color: black}
        .emusoereserv { border-color: #ff0000ef; border: 5px solid #ff0000a9; color: black}
        .dispereserv { border-color: #fff79e; border: 5px solid #ffd800ff; color: black}

        .emusoedisp-legenda { background-color: #27b91c }
        .emusoereserv-legenda { background-color: #ff0000a9 }
        .dispereserv-legenda { background-color: #ffd800ff }

        .retirado { color: red }
        .disponivel { color: black }

        .recurso { color: black}

        .legend {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: 20px;
            padding: 10px;
            /*background: #f8f8f8;
            border-radius: 8px;*/
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
        
        /**/

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
            font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
            line-height: 19px;
            white-space: pre-line;
            width: max-content;
            max-width: 450px;
            opacity: 0;
            visibility: hidden;
            transition: opacity 0.1s; /* ease-in-out; */
        }

        .block:hover .tooltip,
        .block.active .tooltip,
        tooltip.visible {
            opacity: 1;
            visibility: visible;
        }

        .tooltip.activeLeft {
            transform: translateX(-70%);
        }

        /**/

        /* Responsive Adjustments */
        @media (max-width: 700px) {
            .row {
                grid-template-columns: 100px auto;
            }
            .category {
                font-size: 12px;
                padding: 8px;
            }
        }
    </style>

    <table style="width: 100%">
        <tr>
            <td align="center" class="ms-toolbar" style="text-align: center;" valign="top">
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Login ID="loginEntrada" runat="server" CssClass="ms-toolbar" FailureText="Usuário inválido ou senha inválida."
                                Font-Bold="False" LoginButtonText="Entrar" PasswordLabelText="Senha:" PasswordRequiredErrorMessage="Senha não pode ser nula."
                                RememberMeText="Lembrar de mim da próxima vez." UserNameLabelText="Usuário:"
                                UserNameRequiredErrorMessage="Informe usuário." Width="862px" DestinationPageUrl="~/Default/SelecionarCalendario.aspx"
                                OnLoginError="loginEntrada_LoginError" TitleText=""
                                AccessKey="M" BorderPadding="3" EnableTheming="True" Orientation="Horizontal"
                                OnAuthenticate="loginEntrada_Authenticate" PasswordRecoveryIconUrl="~/_layouts/images/attention16by16.gif" PasswordRecoveryUrl="~/Default/ResetSenha.aspx" PasswordRecoveryText="Esqueci">
                                <TitleTextStyle CssClass="ms-toolbar" Font-Bold="False" Font-Names="Verdana" Font-Size="12px" />
                                <CheckBoxStyle CssClass="ms-toolbar" />
                                <HyperLinkStyle CssClass="ms-toolbar" Font-Bold="True" ForeColor="Red" />
                                <InstructionTextStyle CssClass="ms-toolbar" />
                                <TextBoxStyle CssClass="ms-toolbar" Width="200px" />
                                <LoginButtonStyle CssClass="ms-toolbar" />
                                <LabelStyle CssClass="ms-toolbar" />
                            </asp:Login>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div align="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDataHora" runat="server" CssClass="ms-blogrss"></asp:Label>
            </div>
            <br />

<div class="container" runat="server" id="container">
    <!-- Labs -->
    <div class="row">
        <div class="category"><img src="/srrc/img/lab.png" alt=""> Labs</div>
        <div class="grid">
            <div class="block labs">301 <span>D. Ruiz</span></div>
            <div class="block labs">309 <span>D. Silva</span></div>
            <div class="block labs">310 <span>D. Silva</span></div>
            <div class="block labs">311 <span>M. Mangan</span></div>
            <div class="block labs">312 <span>D. Ruiz</span></div>
            <div class="block labs">318 <span>D. Venturini</span></div>
        </div>
    </div>

    <!-- Notebooks -->
    <div class="row">
        <div class="category"><img src="/srrc/img/notebook.png" alt=""> Notebooks</div>
        <div class="grid">
            <div class="block notebooks">508 <span>A. Bacelo</span></div>
            <div class="block notebooks">NB 1 <span>P. Alves</span></div>
            <div class="block notebooks">NB 2 <span>I. Jauris</span></div>
            <div class="block notebooks">NB 3 <span>A. Cardona</span></div>
            <div class="block notebooks">NB 4 <span>A. Cardona</span></div>
        </div>
    </div>

    <!-- Kits HDMI -->
    <div class="row">
        <div class="category"><img src="/srrc/img/cabo-hdmi.png" alt=""> Kits HDMI</div>
        <div class="grid">
            <div class="block kits-hdmi">KH 1 <span>S. Moraes</span></div>
            <div class="block kits-hdmi">KH 2 <span>P. Carneiro</span></div>
            <div class="block kits-hdmi">KH 3 <span>C. Marcon</span></div>
        </div>
    </div>

    <!-- Kits VGA -->
    <div class="row">
        <div class="category"><img src="/srrc/img/cabo-vga.png" alt=""> Kits VGA</div>
        <div class="grid">
            <div class="block kits-vga">KC 1 <span>I. Lara</span></div>
            <div class="block kits-vga">KC 2 <span>P. Carneiro</span></div>
            <div class="block kits-vga">KC 3 <span>M. Mora</span></div>
            <div class="block kits-vga">KC 4 <span>V. Venturini</span></div>
        </div>
    </div>

    <!-- Auditoriums -->
    <div class="row">
        <div class="category"><img src="/srrc/img/auditorio.png" alt=""> Auditórios</div>
        <div class="grid">
            <div class="block auditoriums">516 <span>A. Agustin</span></div>
            <div class="block auditoriums">517 <span>S. Filio</span></div>
        </div>
    </div>

    <!-- Sound Boxes -->
    <div class="row">
        <div class="category"><img src="/srrc/img/speaker.png" alt=""> Cx de Som</div>
        <div class="grid">
            <div class="block speakers">SOM 1 <span>N. Nunes</span></div>
            <div class="block speakers">SOM 2 <span>V. Villeneuve</span></div>
            <div class="block speakers">SOM 3 <span>R. Silva</span></div>
            <div class="block speakers">SOM 4 <span>H. Nunes</span></div>
        </div>
    </div>
</div>
<div class="legend">
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
</div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <!--meta name="viewport" content="width=device-width, initial-scale=1.0"-->
    
    <div align="center">
        <p style="padding: 4px; margin: 4px; text-align: center">
            O OpenSARC é um sistema para alocação de recursos computacionais. Além da solicitação
            de recursos durante o período de planejamento semestral,<br />
            o sistema permite agendar eventos, consultar datas de avaliações e trocar e transferir
            recursos durante todo o ano.<br />
            <br />
            O OpenSARC é <i>software</i> livre. Caso deseje participar, reclamar ou dar sugestões,
            visite <a href="https://github.com/mflash/OpenSARC">https://github.com/mflash/OpenSARC</a>.<br />
            <br />
            Em especial, aguardamos voluntários interessados em utilizar o sistema como estudo
            de caso para suas disciplinas de desenvolvimento de <i>software</i>.</p>
    </div>
</asp:Content>
