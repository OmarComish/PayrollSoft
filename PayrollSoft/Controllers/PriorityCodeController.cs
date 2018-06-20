using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;

namespace PayrollSoft.Controllers
{
    public class PriorityCodeController : Controller
    {
        //
        // GET: /PriorityCode/

        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PriorityCode/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PriorityCode/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(PriorityCodes code)
        {
            PriorityCodes prioritycode = new PriorityCodes();

            prioritycode = PriorityCodeDetails(code.PriorityCode);

            prioritycode.Description = code.Description;
            prioritycode.DateUpdated = DateTime.Now.Date;
            prioritycode.UpdatedBy = (int)Session["LoggedUserId"];

            UpdatePriorityCode(prioritycode);
            
            return Json(true);
        }

        private void UpdatePriorityCode(PriorityCodes prioritycode)
        {
            using (var context = new DataContext())
            {
                try
                {
                    context.Entry(prioritycode).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }
            }
        }

        private PriorityCodes PriorityCodeDetails(int id)
        {
            PriorityCodes priorityCode = new PriorityCodes();
            var results = from pcode in db.PriorityCodes
                          where pcode.PriorityCode == id
                          select new {pcode.PriorityCode,pcode.Description,pcode.DateCreated,pcode.CreatedBy,
                                      pcode.DateUpdated,pcode.UpdatedBy,pcode.VoidedBy,pcode.Voided, pcode.DateVoided};
            foreach (var x in results)
            {
                priorityCode.PriorityCode = x.PriorityCode;
                priorityCode.Description = x.Description;
                priorityCode.DateUpdated = x.DateUpdated;
                priorityCode.DateCreated = x.DateCreated;
                priorityCode.DateVoided = x.DateVoided;
                priorityCode.CreatedBy = x.CreatedBy;
                priorityCode.UpdatedBy = x.UpdatedBy;
                priorityCode.VoidedBy = x.VoidedBy;

            }

            return priorityCode;

        }

        [HttpPost]
        public ActionResult Create(PriorityCodes code)
        {
            string Message = null;
            string Status = null;
            code.CreatedBy = (int)Session["LoggedUserId"];
            code.DateCreated = DateTime.Now.Date;

            try
            {

                if (ModelState.IsValid)
                {
                    db.PriorityCodes.Add(code);
                    db.SaveChanges();
                    Status = "success";
                    Message = "Priority code created successfully";
                }
                else 
                {
                    Message = "Invalid model state";
                    Status = "error";
                }
            }
            catch(Exception e)
            {
                Status = "error";
                Message = e.Message;
            }

            return Json(new {Status = Status, Message = Message });
        }
        
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeletePriorityCode(PriorityCodes code)
        {

            PriorityCodes prioritycode = new PriorityCodes();

            prioritycode = PriorityCodeDetails(code.PriorityCode);

            prioritycode.Voided = 1;
            prioritycode.DateVoided = DateTime.Now.Date;
            prioritycode.VoidedBy = (int)Session["LoggedUserId"];

            UpdatePriorityCode(prioritycode);

            return Json(true);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
