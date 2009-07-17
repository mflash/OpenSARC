<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SemMaster.aspx.cs" Inherits="Teste_SemMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Src="../Default/SelecionaCalendario.ascx" TagName="SelecionaCalendario" TagPrefix="uc3" %>
<%@ Register Src="../Default/MenuProfessor.ascx" TagName="MenuProfessor" TagPrefix="uc2" %>
<%@ Register Src="../Default/MenuAdmin.ascx" TagName="MenuAdmin" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sistema de Alocação de Recursos Computacionais - FACIN</title>
    <style type="text/css">
        @import url("~/CORE.CSS")
    </style>
    <link href="../CORE.CSS" rel="stylesheet" type="text/css" />
</head>

<body>

    <script language="javascript" type="text/javascript">
function confirm_delete()
{
  if (confirm("Confirma a exclusão?")==true)
    return true;
  else
    return false;
}
    </script>

    <form id="Form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="ms-main" style="height: 100%"
            width="100%">
            <tr style="height: 100%">
                <td style="height: 100%">
                    <table cellpadding="0" cellspacing="0" style="height: 100%" width="100%">
                        <tr>
                            <td class="ms-titleareaframe" colspan="2" style="height: 20px">
                                <img alt="" src="../_layouts/images/facinTopo.gif" style="left: 39px; position: absolute;
                                    top: 7px" />
                            </td>
                            <td id="onetidPageTitleAreaFrame" class="ms-pagetitleareaframe" nowrap="nowrap" style="width: 669px;
                                height: 20px" valign="top">
                                <table id="onetidPageTitleAreaTable" border="0" cellpadding="0" cellspacing="0" style="height: 12px"
                                    width="100%">
                                    <tr>
                                        <td class="ms-titlearea" style="height: 21px; width: 669px;" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="onetidPageTitle" class="ms-pagetitle" style="height: 100%; width: 669px;" valign="top">
                                            <h2 class="ms-pagetitle">
                                                Sistema de Alocação de Recursos Computacionais</h2>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="ms-titlearearight" style="width: 24px; height: 20px">
                                <div class="ms-titleareaframe" style="height: 100%">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td id="LeftNavigationAreaCell" class="ms-leftareacell" style="width: 118px; height: 657px"
                                valign="top">
                                <table cellpadding="0" cellspacing="0" class="ms-nav" style="height: 100%" width="100%">
                                    <tr>
                                        <td style="width: 132px">
                                            <table border="0" cellpadding="0" cellspacing="0" class="ms-navframe" style="height: 100%">
                                                <tr valign="top">
                                                    <td style="width: 4px; height: 86px">
                                                    </td>
                                                    <td style="width: 90%; height: 86px" valign="top">
                                                        &nbsp;<br />
                                                        <br />
                                                        <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" valign="bottom">
                                                        <div style="border-right: #a8cefe 3px solid; border-top: #a8cefe 3px solid; border-left: #a8cefe 3px solid;
                                                            border-bottom: #a8cefe 3px solid; background-color: white">
                                                            <br />
                                                            <img alt="" height="80" src="../_layouts/images/facinTopo.gif" width="70" /><br />
                                                            <br />
                                                            &nbsp;<img alt="Centro de Inovação" height="54" src="../_layouts/images/logoci_135x58.gif"
                                                                width="135" /><br />
                                                        </div>
                                                        <img alt="" height="1" src="/_layouts/images/blank.gif" width="138" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 12px; height: 657px;" class="ms-pagemargin">
                                <div class="ms-pagemargin">
                                    <img alt="" height="1" src="/_layouts/images/blank.gif" width="10" /></div>
                            </td>
                            <td class="ms-bodyareacell" valign="top" style="height: 657px">
                                <table id="MSO_ContentTable" border="0" cellpadding="0" cellspacing="0" class="ms-propertysheet"
                                    style="height: 100%" width="100%">
                                    <tr>
                                        <td class="ms-bodyareaframe" style="width: 100%; height: 100%" valign="top">
                                            <div style="text-align: justify">
                                                <br />
                                                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                                                <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
                                                </ajaxToolkit:ToolkitScriptManager>
                                                <ajaxToolkit:ModalPopupExtender ID="programmaticModalPopup" runat="server" BackgroundCssClass="modalBackground"
                                                    DropShadow="True" PopupControlID="programmaticPopup" PopupDragHandleControlID="programmaticPopupDragHandle"
                                                    TargetControlID="hiddenTargetControlForModalPopup" X="500" Y="500">
                                                </ajaxToolkit:ModalPopupExtender>
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="ms-rightareacell" style="width: 24px; height: 657px;">
                                <div class="ms-pagemargin">
                                    <img alt="" height="1" src="/_layouts/images/blank.gif" width="10" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-pagebottommarginleft" style="width: 118px; height: 11px">
                                <img alt="" height="10" src="/_layouts/images/blank.gif" width="1" /></td>
                            <td class="ms-pagebottommargin" style="width: 12px; height: 11px">
                                <img alt="" height="10" src="/_layouts/images/blank.gif" width="1" /></td>
                            <td class="ms-bodyareapagemargin" style="width: 669px; height: 11px">
                                <img alt="" height="10" src="/_layouts/images/blank.gif" width="1" /></td>
                            <td class="ms-pagebottommarginright" style="width: 24px; height: 11px">
                                <img alt="" height="10" src="/_layouts/images/blank.gif" width="1" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>

</html>
