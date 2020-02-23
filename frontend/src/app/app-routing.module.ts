import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PageHomeComponent } from './page-home/page-home.component'
import { PageAboutComponent } from './page-about/page-about.component'
import { PageLoginComponent } from './page-login/page-login.component'
import { PageRegisterComponent } from './page-register/page-register.component'
import { PageDashboardComponent } from './page-dashboard/page-dashboard.component'
import { PageSettingsComponent } from './page-settings/page-settings.component'
import { PageLogoutComponent } from './page-logout/page-logout.component'

import { NotFoundComponent } from './not-found/not-found.component'

import { AuthGuard } from './_guards/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component: PageHomeComponent
  },
  {
    path: 'about',
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
    component: PageDashboardComponent,
    canActivate: [AuthGuard] 
  },
  {
    path: 'settings',
    component: PageSettingsComponent
  },
  {
    path: 'logout',
    component: PageLogoutComponent
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  },
  {
    path: '**',
    redirectTo: 'not-found'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }