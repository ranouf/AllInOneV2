import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationComponent } from './authentication.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PasswordForgottenComponent } from './passwordforgotten/passwordforgotten.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';
import { ResendConfirmationComponent } from './resendconfirmation/resendconfirmation.component';
import { ConfirmEmailComponent } from './confirmemail/confirmemail.component';

const routes: Routes = [
  {
    path: '',
    component: AuthenticationComponent,
    children: [
      {
        path: '', component: LoginComponent,
        data: { title: 'Login' },
      },
      {
        path: 'register',
        component: RegisterComponent,
        data: { title: 'Register' },
      },
      {
        path: 'passwordforgotten',
        component: PasswordForgottenComponent,
        data: { title: 'Password forgotten' },
      },
      {
        path: 'resetpassword',
        component: ResetPasswordComponent,
        data: { title: 'Reset password' },
      },
      {
        path: 'resendconfirmation',
        component: ResendConfirmationComponent,
        data: { title: 'Resend email confirmation' },
      },
      {
        path: 'confirmemail',
        component: ConfirmEmailComponent,
        data: { title: 'Confirm email' },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
