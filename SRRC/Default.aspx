<%-- $Id$ --%>

<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="BusinessData.Entities" %>
<%-- Add content controls here --%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SRRC - Web</title>
    <style type="text/css">
        body { margin:0; }
        #all {
            width: 1020px;
            height: 740px;
            margin-left: auto;
            margin-right: auto;
            border: 0px;
            background-color: beige;
        }
        #canvas {
            width: 1020px;
            height: 634px;
            position: absolute;
            top: 84px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 0px;
            margin-bottom:0px;
            border: 0px;
            background-color: brown;
        }

        .biglabel {
            font-size: 26px;
            font-family: Arial, Helvetica, sans-serif;
            margin: 2px;
        }

        input[type="text"] {
            font-size: 30px;
            font-family: Arial, Helvetica, sans-serif;
            text-transform:uppercase;
        }

        smallfont {
            font-size: 13px;
            font-family: sans-serif;
        }

        largefont {
            font-size: 23px;
            font-weight: bold;
            font-family: sans-serif;
        }
        </style>
 </head>
<body>
<script  type="text/javascript" language="javascript">
function confirm_delete()
{
  if (confirm("Confirma a exclusão?")==true)
    return true;
  else
    return false;
}

function click(objname) {
    alert(objname);
    obj = document.getElementById("txtMatricula");
    obj.value = "";
}

    function keyup(objname) {
        obj = document.getElementById(objname);
//        alert(objname.value.length);
        console.log(objname.value.length);
        if (objname.value.length == 10) {
            document.getElementById('txtRecurso').focus();
            return true;
        }
        return false;
    }

    function keyUpRecurso(objname) {
        obj = document.getElementById(objname);
        if (objname.value.length == 4) {
            document.getElementById('txtMatricula').focus();
            return true;
        }
        return false;
    }

    window.onload =
        function checkUserEntered() {
            if (document.getElementById("txtMatricula").value.length > 0)
                document.getElementById("txtRecurso").focus();
            else
                document.getElementById("txtMatricula").focus();
        }
</script>
<form id="Form1" runat="server">
    <div id="all" runat="server">
        <asp:Label CssClass="biglabel" ID="lblMatr" runat="server">Matrícula:</asp:Label>
        <asp:TextBox ID="txtMatricula" runat="server" MaxLength="10" Width="650px" AutoPostBack="true" OnTextChanged="txtMatricula_TextChanged" onkeyup="keyup(this)" onclick="document.getElementById(this.id).value=''; document.getElementById('txtRecurso').value='';" />
        <asp:Label CssClass="biglabel" ID="lblRecurso" runat="server">Recurso:</asp:Label>
        <asp:TextBox ID="txtRecurso" runat="server" MaxLength="6" Width="100px" autopostback="true" OnTextChanged="txtRecurso_TextChanged" onkeyup="keyUpRecurso(this)" onclick="document.getElementById(this.id).value=''"/>
        <asp:Label CssClass="biglabel" ID="lblStatus" runat="server" Width="1010px" style="margin-top: 10px"></asp:Label>
    <div id="content" runat="server">
        <div id="canvas" runat="server">

        </div>
    </div>
    </div>
</form>

</body>
</html>