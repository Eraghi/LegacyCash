using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cash.Models
{
    public partial class Fund
    {
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
}