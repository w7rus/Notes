import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {
  constructor(private jwtHelper: JwtHelperService) { }

  isUserAuthenticated() {
    let token: string = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    else {
      return false;
    }
  }

  getUsername() {
    let username: string = localStorage.getItem("username")
    if (username)
      return username
    else
      return "unknown"
  }

  getUserid() {
    let userid: string = localStorage.getItem("userid")
    if (userid)
      return userid
    else
      return "unknown"
  }

  ngOnInit(): void {}
}
