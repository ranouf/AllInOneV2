import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, PasswordForgottenRequestDto } from '../../services/api/api.services';
import { tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-passwordforgotten',
  templateUrl: './passwordforgotten.component.html',
  styleUrls: ['./passwordforgotten.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class PasswordForgottenComponent implements OnInit {
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

  passwordforgotten() {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.passwordForgotten(<PasswordForgottenRequestDto>this.form.value)
      .subscribe(() => {
        this.notificationsService.success(
          "An email has been sent"
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
