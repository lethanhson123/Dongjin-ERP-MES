import { Component } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map } from "rxjs/operators";

import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from './shared/ERP/BaseResult.model';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Sanitizer: DomSanitizer,
    public TitleService: Title,
    public NotificationService: NotificationService,

  ) {

  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
}
