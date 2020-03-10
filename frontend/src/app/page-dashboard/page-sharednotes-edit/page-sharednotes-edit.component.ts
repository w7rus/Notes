import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';

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

  id: number;
  updateForm: FormGroup;
  canEdit: boolean;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private jwtHelper: JwtHelperService,
    private activateRoute: ActivatedRoute,
    private router: Router
  ) { 
    this.id = this.activateRoute.snapshot.params['id'];
  }

  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token))
      return true;
    else
      return false;
  }

  updateNote() {
    if (this.isUserAuthenticated()) {
      this.http.put("http://localhost:5000/api/sharednotes/" + this.id, {
        "title": this.updateForm.controls.title.value,
        "body": this.updateForm.controls.body.value
      }, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        this.updateForm.controls.title.reset();
        this.updateForm.controls.body.reset();
        this.router.navigate(["/dashboard/sharednotes"]);
      }, err => {
        console.log(err);
      });
    }
  }

  ngOnInit(): void {
    this.updateForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      body: ['', [Validators.required]]
    });

    if (this.isUserAuthenticated()) {
      this.http.get("http://localhost:5000/api/sharednotes/" + this.id, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        let data = (<{
          id: number,
          title: string,
          body: string,
          sharedUsersData: [{
            username: string,
            userId: number,
            level: number
          }]
        }>response);

        this.updateForm.controls.title.setValue(data.title);
        this.updateForm.controls.body.setValue(data.body);
        this.canEdit = data.sharedUsersData.find(x=>x!==undefined).level > 1;
      }, err => {
        console.log(err);
        this.router.navigate(["/dashboard/sharednotes"]);
      });
    }
  }

}
