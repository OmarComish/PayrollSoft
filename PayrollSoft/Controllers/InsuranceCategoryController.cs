using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{ 
    public class InsuranceCategoryController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /InsuranceCategory/

        public ViewResult Index()
        {
            var insurancecategory = db.InsuranceCategories.Include(i => i.Insurances);
            return View(insurancecategory.ToList());
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetInsuranceCategory(string columnId)
        {
            var insuranceCat = from i in db.InsuranceCategories
                               where i.InsuranceCode == columnId
                               select new {i.InsuranceCode,i.Description,i.MonthlyRate};

            return Json(insuranceCat,JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /InsuranceCategory/Details/5

        public ViewResult Details(string id)
        {
            InsuranceCategory insurancecategory = db.InsuranceCategories.Find(id);
            return View(insurancecategory);
        }

        //check if the value exists in the table
        [HttpPost]
        public JsonResult checkIfRecordExists(string columnId) 
        {
            if (db.InsuranceCategories.Any(o => o.CategoryID == columnId)) 
            {
                return Json(true);
            }

            return Json(false);
        }

        //
        // GET: /InsuranceCategory/Create

        public ActionResult Create()
        {
            ViewBag.InsuranceCode = new SelectList(db.Insurances, "InsuranceCode", "Description");
            return View();
        } 

        //
        // POST: /InsuranceCategory/Create

        [HttpPost]
        public ActionResult Create(InsuranceCategory insurancecategory)
        {
            

                if (ModelState.IsValid)
                {
                    db.InsuranceCategories.Add(insurancecategory);
                    db.SaveChanges();
                    return Json(true);
                }

                return Json(false);
            
            //ViewBag.InsuranceCode = new SelectList(db.Insurance, "InsuranceCode", "Description", insurancecategory.InsuranceCode);
            //return View(insurancecategory);
            
        }
        
        //
        // GET: /InsuranceCategory/Edit/5
 
        public ActionResult Edit(string id)
        {
            InsuranceCategory insurancecategory = db.InsuranceCategories.Find(id);
            ViewBag.InsuranceCode = new SelectList(db.Insurances, "InsuranceCode", "Description", insurancecategory.InsuranceCode);
            return View(insurancecategory);
        }

        //
        // POST: /InsuranceCategory/Edit/5

        [HttpPost]
        public ActionResult Edit(InsuranceCategory insurancecategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurancecategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InsuranceCode = new SelectList(db.Insurances, "InsuranceCode", "Description", insurancecategory.InsuranceCode);
            return View(insurancecategory);
        }

        //
        // GET: /InsuranceCategory/Delete/5
 
        public ActionResult Delete(string id)
        {
            InsuranceCategory insurancecategory = db.InsuranceCategories.Find(id);
            return View(insurancecategory);
        }

        //
        // POST: /InsuranceCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            InsuranceCategory insurancecategory = db.InsuranceCategories.Find(id);
            db.InsuranceCategories.Remove(insurancecategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}