<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DCAWebPage.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DCA Web</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="DCAWebPage.js"></script>
</head>
<body onload="LoadAirports();">
    <form id="form1" runat="server">
        <div style="margin: 10px">
            <button class="btn btn-primary" id="btnStep" type="button" onclick="Step()">Step</button>
            <div class="row">
                <div id="divAirports" runat="server" class="col-sm-2">
                </div>
                <div id="divAirplanes" runat="server" class="col-sm-10">
                </div>

            </div>
        </div>
    </form>
</body>
</html>
