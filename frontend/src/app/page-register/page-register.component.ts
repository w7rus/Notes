import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-page-register',
  templateUrl: './page-register.component.html',
  styleUrls: ['./page-register.component.css'],
  host: {
    class:'d-flex',
    style:'height: 100%;'
  }
})
export class PageRegisterComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
