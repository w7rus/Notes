import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-logout',
  templateUrl: './page-logout.component.html',
  styleUrls: ['./page-logout.component.css']
})
export class PageLogoutComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
    async function clearLocalStorage(callback?: Function) {
      if (localStorage.getItem("jwt") != null)
        await localStorage.removeItem("jwt");
      if (localStorage.getItem("username") != null)
        await localStorage.removeItem("username");
      if (localStorage.getItem("userid") != null)
        await localStorage.removeItem("userid");
      if (callback) await callback();
    }

    clearLocalStorage(() => {
      this.router.navigate(["/"]);
    })
  }

}
