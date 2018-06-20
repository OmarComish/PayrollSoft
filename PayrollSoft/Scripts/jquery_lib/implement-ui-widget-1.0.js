/*var index = 0;
var missingRoles = [];

function findItem(assignedRoles){

    return assignedRoles.name === _glapplicationModule[index];
}

function retrieveUnassignedRoles(userPermissions){

  for(var x = 0; x < _glapplicationModule.length; x ++){

    missingRoles.push(userPermissions.find(findItem));
    index ++;
  }

}

function _checkAccessRights(appModule, userPermissions){

    for(var x = 0; x < userPermissions.length; x ++){

        if(userPermissions[x].name === appModule){

            console.log(userPermissions[x].name + " = " + userPermissions[x].roles);

                switch(userPermissions[x].roles){

                      case 'CRUD':

                      console.log("Create, Read, Update and Delete rights");

                      createRole = true;
                      readRole = true;
                      updateRole = true;
                      deleteRole = true;

                      break;

                      case 'CRU':

                          console.log("Create, Read and Update rights");
                          createRole = true;
                          readRole = true;
                          updateRole = true;

                          break;

                      case 'CR':

                          console.log("Create and Read rights");

                          createRole = true;
                          readRole = true;

                          break;

                      case 'R':

                          console.log("Read rights");
                          readRole = true;

                          break;

                      case 'C':

                          console.log("Create rights");
                          createRole = true;
                          break;

                      case undefined:
                          console.log("No rights for " + key);
                          break;
                  }
          }
     }
}

function _getUserAccessRights(){

          $.ajax({
                  cache: false,
                  datatype: "jsonp",
                  url:"/Home/RetrieveUserRoles",
                  type: 'GET',
                  success: function (data){

                     for(var x = 0; x < data.length; x ++){
                       userPrivilegdes.push({name:data[x].ModuleId, roles: data[x].AccessId});
                     }

                     retrieveUnassignedRoles(userPrivilegdes);

                     //_checkAccessRights(appmodule, userPrivilegdes);

                  },
                  error: function(jqXHR, textStatus, errorThrown){
                         console.log("Error, " + errorThrown);
                  }
          });
}*/

$(document).ready(function () {

    var theme = getDemoTheme();



    //$("#jqxNavigationBar").jqxNavigationBar({ width: 165, expandMode: "multiple", expandedIndexes: [0] });

    /*$("#jqxNavigationBar").on('click',function(e){

         ///get all the modules the User has no complete rights to and disable the menu items


         _getUserAccessRights();

         ///<summary>
         ///identifying which modules are unassigned for this user...
         ///</summary>
         console.log("Listing Unassigned modules...");

         for(var j = 0; j < missingRoles.length; j ++){

            if(missingRoles[j] === undefined){

                 switch (j) {
                   case 0:
                     console.log("Payroll");
                     $("#jqxNavigationBar").jqxNavigationBar('disableAt',1);
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
                      $("#jqxNavigationBar").jqxNavigationBar('disableAt',2);
                     break;

                   case 6:
                     console.log("Reports");
                     $("#jqxNavigationBar").jqxNavigationBar('disableAt',3);
                     break;

                   case 7:
                   console.log("Security");
                   break;

                   default:

                 }
            }
         }


    })*/
});
