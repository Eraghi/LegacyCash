using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cash.Models;
using Cash.ViewModels;

namespace Cash.Controllers
{
    public class AccountsController : Controller
    {
        private CashEntities db = new CashEntities();

        // GET: Accounts
        public async Task<ActionResult> Index(int? personID)
        {
            if (personID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accounts = db.Accounts.Include(a => a.Loans).Include(a => a.Loans.Select(l => l.Installments)).Include(a => a.Funds).Where(a=>a.PersonID == personID && a.IsActive).ToList();
            var accountList = accounts.Select(a => new AccountLoanViewModel
            {
                AccountID = a.ID,
                LoanID = a.CurrentLoan != null ? a.CurrentLoan.ID : 0,
                Name = a.Name,
                Value = a.Value,
                BaselineValue = a.BaseLineValue,
                LoanAmount = a.CurrentLoan != null ? a.CurrentLoan.Amount : 0,
                PaidAmount = a.CurrentLoan != null ? a.CurrentLoan.PaidAmount : 0,
                RemainingAmount = a.CurrentLoan != null ? a.CurrentLoan.RemainingAmount : 0,
                InstallmentQty = a.CurrentLoan != null ? a.CurrentLoan.InstallmentQty : 0,
                InstallmentAmount = a.CurrentLoan != null ? a.CurrentLoan.InstallmentAmount : 0,
                PaidInstallmentQty = a.CurrentLoan != null ? a.CurrentLoan.PaidInstallmentQty : 0,
                RemainingInstallmentQty = a.CurrentLoan != null ? a.CurrentLoan.RemainingInstallmentQty : 0,
                LoanYear = a.CurrentLoan != null ? a.CurrentLoan.Year : default(int?),
                LoanMonth = a.CurrentLoan != null ? a.CurrentLoan.Month : default(int?),
                EndYearMonth = a.CurrentLoan != null ? a.CurrentLoan.EndYearMonth : null,
                LastInstallmentYearMonth = a.CurrentLoan != null ? a.CurrentLoan.LastInstallmentYearMonth : null
            });
            return View(accountList);
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.People, "ID", "FirstName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,BaseLineValue,Value,PersonID")] Account account)
        {
            if (ModelState.IsValid)
            {
                var person = db.People.First(p => p.ID == account.PersonID);
                //account.Name = $"{person.FirstName} {person.LastName}";
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.People, "ID", "FirstName", account.PersonID);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.People, "ID", "FirstName", account.PersonID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,BaseLineValue,Value,PersonID")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.People, "ID", "FirstName", account.PersonID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(account);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult EditAccount(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult EditLoan(int? id)
        {
            throw new NotImplementedException();
        }

        public ActionResult DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult DeleteLoan(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
