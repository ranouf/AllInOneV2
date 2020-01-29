import { PipeTransform, Pipe } from '@angular/core';
import { UserDto } from '../services/api/api.services';


@Pipe({
  name: 'isUser'
})
export class IsUserPipe implements PipeTransform {

  constructor(
  ) {
  }

  transform(user: UserDto): boolean {
    return user.roleName == "User";
  }
}
