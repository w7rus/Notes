import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-page-dashboard',
  templateUrl: './page-dashboard.component.html',
  styleUrls: ['./page-dashboard.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageDashboardComponent implements OnInit {

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private jwtHelper: JwtHelperService) { }

  notesArray: any[];
  selectedNote: number  = -1;

  addForm: FormGroup;
  updateForm: FormGroup;

  get f() { return this.updateForm.controls; }
  get g() { return this.addForm.controls; }

  //completed
  refreshNotes() {
    this.getNotes(true)
  }

  //completed
  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt")
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true
    else
      return false
  }

  //completed
  selectNote(id: number) {
    this.selectedNote = id;
    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/notes/" + id, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        let data = (<{id: number, title: string, body: string, userId: number, user: null}>response)

        this.f.title.setValue(data.title)
        this.f.body.setValue(data.body)
      }, err => {
        console.log(err)
      });
    }
  }

  //completed
  updateSelectedNote() {
    if (this.isUserAuthenticated()) {
      this.http.put("http://localhost:5000/api/notes/" + this.selectedNote, {
        "title": this.f.title.value,
        "body": this.f.body.value
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.selectedNote = -1
        this.f.title.reset()
        this.f.body.reset()
        this.refreshNotes()
      }, err => {
        console.log(err)
      });
    }
  }

  //completed
  deleteNote(id: number) {
    if (id == this.selectedNote)
    {
      this.selectedNote = -1;
      this.f.title.reset();
      this.f.body.reset();
    }
    if (this.isUserAuthenticated()) {
      this.http.delete("http://localhost:5000/api/notes/" + id, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.refreshNotes()
      }, err => {
        console.log(err)
      });
    }
  }

  //completed
  getNotes(updateGlobal?: boolean): any {
    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/notes", {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        if (updateGlobal)
          this.notesArray = Object.values(response);
        return Object.values(response);
      }, err => {
        if (updateGlobal)
          this.notesArray = [];
        console.log(err)
        return [];
      });
    }
  }

  //completed
  addNoteClearFields() {
    this.g.title.reset();
    this.g.body.reset();
  }

  //completed
  addNote() {
    if (this.isUserAuthenticated()) {
      this.http.post("http://localhost:5000/api/notes/", {
        "title": this.g.title.value,
        "body": this.g.body.value
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.g.title.reset();
        this.g.body.reset();
        this.refreshNotes();
      }, err => {
        console.log(err)
      });
    }
  }

  //completed
  ngOnInit(): void {
    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/notes", {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.notesArray = Object.values(response)
      }, err => {
        console.log(err)
        this.notesArray = [];
      });
    }

    this.updateForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]],
    });

    this.addForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]],
    });
  }

}
