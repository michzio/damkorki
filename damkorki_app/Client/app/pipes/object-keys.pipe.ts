import { PipeTransform, Pipe } from "@angular/core"; 

@Pipe({
    name: 'keys'
})
export class ObjectKeysPipe implements PipeTransform { 
    
    transform(value: any, ...args: any[]) :any {
        let keys = []; 
        for(let key in value) { 
            keys.push(key); 
        }
        return keys; 
    }
}