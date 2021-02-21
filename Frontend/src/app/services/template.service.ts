import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged.result';
import { Template } from '../models/template';
import { TestTemplate } from '../models/test.template';
import { AbstractService } from './abstract.service';

@Injectable({
  providedIn: 'root'
})
export class TemplateService extends AbstractService<Template> {

  constructor(http: HttpClient) {
    super('Template', http)
  }

  public test(template: TestTemplate){
    return this.http.post<void>('/api/Template/Test', template);
  }
}
