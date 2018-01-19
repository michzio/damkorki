import {Component, OnInit, Inject} from '@angular/core';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

import { LegacyLibrariesService } from '../../../../services/legacy-libraries.service'; 

@Component({
    selector: 'leaflet-map',
    templateUrl: 'leaflet-map.component.html',
    styleUrls: ['leaflet-map.component.css'],
    providers: [
        LegacyLibrariesService
    ]
})
export class LeafletMapComponent implements OnInit { 

    constructor(@Inject(PLATFORM_ID) private _platformId: Object, private _legacyLibraries : LegacyLibrariesService) { 
        
     }

    ngAfterViewInit() { 

        if (isPlatformBrowser(this._platformId)) {

             console.log("Rendering leaflet map...");

             var map = this._legacyLibraries.getL()
                                            .map('leafletMap'); //.setView([50.080, 19.93], 13);
             map.locate({setView: true, maxZoom: 16}); // geolocation

             // create the tile layer with correct attribution
	         var osmUrl='http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
             var osmAttrib='Map data © <a href="http://openstreetmap.org">OpenStreetMap</a> contributors';
             var mapboxUrl = 'https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw';
             var mapboxAttrib = 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
			                                                    '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
			                                                    'Imagery © <a href="http://mapbox.com">Mapbox</a>';

             this._legacyLibraries.getL().tileLayer(osmUrl, {
                                                    minZoom: 8,
		                                            maxZoom: 18,
		                                            attribution: osmAttrib,
		                                            id: 'mapbox.streets'
	                                }).addTo(map);        

               


             this._legacyLibraries.getL()
                                  .tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
                                              attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                                   }).addTo(map);

             this._legacyLibraries.getL()
                                  .marker([50.0817444,19.9253805]).addTo(map)
                                  .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
                                  .openPopup();

             var marker = this._legacyLibraries.getL().marker([50.0817444,20.9253805]).addTo(map);

             var circle = this._legacyLibraries.getL().circle([51.508, 18.9253805], {
                                    color: 'blue',
                                    fillColor: 'blue',
                                    fillOpacity: 0.5,
                                    radius: 500
                                }).addTo(map);

              var polygon = this._legacyLibraries.getL().polygon([
                                        [51.509, 18.9253805],
                                        [52.503, 19.3253805],
                                        [53.51, 19.9253805],
                                        [52.509, 19.1253805],
                                        [51.503, 18.3253805],
                                    ]).addTo(map);
             marker.bindPopup("<b>Hello world!</b><br>I am a popup.").openPopup();
             circle.bindPopup("I am a circle.");
             polygon.bindPopup("I am a polygon.");

             var popup = this._legacyLibraries.getL().popup(); 
             map.on('click', function(e) { 
                     popup
                    .setLatLng(e.latlng)
                    .setContent("You clicked the map at " + e.latlng.toString())
                    .openOn(map);
        
             });
         }

        if (isPlatformServer(this._platformId)) {
            // Server only code.
            // https://github.com/angular/universal#universal-gotchas
        }
    }

    ngOnInit() {  
       
    }
  
}