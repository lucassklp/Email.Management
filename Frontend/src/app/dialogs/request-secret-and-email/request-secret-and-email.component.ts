import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RecipientAndSecret } from 'src/app/models/recipient.and.secret';

@Component({
    selector: 'app-request-secret-and-email',
    templateUrl: './request-secret-and-email.component.html',
    styleUrls: ['./request-secret-and-email.component.scss'],
    standalone: false
})
export class RequestSecretAndEmailComponent {
  form: UntypedFormGroup;
  constructor(fb: UntypedFormBuilder, 
    public dialogRef: MatDialogRef<RequestSecretAndEmailComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: RecipientAndSecret) {
    this.form = fb.group({
      'email': ['', [Validators.required, Validators.email]],
      'save': [true]
    });

    if(this.data){
      this.form.get('email')?.setValue(this.data.email);
      this.form.get('secret')?.setValue(this.data.secret);
    }
  }

  send(){
    this.dialogRef.close(this.form.value)
  }

  cancel(){
    this.dialogRef.close();
  }

}
