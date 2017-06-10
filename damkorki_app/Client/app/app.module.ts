import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { HttpModule, Http } from '@angular/http'; 
import { FormsModule } from '@angular/forms'; 

// Bootstrap support 
import { Ng2BootstrapModule } from 'ng2-bootstrap';
// My custom app routing module 
import { AppRoutingModule } from './app-routing.module'

// from angular 2 universal not compatible with angular 4
// import { UniversalModule } from 'angular2-universal';

// i18n support (Internacionalisation)
import { TranslateModule, TranslateLoader } from '@ngx-translate/core'; 
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './components/app/app.component'
import { HomeComponent } from './components/home/home.component';
import { LessonOffersComponent } from './components/lesson-offers/lesson-offers.component'
import { SearchBoxComponent } from './components/search-box/search-box.component'
import { IfRoutesDirective } from './directives/if-routes.directive'
import { HeaderBarComponent } from './components/header-bar/header-bar.component' 
import { GridComponent } from './components/grid/grid.component'; 
import { OffersGridComponent } from './components/offers-grid/offers-grid.component';
import { GridCardComponent } from './components/grid-card/grid-card.component'; 
import { OffersGridCardComponent } from './components/offers-grid-card/offers-grid-card.component'; 
import { OffersMapComponent } from './components/offers-map/offers-map.component';
import { LeafletMapComponent } from './components/leaflet-map/leaflet-map.component';
import { FooterBarComponent } from './components/footer-bar/footer-bar.component';
import { LanguageSwitcherComponent } from './components/language-switcher/language-switcher.component'

import { LinkService } from './shared/link.service'; 
import { ConnectionResolver } from './shared/route.resolver';
import { ORIGIN_URL } from './shared/constants/baseurl.constants';
import { TransferHttpModule } from '../modules/transfer-http/transfer-http.module'; 


export function createTranslateLoader(http: Http, baseHref) { 
    // Temporary Azure hack 
    if(baseHref === null && typeof window !== 'undefined') { 
        baseHref = window.location.origin; 
    }
    // i18n files are in 'wwwroot/assets/'
    return new TranslateHttpLoader(http, baseHref + "/assets/i18n/", '.json'); 
}

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LessonOffersComponent,
        SearchBoxComponent,
        IfRoutesDirective,
        HeaderBarComponent,
        GridComponent,
        OffersGridComponent,
        GridCardComponent,
        OffersGridCardComponent,
        OffersMapComponent,
        LeafletMapComponent,
        FooterBarComponent,
        LanguageSwitcherComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule, 
        Ng2BootstrapModule.forRoot(), // You could also split this up if you don't want the Entire Module imported

        TransferHttpModule, // our Http TransferData method 

        // i18n support 
        TranslateModule.forRoot({
            loader: { 
                provide: TranslateLoader, 
                useFactory: (createTranslateLoader),
                deps: [Http, [ORIGIN_URL]]
            }
        }),

        // Application Routing 
        AppRoutingModule,
    ],
    providers: [
        LinkService,
        TranslateModule
    ]
})
export class AppModule {
    
}
