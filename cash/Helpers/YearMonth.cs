using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Cash
{
    public class YearMonth : IEquatable<YearMonth>, IComparable<YearMonth>
    {
        public static bool IgnoreFirstMonthOfYear = true;

        public YearMonth()
        {

        }
        public YearMonth(int year, int month)
        {
            Year = year;
            Month = month;
        }
        public int Year { get; set; }
        public int Month { get; set; }

        public static YearMonth GetNext(YearMonth yearMonth, int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            return GetNext(yearMonth.Year, yearMonth.Month, yearToJumpQty, monthToJumpQty, applyIgnoreFirstMonthOfYear);
        }
        public static YearMonth GetPrevious(YearMonth yearMonth, int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            return GetPrevious(yearMonth.Year, yearMonth.Month, yearToJumpQty, monthToJumpQty, applyIgnoreFirstMonthOfYear);
        }
        public static YearMonth GetNext(int year, int month, int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            for (int i = 0; i < yearToJumpQty; i++)
            {
                year++;
            }
            for (int i = 0; i < monthToJumpQty; i++)
            {
                if (month == 12)
                {
                    year++;
                    month = (applyIgnoreFirstMonthOfYear && IgnoreFirstMonthOfYear) ? 2 : 1;
                }
                else
                {
                    month++;
                }
            }

            return new YearMonth(year, month);
        }
        public static YearMonth GetPrevious(int year, int month, int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            for (int i = 0; i < yearToJumpQty; i++)
            {
                year--;
            }
            for (int i = 0; i < monthToJumpQty; i++)
            {
                if (month == ((applyIgnoreFirstMonthOfYear && IgnoreFirstMonthOfYear) ? 2 : 1))
                {
                    year--;
                    month = 12;
                }
                else
                {
                    month--;
                }
            }
            return new YearMonth(year, month);
        }
        public static YearMonth Current
        {
            get
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                return new YearMonth(persianCalendar.GetYear(DateTime.Now), persianCalendar.GetMonth(DateTime.Now));
            }
        }


        public YearMonth Next
        {
            get
            {
                return GetNext(this);
            }
        }
        public YearMonth Previous
        {
            get
            {
                return GetPrevious(this);
            }
        }

        public YearMonth JumpNext(int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            return GetNext(Year, Month, yearToJumpQty, monthToJumpQty, applyIgnoreFirstMonthOfYear);
        }
        public YearMonth JumpPrevious(int yearToJumpQty = 0, int monthToJumpQty = 1, bool applyIgnoreFirstMonthOfYear = false)
        {
            return GetPrevious(Year, Month, yearToJumpQty, monthToJumpQty, applyIgnoreFirstMonthOfYear);
        }

        public int CompareTo(YearMonth other)
        {
            if (other == null)
                return 1;
            if (this.Year == other.Year)
            {
                if (this.Month == other.Month)
                    return 0;
                else if (this.Month < other.Month)
                    return -1;
                else
                    return 1;
            }
            else if (this.Year < other.Year)
                return -1;
            else
                return 1;
        }

        public bool Equals(YearMonth other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return (this.Year == other.Year) && (this.Month == other.Month);
        }

        public static bool operator ==(YearMonth left, YearMonth right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                if (Object.ReferenceEquals(right, null))
                {
                    return true;
                }

                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(YearMonth left, YearMonth right)
        {
            return !(left == right);
        }

        public static bool operator <(YearMonth left, YearMonth right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(YearMonth left, YearMonth right)
        {
            return left.CompareTo(right) <= 0;
        }


        public static bool operator >(YearMonth left, YearMonth right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(YearMonth left, YearMonth right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}