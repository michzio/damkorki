// Polyfills
import './polyfills/browser.polyfills';
// from angular 2 universal not compatible with angular 4 
// import 'angular2-universal-polyfills/browser';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic'; 
// from angular 2 universal not compatible with angular 4 
// import { platformUniversalDynamic } from 'angular2-universal';
// import { AppModule } from './app/app.module';
// Note: instead we have ./app/browser-app.module && ./app/server-app.module
import { BrowserAppModule } from './app/browser-app.module';

// Update this if you change your root component selector
const rootElemTagName = 'app'; 

// Enable either Hot Module Reloading or production mode
if (module['hot']) {
    module['hot'].accept();
    module['hot'].dispose(() => {
        // Before restarting the app, we create a new root element and dispose the old one
        const oldRootElem = document.querySelector(rootElemTagName);
        const newRootElem = document.createElement(rootElemTagName);
        oldRootElem.parentNode.insertBefore(newRootElem, oldRootElem);
        
       modulePromise.then(appModule => appModule.destroy());
    });
} else {
    enableProdMode();
}

const modulePromise = platformBrowserDynamic().bootstrapModule(BrowserAppModule);