import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, RegistrationRequestDto, ResendEmailConfirmationRequestDto, ConfirmRegistrationEmailRequestDto } from '../../services/api/api.services';
import { tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-confirmEmail',
  templateUrl: './confirmEmail.component.html',
  styleUrls: ['./confirmEmail.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class ConfirmEmailComponent implements OnInit {
  public errorMessage: string;

  constructor(
    private router: Router,
    protected route: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    private notificationsService: NotificationsService,
  ) {
  }

  confirmRegistrationEmail(token, email) {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.confirmRegistrationEmail(<ConfirmRegistrationEmailRequestDto>{ token: token, email: email })
      .subscribe(() => {
        this.notificationsService.success(
          "Email approved"
        );
        this.router.navigate(['/authentication/']);
        this.loadingService.resolve();
      }, error => {
        this.loadingService.resolve();
        this.errorMessage = error.message;
      });
  }

  ngOnInit() {
    this.route.queryParams
      .subscribe(results => {
        let token = results['token'];
        let email = results['email'];
        this.confirmRegistrationEmail(token, email);
      });
  }
}
