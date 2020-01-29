import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService, RoleService, RoleDto, UserDto, UserService, UpdateUserRequestDto, InviteUserRequestDto } from '../../../services/api/api.services';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { TdLoadingService } from '@covalent/core';
import { NotificationsService } from 'angular2-notifications';
import { observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  public form: FormGroup;
  public roles: RoleDto[];
  public errorMessage: string;

  private userId: string;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private authenticationService: AuthenticationService,
    private loadingService: TdLoadingService,
    private notificationsService: NotificationsService,
    private roleService: RoleService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.loadRoles();
    this.form = this.fb.group({
      email: [null, [Validators.required]],
      firstname: [null, [Validators.required, Validators.maxLength(50)]],
      lastname: [null, [Validators.required, Validators.maxLength(50)]],
      description: [null],
      roleId: [null, [Validators.required]],
    });
  }

  reset() {
    this.errorMessage = null;
  }

  private loadRoles(): void {
    this.loadingService.register();
    this.roleService.getAllRoles()
      .subscribe(result => {
        this.loadingService.resolve();
        this.roles = result;
        this.roles = this.roles.filter(r => r.name != "User");
      }, error => {
        this.loadingService.resolve();
        this.notificationsService.error("Error", error.message);
        console.error(error);
      });
  }

  private loadData(userId: string) {
    this.reset();
    this.loadingService.register();
    this.userService.getUserById(userId).subscribe(response => {
      this.updateForm(response);
      this.loadingService.resolve();
    }, error => {
      this.errorMessage = error.message
      console.error(error);
      this.loadingService.resolve();
    });
  }

  private updateForm(data: UserDto) {
    let role = this.roles.filter(r => r.name == data.roleName)[0];
    this.form.patchValue({ email: data.email });
    this.form.get('email').disable();
    this.form.patchValue({ firstname: data.firstname });
    this.form.patchValue({ lastname: data.lastname });
    this.form.patchValue({ description: data.description });
    this.form.patchValue({ roleId: role == null ? null : role.id });
  }

  public save() {
    this.reset();
    this.loadingService.register();
    if (this.userId) {
      this.userService.updateUser(this.userId, <UpdateUserRequestDto>this.form.value)
        .subscribe(response => {
          this.notificationsService.success(
            "User updated",
            "By " + this.authenticationService.currentUser.value.fullName
          );
          this.loadingService.resolve();
        }, error => {
          this.errorMessage = error.message
          console.error(error);
          this.loadingService.resolve();
        });
    } else {
      this.userService.inviteUser(<InviteUserRequestDto>this.form.value)
        .subscribe(response => {
          this.notificationsService.success(
            "An invitation has been sent to",
            response.email
          );
          this.loadingService.resolve();
          this.router.navigate(['/private/administration/users/', response.id]);
        }, error => {
          this.errorMessage = error.message
          console.error(error);
          this.loadingService.resolve();
        });
    }
  }

  ngOnInit() {
    this.route.params.subscribe((params: { id: string }) => {
      if (params.id && params.id != "new") {
        this.userId = params.id;
        this.loadData(params.id);
      }
    });
  }
}
