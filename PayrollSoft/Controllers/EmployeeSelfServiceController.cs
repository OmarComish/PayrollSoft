using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using PayrollSoft.Utility;
using PayrollSoft.BusinessLogicLayer;

namespace PayrollSoft.Controllers
{
    public class EmployeeSelfServiceController : Controller
    {
        private DataContext db = new DataContext();
        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestLeave()
        {
            SessionActive();
            return View();
        }

        public ActionResult TimeSheet()
        {
            SessionActive();
            return View();
        }

        public ActionResult Payslips()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SubmitRequest(LeaveRequest record)
        {
            SessionActive();

            using (var context = new DataContext())
            {
                string _status = string.Empty, _message = string.Empty; 
                try
                {
                    record.EmployeeId = GetEmployeeId();
                    record.CreatedBy = (int)Session["LoggedUserId"];
                    record.DateCreated = DateTime.Now.Date;
                    record.RecordStatusId = util.GetRecordStatusId("PENDING");
                    record.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    record.RecordStatusDateChanged = DateTime.Now.Date;

                    context.LeaveRequests.Add(record);
                    context.SaveChanges();

                    PendingTransaction pendTxn = new PendingTransaction(GetIdForLeaveRequest().ToString(), (int)Session["LoggedUserId"], "ESS", "ESS", "LeaveRequests");
                    pendTxn.LogPendingTransaction();

                    _status = "success";
                    _message = "Leave request submitted";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: method SubmitRequest() in EmployeeSelfService controller");
                    _status = "error";
                    _message = "Failed to submit request " + e.Message.ToString();
                }

                return Json(new {status = _status, message = _message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SubmitTimesheet(TaskTracker record)
        {
            SessionActive();
            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTimeSheet()
        {
            SessionActive();

            int Id = (int)Session["LoggedUserId"];
            DateTime __date = DateTime.Now.Date;

            var res = from t in db.TaskTrackers
                      where t.CreatedBy == Id && t.DateCreated == __date
                      select new {t.Activity, t.BeginTime, t.FinishTime, t.Duration };

            return Json(res, JsonRequestBehavior.AllowGet);                     
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetWeeklyTimeSheet(DateTime __mindate, DateTime __maxdate)
        {
            int Id = (int)Session["LoggedUserId"];
            
            var res = from t in db.TaskTrackers
                      where t.CreatedBy == Id && t.DateCreated >= __mindate && t.DateCreated <= __maxdate
                      select new { t.Activity, t.Duration, t.DateCreated };

            return Json(res, JsonRequestBehavior.AllowGet);   
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddTimesheetEntry(TaskTracker record)
        {
            using (var _context = new DataContext())
            {
                string _status = string.Empty, _message = string.Empty;
                try 
                {
                    record.CreatedBy = (int)Session["LoggedUserId"];
                    record.DateCreated = DateTime.Now.Date;

                    _context.TaskTrackers.Add(record);
                    _context.SaveChanges();

                    _status ="success";
                    _message ="Timesheet entry saved";
                }
                catch(Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: method AddTimesheetEntry() in EmployeeSelfService controller");
                    _status = "error";
                    _message = "Failed to save entry " + e.Message.ToString();
                }

                return Json(new { status = _status, message = _message });
            }
        }


        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /EmployeeSelfService/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EmployeeSelfService/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

        public ActionResult Delete(int id)
        {
            return View();
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

        ///<summary>
        /// Private Region
        /// </summary>
        private string GetEmployeeId()
        { 
            string id = string.Empty;
            using (var context = new DataContext())
            {
                try
                {
                    User user = context.Users.Find((int)Session["LoggedUserId"]);
                    Employee employee = context.Employees.Where(x => x.EmpName == user.FullName).FirstOrDefault();
                    id = employee.EmpID;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: method GetEmployeeId() in EmployeeSelfService controller");
                }

                return id;
            }                      
        }

        private int GetIdForLeaveRequest()
        {
            using (var context = new DataContext())
            {
                //LeaveRequest request = context.LeaveRequests.Select(x => x.RequestId).Max();
                int id = context.LeaveRequests.Select(x => x.RequestId).Max();
                return id;
            }
        }

        private void SessionActive()
        {
            if (Session["LoggedUserId"] == null)
            {
                RedirectToAction("Login", "Home");
                return;
            }
        }
    
    }
}
