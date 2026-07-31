import { Component } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map } from "rxjs/operators";

import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from './shared/ERP/BaseResult.model';

import { DownloadService } from 'src/app/shared/Download.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  PageTitle = environment.PageTitle;
  PageDescription = environment.PageDescription;
  PageTitleShort = environment.PageTitleShort;
  queryString: string = environment.InitializationString;
  queryStringSub: string = environment.InitializationString;
  Token: string = environment.InitializationString;
  IsAuthentication: boolean = false;
  MenuWidth: number = 130;
  MenuCSS: any;
  LanguageFlag: string = "en.png";
  LanguageID: number = 1;
  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Sanitizer: DomSanitizer,
    public TitleService: Title,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    
    
    
    
  ) {
    
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {    
  }
  ChangeLanguage(ID: number) {
   
  }
 
}
