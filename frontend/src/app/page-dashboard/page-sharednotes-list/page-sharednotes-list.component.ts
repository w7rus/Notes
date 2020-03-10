import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-page-sharednotes-list',
  templateUrl: './page-sharednotes-list.component.html',
  styleUrls: ['./page-sharednotes-list.component.css']
})
export class PageSharednotesListComponent implements OnInit {

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

  searchNotes(page?: number) {
    page = (page === undefined) ? 0 : page

    this.pageSelected = page;

    if (this.isUserAuthenticated()) {
      // Get notes count with specified filters
      this.http.post("http://localhost:5000/api/sharednotes/countFiltered", {
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
      this.http.post("http://localhost:5000/api/sharednotes/list", {
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
