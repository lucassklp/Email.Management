import { Injectable } from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { LegacyPageEvent as PageEvent } from '@angular/material/legacy-paginator';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmDialogComponent } from 'src/app/dialogs/confirm-dialog/confirm-dialog.component';
import { DialogModel } from 'src/app/models/dialog.model';
import { AbstractService } from '../abstract.service';

@Injectable({
  providedIn: 'root'
})
export abstract class AbstractTableService<TModel> {
  length = 0;
  pageSize = 5;
  pageEvent: PageEvent = {
    pageIndex: 0,
    pageSize: this.pageSize,
    length: this.length
  }

  public dataSource: MatTableDataSource<TModel>;
  
  public abstract get displayedColumns(): string[];
  public abstract get pageSizeOptions(): number[];

  constructor(private service: AbstractService<TModel>,
    protected toastr: ToastrService,
    protected router: Router,
    protected dialog: MatDialog) {
    this.dataSource = new MatTableDataSource<TModel>([])
  }

  update(event?: PageEvent){
    if(event){
      this.pageEvent = event;
    }
    this.service.list((this.pageEvent.pageIndex) * this.pageEvent.pageSize, this.pageEvent.pageSize).subscribe(result => {
      this.length = result.total
      this.dataSource = new MatTableDataSource(result.content)
    })
  }

  delete(id: number){
    const message = `Are you sure you want to delete this?`;
    const dialogData = new DialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.service.delete(id).subscribe(res => {
          this.toastr.success('Action executed successfully');
          this.update(this.pageEvent);
        });
      }
    });
  }

  abstract edit(id: number): void;
}
