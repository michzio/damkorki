import { NgModule } from '@angular/core'; 
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { LessonOffersComponent } from './components/lesson-offers/lesson-offers.component';

const APP_ROUTES : Routes = [
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
        path: 'lesson-offers', 
        component: LessonOffersComponent,
        data: { title: 'Przeglądaj Oferty Lekcji' }
    },
    // Otherwise - go home!
    { path: '**', redirectTo: 'home' }
]

@NgModule({
    imports: [
        RouterModule.forRoot(APP_ROUTES, /* { preloadingStrategy: SelectivePreloadingStrategy } */ )
    ],
    exports: [
        RouterModule
    ],
    providers: [
        // SelectivePreloadingStrategy
    ]
})
export class AppRoutingModule { }