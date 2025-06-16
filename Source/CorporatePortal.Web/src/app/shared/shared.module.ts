import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { TranslateModule } from '@ngx-translate/core';
import { BirthdayComponent } from './components/birthday/birthday.component';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    BirthdayComponent
  ],
  imports: [
    CommonModule,
    TranslateModule
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    BirthdayComponent
  ]
})
export class SharedModule { }
