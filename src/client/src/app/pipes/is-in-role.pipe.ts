import { Pipe, PipeTransform } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';

@Pipe({
  name: 'isInRole'
})
export class IsInRolePipe implements PipeTransform {

  constructor(
    private authenticationService: AuthenticationService
  ) {
  }

  transform(roles: string[]): boolean {

    let role = this.authenticationService.isAuthenticated()
      ? this.authenticationService.currentUser.value.roleName
      : null;
    return roles.indexOf(role) > -1;
  }
}
