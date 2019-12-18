import { Component, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';
import { AuthenticationService } from './services/authentication/authentication.service';
import { UserDto } from './services/api/api.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'AllInOne';
  public isNavigating = false;
  public isLoading = true;
  public currentUser: UserDto;

  //Notification options
  public options = {
    timeOut: 5000,
    showProgressBar: true,
    pauseOnHover: false,
    clickToClose: false,
    clickIconToClose: true
  }

  constructor(
    private router: Router,
    public authenticationService: AuthenticationService,
  ) {
  }

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  ngOnInit(): void {
    this.router.events.subscribe(
      async (event) => {
        if (event instanceof NavigationStart) {
          this.isNavigating = true;
        }
        if (event instanceof NavigationEnd || event instanceof NavigationError || event instanceof NavigationCancel) {
          await this.delay(300);
          this.isNavigating = false;
        }
      }
    );
    this.authenticationService.currentUser.subscribe(value => {
      this.currentUser = value;
    })
  }
}
