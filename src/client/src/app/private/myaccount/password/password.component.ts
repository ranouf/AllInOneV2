import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { TdLoadingService, tdHeadshakeAnimation, tdCollapseAnimation,  } from '@covalent/core';
import { AccountService, ChangeProfileRequestDto, ChangePasswordRequestDto } from '../../../services/api/api.services';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class PasswordComponent implements OnInit {
  public form: FormGroup;
  public errorMessage: string;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    private notificationsService: NotificationsService,
  ) {
    this.form = this.fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(50)]],
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

  reset() {
    this.errorMessage = null;
  }

  update() {
    this.reset();
    this.loadingService.register();
    this.accountService.changePasword(<ChangePasswordRequestDto>this.form.value)
      .subscribe(() => {
        this.notificationsService.success(
          "Password updated",
          "By " + this.authenticationService.currentUser.value.fullName
        );
        this.loadingService.resolve();
      }, error => {
        this.errorMessage = error.message
        this.loadingService.resolve();
      });
  }

  ngOnInit() {
  }
}
