<%@ Page Language="C#" MasterPageFile="~/Master/MasterBootstrap.master" AutoEventWireup="true"
    CodeFile="ListaTurmas2.aspx.cs" Inherits="Pagina2" 
    Title="Sistema de Alocação de Recursos Computacionais - FACIN" %>

<%@ Register Src="../UserControls/SelecionaCalendario.ascx" TagName="SelecionaCalendario" TagPrefix="uc2" %>
<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter"></div>
                    <div id="processMessage">
                        <uc1:Aguarde ID="Aguarde1" runat="server" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!-- ═══════════════════════════════════════
                 TÍTULO
            ═══════════════════════════════════════ -->
            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                <i class="bi bi-list-ul me-2 text-primary"></i>
                <asp:Label ID="Label1" runat="server"
                    CssClass="fw-bold text-uppercase text-primary fs-6 mb-0"
                    Text="Lista de Turmas" />
            </div>

            <!-- ═══════════════════════════════════════
                 MENSAGEM DE STATUS
            ═══════════════════════════════════════ -->
            <asp:Label ID="lblStatus" runat="server"
                CssClass="alert alert-info d-block mb-3"
                Visible="False" />

            <!-- ═══════════════════════════════════════
                 GRID DE TURMAS
            ═══════════════════════════════════════ -->
            <div class="table-responsive mb-3">
                <asp:GridView ID="grvListaTurmas" runat="server"
                    Width="100%"
                    AutoGenerateColumns="False"
                    DataKeyNames="Id"
                    AllowSorting="True"
                    CssClass="table table-bordered table-hover table-sm align-middle"
                    HeaderStyle-CssClass="table-primary text-center fw-semibold"
                    RowStyle-CssClass="align-middle"
                    AlternatingRowStyle-CssClass="table-light">

                    <Columns>

                        <asp:BoundField DataField="Id" Visible="False" HeaderText="ID" />

                        <asp:BoundField DataField="Infra" HeaderText="Infra" HtmlEncode="False">
                            <ItemStyle CssClass="text-center" Width="50px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Disciplina.NomeCodCred" HeaderText="Disciplina">
                            <ItemStyle Width="320px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Numero" HeaderText="Número">
                            <ItemStyle CssClass="text-center" Width="80px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>


                        <asp:BoundField DataField="DataHora" HeaderText="Data & Hora">
                            <ItemStyle Width="180px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Professor" HeaderText="Professor">
                            <ItemStyle Width="200px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Sala" HeaderText="Sala" SortExpression="Sala">
                            <ItemStyle CssClass="text-center" Width="100px" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:BoundField>

                    </Columns>

                </asp:GridView>
            </div>

            <!-- ═══════════════════════════════════════
                 BOTÃO VOLTAR
            ═══════════════════════════════════════ -->
            <div class="d-flex justify-content-start">
                <asp:LinkButton ID="LinkButton1" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    OnClick="lbtnVoltar_Click">
                    <i class="bi bi-arrow-left me-1"></i>Voltar
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

