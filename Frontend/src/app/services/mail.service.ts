import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Mail } from '../models/mail';
import { AbstractService } from './abstract.service';

@Injectable({
  providedIn: 'root'
})
export class MailService extends AbstractService<Mail> {

  constructor(http: HttpClient) {
    super('Mail', http)
  }

  listAll(): Observable<{id: number, name: string}[]> {
    return this.http.get<{id: number, name: string}[]>('/api/Mail/ListAll')
  }
}
