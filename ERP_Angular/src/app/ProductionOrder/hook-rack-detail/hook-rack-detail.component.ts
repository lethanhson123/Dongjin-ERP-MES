import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { trackmtim } from 'src/app/shared/MES/trackmtim.model';
import { trackmtimService } from 'src/app/shared/MES/trackmtim.service';

@Component({
  selector: 'app-hook-rack-detail',
  templateUrl: './hook-rack-detail.component.html',
  styleUrls: ['./hook-rack-detail.component.css']
})
export class HookRackDetailComponent implements OnInit {

  @ViewChild('trackmtimSort') trackmtimSort: MatSort;
  @ViewChild('trackmtimPaginator') trackmtimPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<HookRackDetailComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public trackmtimService: trackmtimService,


  ) { }

  ngOnInit(): void {
    this.trackmtimSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  trackmtimSearch() {
    if (this.trackmtimService.BaseParameter.SearchString.length > 0) {
      this.trackmtimService.BaseParameter.SearchString = this.trackmtimService.BaseParameter.SearchString.trim();
      if (this.trackmtimService.DataSource) {
        this.trackmtimService.DataSource.filter = this.trackmtimService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.trackmtimService.IsShowLoading = true;
      this.trackmtimService.GetByCompanyID_LEADNM_Begin_EndToListAsync().subscribe(
        res => {
          this.trackmtimService.List = (res as BaseResult).List.sort((a, b) => (a.RACKDTM < b.RACKDTM ? 1 : -1));
          this.trackmtimService.BaseParameter.Count = 0;
          for (let i = 0; i < this.trackmtimService.List.length; i++) {
            this.trackmtimService.BaseParameter.Count = this.trackmtimService.BaseParameter.Count + this.trackmtimService.List[i].QTY;
          }
          this.trackmtimService.DataSource = new MatTableDataSource(this.trackmtimService.List);
          this.trackmtimService.DataSource.sort = this.trackmtimSort;
          this.trackmtimService.DataSource.paginator = this.trackmtimPaginator;
        },
        err => {
        },
        () => {
          this.trackmtimService.IsShowLoading = false;
        }
      );
    }
  }
  trackmtimDownload() {

  }
  trackmtimSave(element: trackmtim) {
    this.trackmtimService.IsShowLoading = true;
    this.trackmtimService.BaseParameter.BaseModel = element;
    this.trackmtimService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.trackmtimService.IsShowLoading = false;
      }
    );
  }
}