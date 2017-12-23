import { Component, Inject, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription'; 
import { Meta, Title, DOCUMENT, MetaDefinition } from '@angular/platform-browser'; 
import { LinkService } from '../../shared/link.service';

// i18n support (Internacionalisation)
import { TranslateService } from '@ngx-translate/core'
import { ORIGIN_URL, REQUEST } from '@nguniversal/aspnetcore-engine';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./theme.material.scss', './app.component.scss'], 
    //disable this [_ngcontent-c0] on CSS styles and elements of Root App Component
    encapsulation: ViewEncapsulation.None 
})
export class AppComponent implements OnInit, OnDestroy {

    // This will go at the END of your title for example "Home - Angular Universal..." <-- after the dash (-)
    private _endPageTitle : string = 'Damkorki.pl'; 
    // If no Title is provided, we'll use a default one before the dash (-)
    private _defaultPageTitle : string = "Oferty korepetycji"; 

    private _routerSubscription : Subscription; 

    constructor(private _router: Router, 
                private _activatedRoute: ActivatedRoute, 
                private _title: Title, 
                private _meta: Meta, 
                private _linkService: LinkService, 
                public translate : TranslateService,
                @Inject(REQUEST) private _request, 
                @Inject(ORIGIN_URL) private _originUrl: string
            )
    {
        // this language will be used as a fallback when a translation isn't found in the current language 
        translate.setDefaultLang('pl'); 

        // the language to use, if the language isn't available, it will use the current loader to get them
        translate.use('pl'); 

        // REQUEST object 
        console.log("What's our REQUEST object looks like?");
        console.log("The REQUEST object only exists on the Server side, on the Browser side we can at least see Cookies");
        console.log(this._request); 
        console.log("ORIGIN_URL " + this._originUrl); 
    }

    ngOnInit() 
    {
        console.log("Reloaded page"); 
        // change "Title" on every "navigationEnd" event
        // Titles come from the "data.title" property on all Routes (see app.routes.ts)
        this._changeTitleOnNavigation();  
    }

    ngOnDestroy() 
    { 
        // subscriptions clean-up 
        this._routerSubscription.unsubscribe(); 
    }

    private _changeTitleOnNavigation() { 

        this._routerSubscription = this._router.events
                                        .filter(event => event instanceof NavigationEnd )
                                        .map( () => this._activatedRoute )
                                        .map(route => { 
                                            while( route.firstChild ) route = route.firstChild; 
                                            return route;  
                                        })
                                        .filter(route => route.outlet === 'primary')
                                        .mergeMap(route => route.data)
                                        .subscribe((event) => { 
                                            this._setMetaAndLinks(event); 
                                        });
    }

    private _setMetaAndLinks(event) { 

        // set Title if available, otherwise leave the default one 
        const title = event['title'] ? event['title'] + " - " + this._endPageTitle 
                                     : this._defaultPageTitle + " - " + this._endPageTitle; 

        this._title.setTitle(title); 

        const metaItems= event['meta'] || []; 
        const linkItems = event['links'] || []; 

        metaItems.forEach(metaItem => {
            this._meta.updateTag(metaItem); 
        });

        linkItems.forEach(linkItem => {
            this._linkService.addTag(linkItem); 
        });
    }

}
