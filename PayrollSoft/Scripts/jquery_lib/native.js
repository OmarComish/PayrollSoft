var buttons = null;

function addEventListeners() {


   $('#personnelBtn').on('click',function(){

       if(_userhasAccesstoModule(userPrivilegdes,'Personnel')){

            buttons = "";

            buttons = '<ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">';
            buttons += '<button class="btn btn-default sub_menu_btn" id="search-options" onclick="CustomerAction(this.id)">';
            buttons += '<span class="glyphicon glyphicon-search pull-left"></span>  Search options </button></li>';
            buttons += '<li role="presentation"><button class="btn btn-default sub_menu_btn" id="view-records"';
            buttons += ' onclick="CustomerAction(this.id)"><span class="glyphicon glyphicon-check pull-left"></span>';
            buttons += 'Listings </button></li><li role="presentation" class="active">';
            buttons += '<button class="btn btn-default sub_menu_btn" id="create-record" onclick="CustomerAction(this.id)">';
            buttons += '<span class="glyphicon glyphicon-folder-open pull-left"></span>  Add personnel </button></li>';
            buttons += '<li role="presentation"><button class="btn btn-default sub_menu_btn" id="database-backup"';
            buttons += 'onclick="CustomerAction(this.id)"><span class="glyphicon glyphicon-book pull-left"></span>';
            buttons += 'Database backup</button></li>';
            buttons += '<li role="presentation"><button class="btn btn-default sub_menu_btn" id="criminal-records"';
            buttons += 'onclick="CustomerAction(this.id)"><span class="glyphicon glyphicon-link pull-left"></span>';
            buttons += 'Criminal record</button></li></ul>';

            document.getElementById("actionContent-item").innerHTML = buttons;
       }
    })


    $('#payrollBtn').on('click',function(){

        
        if(_userhasAccesstoModule(userPrivilegdes,'Payroll')){

          buttons = "";

          buttons = '<ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">';
          buttons +='<button class="btn btn-default sub_menu_btn" id="payroll-service"';
          buttons += ' onclick="CustomerAction(this.id)"><span class="glyphicon glyphicon-check pull-left"></span>';
          buttons += 'Payroll Service</button></li><li role="presentation" class="active">';
          buttons += '<button class="btn btn-default sub_menu_btn" id="payroll-list" onclick="CustomerAction(this.id)">';
          buttons += '<span class="glyphicon glyphicon-folder-open pull-left"></span> Payroll list</button></li></ul>';

          document.getElementById("actionContent-item").innerHTML = buttons;
       }

     });

    $('#paymentsBtn').on('click',function(){

        

         if(_userhasAccesstoModule(userPrivilegdes,'Payments')){

              buttons = "";

               document.getElementById("actionContent-item").innerHTML ='';

               buttons = '<ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">';
               buttons += '<button class="btn btn-default sub_menu_btn" id="deduct-payment" onclick="CustomerAction(this.id)">';
               buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Deductions </button></li>';
               buttons += '<li role="presentation" class="active">';
               buttons += '<button class="btn btn-default sub_menu_btn" id="earn-payment" onclick="CustomerAction(this.id)">';
               buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Earnings </button></li>';

               if(_userhasAccesstoModule(userPrivilegdes,'Loans')){

                   buttons += '<li role="presentation" class="active">';
                   buttons += '<button class="btn btn-default sub_menu_btn" id="loan-payment" onclick="CustomerAction(this.id)">';
                   buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Loans </button></li>';
               }

               buttons +='</ul>';

               document.getElementById("actionContent-item").innerHTML = buttons;
         }
  


    });



    $('#settingsBtn').on('click',function(){


        if(_userhasAccesstoModule(userPrivilegdes,'Settings')){

            buttons ='';
      
            document.getElementById("actionContent-item").innerHTML ='';

            buttons = '<ul class="nav nav-pills nav-stacked">';

            if(_userhasAccesstoModule(userPrivilegdes,'Security')){

                buttons += '<li role="presentation" class="active"><button class="btn btn-default sub_menu_btn" id="user-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Users</button></li>';
            }

            if(_userhasAccesstoModule(userPrivilegdes,'Holiday')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="holiday-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Holiday </button></li>';
            }

            if(_userhasAccesstoModule(userPrivilegdes,'Loans')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="loan-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Loans </button></li>';

            }

            if(_userhasAccesstoModule(userPrivilegdes,'Personnel')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="personnel-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Personnel </button></li>';

            }

            if(_userhasAccesstoModule(userPrivilegdes,'Payroll')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="payroll-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Payroll </button></li>';
            }

            if(_userhasAccesstoModule(userPrivilegdes,'Taxation')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="tax-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Taxation </button></li>';

            }
            
            if(_userhasAccesstoModule(userPrivilegdes,'Severance')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="severance-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Severance Pay </button></li>';
            }

            if(_userhasAccesstoModule(userPrivilegdes,'Pension')){

                buttons += '<li role="presentation" class="active">';
                buttons += '<button class="btn btn-default sub_menu_btn" id="pension-settings" onclick="CustomerAction(this.id)">';
                buttons += '<span class="glyphicon glyphicon-briefcase pull-left"></span> Pension </button></li>';
            }

                buttons += '</ul>';


            document.getElementById('actionContent-item').innerHTML = buttons;
        }
    });

    $('#reportsBtn').on('click',function(){

        if(_userhasAccesstoModule(userPrivilegdes,'Reports')){

               buttons = "";
              _glcontroller = "Reports";
              document.getElementById("actionContent-item").innerHTML ='';

              buttons = '<ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">';
              buttons += '<button class="btn btn-default sub_menu_btn" id="search-options" onclick="CustomerAction(this.id)">';
              buttons += '<span class="glyphicon glyphicon-search pull-left"></span>  Search options </button></li>';
              buttons += '</li></ul>';
              buttons += '<div class="panel-group sub_menu_with_accordion" id="accordion">\
            <div class="panel panel-default">\
              <div class="panel-heading">\
                  <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">\
                  <span class="glyphicon glyphicon-file pull-left"></span>Payslip reports</a>\
              </div>\
              <div id="collapse1" class="panel-collapse collapse">\
                <ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="payslip-rpt" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Personnel payslip</button></li>\
                <li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="company-payslip-rpt" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Company payslip</button></li></ul></div>\
            </div>\
            <div class="panel panel-default">\
              <div class="panel-heading">\
                  <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">\
                  <span class="glyphicon glyphicon-file pull-left"></span>Collapsible Group 2</a>\
              </div>\
              <div id="collapse2" class="panel-collapse collapse">\
                <ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="manage-user" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Manage User</button></li>\
                <li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="manage-user" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Manage User</button></li></ul>\
            </div>\
            </div>\
            <div class="panel panel-default">\
              <div class="panel-heading">\
                  <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">\
                  <span class="glyphicon glyphicon-file pull-left"></span>Collapsible Group 3</a>\
              </div>\
              <div id="collapse3" class="panel-collapse collapse">\
                <ul class="nav nav-pills nav-stacked"><li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="manage-user" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Manage User</button></li>\
                <li role="presentation" class="active">\
                <button class="btn btn-default sub_menu_btn" id="manage-user" onclick="CustomerAction(this.id)">\
                <span class="glyphicon glyphicon-briefcase pull-left"></span>Manage User</button></li></ul>\
            </div>\
            </div>\
          </div>';

              document.getElementById("actionContent-item").innerHTML = buttons;
         }

     });


    $('#homeBtn').on('click',function(){

    	document.getElementById("actionContent-item").innerHTML ='';
      window.location.href = '/Home/Index';

    });
}

function createElements(){

 	   var jqxWidget = $('#jqxWidget');

     var offset = jqxWidget.offset();


     $('#payrollBtn').jqxButton({width: 180, height: 40});

     $('#settingsBtn').jqxButton({width: 180, height: 40});

     $('#homeBtn').jqxButton({width: 180, height: 40});

     $('#reportsBtn').jqxButton({width: 180, height: 40});

     $('#paymentsBtn').jqxButton({width: 180, height: 40});

     $('#personnelBtn').jqxButton({width: 180, height: 40});
}

function CustomerAction(id){

    container = document.getElementById('chartContainer');

    switch(id){

        case 'manage-user':
             window.location.href = '/Settings/Index';

        break;
        case 'deduct-payment':
              window.location.href = '/Deductions/Index';
        break;
        case 'earn-payment':
             window.location.href = '/Earning/Index';
        break;
        case 'loan-payment':
             window.location.href = '/Loan/Index';
        break;

        case 'view-records':
             window.location.href = '/Employee/Index';
        break;

        case 'create-record':
             window.location.href = '/Employee/Create';
        break;
        case 'user-settings':
             window.location.href = '/Security/Index';
        break;
        case 'loan-settings':
             window.location.href = '/Settings/LoanSettings';
        break;
        case 'holiday-settings':
             window.location.href ='/Settings/HolidaySettings';
        break;
        case 'tax-settings':
            window.location.href ='/Settings/TaxationSettings';
        break;

        case 'payroll-settings':
            window.location.href ='/Settings/PayrollSettings';
        break;

        case 'personnel-settings':
            window.location.href ='/Settings/PersonnelSettings';
        break;

        case 'payroll-list':
            window.location.href ='/Payroll/Create';

        break;

        case 'payroll-service':
            window.location.href ='/Payroll/Details';
        break;

        case 'payslip-rpt':

          window.location.href = '/Report/Index';   
            
        break;

        case 'search-options':

            console.log(_glcontroller);
            document.getElementById("searchpopupWindow").style.visibility = "visible";
           _searchOptionsWindow();

        break;

        case 'severance-settings':
           window.location.href = '/Settings/SeveranceSettings';
        break;

        case 'pension-settings':
            window.location.href ='/Pension/Create';
        break;

    }
}

$(document).ready(function(){

      _getUserAccessRights();

      addEventListeners();

      createElements();

      $("#jqxWidget").css('visibility', 'visible');

      $("#searchWidget").css('visibility','hidden');
      
});



