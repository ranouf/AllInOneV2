import { Pipe, PipeTransform } from '@angular/core';
import { UserDto } from '../services/api/api.services';
import { IsAdministratorPipe } from './is-administrator.pipe';
import { IsManagerPipe } from './is-manager.pipe';
import { IsUserPipe } from './is-user.pipe';

@Pipe({
  name: 'userLogo'
})
export class UserLogoPipe implements PipeTransform {

  constructor(
    private isAdministrator: IsAdministratorPipe,
    private isManager: IsManagerPipe,
    private isUser: IsUserPipe,
  ) {
  }

  transform(user: UserDto): string {
    if (this.isAdministrator.transform(user)) {
      return "face";
    } else if (this.isManager.transform(user)) {
      return "person";
    } else if (this.isUser.transform(user)) {
      return "sentiment_satisfied_alt";
    }
  }
}
