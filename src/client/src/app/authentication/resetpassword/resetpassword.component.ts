import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, PasswordForgottenRequestDto, ResetPasswordRequestDto } from '../../services/api/api.services';
import { tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class ResetPasswordComponent implements OnInit {
  public form: FormGroup;
  public errorMessage: string;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    protected route: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    private notificationsService: NotificationsService,
  ) {
    this.form = this.fb.group({
      email: ['', Validators.required],
      token: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(50)]],
      newPasswordConfirmation: ['', [Validators.required, this.validateAreEqual.bind(this)]]
    });
  }

  private validateAreEqual(fieldControl: FormControl) {
    if (this.form) {
      return fieldControl.value === this.form.get("newPasswordConfirmation").value
        ? null
        : { notEqual: true };
    }
    return null;
  }

  cancel(): void {
    this.form.reset();
  }

  resetPassword() {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.resetPassword(<ResetPasswordRequestDto>this.form.value)
      .subscribe(() => {
        this.notificationsService.success(
          "Password reset!"
        );
        this.router.navigate(['/authentication/']);
        this.loadingService.resolve();
      }, error => {
        this.loadingService.resolve();
        this.errorMessage = error.message;
      });
  }

  ngOnInit() {
    this.route
      .queryParamMap
      .subscribe(params => {
        this.form.patchValue({ email: params.get('email') });
        this.form.patchValue({ token: params.get('token') });
      });
  }

}
