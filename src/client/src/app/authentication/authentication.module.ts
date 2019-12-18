import { NgModule } from '@angular/core';
import { AuthenticationComponent } from './authentication.component';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AngularModule } from '../modules/angular.module';
import { AngularMaterialModule } from '../modules/angular-material.module';
import { CovalentModule } from '../modules/covalent.module';
import { CommonModule } from '@angular/common';


@NgModule({
  declarations: [
    AuthenticationComponent,
    RegisterComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
  ],
  providers: [
  ]
})
export class AuthenticationModule { }
