import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserInfo } from "../models/user-info.model";
import { Subject, debounceTime, switchMap } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  searchText: string = '';
  showResults: boolean = false;
  filteredResults: UserInfo[] = [];
  private searchSubject: Subject<string> = new Subject();

  constructor(private http: HttpClient) {
    this.searchSubject.pipe(
      debounceTime(300), // Wait for 300ms pause in events
      switchMap((searchTerm) => this.searchUsers(searchTerm))
    ).subscribe(results => {
      this.filteredResults = results;
      this.showResults = results.length > 0;
    });
  }

  onSearch(event: Event) {
    const input = event.target as HTMLInputElement; // Cast the target to HTMLInputElement
    this.searchText = input.value; // Get the value from the input

    // Emit the new value to the subject, triggering the API call
    if (this.searchText) {
      this.searchSubject.next(this.searchText);
    } else {
      this.filteredResults = []; // Clear results if the input is empty
      this.showResults = false;
    }
  }

  searchUsers(searchTerm: string) {
    // Only make the request if the search term is not empty
    return this.http.get<UserInfo[]>(`/userinfo/search/${searchTerm}`);
  }

  searchUser(searchTerm: string) {
    if (searchTerm) {
      // Perform a search with the provided value
      this.http.get<UserInfo[]>(`/userinfo/search/${searchTerm}`)
        .subscribe(results => {
          this.filteredResults = results;
          this.showResults = results.length > 0;
        });
    }
  }

  goToUser(userId: string) {
    console.log(userId);
    window.location.href = `/user/${userId}`; // Adjust URL according to your routing
  }

  goToUsers(search: string) {
    window.location.href = `/users?search=${search}`;
  }


  hideResults() {
    setTimeout(() => {
      this.showResults = false;
    }, 100);
  }
}
