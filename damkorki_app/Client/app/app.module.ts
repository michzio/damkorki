import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { HttpModule } from '@angular/http'; 
import { ReactiveFormsModule, FormsModule } from '@angular/forms'; 
import { HttpClientModule, HttpClient } from '@angular/common/http'; 

// Bootstrap support 
import { AlertModule } from 'ngx-bootstrap';
// My custom app routing module 
import { AppRoutingModule } from './app-routing.module';
// My custom angular material design module
import { MaterialDesignModule } from './material-design.module';

// from angular 2 universal not compatible with angular 4
// import { UniversalModule } from 'angular2-universal';

// i18n support (Internacionalisation)
import { TranslateModule, TranslateLoader } from '@ngx-translate/core'; 
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

// HTTP interceptors 
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './shared/auth.interceptor';
import { AuthRefreshInterceptor } from './shared/auth-refresh.interceptor'; 

import { LinkService } from './shared/link.service'; 
import { ConnectionResolver } from './shared/route.resolver';
import { ORIGIN_URL } from '@nguniversal/aspnetcore-engine';
import { TransferHttpModule } from '../modules/transfer-http/transfer-http.module'; 

// components 
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ExternalLoginComponent } from './components/external-login/external-login.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ResetPasswordPageComponent } from './components/reset-password-page/reset-password-page.component';
import { RegisterComponent } from './components/register/register.component'; 
import { RegisterPageComponent } from './components/register-page/register-page.component'; 
import { LessonOffersComponent } from './components/lesson-offers/lesson-offers.component';
import { SearchBoxComponent } from './components/search-box/search-box.component';
import { IfRoutesDirective } from './directives/if-routes.directive';
import { HeaderBarComponent } from './components/header-bar/header-bar.component'; 
import { GridComponent } from './components/grid/grid.component'; 
import { LessonOffersGridComponent } from './components/lesson-offers-grid/lesson-offers-grid.component';
import { GridCardComponent } from './components/grid-card/grid-card.component'; 
import { LessonOfferGridCardComponent } from './components/lesson-offer-grid-card/lesson-offer-grid-card.component'; 
import { LessonOffersMapComponent } from './components/lesson-offers-map/lesson-offers-map.component';
import { LeafletMapComponent } from './components/leaflet-map/leaflet-map.component';
import { FooterBarComponent } from './components/footer-bar/footer-bar.component';
import { LanguageSwitcherComponent } from './components/language-switcher/language-switcher.component';
import { ProfileDropdownComponent } from './components/profile-dropdown/profile-dropdown.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

// services 
import { AuthService } from "./services/auth.service";
import { UserService } from "./services/user.service"; 

// guards
import { AuthGuard } from './services/auth-guard.service';
import { CanDeactivateGuard } from './services/can-deactivate-guard.service';

// feature modules 
import { ProfileModule } from "./modules/profile/profile.module";
import { AdminModule } from "./modules/admin/admin.module";
import { JwtHelperService } from '@auth0/angular-jwt';

//import { JwtModule } from '@auth0/angular-jwt';

export function createTranslateLoader(httpClient: HttpClient, baseHref) { 
    // Temporary Azure hack 
    if(baseHref === null && typeof window !== 'undefined') { 
        baseHref = window.location.origin; 
    }
    // i18n files are in 'wwwroot/assets/'
    return new TranslateHttpLoader(httpClient, baseHref + "/assets/i18n/", '.json'); 
}

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        ExternalLoginComponent, 
        LoginPageComponent,
        ResetPasswordComponent,
        ResetPasswordPageComponent, 
        RegisterComponent, 
        RegisterPageComponent, 
        LessonOffersComponent,
        SearchBoxComponent,
        IfRoutesDirective,
        HeaderBarComponent,
        GridComponent,
        LessonOffersGridComponent,
        GridCardComponent,
        LessonOfferGridCardComponent,
        LessonOffersMapComponent,
        LeafletMapComponent,
        FooterBarComponent,
        LanguageSwitcherComponent,
        ProfileDropdownComponent,
        PageNotFoundComponent,
    ],
    entryComponents: [
        ResetPasswordComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        HttpClientModule, 
        FormsModule, 
        ReactiveFormsModule,
        // ngx-bootstrap modules:
        AlertModule.forRoot(), 
        //Ng2BootstrapModule.forRoot(), // You could also split this up if you don't want the Entire Module imported

        TransferHttpModule, // our Http TransferData method 

        // i18n support 
        TranslateModule.forRoot({
            loader: { 
                provide: TranslateLoader, 
                useFactory: (createTranslateLoader),
                deps: [HttpClient, [ORIGIN_URL]]
            }
        }),

        // feature modules 
        ProfileModule,
        AdminModule, 

        // Application Routing 
        AppRoutingModule,

        // Material Design 
        MaterialDesignModule, 
    ],
    providers: [
        LinkService,
        TranslateModule, 
        AuthService, 
        { 
            provide: HTTP_INTERCEPTORS, 
            useClass: AuthInterceptor, 
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS, 
            useClass: AuthRefreshInterceptor, 
            multi: true 
        },
        AuthGuard,
        CanDeactivateGuard,
        UserService
    ]
})
export class AppModule {
    
}
