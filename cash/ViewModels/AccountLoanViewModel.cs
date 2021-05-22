using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cash.ViewModels
{
    public class AccountLoanViewModel
    {
        public int AccountID { get; set; }
        public int? LoanID { get; set; }
        [Display(Name = "نام حساب")]
        public string Name { get; set; }
        [Display(Name = "ارزش حساب")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Value { get; set; }
        [Display(Name = "ارزش اولیه حساب")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal BaselineValue { get; set; }
        [Display(Name = "مبلغ وام")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? LoanAmount { get; set; }
        [Display(Name = "مبغ پرداخت شده وام")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PaidAmount { get; set; }
        [Display(Name = "مبلغ باقیمانده وام")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? RemainingAmount { get; set; }
        [Display(Name = "تعداد کل اقساط")]
        public int? InstallmentQty { get; set; }
        [Display(Name = "مبغ هر قسط")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? InstallmentAmount { get; set; }
        [Display(Name = "تعداد اقساط پرداخت شده")]
        public int? PaidInstallmentQty { get; set; }
        [Display(Name = "تعداد اقساط باقیمانده")]
        public int? RemainingInstallmentQty { get; set; }
        [Display(Name = "سال وام")]
        public int? LoanYear { get; set; }
        [Display(Name = "ماه وام")]
        public int? LoanMonth { get; set; }
        [Display(Name = "تاریخ دریافت وام")]
        public string LoanTime { get { return $"{this.LoanYear}/{this.LoanMonth}"; } }
        [Display(Name = "تاریخ پایان وام")]
        public string EndTime { get { return $"{EndYearMonth?.Year}/{EndYearMonth?.Month}"; } }
        [Display(Name = "تاریخ آخرین قسط")]
        public string LastInstallmentTime { get { return $"{LastInstallmentYearMonth?.Year}/{LastInstallmentYearMonth?.Month}"; } }
        public YearMonth EndYearMonth { get; set; }
        public YearMonth LastInstallmentYearMonth { get; set; }

    }
}