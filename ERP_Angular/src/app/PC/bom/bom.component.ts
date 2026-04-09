import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { BOM } from 'src/app/shared/ERP/BOM.model';
import { BOMService } from 'src/app/shared/ERP/BOM.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-bom',
  templateUrl: './bom.component.html',
  styleUrls: ['./bom.component.css']
})
export class BOMComponent {

  @ViewChild('BOMSort') BOMSort: MatSort;
  @ViewChild('BOMPaginator') BOMPaginator: MatPaginator;

  URLTemplate: string;
  URLTemplateRawMaterial: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMService: BOMService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.URLTemplate = this.BOMService.APIRootURL + "Download/BOMLead.xlsx";
    this.URLTemplateRawMaterial = this.BOMService.APIRootURL + "Download/BOMRawMaterial.xlsx";
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.BOMSearch();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.BOMService);
  }
  BOMSearch() {
    this.BOMService.IsShowLoading = true;
    if (this.BOMService.BaseParameter.SearchString.length > 0) {
      this.BOMService.BaseParameter.SearchString = this.BOMService.BaseParameter.SearchString.trim();
      this.BOMService.GetByCompanyID_SearchStringToListAsync().subscribe(
        res => {
          this.BOMService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.BOMService.DataSource = new MatTableDataSource(this.BOMService.List);
          this.BOMService.DataSource.sort = this.BOMSort;
          this.BOMService.DataSource.paginator = this.BOMPaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
    else {
      this.BOMService.BaseParameter.Page = 0;
      this.BOMService.BaseParameter.PageSize = 100;
      this.BOMService.GetByCompanyID_PageAndPageSizeToListAsync().subscribe(
        res => {
          this.BOMService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
          this.BOMService.DataSource = new MatTableDataSource(this.BOMService.List);
          this.BOMService.DataSource.sort = this.BOMSort;
          this.BOMService.DataSource.paginator = this.BOMPaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
    //this.BOMService.Search(this.BOMSort, this.BOMPaginator);
  }
  BOMDelete(element: BOM) {
    this.BOMService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.BOMService.ComponentDelete(this.BOMSort, this.BOMPaginator));
  }
  CreateAutoAsync() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.CreateAutoAsync().subscribe(
      res => {        
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  MaterialModalOpen(element: Material) {
    this.MaterialService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
  MaterialModal(ID: number) {
    this.BOMService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMChange(event, files: FileList) {
    if (files) {
      this.BOMService.FileToUpload = files;
      this.BOMService.BaseParameter.Active = false;
      this.BOMService.BaseParameter.Event = event;
    }
  }
  Save() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.SaveByLEADAndUploadFileAsync().subscribe(
      res => {
        //this.BOMService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.BOMSearch();
        this.BOMService.FileToUpload = null;
        this.BOMService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  SaveBOMRawMaterial() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.SaveByBOMRawMaterialAndUploadFileAsync().subscribe(
      res => {
        this.BOMSearch();
        this.BOMService.FileToUpload = null;
        this.BOMService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMDownload() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.ExportBOMLeadByECNToExcelAsync().subscribe(
      res => {
        this.BOMService.ListParent = (res as BaseResult).List;
        for (let i = 0; i < this.BOMService.ListParent.length; i++) {
          window.open(this.BOMService.ListParent[i].Code, "_blank");
        }

      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
}
