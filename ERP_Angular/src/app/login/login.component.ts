import { Component, OnInit, Inject, ElementRef, ViewChild, Renderer2 } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  private bodyClickListener?: () => void;
  MatKhauIsActive: boolean = true;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    private Renderer: Renderer2,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,

  ) {
    let MembershipID = 0;
    let UserID = localStorage.getItem(environment.UserID);
    if (UserID) {
      MembershipID = Number(UserID);
      if (MembershipID > 0) {
        this.Router.navigate(['/' + environment.Homepage]);
      }
    }
  }

  ngOnInit(): void {
    this.BodyListener();
  }
  ngAfterViewInit() {

  }
  ngOnDestroy(): void {

  }
  MatKhauChangeType() {
    this.MatKhauIsActive = !this.MatKhauIsActive;
  }
  BodyListener() {
    this.bodyClickListener = this.Renderer.listen(
      document.body,
      'keyup',
      (event) => {
        if (event) {
          if (event["keyCode"] == 13) {
            //this.Login();
          }
        }
      }
    );
  }
  Login() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.AuthenticationAsync().subscribe(
      res => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.MembershipService.BaseParameter.BaseModel) {
          if (this.MembershipService.BaseParameter.BaseModel.ID > 0) {
            localStorage.setItem(environment.Token, this.MembershipService.BaseParameter.BaseModel.Code);
            localStorage.setItem(environment.UserName, this.MembershipService.BaseParameter.BaseModel.UserName);
            localStorage.setItem(environment.FullName, this.MembershipService.BaseParameter.BaseModel.Name);
            localStorage.setItem(environment.FileName, this.MembershipService.BaseParameter.BaseModel.FileName);

            try {
              localStorage.setItem(environment.UserID, this.MembershipService.BaseParameter.BaseModel.ID.toString());
              localStorage.setItem(environment.MembershipID, this.MembershipService.BaseParameter.BaseModel.ID.toString());
            } catch (error) {
            }
            try {
              localStorage.setItem(environment.CompanyID, this.MembershipService.BaseParameter.BaseModel.CompanyID.toString());
            } catch (error) {
            }
            try {
              localStorage.setItem(environment.CategoryDepartmentID, this.MembershipService.BaseParameter.BaseModel.CategoryDepartmentID.toString());
            } catch (error) {
            }
            this.MembershipService.FormData = this.MembershipService.BaseParameter.BaseModel;

            window.location.reload();

            // let url = environment.ERPURL + environment.Login + "/" + this.MembershipService.BaseParameter.BaseModel.ID;
            // window.location.href = url;
          }
          else {
            this.NotificationService.success(environment.LoginNotSuccess);
          }
        }
      },
      err => {
        this.NotificationService.warn(environment.LoginNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
}
