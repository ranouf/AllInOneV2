import { Pipe, PipeTransform } from '@angular/core';
import { UserDto } from '../services/api/api.services';

@Pipe({
  name: 'isAdministrator'
})
export class IsAdministratorPipe implements PipeTransform {

  constructor(
  ) {
  }

  transform(user: UserDto): boolean {
    return user.roleName == "Administrator";
  }
}
