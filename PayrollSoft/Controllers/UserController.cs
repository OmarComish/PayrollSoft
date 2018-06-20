using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using PayrollSoft.Utility;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using PayrollSoft.BusinessLogicLayer;

namespace PayrollSoft.Controllers
{
    public class UserController : Controller
    {
        private UtilityBase util = new UtilityBase();

        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeUserAccount(string id)
        {
            string __status = string.Empty, __message = string.Empty;

            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendTxn = new PendingTransaction(id, (int)Session["LoggedUserId"], "UserProfile", "User", "Users");
                    User user = context.Users.FirstOrDefault(x => x.UserName == id);
                    user.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    user.RecordStatusId = util.GetRecordStatusId("AUTHORIZED");
                    user.RecordStatusDateChanged = DateTime.Now.Date;
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();

                    pendTxn.ClearPendingTransaction();
                    __status = "success";
                    __message = "Record successfully authorized";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: AuthorizeUserAccount() in settings controller");
                    __status = "error";
                    __message = e.Message.ToString();
                }

                return Json(new { status = __status, message = __message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult CreateUser(User user)
        {
            string _status = string.Empty, _message = string.Empty;
            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendTxn = new PendingTransaction(user.UserName, (int)Session["LoggedUserId"], "UserProfile", "User", "Users");
                    user.DateCreated = DateTime.Now.Date;
                    user.CreatedBy = (int)Session["LoggedUserId"];
                    user.RecordStatusId = util.GetRecordStatusId("PENDING");

                    context.Users.Add(user);
                    context.SaveChanges();
                    pendTxn.LogPendingTransaction();
                    _status = "success";
                    _message = "Record successfully created";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    _status = "error";
                    _message = e.Message.ToString();
                }

                return Json(new { status = _status, message = _message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetRecordToAuthorize(string id)
        {
            var result = from u in db.Users
                         where u.UserName == id
                         select new {u.UserId,u.UserName,u.Status,u.FullName,u.DateCreated };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

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

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

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
