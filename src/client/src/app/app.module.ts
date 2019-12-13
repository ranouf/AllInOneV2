import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularModule } from './modules/angular.module';
import { CovalentModule } from './modules/covalent.module';
import { AngularMaterialModule } from './modules/angular-material.module';
import { AuthenticationService } from './services/api/api.services';
import { ServiceBaseConfiguration, ServiceBase } from './services/api/api.services.base';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
    AppRoutingModule,
    BrowserAnimationsModule,
  ],
  providers: [
    AuthenticationService,
    ServiceBaseConfiguration,
    ServiceBase,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
