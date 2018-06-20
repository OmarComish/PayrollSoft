using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;
using PayrollSoft.BusinessLogicLayer;

namespace PayrollSoft.Controllers
{

    public class employeeList
    {
        public string empName { get; set; }
        public string empID { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string deptID { get; set; }
        public string gradeID { get; set; }
        public DateTime hireDate { get; set; }
        public string jobTitle { get; set; }
        public string mobileNumber { get; set; }
        public string gender { get; set; }
        public string EmploymentStatus {get;set;}
    }

    public class EmployeeRecord
    {
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string DeptID { get; set; }
        public string GradeID { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentType { get; set; }
        public string PaySchedule { get; set; }
        public int onPayroll { get; set; }

        public string IdentityNumber { get; set; }
        public string IdentityType { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }

        public string QualificationName { get; set; }
        public string Description { get; set; }
        public DateTime DateAttained { get; set; }

        public string NextOfKinName { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
    }

    public class EmployeeController : Controller
    {
        private UtilityBase util = new UtilityBase();

        private DataContext db = new DataContext();

        public ViewResult Index()
        {
            //var employees = db.Employees.Include(e => e.Departments).Include(e => e.EmployeeGrades);
           // return View(employees.ToList());
            ViewBag.Message = Session["LoggedUserFullName"];
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEmployees()
        {
            var result = db.Employees.ToList();
            var employee = from e in result
                           select new {e.EmpName,e.EmpID,e.DateOfBirth,e.DeptID,
                                     e.GradeID,e.Gender,e.HireDate,e.JobTitle,e.MobileNumber1};
            return Json(employee,JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEmploymentTypes()
        {
            var result = from et in db.EmploymentTypes
                         select new {et.EmploymentTypeId,et.Description };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Employees()
        {
            try
            {
               int recstat = GetRecordStatusId("AUTHORIZED");
               var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           join r in db.Relations on e.EmpID equals r.EmpID
                           join pi in db.PersonalIdentifications on e.EmpID equals pi.EmpID
                           join et in db.EmploymentTypes on e.EmploymentTypeId equals et.EmploymentTypeId
                           where e.RecordStatusId == recstat
                           select new { e.EmpName, 
                                        e.EmpID, d.DeptName,e.DeptID, e.Gender,e.PaySchedule,
                                        e.HireDate, e.DateOfBirth,e.MobileNumber1,e.MobileNumber2,
                                        e.JobTitle,e.GradeID,e.EmploymentStatus,e.onPayroll,e.MaritalStatus,
                                        r.NextOfKinName,r.ContactNo1,r.ContactNo2,pi.IdentityNumber,pi.IdentityType,
                                        pi.Nationality,pi.Address,pi.ValidityDate,et.Description};

                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
               util.WriteToLog(e.Message.ToString() + " Exception:" + e.InnerException.ToString());
               return Json(false);
            }

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDepartments() 
        {
            //var result = db.Departments.ToList();
            var department = from d in db.Departments
                             select new {d.DeptID,d.DeptName };

          
            return Json(department,JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEmployeeGrades()
        {
            var result = db.EmployeeGrades.ToList();
            var grades = from g in result
                             select new { g.GradeId,g.GradeName};
            return Json(grades, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(string id)
        {
            Employee employee = db.Employees.Find(id);
            return View(employee);
        }
      
        public ActionResult Create()
        {
            //ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName");
            //ViewBag.GradeID = new SelectList(db.EmployeeGrades, "GradeId", "GradeName");
            return View();
        }

        [HttpPost]
        public JsonResult GetRecordToAuthorize(string id)
        {
            var record = from emp in db.Employees
                         join r in db.Relations on emp.EmpID equals r.EmpID
                         join p in db.PersonalIdentifications on r.EmpID equals p.EmpID
                         join d in db.Departments on emp.DeptID equals d.DeptID
                         join e in db.EmploymentTypes on emp.EmploymentTypeId equals e.EmploymentTypeId
                         where emp.EmpID == id
                         select new {EmployeeID = emp.EmpID,
                                     EmployeeName = emp.EmpName,
                                     Status = emp.EmploymentStatus,
                                     Department = d.DeptName,
                                     emp.HireDate,
                                     emp.onPayroll,
                                     emp.DateOfBirth,
                                     Position = emp.JobTitle,
                                     emp.Gender,
                                     emp.MaritalStatus,
                                     emp.MobileNumber1,
                                     emp.MobileNumber2,
                                     p.IdentityNumber,
                                     p.IdentityType,
                                     p.ValidityDate,
                                     p.Nationality,
                                     p.Address,
                                     Employment = e.Description,
                                     NextOfKin = r.NextOfKinName,
                                     r.ContactNo1,
                                     r.ContactNo2,
                         };

            return Json(record, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeEmployee(string Id)
        {
            string _message = string.Empty, _status = string.Empty;
           
            using (var context = new DataContext())
            {
                try
                {
                    int recstat = GetRecordStatusId("AUTHORIZED");
                    Relation relation = context.Relations.FirstOrDefault(x => x.EmpID == Id);
                    PersonalIdentification identification = context.PersonalIdentifications.FirstOrDefault(x => x.EmpID == Id);

                    UpdateEmployee(Id, recstat);

                    if (RecordsToAuthorizeExist(Id, "Relations"))
                    {
                        UpdateRelation(relation, recstat);
                    }

                    if (RecordsToAuthorizeExist(Id, "PersonalIdentifications"))
                    {
                        UpdatePersonalIdentification(identification, recstat);
                    }

                    _message = "Record successfully authorized!";
                    _status = "success";

                    if (_status == "success")
                    {
                        DeletePendingItem(Id);
                    }
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.InnerException.ToString() + " " + e.Message.ToString());
                    _message = e.Message.ToString();
                    _status = "error";
                }

                return Json(new {status = _status, _message = _message});

            }
        }

        [HttpPost]
        public JsonResult Create(EmployeeRecord employeerec)
        {
            string status = null,  Message = null;
            Employee employee = new Employee();
            PersonalIdentification personalId = new PersonalIdentification();
            Relation relation = new Relation();
            ProfessionalQualification qualification = new ProfessionalQualification();

            employee.EmpID = employeerec.EmpID;
            employee.EmpName = employeerec.EmpName;
            employee.DeptID = employeerec.DeptID;
            employee.GradeID = employeerec.GradeID;
            employee.Gender = employeerec.Gender;
            employee.HireDate = employeerec.HireDate;
            employee.DateOfBirth = employeerec.DateOfBirth;
            employee.JobTitle = employeerec.JobTitle;
            employee.MaritalStatus = employeerec.MaritalStatus;
            employee.EmploymentStatus = employeerec.EmploymentStatus;
            employee.EmploymentTypeId = GetEmploymentTypeId(employeerec.EmploymentType);
            employee.MobileNumber1 = employeerec.MobileNumber1;
            employee.MobileNumber2 = employeerec.MobileNumber2;
            employee.onPayroll = employeerec.onPayroll;
            employee.CreatedBy = (int)Session["LoggedUserId"];
            employee.DateCreated = DateTime.Now.Date;
            employee.RecordStatusId = GetRecordStatusId("PENDING");
            employee.PaySchedule = employeerec.PaySchedule;

            personalId.EmpID = employeerec.EmpID;
            personalId.IdentityNumber = employeerec.IdentityNumber;
            personalId.IdentityType = employeerec.IdentityType;
            personalId.ValidityDate = employeerec.ValidityDate;
            personalId.Address = employeerec.Address;
            personalId.Nationality = employeerec.Nationality;
            personalId.CreatedBy = (int)Session["LoggedUserId"];
            personalId.DateCreated = DateTime.Now.Date;
            personalId.RecordStatusId = employee.RecordStatusId;

            relation.EmpID = employeerec.EmpID;
            relation.NextOfKinName = employeerec.NextOfKinName;
            relation.ContactNo1 = employeerec.ContactNo1;
            relation.ContactNo2 = employeerec.ContactNo2;
            relation.CreatedBy = (int)Session["LoggedUserId"];
            relation.DateCreated = DateTime.Now.Date;
            relation.RecordStatusId = employee.RecordStatusId;

            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendingTxn = new PendingTransaction(employee.EmpID, employee.CreatedBy, "Employee","Employee", "Employees");

                    if (!context.Employees.Any(x => x.EmpID == employee.EmpID))
                    {
                        context.Employees.Add(employee);
                        context.SaveChanges();
                        pendingTxn.LogPendingTransaction();
                        AddPersonalIdentification(personalId);
                        AddRelation(relation);
                        status = "success";
                        Message = "Record saved successfully";
                    }
                    else
                    {
                        status = "error";
                        Message = "Duplicate key found for EmpId. Please change the EmpId";
                    }

                }
                catch (Exception e)
                {
                    status = "error";
                    Message = e.InnerException.ToString();
                }

                return Json(new {status= status, message = Message });
            }
            
        }
 
        public ActionResult Edit(string id)
        {
            Employee employee = db.Employees.Find(id);
            ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName", employee.DeptID);
            ViewBag.GradeID = new SelectList(db.EmployeeGrades, "GradeId", "GradeName", employee.GradeID);
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName", employee.DeptID);
            ViewBag.GradeID = new SelectList(db.EmployeeGrades, "GradeId", "GradeName", employee.GradeID);
            return View(employee);
        }
 
        public ActionResult Delete(string id)
        {
            Employee employee = db.Employees.Find(id);
            return View(employee);
        }

        [AcceptVerbs(HttpVerbs.Post), ActionName("Update")]
        public ActionResult Update(EmployeeRecord employeerec)
        {
            string __status = null, __message = null;
            //Employee employee = new Employee();
            PersonalIdentification personalId = new PersonalIdentification();
            Relation relation = new Relation();
            ProfessionalQualification qualification = new ProfessionalQualification();

            //employee.EmpID = employeerec.EmpID;
            //employee.EmpName = employeerec.EmpName;
            //employee.DeptID = employeerec.DeptID;
            //employee.GradeID = employeerec.GradeID;
            //employee.Gender = employeerec.Gender;
            //employee.HireDate = employeerec.HireDate;
            //employee.DateOfBirth = employeerec.DateOfBirth;
            //employee.JobTitle = employeerec.JobTitle;
            //employee.MaritalStatus = employeerec.MaritalStatus;
            //employee.EmploymentStatus = employeerec.EmploymentStatus;
            //employee.EmploymentTypeId = GetEmploymentTypeId(employeerec.EmploymentType);
            //employee.MobileNumber1 = employeerec.MobileNumber1;
            //employee.MobileNumber2 = employeerec.MobileNumber2;
            //employee.onPayroll = employeerec.onPayroll;
            //employee.CreatedBy = (int)Session["LoggedUserId"];
            //employee.DateCreated = DateTime.Now.Date;
            //employee.RecordStatusId = GetRecordStatusId("PENDING");

            personalId.EmpID = employeerec.EmpID;
            personalId.IdentityNumber = employeerec.IdentityNumber;
            personalId.IdentityType = employeerec.IdentityType;
            personalId.ValidityDate = employeerec.ValidityDate;
            personalId.Address = employeerec.Address;
            personalId.Nationality = employeerec.Nationality;
            personalId.CreatedBy = (int)Session["LoggedUserId"];
            personalId.DateCreated = DateTime.Now.Date;
            personalId.RecordStatusId = GetRecordStatusId("PENDING");

            relation.EmpID = employeerec.EmpID;
            relation.NextOfKinName = employeerec.NextOfKinName;
            relation.ContactNo1 = employeerec.ContactNo1;
            relation.ContactNo2 = employeerec.ContactNo2;
            relation.CreatedBy = (int)Session["LoggedUserId"];
            relation.DateCreated = DateTime.Now.Date;
            relation.RecordStatusId = personalId.RecordStatusId;

            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendingTxn = new PendingTransaction(employeerec.EmpID, relation.CreatedBy, "Employee","Employee", "Employees");

                    //context.Entry(employee).State = EntityState.Modified;
                    //context.SaveChanges();
                    UpdateEmployeeRecord(employeerec);
                    pendingTxn.LogPendingTransaction();
                    UpdatePersonalIdentification(personalId,relation.RecordStatusId);
                    UpdateRelation(relation,relation.RecordStatusId);
                    __status = "success";
                    __message = "Changes saved successfully";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                    __status = "error";
                    __message = e.Message.ToString();
                }

                return Json(new { status = __status, message = __message });
            }

        }
        
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(string id)
        {
           string __status = string.Empty, __message = string.Empty;
           using (var context = new DataContext())
           {
            Employee employee = context.Employees.Find(id);

                if (!HasActiveLoanContract(id))
                {
                    PendingTransaction pendingTxn = new PendingTransaction(id, (int)Session["LoggedUserId"], "Employee","Employee", "Employees");
                    employee.RecordStatusId = GetRecordStatusId("PENDING");
                    employee.Voided = 1;
                    employee.VoidedBy = (int)Session["LoggedUserId"];
                    employee.DateVoided = DateTime.Now.Date;

                    context.Entry(employee).State = EntityState.Modified;
                    context.SaveChanges();
                    pendingTxn.LogPendingTransaction();
                    __status = "success";
                    __message = "Record deleted successfully";
                
                }
                else
                {
                    __status = "error";
                    __message = "Employee has active loan contract. Please liquidate before you delete";
                }
           }
            return Json(new {status = __status, message = __message });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult SearchParam(string id) 
        {
            var results = from lm in db.LoanMasters
                          join lt in db.LoanTypes on lm.LoanTypeNumber equals lt.Code
                          where lm.LoanRefNumber == id
                          select new { lm.LoanRefNumber, lt.Description, lm.startDate, lm.EndDate, lm.PaybackPeriods, lm.LoanAmount, lm.LoanBalance };
            return Json(results);
            //var returnparam = from e in db.Employees
            //                  where e.EmpID == id
            //                  select new { e.DeptID };
            //return Json(returnparam, JsonRequestBehavior.AllowGet);
         
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetNames(string prefix)
        {
            var names = (from n in db.Employees
                         where n.EmpName.StartsWith(prefix) select new {n.EmpName});
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByName(string empName)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.EmpName == empName
                           select new
                           {
                               e.EmpName,
                               e.EmpID,
                               d.DeptName,
                               e.DeptID,
                               e.Gender,
                               e.HireDate,
                               e.DateOfBirth,
                               e.MobileNumber1,
                               e.JobTitle,
                               e.GradeID,
                               e.EmploymentStatus
                           };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByPersonnelId(string empId)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.EmpID == empId
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByGradeId(string GradeId)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.GradeID == GradeId
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByDesignation(string designation)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.JobTitle == designation
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByStatus(string status)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.EmploymentStatus == status
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByEmploymentStartDate(DateTime startdate)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.HireDate == startdate
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByGender(string gender)
        {
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.Gender == gender
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByDepartment(string department)
        {
            string dept = GetDeptId(department);
            var employee = from e in db.Employees
                           join d in db.Departments on e.DeptID equals d.DeptID
                           where e.DeptID == dept
                           select new { e.EmpName, e.EmpID, d.DeptName, e.DeptID, e.Gender,e.HireDate,
                                        e.DateOfBirth,e.MobileNumber1,e.JobTitle,e.GradeID,e.EmploymentStatus };

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult RecordExists(string id)
        {

            var returnparam = from e in db.Employees
                              where e.EmpID == id
                              select new { e.EmpID };
            return Json(returnparam, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Private Region
        /// </summary>
        private void AddPersonalIdentification(PersonalIdentification personalId)
        {
            using (var context = new DataContext())
            {
                PendingTransaction pendingTxn = new PendingTransaction(personalId.EmpID, personalId.CreatedBy, "Employee","Employee", "PersonalIdentifications");
                try
                {
                        context.PersonalIdentifications.Add(personalId);
                        context.SaveChanges();
                        pendingTxn.LogPendingTransaction();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.Source.ToString());
                }
            }
        }

        private void AddRelation(Relation relation)
        {
            using (var context = new DataContext())
            {
                PendingTransaction pendingTxn = new PendingTransaction(relation.EmpID,relation.CreatedBy,"Employee","Employee","Relations");

                try
                {
                    context.Relations.Add(relation);
                    context.SaveChanges();
                    pendingTxn.LogPendingTransaction();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.Source.ToString());
                }
            }
        }

        private void AddProfessionalQualification(PersonalIdentification personalId)
        {
            using (var context = new DataContext())
            {
                try
                {

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private string GetDeptId(string department)
        {
            string deptid = null;
            var dept = from d in db.Departments
                       where d.DeptName == department
                       select new {d.DeptID };
             foreach(var x in dept) 
             {
                 deptid = x.DeptID;
             }

             return deptid;
        }

        private int GetRecordStatusId(string status)
        {
            int id = 0;
            using(var context = new DataContext())
            {
  
                try
                {
                    RecordStatus recordstat = context.RecordStatuses.FirstOrDefault(x => x.Description == status);
                    id = recordstat.RecordStatusId;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + "" + e.Source.ToString());
                }
            }


            return id;
        }

        private string GetEmploymentTypeId(string p)
        {

            using (var context = new DataContext())
            {
                string id = string.Empty;
                try
                {
                    EmploymentType employmenttype = context.EmploymentTypes.FirstOrDefault(x => x.Description == p);
                    id = employmenttype.EmploymentTypeId;

                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                }

                return id;

            }

        }

        private Boolean UpdateEmployee(string Id, int recordstat)
        {
            Boolean result = false;
            using(var context = new DataContext())
            {
                Employee employee = context.Employees.Find(Id);
                employee.RecordStatusId = recordstat;
                context.Entry(employee).State = EntityState.Modified;
                context.SaveChanges();
                result = true;
            }

            return result;
        }

        private Boolean UpdateRelation(Relation rec, int recordstat)
        {
            Boolean result = false;
            
            using(var context = new DataContext())
            {
                Relation relation = context.Relations.FirstOrDefault(x => x.EmpID == rec.EmpID);
                relation.RecordStatusId = recordstat;
                context.Entry(relation).State = EntityState.Modified;
                context.SaveChanges();
                result = true;
            }

            return result;
        }

        private Boolean UpdatePersonalIdentification(PersonalIdentification identification, int recordstat)
        {
            Boolean result = false;
            identification.RecordStatusId = recordstat;
            using (var context = new DataContext())
            {
                context.Entry(identification).State = EntityState.Modified;
                context.SaveChanges();
                result = true;
            }

            return result;
        }

        private Boolean RecordsToAuthorizeExist(string Id, string model)
        {
            Boolean recordfound = false;
            int status = GetRecordStatusId("PENDING");
            if (model == "Relation")
            {
                using (var context = new DataContext())
                {
                    var result = from r in context.Relations
                                 where r.EmpID == Id && r.RecordStatusId == status
                                 select new { r.Rid };
                    recordfound = (result.Count() > 0 ? true : false);
                }

            }
            else
            {
                using (var context = new DataContext())
                {
                    var result = from p in context.PersonalIdentifications
                                 where p.EmpID == Id && p.RecordStatusId == status
                                 select new {p.IdentityNumber};
                    recordfound = (result.Count() > 0 ? true : false);
                }
            }

            return recordfound;

        }

        private Boolean UpdateEmployeeRecord(EmployeeRecord employeerec)
        {
            Boolean result = false;
            using (var context = new DataContext())
            {
                Employee employee = context.Employees.Find(employeerec.EmpID);

                employee.EmpID = employeerec.EmpID;
                employee.EmpName = employeerec.EmpName;
                employee.DeptID = employeerec.DeptID;
                employee.GradeID = employeerec.GradeID;
                employee.Gender = employeerec.Gender;
                employee.HireDate = employeerec.HireDate;
                employee.DateOfBirth = employeerec.DateOfBirth;
                employee.JobTitle = employeerec.JobTitle;
                employee.MaritalStatus = employeerec.MaritalStatus;
                employee.EmploymentStatus = employeerec.EmploymentStatus;
                employee.EmploymentTypeId = GetEmploymentTypeId(employeerec.EmploymentType);
                employee.MobileNumber1 = employeerec.MobileNumber1;
                employee.MobileNumber2 = employeerec.MobileNumber2;
                employee.onPayroll = employeerec.onPayroll;
                employee.CreatedBy = (int)Session["LoggedUserId"];
                employee.DateCreated = DateTime.Now.Date;
                employee.RecordStatusId = GetRecordStatusId("PENDING");

                context.Entry(employee).State = EntityState.Modified;
                context.SaveChanges();
                result = true;
            }

            return result;
        }

        private Boolean HasActiveLoanContract(string Id)
        {
            Boolean __hascontracts;
            try
            {
                using (var context = new DataContext())
                {

                    List<loanMaster> loan = context.LoanMasters.Where(x => x.EmpID == Id && x.Status == "Active").ToList();
                    __hascontracts = loan.Count() > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + "" + e.InnerException.Source.ToString());
                __hascontracts = false;
            }

            return __hascontracts;
        }
        private void DeletePendingItem(string Id)
        {
            using (var context = new DataContext())
            {
                try
                {
                    PendingItem pendingitem = context.PendingItems.FirstOrDefault(x => x.ReferenceNumber == Id);
                    context.Entry(pendingitem).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}