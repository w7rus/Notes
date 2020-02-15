import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-page-login',
  templateUrl: './page-login.component.html',
  styleUrls: ['./page-login.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageLoginComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
