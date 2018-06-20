//Application level Global variables and constants

//VARIABLE: _winID
//TYPE: string
//PURPOSE: to identify each window that is active at a given time at runtime of the application
var _winID = null;

//VARIABLE: _winElementID
//TYPE: string
//PURPOSE: to identify each window element that is active at a given time. E.g the current TAB of the window
var _winElementID = null;

var _mouseClickCount = 0;
var _windowStack = new Array();
var _genderDataSource = ["Male", "Female"];
var _recordinsertSuccessful = false; //Bolean variable to check if INSERT of record into the database succeeded.

//VARIABLE: _toolbarIcon
//TYPE: string
//PURPOSE: to identify each icon on the toolbar clicked
var _toolbarIcon = null;

//VARIABLE: _glController
//TYPE: string
//PURPOSE: to identify the controller in which the events are occurring
//NOTE: Elsewhere in the system, there is a local varible named CONTROLLER, please take note to avoid conflicts.Use them with care or delete 1.
var _glcontroller = null;


//VARIABLE: _glOnPayroll
//TYPE: boolean
//PURPOSE: to specify whether the employee is listed on Payroll or not. By default the value is NOT.
var _glOnPayroll = false;

//VARIABLE: _glAction
//TYPE: string
//PURPOSE: name of the action to invoke in the controller.
var _glAction = null;

//VARIABLE: _glfunctionInvoked
//TYPE: string
//PURPOSE: name of the method/function invoked in the view.
var _glfunctionInvoked = null;

//VARIABLE: _glrecordExists
//TYPE: boolean
//PURPOSE: confirm the existence of a record in a database table
var _glrecordExists = false;
var _glactionState = false;

///<summary>
/// userPrivilegdes is a global array with key value pair of the user roles for each module
/// assignedModules is a global array with all modules the user has access to
/// payslipreportDataSource global array that holds all the data for the payslip report
///</summary>


var userPrivilegdes = [];
var userFullName = null;
var assignedModules = [];
var tableRowColors = ['warning','success'];
var payslipreportEarningsDataSource = [];
var payslipreportDeductionsDataSource = [];
var payslipreportDemographicsDataSource = [];
var payslipreportLeavesDataSource = [];
var payslipreportList = [];
var payslipreportTotalEarnings = 0;
var payslipreportTotalDeductions = 0;
var payslipreportNetPay = 0;
var paymentCodes = [];
var pendingItemsCount = 0;
var notificationText = '<span class="label label-default">'+ pendingItemsCount + '</span>';

var menuItems = [{id: 'homeMenuBtn' , name: '/Home/Index'},
                 {id: 'payrollServiceMenuBtn', name: '/Payroll/Details'},
                 {id: 'payrollListMenuBtn', name: '/Payroll/Create'},
                 {id: 'deductionsMenuBtn', name: '/Deductions/Index'},
                 {id: 'earningsMenuBtn', name: '/Earning/Index'},
                 {id: 'loansMenuBtn', name: '/Loan/Index'},
                 {id: 'userMgtMenuBtn', name: '/Security/Index'},
                 {id: 'leavesMenuBtn', name: '/Settings/HolidaySettings'},
                 {id: 'personnelListMenuBtn', name: '/Employee/Index'},
                 {id: 'personnelNewMenuBtn', name: '/Employee/Create'},
                 {id: 'pensionMenuBtn', name: '/Pension/Create'},
                 {id: 'taxationMenuBtn', name: '/Settings/TaxationSettings'},
                 {id: 'personnelPayslipRpt', name:  '/Report/Index'},
                 {id: 'pensionRpt', name: '/Report/PensionReport'},
                 {id: 'companyPayslipMenuBtn', name: 'CompanyPayslip'},
                 {id: 'personnelsettingsMenuBtn', name: '/Settings/PersonnelSettings'},
                 {id: 'loanSettingsMenuBtn', name: '/Settings/LoanSettings'},
                 {id: 'payrollSettingsMenuBtn', name: '/Settings/PayrollSettings'},
                 {id: 'severanceMenuBtn', name:  '/Settings/SeveranceSettings'},
                 {id: 'overtimeSettingsMenuBtn', name: '/Settings/OvertimeSettings'},
                 {id: 'approvalsBtn', name: '/Approval/Index'},
                 {id: 'requestLeaveMenuBtn', name: '/EmployeeSelfService/RequestLeave'},
                 {id: 'timesheetMenuBtn', name: '/EmployeeSelfService/TimeSheet'},
                 {id: 'payslipsMenuBtn', name: '/EmployeeSelfService/Payslips'}];

var controllerMethodMapping = [{id: 'Employee', controller: 'Employee', action: 'AuthorizeEmployee'},
                               {id: 'Loans', controller: 'Loan', action: 'AuthorizeLoanContract'},
                               {id: 'Pension', controller: 'Pension', action: 'AuthorizePension'},
                               {id: 'Settings', controller: 'Settings', action: 'AuthorizeSettings'},
                               {id: 'Overtime', controller: 'Settings', action: 'AuthorizeOvertime'},
                               {id: 'User', controller: 'Security', action: 'AuthorizeUserRole'},
                               {id: 'Taxation', controller: 'Taxation', action: 'AuthorizeTaxation'},
                               {id: 'UserProfile', controller: 'User', action: 'AuthorizeUserAccount'}];

var crudMethodMapping = [{id: 'saveLoanBtn', controller: 'Settings', action: 'LoanDetails'},
                         {id: 'editLoanBtn', controller: 'Loan', action: 'UpdateLoanDetails'},
                         {id: 'userSaveBtn', controller: 'Security', action: 'CreateUser'},
                         {id: 'grantRoleBtn', controller: 'Security', action: 'CreateUserRole'},
                         {id: 'editRoleBtn', controller: 'Security', action: 'UpdateUserRole'},
                         {id: 'deleteDialogBtn', controller: 'Security', action: 'RevokeRoles'},
                         {id: 'leavesettingsAddBtn', controller: 'Settings', action: 'CreateLeaveType'},
                         {id: 'submitRequestBtn', controller: 'EmployeeSelfService', action: 'SubmitRequest'},
                         {id: 'submitTimeSheetBtn', controller: 'EmployeeSelfService', action: 'SubmitTimeSheet'},
                         {id: 'addTimeSheetEntryBtn', controller: 'EmployeeSelfService', action: 'AddTimeSheetEntry'}];

var toolbarMethodMapping = [{id: 'export-rpt', controller: undefined, action: 'exportReport', url: 'reporturl'},
                            {id: 'edit-icon', controller: undefined, action: 'editUI', url: 'edit'}];

///<summary>
/// applicationModule: global array that holds the collection of all the modules the application has
///</summary>

//var _glapplicationModule = null;

var applicationModule = [{id: 2000, name: 'Payroll'},
                        {id: 2001, name: 'Loans'},
                        {id: 2011, name: 'Deductions'},
                        {id: 2012, name: 'Earnings'},
                        {id: 2002, name: 'Pension'},
                        {id: 2003, name: 'Holiday'},
                        {id: 2004, name: 'Loans'},
                        {id: 2005, name: 'Personnel'},
                        {id: 2006, name: 'Reports'},
                        {id: 2009, name: 'Security'},
                        {id: 2007, name: 'Settings'},
                        {id: 2008, name: 'Severance'},
                        {id: 2010, name: 'Taxation'},
                        {id: 2013, name: 'UserManagement'},
                        {id: 2014, name: 'Leaves'},
                        {id: 2015, name: 'SettingsLoans'},
                        {id: 2016, name: 'Overtime'},
                        {id: 2017, name: 'SettingsPayroll'},
                        {id: 2018, name: 'SettingsPersonnel'},
                        {id: 2019, name: 'ReportPayslip'},
                        {id: 2020, name: 'ReportPension'},
                        {id: 2021, name: 'ReportCompanyPaylip'},
                        {id: 2022, name: 'Pendings'},
                        {id: 1000, name: 'ESS'}];

var moduleData = {
            localdata: applicationModule,
            datatype: 'json',
            datafields:[
              {name:'id',type: 'number'},
              {name:'name',type: 'string'},
            ],
          };

var _glapplicationModule = new $.jqx.dataAdapter(moduleData);

//var _glapplicationModule = new $.jqx.dataAdapter(JSON.stringify(applicationModule));

///<summary>
///_glcreateRole, _glreadRole, _glupdateRole and _gldeleteRole;
///</summary>
_glcreateRole = false;
_glreadRole = false;
_glupdateRole = false;
_gldeleteRole = false;

///<summary>
/// Search options global variables.
///Type: Boolean
///</summary>
var _glsearchByPersonnelName = false;
var _glsearchByPersonnelId = false;
var _glsearchByGradeId = false;
var _glsearchByDesignation = false;
var _glsearchByEmploymentStartDate = false;
var _glsearchByEmploymentStatus = false;
var _glsearchByGender = false;
var _glsearchByDepartment = false;

var _glpersonnelId = null;

///<summary>
/// _glpagehead holds the HTML for the report page head
/// _glpagecontent holds the content for the report page
///</summary>

//_glpagehead = "<!DOCTYPE html><html><head><meta charset="utf-8" /><title>Report - Payslip</title></head><body>";
//_glpagecontent = "Testing the code...";

