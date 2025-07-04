import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Mail } from 'src/app/models/mail';
import { MailService } from 'src/app/services/mail.service';

@Component({
    selector: 'app-mail',
    templateUrl: './mail.component.html',
    styleUrls: ['./mail.component.scss'],
    standalone: false
})
export class MailComponent implements OnInit {

  id = 0;

  @ViewChild(TemplateRef)
  dialogTemplate!: TemplateRef<any>;

  form: UntypedFormGroup;

  constructor(fb: UntypedFormBuilder,
    private mailService: MailService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private dialog: MatDialog) {
    this.form = fb.group({
      'name': ['', [Validators.required]],
      'host': ['', [Validators.required]],
      'port': [584, [Validators.required]],
      'enableSsl': [true],
      'emailAddress': ['', [Validators.required]],
      'username': ['', [Validators.required]],
      'password': ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if(params['id']){
        this.id = +params['id']; 
        this.mailService.get(this.id).subscribe(mail => {
          this.form.get('name')?.setValue(mail.name)
          this.form.get('host')?.setValue(mail.host);
          this.form.get('port')?.setValue(mail.port);
          this.form.get('enableSsl')?.setValue(mail.enableSsl);
          this.form.get('emailAddress')?.setValue(mail.emailAddress);
          this.form.get('username')?.setValue(mail.username);
        }, err => {
          this.id = 0;
          this.form.get('password')?.setValidators([Validators.required]);
        })
      } else {
        this.form.get('password')?.setValidators([Validators.required]);
      }
   });
  }

  send(){
    const mail = this.form.value as Mail;
    mail['id'] = this.id;
    this.mailService.save(mail).subscribe(result => {
      this.toastr.success('Mail saved successfully');
      this.router.navigate(['/main/home']);
    });
  }
}
