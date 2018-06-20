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
    public class EmployeeGradeController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /EmployeeGrade/

        public ViewResult Index()
        {
            return View(db.EmployeeGrades.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetGrades()
        {
            //var result = db.EmployeeGrades.ToList();
            var grades = from g in db.EmployeeGrades
                         select new { g.GradeId, g.GradeName,g.MinSalary,
                                     g.AllowanceRefNum,g.InsuranceCode,g.PensionRefNum,
                                     g.OvertimeRate,g.ThresholdID};
            return Json(grades,JsonRequestBehavior.AllowGet);
        
        }
        //
        // GET: /EmployeeGrade/Details/5

        public ViewResult Details(string id)
        {
            EmployeeGrade employeegrade = db.EmployeeGrades.Find(id);
            return View(employeegrade);
        }

        //
        // GET: /EmployeeGrade/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /EmployeeGrade/Create

        [HttpPost]
        public ActionResult Create(EmployeeGrade employeegrade)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeGrades.Add(employeegrade);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Json(true);
                
            }

            //return View(employeegrade);
            return Json(false);
        }
        
        //
        // GET: /EmployeeGrade/Edit/5
 
        public ActionResult Edit(string id)
        {
            EmployeeGrade employeegrade = db.EmployeeGrades.Find(id);
            return View(employeegrade);
            //return Json(true);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(EmployeeGrade employeegrade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeegrade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post),ActionName("Update")]
        public ActionResult Update(EmployeeGrade employeegrade)
        {
            if (ModelState.IsValid) 
            {
                db.Entry(employeegrade).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }

            return Json(false);
        }
        //
        // GET: /EmployeeGrade/Delete/5
 
        public ActionResult Delete(string id)
        {
            EmployeeGrade employeegrade = db.EmployeeGrades.Find(id);
            return View(employeegrade);
        }

        //
        // POST: /EmployeeGrade/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            EmployeeGrade employeegrade = db.EmployeeGrades.Find(id);
            db.EmployeeGrades.Remove(employeegrade);
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