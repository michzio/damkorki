// import modules 
import { NgModule } from "@angular/core"
import { CommonModule } from "@angular/common"; 
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";

@NgModule({
    imports: [
        CommonModule,  
    ], 
    exports: [
        CommonModule, 
        TranslateModule
    ]
})
export class SharedModule { }