<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPayment.aspx.cs" Inherits="SADADPayment.TestPayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtRefNumber" runat="server" Text="SUBS 100201150600669" Width="175px"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtDescription" runat="server" Text="SPTN 1529858520" Width="175px"></asp:TextBox>
        </div>
    <div>
    <asp:Button ID="btnTest" runat="server" Text="Test" OnClick="btnTest_Click" />
    </div>
    </form>
</body>
</html>
