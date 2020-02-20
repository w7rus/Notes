import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {

  loggedIn: boolean = false
  username: string = "unknown"
  userid: number = -1

  constructor() { }

  ngOnInit(): void {
    if (localStorage.getItem("jwt") != null)
      this.loggedIn = true
    if (localStorage.getItem("username") != null)
      this.username = localStorage.getItem("username")
    if (localStorage.getItem("userid") != null)
      this.userid = parseInt(localStorage.getItem("userid"))
  }
}
