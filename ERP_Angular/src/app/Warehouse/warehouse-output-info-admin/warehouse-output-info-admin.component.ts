import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';

import { WarehouseOutputDetail } from 'src/app/shared/ERP/WarehouseOutputDetail.model';
import { WarehouseOutputDetailService } from 'src/app/shared/ERP/WarehouseOutputDetail.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';



import { WarehouseOutputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseOutputDetailBarcodeMaterial.model';
import { WarehouseOutputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcodeMaterial.service';

import { WarehouseOutputMaterial } from 'src/app/shared/ERP/WarehouseOutputMaterial.model';
import { WarehouseOutputMaterialService } from 'src/app/shared/ERP/WarehouseOutputMaterial.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';


import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';
import { WarehouseOutputDetailBarcodeModalComponent } from '../warehouse-output-detail-barcode-modal/warehouse-output-detail-barcode-modal.component';
import { WarehouseOutputDetailBarcodeMaterialModalComponent } from '../warehouse-output-detail-barcode-material-modal/warehouse-output-detail-barcode-material-modal.component';


@Component({
  selector: 'app-warehouse-output-info-admin',
  templateUrl: './warehouse-output-info-admin.component.html',
  styleUrls: ['./warehouse-output-info-admin.component.css']
})
export class WarehouseOutputInfoAdminComponent {

  @ViewChild('WarehouseOutputDetailSort') WarehouseOutputDetailSort: MatSort;
  @ViewChild('WarehouseOutputDetailPaginator') WarehouseOutputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;



  @ViewChild('WarehouseOutputMaterialSort') WarehouseOutputMaterialSort: MatSort;
  @ViewChild('WarehouseOutputMaterialPaginator') WarehouseOutputMaterialPaginator: MatPaginator;

  @ViewChild('Barcode') Barcode!: ElementRef;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputService: WarehouseOutputService,
    public WarehouseOutputDetailService: WarehouseOutputDetailService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public WarehouseOutputDetailBarcodeMaterialService: WarehouseOutputDetailBarcodeMaterialService,
    public WarehouseOutputMaterialService: WarehouseOutputMaterialService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
    public WarehouseRequestService: WarehouseRequestService,
  ) {
    this.WarehouseOutputService.BaseParameter.BaseModel = {
    };

    this.WarehouseOutputDetailService.List = [];
    this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
    this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
    this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;

    this.WarehouseOutputDetailBarcodeService.List = [];
    this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
    this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
    this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;


    this.WarehouseRequestSearch();
    this.CategoryUnitSearch();
    //this.MaterialSearch();    
    this.CompanySearch();
    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseOutputService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.WarehouseOutputSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseOutputService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseOutputService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseOutputService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseOutputService);
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.WarehouseRequestService.ComponentGetByMembershipID_ConfirmToListAsync(this.WarehouseOutputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.WarehouseOutputService);
  }
  MaterialSearchByWarehouseOutputID() {
    this.MaterialService.BaseParameter.GeneralID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.MaterialService.ComponentGetByWarehouseOutputIDToListAsync(this.WarehouseOutputService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.ComponentGetByActiveToListAsync(this.WarehouseOutputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  MaterialFilter02(searchString: string) {
    this.MaterialService.Filter02(searchString);
  }
  Date(value) {
    this.WarehouseOutputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  DateDateScan(element: WarehouseOutputDetailBarcode, value) {
    element.DateScan = new Date(value);
  }
  WarehouseOutputFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseOutputService.FileToUpload = files;
      this.WarehouseOutputService.File = files.item(0);
    }
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CompanyChange();
        this.WarehouseOutputDetailSearch();
        this.MaterialSearchByWarehouseOutputID();
        this.WarehouseOutputDetailBarcodeSearch();
        this.WarehouseOutputMaterialSearch();
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("WarehouseOutputInfo/" + ID);
    this.WarehouseOutputService.BaseParameter.ID = ID;
    this.WarehouseOutputSearch();
  }
  Save() {
    //this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.Add(this.WarehouseOutputService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
    this.NotificationService.warn(environment.SaveSuccess);
  }
  WarehouseOutputPrint() {
    this.WarehouseOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.ID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputService.BaseParameter.Active = this.WarehouseOutputService.BaseParameter.BaseModel.IsComplete;
    this.WarehouseOutputService.PrintGroup2026Async().subscribe(
      res => {
        this.WarehouseOutputService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseOutputService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailSearch() {
    this.WarehouseOutputDetailService.List = [];
    if (this.WarehouseOutputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseOutputDetailService.BaseParameter.SearchString.length > 0) {
        this.WarehouseOutputDetailService.BaseParameter.SearchString = this.WarehouseOutputDetailService.BaseParameter.SearchString.trim();
        if (this.WarehouseOutputDetailService.DataSource) {
          this.WarehouseOutputDetailService.DataSource.filter = this.WarehouseOutputDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {

        this.WarehouseOutputDetailService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
        this.WarehouseOutputService.IsShowLoading = true;
        this.WarehouseOutputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseOutputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
            this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
            this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseOutputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseOutputDetailService.List = [];
      this.WarehouseOutputDetailService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailService.List);
      this.WarehouseOutputDetailService.DataSource.sort = this.WarehouseOutputDetailSort;
      this.WarehouseOutputDetailService.DataSource.paginator = this.WarehouseOutputDetailPaginator;
    }
  }
  WarehouseOutputDetailDelete(element: WarehouseOutputDetail) {
    this.WarehouseOutputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailSave(element: WarehouseOutputDetail) {
    //this.WarehouseOutputService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputDetailService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
    this.WarehouseOutputDetailSearch();
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
  MaterialModal(element: WarehouseOutputDetail) {
    this.WarehouseOutputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  MaterialChange(element: WarehouseOutputDetail) {
    let List = this.MaterialService.ListFilter.filter(item => item.ID == element.MaterialID);
    if (List) {
      if (List.length > 0) {
      }
    }
  }
  WarehouseOutputDetailBarcodeModal(element: WarehouseOutputDetail) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.GeneralID = element.ID;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputDetailBarcodeModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseOutputDetailBarcodeSearch() {
    this.WarehouseOutputDetailBarcodeService.List = [];
    if (this.WarehouseOutputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
        this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.trim();
        if (this.WarehouseOutputDetailBarcodeService.DataSource) {
          this.WarehouseOutputDetailBarcodeService.DataSource.filter = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseOutputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
        this.WarehouseOutputService.IsShowLoading = true;
        this.WarehouseOutputDetailBarcodeService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
            this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
            this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;
          },
          err => {
          },
          () => {
            this.WarehouseOutputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseOutputDetailBarcodeService.List = [];
      this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
      this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
      this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;
    }
  }
  WarehouseOutputDetailBarcodeDelete(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputDetailBarcodeSave(element: WarehouseOutputDetailBarcode) {
    //this.WarehouseOutputService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseOutputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputDetailSearch();
        this.WarehouseOutputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
    this.WarehouseOutputDetailSearch();
    this.WarehouseOutputDetailBarcodeSearch();
  }
  @ViewChild("WarehouseOutputDetailTABLE") WarehouseOutputDetailTable: ElementRef;
  WarehouseOutputDetailExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.WarehouseOutputDetailTable.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = this.WarehouseOutputService.BaseParameter.BaseModel.Code + "-WarehouseOutputDetail.xlsx";
    XLSX.writeFile(wb, filename);
  }
  @ViewChild("WarehouseOutputDetailBarcodeTABLE") WarehouseOutputDetailBarcodeTable: ElementRef;
  WarehouseOutputDetailBarcodeExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.WarehouseOutputDetailBarcodeTable.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = this.WarehouseOutputService.BaseParameter.BaseModel.Code + "-WarehouseOutputDetailBarcode.xlsx";
    XLSX.writeFile(wb, filename);
  }

  WarehouseOutputMaterialSearch() {
    if (this.WarehouseOutputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseOutputMaterialService.BaseParameter.SearchString.length > 0) {
        this.WarehouseOutputMaterialService.BaseParameter.SearchString = this.WarehouseOutputMaterialService.BaseParameter.SearchString.trim();
        if (this.WarehouseOutputMaterialService.DataSource) {
          this.WarehouseOutputMaterialService.DataSource.filter = this.WarehouseOutputMaterialService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseOutputMaterialService.BaseParameter.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
        this.WarehouseOutputService.IsShowLoading = true;
        this.WarehouseOutputMaterialService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseOutputMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
            this.WarehouseOutputMaterialService.DataSource = new MatTableDataSource(this.WarehouseOutputMaterialService.List);
            this.WarehouseOutputMaterialService.DataSource.sort = this.WarehouseOutputMaterialSort;
            this.WarehouseOutputMaterialService.DataSource.paginator = this.WarehouseOutputMaterialPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseOutputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseOutputMaterialService.List = [];
      this.WarehouseOutputMaterialService.DataSource = new MatTableDataSource(this.WarehouseOutputMaterialService.List);
      this.WarehouseOutputMaterialService.DataSource.sort = this.WarehouseOutputMaterialSort;
      this.WarehouseOutputMaterialService.DataSource.paginator = this.WarehouseOutputMaterialPaginator;
    }
  }
  WarehouseOutputMaterialDelete(element: WarehouseOutputMaterial) {
    this.WarehouseOutputMaterialService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseOutputService.IsShowLoading = true;
      this.WarehouseOutputMaterialService.RemoveAsync().subscribe(
        res => {
          this.WarehouseOutputMaterialSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseOutputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseOutputMaterialSave(element: WarehouseOutputMaterial) {
    this.WarehouseOutputService.IsShowLoading = true;
    element.ParentID = this.WarehouseOutputService.BaseParameter.BaseModel.ID;
    this.WarehouseOutputMaterialService.BaseParameter.BaseModel = element;
    this.WarehouseOutputMaterialService.SaveAsync().subscribe(
      res => {
        this.WarehouseOutputMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseOutputService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeMaterialModal(element: WarehouseOutputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeMaterialService.BaseParameter.GeneralID = element.ID;    
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputDetailBarcodeMaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
}
