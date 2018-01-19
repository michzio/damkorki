import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'footer-bar', 
    templateUrl: './footer-bar.component.html', 
    styleUrls: ['./footer-bar.component.css']
})
export class FooterBarComponent implements OnInit { 

    ngOnInit() { 
        console.log("Loading footer-bar...");
    }
}