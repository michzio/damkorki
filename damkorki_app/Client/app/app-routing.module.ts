import { NgModule } from '@angular/core'; 
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { LessonOffersComponent } from './components/lesson-offers/lesson-offers.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';

const appRoutes : Routes = [
     { 
        path: '', 
        redirectTo: 'home', 
        pathMatch: 'full',
    },
    {
        path: 'home', 
        component: HomeComponent,

        /***  SEO magic ***/
        // Use "data" object to pass in <title>, <meta>, <link> tag information
        // Note: only for ROOT level Routes
        // When you change Routes it will automatically append these to your document 
        // for you on the Server side (more in app.component.ts)
        data: {
            title: 'Home',
            meta: [{ name: 'description', content: 'Strona główna aplikacji damkorki.pl' }],
            links: [
                { rel: 'canonical', href: 'http:/blogs.example.com/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com' }
            ]
        }
    },
    {
        path: 'login', 
        component: LoginPageComponent,
        data: { title: 'Log in'}
    },
    {
        path: 'signin', 
        redirectTo: 'login', 
        pathMatch: 'full'
    },
    {
        path: 'register', 
        component: RegisterPageComponent, 
        data: { title: 'Sign up' }
    },
    { 
        path: 'signup', 
        redirectTo: 'register', 
        pathMatch: 'full'
    }, 
    {
        path: 'lesson-offers', 
        component: LessonOffersComponent,
        data: { title: 'Przeglądaj Oferty Lekcji' }
    },
    // Otherwise - page not found!
    { 
        path: '**', 
        component: PageNotFoundComponent 
    }
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes, /* { preloadingStrategy: SelectivePreloadingStrategy } */ )
    ],
    exports: [
        RouterModule
    ],
    providers: [
        // SelectivePreloadingStrategy
    ]
})
export class AppRoutingModule { }