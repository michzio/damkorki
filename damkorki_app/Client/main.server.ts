// Polyfills
import 'zone.js/dist/zone-node';
import './polyfills/server.polyfills'; 
// from angular 2 universal not compatible with angular 4
// import 'angular2-universal-polyfills';
// import 'angular2-universal-patch';
// import 'es6-promise';
// import 'es6-shim'; 
// import 'reflect-metadata';
import { enableProdMode } from '@angular/core';
import { INITIAL_CONFIG } from '@angular/platform-server';
import { APP_BASE_HREF } from '@angular/common'; 
import { createServerRenderer, RenderResult } from 'aspnet-prerendering';
import { ORIGIN_URL } from './app/shared/constants/baseurl.constants'
// from angular 2 universal not compatible with angular 4
// import { platformNodeDynamic } from 'angular2-universal';
// import { AppModule } from './app/app.module';
// Note: instead we have ./app/browser-app.module && ./app/server-app.module
import { ServerAppModule } from './app/server-app.module';
// ASP.NET Core Angular Engine - soon available on npm!
// import { ngAspnetCoreEngine } from '@universal/ng-aspnetore-engine';
// temporary workaround 
import { ngAspnetCoreEngine, IEngineOptions, createTransferScript } from './polyfills/temporary-aspnetcore-engine'; 

// for faster server rendered builds
enableProdMode(); 

// from angular 2 universal not compatible with angular 4
// const platform = platformNodeDynamic();

export default createServerRenderer( (params: BootFuncParams) => {
    
    /* from angular 2 universal not compatible with angular 4 

    return new Promise<RenderResult>((resolve, reject) => {
        const requestZone = Zone.current.fork({
            name: 'angular-universal request',
            properties: {
                baseUrl: '/',
                requestUrl: params.url,
                originUrl: params.origin,
                preboot: false,
                document: '<app></app>'
            },
            onHandleError: (parentZone, currentZone, targetZone, error) => {
                // If any error occurs while rendering the module, reject the whole operation
                reject(error);
                return true;
            }
        });

        return requestZone.run<Promise<string>>(() => platform.serializeModule(AppModule)).then(html => {
            resolve({ html: html });
        }, reject);
    });*/

    /*
     * How can we access data we passed from .NET?
     * you'd access it directly from `params.data` under the name you passed it
     * i.e.: params.data.WHATEVER_YOU_PASSED
     * -------
     * We'll show in the next section WHERE you pass this "data"" in on the .NET side
     */

    // Platform-server provider configuration
    const setupOptions: IEngineOptions = {
      appSelector: '<app></app>',
      ngModule: ServerAppModule,
      request: params,
      providers: [
       /*  Optional: any other providers you want to pass into the App would go here
        *  { provider: CookieService, useClass: ServerCookieService }
        *  i.e.: Just an example of Dependency injecting a class for providing Cookies 
        *  (that you passed down from the server)
        *  (Where on the browser you'd have a different class handling cookies normally)
        */
      ]
    };

    /*** Pass in those Providers & your Server NgModule, and that's it! ***/
    return ngAspnetCoreEngine(setupOptions).then(response => {

      // Want to transfer data from Server -> Client?
      // Add transferData to the "response.globals"" object, 
      // and call createTransferScript({}) passing in the Object key/values of data
      // createTransferScript() will JSON Stringify it and return it as a <script>window.TRANSFER_CACHE={}</script>
      // That your browser can pluck and grab the data from
      response.globals.transferData = createTransferScript({
          someData: 'Transfer this to the client on the window.TRANSFER_CACHE {} object',
          fromDotnet: params.data.thisCameFromDotNET // example of data coming from dotnet, in HomeController
      });

      return ({
          html: response.html,
          globals: response.globals
      });
    });
});