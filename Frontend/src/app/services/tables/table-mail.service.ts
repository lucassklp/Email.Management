import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Mail } from 'src/app/models/mail';
import { MailService } from '../mail.service';
import { AbstractTableService } from './abstract-table.service';

@Injectable({
  providedIn: 'root'
})
export class TableMailService extends AbstractTableService<Mail> {
  
  edit(id: number): void {
    this.router.navigate(['/main/mail', id])
  }
  
  public get pageSizeOptions(): number[] {
    return [5, 10, 25, 100];
  }

  public get displayedColumns(): string[] {
    return ['name', 'email', 'smtp', 'ssl', 'port', 'actions']
  }

  constructor(service: MailService, toastr: ToastrService, router: Router, dialog: MatDialog) {
    super(service, toastr, router, dialog)
  }
}
