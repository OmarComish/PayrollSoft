using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.EntitySql;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using PayrollSoft.Utility;
using System.Data.Common;
using PayrollSoft.BusinessLogicLayer;

namespace PayrollSoft.Controllers
{
    public class UserRoleRecord
    {
        public string ModuleId { get; set; }
        public string UserName { get; set; }
        public int CreateRole { get; set; }
        public int ReadRole { get; set; }
        public int UpdateRole { get; set; }
        public int DeleteRole { get; set; }
        public int AuthRole { get; set; }
    }

    public class SecurityController : Controller
    {

        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            ViewBag.Message = Session["LoggedUserFullName"];
            return View();
        }

        public ActionResult LoanSettings()
        {
            //ViewBag.Message = "We are in the security Module";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetRecordToAuthorize(int id)
        {
            var result = from up in db.UserPriviledges
                         where up.Id == id
                         join u in db.Users on up.UserId equals u.UserId
                         select new { up.ModuleId, up.CreateRole,up.ReadRole,up.DeleteRole,up.UpdateRole,up.AuthRole,u.FullName,up.RecordStatusDateChanged };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeUserRole(int id)
        {
            string __status = string.Empty, __message = string.Empty;
            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendTxn = new PendingTransaction(id.ToString(), (int)Session["LoggedUserId"], "User", "Security", "UserPriviledges");
                    UserPriviledge userpriv = context.UserPriviledges.FirstOrDefault(x => x.Id == id);
                    userpriv.RecordStatusId = util.GetRecordStatusId("AUTHORIZED");
                    userpriv.RecordStatusDateChanged = DateTime.Now.Date;
                    userpriv.RecordStatusChangedBy = (int)Session["LoggedUserId"];

                    context.Entry(userpriv).State = EntityState.Modified;
                    context.SaveChanges();
                    pendTxn.ClearPendingTransaction();

                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: AuthorizeUserRole in Security controller");
                    __status = "error";
                    __message = e.Message.ToString();
                }

                return Json(new {Status = __status, Message = __message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeUserAccount(string id)
        {
            string __status = string.Empty, __message = string.Empty;

            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendTxn = new PendingTransaction(id, (int)Session["LoggedUserId"], "User", "User", "Users");
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

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUserNames()
        {
            var result = from u in db.Users
                         select new {u.FullName,u.UserName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUserPriviledges(string UserName)
        {
            //int i = (int)Session["UserId"];
            int id = GetUserId(UserName);

            var results = from u in db.UserPriviledges
                          join s in db.Users on u.UserId equals s.UserId
                          where u.UserId == id
                          select new {u.Id,s.UserName,u.ModuleId,u.CreateRole,u.ReadRole,u.UpdateRole,u.DeleteRole,u.AuthRole};

            return Json(results,JsonRequestBehavior.AllowGet);
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateUser(User user)
        {
            user.DateCreated = DateTime.Now.Date;
            user.LastSignedOn = DateTime.Now.Date;
            string _status = string.Empty, _message = string.Empty;
            using (var context = new DataContext())
            {
                if (!context.Users.Any(j => j.UserName == user.UserName))
                {
                    if (SaveUserRecord(user))
                    {
                        _status = "success";
                        _message = "User created successfully";
                    }
                }
                else 
                {
                    _status = "error";
                    _message = "Duplicate record! User record with same details found. Please use another name";
                }

                return Json(new {status = _status, message = _message });
            }
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RevokeRoles(UserRoleRecord record)
        {
            string Message = null;
            string Status = null;
            int id = GetUserId(record.UserName);
            UserPriviledge userpriv = new UserPriviledge();
            try
            {
                userpriv = GetRoleDetails(id, record.ModuleId);
                using(var context = new DataContext())
                {
                    context.Entry(userpriv).State = EntityState.Deleted;
                    context.SaveChanges();
                    Message = "Role successfully revoked";
                    Status = "success";
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
                Message = "Error revoking role";
                Status = "error";
            }
            return Json(new { Status = Status, Message = Message });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateUserRole(UserRoleRecord role)
        {
            UserPriviledge userpriv = new UserPriviledge();
            string _message = string.Empty, _status = string.Empty;

            userpriv.UserId = GetUserId(role.UserName);
            
            Session["UserId"] = userpriv.UserId;

            using(var context = new DataContext())
            {
                try
                {
                    if (!context.UserPriviledges.Any(j => j.UserId == userpriv.UserId && j.ModuleId == role.ModuleId))
                    {
                       
                        userpriv.Id = 0;
                        userpriv.ModuleId = role.ModuleId;
                        userpriv.CreateRole = role.CreateRole;
                        userpriv.ReadRole = role.ReadRole;
                        userpriv.UpdateRole = role.UpdateRole;
                        userpriv.DeleteRole = role.DeleteRole;
                        userpriv.AuthRole = role.AuthRole;
                        userpriv.CreatedBy = (int)Session["LoggedUserId"];
                        userpriv.DateCreated = DateTime.Now.Date;
                        userpriv.RecordStatusId = util.GetRecordStatusId("PENDING");
                        userpriv.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                        userpriv.RecordStatusDateChanged = DateTime.Now.Date;

                        if (SaveUserRole(userpriv))
                        {
                            int id = GetLastRecordId();
                            PendingTransaction pendTxn = new PendingTransaction(id.ToString(), (int)Session["LoggedUserId"], "User", "Security", "UserPriviledges");
                            pendTxn.LogPendingTransaction();
                            _message = "Record saved successfully";
                            _status = "success";
                        }
 
                    }
                    else
                    {                
                            _message = "The entry already exist for this user.";
                            _status = "error";
                    }
                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString());
                    _message = e.Message.ToString();
                    _status = "error";
                }

                return Json(new { Status = _status, Message = _message });
            
            }
            
   
        }

        private int GetLastRecordId()
        {
            using (var context = new DataContext())
            {
                var result = (from up in context.UserPriviledges
                              select up.Id).Max();
                return result;
            }
        }

        private Boolean SaveUserRole(UserPriviledge userpriviledge)
        {
            
                using (var context = new DataContext())
                {
                    Boolean _saved;
                    try
                    {
                        context.UserPriviledges.Add(userpriviledge);
                        context.SaveChanges();
                        _saved = true;
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString() + " Source: SaveUserRole() in Security controller");
                        _saved = false;
                    }

                    return _saved;
                }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateUserRole(UserRoleRecord role)
        {
           int userId = GetUserId(role.UserName);
           string __status = string.Empty, __message = string.Empty;


            if (ModelState.IsValid)
            {
                   using(var context = new DataContext())
                    {
                        try
                        {

                            UserPriviledge userpriv = context.UserPriviledges.FirstOrDefault(x => x.UserId == userId && x.ModuleId == role.ModuleId);
                            PendingTransaction pendTxn = new PendingTransaction(userpriv.Id.ToString(), (int)Session["LoggedUserId"], "User", "Security", "UserPriviledges");
                            userpriv.CreateRole = role.CreateRole;
                            userpriv.ReadRole = role.ReadRole;
                            userpriv.UpdateRole = role.UpdateRole;
                            userpriv.DeleteRole = role.DeleteRole;
                            userpriv.AuthRole = role.AuthRole;
                            userpriv.ModifiedBy = (int)Session["LoggedUserId"];
                            userpriv.DateModified = DateTime.Now.Date;
                            userpriv.RecordStatusId = util.GetRecordStatusId("PENDING");
                            userpriv.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                            userpriv.RecordStatusDateChanged = DateTime.Now.Date;

                            context.Entry(userpriv).State = EntityState.Modified;
                            context.SaveChanges();
                            pendTxn.LogPendingTransaction();
                            __message = "Changes applied successfully";
                            __status = "success";
                        }
                        catch (Exception e)
                        {
                            util.WriteToLog(e.Message.ToString() + " Source: UpdateUserRole() in Security controller");
                            __message = "Changes applied successfully";
                            __status = "success";
                        }
                    }
               
            }

            return Json(new { Status = __status, Message = __message });
        }

        private Boolean SaveUserRecord(User user)
        {
            Boolean _saved;
            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendTxn = new PendingTransaction(user.UserName, (int)Session["LoggedUserId"], "User", "User", "Users");
                    user.DateCreated = DateTime.Now.Date;
                    user.CreatedBy = (int)Session["LoggedUserId"];
                    user.RecordStatusId = util.GetRecordStatusId("PENDING");

                    context.Users.Add(user);
                    context.SaveChanges();
                    pendTxn.LogPendingTransaction();
                    _saved = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    _saved = false;
                }
                return _saved;
            }
        }

        private int GetUserId(string username)
        {
            int UserId = 0;
             using( var context = new DataContext())
             {
                 var results = from u in context.Users
                                where u.UserName == username
                                select new {u.UserId };
                 foreach (var x in results)
                 {
                     UserId = x.UserId;
                 }

                 return UserId;
             }
        }

        private UserPriviledge GetRoleDetails(int userid, string module)
        {
            UserPriviledge userpriv = new UserPriviledge();
            var result = from u in db.UserPriviledges
                         where u.UserId == userid && u.ModuleId == module
                         select new { u.Id, u.UserId, u.ModuleId, u.CreateRole, u.ReadRole, u.UpdateRole,
                                     u.DeleteRole,u.DateCreated,u.DateModified,u.CreatedBy,u.ModifiedBy };
            foreach(var j in result)
            {
                userpriv.Id = j.Id;
                userpriv.UserId = j.UserId;
                userpriv.ModuleId = j.ModuleId;
                userpriv.CreateRole = j.CreateRole;
                userpriv.ReadRole = j.ReadRole;
                userpriv.UpdateRole = j.UpdateRole;
                userpriv.DeleteRole = j.DeleteRole;
                userpriv.DateCreated = j.DateCreated;
                userpriv.DateModified = j.DateModified;
                userpriv.CreatedBy = j.CreatedBy;
            }

            return userpriv;
        }
        
    }
}
