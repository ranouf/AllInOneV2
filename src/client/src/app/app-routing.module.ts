import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from './services/authentication/authentication-guard';
import { NoAuthenticationGuard } from './services/authentication/no-authentication-guard';
import { PublicComponent } from './public/public.component';

const routes: Routes = [
  { path: '', component: PublicComponent },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  {
    path: 'authentication',
    loadChildren: 'src/app/authentication/authentication.module#AuthenticationModule',
    canActivate: [NoAuthenticationGuard],
  },
  {
    path: 'private',
    loadChildren: 'src/app/private/private.module#PrivateModule',
    canActivate: [AuthenticationGuard],
    data: { roles: ["Administrator", "Manager"] }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
