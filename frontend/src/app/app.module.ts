import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { JwtModule } from "@auth0/angular-jwt";

import { AuthGuard } from './_guards/auth-guard.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { PageLoginComponent } from './page-login/page-login.component';
import { PageRegisterComponent } from './page-register/page-register.component';
import { PageNotesListComponent } from './page-dashboard/page-notes-list/page-notes-list.component';
import { PageNotesEditComponent } from './page-dashboard/page-notes-edit/page-notes-edit.component';
import { PageSharednotesListComponent } from './page-dashboard/page-sharednotes-list/page-sharednotes-list.component';
import { PageSharednotesEditComponent } from './page-dashboard/page-sharednotes-edit/page-sharednotes-edit.component';
import { PageNotesNewComponent } from './page-dashboard/page-notes-new/page-notes-new.component';
import { PagePublicnotesListComponent } from './page-publicnotes-list/page-publicnotes-list.component';
import { PagePublicnotesEditComponent } from './page-publicnotes-edit/page-publicnotes-edit.component';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    PageLoginComponent,
    PageRegisterComponent,
    PageNotesListComponent,
    PageNotesEditComponent,
    PageSharednotesListComponent,
    PageSharednotesEditComponent,
    PageNotesNewComponent,
    PagePublicnotesListComponent,
    PagePublicnotesEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: []
      }
    })
  ],
  providers: [AuthGuard],
  bootstrap: [
    AppComponent,
    NavbarComponent
  ]
})
export class AppModule { }
