import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from "@angular/router";

// import custom validator to validate that password and confirm password fields match
import { MustMatch } from './_helpers/must-match.validator';

@Component({
  selector: 'app-page-register',
  templateUrl: './page-register.component.html',
  styleUrls: ['./page-register.component.css'],
  host: {
    class: 'd-flex',
    style: 'height: 100%;'
  }
})

export class PageRegisterComponent implements OnInit {

  invalidLogin: boolean = false;

  constructor(private router: Router, private http: HttpClient, private formBuilder: FormBuilder) {}

  registerForm: FormGroup;

  register() {
    if (this.registerForm.valid) {
      if (this.f.password.value != this.f.passwordRepeat.value)
        return;

      this.http.post("http://localhost:5000/api/auth/register", this.registerForm.value, {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).subscribe(response => {
        this.invalidLogin = false;
        this.registerForm.reset();
        this.router.navigate(["/login"]);
      }, err => {
        console.log(err);
        this.invalidLogin = true;
      });
    }
  }

  get f() { return this.registerForm.controls; }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(5)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordRepeat: ['', Validators.required],
    }, {
      validator: MustMatch('password', 'passwordRepeat')
    });
  }

}
