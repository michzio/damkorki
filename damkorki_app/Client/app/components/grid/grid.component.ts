import { Component, OnInit, Input, ElementRef, Injector, ViewChild, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'grid', 
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit, AfterViewChecked { 

    private GRID_CARD_MARGIN_LEFT = 15; // [px]

    protected _elementRef:ElementRef;
    protected _changeDetectorRef:ChangeDetectorRef; 

    @ViewChild('gridElement') gridElement: ElementRef;

    // it's generic to all grids 
    @Input() title? : string; // <- optionaly grid can have a title 
    @Input('max-cols') maxCols : number = 5; // <- default max number of cols
    @Input('max-rows') maxRows : number = 5; // <- default max number of rows 
    @Input('min-card-width') minCardWidth : number; 
    @Input() contentObjects : any[] = []; // <- get array of any json object 

    public contentObjectsObservable : Observable<any>; 
; 

    public gridCardWidth = 0;
    public gridCols = this.maxCols;   

    constructor(injector: Injector) { 
        this._elementRef = injector.get(ElementRef); 
        this._changeDetectorRef = injector.get(ChangeDetectorRef); 
    }

    ngOnInit() { 
        console.log("My dummy objects", this.contentObjects); 
        // console.log(this._elementRef.nativeElement);
        // console.log("Grid Element width:", this.gridElement.nativeElement.offsetWidth); 
        this.adjustCardWidthToAvailableEstate();
        this.contentObjectsObservable = Observable.from(this.contentObjects); 
    }


    ngAfterViewChecked() { 
        if(this.gridElement == null) return; 
        // console.log("ngAfterViewChecked: Grid Element width:", this.gridElement.nativeElement.offsetWidth); 
        this.adjustCardWidthToAvailableEstate();
        this._changeDetectorRef.detectChanges();
        
    }

    onResize(event) {
        // console.log("Inner window width:", event.target.innerWidth);
        // console.log("windows:resize: Grid Element width:", this.gridElement.nativeElement.offsetWidth); 
        this.adjustCardWidthToAvailableEstate(); 
    }

    adjustCardWidthToAvailableEstate() { 
        var gridWidth : number = this.gridElement.nativeElement.offsetWidth; // [px]
        var newGridCardWidth = this.calculateCardWidth(gridWidth); // [px]
        this.gridCardWidth = ((newGridCardWidth - this.GRID_CARD_MARGIN_LEFT)/gridWidth)*100; // [%]
        this.gridCols = Math.floor(gridWidth/newGridCardWidth);

        // refresh list of cards based on content objects
        this.contentObjectsObservable = Observable.from(this.contentObjects);
    }

    calculateCardWidth(gridWidth : number) : number { 

        var cols =  this.maxCols > this.contentObjects.length ? this.contentObjects.length : this.maxCols; 

        var cardWidth : number = Math.floor(gridWidth / cols); 
        if(cardWidth < this.minCardWidth) { 
            console.log("Can not use max cols number, recalculate number of cols based on min width!"); 
            // max number of columns doesn't fit into available width of entire grid element 
            // adjust number of columns to min card width
             var cols : number = Math.floor(gridWidth / (this.minCardWidth + this.GRID_CARD_MARGIN_LEFT)); 
             cardWidth = Math.floor(gridWidth / cols); 
        }
        return cardWidth; 
    }

    trackByFn(index: number, content: Object) { 
        // console.log("Track by function for: " + index + " and object: " + content);
    }
}
