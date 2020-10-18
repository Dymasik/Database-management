import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TableComponent } from './database/table/table.component';
import { TableEditComponent } from './database/table/table-edit/table-edit.component';
import { DatabaseComponent } from './database/database.component';
import { TableStartComponent } from './database/table/table-start/table-start.component';
import { TableContentComponent } from './database/table-content/table-content.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    TableComponent,
    TableEditComponent,
    DatabaseComponent,
    TableStartComponent,
    TableContentComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      {
        path: 'database', component: DatabaseComponent, children: [
          { path: '', component: TableStartComponent },
          { path: 'new', component: TableEditComponent },
          { path: 'table-content/:name', component: TableContentComponent }
        ]
      },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
