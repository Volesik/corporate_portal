import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {UserInfo} from "../models/user-info.model";

@Component({
  selector: 'app-birthday',
  templateUrl: './birthday.component.html',
  styleUrl: './birthday.component.css'
})
export class BirthdayComponent implements OnInit {
  public birthdays: UserInfo[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsersByBirthday();
  }

  getUsersByBirthday() {
    this.http.get<UserInfo[]>('/userinfo/getTodayBirthdayUsers').subscribe(
      (data) => {
        this.birthdays = data;
      },
      (error) => {
        console.error('Error fetching birthdays user info data: ' + error);
      }
    )
  }
}
