import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularModule } from './modules/angular.module';
import { CovalentModule } from './modules/covalent.module';
import { AngularMaterialModule } from './modules/angular-material.module';
import { ServiceBaseConfiguration, ServiceBase } from './services/api/api.services.base';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthenticationInterceptor } from './services/authentication/authentication-interceptor';
import { AuthenticationService } from './services/authentication/authentication.service';
import { AuthenticationGuard } from './services/authentication/authentication-guard';
import { NoAuthenticationGuard } from './services/authentication/no-authentication-guard';
import { PublicComponent } from './public/public.component';
import { SimpleNotificationsModule } from 'angular2-notifications';

@NgModule({
  declarations: [
    AppComponent,
    PublicComponent,
  ],
  imports: [
    BrowserModule,
    AngularModule,
    CovalentModule,
    AngularMaterialModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SimpleNotificationsModule.forRoot()
  ],
  providers: [
    AuthenticationService,
    AuthenticationGuard,
    NoAuthenticationGuard,
    ServiceBaseConfiguration,
    ServiceBase,
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
