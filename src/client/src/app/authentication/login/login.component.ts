import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroupDirective, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequestDto, ApiException } from '../../services/api/api.services';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { TdMediaService, tdHeadshakeAnimation, tdCollapseAnimation, TdLoadingService } from '@covalent/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [
    tdHeadshakeAnimation, 
    tdCollapseAnimation,
  ],
})
export class LoginComponent implements OnInit {
  public form: FormGroup;
  public errorMessage: string;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    public media: TdMediaService,
  ) {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }


  login() {
    this.errorMessage = null;
    this.loadingService.register();
    this.authenticationService.login(<LoginRequestDto>this.form.value)
      .subscribe(() => {
        console.log("[LoginComponent], Authentication success");
        this.loadingService.resolve();
        this.router.navigate(['/private/']);
      }, error => {
        this.errorMessage = error.message
        console.error(error);
        this.loadingService.resolve();
      });
  }

  cancel(): void {
    this.errorMessage = null;
    this.form.reset();
  }

  ngOnInit() {
    this.media.broadcast();
    this.authenticationService.authenticationStatus.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigate(['/administration/']);
      }
    });
  }
}
