import { Component, OnInit, ViewChild } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Template } from 'src/app/models/template';
import { TableMailService } from 'src/app/services/tables/table-mail.service';
import { TableTemplateService } from 'src/app/services/tables/table-template.service';
import { TemplateService } from 'src/app/services/template.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
    standalone: false
})
export class HomeComponent implements OnInit {

  constructor(public tableTemplate: TableTemplateService, public tableMail: TableMailService, private router: Router) {

  }

  ngOnInit(): void {
    this.tableMail.update();
    this.tableTemplate.update();
  }

  goToAddTemplate(){
    this.router.navigate(['/main/template/'])
  }

  goToAddMail(){
    this.router.navigate(['/main/mail/'])
  }
}