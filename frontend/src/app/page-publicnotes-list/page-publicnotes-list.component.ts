import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

interface Note {
  id: number,
  title: string,
  body: string
}

@Component({
  selector: 'app-page-publicnotes-list',
  templateUrl: './page-publicnotes-list.component.html',
  styleUrls: ['./page-publicnotes-list.component.css']
})
export class PagePublicnotesListComponent implements OnInit {
  noteForm: FormGroup;
  noteFilterForm: FormGroup;
  notePaginationForm: FormGroup;
  noteCountPerPage: number[] = [5, 10, 20, 30, 40, 50]

  noteList: Note[];
  noteCount: number = 0;
  notePageSelected: number = 0;
  notePageCount: number = 0;

  noteCountDisplay: number = 0;
  noteCountTotal: number = 0;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder
  ) { }

  //Note
  readNotesCount() {
    this.http.post("http://localhost:5000/api/publicnotes/countFiltered", {
      "search": this.noteFilterForm.controls.title.value
    }, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteCount = <number>response;
      this.notePageCount = Math.ceil(this.noteCount / parseInt(this.notePaginationForm.controls.display.value));
    }, err => {
      console.log(err)
    });
  }

  readNotesCountTotal() {
    this.http.get("http://localhost:5000/api/publicnotes/count", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.noteCountTotal = <number>response;
    }, err => {
      console.log(err)
    });
  }

  readNotes() {
    this.http.post("http://localhost:5000/api/publicnotes/list", {
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

    this.readNotesCount();
    this.readNotesCountTotal();
    this.readNotes();
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
      display: [10, [Validators.required, Validators.min(5), Validators.max(50)]],
    });

    this.searchNotes();
  }
}
