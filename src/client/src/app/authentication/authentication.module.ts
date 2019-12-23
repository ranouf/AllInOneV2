import { NgModule } from '@angular/core';
import { AuthenticationComponent } from './authentication.component';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AngularModule } from '../modules/angular.module';
import { AngularMaterialModule } from '../modules/angular-material.module';
import { CovalentModule } from '../modules/covalent.module';
import { CommonModule } from '@angular/common';
import { PasswordForgottenComponent } from './passwordforgotten/passwordforgotten.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import { ComponentsModule } from '../components/components.module';


@NgModule({
  declarations: [
    AuthenticationComponent,
    RegisterComponent,
    LoginComponent,
    PasswordForgottenComponent,
    ResetPasswordComponent
  ],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
    ComponentsModule
  ],
  providers: [
  ]
})
export class AuthenticationModule { }
