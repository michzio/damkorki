import { Component } from "@angular/core";

@Component({
    selector: 'page-not-found',
    template: `
        <div id="page-not-found">
            <h2>404 - Page Not Found</h2>
        </div>
    `,
    styles: [`
        #page-not-found { 
            margin: 5% 15%; 
        }
    `]
})
export class PageNotFoundComponent { }