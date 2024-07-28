import { Routes } from '@angular/router';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { canActivateAuth } from './auth/access.guard';
import { BasePageComponent } from './pages/base-page/base-page.component';
import { UserPageComponent } from './pages/user-page/user-page.component';
import { CreateUserPageComponent } from './pages/create-user-page/create-user-page.component';

export const routes: Routes = [
    { path: '', component: LayoutComponent, children:[
            { path: '', component: BasePageComponent  },
            { path: 'profile', component: ProfilePageComponent  },
            { path: 'user', component: UserPageComponent  },
        ], canActivate: [canActivateAuth]
    },
    { path: 'login', component: LoginPageComponent },
    { path: 'createuser', component: CreateUserPageComponent }
];
