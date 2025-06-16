import { Component, OnInit } from '@angular/core';
import { UserInfo } from '../../../models/user-info.model';
import { parseUkrDateLabel } from '../../helpers/date-utils';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-birthday',
  templateUrl: './birthday.component.html',
  styleUrl: './birthday.component.css'
})

export class BirthdayComponent implements OnInit {
  public birthdaysByDate: { [key: string]: UserInfo[] } = {};

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.getGroupedBirthdaysWithinDays().subscribe(grouped => {
      this.birthdaysByDate = grouped;
    })
  }

  get sortedDateKeys(): string[] {
    return Object.keys(this.birthdaysByDate).sort((a, b) =>
      parseUkrDateLabel(a).getTime() - parseUkrDateLabel(b).getTime()
    );
  }
}
