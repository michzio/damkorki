import { NgModule } from "@angular/core"; 
import { RouterModule, Routes } from "@angular/router"; 

import { AuthGuard } from "../../services/auth-guard.service";
import { CanDeactivateGuard } from "../../services/can-deactivate-guard.service";

// import components
import { LessonOffersComponent } from "./components/lesson-offers/lesson-offers.component";
import { AddLessonOfferComponent } from "./components/add-lesson-offer/add-lesson-offer.component";

const lessonOffersRoutes: Routes = [
    {
        path: 'lesson-offers', 
        component: LessonOffersComponent, 
        data: { title: 'Przeglądaj Oferty Lekcji' }
    }, 
    {
        path: 'lesson-offers/add', 
        component: AddLessonOfferComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(lessonOffersRoutes),
    ], 
    exports: [
        RouterModule
    ]
})
export class LessonOffersRoutingModule { }