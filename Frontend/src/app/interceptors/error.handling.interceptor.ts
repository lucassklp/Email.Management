import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {

    constructor(private toastr: ToastrService, private router: Router, private authenticationService: AuthenticationService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
        return next.handle(req).pipe(
            catchError((response: HttpErrorResponse) => {

                if(response.status == 401){
                    this.authenticationService.logout();
                    return throwError(response.error)
                }

                const msg = response.error;
                this.toastr.error(msg.message, msg.token, {
                    closeButton: true
                });
                return throwError(response.error);
            })
        );
    }
}
