import { Component, OnInit, ViewChild } from '@angular/core';
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
import { MembershipModalComponent } from '../membership-modal/membership-modal.component';



@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.css']
})
export class MembershipComponent {
  @ViewChild('MembershipSort') MembershipSort: MatSort;
  @ViewChild('MembershipPaginator') MembershipPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public MembershipService: MembershipService,

  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.MembershipSearch();
  }
  ngOnDestroy(): void {

  }
  
  MembershipSearch() {
    this.MembershipService.Search(this.MembershipSort, this.MembershipPaginator);
  }
  Add() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.BaseParameter.ID = environment.InitializationNumber;
    this.MembershipService.GetByIDAsync().subscribe(
      res => {
        this.MembershipService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MembershipModal(this.MembershipService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
  MembershipModal(element: Membership) {
    this.MembershipService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MembershipModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.MembershipSearch();
    });
  }
  CreateAutoAsync() {
    this.MembershipService.IsShowLoading = true;
    this.MembershipService.CreateAutoAsync().subscribe(
      res => {
        this.MembershipSearch();
      },
      err => {
      },
      () => {
        this.MembershipService.IsShowLoading = false;
      }
    );
  }
}
