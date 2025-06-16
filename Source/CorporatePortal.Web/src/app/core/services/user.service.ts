import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { UserInfo } from '../../models/user-info.model';
import { formatBirthdayLabel, normalizeDate } from '../../shared/helpers/date-utils';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getGroupedBirthdaysWithinDays(daysAhead: number = 2): Observable<Record<string, UserInfo[]>> {
    return this.http.get<UserInfo[]>('/userinfo/getTodayBirthdayUsers').pipe(
      map(users => {
        const today = normalizeDate(new Date());
        const result: Record<string, UserInfo[]> = {};

        for (const user of users) {
          const birthday = normalizeDate(new Date(user.birthday));
          birthday.setFullYear(today.getFullYear());

          const diffDays = Math.floor((birthday.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
          if (diffDays < 0 || diffDays > daysAhead) continue;

          const label = formatBirthdayLabel(birthday);
          if (!result[label]) result[label] = [];
          result[label].push(user);
        }

        return result;
      })
    );
  }
}
