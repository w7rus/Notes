import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

interface Note {
  id: number,
  title: string,
  body: string
}

@Component({
  selector: 'app-page-notes-list',
  templateUrl: './page-notes-list.component.html',
  styleUrls: ['./page-notes-list.component.css']
})
export class PageNotesListComponent implements OnInit {
  noteForm: FormGroup;
  noteFilterForm: FormGroup;
  notePaginationForm: FormGroup;
  noteCountPerPage: number[] = [1, 5, 10]

  noteList: Note[];
  noteCount: number = 0;
  notePageSelected: number = 0;
  notePageCount: number = 0;
  notePageSelectionOptions: number[] = [];

  noteCountDisplay: number = 0;
  noteCountTotal: number = 0;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private jwtHelper: JwtHelperService
  ) { }

  //Auth
  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt")
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true
    else
      return false
  }

  //Note
  addNote() {
    if (this.isUserAuthenticated()) {
      this.http.post("http://localhost:5000/api/notes/", {
        "title": this.noteForm.controls.title.value,
        "body": this.noteForm.controls.body.value
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.noteForm.controls.title.reset();
        this.noteForm.controls.body.reset();
        this.searchNotes();
      }, err => {
        console.log(err)
      });
    }
  }

  deleteNote(noteid: number) {
    if (this.isUserAuthenticated()) {
      this.http.delete("http://localhost:5000/api/notes/" + noteid, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.searchNotes();
      }, err => {
        console.log(err)
      });
    }
  }

  readNotesCountTotal() {
    this.http.get("http://localhost:5000/api/notes/count", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteCountTotal = <number>response;
    }, err => {
      console.log(err)
    });
  }

  readNotesCount(page: number) {
    this.http.post("http://localhost:5000/api/notes/countFiltered", {
      "search": this.noteFilterForm.controls.title.value
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteCount = <number>response;
      this.notePageCount = Math.ceil(this.noteCount / parseInt(this.notePaginationForm.controls.display.value));

      this.notePageSelectionOptions = []
      for (let index = -1; index <= 3; index++) {
        if (page + index > 0 && page + index <= this.notePageCount){
          this.notePageSelectionOptions.push(page + index);
        }
      }
      this.notePageSelectionOptions = [...this.notePageSelectionOptions];
    }, err => {
      console.log(err)
    });
  }

  readNotes() {
    this.http.post("http://localhost:5000/api/notes/list", {
      "search": this.noteFilterForm.controls.title.value,
      "sorting": (this.noteFilterForm.controls.sorting.value == "Ascending") ? 0 : 1,
      "display": parseInt(this.notePaginationForm.controls.display.value),
      "page": this.notePageSelected,
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteList = <Note[]>response;
      this.noteCountDisplay = this.noteList.length
    }, err => {
      console.log(err)
      this.noteList = [];
    });
  }

  searchNotes(page?: number) {
    page = (page === undefined) ? 0 : page
    this.notePageSelected = page;

    if (this.isUserAuthenticated()) {
      this.readNotesCountTotal();
      this.readNotesCount(page);
      this.readNotes();
    }
  }

  //Init
  ngOnInit(): void {
    this.noteForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]],
    });
    this.noteFilterForm = this.formBuilder.group({
      title: ['', []],
      sorting: ['Ascending', [Validators.required]],
    });
    this.notePaginationForm = this.formBuilder.group({
      display: [5, []],
    });

    this.searchNotes();
  }
}
