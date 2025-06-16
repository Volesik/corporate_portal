import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  private labelsSubject = new BehaviorSubject<any>({});
  public labels$: Observable<any> = this.labelsSubject.asObservable();
  constructor() { }
}
