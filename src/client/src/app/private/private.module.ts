import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PrivateComponent } from './private.component';
import { PrivateRoutingModule } from './private-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyAccountComponent } from './myaccount/myaccount.component';
import { AngularModule } from '../modules/angular.module';
import { CovalentModule } from '../modules/covalent.module';
import { AngularMaterialModule } from '../modules/angular-material.module';

@NgModule({
  declarations: [
    PrivateComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    PrivateRoutingModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
  ]
})
export class PrivateModule { }
