<%@ Page Language="C#" MasterPageFile="~/Master/Master.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Teste_Default2" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" Runat="Server">
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />&nbsp;
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <ajaxtoolkit:modalpopupextender id="programmaticModalPopup"
     runat="server" 
     backgroundcssclass="modalBackground"
     dropshadow="True" 
     popupcontrolid="programmaticPopup" 
     popupdraghandlecontrolid="programmaticPopupDragHandle"
     Targetcontrolid="hiddenTargetControlForModalPopup"
     X="500"
     y="500"
     >
        </ajaxtoolkit:modalpopupextender>
    <br />
    <asp:Button ID="hiddenTargetControlForModalPopup" runat="server" Style="display: none" /><br />
    &nbsp;<asp:Panel ID="programmaticPopup" runat="server" CssClass="modalPopup" Style="padding-right: 10px;
        display: none; padding-left: 10px; padding-bottom: 10px; width: 350px; padding-top: 10px">
        <asp:Panel ID="programmaticPopupDragHandle" runat="Server" Style="border-right: gray 1px solid;
            border-top: gray 1px solid; border-left: gray 1px solid; cursor: move; color: black;
            border-bottom: gray 1px solid; background-color: #dddddd; text-align: center"
            Width="338px">
            ModalPopup shown and hidden in code
        </asp:Panel>
        &nbsp;
                <asp:Label ID="lblStatus" runat="server" CssClass="ms-toolbar"></asp:Label>
                <asp:Button ID="btnConfirmar" runat="server" CssClass="ms-toolbar" OnClick="btnConfirmar_Click"
                    Text="Confirmar" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="ms-toolbar" OnClick="btnCancelar_Click"
                    Text="Cancelar" />
    </asp:Panel>
    B
    &nbsp;
</asp:Content>

