import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { TdLoadingService, tdHeadshakeAnimation, tdCollapseAnimation, tdFadeInOutAnimation } from '@covalent/core';
import { AccountService, ChangeProfileRequestDto } from '../../../services/api/api.services';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  animations: [
    tdHeadshakeAnimation, 
    tdCollapseAnimation,
    tdFadeInOutAnimation,
  ],
})
export class ProfileComponent implements OnInit {
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
      firstname: ['', Validators.required],
      lastname: ['', Validators.required]
    });
  }

  reset() {
    this.errorMessage = null;
  }

  update() {
    this.reset();
    this.loadingService.register();
    this.accountService.updateProfile(<ChangeProfileRequestDto>this.form.value)
      .subscribe(response => {
        this.authenticationService.setCurrentUser(response);
        this.notificationsService.success(
          "Profile updated",
          "By " + this.authenticationService.currentUser.value.fullName
        );
        this.loadingService.resolve();
      }, error => {
        this.errorMessage = error.message
        console.error(error);
        this.loadingService.resolve();
      });
  }

  ngOnInit() {
    let currentUser = this.authenticationService.currentUser
    this.form.patchValue({ firstname: currentUser.value.firstname });
    this.form.patchValue({ lastname: currentUser.value.lastname });
  }

}
