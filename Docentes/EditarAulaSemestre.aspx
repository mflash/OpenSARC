<%@ Page Language="C#" MasterPageFile="~/Master/Master2.master" AutoEventWireup="true"
    CodeFile="EditarAulaSemestre.aspx.cs" Inherits="Docentes_EditarAula" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="BusinessData.Util" %>
<%-- Add content controls here --%>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">

<style>

textarea.normal{
/*background-color:FFFFFF;*/
font-family:verdana;
font-size:8pt;
text-decoration:none;
color:#003399;
}

textarea.changed{
/*background-color:FF0000;*/
font-family:verdana;
font-size:8pt;
text-decoration:none;
color:#ff0000;
}
</style>

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

function testAlert(txt, num) 
{
    txt.style.color = '#FF0000';    
    c = $get('ctl00_cphTitulo_dgAulas_ctl'+num+'_cbChanged');
    c.checked = true;
	b = $get('ctl00_cphTitulo_dgAulas_ctl'+num+'_butConfirm');
	b.src = '../_layouts/images/STAR.gif';
	b.disabled=false;
    //alert(num);
    setDirtyFlag(); 
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
return "Suas altera��es n�o foram salvas. Deseja descartar as altera��es feitas?";
}
}


    </script>

    <div align="left" class="ms-menutoolbar" style="width: 100%; height: 14px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="ms-WPTitle" Text="Aulas"></asp:Label>
        <asp:Label ID="lblHoras" runat="server" CssClass="ms-toolbar" Text="Horas-relogio:"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    	<asp:Button ID="Button1" runat="server" CssClass="ms-toolbar" OnClick="btnExportarHTML_Click" ToolTip="Faz download de um arquivo HTML com o cronograma"
                    Text="Exportar HTML" />
        <asp:HyperLink ID="Link1" runat="server" CssClass="ms-toolbar" NavigateUrl="" ToolTip="Este link pode ser usado em qualquer lugar para visualizar o cronograma"
                    Text="Link para HTML" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblCals" runat="server" CssClass="ms-consolestatus" Text="Compartilhar calend�rio:"></asp:Label>
        <asp:HyperLink ID="Link2" runat="server" CssClass="ms-toolbar" NavigateUrl="" ToolTip="Clique aqui para importar o cronograma como um calend�rio no Google Calendar"
                    Text="Google Calendar"  Target="_blank"/>&nbsp;&nbsp;
        <asp:HyperLink ID="Link3" runat="server" CssClass="ms-toolbar" NavigateUrl="" ToolTip="Este link pode ser usado para importar o cronograma no Outlook/Apple Calendar"
                    Text="Link para webcal" />&nbsp;&nbsp;
        <asp:HyperLink ID="Link4" runat="server" CssClass="ms-toolbar" NavigateUrl="" ToolTip="Download do arquivo .ics (formato iCal)"
            Text="Download arquivo .ics" />

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
            <asp:Label ID="lblResultado" runat="server" CssClass="lblStatus" Text="" Visible="true">
            </asp:Label>
            <asp:CheckBox ID="chbAutoSave" runat="server" CssClass="ms-toolbar" Text="Auto Save"
                EnableViewState="true" Visible="false" /><div style="width: 244px; height: 30px">
                    
                    <asp:Button accesskey="S" ID="btnSalvarTudo" runat="server" CssClass="ms-toolbar" Text="Salvo"
                        OnClick="btnSalvarTudo_Click" Enabled="False" />
                </div>
            <asp:DataGrid ID="dgAulas" runat="server" AutoGenerateColumns="False" Width="100%"
                HorizontalAlign="Center" OnItemDataBound="dgAulas_ItemDataBound" DataKeyField="Id"
                >
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
							<table><tr><td>
                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="ms-toolbar" 
                                Width="300px" TextMode="MultiLine" 
                                Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>' 
                                AutoPostBack="False"></asp:TextBox>
								</td><td>
							<asp:ImageButton ID="butConfirm" Enabled="False" runat="server"
									OnClick="btnSalvarTudo_Click" ImageURL="~/_layouts/images/STARgray.gif" />
							<asp:CheckBox ID="cbChanged" style="display: none" runat="server"></asp:CheckBox>
							</td></tr></table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Atividade">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAtividade" AutoPostback="true" runat="server" CssClass="ms-toolbar" OnSelectedIndexChanged="ddlAtividade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Recursos Dispon�veis">
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
                                ImageUrl="~/_layouts/images/CRIT_16.GIF" onclick="butDeletar_Click" title="Liberar recurso" />
						    <br />
                            <asp:ImageButton ID="butTransferir" runat="server" 
                                onclick="butTransferir_Click" ImageUrl="~/_layouts/images/PLNEXT1.GIF" title="Transferir recurso" />
                            <br />
                            <asp:ImageButton ID="butTrocar" runat="server" 
                                onclick="butTrocar_Click" ImageUrl="~/_layouts/images/recurrence.gif" title="Trocar recurso" />
						</td>
						</tr>
						</table>												
                         <asp:Label ID="lblRecursosAlocados" runat="server"
                                Width="250px" visible="false"></asp:Label>                            
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
