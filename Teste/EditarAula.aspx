<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" 
CodeFile="EditarAula.aspx.cs" Inherits="Professores_EditarAula" Title="Untitled Page" %>

<%@ Register Src="../Default/Aguarde.ascx" TagName="Aguarde" TagPrefix="uc1" %>

  <%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<%-- Add content controls here --%>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphTitulo">
    <asp:Label ID="lblTitulo" runat="server" CssClass="ms-pagetitle"></asp:Label><br />
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <div align="left">
            <asp:CheckBox ID="chbAutoSave" runat="server" CssClass ="ms-toolbar" Text="Auto Save" EnableViewState="true" />
         </div>
        <asp:Label ID="lblResultado" runat="server" CssClass="lblStatus" Text="" Visible="true"></asp:Label>
            <div align="right">
            <table style="width: 67px">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSalvarTudo" runat="server" CssClass="ms-toolbar" Text="Salvar Todos" OnClick="btnSalvarTudo_Click" /></td>
                </tr>
            </table>
            </div>
   <asp:DataGrid ID="dgAulas" 
                 runat="server"       
                 AutoGenerateColumns="False" 
                 Width="100%" 
                 HorizontalAlign="Center" 
                 OnItemDataBound="dgAulas_ItemDataBound" 
                 DataKeyField="Id" 
                 OnItemCommand="dgAulas_ItemCommand">
    
        <ItemStyle CssClass="ms-toolbar"  HorizontalAlign="Center"/>
        <HeaderStyle CssClass="ms-wikieditthird" HorizontalAlign="Center" />
        <Columns>
            
            <asp:TemplateColumn HeaderText="AulaId" Visible="False"> 
                <ItemTemplate>
                    <asp:Label ID="lblAulaId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
                                
            <asp:TemplateColumn HeaderText="Aula">
                <ItemTemplate>
                    <asp:Label ID="lblAula" runat="server"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Data" >
                <ItemTemplate>
                    <asp:Label ID="lblData" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToShortDateString()%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
                     
            <asp:TemplateColumn HeaderText="Hora">
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
                    <asp:TextBox ID="txtDescricao" runat="server" CssClass="ms-toolbar" Height="38px"
                        Width="400px" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "DescricaoAtividade") %>'></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn HeaderText="Atividade">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlAtividade" runat="server" CssClass="ms-toolbar" >
                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn>
            
            <asp:ButtonColumn CommandName="Select" Text="Recurso" ></asp:ButtonColumn>
                       
            <asp:TemplateColumn HeaderText="Recursos Selecionados" >
                <ItemTemplate>
                    <asp:Label ID="lblRecursosSelecionados" runat="server"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:ButtonColumn CommandName="Salvar" Text="Salvar"></asp:ButtonColumn>
            
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
            <table style="width: 67px">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSalvarTudo2" runat="server" CssClass="ms-toolbar" Text="Salvar Todos"  OnClick="btnSalvarTudo_Click"/>
                        </td>
                </tr>
            </table>
            </div>
            <br />
            <ajaxToolkit:ModalPopupExtender ID="programmaticModalPopup" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="True" PopupControlID="programmaticPopup" PopupDragHandleControlID="programmaticPopupDragHandle"
                TargetControlID="dgAulas">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="hiddenTargetControlForModalPopup" runat="server"/><br />
            <asp:Panel ID="programmaticPopup" runat="server" CssClass="modalPopup" Style="padding-right: 10px;
                display: none; padding-left: 10px; padding-bottom: 10px; width: 350px; padding-top: 10px">
                <asp:Panel ID="programmaticPopupDragHandle" runat="Server" Style="border-right: gray 1px solid;
                    border-top: gray 1px solid; border-left: gray 1px solid; cursor: move; color: black;
                    border-bottom: gray 1px solid; background-color: #dddddd; text-align: center"
                    Width="338px">
                    ModalPopup shown and hidden in code
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        &nbsp;<br />
                        <asp:DropDownList ID="ddlPrioridadeRequisicao" runat="server" AutoPostBack="True"
                            CssClass="ms-toolbar" OnSelectedIndexChanged="ddlRequisicoes_SelectedIndexChanged"
                            Width="200px">
                            <asp:ListItem Selected="True" Text="1&#170; Op&#231;&#227;o" Value="1"></asp:ListItem>
                            <asp:ListItem Value="2">2&#170; Op&#231;&#227;o</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp;&nbsp;
                        <asp:Button ID="btnNovaOpcao" runat="server" CssClass="ms-toolbar" OnClick="btnNovaOpcao_Click"
                            Text="Nova opção" /><br />
                        <table style="width: 140px">
                            <tr>
                                <td class="ms-toolbar" style="width: 60px; height: 62px">
                                    Categoria:</td>
                                <td style="width: 251px; height: 62px">
                                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlCategoriaRecurso" runat="server" CssClass="ms-toolbar" OnSelectedIndexChanged="ddlCategoriaRecurso_SelectedIndexChanged"
                                        Width="177px">
                                    </asp:DropDownList>&nbsp; &nbsp;&nbsp;
                                </td>
                                <td style="width: 77px; height: 62px">
                                    <asp:Button ID="btnAdicionar" runat="server" CssClass="ms-toolbar" OnClick="btnAdicionar_Click"
                                        Text="Adicionar" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ListBox ID="lbCategoriasSelecionadas" runat="server" CssClass="ms-toolbar" Width="248px">
                                    </asp:ListBox></td>
                                <td style="width: 77px">
                                    <asp:Button ID="btnRemover" runat="server" CssClass="ms-toolbar" OnClick="btnRemover_Click"
                                        Text="Remover" /></td>
                            </tr>
                        </table>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="ms-toolbar" OnClick="btnCancelar_Click"
                            Text="Cancelar" />
                        <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" OnClick="btnConfirmar_Click"
                            Text="Confirmar" />
                        <br />
                        <asp:Label ID="lblStatus" runat="server" CssClass="ms-toolbar"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dgAulas" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <uc1:Aguarde ID="Aguarde1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
  
</asp:Content>
