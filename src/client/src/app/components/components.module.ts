import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageComponent } from './message/message.component';
import { AngularMaterialModule } from '../modules/angular-material.module';



@NgModule({
  declarations: [MessageComponent],
  imports: [
    CommonModule,
    AngularMaterialModule,
  ],
  exports: [
    MessageComponent
  ]
})
export class ComponentsModule { }
