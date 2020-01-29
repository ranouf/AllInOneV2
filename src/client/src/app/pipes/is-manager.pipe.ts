import { UserDto } from '../services/api/api.services';
import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
  name: 'isManager'
})
export class IsManagerPipe implements PipeTransform {

  constructor(
  ) {
  }

  transform(user: UserDto): boolean {
    return user.roleName == "Manager";
  }
}
