import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from "./user-list/user-list.component";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { NotFoundComponent } from "./not-found/not-found.component";
import { MainComponent } from "./main/main.component";
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';


const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'main', component: MainComponent },
      { path: '', redirectTo: '/main', pathMatch: 'full' },
      { path: 'users', component: UserListComponent },
      { path: 'user/:id', component: UserDetailComponent },
    ]
  },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
