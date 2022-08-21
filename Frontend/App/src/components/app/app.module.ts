import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RecaptchaModule} from "ng-recaptcha";

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {FeedbackComponent} from "../feedback/feedback.component";

@NgModule({
  declarations: [
    AppComponent,
    FeedbackComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RecaptchaModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
