import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpEventType } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

interface Share {
  username: string,
  userId: number,
  level: number
}

interface Note {
  id: number,
  title: string,
  body: string
}

interface Attachment {
  id: number,
  filename: string
}

@Component({
  selector: 'app-page-publicnotes-edit',
  templateUrl: './page-publicnotes-edit.component.html',
  styleUrls: ['./page-publicnotes-edit.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PagePublicnotesEditComponent implements OnInit {
  noteForm: FormGroup;

  noteId: number = 0;
  noteCanEdit: boolean = false;
  noteUpdateSuccess: boolean = false;
  noteUpdateFail: boolean = false;
  noteUpdateFailString: string = "";

  share: Share;

  attachmentList: Attachment[];
  attachmentListCount: number = 0;
  attachmentUploadSuccess = false;
  attachmentUploadSuccessString = "";
  attachmentUploadFail = false;
  attachmentUploadFailString = "";
  attachmentUploadProgress: number = 0;
  attachmentUploadProgressString: string = "Upload File";

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private activateRoute: ActivatedRoute,
    private router: Router
  ) {
    this.noteId = this.activateRoute.snapshot.params['id'];
  }

  //Note
  readNote() {
    // Get Note
    this.http.get("http://localhost:5000/api/publicnotes/" + this.noteId, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      let data = <Note>response;
      this.noteForm.controls.title.setValue(data.title);
      this.noteForm.controls.body.setValue(data.body);
    }, err => {
      console.log(err);
    });
  }

  updateNote() {
    this.http.put("http://localhost:5000/api/publicnotes/" + this.noteId, {
      "title": this.noteForm.controls.title.value,
      "body": this.noteForm.controls.body.value
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteUpdateSuccess = true;
      setTimeout(() => {
        this.noteUpdateSuccess = false;
      }, 5000);
    }, err => {
      this.noteUpdateFail = true;
      this.noteUpdateFailString = err.error;
      setTimeout(() => {
        this.noteUpdateFail = false;
        this.noteUpdateFailString = "";
      }, 5000);
      console.log(err);
    });
  }

  //Shares
  readShares() {
    // Get Shares for Note
    this.http.get("http://localhost:5000/api/publicshares/" + this.noteId, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.share = <Share>response;
      this.noteCanEdit = this.share.level == 2
    }, err => {
      console.log(err);
    });
  }

  //Attachments
  addAttachment(files) {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload);

    this.http.post("http://localhost:5000/api/publicattachment/" + this.noteId, formData, {reportProgress: true, observe: 'events'}).subscribe(response => {
      if (response.type === HttpEventType.UploadProgress) {
        this.attachmentUploadProgress = Math.round(100 * response.loaded / response.total);
        this.attachmentUploadProgressString = "Uploading... " + this.attachmentUploadProgress
      } else if (response.type === HttpEventType.Response) {
        this.attachmentUploadProgressString = "Upload File";
        this.attachmentUploadSuccess = true;
        setTimeout(() => {
          this.attachmentUploadSuccess = false;
        }, 5000);
        this.listAttachments();
      }
    }, err => {
      this.attachmentUploadSuccess = true;
      this.attachmentUploadFail = true;
      this.attachmentUploadFailString = err.error;
      setTimeout(() => {
        this.attachmentUploadFail = false;
        this.attachmentUploadFailString = "";
      }, 5000);
      this.attachmentUploadProgressString = "Upload File";
    });
  }

  listAttachments() {
    // Get Attachment
    this.http.get("http://localhost:5000/api/publicattachment/findAttachments/" + this.noteId, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.attachmentList = <Attachment[]>response;
      this.attachmentListCount = this.attachmentList.length
      console.log(this.attachmentListCount)
    }, err => {
      console.log(err);
    });
  }

  readAttachment(attachmentid: number) {
    window.open("http://localhost:5000/api/attachment/" + attachmentid)
  }

  removeAttachment(attachmentid: number) {
    console.log("removed attachment" + attachmentid)

    // Get Shares for Note
    this.http.delete("http://localhost:5000/api/publicattachment/" + this.noteId + "/" + attachmentid, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.listAttachments();
    }, err => {
      console.log(err);
    });
  }

  //Init
  ngOnInit(): void {
    this.noteForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]]
    });

    this.readNote();
    this.readShares();
    this.listAttachments();
  }
}
