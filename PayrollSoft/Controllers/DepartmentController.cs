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
    public class DepartmentController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Department/

        public ViewResult Index()
        {
            return View(db.Departments.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDepartments()
        {
            var result = db.Departments.ToList();
            var departments = from d in result
                              select new { d.DeptID, d.DeptName };
            return Json(departments, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Department/Details/5

        public ViewResult Details(string id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Json(true);
            }

            //return View(department);
            return Json(false);
        }
        
        //
        // GET: /Department/Edit/5
 
        public ActionResult Edit(string id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        //
        // GET: /Department/Delete/5
 
        public ActionResult Delete(string id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }

        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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