using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;

namespace PayrollSoft.AuditTrails
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            ///<summary>
            ///Below the record to save to the database is constructed
            ///</summary>
            
            var request = filterContext.HttpContext.Request;

            Audit auditrail = new Audit()
            {
                
                AuditID = Guid.NewGuid(),
                UserName = (request.IsAuthenticated)? filterContext.HttpContext.User.Identity.Name: "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"]?? request.UserHostAddress,
                AreaAccessed = request.RawUrl,
                TimeStamp = DateTime.UtcNow
            };
   

            createAuditTrail(auditrail);

            base.OnActionExecuted(filterContext);
        }

        private void createAuditTrail(Audit audit) 
        {

            ///<summary>
            ///save the audit trail record to Audits table.This method gets the audit as an argument from the caller of the method.
            ///</summary>
            
             using(DataContext context = new DataContext())
             {
                 context.Audits.Add(audit);
                 context.SaveChanges();
             }
        }
    }
}