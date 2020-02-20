import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from "@angular/router";

@Component({
  selector: 'app-page-login',
  templateUrl: './page-login.component.html',
  styleUrls: ['./page-login.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})

export class PageLoginComponent implements OnInit{

  invalidLogin: boolean = false;

  constructor(private router: Router, private http: HttpClient, private formBuilder: FormBuilder) { }

  loginForm: FormGroup;

  login() {
    if (this.loginForm.valid) {
      this.http.post("http://localhost:5000/api/auth/login", this.loginForm.value, {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).subscribe(response => {
        async function updateLocalStorage(callback?: Function) {
          await localStorage.setItem("jwt", (<any>response).token);
          await localStorage.setItem("username", (<any>response).username)
          await localStorage.setItem("userid", (<any>response).userID)
          if (callback) await callback();
        }

        updateLocalStorage(() => {
          this.invalidLogin = false;
          this.loginForm.reset();
          this.router.navigate(["/"]);
        })
      }, err => {
        this.invalidLogin = true;
      });
    }
  }

  get f() { return this.loginForm.controls; }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(5)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      rememberme: [''],
    });
  }

}
