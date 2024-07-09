import { Routes } from '@angular/router';
import { IndexComponent } from './core/components/index/index.component';
import { LoginComponent } from './auth/components/login/login.component';

export const routes: Routes = [

  {
    path: '',
    component: IndexComponent,
  },
  {
    path: 'dashboard',
    //TODO : GUARD
    loadChildren: () =>
      import('./core/routes/dashboard.routes').then((m) => m.dashboardRoutes),
  },
  {
    path: '**',
    redirectTo: '',
  },

];
