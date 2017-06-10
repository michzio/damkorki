import {Directive, ElementRef, Renderer, Input, OnInit} from '@angular/core'; 
import {Router, NavigationEnd} from '@angular/router';

@Directive({
    selector:  '[if-routes]'

})
export class IfRoutesDirective implements OnInit { 

    @Input("if-routes") routes : string[] = [];
    @Input("unless-routes") unlessRoutes : string[] = [];
    @Input("apply-styles") applyStyles : Object; 

    constructor(private _router : Router, 
                private _elemRef : ElementRef, 
                private _renderer: Renderer) {    
    }

    ngOnInit() { 
         //console.log(this.routes); 
         //console.log(this.unlessRoutes);

         this._router.events.subscribe(evt => {
           if(evt instanceof NavigationEnd) { 
                var url = evt.urlAfterRedirects;
                //console.log(url);
                if(this.routes.indexOf(url) >= 0 || this.routes.indexOf('**') >= 0) { 
                    // add element to DOM
                    // console.log("if routes matched!"); 
                    this._renderer.setElementStyle(this._elemRef.nativeElement, "display", "block"); 
                }  
                if(this.unlessRoutes.indexOf(url) >= 0 || this.unlessRoutes.indexOf('**') >= 0) { 
                    // remove element from DOM
                    // console.log("unless routes matched!");
                    this._renderer.setElementStyle(this._elemRef.nativeElement, "display", "none"); 
                } 

                if(this.applyStyles != null && ( this.routes.indexOf(url)  >= 0 || this.routes.indexOf('**')  >= 0) ) { 

                    if(this.unlessRoutes.indexOf(url) < 0) { 
                        // apply given styles to current DOM element 
                        for(var style in this.applyStyles) { 
                             this._renderer.setElementStyle(this._elemRef.nativeElement, style, this.applyStyles[style]); 
                        };
                    }
                } 
           }
        });
    }

}
