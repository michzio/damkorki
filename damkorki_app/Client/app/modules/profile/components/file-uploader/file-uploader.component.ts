import { Component, OnInit, Input, Output, HostListener, EventEmitter } from "@angular/core"; 
import { FileService } from "../../../../services/file.service";

export enum UploadEventType { 
    FILES_NOT_FOUND, 
    MAX_FILES_COUNT_EXCEEDED, 
    INCORRECT_FILE_EXTENSION, 
    MAX_FILES_SIZE_EXCEEDED, 
    UPLOAD_SUCCEEDED, 
    UPLOAD_FAILED
}

export interface IUploadEvent {
    type : UploadEventType; 
    success: boolean;
    message : string; 
    response? : any; 
}

@Component({
    selector: 'file-uploader', 
    templateUrl: 'file-uploader.component.html',
    styleUrls: ['./file-uploader.component.css']
})
export class FileUploaderComponent implements OnInit { 

    uploadAreaClass : string = 'drag-area'; 

    @Input('endpoint') endpoint : string = ""; 
    @Input('extensions') fileExtensions: string = "PNG, JPG, JPEG, GIF"; 
    @Input('max-count') maxFilesCount: number = 5; 
    @Input('max-size') maxFileSize: number = 5; // MB
    @Output('uploadchange') uploadChangeEmitter : EventEmitter<any> = new EventEmitter(); 

    constructor(private fileService: FileService) { }

    ngOnInit() { 

    }

    onFileChange(event : Event) { 
        event.preventDefault(); 

        let files = (<any>event.target).files; 
        this.uploadFiles(files); 
    }

    uploadFiles(files : FileList) { 
        
       if(!this.isValidCount(files)) { 
            return; 
       }

       if(!this.isValidExtension(files)) { 
           return; 
       }
       
       if(!this.isValidSize(files)) { 
           return; 
       }

       let formData = new FormData(); 
       for(var i = 0; i < files.length; i++) { 
            formData.append("file[]", files[i], files[i].name); 
            // formData.set(files[i].name, files[i], files[i].name);
       }

       var params = { }; 
       this.fileService.upload(this.endpoint, formData, params)
               .subscribe( (response) => { 
                    this.uploadChangeEmitter.emit({
                        type: UploadEventType.UPLOAD_SUCCEEDED, 
                        success: true, 
                        message: "Upload succeeded", 
                        response: response
                    });
               }, (error) => { 
                    this.uploadChangeEmitter.emit({
                        type: UploadEventType.UPLOAD_FAILED, 
                        success: false, 
                        message: "Upload failed"
                    });
               }); 
    }

    isValidCount(files : FileList) : boolean { 
        // check number of files is not exceeded 
        if(files.length > this.maxFilesCount) { 
            this.uploadChangeEmitter.emit({
                type: UploadEventType.MAX_FILES_COUNT_EXCEEDED,
                success: false,
                message: "You can upload only " + this.maxFilesCount + " files"
            });
            return false; 
        }
        // check there are any files 
        if(files.length < 1) { 
            this.uploadChangeEmitter.emit({
                type: UploadEventType.FILES_NOT_FOUND, 
                success: false, 
                message: "Files not found to upload"
            });
            return false; 
        }
        return true; 
    }

    isValidExtension(files : FileList) : boolean { 
        // get allowed extentions  
        let exts = this.fileExtensions.split(',')
                        .map( (ext) => { return ext.toLowerCase().trim() }); 
        // check files extensions
        for(var i=0; i < files.length; i++) { 
            let ext = files[i].name.split('.').pop().toLowerCase(); 
            if(!exts.includes(ext)) { 
                // incorrect extension
                this.uploadChangeEmitter.emit({
                    type: UploadEventType.INCORRECT_FILE_EXTENSION, 
                    success: false, 
                    message: "You can upload only files with extensions: " + this.fileExtensions
                }); 
                return false; 
            }
        }
        return true; 
    }

    isValidSize(files : FileList) : boolean { 
        // check files size  
        for(var i=0; i<files.length; i++) { 
            let size = files[i].size / (1024*1000); 
            if(size > this.maxFileSize) { 
                this.uploadChangeEmitter.emit({
                    type: UploadEventType.MAX_FILES_SIZE_EXCEEDED, 
                    success: false, 
                    message: "You can upload only files to " + this.maxFileSize + "MB"
                }); 
                return false; 
            }
        }
        return true; 
    }

    //@HostListener('dragover', ['$event'])
    onDragOver(event : Event) { 
        event.preventDefault(); 
        this.uploadAreaClass = "drop-area"
    }

    //@HostListener('dragenter', ['$event'])
    onDragEnter(event : Event) { 
        event.preventDefault(); 
        this.uploadAreaClass = "drop-area"; 
    }

    //@HostListener('dragend', ['$event'])
    onDragEnd(event : Event) { 
        event.preventDefault(); 
        this.uploadAreaClass = "drag-area"; 
    }

    //@HostListener('dragleave', ['$event'])
    onDragLeave(event : Event) { 
        event.preventDefault(); 
        this.uploadAreaClass = "drag-area"; 
    }

    //@HostListener('drop', ['$event'])
    onDrop(event : any) { 
        event.preventDefault(); 
        this.uploadAreaClass = "drag-area"; 

        event.stopPropagation(); 
        var files = event.dataTransfer.files; 
        this.uploadFiles(files); 
    }
}