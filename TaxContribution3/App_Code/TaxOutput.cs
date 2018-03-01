using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TaxOutput
/// </summary>
public class TaxOutput
{
    public double PersonalAllowance { get; set; }
    public double TaxBasicRate { get; set; }
    public double TaxHigherRate { get; set; }
    public double TaxAdditionalRate { get; set; }
    public double NationalInsurance { get; set; }
    public double TotalDeductions { get; set; }
    public double NetSalary { get; set; }


    public TaxOutput()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}