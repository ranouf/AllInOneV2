import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, RegistrationRequestDto } from '../../services/api/api.services';
import { tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class RegisterComponent implements OnInit {
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
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(50)]],
      passwordConfirmation: ['', [Validators.required, this.validateAreEqual.bind(this)]]
    });
  }

  private validateAreEqual(fieldControl: FormControl) {
    if (this.form) {
      return fieldControl.value === this.form.get("passwordConfirmation").value
        ? null
        : { notEqual: true };
    }
    return null;
  }

  cancel(): void {
    this.form.reset();
  }

  register() {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.registerUser(<RegistrationRequestDto>this.form.value)
      .subscribe(() => {
        this.notificationsService.success(
          "Registration succeeded"
        );
        this.router.navigate(['/authentication/']);
        this.loadingService.resolve();
      }, error => {
        this.loadingService.resolve();
        this.errorMessage = error.message;
      });
  }

  ngOnInit() {
  }

}
