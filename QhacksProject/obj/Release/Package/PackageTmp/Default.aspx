<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QhacksProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mainInf">
        <div class="row">
            <img src="images/logo.png" alt="logo" height="300" width="450" class="logoPos">

            <h1 class="h1Lato">Is this good?</h1>
            <p class="lead" style="font-size-adjust: 0.67; margin-top: 25px;">Let's find out...</p>
        </div>
        <br />
        <div class="row inOutRow">
            <div class="col-md-5" style="padding-left: 0px;">

                <h2></h2>

                <asp:UpdatePanel runat="server" ID="updatPanel">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="goButton" />
                    </Triggers>
                    <ContentTemplate>
                        <%--                        <label runat="server" class="h1Lato" style="color: white;" type="text" id="questionDisp"></label>--%>
                        <asp:Label runat="server" CssClass="h1Lato aspTextBox" ID="questionLabel"></asp:Label>
                        <%--                        <input runat="server" class="h1Lato" style="color: white;" type="text" name="usrInput" id="usrInputField" placeholder="Input">--%>
                        <asp:TextBox CssClass="h1Lato" ID="usrInputField" runat="server"></asp:TextBox>
                        <asp:Button runat="server" ID="goButton" OnClick="goButton_Click" Text="Go!"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-7">
                <h2 class="h1Lato" style="margin-top: 0px;">output </h2>
            </div>

        </div>
    </div>

    <script>
        jQuery(document).ready(function () {
            jQuery("#goButton").hide();
        });

        jQuery("#usrInputField").change(function () {
            if (this.value.replace(/\s/g, "") === "") {
                jQuery("#goButton").hide();
            } else {
                jQuery("#goButton").show();
            }
        });

        jQuery("#goButton").click(function () {
            jQuery("#usrInputField").val("");
            jQuery(this).hide();
        });


    </script>

</asp:Content>
