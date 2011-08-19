<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="VisualizarAtividades.aspx.cs" Inherits="Secretarios_VisualizarAtividades" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

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
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="600px" Width="100%">
        <LocalReport ReportPath="Secretarios\Report.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                    Name="SARCFACINDataSet_CriaRelatorioAtividades" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
</asp:Content>

