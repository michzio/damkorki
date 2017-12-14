// import modules 
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

// import components 
import { AdminComponent } from "./components/admin/admin.component"; 

// import services 

// import routing module 
import { AdminRoutingModule } from "./admin-routing.module"; 

@NgModule({
    imports: [
        CommonModule, 
        AdminRoutingModule, 
    ],
    declarations: [
        AdminComponent,
    ], 
    providers: [ ]
})
export class AdminModule { }