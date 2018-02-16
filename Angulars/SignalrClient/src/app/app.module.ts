import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import 'rxjs/add/operator/map';

import {ChannelService, ChannelConfig, SignalrWindow} from './services/channel.service';

import { AppComponent } from './app.component';
import { TaskComponent } from './task/task.component';

@NgModule({
  imports:      [ BrowserModule, HttpModule ],
  declarations: [ AppComponent, TaskComponent ],
  providers: [
    ChannelService,
    { provide: SignalrWindow, useValue: window }
  ],
  bootstrap:    [ AppComponent ]
})

export class AppModule { }