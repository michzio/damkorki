// import modules 
import { NgModule } from "@angular/core"; 
import { CommonModule } from "@angular/common"; 
import { FormsModule, ReactiveFormsModule } from "@angular/forms"; 

// import material design modules 
import {MatSliderModule} from '@angular/material/slider';
import { MatSelectModule } from "@angular/material/select";

// import components
import { GridComponent } from './components/grid/grid.component'; 
import { GridCardComponent } from './components/grid-card/grid-card.component'; 
import { LeafletMapComponent } from './components/leaflet-map/leaflet-map.component';
import { LessonOffersComponent } from './components/lesson-offers/lesson-offers.component';
import { LessonOffersGridComponent } from './components/lesson-offers-grid/lesson-offers-grid.component';
import { LessonOfferGridCardComponent } from './components/lesson-offer-grid-card/lesson-offer-grid-card.component'; 
import { LessonOffersMapComponent } from './components/lesson-offers-map/lesson-offers-map.component';
import { AddLessonOfferComponent } from "./components/add-lesson-offer/add-lesson-offer.component";

// import services 
import { LessonOfferService } from "../../services/lesson-offer.service";
import { LegacyLibrariesService } from "../../services/legacy-libraries.service";

// import routing module 
import { LessonOffersRoutingModule } from "./lesson-offers-routing.module";

// import custom module 
import { CoreModule } from "../core/core.module";
import { SubjectService } from "../../services/subject.service";

@NgModule({
    imports: [
        CommonModule, 
        FormsModule, 
        ReactiveFormsModule,
        // material design 
        MatSliderModule,
        MatSelectModule,

        // routing modules
        LessonOffersRoutingModule,
        // custom modules
        CoreModule, 
    ], 
    declarations: [
        GridComponent,
        GridCardComponent,
        LeafletMapComponent,
        LessonOffersComponent, 
        LessonOffersGridComponent,
        LessonOfferGridCardComponent,
        LessonOffersMapComponent,
        AddLessonOfferComponent,
    ], 
    providers: [
        // custom services
        LessonOfferService,
        LegacyLibrariesService, 
        SubjectService
    ], 
    exports: [
        LessonOffersMapComponent, 
        LessonOffersGridComponent
    ]
})
export class LessonOffersModule { }