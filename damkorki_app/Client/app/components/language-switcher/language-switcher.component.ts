import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({ 
    selector: 'language-switcher', 
    templateUrl: './language-switcher.component.html',
    styleUrls: ['./language-switcher.component.css']
})
export class LanguageSwitcherComponent implements OnInit { 

    public langs : string[] = ['en', 'pl', 'de', 'fr', 'es', 'ru'];
    public isOpen = false; 
    public useLang : string; 

    constructor(private _translate : TranslateService) { 

        var defaultLang : string = this.langs.indexOf(this._translate.getBrowserLang()) < 0 ? 'en' : this._translate.getBrowserLang();
        this.setLanguage(defaultLang);
    }

    ngOnInit() {
       
    }

    public setLanguage(lang: string) { 
        console.log("Language: " + lang + " has been set.");
        this.useLang = lang; 
        this._translate.use(lang); 
    }
}