using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cash.Models
{
    [MetadataType(typeof(MetaLoan))]
    public partial class Loan
    {

        [Display(Name = "تعداد اقساط")]
        public int InstallmentQty { get { return Convert.ToInt32(this.Amount / this.InstallmentAmount); } }
        public YearMonth EndYearMonth { get { return YearMonth.GetNext(this.YearMonth, monthToJumpQty: this.InstallmentQty, applyIgnoreFirstMonthOfYear: true); } }

        public YearMonth LastInstallmentYearMonth
        {
            get
            {
                var monthToJumpQty = ((int)(RemainingAmount / InstallmentAmount));
                return YearMonth.GetNext(YearMonth.Current, monthToJumpQty: monthToJumpQty, applyIgnoreFirstMonthOfYear: true);
            }
        }

        [Display(Name = "تعداد اقساط پرداخت شده")]
        public int PaidInstallmentQty
        {
            get
            {
                return !Installments.Any() ? 0 : Convert.ToInt32(this.Installments.Where(i => i.YearMonth <= YearMonth.Current).Sum(i => i.Amount) / this.InstallmentAmount);
            }
        }
        [Display(Name = "تعداد اقساط باقی مانده")]
        public int RemainingInstallmentQty { get { return this.InstallmentQty - PaidInstallmentQty; } }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "مبلغ پرداخت شده")]
        public decimal PaidAmount
        {
            get
            {
                return !Installments.Any() ? 0 : this.Installments.Where(i => i.YearMonth <= YearMonth.Current).Select(i => i.Amount).DefaultIfEmpty(0).Sum();
            }
        }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "مبلغ باقی مانده")]
        public decimal RemainingAmount { get { return this.Amount - this.PaidAmount; } }
        public YearMonth YearMonth
        {
            get
            {
                return new YearMonth(this.Year, this.Month);
            }
            set
            {
                this.Year = value.Year;
                this.Month = value.Month;
            }
        }

        
    }

    public class MetaLoan
    {
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "مبلغ")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "مبلغ قسط")]
        public decimal InstallmentAmount { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "حساب")]
        public int? AccountID { get; set; }
        [Display(Name = "سال")]
        public int Year { get; set; }
        [Display(Name = "ماه")]
        public int Month { get; set; }
    }
}