import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-page-notes-list',
  templateUrl: './page-notes-list.component.html',
  styleUrls: ['./page-notes-list.component.css']
})
export class PageNotesListComponent implements OnInit {

  items: any[];
  addForm: FormGroup;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private jwtHelper: JwtHelperService
  ) { }

  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt")
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true
    else
      return false
  }

  getNotes(): any {
    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/notes", {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.items = Object.values(response);
        return Object.values(response);
      }, err => {
        this.items = [];
        console.log(err)
        return [];
      });
    }
  }

  addNoteModalReset() {
    this.addForm.controls.title.reset();
    this.addForm.controls.body.reset();
  }

  addNote() {
    if (this.isUserAuthenticated()) {
      this.http.post("http://localhost:5000/api/notes/", {
        "title": this.addForm.controls.title.value,
        "body": this.addForm.controls.body.value
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.addNoteModalReset();
        this.getNotes();
      }, err => {
        console.log(err)
      });
    }
  }

  deleteNote(id: number) {
    if (this.isUserAuthenticated()) {
      this.http.delete("http://localhost:5000/api/notes/" + id, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.getNotes()
      }, err => {
        console.log(err)
      });
    }
  }

  ngOnInit(): void {
    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/notes", {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.items = Object.values(response);
      }, err => {
        console.log(err)
        this.items = [];
      });
    }

    this.addForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]],
    });
  }

}
