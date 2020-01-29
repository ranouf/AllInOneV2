import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './user-list/user-list.component';
import { UserItemComponent } from './user-list/user-item/user-item.component';
import { AdministrationComponent } from './administration.component';
import { AdministrationRoutingModule } from './administration-routing.module';
import { AngularMaterialModule } from '../../modules/angular-material.module';
import { CovalentModule } from '../../modules/covalent.module';
import { AngularModule } from '../../modules/angular.module';
import { UserComponent } from './user/user.component';
import { IsAdministratorPipe } from '../../pipes/is-administrator.pipe';
import { IsManagerPipe } from '../../pipes/is-manager.pipe';
import { IsUserPipe } from '../../pipes/is-user.pipe';
import { UserLogoPipe } from '../../pipes/user-logo.pipe';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  declarations: [
    UserListComponent,
    UserItemComponent,
    UserComponent,
    AdministrationComponent,
    IsAdministratorPipe,
    IsManagerPipe,
    IsUserPipe,
    UserLogoPipe,
  ],
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
    ComponentsModule,
  ],
  providers: [
    IsAdministratorPipe,
    IsManagerPipe,
    IsUserPipe,
  ]
})
export class AdministrationModule { }
