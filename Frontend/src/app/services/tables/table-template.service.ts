import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Template } from 'src/app/models/template';
import { TemplateService } from '../template.service';
import { AbstractTableService } from './abstract-table.service';

@Injectable({
  providedIn: 'root'
})
export class TableTemplateService extends AbstractTableService<Template> {

  edit(id: number): void {
    this.router.navigate(['/main/template', id])
  }

  public get pageSizeOptions(): number[] {
    return [5, 10, 25, 100];
  }
  
  public get displayedColumns(): string[] {
    return ['name', 'description', 'actions']
  }

  constructor(service: TemplateService, toastr: ToastrService, router: Router, dialog: MatDialog) {
    super(service, toastr, router, dialog)
  }
}
