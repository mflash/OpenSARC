<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true"
    CodeFile="EditarAulaSemestre.aspx.cs" Inherits="Docentes_EditarAula" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="BusinessData.Util" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

    <script type="text/javascript" language="javascript">

function popitup(url) {
	newwindow=window.open(url,'name','width=400, height=300, menubar=no, resizable=no');
	if (window.focus) {newwindow.focus()}
	return false;
}

function popitup(url,h,w) {
	newwindow=window.open(url,'name','width='+h+', height='+w+', menubar=no, resizable=no');
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div align="left">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <uc1:Aguarde ID="Aguarde1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:Label ID="lblResultado" runat="server" CssClass="lblStatus" Text="" Visible="true">
            </asp:Label>
            <asp:CheckBox ID="chbAutoSave" runat="server" CssClass="ms-toolbar" Text="Auto Save"
                EnableViewState="true" Visible="false" /><div style="width: 244px; height: 30px">
                    <asp:Button ID="btnExportarHTML" runat="server" CssClass="ms-toolbar" OnClick="btnExportarHTML_Click"
                        Text="Exportar HTML" />
                    <asp:Button accesskey="S" ID="btnSalvarTudo" runat="server" CssClass="ms-toolbar" Text="Salvo"
                        OnClick="btnSalvarTudo_Click" Enabled="False" />
                </div>
            <asp:DataGrid ID="dgAulas" runat="server" AutoGenerateColumns="False" Width="100%"
                HorizontalAlign="Center" OnItemDataBound="dgAulas_ItemDataBound" DataKeyField="Id"
                OnItemCommand="dgAulas_ItemCommand">
                <ItemStyle CssClass="ms-toolbar" HorizontalAlign="Center" />
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
                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="ms-toolbar" 
                                Width="300px" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Atividade">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAtividade" runat="server" CssClass="ms-toolbar" onChange="setDirtyFlag();">
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
						<tr><td>
                            <asp:CheckBoxList ID="cbRecursos" runat="server" CssClass="UserConfiguration">
                            </asp:CheckBoxList>
                            </td>
						<td>
						<asp:ImageButton ID="butDeletar" runat="server" 
                                ImageUrl="~/_layouts/images/CRIT_16.GIF" onclick="butDeletar_Click" />
						    <br />
                            <asp:ImageButton ID="butTransferir" runat="server" 
                                ImageUrl="~/_layouts/images/PLNEXT1.GIF" />
                            <br />
                            <asp:ImageButton ID="butTrocar" runat="server" 
                                ImageUrl="~/_layouts/images/recurrence.gif"  />
						</td>
						</tr>
						</table>						
						
                         <asp:Label ID="lblRecursosAlocados" runat="server"
                                Width="250px" visible="false"></asp:Label>
                            <br>                            
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
            <div align="right">
                <asp:Button ID="btnSalvarTudo2" runat="server" CssClass="ms-toolbar" Text="Salvo"
                    OnClick="btnSalvarTudo_Click" Enabled="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
