import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { OverviewComponent } from './overview/overview.component';
import { MembershipComponent } from './membership/membership.component';

const routes: Routes = [
  { path: '', redirectTo: '/Homepage', pathMatch: 'full' },
  {
    path: 'Homepage', component: HomepageComponent, data: { title: 'Homepage' }
  },
  {
    path: 'Overview', component: OverviewComponent, data: { title: 'Overview' }
  },
  {
    path: 'Membership', component: MembershipComponent, data: { title: 'Membership' }
  },
  

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true, initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }









































































