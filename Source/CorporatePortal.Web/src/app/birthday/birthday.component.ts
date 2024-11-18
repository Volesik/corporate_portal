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
  private noImagePlaceholder = 'assets/images/no-image-placeholder.svg';
  private imageFolder = 'assets/images/employees/';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsersByBirthday();
  }

  async getImageUrl(uniqueId: string): Promise<string> {
    const jpgPath = `${this.imageFolder}${uniqueId}.jpg`;
    const pngPath = `${this.imageFolder}${uniqueId}.png`;

    // Use a temporary Image object to check for existence
    const img = new Image();
    img.src = jpgPath;

    return await new Promise<string>((resolve) => {
      img.onload = () => resolve(jpgPath);
      img.onerror = () => {
        // Check for PNG
        img.src = pngPath;
        img.onload = () => resolve(pngPath);
        img.onerror = () => resolve(this.noImagePlaceholder); // Fallback to placeholder
      };
    });
  }

  getUsersByBirthday() {
    this.http.get<UserInfo[]>('/userinfo/getTodayBirthdayUsers').subscribe(
      async (data) => {
        const birthdayPromises = data.map(async (user) => {
          const imageUrl = await this.getImageUrl(user.uniqueId); // Wait for the promise to resolve
          return {
            ...user,
            imageUrl // Add imageUrl to the user object
          };
        });

        // Wait for all promises to resolve
        this.birthdays = await Promise.all(birthdayPromises);
      },
      (error) => {
        console.error('Error fetching birthdays user info data: ' + error);
      }
    );
  }
}
