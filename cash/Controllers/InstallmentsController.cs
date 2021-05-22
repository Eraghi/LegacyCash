using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cash.Models;

namespace Cash.Controllers
{
    public class InstallmentsController : Controller
    {
        private CashEntities db = new CashEntities();

        // GET: Installments
        public ActionResult Index(int? loanID)
        {
            if (loanID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var installments = db.Installments.Where(i=>i.LoanID == loanID).Include(i => i.Loan);
            return View(installments.ToList());
        }

        // GET: Installments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment installment = db.Installments.Find(id);
            if (installment == null)
            {
                return HttpNotFound();
            }
            return View(installment);
        }

        // GET: Installments/Create
        public ActionResult Create()
        {
            ViewBag.LoanID = new SelectList(db.Loans, "ID", "ID");
            return View();
        }

        // POST: Installments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount,LoanID,Year,Month")] Installment installment)
        {
            if (ModelState.IsValid)
            {
                db.Installments.Add(installment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoanID = new SelectList(db.Loans, "ID", "ID", installment.LoanID);
            return View(installment);
        }

        // GET: Installments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment installment = db.Installments.Find(id);
            if (installment == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoanID = new SelectList(db.Loans, "ID", "ID", installment.LoanID);
            return View(installment);
        }

        // POST: Installments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Amount,LoanID,Year,Month")] Installment installment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoanID = new SelectList(db.Loans, "ID", "ID", installment.LoanID);
            return View(installment);
        }

        // GET: Installments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment installment = db.Installments.Find(id);
            if (installment == null)
            {
                return HttpNotFound();
            }
            return View(installment);
        }

        // POST: Installments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Installment installment = db.Installments.Find(id);
            db.Installments.Remove(installment);
            db.SaveChanges();
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
    }
}
