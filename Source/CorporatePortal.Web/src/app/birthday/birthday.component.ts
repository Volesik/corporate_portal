import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {UserInfo} from "../models/user-info.model";

@Component({
  selector: 'app-birthday',
  templateUrl: './birthday.component.html',
  styleUrl: './birthday.component.css'
})
export class BirthdayComponent implements OnInit {
  public birthdaysByDate: { [key: string]: UserInfo[] } = {};
  private noImagePlaceholder = 'assets/images/no-image-placeholder.svg';
  private imageFolder = 'assets/images/employees/';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsersByBirthday();
  }

  getUsersByBirthday() {
    this.http.get<UserInfo[]>('/userinfo/getTodayBirthdayUsers').subscribe(async users => {
      const today = new Date();

      for (const user of users) {
        const birthday = new Date(user.birthday);
        birthday.setFullYear(today.getFullYear());

        const diffDays = Math.floor((birthday.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
        if (diffDays < 0 || diffDays > 2) continue;

        const label = birthday.toLocaleDateString('uk-UA', {
          day: 'numeric',
          month: 'long'
        });

        if (!this.birthdaysByDate[label]) this.birthdaysByDate[label] = [];
        this.birthdaysByDate[label].push(user);
      }
    });
  }

  get sortedDateKeys(): string[] {
    const monthMap: { [key: string]: number } = {
      'січня': 0, 'лютого': 1, 'березня': 2, 'квітня': 3, 'травня': 4, 'червня': 5,
      'липня': 6, 'серпня': 7, 'вересня': 8, 'жовтня': 9, 'листопада': 10, 'грудня': 11
    };
  
    return Object.keys(this.birthdaysByDate).sort((a, b) => {
      const [dayA, monthA] = a.split(' ');
      const [dayB, monthB] = b.split(' ');
  
      const dateA = new Date(new Date().getFullYear(), monthMap[monthA], +dayA);
      const dateB = new Date(new Date().getFullYear(), monthMap[monthB], +dayB);
  
      return dateA.getTime() - dateB.getTime();
    });
  }
}
