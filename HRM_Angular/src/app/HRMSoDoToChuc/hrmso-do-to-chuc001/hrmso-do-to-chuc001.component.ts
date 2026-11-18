import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { HRMSoDoToChuc } from 'src/app/shared/ERP/HRMSoDoToChuc.model';
import { HRMSoDoToChucService } from 'src/app/shared/ERP/HRMSoDoToChuc.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';
import { HRMSoDoToChucModalComponent } from '../hrmso-do-to-chuc-modal/hrmso-do-to-chuc-modal.component';

@Component({
  selector: 'app-hrmso-do-to-chuc001',
  templateUrl: './hrmso-do-to-chuc001.component.html',
  styleUrls: ['./hrmso-do-to-chuc001.component.css']
})
export class HRMSoDoToChuc001Component implements OnInit {

 @ViewChild('HRMSoDoToChucSort') HRMSoDoToChucSort: MatSort;
  @ViewChild('HRMSoDoToChucPaginator') HRMSoDoToChucPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public HRMSoDoToChucService: HRMSoDoToChucService,
    public CompanyService: CompanyService,
  ) {
    this.HRMSoDoToChucService.BaseParameter.Count = 1000;
    this.CompanySearch();
  }

  ngOnInit(): void {}
  ngAfterViewInit() {}
  zoomLevel: number = 1;  
  zoomIn() {
    this.zoomLevel = Math.min(this.zoomLevel + 0.1, 2);
  }

  zoomOut() {
    this.zoomLevel = Math.max(this.zoomLevel - 0.1, 0.1);
  }

  resetZoom() {
    this.zoomLevel = 1; // Trở về mặc định 100%
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      (res) => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
          this.HRMSoDoToChucService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
          this.HRMSoDoToChucSearch();
        }
      },
      (err) => {},
      () => {},
    );
  }

  buildTree(list: any[], parentId: any = 0, visited: Set<any> = new Set()): any[] {
    return list
      .filter((item) => item.ParentID == parentId)
      .map((item) => {
        if (visited.has(item.ID)) {
          console.warn(`Phát hiện vòng lặp vô hạn ở ID: ${item.ID}`);
          return null;
        }
        visited.add(item.ID);

        const children = this.buildTree(list, item.ID, visited);

        return {
          ...item,
          children: children.filter((c) => c !== null),
        };
      })
      .filter((item) => item !== null);
  }
  HRMSoDoToChucSearch() {
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.BaseParameter.Active = true;
    this.HRMSoDoToChucService.GetByCompanyIDAndActiveToListAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseResult = res as BaseResult;
        this.HRMSoDoToChucService.List = this.HRMSoDoToChucService.BaseResult.List;
        this.HRMSoDoToChucService.BaseParameter.LanguageID = Number(localStorage.getItem(environment.LanguageID));
        for (let i = 0; i < this.HRMSoDoToChucService.List.length; i++) {
          switch (this.HRMSoDoToChucService.BaseParameter.LanguageID) {
            case 1:
              this.HRMSoDoToChucService.List[i].CategoryDepartmentName = this.HRMSoDoToChucService.List[i].CategoryDepartmentName01;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName01;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission01;
              break;
            case 2:
              this.HRMSoDoToChucService.List[i].CategoryDepartmentName = this.HRMSoDoToChucService.List[i].CategoryDepartmentName02;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName02;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission02;
              break;
            case 3:
              this.HRMSoDoToChucService.List[i].CategoryDepartmentName = this.HRMSoDoToChucService.List[i].CategoryDepartmentName03;
              this.HRMSoDoToChucService.List[i].CategoryPositionName = this.HRMSoDoToChucService.List[i].CategoryPositionName03;
              this.HRMSoDoToChucService.List[i].Mission = this.HRMSoDoToChucService.List[i].Mission03;
              break;
          }
        }
      },
      (err) => {},
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }
  HRMSoDoToChucSearch001(ID: number) {
    let List = this.HRMSoDoToChucService.List.filter((o) => o.ParentID == ID).sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));    
    if (List.length > 1) {
      this.HRMSoDoToChucService.BaseParameter.Sum = this.HRMSoDoToChucService.BaseParameter.Sum - this.HRMSoDoToChucService.BaseParameter.Count / 2;
    }
    return List;
  }
  HRMSoDoToChucAdd(ID: number) {
    this.HRMSoDoToChucService.IsShowLoading = true;
    this.HRMSoDoToChucService.BaseParameter.ID = ID;
    this.HRMSoDoToChucService.GetByIDAsync().subscribe(
      (res) => {
        this.HRMSoDoToChucService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.HRMSoDoToChucService.BaseParameter.BaseModel.CompanyID = this.HRMSoDoToChucService.BaseParameter.CompanyID;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(HRMSoDoToChucModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
          this.HRMSoDoToChucSearch();
        });
      },
      (err) => {},
      () => {
        this.HRMSoDoToChucService.IsShowLoading = false;
      },
    );
  }  
}