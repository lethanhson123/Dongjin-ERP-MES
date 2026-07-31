import {
  Component,
  OnInit,
  Inject,
  ElementRef,
  ViewChild,
  Renderer2,
} from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Application } from 'src/app/shared/ERP/Application.model';
import { ApplicationService } from 'src/app/shared/ERP/Application.service';

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.css'],
})
export class OverviewComponent implements OnInit {
  MatKhauIsActive: boolean = true;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    private Renderer: Renderer2,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,
    public ApplicationService: ApplicationService,
  ) {
    this.MembershipSearch();
    this.MembershipApplicationSearch();
  }

  ngOnInit(): void {}

  StartTimer() {
    setInterval(() => {
      if (this.ApplicationService.List.length <= 3) {
        let DateTime01 = new Date(localStorage.getItem(environment.DateTime));
        let DateTime02 = new Date();
        let diffMs = Math.abs(DateTime02.getTime() - DateTime01.getTime());
        let diffHours = diffMs / (1000 * 60 * 60);
        if (diffHours >= 1) {
          localStorage.setItem(environment.UserID, '0');
          localStorage.setItem(environment.MembershipID, '0');
          let url = window.location.origin + '/#/' + environment.Homepage;
          window.location.href = url;
        }
      }
    }, 600000);
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.UserName = localStorage.getItem(
      environment.UserName,
    );
    this.MembershipService.BaseParameter.Name = localStorage.getItem(
      environment.FullName,
    );
  }
  Membership() {
    let url = window.location.origin + '/#/Membership';
    window.location.href = url;
  }
  Logout() {
    localStorage.setItem(environment.Token, environment.InitializationString);
    localStorage.setItem(environment.UserID, environment.InitializationString);
    localStorage.setItem(
      environment.MembershipID,
      environment.InitializationString,
    );
    localStorage.setItem(
      environment.UserName,
      environment.InitializationString,
    );
    localStorage.setItem(
      environment.FullName,
      environment.InitializationString,
    );
    localStorage.setItem(
      environment.FileName,
      environment.InitializationString,
    );
    let url = window.location.origin + '/#/Homepage';
    window.location.href = url;
  }
  MembershipApplicationSearch() {
    this.ApplicationService.IsShowLoading = true;
    this.ApplicationService.BaseParameter.Code = localStorage.getItem(
      environment.UserName,
    );
    this.ApplicationService.BaseParameter.Active = true;
    this.ApplicationService.BaseParameter.MembershipID = Number(
      localStorage.getItem(environment.MembershipID),
    );
    if (this.ApplicationService.BaseParameter.MembershipID) {
      this.ApplicationService.GetByMembershipID_ActiveToListAsync().subscribe(
        (res) => {
          this.ApplicationService.List = (res as BaseResult).List.sort(
            (a, b) => (a.SortOrder > b.SortOrder ? 1 : -1),
          );
          for (let i = 0; i < this.ApplicationService.List.length; i++) {
            this.ApplicationService.List[i].Code =
              this.ApplicationService.List[i].Code +
              '?Token=' +
              localStorage.getItem(environment.Token);
            const img = new Image();
            img.src = this.ApplicationService.List[i].FileName;

            img.onload = () => {};
            img.onerror = () => {
              this.ApplicationService.List[i].Code =
                this.ApplicationService.List[i].Note +
                '?Token=' +
                localStorage.getItem(environment.Token);
              this.ApplicationService.List[i].FileName =
                'http://api.dongjin-global.com:83/Upload/Application/mes-djm_20260713151125.jpg';
            };
          }
          this.StartTimer();
        },
        (err) => {},
        () => {
          this.ApplicationService.IsShowLoading = false;
        },
      );
    } else {
      let url = window.location.origin + '/#/' + environment.Homepage;
      window.location.href = url;
    }
  }
}
