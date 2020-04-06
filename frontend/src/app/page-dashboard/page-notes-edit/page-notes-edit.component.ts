import { Component, OnInit, Output } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEventType } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';
import { EventEmitter } from 'protractor';

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

interface User {
  id: number,
  username: string,
}

@Component({
  selector: 'app-page-notes-edit',
  templateUrl: './page-notes-edit.component.html',
  styleUrls: ['./page-notes-edit.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageNotesEditComponent implements OnInit {
  noteForm: FormGroup;
  userFilterForm: FormGroup;
  userPaginationForm: FormGroup;
  sharePaginationForm: FormGroup;
  userCountPerPage: number[] = [1, 5, 10];
  shareCountPerPage: number[] = [1, 5, 10];

  noteId: number = 0;
  noteUpdateSuccess: boolean = false;
  noteUpdateFail: boolean = false;
  noteUpdateFailString: string = "";
  shareUpdateSuccess: boolean = false;
  shareUpdateFail: boolean = false;
  shareUpdateFailString: string = "";

  shareList: Share[];
  sharePublic: Share[];
  userList: User[];

  userCount: number = 0;
  userPageSelected: number = 0;
  userPageCount: number = 0;
  shareCount: number = 0;
  sharePageSelected: number = 0;
  sharePageCount: number = 0;

  attachmentUploadProgress: number;
  attachmentUploadProgressString: string;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private jwtHelper: JwtHelperService,
    private activateRoute: ActivatedRoute,
    private router: Router
  ) {
    this.noteId = this.activateRoute.snapshot.params['id'];
  }

  //Auth
  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true;
    else
      return false;
  }

  //Note
  readNote() {
    // Get Note
    this.http.get("http://localhost:5000/api/notes/" + this.noteId, {
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
    if (this.isUserAuthenticated()) {
      this.http.put("http://localhost:5000/api/notes/" + this.noteId, {
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
  }

  //Shares
  addShare(userid: number) {
    let data: Share = {
      username: null,
      userId: userid,
      level: 0
    }

    // Get Shares for Note
    this.http.post("http://localhost:5000/api/shares/" + this.noteId, data, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.shareUpdateSuccess = true;
      setTimeout(() => {
        this.shareUpdateSuccess = false;
      }, 5000);
      this.readShares();
      this.searchUsers();
      this.searchShares();
    }, err => {
      this.shareUpdateFail = true;
      this.shareUpdateFailString = err.error;
      setTimeout(() => {
        this.shareUpdateFail = false;
        this.shareUpdateFailString = "";
      }, 5000);
      console.log(err);
    });
  }

  readSharesCount() {
    // Get Shares for Note
    this.http.get("http://localhost:5000/api/shares/findSharesCount/" + this.noteId, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.shareCount = <number>response;
      this.sharePageCount = Math.ceil(this.shareCount / parseInt(this.sharePaginationForm.controls.display.value));
    }, err => {
      console.log(err);
    });
  }

  readShares() {
    // Get Shares for Note
    this.http.post("http://localhost:5000/api/shares/findShares/" + this.noteId, {
      "display": parseInt(this.sharePaginationForm.controls.display.value),
      "page": this.sharePageSelected,
    },{
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.shareList = <Share[]>response
    }, err => {
      console.log(err);
    });

    // Get Shares for Note
    this.http.get("http://localhost:5000/api/shares/findSharePublic/" + this.noteId,{
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.sharePublic = <Share[]>response
    }, err => {
      console.log(err);
    });
  }

  searchShares(page?: number) {
    page = (page === undefined) ? 0 : page;
    this.sharePageSelected = page;

    if (this.isUserAuthenticated()) {
      this.readSharesCount()
      this.readShares()
    }
  }

  updateShare(userid: number, level: number) {
    let data: Share = {
      username: null,
      userId: userid,
      level: level
    }

    // Get Shares for Note
    this.http.put("http://localhost:5000/api/shares/" + this.noteId, data, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.shareUpdateSuccess = true;
      setTimeout(() => {
        this.shareUpdateSuccess = false;
      }, 5000);
      this.readShares();
      this.searchShares();
    }, err => {
      this.shareUpdateFail = true;
      this.shareUpdateFailString = err.error;
      setTimeout(() => {
        this.shareUpdateFail = false;
        this.shareUpdateFailString = "";
      }, 5000);
      console.log(err);
    });
  }

  deleteShare(userid: number) {
    // Get Shares for Note
    this.http.delete("http://localhost:5000/api/shares/" + this.noteId + "/" + userid, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.shareUpdateSuccess = true;
      setTimeout(() => {
        this.shareUpdateSuccess = false;
      }, 5000);
      this.readShares();
      this.searchShares();
    }, err => {
      this.shareUpdateFail = true;
      this.shareUpdateFailString = err.error;
      setTimeout(() => {
        this.shareUpdateFail = false;
        this.shareUpdateFailString = "";
      }, 5000);
      console.log(err);
    });
  }

  //Users
  readUsersCount() {
    this.http.post("http://localhost:5000/api/shares/findUsersFilteredCount/" + this.noteId, {
      "search": this.userFilterForm.controls.username.value,
      "sorting": 0,
      "display": parseInt(this.userPaginationForm.controls.display.value),
      "page": this.userPageSelected,
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.userCount = <number>response;
      this.userPageCount = Math.ceil(this.userCount / parseInt(this.userPaginationForm.controls.display.value));
    }, err => {
      console.log(err)
    });
  }

  readUsers() {
    this.http.post("http://localhost:5000/api/shares/findUsers/" + this.noteId, {
      "search": this.userFilterForm.controls.username.value,
      "sorting": 0,
      "display": parseInt(this.userPaginationForm.controls.display.value),
      "page": this.userPageSelected,
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.userList = <User[]>response;
    }, err => {
      console.log(err)
    });
  }

  searchUsers(page?: number) {
    page = (page === undefined) ? 0 : page;
    this.userPageSelected = page;

    if (this.isUserAuthenticated()) {
      if ((<string>this.userFilterForm.controls.username.value).length > 0)
      {
        this.readUsersCount()
        this.readUsers()
      } else {
        this.userCount = 0;
        this.userPageCount = Math.ceil(this.userCount / parseInt(this.userPaginationForm.controls.display.value));
        this.userList = [];
      }
    }
  }

  //Attachments
  addAttachment(files) {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload);

    this.http.post("http://localhost:5000/api/attachment/" + this.noteId, formData, {reportProgress: true, observe: 'events'}).subscribe(response => {
      if (response.type === HttpEventType.UploadProgress)
        this.attachmentUploadProgress = Math.round(100 * response.loaded / response.total);
      else if (response.type === HttpEventType.Response) {
        this.attachmentUploadProgressString = "Upload success.";
      }
    }, err => {
      
    });
  }

  readAttachment() {

  }

  removeAttachment() {

  }

  //Init
  ngOnInit(): void {
    this.noteForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]]
    });
    this.userFilterForm = this.formBuilder.group({
      username: ['', [Validators.required]]
    })
    this.userPaginationForm = this.formBuilder.group({
      display: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
    });
    this.sharePaginationForm = this.formBuilder.group({
      display: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
    });

    if (this.isUserAuthenticated()) {
      this.readNote();
      this.readShares();
      this.searchShares();
    }

    console.log(this.sharePublic)
  }
}
