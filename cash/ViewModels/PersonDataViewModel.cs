using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Cash.ViewModels
{
    public class PersonDataViewModel
    {
        public int PersonID { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "تعداد حساب ها")]
        public int NumberOFAccounts { get; set; }

        [Display(Name = "ارزش حساب ها")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ValueOFAccounts { get; set; }

        [Display(Name = "بدهکاری این ماه")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ThisMonthDebit { get; set; }
        //[Display(Name = "پرداختی این ماه")]
        //[DisplayFormat(DataFormatString = "{0:N0}")]
        //public decimal ThisMonthPay { get; set; }
        [Display(Name = "بدهکاری ماه بعد")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal NextMonthDebit { get; set; }
        [Display(Name = "مبلغ باقیمانده وامها")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LoansRemainingAmount { get; set; }
    }
}