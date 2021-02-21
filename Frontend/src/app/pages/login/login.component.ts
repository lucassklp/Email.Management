import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  constructor(private router: Router, private authenticationService: AuthenticationService, fb: FormBuilder) {
    this.form = fb.group({
      'email': ['', [Validators.required, Validators.email]],
      'password': ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
  }

  login() : void {
    this.authenticationService.authenticate(this.form.value.email, this.form.value.password)
      .subscribe(token => {
        this.router.navigate(['/main/home'])
      })
  }
  
}
