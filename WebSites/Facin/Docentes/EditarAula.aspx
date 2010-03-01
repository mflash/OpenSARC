<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="EditarAula.aspx.cs" Inherits="Docentes_EditarAula" Title="Sistema de Alocação de Recursos Computacionais - FACIN" Trace="false" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

  <%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<%-- Add content controls here --%>
<%@ Import Namespace ="BusinessData.Util" %>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <script  type="text/javascript" language="javascript">

function popitup(url) {
	newwindow=window.open(url,'name','height=140,width=333');
	if (window.focus) {newwindow.focus()}
	return false;
}

var needToConfirm = false;

function setDirtyFlag()
{
needToConfirm = true;
b = $get('ctl00_cphTitulo_btnSalvarTudo');
b.value = "Salvar Agora";
b.disabled = false;
b = $get('ctl00_cphTitulo_btnSalvarTudo2');
b.value = "Salvar Agora";
b.disabled = false;
}

function releaseDirtyFlag()
{
needToConfirm = false;
}

window.onbeforeunload = 
function exitpop()
{
if (needToConfirm)
{
return "Suas alterações não foram salvas. Deseja descartar as alterações feitas?";
}
}

</script>

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="Aulas"></asp:Label>
    </div>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <div align="left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
             &nbsp;</div>
        <asp:Label ID="lblResultado" runat="server" CssClass="lblStatus" Text="" Visible="true"></asp:Label>
            <asp:CheckBox ID="chbAutoSave" runat="server" CssClass ="ms-toolbar" Text="Auto Save" EnableViewState="true"  Visible="false"/><div style="width: 244px; height: 30px">
                <asp:Button ID="btnExportarHTML" runat="server" CssClass="ms-toolbar" OnClick="btnExportarHTML_Click"
                    Text="Exportar HTML" />
                        <asp:Button ID="btnSalvarTudo" runat="server" CssClass="ms-toolbar" Text="Salvar Todos" OnClick="btnSalvarTudo_Click" /></div>
   <asp:DataGrid ID="dgAulas" 
                 runat="server"       
                 AutoGenerateColumns="False" 
                 Width="100%" 
                 HorizontalAlign="Center" 
                 OnItemDataBound="dgAulas_ItemDataBound" 
                 DataKeyField="Id" 
                 OnItemCommand="dgAulas_ItemCommand" BorderColor="#E0E0E0">
    
        <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center" BorderColor="#E0E0E0" BorderStyle="Inset" BorderWidth="1px"/>
        <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
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
            <asp:TemplateColumn HeaderText="Dia" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblDia" runat="server" Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblDiaEdit" runat="server" Text='<%#(DataHelper.GetDia((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label>
                </EditItemTemplate>
            </asp:TemplateColumn>
                
            <asp:TemplateColumn HeaderText="Data" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblData" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
                     
            <asp:TemplateColumn HeaderText="Hora" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblHora" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Data Hora">
                <ItemTemplate>
                            <asp:Label ID="lblData2" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yy")%>'></asp:Label>
                            <asp:Label ID="lblDia2" runat="server" Text='<%#(DataHelper.GetDiaPUCRS((DayOfWeek)((DateTime)DataBinder.Eval(Container.DataItem, "Data")).DayOfWeek))%>'></asp:Label><asp:Label
                                ID="lblHora2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hora") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Descri&#231;&#227;o">
                <ItemTemplate>
                    <asp:TextBox ID="txtDescricao" runat="server" style="resize:none;" CssClass="ms-toolbar" Height="38px"
                        Width="400px" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Atividade">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlAtividade" runat="server" CssClass="ms-toolbar" AutoPostBack=true OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged" >
                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Recurso">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlRecurso" runat="server" CssClass="ms-toolbar" 
                        onselectedindexchanged="ddlRecurso_SelectedIndexChanged" AutoPostBack=true>
                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
                       
            <asp:TemplateColumn HeaderText="Selecionados" >
                <ItemTemplate>
                    <asp:Label ID="lblRecursosSelecionados" runat="server"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:ButtonColumn CommandName="Select" Text="Editar..."></asp:ButtonColumn>
            
            <asp:TemplateColumn HeaderText="CorDaData" Visible="False"> 
                <ItemTemplate>
                    <asp:Label ID="lblCorDaData" runat="server" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="DescData" Visible="False"> 
                <ItemTemplate>
                    <asp:Label ID="lblDescData" runat="server" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            
                   
        </Columns>
    </asp:DataGrid>
    <div align="right">
        &nbsp;&nbsp;
                        <asp:Button ID="btnSalvarTudo2" runat="server" CssClass="ms-toolbar" Text="Salvar Todos"  OnClick="btnSalvarTudo_Click"/>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
  
</asp:Content>
