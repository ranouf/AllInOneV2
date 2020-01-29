import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, RegistrationRequestDto, ResendEmailConfirmationRequestDto } from '../../services/api/api.services';
import { tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-resendConfirmation',
  templateUrl: './resendConfirmation.component.html',
  styleUrls: ['./resendConfirmation.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class ResendConfirmationComponent implements OnInit {
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
    });
  }

  cancel(): void {
    this.form.reset();
  }

  resendConfirmation() {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.resendEmailConfirmation(<ResendEmailConfirmationRequestDto>this.form.value)
      .subscribe(() => {
        this.notificationsService.success(
          "Confirmation sent"
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
