import { FormControl, FormGroup } from "@angular/forms";

export class DateValidators { 

    public static daysOfMonths : [number] = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]; 

    public static validDay(group : FormGroup) : any { 

        let monthOfBirth : number = +(group.get('monthOfBirth').value) || 0; 
        let dayOfBirth : number = +(group.get('dayOfBirth').value) || 0; 
        let yearOfBirth : number = +(group.get('yearOfBirth').value) || 0; 

        var daysOfMonth = DateValidators.daysOfMonths[monthOfBirth];

        // if leap year then add +1 day in Fabruary
       if(monthOfBirth == 1 && yearOfBirth % 4 == 0) daysOfMonth += 1;

       return dayOfBirth < 1 || dayOfBirth > daysOfMonth ? { validDay: true } : null; 
    }
}