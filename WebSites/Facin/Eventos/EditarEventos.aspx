<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="EditarEventos.aspx.cs" Inherits="Eventos_EditarEventos" Title="Editar Evento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="Server">
    <div align = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="EDITAR EVENTO"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 415px">
                <tr>
                    <td style="width: 98px; height: 26px;" class="ms-toolbar">
                        Título</td>
                    <td colspan="2" style=" width: 317px; height: 26px" class="ms-toolbar">
                        <asp:TextBox ID="txtTitulo" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
                            CssClass="ms-toolbar" ErrorMessage="Digite um título.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 98px; height: 26px">
                        Responsável</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtResponsavel" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtResponsavel"
                            CssClass="ms-toolbar" ErrorMessage="Digite um Responsável pelo Evento.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 98px; height: 26px">
                        Unidade</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtUnidade" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtUnidade"
                            CssClass="ms-toolbar" ErrorMessage="Digite uma Unidade.">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="ms-toolbar" style="width: 98px; height: 40px">
                        Descrição</td>
                    <td class="ms-toolbar" colspan="2" style="width: 317px; height: 26px">
                        <asp:TextBox ID="txtaDescricao" runat="server" TextMode="MultiLine" Height="40px" Width="317px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="Panel1" runat="server" Height="50px" Width="415px">
                            <table style="width: 415px; height: 69px">
                                <tr>
                                    <td class="ms-toolbar" style="width: 92px; height: 26px;">
                                        Data</td>
                                    <td class="ms-toolbar" style="width: 323px">
                                        <asp:TextBox ID="txtData" runat="server"></asp:TextBox>
                                        <asp:ImageButton ID="ibtnAbrirCalendario" runat = "server" ImageUrl="~/_layouts/images/CALENDAR.GIF" CausesValidation="False" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtData"
                                            CssClass="ms-toolbar" ErrorMessage="Digite uma data de início.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtData"
                                            CssClass="ms-toolbar" ErrorMessage="Digite uma data no formato válido." ValidationExpression="\d{2}/\d{2}/\d{4}">*</asp:RegularExpressionValidator><br />
                                        <cc1:CalendarExtender ID="calextData"
                                        runat="server"
                                        TargetControlID="txtData"
                                        PopupButtonID = "ibtnAbrirCalendario"
                                        Format="dd/MM/yyyy"
                                        >
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-toolbar" style="width: 92px; height: 26px;">
                                        Horários</td>
                                    <td class="ms-toolbar" style="width: 323px; height: 40px;">
                        Inicio:<asp:DropDownList ID="ddlInicio" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlInicio_SelectedIndexChanged">
                            <asp:ListItem>AB</asp:ListItem>
                            <asp:ListItem>CD</asp:ListItem>
                            <asp:ListItem>E</asp:ListItem>
                            <asp:ListItem>FG</asp:ListItem>
                            <asp:ListItem>HI</asp:ListItem>
                            <asp:ListItem>JK</asp:ListItem>
                            <asp:ListItem>LM</asp:ListItem>
                            <asp:ListItem>NP</asp:ListItem>
                        </asp:DropDownList>
                        Até:<asp:DropDownList ID="ddlFim" runat="server">
                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 98px; height: 22px;" class="ms-toolbar">
                        Recorrência</td>
                    <td colspan="2" style="width: 317px; height: 22px">
                        <asp:CheckBox ID="ckbEhRecorrente" runat="server" AutoPostBack="True" Text="Tornar Recorrente" OnCheckedChanged="ckbEhRecorrente_CheckedChanged" CssClass="ms-toolbar" Enabled="False" /></td>
                </tr>
                <tr>
                    <td style="width: 98px; height: 9px;">
                    </td>
                    <td style="height: 9px; width: 98px;">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" Visible="false" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True" CssClass="ms-toolbar" Width="124px" Enabled="False">
                            <asp:ListItem Value="0">Diario</asp:ListItem>
                            <asp:ListItem Value="1">Selecionar Dias</asp:ListItem>
                            <asp:ListItem Value="2">Sele&#231;&#227;o Manual</asp:ListItem>
                        </asp:RadioButtonList></td>
                    <td style="width: 219px; height: 9px;">
                        <asp:Panel ID="pnlDias" runat="server" Height="50px" Width="172px" Visible="false">
                            <asp:CheckBox ID="seg" runat="server" Text="SEG" CssClass="ms-toolbar" />
                            &nbsp;
                            <asp:CheckBox ID="ter" runat="server" Text="TER" CssClass="ms-toolbar" />&nbsp;
                            <asp:CheckBox ID="qua"
                                runat="server" Text="QUA" CssClass="ms-toolbar" />
                            <asp:CheckBox ID="qui" runat="server" Text="QUI" CssClass="ms-toolbar" Width="44px" />
                            &nbsp; <asp:CheckBox ID="sex" runat="server" Text="SEX" CssClass="ms-toolbar" Width="34px" />&nbsp;
                            <asp:CheckBox ID="sab" runat="server" Text="SAB" CssClass="ms-toolbar" />
                            <asp:CheckBox ID="dom" runat="server" Text="DOM" CssClass="ms-toolbar" /></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel id="pnlSelecionarDatas" runat="server" Visible ="false">
                            <table id="tblSelecionarDatas" style="width: 415">
                                <tr>
                                    <td style="width: 92px; height: 26px;" class="ms-toolbar"> Data Final</td>
                                    <td style="height: 26px; width: 323px;">
                            <asp:TextBox ID="txtDataFinal" runat="server" Width="150px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/_layouts/images/CALENDAR.GIF" CausesValidation="False" /><asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataFinal"
                                            CssClass="ms-toolbar" ErrorMessage="Digite uma data de fim.">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDataFinal"
                                            CssClass="ms-toolbar" ErrorMessage="Digite a data no formato válido." ValidationExpression="\d{2}/\d{2}/\d{4}">*</asp:RegularExpressionValidator>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                        TargetControlID="txtDataFinal"
                        PopupButtonID = "ImageButton2"
                        Format="dd/MM/yyyy"
                        >
                            </cc1:CalendarExtender>
                        </td> 
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel id="Panel2" style="width: 415px" runat="server" Visible ="false" Width="415px">
                            <table id="Table1" style="width: 415px">
                                <tr>
                                    <td class="ms-toolbar" style="width: 92px">
                                        Data
                                    </td>
                                    <td style="width: 323px">
                        <asp:TextBox ID="txtDataFim" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnDataFim" runat = "server" ImageUrl="~/_layouts/images/CALENDAR.GIF" CausesValidation="False" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtDataFim"
                                            CssClass="ms-toolbar" ErrorMessage="Digite a data no formato válido." ValidationExpression="\d{2}/\d{2}/\d{4}">*</asp:RegularExpressionValidator>
                        <cc1:CalendarExtender ID="calexDataFim"
                        runat="server"
                        TargetControlID="txtDataFim"
                        PopupButtonID = "imgbtnDataFim"
                        Format="dd/MM/yyyy"
                        >
                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-toolbar" style="width: 92px">
                                        Horários</td>
                                    <td style="width: 323px" class="ms-toolbar">
                        Início:<asp:DropDownList ID="ddlHoraInicio" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlHoraInicio_SelectedIndexChanged">
                            <asp:ListItem>AB</asp:ListItem>
                            <asp:ListItem>CD</asp:ListItem>
                            <asp:ListItem>E</asp:ListItem>
                            <asp:ListItem>FG</asp:ListItem>
                            <asp:ListItem>HI</asp:ListItem>
                            <asp:ListItem>JK</asp:ListItem>
                            <asp:ListItem>LM</asp:ListItem>
                            <asp:ListItem>NP</asp:ListItem>
                        </asp:DropDownList>
                                        Até:<asp:DropDownList ID="ddlHoraFim" runat="server">
                        </asp:DropDownList></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar Horário" CssClass="ms-toolbar" OnClick="btnAdicionar_Click1" />
                            <asp:GridView ID="grdHorarios" runat="server" AutoGenerateColumns="False" Width="410px" HorizontalAlign="Left" OnRowDeleting="grdHorarios_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Data">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# ((DateTime)Eval("data")).ToShortDateString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Horario de Inicio">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("horarioInicio") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Horario de Fim">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("horarioFim") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="" DeleteText="Deletar" EditText="" InsertText="" NewText=""
                                    SelectText="" ShowCancelButton="False" ShowDeleteButton="True" UpdateText="" />
                            </Columns>
                            <RowStyle CssClass="ms-toolbar" />
                            <HeaderStyle CssClass="ms-wikieditthird" />
                        </asp:GridView>
                        </asp:Panel>
                    
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 26px">
                        <asp:Label ID="lblResultado" runat="server" CssClass="lblstatus"></asp:Label><br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ms-toolbar" Width="238px" />
                        </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 26px">
                        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click"
                         Text="OK" Width="95px" CssClass="ms-toolbar" /><br />
                        <asp:LinkButton ID="lbtnVoltar" runat="server" CssClass="ms-toolbar" OnClick="lbtnVoltar_Click1" CausesValidation="False">Voltar</asp:LinkButton></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

