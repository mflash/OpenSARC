<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="VisualizarAtividades.aspx.cs" Inherits="Secretarios_VisualizarAtividades" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    Selecione as categorias para o relatório:
    <br />
    
    <asp:CheckBoxList ID="cblCategoriasSelecionadas" runat="server" CssClass="ms-toolbar" 
        DataSourceID="ObjectDataSource1" DataTextField="Descricao" DataValueField="Id" 
        RepeatColumns="2" RepeatDirection="Horizontal">
    </asp:CheckBoxList>
    
    
    <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" 
        onclick="btnGerarRelatorio_Click" />
    
    
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetCategoriaAtividade" 
        TypeName="BusinessData.BusinessLogic.CategoriaAtividadeBO">
    </asp:ObjectDataSource>
    <br />
    <br />
    <asp:HiddenField ID="hdfIdsSelecionados" runat="server" />
    <asp:HiddenField ID="hdfIdCalendario" runat="server" />
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="BusinessData.Reports.SARCFACINDataSetTableAdapters.CriaRelatorioAtividadesTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdfIdsSelecionados" Name="IDsCategorias" 
                PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="hdfIdCalendario" DbType="Guid" 
                Name="IDCalendario" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>    
    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="SARCFACINDataSetTableAdapters.CriaRelatorioAtividadesTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdfIdsSelecionados" Name="IDsCategorias" PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="hdfIdCalendario" Name="IDCalendario" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" DataKeyNames="Cod" DataSourceID="ObjectDataSource3" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:BoundField DataField="Categoria de Atividade" HeaderText="Categoria de Atividade" SortExpression="Categoria de Atividade" />
            <asp:BoundField DataField="Descricao" HeaderText="Descricao" SortExpression="Descricao" />
            <asp:BoundField DataField="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data" SortExpression="Data" />
            <asp:BoundField DataField="Hora" HeaderText="Hora" SortExpression="Hora" />
            <asp:BoundField DataField="Turma" HeaderText="Turma" SortExpression="Turma" />
            <asp:BoundField DataField="Cod" HeaderText="Cod" ReadOnly="True" SortExpression="Cod" />
            <asp:BoundField DataField="Cred" HeaderText="Cred" SortExpression="Cred" />
            <asp:BoundField DataField="Disciplina" HeaderText="Disciplina" SortExpression="Disciplina" />
            <asp:BoundField DataField="Professor" HeaderText="Professor" SortExpression="Professor" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="#FFFFCC" Font-Bold="True" ForeColor="White" CssClass="ms-wikieditthird" Font-Italic="False" Font-Underline="False"  />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
</asp:Content>

