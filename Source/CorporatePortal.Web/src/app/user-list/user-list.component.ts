import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserInfo } from "../models/user-info.model";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit {
  public userInfos: UserInfo[] = [];
  public totalUsers: number = 0;
  public currentPage: number = 1;
  public itemsPerPage: number = 20;
  public totalPages: number = 0;
  public pageNumbers: number[] = [];
  private noImagePlaceholder = 'assets/images/no-image-placeholder.svg';
  private imageFolder = 'assets/images/employees/';

  constructor(private http: HttpClient, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const searchTerm = params['search'];
      this.getTotalUsers(searchTerm);
      this.getUserInfos(searchTerm, this.currentPage);
    });
  }

  getUserInfos(searchTerm?: string, page: number = 1) {
    const skip = (page - 1) * this.itemsPerPage;
    const url = `/userinfo?searchTerm=${searchTerm || ''}&skip=${skip}`;

    this.http.get<UserInfo[]>(url).subscribe(
      async (result) => {
        const userPromises = result.map(async user => {
          const imageUrl = await this.getImageUrl(user.uniqueId);
          return {
            ...user,
            imageUrl
          };
        });

        this.userInfos = await Promise.all(userPromises);
        this.updatePagination();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  getTotalUsers(searchTerm?: string) {
    const url = `/userinfo/count?searchTerm=${searchTerm || ''}`;
    this.http.get<number>(url).subscribe(
      (result) => {
        this.totalUsers = result;
        this.totalPages = Math.ceil(this.totalUsers / this.itemsPerPage);
        this.updatePagination();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  updatePagination() {
    this.pageNumbers = [];

    if (this.totalPages <= 4) {
      for (let i = 1; i <= this.totalPages; i++) {
        this.pageNumbers.push(i);
      }
    } else {
      this.pageNumbers.push(1);

      if (this.currentPage > 4) {
        this.pageNumbers.push(-1);
      }

      const startPage = Math.max(2, this.currentPage - 1);
      const endPage = Math.min(this.currentPage + 3, this.totalPages - 1);

      for (let i = startPage; i <= endPage; i++) {
        this.pageNumbers.push(i);
      }

      if (this.currentPage < this.totalPages - 3) {
        this.pageNumbers.push(-1);
      }

      if (this.currentPage !== this.totalPages) {
        this.pageNumbers.push(this.totalPages);
      }
    }
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getUserInfos(this.route.snapshot.queryParams['search'], page);
  }

  async getImageUrl(uniqueId: string): Promise<string> {
    const jpgPath = `${this.imageFolder}${uniqueId}.jpg`;

    const img = new Image();
    img.src = jpgPath;

    return await new Promise<string>((resolve) => {
      img.onload = () => resolve(jpgPath);
      img.onerror = () => resolve(this.noImagePlaceholder);
    });
  }

  title = 'corporateportal.web';
}
