import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { HomeRoutingModule } from './home-routing.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    RouterModule,
    HomeRoutingModule
  ],
  providers: [],
  exports: [HomeRoutingModule, HomeComponent]
})
export class HomeModule { }
