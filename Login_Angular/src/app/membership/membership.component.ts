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

import { Membership } from 'src/app/shared/ERP/Membership.model';
import { MembershipService } from 'src/app/shared/ERP/Membership.service';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.css'],
})
export class MembershipComponent implements OnInit {
  MatKhauIsActive: boolean = true;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    private Renderer: Renderer2,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MembershipService: MembershipService,
  ) {
    this.MembershipSearch();
  }

  ngOnInit(): void {}

  MatKhauChangeType() {
    this.MatKhauIsActive = !this.MatKhauIsActive;
  }
  MembershipSearch() {
    this.MembershipService.BaseParameter.ID = Number(
      localStorage.getItem(environment.MembershipID),
    );    
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.GetByIDAsync().subscribe(
      (res) => {
        this.MembershipService.BaseResult = res as BaseResult;
        this.MembershipService.BaseParameter.BaseModel =
          this.MembershipService.BaseResult.BaseModel;
      },
      (err) => {},
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }
  Save() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.SaveAsync().subscribe(
      (res) => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      (err) => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      },
    );
  }
}
