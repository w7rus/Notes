import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

interface Note {
  id: number,
  title: string,
  body: string
}

@Component({
  selector: 'app-page-notes-new',
  templateUrl: './page-notes-new.component.html',
  styleUrls: ['./page-notes-new.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageNotesNewComponent implements OnInit {
  noteForm: FormGroup;

  noteUpdateSuccess: boolean = false;
  noteUpdateFail: boolean = false;
  noteUpdateFailString: string = "";

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  //Auth
  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true;
    else
      return false;
  }

  //Note
  addNote() {
    // Get Note
    this.http.post("http://localhost:5000/api/notes", {
      title: this.noteForm.controls.title.value,
      body: this.noteForm.controls.body.value
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.router.navigate(["/notes"])
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

  //Init
  ngOnInit(): void {
    this.noteForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]]
    });
  }
}
