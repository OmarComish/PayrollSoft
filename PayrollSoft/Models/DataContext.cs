using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PayrollSoft.Models
{
    public class DataContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPriviledge> UserPriviledges { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AccessLevel> AccessLevels { get; set; }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<AllowanceCategory> AllowanceCategories { get; set; }
        public DbSet<AppConfig_LeavePay> AppConfig_LeavePay { get; set; }
        public DbSet<ApplicationModule> ApplicationModules { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<creditGeneralLedger> creditGeneralLedger { get; set; }
        public DbSet<DeductionPayments> DeductionPayments { get; set; }
        public DbSet<Deductions> Deductions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EarningPayments> EarningPayments { get; set; }
        public DbSet<Earnings> Earnings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeGrade> EmployeeGrades { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<InsuranceCategory> InsuranceCategories { get; set; }
        public DbSet<LoanAmortization> LoanAmortizations { get; set; }
        //public DbSet<LoanBalance> LoanBalances { get; set; }
        public DbSet<LoanDetails> LoanDetails { get; set; }
        public DbSet<loanMaster> LoanMasters { get; set; }
        public DbSet<LoanPortifolio> LoanPortifolios { get; set; }
        public DbSet<LoanPortfolioFirstPart> LoanPortfolioFirstParts { get; set; }
        public DbSet<LoanPortfolioSecondPart> LoanPortfolioSecondParts { get; set; }
        public DbSet<LoanRegister> LoanRegisters { get; set; }
        public DbSet<LoanSettings> LoanSettings { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        //public DbSet<MasterCodes> MasterCodes { get; set; }
        public DbSet<PaymentCodes> PaymentCodes { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollHistLog> PayrollHistLogs { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<Pension> Pensions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PersonnelPayslip> PersonnelPayslips { get; set; }
        public DbSet<PriorityCodes> PriorityCodes { get; set; }
        public DbSet<RotativaTest> RotativaTests { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Taxation> Taxations { get; set; }
        public DbSet<TaxationThreshold> TaxationThresholds { get; set; }
        public DbSet<GroupPayrollPayment> GroupPayrollPayments { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<PersonnelSettings> PersonnelSettings { get; set; }
        public DbSet<JournalAccount> JournalAccounts { get; set; }
        public DbSet<PayrollFormulae> PayrollFormulae { get; set; }
        public DbSet<SeveranceEarning> SeveranceEarnings { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<PersonalIdentification> PersonalIdentifications { get; set; }
        public DbSet<ProfessionalQualification> ProfessionalQualifications { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<RecordStatus> RecordStatuses { get; set; }
        public DbSet<PendingItem> PendingItems { get; set; }
        public DbSet<PensionAccount> PensionAccounts { get; set; }
        public DbSet<FrozenLoan> FrozenLoans { get; set; }
        public DbSet<WithHoldingTax> WithHoldingTaxes { get; set; }
        public DbSet<Overtime> OverTimes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskTracker> TaskTrackers { get; set; }


    }
}