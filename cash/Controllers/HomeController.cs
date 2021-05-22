using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cash.Models;
using Cash.ViewModels;

namespace Cash.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var currentYearMonth = YearMonth.Current;
            var nextYearMonth = YearMonth.Current.Next;
            var db = new CashEntities();
            var peopleData = (from a in db.Accounts
                              where  a.IsActive
                              group a by a.Person into g
                              select g).ToList();
            var peopleDataList = peopleData.Select(g => new PersonDataViewModel
            {
                PersonID = g.Key.ID,
                Name = g.Key.FirstName + " " + g.Key.LastName,
                NumberOFAccounts = g.Count(),
                ValueOFAccounts = g.Sum(a => a.Value),
                ThisMonthDebit = g.Where(gi=>gi.CurrentLoan != null).Sum(a => a.CurrentLoan.Installments.Where(i => i.YearMonth == currentYearMonth).Select(i => i.Amount).DefaultIfEmpty(0).Sum()) + g.Sum(a => a.Funds.Where(f => f.YearMonth == currentYearMonth).Select(f => f.Amount).DefaultIfEmpty(0).Sum()),
                NextMonthDebit = g.Where(gi => gi.CurrentLoan != null).Sum(a => a.CurrentLoan.Installments.Where(i => i.YearMonth == nextYearMonth).Select(i => i.Amount).DefaultIfEmpty(0).Sum()) + g.Sum(a => a.Funds.Where(f => f.YearMonth == nextYearMonth).Select(f => f.Amount).DefaultIfEmpty(0).Sum()),
                LoansRemainingAmount = g.Where(gi => gi.CurrentLoan != null).Select(a => a.CurrentLoan.RemainingAmount).DefaultIfEmpty(0).Sum()
            }).ToList();
            peopleDataList.Add(new PersonDataViewModel()
            {
                PersonID = -1,
                Name = "جمع",
                NumberOFAccounts = peopleDataList.Sum(p=>p.NumberOFAccounts),
                ValueOFAccounts = peopleDataList.Sum(p => p.ValueOFAccounts),
                ThisMonthDebit = peopleDataList.Sum(p => p.ThisMonthDebit),
                NextMonthDebit = peopleDataList.Sum(p => p.NextMonthDebit),
                LoansRemainingAmount = peopleDataList.Sum(p => p.LoansRemainingAmount),
            });
            return View(peopleDataList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Calculate()
        {
            var calculator = new Calculator();
            calculator.Calculate();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCalculation()
        {
            var calculator = new Calculator();
            calculator.DeleteCalculation();
            return RedirectToAction("Index");
        }

        public ActionResult Prepare()
        {
            using (var db = new CashEntities())
            {
                foreach (var account in db.Accounts.Where(a => a.IsActive))
                {
                    YearMonth date = new YearMonth(1398, 4);
                    YearMonth toDate = YearMonth.Current.Previous;

                    while (date <= toDate)
                    {
                        if (account.Funds.All(f => f.YearMonth != date))
                        {
                            var foundAmount = (date.Month == 2) ? Calculator.FUND_AMOUNT * 2 : Calculator.FUND_AMOUNT;
                            if (date.Month != 1)
                            {
                                account.Funds.Add(new Fund
                                {
                                    Amount = foundAmount,
                                    Year = date.Year,
                                    Month = date.Month
                                });
                            }

                        }

                        var nextDate = date.Next;
                        date = nextDate;
                    }
                    var loan = account.Loans.FirstOrDefault(l => l.IsActive);
                    if (loan != null)
                    {
                        date = YearMonth.GetNext(loan.YearMonth);
                        while (date <= toDate)
                        {
                            if (loan.Installments.All(i => i.YearMonth != date))
                            {
                                if (date.Month != 1)
                                {
                                    loan.Installments.Add(new Installment
                                    {
                                        Amount = loan.InstallmentAmount,
                                        Year = date.Year,
                                        Month = date.Month,
                                    });
                                }
                            }
                            var nextDate = YearMonth.GetNext(date.Year, date.Month);
                            date = nextDate;
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}