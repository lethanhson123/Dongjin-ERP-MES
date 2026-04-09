import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialModalComponent } from '../material-modal/material-modal.component';

import { MaterialConvert } from 'src/app/shared/ERP/MaterialConvert.model';
import { MaterialConvertService } from 'src/app/shared/ERP/MaterialConvert.service';

import { CategoryMaterial } from 'src/app/shared/ERP/CategoryMaterial.model';
import { CategoryMaterialService } from 'src/app/shared/ERP/CategoryMaterial.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-material',
  templateUrl: './material.component.html',
  styleUrls: ['./material.component.css']
})
export class MaterialComponent {

  @ViewChild('MaterialSort') MaterialSort: MatSort;
  @ViewChild('MaterialPaginator') MaterialPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MaterialService: MaterialService,
    public MaterialConvertService: MaterialConvertService,
    public CategoryMaterialService: CategoryMaterialService,
    public CompanyService: CompanyService,
  ) {
    //this.URLTemplate = this.MaterialService.APIRootURL + "Download/MaterialCovert.xlsx";
    //this.URLTemplate = this.MaterialService.APIRootURL + "Download/Material.xlsx";
    this.URLTemplate = this.MaterialService.APIRootURL + "Download/MaterialLocaltion.xlsx";
    this.CompanySearch();
    this.CategoryMaterialSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.MaterialService.BaseParameter.GeneralID = 9;
    //this.MaterialConvertSearch();
  }
  MaterialCovertFileChange(event, files: FileList) {
    if (files) {
      this.MaterialConvertService.FileToUpload = files;
      this.MaterialConvertService.BaseParameter.Event = event;
    }
  }
  MaterialFileChange(event, files: FileList) {
    if (files) {
      this.MaterialService.FileToUpload = files;
      this.MaterialService.BaseParameter.Event = event;
    }
  }
  MaterialSaveAndUploadFile() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.MaterialConvertService.FileToUpload = null;
        this.MaterialConvertService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialLocaltionSaveAndUploadFile() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.SaveFamilyAndUploadFileAsync().subscribe(
      res => {       
        this.MaterialConvertService.FileToUpload = null;
        this.MaterialConvertService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);


      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialLineSaveAndUploadFile() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.SaveLineAndUploadFileAsync().subscribe(
      res => {
        this.MaterialConvertService.FileToUpload = null;
        this.MaterialConvertService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialCovertSaveAndUploadFile() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialConvertService.BaseParameter.CompanyID = this.MaterialService.BaseParameter.CompanyID;
    this.MaterialConvertService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.MaterialConvertService.FileToUpload = null;
        this.MaterialConvertService.BaseParameter.Event = null;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.MaterialService);
  }
  CategoryMaterialSearch() {
    this.CategoryMaterialService.BaseParameter.Active = true;
    this.CategoryMaterialService.ComponentGetByActiveToListAsync(this.MaterialService);
  }
  MaterialConvertSearch() {
    this.MaterialConvertService.GetAllToListAsync().subscribe(
      res => {
        this.MaterialConvertService.ListFilter001 = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      err => {
      },
      () => {
      }
    );
  }
  MaterialSearch() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.BaseParameter.SearchString = this.MaterialService.BaseParameter.SearchString.trim();
    this.MaterialService.GetByCompanyID_CategoryMaterialID_SearchStringToListAsync().subscribe(
      res => {
        this.MaterialService.List = (res as BaseResult).List;
        this.MaterialService.DataSource = new MatTableDataSource(this.MaterialService.List);
        this.MaterialService.DataSource.sort = this.MaterialSort;
        this.MaterialService.DataSource.paginator = this.MaterialPaginator;
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
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
      this.MaterialSearch();
    });
  }
  MaterialModal(ID: number) {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialAdd() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = environment.InitializationNumber;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  CreateAutoAsync() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.CreateAutoAsync().subscribe(
      res => {
        this.MaterialSearch();
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  SyncParentChildAsync() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.SyncParentChildAsync().subscribe(
      res => {
        this.MaterialSearch();
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  @ViewChild("TABLE") table: ElementRef;
  MaterialExcel() {

    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    let filename = "Material.xlsx";
    XLSX.writeFile(wb, filename);


  }
  ExportToExcel() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.ExportToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
}
