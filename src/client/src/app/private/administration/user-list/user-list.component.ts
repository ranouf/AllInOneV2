import { Component, OnInit } from '@angular/core';
import { UserDto, UserService } from '../../../services/api/api.services';
import { TdLoadingService, IPageChangeEvent, TdDialogService, tdHeadshakeAnimation, tdCollapseAnimation } from '@covalent/core';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  animations: [
    tdHeadshakeAnimation,
    tdCollapseAnimation,
  ],
})
export class UserListComponent implements OnInit {
  public data: UserDto[];
  public filter: string;
  public maxResultCount: number = 50;
  public skipCount: number = 0;
  public total: number = 0;

  public errorMessage: string;
  public currentUser: UserDto;

  constructor(
    private authenticationService: AuthenticationService,
    private notificationsService: NotificationsService,
    private dialogService: TdDialogService,
    private userService: UserService,
    private loadingService: TdLoadingService
  ) { }


  search(filter: string): void {
    this.filter = filter;
    this.loadData();
  }

  page(pagingEvent: IPageChangeEvent): void {
    this.skipCount = pagingEvent.fromRow - 1;
    this.maxResultCount = pagingEvent.pageSize;
    this.loadData();
  }

  reset() {
    this.errorMessage = null;
  }

  loadData() {
    this.reset();
    this.loadingService.register();
    this.userService.getUsers(
      this.filter,
      this.maxResultCount,
      this.skipCount
    ).subscribe(response => {
      this.data = response.items;
      this.total = response.totalCount;
      this.loadingService.resolve();
    }, error => {
      this.errorMessage = error.message
      console.error(error);
      this.loadingService.resolve();
    });
  }

  lock(user: UserDto): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to lock this user?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.loadingService.register('user.list');
          this.userService.lockUser(user.id)
            .subscribe(() => {
              this.loadingService.resolve('user.list');
              this.notificationsService.success(
                "User locked",
                "By " + this.currentUser.fullName
              );
              //this.filter();
              user.isLocked = true;
            }, error => {
              this.loadingService.resolve('user.list');
              this.notificationsService.error("Error", error.message);
              console.error("lock user failed, error:" + error.message);
            });
        }
      });
  }

  unlock(user: UserDto): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to unlock this user?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.loadingService.register('user.list');
          this.userService.unlockUser(user.id)
            .subscribe(() => {
              this.loadingService.resolve('user.list');
              this.notificationsService.success(
                "User unlocked",
                "By " + this.currentUser.fullName
              );
              user.isLocked = false;
              //this.loadData();
            }, error => {
              this.loadingService.resolve('user.list');
              this.notificationsService.error("Error", error.message);
              console.error("unlock user failed, error:" + error.message);
            });
        }
      });
  }

  ngOnInit() {
    this.loadData();
    this.currentUser = this.authenticationService.currentUser.value;
  }
}
