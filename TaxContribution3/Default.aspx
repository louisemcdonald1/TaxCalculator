<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <%--<script src="Scripts/jquery-1.9.1.min.js"></script>--%>
   <%-- <script src="Scripts/canvasjs.min.js"></script>
    <script src="Scripts/myScript.js"></script>--%>
    <title>Tax Contribution Calculator</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class ="bgImage">
    <div class="container body-content">    
        <div class="headingsBackground box">  
            <h1>Tax and Contribution Calculator - 2017-2018</h1>
            <h3>England, Wales and Northern Ireland</h3>
        </div> 
        <br />  
        <div class="row">
            <div class="col-sm-5 leftSide box">
                <div class="row">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMessage" ValidationGroup="allValidators" />
                    <div class="col-sm-6">
                        <asp:Label ID="Label1" runat="server" Text="Please enter your gross annual salary (£):"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbGrossSalary" CssClass ="inputFields" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Please enter your gross annual salary" ControlToValidate="tbGrossSalary" CssClass="errorMessage" runat="server" ValidationGroup="allValidators" >*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Please enter a number between 1 and 1,000,000 for gross annual salary" ControlToValidate="tbGrossSalary" CssClass="errorMessage" Display="Dynamic" MaximumValue="1000000" MinimumValue="1" Type="Double" ValidationGroup="allValidators">*</asp:RangeValidator>   
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label9" runat="server" Text="If you get childcare vouchers, please enter monthly amount (£):"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbChildcareVouchers" CssClass ="inputFields" runat="server" TextMode="Number" ToolTip="Childcare voucher amount" OnTextChanged="tbChildcareVouchers_TextChanged"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Please enter 0 or a positive number for monthly childcare vouchers" ControlToValidate="tbChildcareVouchers" CssClass="errorMessage" MinimumValue="0" MaximumValue="10000" Text="*" Type="Double" ValidationGroup="allValidators"></asp:RangeValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="lblCcvError" CssClass= "errorMessage" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label10" runat="server" Text="If you make pension contributions, please enter the monthly amount or percentage of salary:"></asp:Label>  
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="tbPension" CssClass ="inputFields" runat="server" ToolTip="Pension contribution amount or percentage" TextMode="SingleLine" OnTextChanged="tbPension_TextChanged"></asp:TextBox>
                        <%--regular expression specifies 1-5 digits at the start of the input and allows a optional % sign--%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter 0 or a positive number or percentage for pension contribution" ControlToValidate="tbPension" CssClass=".errorMessage" ValidationExpression="[0-9][0-9]?[0-9]?[0-9]?[0-9]?%?" ValidationGroup="allValidators" Text="*"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="lblPensionError" CssClass= "errorMessage" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label11" runat="server" Text="If you are repaying a student loan, please select the correct plan:"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlStudentLoan" runat="server" OnSelectedIndexChanged="ddlStudentLoan_SelectedIndexChanged">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem Value="Plan1">Plan 1</asp:ListItem>
                            <asp:ListItem Value="Plan2">Plan 2</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4 col-sm-offset-6">
                        <asp:Button ID="btnCalculateTax" runat="server" Text="Calculate Tax" OnClick="btnCalculateTax_Click" ValidationGroup="allValidators" />
                    </div>
                </div> 
                <hr />
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label2" runat="server" Text="Your personal allowance is:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblPersonalAllowance" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label12" runat="server" Text="Childcare vouchers:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblChildcareVoucherAmount" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label13" runat="server" Text="Your student loan repayment is:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblStudentLoanAmount" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label14" runat="server" Text="Your pension contribution is:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblpensionAnnualAmount" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label3" runat="server" Text="Tax paid at 20%:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblBasicRateTax" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                    </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label4" runat="server" Text="Tax paid at 40%:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblHigherRateTax" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label5" runat="server" Text="Tax paid at 45%:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblAdditionalRateTax" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label6" runat="server" Text="National insurance paid:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblNationalInsurancePaid" runat="server" CssClass="numberColumn" ></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label7" runat="server" Text="Total deductions:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblTotalDeductions" runat="server" CssClass="numberColumn"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="Label8" runat="server" Text="Net salary:" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblNetSalary" runat="server" CssClass="numberColumn" Font-Bold="True"></asp:Label>
                    </div>
                </div>
                <hr />
                   
            </div>
            <div class="col-sm-6 col-sm-offset-1 rightSide box" id="startInvisible">
                <div class="row">
                    <div class="col-sm-12 text-center">
                        <div id="chartContainer1" style="height: 400px; width: 100%;">
                            <div id="chartPlaceholder1"></div>
                            <div id="noDataPlaceholder1" class="h1"></div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-12 text-center">
                        <div id="chartContainer2" style="height: 400px; width: 100%;">
                            <div id="chartPlaceholder2"></div>
                            <div id="noDataPlaceholder2" class="h1"></div>
                        </div>
                        <br />
                        <asp:HyperLink ID="hldataSource" runat="server" NavigateUrl="http://www.ukpublicspending.co.uk/year_budget_2017UKbn_17bc1n#usgs302" Target="_blank" Visible="false">Source of UK public spending data</asp:HyperLink>
                        <%--<a id="dataSource" href="http://www.ukpublicspending.co.uk/year_budget_2017UKbn_17bc1n#usgs302" target ="_blank">Source of UK public spending data</a>--%>
                    </div>
                </div>
            </div>
        </div>   
        <br />
        <br />    
        <footer class="box">
            <p>&copy; <%: DateTime.Now.Year %> - Tax and Contribution Calculator - please note that this is a demonstration website and should not be used as a basis for making financial decisions</p>
        </footer>
    </div>
    </div> <%--bgImage--%>
    </form>
</body>
</html>
<%--<script>
    var event1 = document.getElementById("btnCalculateTax");
    event1.addEventListener('click', displaySpoilers, false);
    function displaySpoilers() {
        document.getElementsByClassName("rightSide").style.visibility = "visible";
        //document.getElementById("spoilers").style.display = "none";
    }
</script>--%>
