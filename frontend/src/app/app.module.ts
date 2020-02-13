import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PageAboutComponent } from './page-about/page-about.component';
import { PageLoginComponent } from './page-login/page-login.component';
import { PageDashboardComponent } from './page-dashboard/page-dashboard.component';
import { PageSettingsComponent } from './page-settings/page-settings.component';
import { FormAuthComponent } from './form-auth/form-auth.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { FormItempropsComponent } from './form-itemprops/form-itemprops.component';
import { TableItemsComponent } from './table-items/table-items.component';

@NgModule({
  declarations: [
    AppComponent,
    PageAboutComponent,
    PageLoginComponent,
    PageDashboardComponent,
    PageSettingsComponent,
    FormAuthComponent,
    NavbarComponent,
    ToolbarComponent,
    FormItempropsComponent,
    TableItemsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
