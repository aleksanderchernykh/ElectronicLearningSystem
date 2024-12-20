import { Routes } from '@angular/router';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { canActivateAuth } from './auth/access.guard';
import { BasePageComponent } from './pages/base-page/base-page.component';
import { UserPageComponent } from './pages/user-page/user-page.component';
import { CreateUserPageComponent } from './pages/create-user-page/create-user-page.component';
import { BoardPageComponent } from './pages/board-page/board-page.component';
import { TaskPageComponent } from './pages/task-page/task-page.component';

export const routes: Routes = [
    { path: '', component: LayoutComponent, children:[
            { path: '', component: BasePageComponent },
            { path: 'profile', component: ProfilePageComponent },
            { path: 'user', component: UserPageComponent },
            { path: 'boadr', component: BoardPageComponent},
            { path: 'task/:id', component: TaskPageComponent}
        ], canActivate: [canActivateAuth]
    },
    { path: 'login', component: LoginPageComponent },
    { path: 'createuser', component: CreateUserPageComponent }
];
