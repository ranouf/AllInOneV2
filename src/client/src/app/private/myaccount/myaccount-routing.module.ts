import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { PasswordComponent } from './password/password.component';
import { MyAccountComponent } from './myaccount.component';

const routes: Routes = [
  {
    path: '',
    component: MyAccountComponent,
    children: [
      { path: '', redirectTo: 'profile', pathMatch: 'full' },
      {
        path: 'profile',
        component: ProfileComponent,
        data: { title: 'Profile' },
      },
      {
        path: 'password',
        component: PasswordComponent,
        data: { title: 'Password' },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyAccountRoutingModule { }
