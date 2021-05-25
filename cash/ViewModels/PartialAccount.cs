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
        public YearMonth LastPersistedFundYearMonth
        {
            get
            {
                var lastFund = this.Funds.OrderByDescending(f => f.Year).ThenByDescending(f => f.Month).FirstOrDefault();
                return lastFund?.YearMonth;
            }
        }
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
                return Loans.OrderByDescending(l=>l.Year).ThenBy(l=>l.Month).FirstOrDefault(l => l.IsActive && l.YearMonth <= YearMonth.Current );
            }
        }
    }
}