import { HttpClientModule } from '@angular/common/http';
import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { registerLocaleData } from '@angular/common';
import localeUk from '@angular/common/locales/uk';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserListComponent } from './user-list/user-list.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { BirthdayComponent } from './birthday/birthday.component';
import { MainComponent } from './main/main.component';

registerLocaleData(localeUk);

@NgModule({
  declarations: [AppComponent, UserListComponent, UserDetailComponent, NotFoundComponent, HeaderComponent, FooterComponent, BirthdayComponent, MainComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    NgSelectModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'uk-UA' }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
