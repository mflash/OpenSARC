<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="Cadastro.aspx.cs" Inherits="Calendario_Cadastro" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="controlToolkit" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblCalendario" runat="server" CssClass="ms-WPTitle" Text="CADASTRAR SEMESTRE"></asp:Label></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
       <table align=left style="width: 331px; height: 109px">
        <tr>
            <td colspan="3" rowspan="1" style="font-weight: bold; font-size: 10px; color: black;
                font-family: Verdana; height: 14px">
                Ano:
                <asp:TextBox ID="txtAno" runat="server" CssClass="ms-toolbar" MaxLength="4" Width="53px">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAno" runat="server" 
                                            ControlToValidate="txtAno"
                                            ErrorMessage="Digite um Ano."
                                            ValidationGroup="anoSemestre">
                                            *
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revAno" runat="server" ControlToValidate="txtAno"
                    ErrorMessage="Ano inválido" ValidationExpression="\d\d\d\d">*</asp:RegularExpressionValidator><br />
                
                Semestre:&nbsp;
                
                <asp:DropDownList ID="ddlSemestre" runat="server" 
                                    AutoPostBack="True" CssClass="ms-toolbar">
                    <asp:ListItem Value="0">Selecione</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                </asp:DropDownList>
                
                <asp:RangeValidator ID="rvSemestre" 
                                    runat="server" 
                                    ControlToValidate="ddlSemestre"
                                    ErrorMessage="Selecione um Semestre." 
                                    MaximumValue="2" 
                                    MinimumValue="1" 
                                    ValidationGroup="anoSemestre">*</asp:RangeValidator></td>
        </tr>
           <tr>
               <td class="ms-toolbar" style="width: 158px">
                   <asp:Button ID="btnCarregaDatas" runat="server" OnClick="btnCarregaDatas_Click" Text="Carregar Calendário" CssClass="ms-toolbar" ValidationGroup="anoSemestre" /></td>
               <td colspan="2" rowspan="1" style="font-weight: bold; font-size: 10px; color: black;
                   font-family: Verdana">
                   </td>
           </tr>
           <tr>
               <td class="ms-toolbar" style="width: 158px; height: 15px;">
               </td>
               <td colspan="2" rowspan="1" style="font-weight: bold; font-size: 10px; color: black;
                   font-family: Verdana; height: 15px;">
               </td>
           </tr>
        <tr>
            <td class="ms-toolbar" colspan="3" style="height: 20px">
                Datas Adicionadas:&nbsp;</td>
        </tr>
           <tr>
               <td class="ms-toolbar" colspan="3" style="height: 100px">
                <asp:ListBox ID="lbDatas" runat="server" Width="100%" Height="100%"></asp:ListBox></td>
           </tr>
        <tr>
            <td class="ms-toolbar" colspan="1" style="height: 15px; width: 158px;">
            </td>
            <td colspan="2" style="height: 15px; text-align: right">
                <asp:Button ID="btnExcluirData" runat="server" OnClick="btnExcluirData_Click" Text="Excluir Data" CssClass="ms-toolbar" /></td>
        </tr>
           <tr>
               <td class="ms-toolbar" colspan="1" style="width: 158px; height: 20px">
                Data:</td>
               <td align="left" class="ms-toolbar" colspan="2" style="height: 20px;" >
                                Categoria:</td>
           </tr>
        <tr  valign="top">
            <td class="ms-toolbar" colspan="1" style="height: 144px; width: 158px;">
                <asp:Calendar ID="clndario" runat="server" CssClass="ms-toolbar" Width="150px"></asp:Calendar>
            </td>
            <td colspan="2" style="height: 144px;" align="left" >
                <table align=left style="width: 31px; height: 30">
                <tr>
                <td style="width: 182px"> 
                <asp:DropDownList ID="ddlCategoria" runat="server" Width="170px" CssClass="ms-toolbar">
                </asp:DropDownList></td>
                </tr>
                    <tr>
                        <td style="width: 182px">
                <asp:Button ID="btnAdicionarData" runat="server" CssClass="ms-toolbar" Text="Adicionar Data" OnClick="btnAdicionarData_Click" /></td>
                    </tr>
                </table>
                </td>
        </tr>
        <tr>
            <td class="ms-toolbar" colspan="1" style="height: 10px; width: 158px;">
            </td>
            <td colspan="2" style="height: 10px">
                </td>
        </tr>
           <tr>
               <td class="ms-toolbar" colspan="3"><table align=left style="width: 331px; height: 109px">
                   <tr>
               <td class="ms-toolbar" colspan="1" style="width: 57px; height: 21px;">
                   Início G1:</td>
               <td style="width: 180px; height: 21px;">
                   <asp:TextBox ID="txtInicioG1" runat="server" CssClass="ms-toolbar" MaxLength="10"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="rfvInicioG1" runat="server" ControlToValidate="txtInicioG1"
                       ErrorMessage="Início de G1 não pode ser nulo">*</asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="revInicioG1" runat="server" ControlToValidate="txtInicioG1"
                       ErrorMessage="Campo Inicio G1 Inválido" ValidationExpression="((((0[1-9])|([1-2][0-9])|(3[0-1]))/((0[1-9])|(1[0-2]))/(\d\d\d\d))|((([1-9])|([1-2][0-9])|(3[0-1]))/(([1-9])|(1[0-2]))/(\d\d\d\d)))">*</asp:RegularExpressionValidator></td>
                   </tr>
                   <tr>
               <td class="ms-toolbar" colspan="1" style="width: 57px; height: 31px;">
                   Início G2:</td>
               <td style="width: 180px; height: 31px;">
                   <asp:TextBox ID="txtInicioG2" runat="server" CssClass="ms-toolbar"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="rfvInicioG2" runat="server" ControlToValidate="txtInicioG2"
                       ErrorMessage="Início G2 não pode ser nulo">*</asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="revInicioG2" runat="server" ControlToValidate="txtInicioG2"
                       ErrorMessage="Campo Inicio G2 Inválido" ValidationExpression="((((0[1-9])|([1-2][0-9])|(3[0-1]))/((0[1-9])|(1[0-2]))/(\d\d\d\d))|((([1-9])|([1-2][0-9])|(3[0-1]))/(([1-9])|(1[0-2]))/(\d\d\d\d)))">*</asp:RegularExpressionValidator></td>
                   </tr>
                   <tr>
               <td class="ms-toolbar" colspan="1" style="height: 20px; width: 57px;">
                   Fim G2:</td>
               <td style="width: 180px; height: 20px">
                   <asp:TextBox ID="txtFimG2" runat="server" CssClass="ms-toolbar"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="rfvFimG2" runat="server" ControlToValidate="txtInicioG2"
                       ErrorMessage="Fim de G2 não pode ser nulo">*</asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="revFimG2" runat="server" ControlToValidate="txtFimG2"
                       ErrorMessage="Campo Fim G2 Inválido" ValidationExpression="((((0[1-9])|([1-2][0-9])|(3[0-1]))/((0[1-9])|(1[0-2]))/(\d\d\d\d))|((([1-9])|([1-2][0-9])|(3[0-1]))/(([1-9])|(1[0-2]))/(\d\d\d\d)))">*</asp:RegularExpressionValidator></td>
                   </tr>
                   <tr>
                       <td class="ms-toolbar" colspan="1" style="width: 57px; height: 20px">
                       </td>
                       <td style="width: 180px; height: 20px">
                   <asp:Button ID="btnSalvar" runat="server" CssClass="ms-toolbar" Text="Salvar" 
                   OnClick="btnSalvar_Click" /></td>
                   </tr>
               </table>
               </td>
           </tr>
        <tr>
            <td style="width: 158px">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></td>
            <td style="width: 180px">
            </td>
            <td style="width: 432px">
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 20px">
                <asp:Label ID="lblStatus" runat="server" CssClass="lblstatus"></asp:Label></td>
        </tr>
    </table>
   
        </ContentTemplate>   
   
    </asp:UpdatePanel>
    <div align ="left">
                   <asp:ValidationSummary ID="vsAnoSemestre" runat="server" CssClass="ms-toolbar" ValidationGroup="anoSemestre" />
                   <asp:ValidationSummary ID="vsCalendarios" runat="server" CssClass="ms-toolbar" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
