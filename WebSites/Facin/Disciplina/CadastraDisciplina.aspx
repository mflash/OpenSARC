<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
 CodeFile="CadastraDisciplina.aspx.cs" Inherits="Recursos_Disciplina" Title="Sistema de Alocação de Recursos - FACIN" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%-- Add content controls here --%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphTitulo">
    <div align  = "left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblDisciplina" runat="server" CssClass="ms-WPTitle" Text="CADASTAR DISCIPLINA"></asp:Label></div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<uc1:Aguarde id="Aguarde1" runat="server"></uc1:Aguarde>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 510px" align=left><TBODY><TR><TD style="WIDTH: 93px" class="ms-toolbar">Código: </TD><TD><asp:TextBox id="txtCodigo" runat="server" MaxLength="5" Width="200px" Height="20px"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Código deve ser inserido" ControlToValidate="txtCodigo">*</asp:RequiredFieldValidator></TD></TR><TR><TD style="WIDTH: 93px; HEIGHT: 24px" class="ms-toolbar">Créditos:</TD><TD style="HEIGHT: 24px"><asp:TextBox id="txtCreditos" runat="server" Width="200px" Height="20px"></asp:TextBox> 
    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCreditos"
        ErrorMessage="Créditos deve ser um número maior que zero" MinimumValue="1" Type="Integer" MaximumValue="100">*</asp:RangeValidator>
    <asp:RequiredFieldValidator ID="rfvCreditos" runat="server" ControlToValidate="txtCreditos"
        ErrorMessage="Créditos deve ser inserido">*</asp:RequiredFieldValidator></TD></TR><TR><TD style="WIDTH: 93px; HEIGHT: 24px" class="ms-toolbar">Nome:</TD><TD style="HEIGHT: 24px"><asp:TextBox id="txtNome" runat="server" Width="200px" Height="20px"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Nome deve ser inserido" ControlToValidate="txtNome">*</asp:RequiredFieldValidator> </TD></TR><TR><TD style="WIDTH: 93px; HEIGHT: 24px" class="ms-toolbar">G2:</TD><TD style="HEIGHT: 24px"><asp:RadioButtonList id="rdbG2" runat="server" CssClass="ms-toolbar" Width="198px" BorderStyle="None" RepeatDirection="Horizontal" BackColor="White">
                    <asp:ListItem>N&#227;o</asp:ListItem>
                    <asp:ListItem>Sim</asp:ListItem>
                </asp:RadioButtonList><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Selecione opção de G2" ControlToValidate="rdbG2">*</asp:RequiredFieldValidator></TD></TR>
    <tr>
        <TD style="WIDTH: 93px" class="ms-toolbar">Calendário:</TD><TD><asp:DropDownList id="ddlCalendario" runat="server" CssClass="ms-toolbar" Width="200px" Height="20px">
                </asp:DropDownList></TD>
    </tr>
    <TR><TD style="WIDTH: 93px" class="ms-toolbar">
                    Categoria:</td>
        <TD>
            <asp:DropDownList id="ddlCategoria" runat="server" CssClass="ms-toolbar" Width="200px" Height="20px">
            </asp:DropDownList></td>
    </TR><TR><TD colSpan=2><asp:Button id="btnAdicionar" onclick="btnAdicionar_Click" runat="server" CssClass="ms-toolbar" Text="Adicionar" Height="20px"></asp:Button></TD></TR><TR><TD class="ms-toolbar" colSpan=2><asp:LinkButton id="lblStatus" runat="server" CssClass="lblstatus"></asp:LinkButton></TD></TR><TR><TD class="ms-toolbar" colSpan=2><asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="ms-toolbar" DisplayMode="List" HeaderText="*"></asp:ValidationSummary></TD></TR><TR><TD class="ms-toolbar" colSpan=2><asp:LinkButton id="lbtnVoltar" runat="server" CssClass="ms-toolbar" CausesValidation="False" OnClick="lbtnVoltar_Click">Voltar</asp:LinkButton></TD></TR></TBODY></TABLE>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="btnAdicionar" EventName="Click"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>
