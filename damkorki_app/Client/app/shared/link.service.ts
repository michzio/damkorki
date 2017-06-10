/* 
 * -- LinkService --        [Temporary]
 * @MarkPieszak
 * 
 * Similar to Meta service but made to handle <link> creation for SEO purposes
 * Soon there will be an overall HeadService within Angular that handles Meta/Link everything
 */

import { Injectable, PLATFORM_ID, Optional, RendererFactory2, ViewEncapsulation, Inject } from '@angular/core'; 
import { DOCUMENT } from '@angular/platform-browser';
import { isPlatformServer } from '@angular/common'; 

@Injectable()
export class LinkService { 

    private isServer : boolean = isPlatformServer(this._platform_id); 

    constructor(
        private _rendererFactory : RendererFactory2, 
        @Inject(DOCUMENT) private _document, 
        @Inject(PLATFORM_ID) private _platform_id) {  }

   /**
    * Inject the State into the bottom of the <head>
    */      
    addTag(tag: LinkDefinition, forceCreation? : boolean) 
    {
        try { 

            const renderer = this._rendererFactory.createRenderer(this._document, { 
                    id: '-1',
                    encapsulation: ViewEncapsulation.None,
                    styles: [],
                    data: {}
            });

            const link = renderer.createElement('link'); 
            const head = this._document.head; 
            const selector = this._parseSelector(tag); 

            if(head === null) { 
                throw new Error('<head> not found within DOCUMENT.'); 
            }

            Object.keys(tag).forEach((prop: string) => { 
                return renderer.setAttribute(link, prop, tag[prop]);
            });

            // [TODO]: update the existing one if it exists?
            renderer.appendChild(head, link); 

        } catch(e) { 
            console.error('Error within LinkService : ', e);
        }
    }

    private _parseSelector(tag: LinkDefinition) : string { 

        const attr: string = tag.rel ? 'rel' : 'hreflang'; 
        return `${attr}="${tag[attr]}"`;
    }
}

export declare type LinkDefinition = { 
    charset?: string; 
    crossorigin?: string; 
    href?: string; 
    hreflang?: string; 
    media?: string; 
    rel?: string; 
    rev?: string; 
    sizes?: string; 
    target?: string; 
    type?: string;
} & { 
    [prop: string]: string; 
};