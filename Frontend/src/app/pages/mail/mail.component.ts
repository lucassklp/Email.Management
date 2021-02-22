import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AlertDialogComponent } from 'src/app/dialogs/alert-dialog/alert-dialog.component';
import { DialogModel } from 'src/app/models/dialog.model';
import { Mail } from 'src/app/models/mail';
import { MailService } from 'src/app/services/mail.service';

@Component({
  selector: 'app-mail',
  templateUrl: './mail.component.html',
  styleUrls: ['./mail.component.scss']
})
export class MailComponent implements OnInit {

  id = 0;

  @ViewChild(TemplateRef)
  dialogTemplate!: TemplateRef<any>;

  form: FormGroup;

  constructor(fb: FormBuilder,
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

      if(result.secret){
        const message = `Your secret is ${result.secret}.
          This secret works like a private key used to decrypt your password, 
          because we don't save it on server! 
          This secret will be required when you try to send e-mails. 
          Keep this secret on a safe place!`;
        const dialogData = new DialogModel("Save your secret!", message);
        const dialogRef = this.dialog.open(AlertDialogComponent, {
          maxWidth: "400px",
          data: dialogData
        });
    
        dialogRef.afterClosed().subscribe(_ => {        
          this.router.navigate(['/main/home']);
        })
      }
      else{
        this.router.navigate(['/main/home']);
      }
    });
  }
}
