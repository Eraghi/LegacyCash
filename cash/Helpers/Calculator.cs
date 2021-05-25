using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using Cash.Models;

namespace Cash
{
    public class Calculator
    {
        public const int FUND_AMOUNT = 100000;
        public const int LOAN_AMOUNT = 15000000;
        public const int INSTALLMENT_AMOUNT = 250000;
        public const int CURRENT_CASH_AMOUNT = 12700000;

        public void DeleteCalculation()
        {
            using (var db = new CashEntities())
            {
                foreach (var account in db.Accounts.Where(a => a.IsActive).Include(a => a.Loans).Include(a => a.Loans.Select(l => l.Installments)).Include(a => a.Funds).ToList())
                {
                    var currentYm = YearMonth.Current;
                    var funds = account.Funds.Where(f => f.Year > currentYm.Year || f.Year == currentYm.Year && f.Month > currentYm.Month).ToList();
                    db.Funds.RemoveRange(funds);
                    var deleteLoanList = new List<Loan>();
                    var deleteInstallmentList = new List<Installment>();
                    foreach (var loan in account.Loans)
                    {
                        if (!loan.IsActive || loan.YearMonth > YearMonth.Current)
                        {
                            deleteLoanList.Add(loan);
                        }
                        else
                        {
                            deleteInstallmentList.AddRange(loan.Installments.Where(i =>
                                i.YearMonth > currentYm));
                        }
                    }
                    db.Installments.RemoveRange(deleteInstallmentList);
                    db.Loans.RemoveRange(deleteLoanList);
                }
                var cash = db.Cashes.First();
                cash.Amount = CURRENT_CASH_AMOUNT;
                db.SaveChanges();
            }
        }

        public void Calculate()
        {
            using (var db = new CashEntities())
            {
                var accounts = db.Accounts.Where(a => a.IsActive).Include(a => a.Loans).Include(a => a.Loans.Select(l => l.Installments)).Include(a => a.Funds).ToList();
                foreach (var account in accounts)
                {
                    CalculateFunds(account);
                    var loan = account.CurrentLoan;
                    CalculateInstallments(loan);
                }

                //ForcastNextLoans(accounts, db);

                db.SaveChanges();
            }
        }

        

        private void CalculateFunds(Account account)
        {
            var yearMonth = account.LastPersistedFundYearMonth.JumpNext();
            var targetYearMonth = YearMonth.Current;

            while (yearMonth <= targetYearMonth)
            {
                account.Funds.Add(new Fund()
                {
                    Amount = FUND_AMOUNT,
                    YearMonth = yearMonth
                });
                yearMonth = yearMonth.JumpNext();
            }
        }

        private void CalculateInstallments(Loan loan)
        {
            if (loan == null)
                return;
            var yearMonth = loan.Account.LastPersistedFundYearMonth.JumpNext();
            var targetYearMonth = YearMonth.Current;

            while (yearMonth <= targetYearMonth)
            {
                //در سال ماه دریافت وام نباید قسط پرداخت شود
                if (loan.YearMonth == yearMonth)
                {
                    yearMonth = yearMonth.Next;
                    continue;
                }
                var installmentAmount = loan.InstallmentAmount;
                if (yearMonth <= loan.EndYearMonth)
                {
                    loan.Installments.Add(new Installment()
                    {
                        Amount = installmentAmount,
                        YearMonth = yearMonth
                    });
                }
                yearMonth = yearMonth.JumpNext();

            }

        }

        //private void ForcastNextLoans(List<Account> accounts, CashEntities db)
        //{
        //    var accountQueue = new Queue<Account>();
        //    var cash = db.Cashes.First();
        //    var yearMonth = YearMonth.Current.Next;
        //    var loanlessAccounts = (from a in accounts
        //                            where !a.Loans.Any()
        //                            orderby a.Priority
        //                            select a);

        //    foreach (var loanlessAccount in loanlessAccounts)
        //    {
        //        accountQueue.Enqueue(loanlessAccount);
        //    }
        //    while (yearMonth < _TargetYearMonth)
        //    {

        //        var nominateAccounts = GetNextLoanNominateAccounts(yearMonth, accounts);
        //        foreach (var nominateAccount in nominateAccounts)
        //        {
        //            accountQueue.Enqueue(nominateAccount);
        //        }
        //        decimal installmentAmount = (from a in accounts
        //                                     from l in a.Loans
        //                                     from i in l.Installments
        //                                     where i.YearMonth == yearMonth
        //                                     select i.Amount).DefaultIfEmpty(0).Sum();
        //        decimal fundAmount = (from a in accounts
        //                              from f in a.Funds
        //                              where f.YearMonth == yearMonth
        //                              select f.Amount).DefaultIfEmpty(0).Sum();
        //        cash.Amount += installmentAmount + fundAmount;
        //        while (cash.Amount >= LOAN_AMOUNT)
        //        {
        //            if (!accountQueue.Any())
        //                break;
        //            var account = accountQueue.Dequeue();
        //            cash.Amount -= LOAN_AMOUNT;

        //            var loan = new Loan()
        //            {
        //                Account = account,
        //                Amount = LOAN_AMOUNT,
        //                InstallmentAmount = INSTALLMENT_AMOUNT,
        //                CashRemainingAmount = cash.Amount,
        //                Year = yearMonth.Year,
        //                Month = yearMonth.Month
        //            };
        //            account.Loans.Add(loan);
        //            CalculateLoan(loan);

        //        }
        //        yearMonth = yearMonth.Next;
        //    }
        //}

        //private IEnumerable<Account> GetNextLoanNominateAccounts(YearMonth yearMonth, List<Account> accounts)
        //{
        //    var result = new List<Account>();

        //    var endingAccount = (from a in accounts
        //                         where a.Loans.Any(l => l.EndYearMonth == yearMonth.Previous)
        //                         orderby a.Priority
        //                         select a);
        //    result.AddRange(endingAccount);
        //    return result;
        //}
    }
}