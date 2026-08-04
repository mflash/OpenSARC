<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="EditarAulaSemestre.aspx.cs" Inherits="Docentes_EditarAula" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="BusinessData.Util" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <style>
        textarea.normal {
            /*background-color:FFFFFF;*/
            font-family: verdana;
            font-size: 8pt;
            text-decoration: none;
            color: #003399;
        }

        textarea.changed {
            /*background-color:FF0000;*/
            font-family: verdana;
            font-size: 8pt;
            text-decoration: none;
            color: #ff0000;
        }

    #ctl00_cphTitulo_btnSalvarTudo {
        opacity: 0.5;
        pointer-events: none;
    }

    [id$="dgAulas"] tr[style] td,
    [id$="dgAulas"] tr[style] th {
        background-color: inherit !important;
        --bs-table-bg: inherit !important;
        --bs-table-accent-bg: inherit !important;
    }
    </style>

    <script type="text/javascript" language="javascript">

        function popitup(url) {
            newwindow = window.open(url, 'name', 'width=400, height=300, menubar=no, resizable=no');
            if (window.focus) { newwindow.focus() }
            return false;
        }

        function popitup(url, h, w) {
            newwindow = window.open(url, 'name', 'width=' + h + ', height=' + w + ', menubar=no, resizable=no');
            if (window.focus) { newwindow.focus() }
            return false;
        }

        var needToConfirm = false;

        function setDirtyFlag() {
            needToConfirm = true;
            b = $get('ctl00_cphTitulo_btnSalvarTudo');
            b.value = "Salvar Agora";
            b.style.opacity = '1';
            b.style.pointerEvents = 'auto';
            //b.disabled = false;
            //b = $get('ctl00_cphTitulo_btnSalvarTudo2');
            //b.value = "Salvar Agora";
            //b.disabled = false;
        }

        function testAlert(txt, num) {
            txt.style.color = '#FF0000';
            c = $get('ctl00_cphTitulo_dgAulas_ctl' + num + '_cbChanged');
            c.checked = true;
            b = $get('ctl00_cphTitulo_dgAulas_ctl' + num + '_butConfirm');
            b.src = '../_layouts/images/STAR.gif';
            b.disabled = false;
            //alert(num);
            setDirtyFlag();
        }

        function releaseDirtyFlag() {
            b = $get('ctl00_cphTitulo_btnSalvarTudo');
            b.value = "Salvo";
            b.style.opacity = '0.5';
            b.style.pointEvents = none;
            needToConfirm = false;
        }

        window.onbeforeunload =
            function exitpop() {
                if (needToConfirm) {
                    return "Suas alterações não foram salvas. Deseja descartar as alterações feitas?";
                }
            }

        // Alerta usando bootstrap
        function showBootstrapAlert(message) {
            var html = '<div class="alert alert-danger alert-dismissible fade show" role="alert">'
                + message
                + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>'
                + '</div>';
            document.getElementById('alertContainer').innerHTML = html;
        }

        // Quando o usuário seleciona um arquivo, dispara o submit automaticamente
        document.addEventListener('DOMContentLoaded', function () {
            var fileInput = document.getElementById('ctl00_cphTitulo_csvUpload');
            if (fileInput) {
                fileInput.addEventListener('change', function () {
                    if (this.files.length > 0) {
                        // Show the same progress overlay used by async postbacks
                        var overlay = document.getElementById('modernProgressOverlay');
                        var message = document.getElementById('modernProgressMessage');
                        if (overlay) overlay.style.display = 'flex';
                        if (message) message.style.display = 'block';
                        document.getElementById('ctl00_cphTitulo_btnImportarCSVSubmit').click();
                    }
                });
            }
        });


    </script>

    <!-- ═══════════════════════════════════════
      TÍTULO + HORAS
 ═══════════════════════════════════════ -->
    <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
        <i class="bi bi-calendar3 me-2 text-primary"></i>
        <asp:Label ID="lblTitulo" runat="server"
            CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
            Text="Aulas do Semestre" />
        <asp:Label ID="lblHoras" runat="server"
            CssClass="ms-3 text-muted"
            Text="Horas-relógio:" />
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <!-- ═══════════════════════════════════════
                 TOOLBAR: EXPORTAR & COMPARTILHAR
            ═══════════════════════════════════════ -->
    <div class="card mb-3 border-0 shadow-sm">
        <div class="card-body py-2">
            <div class="d-flex flex-wrap align-items-center gap-2">
                <!-- Exportar -->
                <span class="text-muted fw-semibold me-1">
                    <i class="bi bi-download me-1"></i>Exportar:
                </span>
                <asp:Button ID="Button1" runat="server"
                    OnClick="btnExportarHTML_Click"
                    ToolTip="Faz download de um arquivo HTML com o cronograma"
                    CssClass="btn btn-sm btn-outline-primary"
                    Text="HTML" />
                <asp:Button ID="Button2" runat="server"
                    OnClick="btnExportarCSV_Click"
                    ToolTip="Faz download de um arquivo CSV com o cronograma para o sistsema de atas"
                    CssClass="btn btn-sm btn-outline-secondary"
                    Text="CSV/Atas" />
                <asp:HyperLink ID="Link1" runat="server"
                    NavigateUrl=""
                    ToolTip="Este link pode ser usado em qualquer lugar para visualizar o cronograma"
                    CssClass="btn btn-sm btn-outline-info"
                    Text="Link HTML" />

                <!-- Separador visual -->
                <div class="vr mx-1" style="height: 24px;"></div>

                <!-- Compartilhar -->
                <span class="text-muted fw-semibold me-1">
                    <i class="bi bi-share me-1"></i>Compartilhar:
                </span>
                <asp:HyperLink ID="Link2" runat="server"
                    NavigateUrl=""
                    Target="_blank"
                    ToolTip="Clique aqui para importar o cronograma como um calendário no Google Calendar"
                    CssClass="btn btn-sm btn-outline-success">
                            <i class="bi bi-google me-1"></i>Google
                </asp:HyperLink>
                <asp:HyperLink ID="Link3" runat="server"
                    NavigateUrl=""
                    ToolTip="Este link pode ser usado para importar o cronograma no Outlook/Apple Calendar"
                    CssClass="btn btn-sm btn-outline-primary">
                            <i class="bi bi-calendar-week me-1"></i>Webcal
                </asp:HyperLink>
                <asp:HyperLink ID="Link4" runat="server"
                    NavigateUrl=""
                    ToolTip="Download do arquivo .ics (formato iCal)"
                    CssClass="btn btn-sm btn-outline-secondary">
                            <i class="bi bi-download me-1"></i>.ics
                </asp:HyperLink>

                <!-- Separador visual -->
                <div class="vr mx-1" style="height: 24px;"></div>

                <span class="text-muted fw-semibold me-1">
                    <i class="bi bi-download me-1"></i>Importar:
                </span>

                <asp:Button ID="btnImportarCSV" runat="server"
                            ToolTip="Importa cronograma a partir do CSV do sistema de atas"
                            CssClass="btn btn-sm btn-outline-secondary"
                            Text="CSV/Atas"
                            OnClientClick="document.getElementById('ctl00_cphTitulo_csvUpload').click(); return false;"/>

                        <asp:FileUpload ID="csvUpload" runat="server" Style="display: none" />

                        <%-- Botão oculto que dispara o postback completo após a seleção do ficheiro --%>
                <asp:Button ID="btnImportarCSVSubmit" runat="server"
                            OnClick="btnImportarCSV_Click"
                            Style="display: none;"/>

                <!-- Separador visual -->
                <div class="vr mx-1" style="height: 24px;"></div>

                <!-- Botão Salvar -->
                <asp:Button ID="btnSalvarTudo" runat="server"
                    CssClass="btn btn-primary btn-sm"
                    Text="Salvo"
                    UseSubmitBehavior="false"
                    OnClick="btnSalvarTudo_Click"
                    accesskey="S"
                    Enabled="True" />
            </div>
        </div>
    </div>

            <div id="alertContainer"></div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div align="left">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter"></div>
                        <div id="processMessage">
                            <uc1:Aguarde ID="Aguarde1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:Label ID="lblResultado" runat="server" CssClass="altert alert-info d-inline-block py-1 px-2 small" Text="" Visible="true">
            </asp:Label>
            <asp:CheckBox ID="chbAutoSave" runat="server" CssClass="ms-toolbar" Text="Auto Save"
                EnableViewState="true" Visible="false" />

            <div class="table-responsive mt-3">
                <asp:DataGrid ID="dgAulas" runat="server" AutoGenerateColumns="False" Width="100%"
                    HorizontalAlign="Center" OnItemDataBound="dgAulas_ItemDataBound" DataKeyField="Id"
                    CssClass="table table-bordered table-hover table-sm align-middle">
                    <ItemStyle CssClass="text-center align=middle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="table-primary text-center fw-semibold" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="AulaId" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblAulaId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="lblAula" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle VerticalAlign="Middle" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Dia" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblDia" runat="server" Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblDiaEdit" runat="server" Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Data" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Data Hora">
                            <ItemTemplate>
                                <asp:Label ID="lblData2" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yy")%>'></asp:Label>
                                <asp:Label ID="lblDia2" runat="server" Text='<%#(DataHelper.GetDiaPUCRS((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label><asp:Label
                                    ID="lblHora2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Hora" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle VerticalAlign="Middle" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Descri&#231;&#227;o">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="ms-toolbar"
                                                Width="300px" TextMode="MultiLine"
                                                Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'
                                                AutoPostBack="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="butConfirm" Enabled="False" runat="server"
                                                OnClick="btnSalvarTudo_Click" ImageUrl="~/_layouts/images/STARgray.gif" />
                                            <asp:CheckBox ID="cbChanged" Style="display: none" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Atividade">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlAtividade" AutoPostBack="true" runat="server" CssClass="ms-toolbar" OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Recursos Disponíveis">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDisponiveis" runat="server" CssClass="ms-toolbar" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlDisponiveis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>

                            </EditItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Recursos_Alocados_id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRecursosAlocadosId" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Recursos Selecionados" Visible="True">
                            <ItemTemplate>
                                <asp:Panel ID="pnRecursos" runat="server">

                                    <table id="tabRecursos" runat="server">
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="cbRecursos" runat="server" CssClass="UserConfiguration">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="butDeletar" runat="server"
                                                    ImageUrl="~/_layouts/images/CRIT_16.GIF" OnClick="butDeletar_Click" title="Liberar recurso" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblRecursosAlocados" runat="server"
                                        Width="250px" Visible="false"></asp:Label>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="CorDaData" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCorDaData" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="DescData" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblDescData" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvarTudo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
