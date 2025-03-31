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
  selectedIndex: number = -1;
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
    this.selectedIndex = -1;
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

  onKeyDown(event: KeyboardEvent) {
    if (!this.filteredResults.length) return;
  
    switch (event.key) {
      case 'ArrowDown':
        this.selectedIndex = (this.selectedIndex + 1) % this.filteredResults.length;
        event.preventDefault();
        break;
  
      case 'ArrowUp':
        this.selectedIndex =
          (this.selectedIndex - 1 + this.filteredResults.length) % this.filteredResults.length;
        event.preventDefault();
        break;
  
      case 'Enter':
        if (this.selectedIndex >= 0 && this.selectedIndex < this.filteredResults.length) {
          this.goToUser(this.filteredResults[this.selectedIndex].id);
          event.preventDefault();
        } else {
          this.goToUsers(this.searchText);
        }
        break;
  
      case 'Escape':
        this.showResults = false;
        break;
    }
  }
}
