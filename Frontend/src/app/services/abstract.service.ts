import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged.result';

export class AbstractService<TModel> {

  constructor(private name: string, protected http: HttpClient) { }

  public get(id: number): Observable<TModel> {
    return this.http.get<TModel>(`/api/${this.name}/${id}`)
  }

  public save(template: TModel): Observable<TModel>{
    return this.http.post<TModel>(`/api/${this.name}/Save`, template);
  }

  public list(offset: number, size: number): Observable<PagedResult<TModel[]>> {
    return this.http.get<PagedResult<TModel[]>>(`/api/${this.name}/List`, {
      params: {
        "offset": offset.toString(), 
        "size": size.toString()}
      }
    );
  }

  public delete(id: number): Observable<void>{
    return this.http.delete<void>(`/api/${this.name}/${id}`);
  }
}
