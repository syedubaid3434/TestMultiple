<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPayment.aspx.cs" Inherits="SADADPayment.TestPayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div>
            <asp:Label ID="lblRefNumber" runat="server" Text="Enter Reference No." style="width:100px;" />
            <asp:TextBox ID="txtRefNumber" runat="server" Text="SUBS 100201150600669" Width="175px" ToolTip="Reference Number"></asp:TextBox>
        </div>
        <div>
             <asp:Label ID="lblDescription" runat="server" Text="Enter Description" style="width:100px;" />
            <asp:TextBox ID="txtDescription" runat="server" Text="SPTN 1529858520" Width="175px" ToolTip="Description"></asp:TextBox>
        </div>
         <div>
             <asp:Label ID="lblAmount" runat="server" Text="Enter Amount" style="width:100px;" />
            <asp:TextBox ID="txtAmount" runat="server" Text="1200,00" Width="175px" ToolTip="Amount"></asp:TextBox>
        </div>
    <div>
    <asp:Button ID="btnTest" runat="server" Text="Test" OnClick="btnTest_Click" />
    </div>
    </form>
</body>
</html>
