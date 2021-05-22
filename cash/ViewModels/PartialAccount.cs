using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cash.Models
{
    public partial class Account
    {
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "ارزش حساب")]
        public decimal Value
        {
            get
            {
                var currentYm = YearMonth.Current;
                return !Funds.Any() ? this.BaseLineValue : this.BaseLineValue + this.Funds.Where(f => f.Year < currentYm.Year || f.Year == currentYm.Year && f.Month <= currentYm.Month).Select(f => f.Amount).DefaultIfEmpty(0).Sum();
            }
        }
        public Loan CurrentLoan
        {
            get
            {
                return Loans.FirstOrDefault(l => l.YearMonth <= YearMonth.Current && l.EndYearMonth >= YearMonth.Current && l.IsActive);
            }
        }
    }
}