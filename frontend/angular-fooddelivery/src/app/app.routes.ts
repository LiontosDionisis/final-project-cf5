import { RouterModule,Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { AdminComponent } from './components/admin/admin.component';

export const routes: Routes = [
    {path: "home", component: HomepageComponent},
    {path: "login", component: LoginComponent},
    {path: "", redirectTo: "/home", pathMatch: "full"},
    {path: "register", component: RegisterComponent},
    { path: 'admin', component: AdminComponent},
    { path: '**', redirectTo: '/home' }
];


