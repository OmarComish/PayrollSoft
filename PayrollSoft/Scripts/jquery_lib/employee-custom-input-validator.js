
//<summary>
// Input validation code
//<summary/>


$('#newEmployee-form').jqxValidator({
      rules: [
              { input: '#firstName', message: 'First name is required!', action: 'keyup, blur', rule: 'required' },
              { input: '#firstName', message: 'First name must contain only letters!', action: 'keyup', rule: 'notNumber' },
              { input: '#firstName', message: 'First name must be between 3 and 35 characters!', action: 'keyup', rule:  function(input, commit){
                                        

                     if(input.val().trim().length < 3){

                           return false;

                      } else {

                            return true;
                      }

                    } 
                },
                { input: '#middleName', message: 'Middle name must contain only letters!', action: 'keyup', rule: 'notNumber' },
                { input: '#middleName', message: 'Middle name must be between 3 and 35 characters!', action: 'keyup', rule:  function(input, commit){
                                        

                    if(input.val().trim().length === 0){

                          return true;
                      } else {

                        return false;
                      }

                     } 
                  }
              ],theme:theme
  });