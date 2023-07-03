import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { AuthRoutingModule } from "./modules/auth/auth-routing.module";
import { HomeRoutingModule } from "./modules/home/home-routing.module";

const routes: Routes = [
  { path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule) },
  { path: 'home', loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule) },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    AuthRoutingModule,
    HomeRoutingModule
  ],
  exports: [RouterModule]
})

export class AppRoutingModule { }
