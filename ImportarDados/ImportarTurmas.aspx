<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="ImportarTurmas.aspx.cs" Inherits="ImportarDados_Default" Title="Untitled Page" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    &nbsp;
    <div align="left">
    <table style="width: 209px; height: 70px">
        <tr>
            <td style="width: 3px">
                Calendário:&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 3px"> </td>
        </tr>
        <tr>
            <td style="width: 3px; height: 18px;">
    <asp:LinkButton ID="lbtnImportarTurmas" runat="server" OnClick="lbtnImportarTurmas_Click" Width="159px">Importar Turmas</asp:LinkButton></td>
        </tr>
    </table>
        <asp:Wizard ID="wzImportarTurmas" runat="server" ActiveStepIndex="1" 
            OnFinishButtonClick="wzImportarTurmas_FinishButtonClick" 
            FinishCompleteButtonText="Import" StepNextButtonText="" 
            StepPreviousButtonText="">
            <WizardSteps>
                <asp:WizardStep runat="server" Title="Turmas com Dados Incompletos">
            <asp:GridView ID="grvTurmasNone" runat="server" AutoGenerateColumns="False" Width="601px" OnRowEditing="grvTurmasNone_RowEditing" OnRowCancelingEdit="grvTurmasNone_RowCancelingEdit" OnRowUpdating="grvTurmasNone_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Turma" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Disciplina" HeaderText="Disciplina" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Curso" HeaderText="Curso" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Professor">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProfessores" runat="server" >
                            </asp:DropDownList>
                            
                        </EditItemTemplate>
                        <HeaderStyle CssClass="ms-wikieditthird" />
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DataHora" HeaderText="Horario" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:CommandField CancelText="Cancelar" EditText="Editar" ShowEditButton="True" />
                </Columns>
                <RowStyle CssClass="ms-toolbar" />
            </asp:GridView>
                </asp:WizardStep>
                <asp:WizardStep runat="server" Title="Turmas com Dados Completos">
            <asp:GridView ID="grvListaTurmaOk" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                Height="87px" Width="603px" OnRowEditing="grvListaTurmaOk_RowEditing" OnRowUpdating="grvListaTurmaOk_RowUpdating"  OnRowCancelingEdit="grvListaTurmaOk_RowCancelingEdit">
                <Columns>
                    <asp:BoundField HeaderText="Turma" DataField="Numero" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Disciplina" HeaderText="Disciplina" ReadOnly="True" >
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Curso" HeaderText="Curso" ReadOnly="True">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Professor">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProfessoresOk" runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle CssClass="ms-wikieditthird" />
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlProfessoresOk" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Horario">
                        <HeaderStyle CssClass="ms-wikieditthird" />
                        <ItemTemplate>
                            <asp:Label ID="lblHorario" runat="server" Text='<%#Bind(Container.DataItem, "DataHora") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtHorario" runat="server" Text='<%#Bind(Container.DataItem, "DataHora") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    
                    
                    <asp:CommandField ShowEditButton="True" EditText="Editar" />
                </Columns>
                <RowStyle CssClass="ms-toolbar" />
                <HeaderStyle CssClass="cabecalhoTabela" />
            </asp:GridView>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
        &nbsp;
        <asp:Label ID="lblSucesso" runat="server"></asp:Label><br />
        &nbsp;</div>
</asp:Content>
