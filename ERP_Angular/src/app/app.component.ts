import { Component } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map } from "rxjs/operators";


import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from './shared/ERP/BaseResult.model';



import { MembershipToken } from 'src/app/shared/ERP/MembershipToken.model';
import { MembershipTokenService } from 'src/app/shared/ERP/MembershipToken.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

import { MembershipHistoryURL } from 'src/app/shared/ERP/MembershipHistoryURL.model';
import { MembershipHistoryURLService } from 'src/app/shared/ERP/MembershipHistoryURL.service';

import { CategoryMenu } from 'src/app/shared/ERP/CategoryMenu.model';
import { CategoryMenuService } from 'src/app/shared/ERP/CategoryMenu.service';

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

    public MembershipTokenService: MembershipTokenService,
    public MembershipHistoryURLService: MembershipHistoryURLService,
    public MembershipService: MembershipService,
    public CategoryMenuService: CategoryMenuService,
  ) {

    this.CategoryMenuService.BaseParameter.LanguageID = 1;
    let LanguageID = localStorage.getItem(environment.LanguageID);
    if (LanguageID == null) {
      localStorage.setItem(environment.LanguageID, this.CategoryMenuService.BaseParameter.LanguageID.toString());
    }
    else {
      this.CategoryMenuService.BaseParameter.LanguageID = Number(localStorage.getItem(environment.LanguageID));
    }

    this.Router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let child = this.ActiveRouter.firstChild;
        while (child) {
          if (child.firstChild) {
            child = child.firstChild;
          } else if (child.snapshot.data && child.snapshot.data['title']) {
            return child.snapshot.data['title'];
          } else {
            return null;
          }
        }
        return null;
      })
    ).subscribe((data: any) => {
      if (data) {
        this.TitleService.setTitle(data + ' - DONGJIN GLOBAL');
      }
    });

    this.GetByQueryString();
    this.MenuCSS = "width: " + this.MenuWidth + "px;";
    this.MenuCSS = this.Sanitizer.bypassSecurityTrustStyle(this.MenuCSS);
  }
  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.StartTimer();
  }
  ChangeLanguage(ID: number) {
    this.CategoryMenuService.BaseParameter.LanguageID = ID;
    localStorage.setItem(environment.LanguageID, this.CategoryMenuService.BaseParameter.LanguageID.toString());
    switch (this.CategoryMenuService.BaseParameter.LanguageID) {
      case 1:
        this.LanguageFlag = "en.png";
        break;
      case 2:
        this.LanguageFlag = "vi.png";
        break;
      case 3:
        this.LanguageFlag = "kr.png";
        break;
    }
    for (var i = 0; i < this.CategoryMenuService.ListChild.length; i++) {
      switch (this.CategoryMenuService.BaseParameter.LanguageID) {
        case 1:
          this.CategoryMenuService.ListChild[i].Name = this.CategoryMenuService.ListChild[i].NameEnglish;
          break;
        case 2:
          this.CategoryMenuService.ListChild[i].Name = this.CategoryMenuService.ListChild[i].NameVietNam;
          break;
        case 3:
          this.CategoryMenuService.ListChild[i].Name = this.CategoryMenuService.ListChild[i].NameKorea;
          break;
      }
    }
  }
  GetByQueryString() {
    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {       
        this.queryString = event.url;
        if (this.queryString.indexOf(environment.Token) > -1) {
          localStorage.setItem(environment.Token, this.queryString.split('=')[this.queryString.split('=').length - 1]);
        }
        this.AuthenticationByTokenAsync();
      }
    });
  }
  AuthenticationByTokenAsync() {
    this.MembershipTokenService.BaseParameter.Token = localStorage.getItem(environment.Token);
    let IsAuthentication = true;
    if (this.MembershipTokenService.BaseParameter.Token == null) {
      IsAuthentication = false;
    }
    else {     

      this.MembershipTokenService.AuthenticationByTokenAsync().subscribe(
        res => {
          this.MembershipTokenService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          if (this.MembershipTokenService.BaseParameter.BaseModel != null) {
            if (this.MembershipTokenService.BaseParameter.BaseModel.ParentID > 0) {
              this.MembershipService.BaseParameter.ID = this.MembershipTokenService.BaseParameter.BaseModel.ParentID;
              let Bearer = this.MembershipService.Headers.getAll("Authorization")[0];
              Bearer = Bearer.trim();
              if ((Bearer == environment.Bearer) || (Bearer == "Bearer")) {
                this.MembershipService.Headers = new HttpHeaders();
                this.MembershipService.Headers = this.MembershipService.Headers.append('Authorization', 'Bearer ' + this.MembershipTokenService.BaseParameter.Token);
              }
              this.MembershipService.GetByIDAsync().subscribe(
                res => {
                  this.MembershipService.FormData = (res as BaseResult).BaseModel;
                  localStorage.setItem(environment.Token, this.MembershipTokenService.BaseParameter.Token);
                  localStorage.setItem(environment.UserID, this.MembershipService.FormData.ID.toString());
                  localStorage.setItem(environment.UserName, this.MembershipService.FormData.UserName);
                  localStorage.setItem(environment.FullName, this.MembershipService.FormData.Name);
                  localStorage.setItem(environment.FileName, this.MembershipService.FormData.FileName);
                  localStorage.setItem(environment.CompanyID, this.MembershipService.FormData.CompanyID.toString());
                  localStorage.setItem(environment.CategoryDepartmentID, this.MembershipService.FormData.CategoryDepartmentID.toString());
                  this.Token = this.MembershipTokenService.BaseParameter.Token;
                  this.MembershipHistoryURLSave();
                  this.CategoryMenuGetByMembershipID_ActiveIDToListAsync();

                },
                err => {
                  IsAuthentication = false;
                  this.RouterNavigate(IsAuthentication);
                },
                () => {
                }
              );
            }
            else {
              IsAuthentication = false;
              this.RouterNavigate(IsAuthentication);
            }
          }
          else {
            IsAuthentication = false;
            this.RouterNavigate(IsAuthentication);
          }
        },
        err => {
          IsAuthentication = false;
          this.RouterNavigate(IsAuthentication);
        }
      );
    }
    this.RouterNavigate(IsAuthentication);
  }
  RouterNavigate(IsAuthentication: boolean) {
    if (IsAuthentication == false) {
      //localStorage.setItem(environment.Token, environment.InitializationString);
      let MembershipID = 0;
      let UserID = localStorage.getItem(environment.UserID);
      if (UserID) {
        MembershipID = Number(UserID);
      }
      this.Router.navigate(['/' + environment.Login + '/' + MembershipID]);
    }
  }
  CategoryMenuGetByMembershipID_ActiveIDToListAsync() {
    if (this.queryString) {
      if (this.queryString.length > 0) {
        this.queryStringSub = this.queryString.substring(1, this.queryString.length);
      }
    }
    let Bearer = this.CategoryMenuService.Headers.getAll("Authorization")[0];
    Bearer = Bearer.trim();
    if ((Bearer == environment.Bearer) || (Bearer == "Bearer")) {
      this.CategoryMenuService.Headers = new HttpHeaders();
      this.CategoryMenuService.Headers = this.CategoryMenuService.Headers.append('Authorization', 'Bearer ' + this.Token);
    }
    this.CategoryMenuService.BaseParameter.MembershipID = this.MembershipService.FormData.ID;
    this.CategoryMenuService.BaseParameter.Active = true;
    this.CategoryMenuService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryMenuService.ListChild = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryMenuService.ListParent = [];
        let IsAuthentication = false;
        for (var i = 0; i < this.CategoryMenuService.ListChild.length; i++) {
          if (this.queryStringSub == this.CategoryMenuService.ListChild[i].Code) {
            this.CategoryMenuService.ListChild[i].Active = true;
          }
          else {
            this.CategoryMenuService.ListChild[i].Active = false;
          }
          if (this.queryStringSub.indexOf(this.CategoryMenuService.ListChild[i].Code) > -1) {
            IsAuthentication = true;
          }
          this.CategoryMenuService.ListChild[i].Code = this.CategoryMenuService.ListChild[i].Code;

        }
        for (var i = 0; i < this.CategoryMenuService.ListChild.length; i++) {
          if (this.CategoryMenuService.ListChild[i].ParentID == 0) {
            this.CategoryMenuService.ListChild[i].Active = false;
            this.CategoryMenuService.ListChild[i].ListChild = [];
            for (var j = 0; j < this.CategoryMenuService.ListChild.length; j++) {
              if (this.CategoryMenuService.ListChild[i].ID == this.CategoryMenuService.ListChild[j].ParentID) {
                this.CategoryMenuService.ListChild[i].ListChild.push(this.CategoryMenuService.ListChild[j]);
                if (this.CategoryMenuService.ListChild[j].Active == true) {
                  this.CategoryMenuService.ListChild[i].Active = true;
                }
              }
            }
            this.CategoryMenuService.ListParent.push(this.CategoryMenuService.ListChild[i]);
          }
        }
        if (this.queryStringSub.includes(environment.Homepage)) {
          IsAuthentication = true;
        }
        if (this.queryStringSub.includes("Info")) {
          IsAuthentication = true;
        }
        if (this.queryStringSub.includes("Find")) {
          IsAuthentication = true;
        }
        if (this.queryStringSub.includes("Mobile")) {
          IsAuthentication = true;
        }
        this.RouterNavigate(IsAuthentication);

        this.CategoryMenuService.BaseParameter.LanguageID = Number(localStorage.getItem(environment.LanguageID));
        this.ChangeLanguage(this.CategoryMenuService.BaseParameter.LanguageID);
      },
      err => {
      }
    );
  }
  MenuClick(itemParent: CategoryMenu) {
    for (var i = 0; i < this.CategoryMenuService.ListParent.length; i++) {
      this.CategoryMenuService.ListParent[i].CSS = 'display: none;';
      this.CategoryMenuService.ListParent[i].CSS = this.Sanitizer.bypassSecurityTrustStyle(this.CategoryMenuService.ListParent[i].CSS);

      let left = this.MenuWidth * i;
      let height = itemParent.ListChild.length * 44;
      if (itemParent.ID == this.CategoryMenuService.ListParent[i].ID) {
        itemParent.Active = !itemParent.Active;
        if (itemParent.Active == true) {
          itemParent.CSS = "transform-origin: 100% 0px; opacity: 1; transform: scaleX(1) scaleY(1); display: block; width: 300px; height: " + height + "px; top: 64px; left: " + left + "px;";
          itemParent.CSS = this.Sanitizer.bypassSecurityTrustStyle(itemParent.CSS);

        }
      }
    }
  }
  Logout() {
    localStorage.setItem(environment.Token, environment.InitializationString);
    localStorage.setItem(environment.UserID, environment.InitializationString);
    localStorage.setItem(environment.UserName, environment.InitializationString);
    localStorage.setItem(environment.FullName, environment.InitializationString);
    localStorage.setItem(environment.FileName, environment.InitializationString);
    window.location.reload();
  }
  StartTimer() {
    setInterval(() => {
      this.NotificationGetByParentID();
    }, 5000)
  }
  NotificationGetByParentID() {
    this.NotificationService.GetByParentIDToListAsync().subscribe(
      res => {
        this.NotificationService.List = (res as BaseResult).List;
      },
      err => {
      },
      () => {
      }
    );
  }
  MembershipHistoryURLSave() {
    this.MembershipHistoryURLService.BaseParameter.BaseModel.URL = window.location.href;
    this.MembershipHistoryURLService.BaseParameter.BaseModel.Name = this.MembershipService.FormData.Name;
    this.MembershipHistoryURLService.BaseParameter.BaseModel.Token = this.Token;
    this.MembershipHistoryURLService.BaseParameter.BaseModel.ParentID = this.MembershipService.FormData.ID;
    this.MembershipHistoryURLService.BaseParameter.BaseModel.ParentName = this.MembershipService.FormData.UserName;
    this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress = localStorage.getItem(environment.IPRegistryIP);
    this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddressLocal = window.location.hostname;
    if ((this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress == null) || (this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress.length == 0)) {
      this.DownloadService.GetIPData().then(res => {
        this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress = res["ip"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.Type = res["user_agent"]["device"]["type"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.Longitude = res["location"]["longitude"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.Latitude = res["location"]["latitude"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.Country = res["location"]["country"]["name"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.Region = res["location"]["region"]["name"];
        this.MembershipHistoryURLService.BaseParameter.BaseModel.City = res["location"]["city"];

        localStorage.setItem(environment.IPRegistryIP, this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress);
        localStorage.setItem(environment.IPRegistryDevice, this.MembershipHistoryURLService.BaseParameter.BaseModel.Type);
        localStorage.setItem(environment.IPRegistryLongitude, this.MembershipHistoryURLService.BaseParameter.BaseModel.Longitude);
        localStorage.setItem(environment.IPRegistryLatitude, this.MembershipHistoryURLService.BaseParameter.BaseModel.Latitude);
        localStorage.setItem(environment.IPRegistryCountryName, this.MembershipHistoryURLService.BaseParameter.BaseModel.Country);
        localStorage.setItem(environment.IPRegistryRegionName, this.MembershipHistoryURLService.BaseParameter.BaseModel.Region);
        localStorage.setItem(environment.IPRegistryCityName, this.MembershipHistoryURLService.BaseParameter.BaseModel.City);

      }).catch(error => {
      }).finally(() => {
        this.MembershipHistoryURLService.SaveAsync().subscribe(
          res => {
          },
          err => {
          }
        );
      });

    }
    else {
      this.MembershipHistoryURLService.BaseParameter.BaseModel.IPAddress = localStorage.getItem(environment.IPRegistryIP);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.Type = localStorage.getItem(environment.IPRegistryDevice);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.Longitude = localStorage.getItem(environment.IPRegistryLongitude);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.Latitude = localStorage.getItem(environment.IPRegistryLatitude);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.Country = localStorage.getItem(environment.IPRegistryCountryName);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.Region = localStorage.getItem(environment.IPRegistryRegionName);
      this.MembershipHistoryURLService.BaseParameter.BaseModel.City = localStorage.getItem(environment.IPRegistryCityName);
      this.MembershipHistoryURLService.SaveAsync().subscribe(
        res => {
        },
        err => {
        }
      );
    }

  }
}
