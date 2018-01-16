import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
    name: 'entries'
})
export class ObjectEntriesPipe implements PipeTransform { 
    
    transform(value: any, ...args: any[]) : any {
        let entries = []; 
        for(let key in value) { 
            entries.push({key: key, value: value[key]}); 
        }
        return entries; 
    }
}