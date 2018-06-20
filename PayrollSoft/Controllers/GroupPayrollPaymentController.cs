using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{
    public class GroupPayrollPaymentController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/GroupPayrollPayment
        public IEnumerable<GroupPayrollPayment> GetGroupPayrollPayments()
        {
            return db.GroupPayrollPayments.AsEnumerable();
        }

        // GET api/GroupPayrollPayment/5
        public GroupPayrollPayment GetGroupPayrollPayment(int id)
        {
            GroupPayrollPayment grouppayrollpayment = db.GroupPayrollPayments.Find(id);
            if (grouppayrollpayment == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return grouppayrollpayment;
        }

        // PUT api/GroupPayrollPayment/5
        public HttpResponseMessage PutGroupPayrollPayment(int id, GroupPayrollPayment grouppayrollpayment)
        {
            if (ModelState.IsValid && id == grouppayrollpayment.LogId)
            {
                db.Entry(grouppayrollpayment).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/GroupPayrollPayment
        public HttpResponseMessage PostGroupPayrollPayment(GroupPayrollPayment grouppayrollpayment)
        {
            if (ModelState.IsValid)
            {
                db.GroupPayrollPayments.Add(grouppayrollpayment);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, grouppayrollpayment);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = grouppayrollpayment.LogId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/GroupPayrollPayment/5
        public HttpResponseMessage DeleteGroupPayrollPayment(int id)
        {
            GroupPayrollPayment grouppayrollpayment = db.GroupPayrollPayments.Find(id);
            if (grouppayrollpayment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.GroupPayrollPayments.Remove(grouppayrollpayment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, grouppayrollpayment);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}