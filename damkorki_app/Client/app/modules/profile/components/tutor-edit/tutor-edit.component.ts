import { Component, Input } from '@angular/core'; 
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AuthService } from '../../../../services/auth.service';
import { ITutor } from '../../../../models/tutor.model';
import { IPerson } from '../../../../models/person.model';
import { TutorService } from '../../../../services/tutor.service';
import { PersonService } from '../../../../services/person.service';

@Component({
    selector: 'tutor-edit', 
    templateUrl: 'tutor-edit.component.html', 
    styleUrls: ['./tutor-edit.component.css'],
})
export class TutorEditComponent { 

    private person : IPerson = null; 
    private tutor : ITutor = null; 

    constructor(private tutorService : TutorService,
                private personService : PersonService,
                private authService : AuthService) { 
                
        this.loadCurrentTutor(); 
    }

    private loadCurrentTutor() 
    { 
        this.personService.getPersonByUser(this.authService.decodedAccessToken().uid)
                    .subscribe( (person) => { 
                        this.person = person; 
                        // load current tutor
                        this.tutorService.getTutorEagerlyByPerson(person.personId)
                            .subscribe( (tutor) => { 
                                this.tutor = tutor;
                            }, (error) => { 
                                this.tutor = <ITutor>{}; 
                            });
                    }, (error) => { 
                        this.person = null; 
                    });
    }

    onStatusChange(event) {  
        console.log(event); 
    }

    get isProfileCreated() { return this.person != null; }
}