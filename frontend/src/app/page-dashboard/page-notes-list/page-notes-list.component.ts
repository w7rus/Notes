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
  
  addForm: FormGroup;
  searchForm: FormGroup;
  pageForm: FormGroup;

  items: any[];
  itemsCount: number;
  pageSelected: number;
  pageCount: number;
  pageDisplays: number[] = [5, 10, 20, 30, 40, 50]

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

  // getNotes(): any {
  //   if (this.isUserAuthenticated()) {
  //     this.http.get("http://localhost:5000/api/notes", {
  //       headers: new HttpHeaders({
  //         "Content-Type": "application/json"
  //       })
  //     }).subscribe(response => {
  //       this.items = Object.values(response);
  //       return Object.values(response);
  //     }, err => {
  //       this.items = [];
  //       console.log(err)
  //       return [];
  //     });
  //   }
  // }

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
        this.searchNotes();
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
        this.searchNotes();
      }, err => {
        console.log(err)
      });
    }
  }

  //--

  searchNotes(page?: number) {
    page = (page === undefined) ? 0 : page

    this.pageSelected = page;

    if (this.isUserAuthenticated()) {
      // Get notes count with specified filters
      this.http.post("http://localhost:5000/api/notes/countFiltered", {
        "search": this.searchForm.controls.notesSearch.value,
        "sorting": (this.searchForm.controls.notesSorting.value == "Ascending") ? 0 : 1,
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.itemsCount = <number>response;
        this.pageCount = Math.ceil(this.itemsCount / parseInt(this.pageForm.controls.notesDisplay.value));
      }, err => {
        console.log(err)
        this.itemsCount = 0;
        this.pageCount = Math.ceil(this.itemsCount / parseInt(this.pageForm.controls.notesDisplay.value));
      });

      // Display required page with specified filters
      this.http.post("http://localhost:5000/api/notes/list", {
        "search": this.searchForm.controls.notesSearch.value,
        "sorting": (this.searchForm.controls.notesSorting.value == "Ascending") ? 0 : 1,
        "display": parseInt(this.pageForm.controls.notesDisplay.value),
        "page": this.pageSelected,
      }, {
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
  }

  ngOnInit(): void {
    this.addForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]],
    });
    this.searchForm = this.formBuilder.group({
      notesSearch: ['', []],
      notesSorting: ['Ascending', [Validators.required]],
    });
    this.pageForm = this.formBuilder.group({
      notesDisplay: [10, [Validators.required, Validators.min(5), Validators.max(50)]],
    });

    this.searchNotes(0);
  }

}
