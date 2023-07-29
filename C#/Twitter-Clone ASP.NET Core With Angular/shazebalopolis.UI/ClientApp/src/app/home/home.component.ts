import { Component, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { EventMessage, EventType, InteractionStatus } from '@azure/msal-browser';
import { FetchDataComponent, Tweet } from '../fetch-data/fetch-data.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit
{
  // Global Variables
  loginDisplay = false;
  username!: any;
  preferred_username: string = 'Developer';
  public Tweets: Tweet[] = [];   

  public tweet: Tweet = {
    'Content': '',
    'Username': this.preferred_username
  }

  // Constructor
  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private fetchData: FetchDataComponent
  )
  {
  }

  // OnInit
  async ngOnInit(): Promise<void>
  {
    this.msalBroadcastService.msalSubject$
    .pipe(filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS))
      .subscribe(async (result: any) => {
        localStorage.setItem('Token', JSON.stringify(result.payload.accessToken));
        this.setLoginDisplay();
        if (result !== undefined) this.username = result.payload.account.name;
      });

    this.msalBroadcastService.inProgress$
    .pipe(filter((status: InteractionStatus) => status === InteractionStatus.None))
      .subscribe(() => {
        this.setLoginDisplay();
        if (this.loginDisplay) this.GetAllTweets();
    });
  }

  // Login Status
  setLoginDisplay()
  {
    this.loginDisplay = this.authService.instance.getAllAccounts().length > 0; 
  }

  // Resize #PostContentBox function. 
  ResizePostContentBox(): void
  {
    var postbox = document.getElementById('PostContentBox');
    if (postbox != null)
    {
      let value = postbox.style.minHeight;
      if (value === 'fit-content' || value === '') postbox.style.minHeight = '250px';
      else postbox.style.minHeight = 'fit-content'; 
    }
  }

  // Get all tweets function.
  async GetAllTweets(): Promise<void>
  {
    (await this.fetchData.GetAllTweets())
      .subscribe(async (response: any) => {
        this.Tweets = response.value;
        await this.Tweets.reverse();
      },
      (error: any) => {
        console.error(error)
        //this.LogoutRedirect();
      },
      () => {
        let loadingIcon = document.getElementById('LoadingIcon');
        if (loadingIcon != null) loadingIcon.style.display = 'none';
      }
    );
  }

  // Post tweet function.
  async PostTweet(): Promise<void>
  {
    if (this.tweet.Content !== '') {
      (await this.fetchData.CreateNewTweet(this.tweet))
      .subscribe(async (response: any) => {
        await this.GetAllTweets();
      },
      (error: any) => {
        console.error(error)
      }); 
    }
  }

  // Logout redirect.
  LogoutRedirect()
  {
    this.authService.logoutRedirect({postLogoutRedirectUri: 'https://localhost:44497'});
    localStorage.removeItem('Token');
  }

  // Change iStatus instance.
  ChangeWebsitePreview()
  {
    var istatus = document.getElementById('iStatus');
    if (istatus != undefined)
    {
      if (istatus.getAttribute("src") === "https://shazebs.com") istatus.setAttribute("src", "https://shazebs.net");
      else  istatus.setAttribute("src", "https://shazebs.com");
    }
  }

}
