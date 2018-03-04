using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class _Default : Page
{
    //2017-2018 tax thresholds and rates - all constants as they must not 
    //be changed in the code
    const double personalAllowanceLimit = 11509;
    const double basicRate = 0.2;
    const double higherRateThreshold = 45000;
    const double higherRate = 0.4;
    const double additionalRateThreshold = 150000;
    const double additionalRate = 0.45;
    const double personalAllowanceAdjustmentThreshold = 100000;
    const double nationalInsuranceThreshold = 8164;
    const double nationalInsuranceRate = 0.12;
    const double nationalInsuranceUpperEarningsLimit = 45000;
    const double nationalInsuranceUpperEarningsRate = 0.02;
    const double basicRateMaximumChildcareVouchers = 243;
    const double higherRateMaximumChildcareVouchers = 127;
    const double additionalRateMaximumChildcareVouchers = 110;
    const double studentLoanPlan1Threshold = 17775;
    const double studentLoanPlan2Threshold = 21000;
    const double studentLoanRepaymentRate = 0.09;

    //government spending data - also constants
    const double pensionsSpending = 0.26;
    const double healthSpending = 0.23;
    const double educationSpending = 0.07;
    const double defenceSpending = 0.07;
    const double welfareSpending = 0.09;
    const double protectionSpending = 0.03;
    const double transportSpending = 0.03;
    const double generalSpending = 0.02;
    const double otherSpending = 0.12;
    const double interestSpending = 0.08;

    //variables that are needed by more than one event handler
    static double childcareVoucherAmount = 0;
    static double pensionAnnualAmount = 0;
    static double grossSalaryInput = 0;
    static double pensionPercentageAmount = 0;
    static double pensionMonthlyAmount = 0;
    static double studentLoanAnnualDeduction = 0;
    static bool validInput = true;

    protected void Page_Load(object sender, EventArgs e)
    {

    }





    protected void btnCalculateTax_Click(object sender, EventArgs e)
    {
        //TaxOutput taxOutput = new TaxOutput();
        validInput = Double.TryParse(tbGrossSalary.Text, out grossSalaryInput);

        if (validInput)
        {
            TaxOutput taxOutput = CalculateTax();
            //output displayed with currency symbols and thousands separator
            lblChildcareVoucherAmount.Text = childcareVoucherAmount.ToString("C");
            lblStudentLoanAmount.Text = studentLoanAnnualDeduction.ToString("C");
            lblpensionAnnualAmount.Text = pensionAnnualAmount.ToString("C");
            lblPersonalAllowance.Text = taxOutput.PersonalAllowance.ToString("C");
            lblBasicRateTax.Text = taxOutput.TaxBasicRate.ToString("C");
            lblHigherRateTax.Text = taxOutput.TaxHigherRate.ToString("C");
            lblAdditionalRateTax.Text = taxOutput.TaxAdditionalRate.ToString("C");
            lblNationalInsurancePaid.Text = taxOutput.NationalInsurance.ToString("C");
            lblTotalDeductions.Text = taxOutput.TotalDeductions.ToString("C");
            lblNetSalary.Text = taxOutput.NetSalary.ToString("C");

            //if user says they are paying more pension than they get in salary
            if ((taxOutput.NetSalary == 0) && (pensionAnnualAmount > 0))
            {
                lblPensionError.Text = "Aren't your pension contributions a bit too high?";
            }
            double totalTaxAndNiPaid = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate
                                + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance;

            taxOutput.PensionsContribution = totalTaxAndNiPaid * pensionsSpending;
            taxOutput.HealthContribution = totalTaxAndNiPaid * healthSpending;
            taxOutput.EducationContribution = totalTaxAndNiPaid * educationSpending;
            taxOutput.DefenceContribution = totalTaxAndNiPaid * defenceSpending;
            taxOutput.WelfareContribution = totalTaxAndNiPaid * welfareSpending;
            taxOutput.ProtectionContribution = totalTaxAndNiPaid * protectionSpending;
            taxOutput.TransportContribution = totalTaxAndNiPaid * transportSpending;
            taxOutput.GeneralContribution = totalTaxAndNiPaid * generalSpending;
            taxOutput.OtherContribution = totalTaxAndNiPaid * otherSpending;
            taxOutput.InterestContribution = totalTaxAndNiPaid * interestSpending;

            //DrawCharts(taxOutput);
            //DrawTaxGraph(taxOutput);
            //DrawContributionsGraph(taxOutput);
            DrawBothCharts(taxOutput);
        }
}

protected void tbChildcareVouchers_TextChanged(object sender, EventArgs e)
    {
        childcareVoucherAmount = 0;

        validInput = Double.TryParse(tbGrossSalary.Text, out grossSalaryInput);
        validInput = Double.TryParse(tbChildcareVouchers.Text, out childcareVoucherAmount);
        if (!validInput || childcareVoucherAmount < 0)
        {
            lblCcvError.Text = "Please enter a zero or a positive number for childcare voucher amount";
            
            childcareVoucherAmount = 0;
            validInput = true;
        }
        else
        {
            lblCcvError.Text = "";
        }
        //ensure childcare voucher amount doesn't exceed maximum allowed
        if ((childcareVoucherAmount > basicRateMaximumChildcareVouchers) && (grossSalaryInput <= higherRateThreshold))
        {
            childcareVoucherAmount = basicRateMaximumChildcareVouchers;
            lblCcvError.Text = "Your childcare voucher has been set to the maximum permitted.";
        }
        if ((childcareVoucherAmount > higherRateMaximumChildcareVouchers) && (grossSalaryInput > higherRateThreshold) && (grossSalaryInput <= additionalRateThreshold))
        {
            childcareVoucherAmount = higherRateMaximumChildcareVouchers;
            lblCcvError.Text = "Your childcare voucher has been set to the maximum permitted.";
        }
        if ((childcareVoucherAmount > additionalRateMaximumChildcareVouchers) && (grossSalaryInput > additionalRateThreshold))
        {
            childcareVoucherAmount = additionalRateMaximumChildcareVouchers;
            lblCcvError.Text = "Your childcare voucher has been set to the maximum permitted.";
        }

        childcareVoucherAmount *= 12;
    }

    protected void tbPension_TextChanged(object sender, EventArgs e)
    {
        pensionAnnualAmount = 0;
        //if a percentage has been entered for pension contribution
        if ((tbPension.Text).Contains("%"))
        {
            string[] pensionPercentage = (tbPension.Text).Split('%');

            validInput = Double.TryParse(pensionPercentage[0], out pensionPercentageAmount);
            if (!validInput || (pensionPercentageAmount <= 0) || (pensionPercentageAmount >= 100))
            {
                //lblPensionError.Text = "Please enter a positive amount or percentage (0-100) for pension contribution";
                
                pensionPercentageAmount = 0;
                pensionAnnualAmount = 0;
                validInput = true;
            }
            else
            {
                lblPensionError.Text = "";
                pensionAnnualAmount = grossSalaryInput * (pensionPercentageAmount / 100);
            }
        }
        else //if an amount has been entered for pension contribution
        {
            validInput = Double.TryParse(tbPension.Text, out pensionMonthlyAmount);
            if (!validInput || pensionMonthlyAmount < 0)
            {
                lblPensionError.Text = "Please enter zero or a positive amount or percentage for pension contribution";
                
                pensionMonthlyAmount = 0;
                pensionAnnualAmount = 0;
                validInput = true;
            }
            else
            {
                lblPensionError.Text = "";
                pensionAnnualAmount = pensionMonthlyAmount * 12;
            }
        }
    }

    protected void ddlStudentLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
        studentLoanAnnualDeduction = 0;

        if (ddlStudentLoan.SelectedValue == "Plan1" && grossSalaryInput >= studentLoanPlan1Threshold)
        {
            studentLoanAnnualDeduction = (grossSalaryInput - studentLoanPlan1Threshold) * studentLoanRepaymentRate;
        }
        else if (ddlStudentLoan.SelectedValue == "Plan2" && grossSalaryInput >= studentLoanPlan2Threshold)
        {
            studentLoanAnnualDeduction = (grossSalaryInput - studentLoanPlan2Threshold) * studentLoanRepaymentRate;
        }
        else
        {
            studentLoanAnnualDeduction = 0;
        }
    }

    //static methods for calculations--------------------------------------------------------------------
    static TaxOutput CalculateTax()
    {
        TaxOutput taxOutput = new TaxOutput();

        double grossSalaryForCalculation = 0;
        //deduct childcare vouchers before starting tax calculation
        grossSalaryForCalculation = grossSalaryInput - childcareVoucherAmount;

        taxOutput.PersonalAllowance = CalculatePersonalAllowance(grossSalaryForCalculation);
        taxOutput.TaxBasicRate = CalculateBasicRateTax(grossSalaryForCalculation);
        taxOutput.TaxHigherRate = CalculateHigherRateTax(grossSalaryForCalculation, taxOutput.PersonalAllowance);
        taxOutput.TaxAdditionalRate = CalculateAdditionalRateTax(grossSalaryForCalculation);
        taxOutput.NationalInsurance = CalculateNationalInsurance(grossSalaryForCalculation);
        taxOutput.TotalDeductions = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance + childcareVoucherAmount + pensionAnnualAmount + studentLoanAnnualDeduction;
        taxOutput.NetSalary = grossSalaryInput - taxOutput.TotalDeductions;
        taxOutput.ChildcareVoucherAmount = childcareVoucherAmount;
        taxOutput.PensionAnnualAmount = pensionAnnualAmount;
        taxOutput.GrossSalaryInput = grossSalaryInput;
        taxOutput.StudentLoanAnnualDeduction = studentLoanAnnualDeduction;

        //add contributions

        //if deductions exceed income
        if (taxOutput.NetSalary < 0)
        {
            taxOutput.NetSalary = 0;
            taxOutput.TotalDeductions = grossSalaryInput;
            pensionAnnualAmount = grossSalaryInput - taxOutput.TaxBasicRate - taxOutput.TaxHigherRate - taxOutput.TaxAdditionalRate - taxOutput.NationalInsurance - childcareVoucherAmount - studentLoanAnnualDeduction;
        }
        return taxOutput;
    }

    static double CalculatePersonalAllowance(double grossSalary)
    {
        double personalAllowance = 0;

        //adjust personal allowance downwards for highest earners
        if (grossSalaryInput > personalAllowanceAdjustmentThreshold)
        {
            personalAllowance = personalAllowanceLimit - ((grossSalaryInput - personalAllowanceAdjustmentThreshold) / 2);
            if (personalAllowance < 0)
            {
                personalAllowance = 0;
            }
        }
        else  //don't change personal allowance
        {
            personalAllowance = personalAllowanceLimit;
        }

        return personalAllowance;
    }

    static double CalculateBasicRateTax(double grossSalary)
    {
        double taxBasicRate = 0;
        //if the maximum basic rate tax is paid ...
        if (grossSalary > higherRateThreshold)
        {
            taxBasicRate = (higherRateThreshold - personalAllowanceLimit) * basicRate;
        }
        //if less than the maximum basic rate tax is paid AND the user earns
        //more than the personal allowance
        else if (grossSalary > personalAllowanceLimit)
        {
            taxBasicRate = (grossSalary - personalAllowanceLimit) * basicRate;
        }

        return taxBasicRate;
    }

    static double CalculateHigherRateTax(double grossSalary, double personalAllowance)
    {
        double taxHigherRate = 0;
        //higher rate tax may involve alterations to personal allowance 
        double personalAllowanceDeduction = personalAllowanceLimit - personalAllowance;

        if (grossSalary > higherRateThreshold)
        {
            if (grossSalary > additionalRateThreshold)
            {
                taxHigherRate = (additionalRateThreshold - higherRateThreshold + personalAllowanceDeduction) * higherRate;
            }
            else
            {
                taxHigherRate = (grossSalary - higherRateThreshold + personalAllowanceDeduction) * higherRate;
            }

        }

        return taxHigherRate;
    }

    static double CalculateAdditionalRateTax(double grossSalary)
    {
        double taxAdditionalRate = 0;

        if (grossSalary > additionalRateThreshold)
        {
            taxAdditionalRate = (grossSalary - additionalRateThreshold) * additionalRate;
        }

        return taxAdditionalRate;
    }

    static double CalculateNationalInsurance(double grossSalary)
    {
        double nationalInsurancePaid = 0;

        if (grossSalary > nationalInsuranceThreshold)
        {
            if (grossSalary > nationalInsuranceUpperEarningsLimit)
            {
                nationalInsurancePaid = (nationalInsuranceUpperEarningsLimit - nationalInsuranceThreshold) * 0.12
                    + (grossSalary - nationalInsuranceUpperEarningsLimit) * 0.02;
            }
            else
            {
                nationalInsurancePaid = (grossSalary - nationalInsuranceThreshold) * 0.12;
            }
        }

        return nationalInsurancePaid;
    }

    void DrawCharts(TaxOutput taxOutput)
    {
        hldataSource.Visible = true;

        double totalTaxAndNiPaid = taxOutput.TaxBasicRate + taxOutput.TaxHigherRate
                                 + taxOutput.TaxAdditionalRate + taxOutput.NationalInsurance;

        double pensionsContribution = totalTaxAndNiPaid * pensionsSpending;
        double healthContribution = totalTaxAndNiPaid * healthSpending;
        double educationContribution = totalTaxAndNiPaid * educationSpending;
        double defenceContribution = totalTaxAndNiPaid * defenceSpending;
        double welfareContribution = totalTaxAndNiPaid * welfareSpending;
        double protectionContribution = totalTaxAndNiPaid * protectionSpending;
        double transportContribution = totalTaxAndNiPaid * transportSpending;
        double generalContribution = totalTaxAndNiPaid * generalSpending;
        double otherContribution = totalTaxAndNiPaid * otherSpending;
        double interestContribution = totalTaxAndNiPaid * interestSpending;

        ClientScript.RegisterStartupScript(GetType(), "draw", "drawContributions('" + taxOutput.PersonalAllowance + "','"
                                                                       + taxOutput.TaxBasicRate + "','"
                                                                       + taxOutput.TaxHigherRate + "','"
                                                                       + taxOutput.TaxAdditionalRate + "','"
                                                                       + taxOutput.NationalInsurance + "','"
                                                                       + taxOutput.NetSalary + "','"
                                                                       + childcareVoucherAmount + "','"
                                                                       + studentLoanAnnualDeduction + "','"
                                                                       + pensionAnnualAmount + "','"
                                                                       + grossSalaryInput + "','"
                                                                       + pensionsContribution + "','"
                                                                       + healthContribution + "','"
                                                                       + educationContribution + "','"
                                                                       + defenceContribution + "','"
                                                                       + welfareContribution + "','"
                                                                       + protectionContribution + "','"
                                                                       + transportContribution + "','"
                                                                       + generalContribution + "','"
                                                                       + otherContribution + "','"
                                                                       + interestContribution + "','"
                                                                       + totalTaxAndNiPaid + "');", true);
    }

    void DrawTaxGraph(TaxOutput taxOutput)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "anything", "drawTax('" + taxOutput.NetSalary + "','" 
                                                                                + taxOutput.TaxBasicRate + "','"
                                                                                + taxOutput.TaxHigherRate + "','"
                                                                                + taxOutput.TaxAdditionalRate + "','"
                                                                                + taxOutput.NationalInsurance + "','"
                                                                                + pensionAnnualAmount + "','"
                                                                                + childcareVoucherAmount + "');", true);
    }

    void DrawContributionsGraph(TaxOutput taxOutput)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "anything", "drawContributions('" + taxOutput.PensionsContribution + "','"
                                                                       + taxOutput.HealthContribution + "','"
                                                                       + taxOutput.EducationContribution + "','"
                                                                       + taxOutput.DefenceContribution + "','"
                                                                       + taxOutput.WelfareContribution + "','"
                                                                       + taxOutput.ProtectionContribution + "','"
                                                                       + taxOutput.TransportContribution + "','"
                                                                       + taxOutput.GeneralContribution + "','"
                                                                       + taxOutput.OtherContribution + "','"
                                                                       + taxOutput.InterestContribution + "');", true);

    }


    void DrawBothCharts(TaxOutput taxOutput)
    {
        hldataSource.Visible = true;

        ClientScript.RegisterStartupScript(this.GetType(), "anything", "draw('" + taxOutput.NetSalary + "','"
                                                                                + taxOutput.TaxBasicRate + "','"
                                                                                + taxOutput.TaxHigherRate + "','"
                                                                                + taxOutput.TaxAdditionalRate + "','"
                                                                                + taxOutput.NationalInsurance + "','"
                                                                                + taxOutput.PensionAnnualAmount + "','"
                                                                                + taxOutput.ChildcareVoucherAmount + "','"
                                                                                + taxOutput.StudentLoanAnnualDeduction + "','"
                                                                                + taxOutput.PensionsContribution + "','"
                                                                                + taxOutput.HealthContribution + "','"
                                                                                + taxOutput.EducationContribution + "','"
                                                                                + taxOutput.DefenceContribution + "','"
                                                                                + taxOutput.WelfareContribution + "','"
                                                                                + taxOutput.ProtectionContribution + "','"
                                                                                + taxOutput.TransportContribution + "','"
                                                                                + taxOutput.GeneralContribution + "','"
                                                                                + taxOutput.OtherContribution + "','"
                                                                                + taxOutput.InterestContribution
                                                                                + "');", true);
    }
}