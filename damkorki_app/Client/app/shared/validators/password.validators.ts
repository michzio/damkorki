import { FormControl } from "@angular/forms";

export class PasswordValidators { 
    
        public static containsDigit(control: FormControl) : any { 
            
            let containsDigit = /(?=.*\d)/; 
    
            return containsDigit.test(control.value) ? null : { containsDigit: true }; 
        }
    
        public static containsUppercaseLetter(control: FormControl) : any { 
    
            let containsUppercaseLetter = /(?=.*[A-Z])/;
    
            return containsUppercaseLetter.test(control.value) ? null : { containsUppercaseLetter: true }; 
        }
    
        public static containsLowercaseLetter(control: FormControl) : any { 
    
            let containsLowercaseLetter = /(?=.*[a-z])/;
    
            return containsLowercaseLetter.test(control.value) ? null : { containsLowercaseLetter: true }; 
        }
    
        public static containsSpecialCharacter(control: FormControl) : any {
            
            let containsSpecialCharacter = /(?=.*[$@$!%*#?&])/;
    
            return containsSpecialCharacter.test(control.value) ? null : { containsSpecialCharacter: true }; 
        }
    
        public static notContainsValuesOfInputs(inputs : [string]) { 
            
            return (control: FormControl) => { 
    
                    var contains : boolean = false; 
            
                    let form = control.root; 
                    let password : string = form.get('password').value; 
            
                    inputs.forEach((input) => { 
                        var inputValue : string = form.get(input).value;
                        if(inputValue.length > 2) {
                            contains = contains || password.toLowerCase().includes(inputValue.toLowerCase()); 
                        }
                    }); 
    
                    return contains ? { notContainsValuesOfInputs: true } : null; 
            }; 
        }

        public static matchValuesOfInputs(input1 : string, input2 : string) { 

            return (control: FormControl) => { 
                    
                    let value1 : string = control.root.get(input1).value;
                    let value2 : string = control.root.get(input2).value; 
                   
                    return (value1 === value2) ? null : { matchValuesOfInputs: true };
            }
        }
    }