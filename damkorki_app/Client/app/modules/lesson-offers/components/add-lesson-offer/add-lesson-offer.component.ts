import { Component } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ITutor } from "../../../../models/tutor.model";
import { TutorService } from "../../../../services/tutor.service";
import { PersonService } from "../../../../services/person.service";
import { AuthService } from "../../../../services/auth.service";
import { ISubject } from "../../../../models/subject.model";
import { SubjectService } from "../../../../services/subject.service";
import { ILessonOffer } from "../../../../models/lesson-offer.model";
import { LessonOfferService } from "../../../../services/lesson-offer.service";
import { LevelType } from "../../../../models/enums/level-type.enum";

interface ISubjectView { 
    subjectId : number; 
    name : string; 
    displayName : string; 
}

@Component({
    selector: 'add-lesson-offer', 
    templateUrl: 'add-lesson-offer.component.html', 
    styleUrls: ['./add-lesson-offer.component.css']
})
export class AddLessonOfferComponent { 

    tutor : ITutor = null; 
    subjectViews : Array<ISubjectView> = null; 

    lessonOfferForm : FormGroup = null; 

    updateFailed : boolean = false; 
    updateMessage : string = null; 

    constructor(private fb : FormBuilder, 
                private lessonOfferService : LessonOfferService, 
                private subjectService : SubjectService, 
                private personService : PersonService, 
                private tutorService : TutorService, 
                private authService : AuthService) { 

        this.loadCurrentTutor(); 
        this.loadSubjects(); 
        this.createForm(); 
    }

    private loadCurrentTutor() { 

        this.personService.getPersonByUser(this.authService.decodedAccessToken().uid)
                .subscribe( (person) => { 
                    this.tutorService.getTutorByPerson(person.personId)
                            .subscribe( (tutor) => { 
                                this.tutor = tutor; 
                            });
                });
    }

    private loadSubjects() { 

        this.subjectService.getSubjectsNested()
                .subscribe( (subjects) => { 
                    this.subjectViews = this.makeSubjectViews(subjects, ""); 
                });
    }

    private makeSubjectViews(subjects : Array<ISubject>, prefix : string) : Array<ISubjectView> { 

        if(subjects == null)
            return []; 

        var subjectViews : Array<ISubjectView> = []; 

        for(let s of subjects) { 
            subjectViews.push({ subjectId: s.subjectId, name: s.name, displayName: prefix + " " + s.name } );
            subjectViews.push(...this.makeSubjectViews(s.subSubjects, prefix + "-")); 
        }
        
        return subjectViews; 
    } 

    private createForm() { 

        this.lessonOfferForm = this.fb.group({
            subject: ['', Validators.required], 
            title: ['', Validators.required], 
            description: '', 
            cost: ['', Validators.compose([
                            Validators.required, 
                            Validators.min(0),
                            Validators.max(1000)
                        ])], 
            lessonType: ['None', Validators.required], 
            location: ['', Validators.required], 
            level: [1, Validators.required]
        });
    }

    selectLessonType(value) { 
        this.lessonOfferForm.patchValue({ lessonType : value }); 
    }

    onSubmit(event) { 
        event.preventDefault(); 
        
        if(!this.lessonOfferForm.valid) { 
            this.updateFailed = true; 
            this.lessonOfferForm.setErrors({
                update : "Lesson offer form is invalid."
            });
            return; 
        } 

        // build a temporary lesson offer object from form values 
        var tempLessonOffer = <ILessonOffer>{}; 
        tempLessonOffer.title = this.title.value; 
        tempLessonOffer.description = this.description.value; 
        tempLessonOffer.cost = this.cost.value; 
        tempLessonOffer.type = this.lessonType.value;
        tempLessonOffer.location = this.location.value; 
        tempLessonOffer.level = this.level.value;
        tempLessonOffer.subjectId = this.subject.value; 
        tempLessonOffer.tutorId = this.tutor.tutorId;  

        console.log(tempLessonOffer);

        // create lesson offer 
        this.lessonOfferService.createLessonOffer(tempLessonOffer)
                .subscribe( (lessonOffer) => { 
                    console.log("New lesson offer with id " + lessonOffer.lessonOfferId + " has been created."); 
                    this.updateFailed = false; 
                    this.updateMessage = "Lesson offer has been created successfully."; 
                    this.resetForm(); 
                }, (error) => { 
                    console.log(error); 
                    this.updateFailed = true; 
                    this.lessonOfferForm.setErrors({
                        update: "Lesson offer creation has failed."
                    })
                });
    }

    resetForm() { 
        this.createForm(); 
    }

    get isTutor() { return this.tutor != null; }
    get isUpdateMessage() { return this.updateMessage != null; }

    get subject() { return this.lessonOfferForm.get('subject'); }
    get title() { return this.lessonOfferForm.get('title'); }
    get description() { return this.lessonOfferForm.get('description'); } 
    get cost() { return this.lessonOfferForm.get('cost'); }
    get lessonType() { return this.lessonOfferForm.get('lessonType'); }
    get location() { return this.lessonOfferForm.get('location'); }
    get level() { return this.lessonOfferForm.get('level'); }
}