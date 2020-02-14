import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PageAboutComponent } from './page-about/page-about.component'
import { PageLoginComponent } from './page-login/page-login.component'
import { PageRegisterComponent } from './page-register/page-register.component'
import { PageDashboardComponent } from './page-dashboard/page-dashboard.component'
import { PageSettingsComponent } from './page-settings/page-settings.component'
import { PageLogoutComponent } from './page-logout/page-logout.component'


const routes: Routes = [
  {
    path: '',
    component: PageAboutComponent
  },
  {
    path: 'login',
    component: PageLoginComponent
  },
  {
    path: 'register',
    component: PageRegisterComponent
  },
  {
    path: 'dashboard',
    component: PageDashboardComponent
  },
  {
    path: 'settings',
    component: PageSettingsComponent
  },
  {
    path: 'logout',
    component: PageLogoutComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
