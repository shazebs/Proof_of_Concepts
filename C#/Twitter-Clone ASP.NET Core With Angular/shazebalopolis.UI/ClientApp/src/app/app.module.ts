import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { InteractionType, PublicClientApplication, BrowserCacheLocation, LogLevel } from '@azure/msal-browser';
import { MsalModule, MsalGuard, MsalInterceptor, MsalRedirectComponent } from '@azure/msal-angular';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'fetch-data', component: FetchDataComponent },
    ]),
    MsalModule.forRoot(new PublicClientApplication({
      // Add Azure AD client credentials here
      auth: {
        clientId: '{client_id}', // insert client id here
        authority: 'https://login.microsoftonline.com/{tenant_id}', // insert tenant id here
        redirectUri: 'https://localhost:44497',
        postLogoutRedirectUri: 'https://localhost:44497',
        navigateToLoginRequestUrl: true
      },
      cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage,
        storeAuthStateInCookie: false
      },
      system: {
        loggerOptions: {
          loggerCallback: (
            level: LogLevel,
            message: string,
            containsPii: boolean
          ): void => {
            if (containsPii) {
              return;
            }
            switch (level) {
              case LogLevel.Error:
                console.error(message);
                return;
              case LogLevel.Info:
                console.info(message);
                return;
              case LogLevel.Verbose:
                console.debug(message);
                return;
              case LogLevel.Warning:
                console.warn(message);
                return;
            }
          },
          piiLoggingEnabled: false
        }
      }
    }), {
      interactionType: InteractionType.Redirect,
      authRequest: {
        scopes: ['{tenant_id}/.default'], // insert tenant id here 
        prompt: 'select_account'
      },
      loginFailedRoute: "https://localhost:44497",
    },
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: new Map([])
    })
  ],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    MsalGuard,
    HomeComponent
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
