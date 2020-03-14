import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
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

@Component({
  selector: 'app-page-sharednotes-edit',
  templateUrl: './page-sharednotes-edit.component.html',
  styleUrls: ['./page-sharednotes-edit.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageSharednotesEditComponent implements OnInit {
  noteForm: FormGroup;

  noteId: number = 0;
  noteCanEdit: boolean = false;
  noteUpdateSuccess: boolean = false;
  noteUpdateFail: boolean = false;
  noteUpdateFailString: string = "";

  share: Share;

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
    this.http.get("http://localhost:5000/api/sharednotes/" + this.noteId, {
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
      this.http.put("http://localhost:5000/api/sharednotes/" + this.noteId, {
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
  readShares() {
    // Get Shares for Note
    this.http.get("http://localhost:5000/api/sharedshares/" + this.noteId, {
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

  //Init
  ngOnInit(): void {
    this.noteForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]]
    });

    if (this.isUserAuthenticated()) {
      this.readNote();
      this.readShares();
    }
  }
}
