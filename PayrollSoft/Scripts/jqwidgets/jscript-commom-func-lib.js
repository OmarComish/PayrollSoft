//LIBRARY script for common miscellaneous functions
//The intention is to implement code reuse
//Date Created: 26-FEB-2014
//Last Date Changed: 10-MAR-2018


///<summary>
/// this object keeps count of the number of times this method is fired.
/// In this case, it is used to as a event counter.
///</summary>

var __eventCounter = (function(){

    var counter = 0;

    function modifyBy(val){
       counter += val;
    }

    return {
      increment: function(){
         modifyBy(1);
      },
      decrement: function(){
         modifyBy(-1);
      },
      value: function(){
         return counter;
      }
    };

})();

///<summary>
/// __currencyFormatter function. 
/// PARAMTERS: currency. 
/// OUTPUT: outputs the digit in a specified currency
///</summary>
var  __currencyFormatter =  new Intl.NumberFormat('en-US', {
                          style: 'currency',
                          currency: 'MWK',
                          minimumFractionDigits: 2,
                          // the default value for minimumFractionDigits depends on the currency
                          // and is usually already 2
                        });

var __genericFormat = new Intl.NumberFormat('en-US',{ maximumSignificantDigits: 3 });

///<summary>
/// clears an array data object of its elements
///</summary>
function __resetArray(arr){

  for(var x = arr.length; x > 0; x-- ){
      arr.pop();
  }
  console.log(arr.length);
}

function _generateReport(__empId){
       
        ///<summary>
        /// Method: accepts 1 argument, __empId 
        ///</summary>
        

        payslipreportTotalEarnings = 0;   ///reinitialize thses variable to 0 to avoid cumulative addition of the totals after each report window is closed
        payslipreportTotalDeductions = 0;

        var demographics = findElementById(payslipreportDemographicsDataSource, __empId);
        //console.log(demographics);
        
        //var title = document.title;
        var title = 'Pay check report';

        var divElements = '<div style="width: 950px;';
        divElements += 'box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);';
        divElements += 'text-align: center;';
        divElements += 'font-family: Helvetica, Arial, sans-serif; font-size: 3px;">';
        divElements += '<table width="950" style="border: 1px solid #ddd; font-family: sans-serif;">';
        divElements += '<tr style="font-size: 12px; font-weight: bold;">';
        divElements += '<td>Spirals Software Foundation Inc ©</td><td colspan="1">' + demographics.name.toUpperCase() +'</td>';
        divElements += '<td> Employment Number: ' + demographics.id.toUpperCase() + '</td><td colspan="1">2018/01/01 to 2018/01/31</td></tr>';
        

        ///<summary>
        /// Earnings and Deductions section
        ///</summary>

        divElements += '<tr><td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 1px solid #ddd; font-size: 12px; font-weight: normal;">';
        divElements += '<tr><td>Remunerations</td><td>Amount</td></tr>';

        for (var i = 0; i < payslipreportEarningsDataSource.length; i++) {

          if(payslipreportEarningsDataSource[i].id === __empId){

            divElements += '<tr><td>' + payslipreportEarningsDataSource[i].type +'</td><td>' + payslipreportEarningsDataSource[i].amount + '</td></tr>';
            payslipreportTotalEarnings += payslipreportEarningsDataSource[i].amount;
          }
          
        }

        //divElements += _populateEarnings();
        
        divElements += '</table>';
        divElements += '</td><td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 1px solid #ddd; font-size: 12px; font-weight: normal;">';
        divElements += '<tr><td>Deductions</td><td>Amount</td></tr>';

        for (var i = 0; i < payslipreportDeductionsDataSource.length; i++) {

          if(payslipreportDeductionsDataSource[i].id === __empId){

             divElements += '<tr><td>' + payslipreportDeductionsDataSource[i].type +'</td><td>' + payslipreportDeductionsDataSource[i].amount + '</td></tr>';
             payslipreportTotalDeductions += payslipreportDeductionsDataSource[i].amount;
             payslipreportNetPay = payslipreportTotalEarnings - payslipreportTotalDeductions;
          }

        }

        //divElements += _populateDeductions();

        divElements += '</table>';
        divElements += '</td></tr>';

        ///<summary>
        ///subtotal section
        ///</summary>
        divElements += '<tr><td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 1px solid #ddd; font-size: 12px; font-weight: normal;">';
        divElements += '<tr><td>TOTAL Remuneration(s)</td><td>'+ __currencyFormatter.format(payslipreportTotalEarnings) +'</td></tr>';
        divElements += '</table></td>';
        divElements += '<td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 1px solid #ddd; font-size: 12px; font-weight: normal;">';
        divElements += '<tr><td>TOTAL Deduction(s)</td><td>'+ __currencyFormatter.format(payslipreportTotalDeductions) +'</td></tr>';
        divElements += '</table></td>';
        divElements += '</td></tr>';

        ///<summary>
        ///Leave holiday section
        ///</summary>

        divElements += '<tr><td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 1px solid #ddd; font-size: 10px; font-weight: normal;">';
        divElements += '<tr><td>Total leave days:</td><td>25</td></tr>';
        divElements += '<tr><td>Days taken:</td><td>20</td></tr>';
        divElements += '<tr><td>Days remaining:</td><td>5</td></tr>';
        divElements += '</table></td>';
        divElements += '<td colspan="2">';
        divElements += '<table width="470" style="font-family: sans-serif; border: 0px solid #ddd; font-size: 12px; font-weight: bold;">';
        divElements += '<tr><td>NET pay:</td><td>' + __currencyFormatter.format(payslipreportNetPay) + '</td></tr>';
        divElements += '</table></td>';
        divElements += '</td></tr>';

        divElements += '</td></tr>';
        divElements += '</table>';
        divElements += '</div>';

        var printWindow = window.open("", title, "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=970,height=800,top="+(screen.height-400)+",left="+(screen.width-840));
        printWindow.document.open();

        printWindow.document.write('<html><head><title>' + title + '</title><link rel="stylesheet" type="text/css" href="<link href="/Content/css/bootstrap.css"></head><body>');
        printWindow.document.write(divElements);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.focus();  
        //The Timeout is ONLY to make Safari work, but it still works with FF, IE & Chrome.
        /*setTimeout(function() {
            printWindow.print();
            printWindow.close();
        }, 100);*/
}

function _parseJsonDate(__date){

   var fulldate = new Date(parseInt(__date.substr(6)));
   var twodigitMonth = (fulldate.getMonth() + 1) + ""; if(twodigitMonth.length === 1) twodigitMonth = "0" + twodigitMonth;
   
   var twodigitDate = fulldate.getDate() + ""; if(twodigitMonth.length === 1) twodigitMonth = "0" + twodigitDate;
   //var currentDate = twodigitDate + "/" + twodigitMonth + "/" + fulldate.getFullYear();
   var currentDate = fulldate.getFullYear() + "/" + twodigitMonth + "/" +  twodigitDate ;
   return currentDate;
}

function AutogenerateCode(){

     var generatedNum = 0;
     var array = new Uint32Array(1);
     window.crypto.getRandomValues(array);
     //console.log("Generated numbers");
        for(var x = 0; x < array.length; x++){
                //console.log(array[x]);
                generatedNum = array[x];
        }

        return generatedNum;

}

function CurrentDate() {

    var today = new Date();
    var dd = today.getDate() + ""; if (dd.length === 1) dd = "0" + dd;
    
    var mm = (today.getMonth() + 1) + ""; if(mm.length === 1) mm = "0" + mm;
    var yyyy = today.getFullYear();
    var curDate = yyyy + "/" + mm + "/" + dd;

    return curDate;
}


function _checkRecord(arg) {

    var recordexists = false;

    $.ajax({
        cache: false,
        datatype: 'jsonp',
        url: "/Employee/RecordExists/" + arg,
        type: 'GET',
        success: function (data, status, xhr) {
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    //recordexists = true;
                    console.log("Record Already Exists");
                }
                else {
                    console.log("Record Already Exists");
                }
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            recordexists = false;
        }

    });

    //console.log("Call to function  _testFunc() succeeded:", arg.trim());
}

//FUNCTION: _checkIfRecordExists
//PARAMS: id (the key column),controller (name of the controller to invoke),action(the method to call in the controller)
//RETURNS: boolean value, TRUE if the record exists and FALSE the other way round
function _checkIfRecordExists(id, column, tbl) {

    var recordexists = false;
    //proceed with an ajax call to the server
    $.ajax({
        cache: false,
        datatype: 'jsonp',
        url: "/Employee/RecordExists/" + id + column + tbl,
        type: 'GET',
        success: function (data, status, xhr) {

            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    //record already exist
                    recordexists = true;
                    console.log("Record Already Exists");

                } else {

                    recordexists = false;
                    console.log("No record Found");
                }

            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
            recordexists = false;
        }

    });




    //return recordexists;

}

//FUNCTION: _verifyDataBeforePosting
//PARAMS: data is json object with values for posting to the controller. This methods verifies the presence of data before posting is done
//RETURNS: void. It simply prints out the values to the console if present
function _verifyDataBeforePosting(data){

   for(var key in data){

       if(data.hasOwnProperty(key)){
           console.log("key: " + key + " "  + " Value: "+ data[key]);
      }else {
          console.log("the json object is empty");
      }
  }

}

function __searchParam(id) {

    //console.log(id + " " + controller + " " + action);
    var __returnval=null;
    $.ajax({

        cache: false,
        datatype: "jsonp",
        url: "/Loan/SearchParam/" + id,
        type: "GET",
        success: function (data, status, xhr) {

            _verifyDataBeforePosting(data[0]);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
           
        }


    });

    //console.log($("#deptno").val());

}
//FUNCTION: clicksCounter
//PARAMS: none
//RETURNS: integer ,the total count a particular control has been clicked. E.g the save command

function clicksCounter() {
    var count = count + 1;
    return count;
}

//FUNCTION: _addNewRecord
//PARAMS: record(the data to save in the database),controller(the controller function to call)
//RETURNS: void

function _addNewRecord(record, controller) {

    switch (controller) {

        case 'Allowance':

            $.ajax({
                cache: false,
                datatype: "jsonp",
                url: "/Allowance/Create",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    alert(status);
                    //$("#jqxgrid").jqxGrid('updatebounddata');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                    //commit(false);
                }

            });

            break;

        case 'Pension':

            $.ajax({
                cache: false,
                datatype: "jsonp",
                url: "/Pension/Create",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                   _notificationMessage(data.status, data.message);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                   
                }

            });

            break;

        case 'Taxation':

            $.ajax({
                cache: false,
                datatype: "jsonp",
                url: "/Taxation/Edit",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                  
                    //_notificationMessage(data.status, data.message); 
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    _notificationMessage('error', errorThrown); 
                }

            });

            break;

        case 'EmployeeGrade':

           /* $.ajax({

                cache: false,
                datatype: "json",
                url: "/EmployeeGrade/Create",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    //alert(status);
                    _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(textStatus);
                    _notificationMessage("error",errorThrown);
                }
            });
            */

            console.log("Successfully triggered the _addNewRecord() method");
            break;

        case 'Payroll':

            $.ajax({

                cache: false,
                datatype: "json",
                url: "/Payroll/Create",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    
                    //_notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(textStatus);
                    _notificationMessage("error",errorThrown); //Notify the user
                }
            });

            break;

        case 'Employee':

           _verifyDataBeforePosting(record);
           

              $.ajax({
                  cache: false,
                  datatype: "json",
                  url: "/Employee/Create",
                  type: "POST",
                  data: record,
                  success: function (data, status, xhr) {

                      _notificationMessage(data.status, data.message); 
                  },
                  error: function (jqXHR, textStatus, errorThrown) {

                      _notificationMessage("error", errorThrown); 
                  }
             });
             
            break;

        case 'Settings':

            if(_glfunctionInvoked == "prioritycodeSave"){

                 console.log(_glfunctionInvoked);
                 console.log(record.PriorityCode + "" + record.Description);

                 $.ajax({
                    cache: false,
                    datatype: "json",
                    url: "/PriorityCode/Create",
                    type: "POST",
                    data: record,
                    success: function (data, status, xhr) {
                         
                            _notificationMessage(data.Status, data.Message);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                         
                        _notificationMessage("error", errorThrown);
                    }
                 });

            } else if (_glfunctionInvoked === "prioritycodeEdit"){

                 /// call the edit action in the controller
                 //_verifyDataBeforePosting(record);
                
                 $.ajax({
                    cache: false,
                    datatype: "json",
                    url: "/PriorityCode/Update",
                    type: "POST",
                    data: record,
                    success: function (data, status, xhr) {
                         
                           // _notificationMessage(data.Status, data.Message);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                         
                        _notificationMessage("error", errorThrown);
                    }
                 });
                 
            } else if (_glfunctionInvoked === "prioritycodeDelete"){

              $.ajax({
                    cache: false,
                    datatype: "json",
                    url: "/PriorityCode/DeletePriorityCode",
                    type: "POST",
                    data: record,
                    success: function (data, status, xhr) {
                         
                           // _notificationMessage(data.Status, data.Message);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                         
                        _notificationMessage("error", errorThrown);
                    }
                 });

            } else if(_glfunctionInvoked=="saveDeductionBtn"){

                console.log("saveDeduction process initialized...");

               // _verifyDataBeforePosting(record);

                var paymentcode = {payCode: record.payCode,Description: record.payCodeDescription};
                var paymenttype = {payType: record.payType,Description: record.payTypeDescription};
                 
                      $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Deductions/Create",
                          type: "POST",
                          data: record,
                         success: function (data, status, xhr) {
                            // alert(status);
                           _notificationMessage(data.Status, data.Message); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                             _notificationMessage("error", errorThrown); //Notify the user
                          }
                      });

                      $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Payments/CreatePaymentCode",
                          type: "POST",
                          data: paymentcode,
                         success: function (data, status, xhr) {
                            // alert(status);
                           //_notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                            // _notificationMessage("error", errorThrown); //Notify the user
                          }
                      });

                     $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Payments/CreatePaymentType",
                          type: "POST",
                          data: paymenttype,
                         success: function (data, status, xhr) {
                            // alert(status);
                           //_notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                            // _notificationMessage("error", errorThrown); //Notify the user
                          }
                      });
                
                 console.log("saveDeduction process completed...");

            } else if(_glfunctionInvoked=="editDeductionsBtn"){

                    $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Deductions/EditDeductionPayment",
                          type: "POST",
                          data: record,
                          success: function (data, status, xhr) {
                               
                              _notificationMessage(data.Status, data.Message);
                          },
                          error: function (jqXHR, textStatus, errorThrown) {
                               
                              _notificationMessage("error", errorThrown);
                          }
                       });

            } else if(_glfunctionInvoked=="saveEarningsBtn"){

               console.log("save earnings process initialized...");

               var paymentcode = {payCode: record.payCode,Description: record.payCodeDescription};
               var paymenttype = {payType: record.payType,Description: record.payTypeDescription};

                    $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Earning/Create",
                          type: "POST",
                          data: record,
                         success: function (data, status, xhr) {
                            
                           _notificationMessage(data.Status, data.Message); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               
                             _notificationMessage("error", errorThrown); //Notify the user
                          }
                      });

                //console.log("save earnings process initialized...");
                //console.log("verifying the contents of paymenttype variable...");

                //console.log(paymenttype.paymentType);
                //console.log(paymenttype.Description);

                     $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Payments/CreatePaymentCode",
                          type: "POST",
                          data: paymentcode,
                         success: function (data, status, xhr) {
                            // alert(status);
                           //_notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                            // _notificationMessage("error", errorThrown); //Notify the user
                          }
                      });


                     $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Payments/CreatePaymentType",
                          type: "POST",
                          data: paymenttype,
                         success: function (data, status, xhr) {
                            // alert(status);
                           //_notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                            // _notificationMessage("error", errorThrown); //Notify the user
                         }
                      });

            } else if (_glfunctionInvoked === "editEarningsBtn"){

                      $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Earning/EditEarningPayment",
                          type: "POST",
                          data: record,
                          success: function (data, status, xhr) {
                               
                              _notificationMessage(data.Status, data.Message);
                          },
                          error: function (jqXHR, textStatus, errorThrown) {
                               
                              _notificationMessage("error", errorThrown);
                          }
                       });

            } else if(_glfunctionInvoked === "timesheetSaveBtn"){

                 console.log("timesheet save action invoked...");

                  $.ajax({
                    cache: false,
                    datatype: "json",
                    url: "/Settings/attendancelog",
                    type: "POST",
                    data: record,
                    success: function (data, status, xhr) {
                        // alert(status);
                        _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                         //alert(textStatus);
                        _notificationMessage("error", errorThrown); //Notify the user
                    }
                  });

            } else if(_glfunctionInvoked === "overtimeSaveBtn"){

                   console.log("overtime save function invoked...");
                   
                    $.ajax({
                      cache: false,
                      datatype: "json",
                      url: "/Settings/CreateOvertimePaymentSetting",
                      type: "POST",
                      data: record,
                      success: function (data, status, xhr) {
                          
                          _notificationMessage(data.status, data.message); //Notify the user
                      },
                      error: function (jqXHR, textStatus, errorThrown) {
  
                          _notificationMessage("error", errorThrown); //Notify the user
                      }

                    });

            } else if(_glfunctionInvoked === "saveLoanSettingsBtn"){

                console.log("save loan settings action triggered");

                $.ajax({
                    cache: false,
                    datatype: "json",
                    url: "/Settings/LoanType",
                    type: "POST",
                    data: record,
                    success: function (data, status, xhr) {
                        // alert(status);
                        _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                         //alert(textStatus);
                        _notificationMessage("error", errorThrown); //Notify the user
                    }

                });

            } else if(_glfunctionInvoked === "saveLoanBtn"){

                    console.log("save loan detail action triggered");

                     $.ajax({
                        cache: false,
                        datatype: "json",
                        url: "/Settings/LoanDetails",
                        type: "POST",
                        data: record,
                        success: function (data, status, xhr) {
                            
                            _notificationMessage(data.status, data.message); 
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                             
                            _notificationMessage(textStatus, errorThrown); 
                        }

                    });

            } else if (_glfunctionInvoked == "saveJobPositionBtn"){

              console.log("save Job position action triggered...");
              console.log(record.Name);

                    $.ajax({
                            cache: false,
                            datatype: "json",
                            url: "/Settings/saveJobPosition",
                            type: "POST",
                            data: record,
                            success: function (data, status, xhr) {
                                // alert(status);
                                _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                 //alert(textStatus);
                                _notificationMessage("error", errorThrown); //Notify the user
                            }
                        });

            } else if(_glfunctionInvoked == "saveJobGradeBtn"){

               console.log("Saving the job grade now...");

                      $.ajax({
                            cache: false,
                            datatype: "json",
                            url: "/Settings/saveJobGrade",
                            type: "POST",
                            data: record,
                            success: function (data, status, xhr) {
                                // alert(status);
                                _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                 //alert(textStatus);
                                _notificationMessage("error", errorThrown); //Notify the user
                            }
                        });

            } else if(_glfunctionInvoked === "saveSettingsBtn") {

                 
                //_verifyDataBeforePosting(record);

                      $.ajax({
                            cache: false,
                            datatype: "json",
                            url: "/Settings/SavePersonnelSettings",
                            type: "POST",
                            data: record,
                            success: function (data, status, xhr) {
                                // alert(status);
                                _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                 //alert(textStatus);
                                _notificationMessage("error", errorThrown); //Notify the user
                            }
                        });

            } else if(_glfunctionInvoked === "saveSeveranceSettingsBtn"){

                 //_verifyDataBeforePosting(record[0]);
                var _severanceRec = record[0];

                $.ajax({
                        cache: false,
                        datatype: "json",
                        url: "/Settings/UpdateSeveranceSettings",
                        type: "POST",
                        data: record[0],
                        success: function (data, status, xhr) {
                            
                            _notificationMessage(data.status, data.message); 
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                             
                            _notificationMessage("error", errorThrown); 
                        }
                    });

                } else {

                          $.ajax({
                            cache: false,
                            datatype: "json",
                            url: "/Settings/Create",
                            type: "POST",
                            data: record,
                            success: function (data, status, xhr) {
                                // alert(status);
                                _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                 //alert(textStatus);
                                _notificationMessage("error", errorThrown); //Notify the user
                            }
                         });
              }

               break;

        case 'Security':

         if(_glfunctionInvoked === "grantRoleBtn"){

               console.log("saving the user roles... ");

               $.ajax({
                   cache: false,
                   datatype: "jsonp",
                   url: "/Security/CreateUserRole",
                   type: "POST",
                   data: record,
                   success: function (data, status, xhr) {

                      _notificationMessage(data.Status, data.Message);

                   },
                   error: function (jqXHR, textStatus, errorThrown) {

                       _notificationMessage("error", errorThrown);
                   }
                });

         } else if (_glfunctionInvoked === "userSaveBtn") {

              console.log("Saving security data...");

              $.ajax({
                  cache: false,
                  datatype: "jsonp",
                  url: "/Settings/CreateUser",
                  type: "POST",
                  data: record,
                  success: function (data, status, xhr) {

                     _notificationMessage("success", "Record Saved Successfully!");

                  },
                  error: function (jqXHR, textStatus, errorThrown) {

                      _notificationMessage("error", errorThrown);
                  }
               });

         } else if(_glfunctionInvoked === "editRoleBtn") {

             console.log("Applying changes to the user role...");
            
              $.ajax({
                   cache: false,
                   datatype: "jsonp",
                   url: "/Security/UpdateUserRole",
                   type: "POST",
                   data: record,
                   success: function (data, status, xhr) {

                      _notificationMessage(data.Status, data.Message);

                   },
                   error: function (jqXHR, textStatus, errorThrown) {

                       _notificationMessage("error", errorThrown);
                   }
                });
         }

           break;

         case 'Payments':

                console.log("Saving payment data to database...");

                    $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/Payments/Create",
                          type: "POST",
                          data: record,
                         success: function (data, status, xhr) {
                            // alert(status);
                           // _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                           console.log("payment data saved to database successfully!");
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                             _notificationMessage("error", errorThrown); //Notify the user
                          }
                       });

            break;

         case 'PaymentType':

                console.log("Saving payment data to database...");

                    $.ajax({
                          cache: false,
                          datatype: "json",
                          url: "/PaymentType/Create",
                          type: "POST",
                          data: record,
                         success: function (data, status, xhr) {
                            // alert(status);
                           // _notificationMessage("success", "Record Saved Successfully!"); //Notify the user
                           console.log("payment type data saved successfully!");
                         },
                          error: function (jqXHR, textStatus, errorThrown) {
                               //alert(textStatus);
                             _notificationMessage("error", errorThrown); //Notify the user
                          }
                       });

            break;
    }
}

function _editRecord(id, record, controller) {

    switch (controller) {

        case 'Allowance':
            $.ajax({
                cache: false,
                datatype: "jsonp",
                url: "/Allowance/Edit/" + id,
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    //alert(status);
                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                    //commit(false);
                }

            });

            break;

        case 'Payroll':
            $.ajax({
                cache: false,
                datatype: "jsonp",
                url: "/Payroll/Update",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    //alert(status);
                    //$("#jqxgrid").jqxGrid('updatebounddata');
                    _notificationMessage("success", "Edit completed successfully!");
                    $("#jqxgrid").jqxGrid('updatebounddata');
                    console.log(status);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    //commit(false);
                }

            });

            break;

        case 'EmployeeGrade':

            //after debugging, this is the only Edit Ajax Code thats working so far
            // its calling the UPDATE action in the EmployeeGrade Controller.
            //Copy this code and use it elsewhere it may be needed in this project
            //Date: 01/01/2015
            //Time: 22:25

            $.ajax({

                cache: false,
                datatype: "jsonp",
                url: "/EmployeeGrade/Update",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                    
                    _notificationMessage("success", "Edit completed successfully!");

                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                    //commit(false);
                }

            });

            break;

        case 'Employee':

           _verifyDataBeforePosting(record);
     
             $.ajax({

                cache: false,
                datatype: "jsonp",
                url: "/Employee/Update",
                type: "POST",
                data: record,
                success: function (data, status, xhr) {
                 
                  _notificationMessage(data.status, data.message);

                  $("#jqxgrid").jqxGrid('updatebounddata');
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    _notificationMessage("error", errorThrown);
                   
                }

              });
             break;

        case 'Loan':
            //logic  goes here
             break;

    }
}


//Notification Method...
//Date: 21-MAR-2015
//PARAMS: _msgetype = Specifies the type of message dialog box to show. E.g success, error, info, or warning
//        _msgetxt = the specific text to show when the dialog box loads

function _notificationMessage(_msgetype,_msgetxt) {


    $("#messageNotification").jqxNotification({
        width: 280,
        position: "bottom-right",
        opacity: 1.2,
        autoOpen: false,
        animationOpenDelay: 800,
        autoClose: true,
        autoCloseDelay: 3200,
        template: _msgetype
    });

    $("#messageNotification").html(_msgetxt);
    $("#messageNotification").jqxNotification("open");
}

function _deleterecordDialog(_msgetext){

    $("#deleteRecDialog").jqxWindow({
            theme:        theme,
            resizable:    false,
            width:        260,
            isModal:      true,
            autoOpen:     false,
            title:        'Delete',
            okButton:     $("#DeleteDialogBtn"),
            cancelButton: $("#CancelDialogBtn"),
            modalOpacity: 0.1,
            initContent: function(){

               $("#DeleteDialogBtn").jqxButton({theme: theme});
               $("#CancelDialogBtn").jqxButton({theme: theme});

            }

      });

   $("#deleteRecDialog").jqxWindow('open');

}

function _confirmActionDialog(__text){

    $("#confirmActionDialog").jqxWindow({
            theme:        theme,
            resizable:    false,
            width:        450,
            height:       100,
            isModal:      true,
            autoOpen:     false,
            title:        'Confirm action',
            okButton:     $("#proceedBtn"),
            cancelButton: $("#cancelBtn"),
            modalOpacity: 0.08,
            initContent: function(){
               document.getElementById('dialogmessage').innerHTML = __text;
               $("#proceedBtn").jqxButton({theme: theme});
               $("#cancelBtn").jqxButton({theme: theme});

            }
    });

    $("#confirmActionDialog").jqxWindow('open');
}

function _setTrue(variablename){

    switch(variablename){
        case "_glsearchByPersonnelName":
            _glsearchByPersonnelName = true;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
            break;

        case "_glsearchByPersonnelId":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = true;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
           break;
       
        case "_glsearchByGradeId":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = true;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
            break;
        
        case "_glsearchByDesignation":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = true;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
            break;
       
        case "_glsearchByEmploymentStartDate":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = true;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
            break;
         
         case "_glsearchByEmploymentStatus":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = true;
            _glsearchByGender = false;
            _glsearchByDepartment = false;
            break;
         
        case "_glsearchByGender":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = true;
            _glsearchByDepartment = false;
            break;
     
         case "_glsearchByDepartment":
            _glsearchByPersonnelName = false;
            _glsearchByPersonnelId = false;
            _glsearchByGradeId = false;
            _glsearchByDesignation = false;
            _glsearchByEmploymentStartDate = false;
            _glsearchByEmploymentStatus = false;
            _glsearchByGender = false;
            _glsearchByDepartment = true;
            break;

    }
} 

function _searchpayslipReportWindow(){

     var records = __payslipReportListDataAdapter();
 
      $('#searchpayslipreportWindow').jqxWindow({
                                    theme: theme,
                                    resizable: true,
                                    width: 650,
                                    height: 280,
                                    isModal: true,
                                    autoOpen: false,
                                    modalOpacity: 0.01,
                                    initContent: function(){

                                          $("#jqxgrid").jqxGrid({
                                                        width: 640,
                                                        source: records,
                                                        theme: theme,
                                                        pageable: true,
                                                        autoheight: true,
                                                        sortable: true,
                                                        selectionmode: 'singlerow',
                                                        columns: [
                                                                  { text: 'Name', datafield: 'EmpName', width: 200 },
                                                                  { text: 'Emp ID', datafield: 'EmpID', width: 70 },
                                                                  { text: 'Department', datafield: 'DeptName', width: 170 },
                                                                  { text: 'Period', datafield: 'Payroll period', width: 130 },
                                                                  { text: 'Action', datafield: 'View',columntype:'button',cellsrenderer: function(){
                                                                              
                                                                              return 'View';

                                                                  }, buttonclick: function(row){

                                                                            var viewrow = row;

                                                                            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', viewrow);

                                                                      },
                                                                  }

                                                               ]

                                                       });
                                                                    
                                    }
        });

       $('#searchpayslipreportWindow').jqxWindow('open');
}      

function _searchOptionsWindow(){

  $("#searchpopupWindow").jqxWindow({
                        theme: theme,
                        resizable: false,
                        width: 600,
                        height: 380,
                        isModal: false,
                        autoOpen: false,
                        //title: title,
                        cancelButton: $("#searchpopupCloseBtn"),
                        modalOpacity: 0.01,
                        initContent: function(){

                                  //$("#startdate").jqxDateTimeInput({ theme: theme, width: '200px', height: '20px', formatString: 'yyyy-MM-dd' });
                                  $("#personnelName").jqxInput({theme: theme, height: '25px',width: '300px'});
                                  $("#personnelId").jqxInput({theme: theme, height: '25px',width: '300px'});
                                  $("#personnelGrade").jqxInput({theme: theme, height: '25px',width: '300px', disabled: true});
                                  $("#personnelDesignation").jqxInput({theme: theme, height: '25px',width: '300px', disabled: true});
                                  $("#personnelHireDate").jqxDateTimeInput({ theme: theme, width: '300px', height: '25px', formatString: 'yyyy-MM-dd',disabled: true });
                                  $("#personnelStatus").jqxInput({theme: theme, height: '25px',width: '300px', disabled: true});
                                  $("#personnelGender").jqxInput({theme: theme, height: '25px',width: '300px', disabled: true});
                                  $("#personnelDepartment").jqxInput({theme: theme, height: '25px',width: '300px', disabled: true});

                                  $("#searchpopupCloseBtn").jqxButton({theme: theme});
                                  $("#searchbynameRadioBtn").jqxCheckBox({ width: 250, height: 25, checked: true, theme: theme });
                                  $("#searchbygradeRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbydesignationRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbydatehiredRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbystatusRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbypersonnelIdRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbygenderRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });
                                  $("#searchbydepartmentRadioBtn").jqxCheckBox({ width: 250, height: 25,checked: false, theme: theme });

                                   $("#searchbynameRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                             _setTrue("_glsearchByPersonnelName");
                                             $("#personnelName").jqxInput({disabled: false});
                                
                                          } else{
                                            $("#personnelName").jqxInput({disabled: true});
                                          }
                                   });

                                   $("#searchbypersonnelIdRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){

                                             $("#searchbynameRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbygradeRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbydesignationRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbystatusRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbydepartmentRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbygenderRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbydatehiredRadioBtn").jqxCheckBox({checked: false});
                                             $("#searchbygenderRadioBtn").jqxCheckBox({checked: false});
                                             $("#personnelId").jqxInput({disabled: false});
                                             

                                            _setTrue("_glsearchByPersonnelId");
                                
                                          } else {
                                             $("#personnelId").jqxInput({disabled: true});
                                          }
                                   });

                                   $("#searchbygradeRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                            $('#personnelGrade').jqxInput({disabled: false});
                                            _setTrue("_glsearchByGradeId");
                                
                                          } else{
                                            $('#personnelGrade').jqxInput({disabled: true});

                                          }
                                   });

                                   $("#searchbydesignationRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                             $("#personnelDesignation").jqxInput({disabled: false});
                                             _setTrue("_glsearchByDesignation");
                                
                                          } else {
                                             $("#personnelDesignation").jqxInput({disabled: true});
                                          }
                                   });

                                   $("#searchbystatusRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                            $('#personnelStatus').jqxInput({disabled: false});
                                            _setTrue("_glsearchByEmploymentStatus");
                                
                                          } else {
                                            $('#personnelStatus').jqxInput({disabled: true});
                                          }
                                   });

                                   $("#searchbydatehiredRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){

                                              $("#personnelHireDate").jqxDateTimeInput({disabled: false});
                                            _setTrue("_glsearchByEmploymentStartDate");
                                
                                          } else {

                                             $("#personnelHireDate").jqxDateTimeInput({disabled: true});
                                          }
                                   });

                                   $("#searchbygenderRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                             $("#personnelGender").jqxInput({disabled: false});
                                            _setTrue("_glsearchByGender");
                                
                                          } else {
                                            $("#personnelGender").jqxInput({disabled: true});
                                          }
                                   });

                                   $("#searchbydepartmentRadioBtn").on('change',function(event){

                                          var checked = event.args.checked;

                                          if(checked){
                                            $('#personnelDepartment').jqxInput({disabled: false});
                                           _setTrue("_glsearchByDepartment");
                                
                                          } else {
                                            $('#personnelDepartment').jqxInput({disabled: true});
                                          }
                                   });

                                   $("#personnelId").on('keyup',function(event){
                                        if(event){
                                            _glpersonnelId = document.getElementById('personnelId').value;
                                        }
                                   });

                                   $("#searchpopupCloseBtn").click(function(){
                                       //parent.document.getElementById('searchval').innerHTML = $("#personnelId").val();
                                       console.log($("#personnelId").val()); 
                                   });
                                    
                        }         

                });

       $("#searchpopupWindow").jqxWindow('open');
}

function _searchByQueryParams(){

  if(_glsearchByPersonnelName){

     console.log("search by personnel name fired");

  } else if(_glsearchByPersonnelId){


  } else if (_glsearchByGradeId ){


  } else if (_glsearchByDesignation){


  } else if (_glsearchByEmploymentStartDate){


  } else if (_glsearchByEmploymentStatus){
    

  } else {

  }
}

//CREATE NEW RECORD method...
//DATE Created: 23-MAR-2015
//Last Changed Date: 24-MAR-2015:01:56
//PARAMS: onPayrollcheck [Boolean variable onPayrollcheck ,that checks whether the on Payroll option is selected or not on the UI]
//RETURNS: void
function _createNewEmployee(onPayrollcheck) {

    var rowdata = {
        EmpID: $("#empID").val(),
        EmpName: $("#name").val(),
        DeptID: $("#deptno").val(),
        GradeID: $("#empgrade").val(),
        HireDate: $("#dateHired").val(),
        DateOfBirth: $("#dob").val(),
        JobTitle: $("#jobposition").val(),
        Gender: $("#sex").val(),
        MobileNumber: $("#contactNo").val(),
        EmploymentStatus: "Active",
        onPayroll: $("#onPayroll").val()
    };
     //console.log(rowdata.DeptID);
     _addNewRecord(rowdata, "Employee");

    if (onPayrollcheck) {

        //if onPayroll checkbox is true
        //insert record in the PayrollList table for this Employee
        //with Dummy values for all the fields except the EmpId

        var PayrollCode = "PAYE" + $("#empID").val(); //Customizable code for generation of Payroll Code
        var insuranceBill = $("#MedSchemContribAmount").val();
        var salaryAdvanceDeduction = $("#advanceLoan").val();
        var overTimeHrs = $("#overtimeHours").val();
        var controller = "Payroll";
        var row = {
            EmpID: $("#empID").val(),
            PayrollCode: PayrollCode,
            insuranceBill: insuranceBill,
            salaryAdvanceDeduction: salaryAdvanceDeduction,
            overtimeHours: overTimeHrs
        };

      _addNewRecord(row,"Payroll");
    }

}


//this function is for code test purposes. Modify it however you wish to suit your needs
//
function _foo(controller){
    console.log("Inside foo function:" + controller);
}

//DELETE RECORD method...
//DATE Created: 18-SEP-2015
//Last Changed Date: 24-MAR-2015:01:56
//PARAMS: controller[name of the controller which is invoking the delete action] and Id[that identifies the record to delete]
//RETURNS: void
function _deleteRecord(id,controller){

      switch(controller){

      case 'Employee':
          //console.log("delete action successfully invoked from" + controller);
          $.ajax({
             cache: false,
             datatype: "jsonp",
             url: "/Employee/Delete/" + id,
             type: "POST",
             success: function (data, status, xhr) {
                 
                 _notificationMessage(data.status, data.message); //Notify the user
                 $("#jqxgrid").jqxGrid('updatebounddata');
             },
             error: function (jqXHR, textStatus, errorThrown) {

                _notificationMessage(textStatus, errorThrown);
                 
             }

         });

         break;

      case 'Earnings':

              $.ajax({
                 cache: false,
                 datatype: "jsonp",
                 url: "/Earning/DeleteEarningPayment/" + id,
                 type: "POST",
                 success: function (data, status, xhr) {

                      $('#jqx-roles-grid').jqxGrid('updatebounddata');
                     //_notificationMessage(data.Status, data.Message); 
                 },
                 error: function (jqXHR, textStatus, errorThrown) {

                      _notificationMessage(textStatus, errorThrown);
                     
                 }
                });

         break;

      case 'Deductions':

              $.ajax({
                 cache: false,
                 datatype: "jsonp",
                 url: "/Deductions/DeleteDeductionPayment/" + id,
                 type: "POST",
                 success: function (data, status, xhr) {
                  
                     _notificationMessage(data.Status, data.Message); //Notify the user
                 },
                 error: function (jqXHR, textStatus, errorThrown) {

                      _notificationMessage(textStatus, errorThrown);
                     
                 }
                });

           break;

      case 'Payroll':
              console.log("delete action invoked from" + controller);
         break;

      case 'Security':

            $.ajax({
                 cache: false,
                 datatype: "jsonp",
                 url: "/Security/RevokeRoles/",
                 type: "POST",
                 data: id,
                 success: function (data, status, xhr) {
                     
                     _notificationMessage(data.Status, data.Message); //Notify the user
                 },
                 error: function (jqXHR, textStatus, errorThrown) {

                      _notificationMessage(textStatus, errorThrown);
                     
                 }
               });

         break;
   }
}

///<SUMMARY>
/// __authorized evaluates the state of the role for each module before any actions is done. It returns a boolean value
/// and takes three arguments: appModule, which is the current module an evaluation is carried out on, userPermissions is the
/// array or collection of roles and modules for a particular user. Finally, role is the actual role being cross checked
/// at the time of evoking the method
/// DATE CREATED: 25/12/2017
///</SUMMARY>

function __authorised(appModule, userPermissions, role){
   
    var auth = false;
    //appModule = applicationModule.find(x => x.name === appModule);
   
    for(var x = 0; x < userPermissions.length; x ++){
        //console.log('in __authorised  ' + typeof(appModule.id) + " " + typeof(userPermissions[0].module));
        if(userPermissions[x].module === appModule){
           
          if(role === "Create"){
               
            auth = userPermissions[x].Create === 1 ? true : false;

          } else if(role === "Read"){

            auth = userPermissions[x].Read === 1 ? true : false;

          } else if (role === "Update"){

             auth = userPermissions[x].Update === 1 ? true : false;

          } else if (role === "Delete"){
             
            auth = userPermissions[x].Delete === 1 ? true : false;
          }
          
        } 
    }

    return auth; 
}

function _checkAccessRights(appModule, userPermissions){

    for(var x = 0; x < userPermissions.length; x ++){

        if(userPermissions[x].name === appModule){

            console.log(userPermissions[x].name + " = " + userPermissions[x].roles);

                switch(userPermissions[x].roles){

                      case 'CRUD':

                      console.log("Create, Read, Update and Delete rights");

                      _glcreateRole = true;
                      _glreadRole = true;
                      _glupdateRole = true;
                      _gldeleteRole = true;

                      break;

                      case 'CRU':

                          console.log("Create, Read and Update rights");
                          _glcreateRole = true;
                          _glreadRole = true;
                          _glupdateRole = true;

                          break;

                      case 'CR':

                          console.log("Create and Read rights");

                          _glcreateRole = true;
                          _glreadRole = true;

                          break;

                      case 'R':

                          console.log("Read rights");
                          _glreadRole = true;

                          break;

                      case 'C':

                          console.log("Create rights");
                          _glcreateRole = true;
                          break;

                      case undefined:
                          console.log("No rights for " + key);
                          break;
                  }
          }
     }
}

function _userhasAccesstoModule(userPermissions,appmodule){

  var hasModule = false;
  var results = userPermissions.find(x => x.module === appmodule);
  if(results != undefined){
     hasModule = true;
  }
  return hasModule;
}

function _getUserAccessRights(){
    
        $.ajax({
                  cache: false,
                  datatype: "jsonp",
                  url:"/Home/RetrieveUserRoles",
                  type: 'GET',
                  success: function (data){

                     for(var x = 0; x < data.length; x ++){

                       module = findElementById(applicationModule,parseInt(data[x].ModuleId));
                       userPrivilegdes.push({module: module.name, 
                                             Create: data[x].CreateRole,
                                               Read: data[x].ReadRole,
                                             Update: data[x].UpdateRole,
                                             Delete: data[x].DeleteRole,
                                               Auth: data[x].AuthRole,
                                         SignedInTo: data[x].SignedInTo,
                                           UserName: data[x].UserName,
                                           FullName: data[x].FullName
                                             });

                       assignedModules.push(module.name);

                     }

                     userFullName = userPrivilegdes[0].FullName;
                      
                  },
                  error: function(jqXHR, textStatus, errorThrown){
                         console.log("Error, " + errorThrown);
                  }
        });  
}

function retrieveUnassignedRoles(userPermissions){

  for(var x = 0; x < _glapplicationModule.length; x ++){

    missingRoles.push(userPermissions.find(findItem));
    index ++;
  }

  ///<summary>
  ///identifying which modules are unassigned for this user...
  ///</summary>
  //console.log("Listing Unassigned modules...");


   for(var j = 0; j < missingRoles.length; j ++){

      if(missingRoles[j] === undefined){

           switch (j) {
             case 0:
               console.log("Payroll");
               break;

             case 1:
               console.log("Payments");
               break;

             case 2:
               console.log("Pension");
               break;

             case 3:
               console.log("Holiday");
               break;

             case 4:
               console.log("Loans");
               break;

             case 5:
               console.log("Personnel");
               break;

             case 6:
               console.log("Reports");
               break;

             case 7:
             console.log("Security");
             break;

             case 8:
             console.log("Settings");
             break;
             
             case 9:
             console.log("Taxation");
             break;

             default:

           }
      }
   }

}

var index = 0;
var missingRoles = [];

function findItem(assignedRoles){
    
    return assignedRoles.name === _glapplicationModule[index];
}

function _retrieveEarningsPayslipData(reportdate){
    
    ///<summary>
    /// To avoid repopulating the payslipreportEarningsDataSource with duplicate data during each event
    /// the _generateReport method is called, a check is made to see if the object has data already.
    /// NOTE: the same check has been implemented in the _retrieveDeductionsPayslipData() method...
    ///</summary>

   //var x = (payslipreportEarningsDataSource.length != 0 ? payslipreportEarningsDataSource = [] : payslipreportEarningsDataSource);
   //console.log('checking the if earnigs datasource is empty: ' + payslipreportEarningsDataSource.length);
   
   
   if(payslipreportEarningsDataSource.length == 0){
      
       $.ajax({
              cache: false,
              datatype: "jsonp",
              url:"/Report/RetrievePayrollEarnings",
              type: 'POST',
              data: {__date: reportdate},
              success: function (data){

                for(var x = 0; x < data.length; x ++){
                    payslipreportEarningsDataSource.push({id: data[x].EmpID ,
                                                        type: data[x].payCodeDescription ,
                                                      amount: data[x].ActualAmount, 
                                                        date: _parseJsonDate(data[x].DATE),
                                                pamentNumber: data[x].paymentNumber });
                }

 
              },
              error: function(jqXHR, textStatus, errorThrown){
                       console.log("Error, " + errorThrown);
              }
     });
   }
   
}

function _retrieveDeductionsPayslipData(currentDate){

      if(payslipreportDeductionsDataSource.length == 0){

           $.ajax({
                cache: false,
                datatype: "jsonp",
                url:"/Report/RetrievePayrollDeductions",
                type: 'POST',
                data: {__date: currentDate},
                success: function (data){

                  for(var x = 0; x < data.length; x ++){
                      payslipreportDeductionsDataSource.push({id: data[x].EmpID,
                                                            type: data[x].payCodeDescription,
                                                            date: _parseJsonDate(data[x].DATE),
                                                          amount: data[x].ActualAmount,
                                                    pamentNumber: data[x].paymentNumber });
                  }

                },
                error: function(jqXHR, textStatus, errorThrown){
                         console.log("Error, " + errorThrown);
                }
           });
      }

}

function _populateEarnings(__empId){

 var divElements;

  for (var i = 0; i < payslipreportEarningsDataSource.length; i++) {

    if(payslipreportEarningsDataSource[i].id === __empId){
        divElements += '<tr><td>' + payslipreportEarningsDataSource[i].type +'</td><td>' + payslipreportEarningsDataSource[i].amount + '</td></tr>';
        payslipreportTotalEarnings = parseFloat(payslipreportTotalEarnings) + parseFloat(payslipreportEarningsDataSource[i].amount);
    }

  }

  return divElements;
}

function _populateDeductions(){

 var divElements;

  for (var i = 0; i < payslipreportDeductionsDataSource.length; i++) {
     divElements += '<tr><td>' + payslipreportDeductionsDataSource[i].type +'</td><td>' + payslipreportDeductionsDataSource[i].amount + '</td></tr>';
     payslipreportTotalDeductions = parseFloat(payslipreportTotalDeductions) + payslipreportDeductionsDataSource[i].amount;
  }

  return divElements;
}

function __retrieveDemographics(){
  
  if(payslipreportDemographicsDataSource.length === 0){

        $.ajax({
           cache: false,
           datatype: 'jsonp',
           url: '/Payroll/PayrollList',
           type: 'GET',
           success: function(data){
              for(var x = 0; x < data.length; x ++){
                payslipreportDemographicsDataSource.push({id: data[x].EmpID,name: data[x].EmpName,grade: data[x].GradeID});
              }
           },
           error: function(jqXHR, textStatus, errorThrown){
               console.log("Error, " + errorThrown);
           }
       });
  }

}

function findElementById(arr, item){
   var r = arr.find(x => x.id === item);
   return r;
}

function __filterPendingItems(arr){
  
  for(var u = 0; u < userPrivilegdes.length; u ++){

     var r = arr.find(x => x.Controller === userPrivilegdes[u].module);

     if(r != undefined){
       var tagname = document.getElementById('notification');
       //tagname.className = "label label-danger";
       //tagname.innerHTML = pendingItemsCount;
     } 
  }

}

function __getPendingItems(){
  
    var t = [];

      $.ajax({
           cache: false,
           datatype: 'jsonp',
           url: '/Settings/GetPendingTransactions',
           type: 'GET',
           success: function(data){
            
              for(var x = 0; x < data.length; x ++){
                t.push({id: data[x].ItemId,
                      name: data[x].FullName,
                    Source: data[x].Source, 
                Controller: data[x].Controller});
              }
             
              pendingItemsCount = t.length;
              if(pendingItemsCount != 0 ) __filterPendingItems(t);
           },
           error: function(jqXHR, textStatus, errorThrown){
               console.log("Error, " + errorThrown);
           }
       });
      
      //__getPendingItems();
}

function __authorizeRecord(controller, action, recordId){
        
             $.ajax({
                   cache: false,
                   datatype: 'jsonp',
                   url: '/' + controller + '/' + action,
                   type: 'POST',
                   data: {Id: recordId},
                   success: function(data, status, xhr){

                       _notificationMessage(data.status, data.message);
                   },
                   error: function(jqXHR, textStatus, errorThrown){
                       console.log("Error, " + errorThrown);
                   }
               }); 

}

function __POSTWrapper(record, functionId){

   var object = findElementById(crudMethodMapping, functionId);
   
   if(object != undefined){

       $.ajax({
           cache: false,
           datatype: "json",
           url: "/" + object.controller + "/" + object.action,
           type: "POST",
           data: record,
           success: function (data, status, xhr) {
            
             _notificationMessage(data.status, data.message);
          },
          error: function (jqXHR, textStatus, errorThrown) {
               
             _notificationMessage("error", errorThrown); 
          }
      }); 
   }
   
}

function __GETWrapper(){
   //logic goes here..
}

function __toolBarMapper(id){
   console.log(id + " " + _glcontroller);
}

function __initializeMainUIElements(){
    
    var signedinto = userPrivilegdes[0].SignedInTo;
    var buttons = '<ul class="list-unstyled components">';
    buttons += '<li class="active">';
    buttons += '<a href="#" onclick="loadAction(this.id);" id="homeMenuBtn">';
    buttons += '<i class="glyphicon glyphicon-home"></i> Home </a></li>';
    buttons += '<li>';

    if(userPrivilegdes[0].SignedInTo === 'ESS'){ 

       ///<summary>
       ///user is logged in to ESS module, load the ESS menu items
       ///</summary>

          buttons += '<li><a href="#optionsSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-briefcase"></i> Options </a>';
          buttons += '<ul class="collapse list-unstyled" id="optionsSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="requestLeaveMenuBtn">Request leave</a></li>';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="timesheetMenuBtn">Timesheets</a></li>';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payslipsMenuBtn">Payslips</a></li></ul>';

       document.getElementById('app-title').innerHTML = '<h4>Employee Self Service</h4><strong>SSF</strong>';
       
    } else {

       ///<summary>
       ///user is logged in to PAYROLL module, load the Payroll menu items
       ///</summary>

      document.getElementById('app-title').innerHTML = '<h3>PayrollSoft</h3><strong>SSF</strong>';

      if(_userhasAccesstoModule(userPrivilegdes,'Payroll')){

          buttons += '<li><a href="#payrollSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-briefcase"></i> Payroll </a>';
          buttons += '<ul class="collapse list-unstyled" id="payrollSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payrollServiceMenuBtn">Payroll service</a></li>';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payrollListMenuBtn">Payroll list</a></li></ul>';
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Deductions')){

          buttons += '<a href="#pageSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-duplicate"></i>Payments</a>';
          buttons += '<ul class="collapse list-unstyled" id="pageSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="deductionsMenuBtn">Deductions</a></li>';

          if(!_userhasAccesstoModule(userPrivilegdes, 'Earnings') && !_userhasAccesstoModule(userPrivilegdes, 'Loans')){
             buttons += '</ul>';
          }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Earnings')){

          if(_userhasAccesstoModule(userPrivilegdes, 'Deductions')){

            buttons +='<li><a href="#" onclick="loadAction(this.id);" id="earningsMenuBtn">Earnings</a></li>';
             
          } else {

            buttons += '<a href="#pageSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-duplicate"></i>Payments</a>';
            buttons += '<ul class="collapse list-unstyled" id="pageSubmenu">';
            buttons +='<li><a href="#" onclick="loadAction(this.id);" id="earningsMenuBtn">Earnings</a></li>';
            
            if(!_userhasAccesstoModule(userPrivilegdes, 'Loans')){
               buttons +='</ul>';
            }
               
          }
      }
      
      if(_userhasAccesstoModule(userPrivilegdes, 'Loans')){

         if(_userhasAccesstoModule(userPrivilegdes, 'Deductions') && _userhasAccesstoModule(userPrivilegdes,'Earnings')){

            buttons +='<li><a href="#" onclick="loadAction(this.id);" id="loansMenuBtn">Loans</a></li>';
            buttons +='</ul>';
              
         } else {

            buttons += '<a href="#pageSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-duplicate"></i>Payments</a>';
            buttons += '<ul class="collapse list-unstyled" id="pageSubmenu">';
            buttons +='<li><a href="#" onclick="loadAction(this.id);" id="loansMenuBtn">Loans</a></li>';
            buttons +='</ul>';
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Personnel')){

          buttons += '<a href="#personnelSubmenu" data-toggle="collapse" aria-expanded="false">'
          buttons += '<i class="glyphicon glyphicon-user"></i>Personnel</a>';
          buttons += '<ul class="collapse list-unstyled" id="personnelSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelListMenuBtn">Listings</a></li>';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelNewMenuBtn">New personnel</a></li></ul>';
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Pendings')){
          //<span id="notification" class="label label-danger">3</span>
          buttons += '<a href="#pendingSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-edit"></i>Pending items <span id="notification" class="label label-default">0</span></a>';
          buttons += '<ul class="collapse list-unstyled" id="pendingSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="approvalsBtn">Approvals</a></li>';
         buttons += '</ul>';
      }


      if(_userhasAccesstoModule(userPrivilegdes, 'ReportPayslip')){
        

          buttons += '<a href="#reportsSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-duplicate"></i>Reports</a>';
          buttons += '<ul class="collapse list-unstyled" id="reportsSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelPayslipRpt">Personnel payslips</a></li>';

          if(!_userhasAccesstoModule(userPrivilegdes, 'ReportPension') && !_userhasAccesstoModule(userPrivilegdes, 'ReportCompanyPayslip')){
             buttons += '</ul>';
          }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'ReportCompanyPayslip')){

         if(_userhasAccesstoModule(userPrivilegdes, 'ReportPayslip')){

             buttons += '<li><a href="#">Company payslips</a></li>';

         } else {

            buttons += '<a href="#reportsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-duplicate"></i>Reports</a>';
            buttons += '<ul class="collapse list-unstyled" id="reportsSubmenu">';
            buttons += '<li><a href="#">Company payslips</a></li>';
         }

         if(!_userhasAccesstoModule(userPrivilegdes, 'ReportPayslip')){
            buttons += '</ul>';
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'ReportPension')){

         if(_userhasAccesstoModule(userPrivilegdes, 'ReportPayslip') || _userhasAccesstoModule(userPrivilegdes, 'ReportCompanyPayslip')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionRpt">Pension Report</a></li>';
         } else {

            buttons += '<a href="#reportsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-duplicate"></i>Reports</a>';
            buttons += '<ul class="collapse list-unstyled" id="reportsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionRpt">Pension Report</a></li>';
            buttons += '</ul>';
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

          buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
          buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="userMgtMenuBtn">User management</a></li>';

         if(!_userhasAccesstoModule(userPrivilegdes, 'Leaves') && !_userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){
           if(!_userhasAccesstoModule(userPrivilegdes, 'Overtime') && !_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll')){
            if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') && !_userhasAccesstoModule(userPrivilegdes, 'Pension')){
              if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
                buttons += '</ul>';
              } 
            }
           }
         }         
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Leaves')){

         if(_userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="leavesMenuBtn">Leaves</a></li>'; 

         } else {
          buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
          buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
          buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="leavesMenuBtn">Leaves</a></li>';
         }

         if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsLoans') && !_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll')){
            if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') && !_userhasAccesstoModule(userPrivilegdes, 'Pension')){
              if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
                buttons += '</ul>';
              } 
            }
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

         if(_userhasAccesstoModule(userPrivilegdes, 'UserManagement') || _userhasAccesstoModule(userPrivilegdes, 'Leaves')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="loanSettingsMenuBtn">Loans</a></li>';

         } else {

            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="loanSettingsMenuBtn">Loans</a></li>';
         }

         if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll') && ! _userhasAccesstoModule(userPrivilegdes, 'Overtime')){
            if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') && !_userhasAccesstoModule(userPrivilegdes, 'Pension')){
              if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
                buttons += '</ul>';
              } 
            }
          }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Overtime')){

        if(_userhasAccesstoModule(userPrivilegdes, 'UserManagement') || _userhasAccesstoModule(userPrivilegdes, 'Leaves') || _userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="overtimeSettingsMenuBtn">Overtime</a></li>';

        } else {

            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="overtimeSettingsMenuBtn">Overtime</a></li>';
        }

         if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll')){
            if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') && !_userhasAccesstoModule(userPrivilegdes, 'Pension')){
              if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
                buttons += '</ul>';
              } 
            }
          }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll')){

        if(_userhasAccesstoModule(userPrivilegdes, 'Overtime') || _userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

           buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payrollSettingsMenuBtn">Payroll</a></li>';

        } else if(_userhasAccesstoModule(userPrivilegdes, 'Leaves') || _userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

           buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payrollSettingsMenuBtn">Payroll</a></li>';

        } else {

            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="payrollSettingsMenuBtn">Payroll</a></li>';
        }

        if(!_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') && !_userhasAccesstoModule(userPrivilegdes, 'Pension')){
          if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
            buttons += '</ul>';
          } 
        }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel')){

         if(_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll') || _userhasAccesstoModule(userPrivilegdes, 'Overtime')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelsettingsMenuBtn">Personnel</a></li>';

         } else if(_userhasAccesstoModule(userPrivilegdes,'SettingsLoans') || _userhasAccesstoModule(userPrivilegdes, 'Leaves') || _userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelsettingsMenuBtn">Personnel</a></li>';

         } else {

            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="personnelsettingsMenuBtn">Personnel</a></li>';
         }

         if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation') && !_userhasAccesstoModule(userPrivilegdes,'Pension')){
            buttons += '</ul>';
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Pension')){

         if(_userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel') || _userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionMenuBtn">Pension</a></li>';

         } else if (_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll') || _userhasAccesstoModule(userPrivilegdes, 'Overtime')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionMenuBtn">Pension</a></li>';

         } else if (_userhasAccesstoModule(userPrivilegdes, 'Leaves') || _userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionMenuBtn">Pension</a></li>';

         } else {

            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="pensionMenuBtn">Pension</a></li>';
         }

         if(!_userhasAccesstoModule(userPrivilegdes, 'Severance') && !_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
            buttons += '</ul>';
         }
      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Severance')){

        if(_userhasAccesstoModule(userPrivilegdes,'Pension') || _userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel')){

           buttons += '<li><a href="#" onclick="loadAction(this.id);" id="severanceMenuBtn">Severance</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll') || _userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="severanceMenuBtn">Severance</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'Overtime') || _userhasAccesstoModule(userPrivilegdes, 'Leaves')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="severanceMenuBtn">Severance</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'UserManagement')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="severanceMenuBtn">Severance</a></li>';

        } else {
          
            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="severanceMenuBtn">Severance</a></li>';
        }

        if(!_userhasAccesstoModule(userPrivilegdes, 'Taxation')){
          buttons += '</ul>';
        }

      }

      if(_userhasAccesstoModule(userPrivilegdes, 'Taxation')){

        if(_userhasAccesstoModule(userPrivilegdes,'Pension') || _userhasAccesstoModule(userPrivilegdes, 'SettingsPersonnel')){

           buttons += '<li><a href="#" onclick="loadAction(this.id);" id="taxationMenuBtn">Taxation</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'SettingsPayroll') || _userhasAccesstoModule(userPrivilegdes, 'SettingsLoans')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="taxationMenuBtn">Taxation</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'Overtime') || _userhasAccesstoModule(userPrivilegdes, 'Leaves')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="taxationMenuBtn">Taxation</a></li>';

        } else if (_userhasAccesstoModule(userPrivilegdes, 'UserManagement') || _userhasAccesstoModule(userPrivilegdes, 'Severance')){

          buttons += '<li><a href="#" onclick="loadAction(this.id);" id="taxationMenuBtn">Taxation</a></li>';

        } else {
          
            buttons += '<a href="#settingsSubmenu" data-toggle="collapse" aria-expanded="false">';
            buttons += '<i class="glyphicon glyphicon-cog"></i>Settings</a>';
            buttons += '<ul class="collapse list-unstyled" id="settingsSubmenu">';
            buttons += '<li><a href="#" onclick="loadAction(this.id);" id="taxationMenuBtn">Taxation</a></li>';
        }
      }

    }
 
    
    buttons += '</li>';
    buttons += '</ul>';
    document.getElementById('menu-content').innerHTML = buttons;
    
}








