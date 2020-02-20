import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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

  constructor(private router: Router, private http: HttpClient) { }

  loginForm: FormGroup;
  username: FormControl;
  password: FormControl;
  rememberme: FormControl;

  createFormControls() {
    this.username = new FormControl("", [
      Validators.required,
      Validators.minLength(5)
    ]);
    this.password = new FormControl("", [
      Validators.required,
      Validators.minLength(8)
    ]);
    this.rememberme = new FormControl("");
  }

  createForm() {
    this.loginForm = new FormGroup({
      username: this.username,
      password: this.password,
      rememberme: this.rememberme
    });
  }

  login() {
    if (this.loginForm.valid) {
      console.log("Form Submitted!");
      console.log(this.loginForm.value);

      let credentials = JSON.stringify(this.loginForm.value);
      console.log(credentials);
      this.http.post("http://localhost:5000/api/auth/login", credentials, {
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

  ngOnInit() {
    this.createFormControls();
    this.createForm();
  }

}
