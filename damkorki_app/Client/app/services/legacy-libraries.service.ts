import { Injectable, Inject, PLATFORM_ID } from '@angular/core'; 
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable()
export class LegacyLibrariesService { 

    private _L : any; 
    private _jquery : any; 

    public getJquery() { 
        return this._safeGet(() => this._jquery); 
    }

    public getL() { 
        return this._safeGet(() => this._L); 
    }

    constructor(@Inject(PLATFORM_ID) private _platformId : Object) { 
        this._init(); 
    }

    private _init() { 
        if(isPlatformBrowser(this._platformId)) { 
            this._requireLegacyLibraries(); 
        }
    }

    private _requireLegacyLibraries() { 
        this._jquery = require('jquery'); 
        this._L = require('leaflet'); 
    }

    private _safeGet(getCallback : () => any) { 
        if(isPlatformServer(this._platformId)) { 
            throw new Error('invalid access to legacy component on the server');
        }
        return getCallback(); 
    }
}