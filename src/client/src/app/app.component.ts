import { Component, OnInit } from '@angular/core';
import { AccountService, LoginRequestDto, AuthenticationService } from './services/api/api.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
 
  title = 'AllInOne';

  constructor(
    public authenticationService: AuthenticationService) {
  }

  ngOnInit(): void {
    // Just for testing
    const credentials: LoginRequestDto = <LoginRequestDto>{
      email: "test@test.com",
      password: "test"
    };

    this.authenticationService.loginUser(credentials)
      .subscribe(result => {
        console.log(result);
      }, error => {
        console.error(error);
      });
  }
}
