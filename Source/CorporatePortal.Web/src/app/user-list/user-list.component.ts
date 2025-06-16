import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit  } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfo } from "../models/user-info.model";
import { ActivatedRoute } from '@angular/router';
import { UserDashboardSearchParameters } from '../models/user-dashboard-search-parameters.enum';

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

  public roomOptions: string[] = [];
  public departmentOptions: string[] = [];
  public positionOptions: string[] = [];
  public cityOptions: string[] = [];

  filters = {
    rooms: [] as string[],
    departments: [] as string[],
    positions: [] as string[],
    cities: [] as string[]
  };

  constructor(private http: HttpClient, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const searchTerm = params['search'];
      const room = params['room'];

      if (room) {
        this.filters.rooms = [room];
      }

      this.getTotalUsers(searchTerm);
      this.getUserInfos(searchTerm, this.currentPage);
    });

    this.loadFilterOptions();
  }

  getUserInfos(searchTerm?: string, page: number = 1) {
    const skip = (page - 1) * this.itemsPerPage;

    const filterJson = JSON.stringify({
      rooms: this.filters.rooms,
      departments: this.filters.departments,
      positions: this.filters.positions,
      cities: this.filters.cities
    });

    const params = new HttpParams()
      .set('searchTerm', searchTerm || '')
      .set('skip', skip.toString())
      .set('take', this.itemsPerPage.toString())
      .set('filter', filterJson);

    this.http.get<UserInfo[]>('/userinfo', { params }).subscribe(
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

  /*getTotalUsers(searchTerm?: string) {
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
  }*/

  getTotalUsers(searchTerm?: string) {
    const filterJson = JSON.stringify({
      rooms: this.filters.rooms,
      departments: this.filters.departments,
      positions: this.filters.positions,
      cities: this.filters.cities
    });

    const params = new HttpParams()
      .set('searchTerm', searchTerm || '')
      .set('filter', filterJson);

    this.http.get<number>('/userinfo/count', { params }).subscribe(
      (result) => {
        this.totalUsers = result;
        this.totalPages = Math.ceil(this.totalUsers / this.itemsPerPage);
        this.updatePagination();
      },
      (error) => {
        console.error('Error loading total user count:', error);
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
    this.applyFilters();
    // this.getUserInfos(this.route.snapshot.queryParams['search'], page);
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

  getFilterOptions(searchType: UserDashboardSearchParameters): Observable<string[]> {
    const params = new HttpParams().set('SearchType', searchType);
    return this.http.get<string[]>('/userinfo/user-dashboard-search', { params });
  }

  loadFilterOptions(): void {
    this.getFilterOptions(UserDashboardSearchParameters.Room).subscribe(options => {
      this.roomOptions = options;
    });

    this.getFilterOptions(UserDashboardSearchParameters.Department).subscribe(options => {
      this.departmentOptions = options;
    });

    this.getFilterOptions(UserDashboardSearchParameters.Position).subscribe(options => {
      this.positionOptions = options;
    });

    this.getFilterOptions(UserDashboardSearchParameters.City).subscribe(options => {
      this.cityOptions = options;
    });
  }

  applyFilters() {
    const searchTerm = this.route.snapshot.queryParams['search'] || '';
    const skip = (this.currentPage - 1) * this.itemsPerPage;

    const filterJson = JSON.stringify({
      rooms: this.filters.rooms,
      departments: this.filters.departments,
      positions: this.filters.positions,
      cities: this.filters.cities
    });

    const params = new HttpParams()
      .set('searchTerm', searchTerm)
      .set('skip', skip.toString())
      .set('take', this.itemsPerPage.toString())
      .set('filter', filterJson);

    this.http.get<UserInfo[]>('/userinfo', { params }).subscribe(
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
        this.getTotalUsers(searchTerm);
      },
      (error) => {
        console.error('Error loading filtered users:', error);
      }
    );
  }

  resetFilters() {
    this.filters = {
      rooms: [],
      departments: [],
      positions: [],
      cities: []
    };
    this.currentPage = 1;
    this.applyFilters();
  }

  title = 'corporateportal.web';
}
