import { NgModule } from "@angular/core";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
    imports: [ MatCheckboxModule, MatDialogModule ],
    exports: [ MatCheckboxModule, MatDialogModule ] 
})
export class MaterialDesignModule { }