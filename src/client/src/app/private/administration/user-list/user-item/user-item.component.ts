import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { UserDto } from '../../../../services/api/api.services';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss']
})
export class UserItemComponent {

  @Input() user: UserDto = <UserDto>{};
  @Input() allowUpdate: boolean = false;
  @Output() onLocked: EventEmitter<UserDto> = new EventEmitter<UserDto>();
  @Output() onUnlocked: EventEmitter<UserDto> = new EventEmitter<UserDto>();

  constructor() { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['user'] != null) {
      this.user = changes['user'].currentValue;
    }
  }

  lock(user: UserDto): void {
    this.onLocked.emit(user);
  }

  unlock(user: UserDto): void {
    this.onUnlocked.emit(user);
  }
}
