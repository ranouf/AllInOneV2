import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { PasswordComponent } from './password/password.component';
import { MyAccountComponent } from './myaccount.component';
import { MyAccountRoutingModule } from './myaccount-routing.module';
import { AngularModule } from '../../modules/angular.module';
import { CovalentModule } from '../../modules/covalent.module';
import { AngularMaterialModule } from '../../modules/angular-material.module';
import { LogPipe } from '../../pipes/log.pipe';

@NgModule({
  declarations: [
    MyAccountComponent,
    ProfileComponent,
    PasswordComponent,
    LogPipe,
  ],
  imports: [
    CommonModule,
    MyAccountRoutingModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
  ]
})
export class MyAccountModule { }
