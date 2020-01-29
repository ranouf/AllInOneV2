import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PrivateComponent } from './private.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component:PrivateComponent,
    children: [
      {
        path: '', component: DashboardComponent,
        data: { title: 'Dashboard' },
      },
      {
        path: 'myaccount',
        loadChildren: 'src/app/private/myaccount/myaccount.module#MyAccountModule',
      },
      {
        path: 'administration',
        loadChildren: 'src/app/private/administration/administration.module#AdministrationModule',
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrivateRoutingModule { }
