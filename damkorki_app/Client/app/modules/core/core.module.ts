// import modules 
import { NgModule } from "@angular/core"
import { CommonModule } from "@angular/common"; 
import { RouterModule } from "@angular/router";

import { SharedModule } from "../shared/shared.module";

// import components
import { FooterBarComponent } from "./components/footer-bar/footer-bar.component";
import { LanguageSwitcherComponent } from "./components/language-switcher/language-switcher.component";


@NgModule({
    imports: [
        SharedModule, 
        RouterModule.forChild([])
    ], 
    declarations: [
        FooterBarComponent, 
        LanguageSwitcherComponent,
    ], 
    providers: [
        // custom services
    ],
    exports: [
        FooterBarComponent
    ]
})
export class CoreModule { }