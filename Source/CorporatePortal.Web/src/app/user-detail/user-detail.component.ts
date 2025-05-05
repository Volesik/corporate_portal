import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserInfo } from "../models/user-info.model";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.css',
  providers: [DatePipe]
})
export class UserDetailComponent {
  public userInfo?: UserInfo;
  private noImagePlaceholder = 'assets/images/no-image-placeholder.svg';
  private imageFolder = 'assets/images/employees/';

  constructor(private route: ActivatedRoute, private http: HttpClient, private datePipe: DatePipe) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const userId = this.route.snapshot.paramMap.get('id');
      if (userId) {
        this.getUserInfoById(userId);
      }
    });
  }

  getYearsInCompany(employmentDate?: Date): string {
    if (!employmentDate) {
      return 'менше року';
    }

    const currentYear = new Date().getFullYear();
    const employmentYear = new Date(employmentDate).getFullYear();
    const yearsInCompany = currentYear - employmentYear;
    return yearsInCompany === 0 ? 'менше року' : yearsInCompany.toString();
  }

  getUserInfoById(id: string) : void {
    this.http.get<UserInfo>(`/userinfo/${id}`).subscribe(
      async (user) => {
        const imageUrl = await this.getImageUrl(user.uniqueId);
        this.userInfo = {
          ...user,
          imageUrl
        };
      },
      (error) => {
        console.error('Error fetching user info data: ' + error);
      }
    )
  }

  async getImageUrl(uniqueId: string): Promise<string> {
    const jpgPath = `${this.imageFolder}${uniqueId}.jpg`;
    const pngPath = `${this.imageFolder}${uniqueId}.png`;

    const img = new Image();
    img.src = jpgPath;

    return await new Promise<string>((resolve) => {
      img.onload = () => resolve(jpgPath);
      img.onerror = () => {
        img.src = pngPath;
        img.onload = () => resolve(pngPath);
        img.onerror = () => resolve(this.noImagePlaceholder);
      };
    });
  }
}
