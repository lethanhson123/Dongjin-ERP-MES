import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { WarehouseInputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.model';
import { WarehouseInputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.service';

import { WarehouseInputMaterial } from 'src/app/shared/ERP/WarehouseInputMaterial.model';
import { WarehouseInputMaterialService } from 'src/app/shared/ERP/WarehouseInputMaterial.service';

import { WarehouseInputDetailCount } from 'src/app/shared/ERP/WarehouseInputDetailCount.model';
import { WarehouseInputDetailCountService } from 'src/app/shared/ERP/WarehouseInputDetailCount.service';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

import { MaterialConvert } from 'src/app/shared/ERP/MaterialConvert.model';
import { MaterialConvertService } from 'src/app/shared/ERP/MaterialConvert.service';


import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';
import { WarehouseInputDetailBarcodeModalComponent } from '../warehouse-input-detail-barcode-modal/warehouse-input-detail-barcode-modal.component';
import { WarehouseOutputDetailBarcodeHistoryModalComponent } from '../warehouse-output-detail-barcode-history-modal/warehouse-output-detail-barcode-history-modal.component';
import { WarehouseInputDetailBarcodeMaterialModalComponent } from '../warehouse-input-detail-barcode-material-modal/warehouse-input-detail-barcode-material-modal.component';


@Component({
  selector: 'app-warehouse-input-info-admin',
  templateUrl: './warehouse-input-info-admin.component.html',
  styleUrls: ['./warehouse-input-info-admin.component.css']
})
export class WarehouseInputInfoAdminComponent {

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailCountSort') WarehouseInputDetailCountSort: MatSort;
  @ViewChild('WarehouseInputDetailCountPaginator') WarehouseInputDetailCountPaginator: MatPaginator;

  @ViewChild('WarehouseInputMaterialSort') WarehouseInputMaterialSort: MatSort;
  @ViewChild('WarehouseInputMaterialPaginator') WarehouseInputMaterialPaginator: MatPaginator;

  IsWarehouseInputDetailBarcodeActiveAll: boolean = false;
  URLTemplate: string;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public WarehouseInputDetailService: WarehouseInputDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public WarehouseInputDetailBarcodeMaterialService: WarehouseInputDetailBarcodeMaterialService,
    public WarehouseInputMaterialService: WarehouseInputMaterialService,
    public WarehouseInputDetailCountService: WarehouseInputDetailCountService,

    public ProductionOrderService: ProductionOrderService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public MaterialConvertService: MaterialConvertService,
    public CategoryUnitService: CategoryUnitService,

    public InvoiceInputService: InvoiceInputService,
    public WarehouseOutputService: WarehouseOutputService,
  ) {
    this.URLTemplate = this.WarehouseInputService.APIRootURL + "Download/WarehouseInputDetailStock.xlsx";
    this.WarehouseInputDetailService.BaseParameter.Active = false;

    this.WarehouseInputService.BaseParameter.BaseModel = {
    };

    this.WarehouseInputDetailService.List = [];
    this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
    this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
    this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;

    this.WarehouseInputDetailBarcodeService.List = [];
    this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
    this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
    this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;

    this.WarehouseInputDetailCountService.List = [];
    this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
    this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
    this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;

    this.CompanySearch();
    //this.ProductionOrderSearch();
    //this.InvoiceInputSearch();

    this.CategoryUnitSearch();
    this.MaterialConvertSearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseInputService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.WarehouseInputSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseInputService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseInputService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseInputService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseInputService);
  }
  InvoiceInputSearch() {
    this.InvoiceInputService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseInputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.WarehouseInputService);
  }
  MaterialSearchByByWarehouseInputID() {
    this.MaterialService.BaseParameter.GeneralID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.MaterialService.ComponentGetByWarehouseInputIDToListAsync(this.WarehouseOutputService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.ComponentGetByActiveToListAsync(this.WarehouseInputService);
  }
  MaterialConvertSearch() {
    this.MaterialConvertService.ComponentGetAllToListAsync(this.WarehouseInputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  MaterialFilter02(searchString: string) {
    this.MaterialService.Filter02(searchString);
  }

  ProductionOrderSearch() {
    this.ProductionOrderService.ComponentGetByMembershipID_Active_IsCompleteToListAsync(this.WarehouseInputService);
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.ComponentGetByMembershipID_Active_IsCompleteToListAsync(this.WarehouseInputService);
  }
  Date(value) {
    this.WarehouseInputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  DateDateScan(element: WarehouseInputDetailBarcode, value) {
    element.DateScan = new Date(value);
  }
  DateDateBegin(element: WarehouseInputDetail, value) {
    element.DateBegin = new Date(value);
  }
  DateDateEnd(element: WarehouseInputDetail, value) {
    element.DateEnd = new Date(value);
  }
  WarehouseInputFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseInputService.FileToUpload = files;
    }
  }
  WarehouseInputSearch() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CompanyChange();
        //this.WarehouseOutputSearch();
        //this.MaterialSearchByByWarehouseInputID();
        this.WarehouseInputDetailSearch();
        this.WarehouseInputDetailBarcodeSearch();
        this.WarehouseInputDetailCountSearch();
        this.WarehouseInputMaterialSearch();
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("WarehouseInputInfoAdmin/" + ID);
    this.WarehouseInputService.BaseParameter.ID = ID;
    this.WarehouseInputSearch();
  }
  Save() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.IsStock == true) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputService.SaveAndUploadFileStockAsync().subscribe(
        res => {
          this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.Add(this.WarehouseInputService.BaseParameter.BaseModel.ID);
          this.NotificationService.warn(environment.SaveSuccess);
        },
        err => {
          this.NotificationService.warn(environment.SaveNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
    else {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputService.SaveAndUploadFileAsync().subscribe(
        res => {
          this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.Add(this.WarehouseInputService.BaseParameter.BaseModel.ID);
          this.NotificationService.warn(environment.SaveSuccess);
        },
        err => {
          this.NotificationService.warn(environment.SaveNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailService.BaseParameter.SearchString = this.WarehouseInputDetailService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputDetailService.DataSource) {
          this.WarehouseInputDetailService.DataSource.filter = this.WarehouseInputDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputDetailService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            if (this.WarehouseInputDetailService.List && this.WarehouseInputDetailService.List.length > 0) {
              for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
                this.WarehouseInputDetailService.List[i].ListChild = this.MaterialConvertService.ListFilter.filter(o => o.Active == true && o.ParentID == this.WarehouseInputDetailService.List[i].MaterialID);
                this.MaterialConvertService.FormData = {
                  ParentID: 0,
                  Code: this.WarehouseInputDetailService.List[i].MaterialName,
                }
                this.WarehouseInputDetailService.List[i].ListChild.push(this.MaterialConvertService.FormData);
                this.WarehouseInputDetailService.List[i].ListChild = this.WarehouseInputDetailService.List[i].ListChild.sort((a, b) => (a.ParentID > b.ParentID ? 1 : -1));
              }
            }
            this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
            this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
            this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailService.List = [];
      this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
      this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
      this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
    }
  }
  WarehouseInputDetailActiveChange(element: WarehouseInputDetail) {
    if (element.ID > 0) {
      if (element.Active == true) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(element.ID);
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.MaterialName;
        this.WarehouseInputDetailBarcodeSearch();
      }
      else {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID = this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.filter(o => o != element.ID);
      }
    }
  }
  WarehouseInputDetailActiveAllChange() {
    this.WarehouseInputDetailService.BaseParameter.ListID = [];
    for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
      if (this.WarehouseInputDetailService.List[i].ID > 0) {
        this.WarehouseInputDetailService.List[i].Active = this.WarehouseInputDetailService.BaseParameter.Active;
        if (this.WarehouseInputDetailService.List[i].Active == true) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(this.WarehouseInputDetailService.List[i].ID);
        }
      }
    }
  }
  WarehouseInputDetailSync() {
    for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
      if (this.WarehouseInputDetailService.List[i].ID > 0) {
        this.WarehouseInputDetailService.List[i].Active = true;
      }
    }
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailService.BaseParameter.List = this.WarehouseInputDetailService.List;
    this.WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailDelete(element: WarehouseInputDetail) {
    this.WarehouseInputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailSave(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailSearch();
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
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
  MaterialModal(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeModal(element: WarehouseInputDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.GeneralID = element.MaterialID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = element.ParentID;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseInputDetailBarcodeModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseOutputDetailBarcodeModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseOutputDetailBarcodeService.BaseParameter.ParentID = element.ParentID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Barcode = element.Barcode;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.Active = true;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseOutputDetailBarcodeHistoryModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseInputDetailBarcodeSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputDetailBarcodeService.DataSource) {
          this.WarehouseInputDetailBarcodeService.DataSource.filter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailBarcodeService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
            this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
            this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailBarcodeService.List = [];
      this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
      this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
      this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
    }
  }
  WarehouseInputDetailBarcodeDelete(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailBarcodeSave(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
    //this.WarehouseInputDetailBarcodeSearch();
  }
  WarehouseInputDetailBarcodePrintByParentID() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByParentIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrintByListWarehouseInputDetailID() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByListWarehouseInputDetailIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeDownload() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.ExportToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailPrint(element: WarehouseInputDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByWarehouseInputDetailIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrint(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputDetailBarcodeService.PrintAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  IsWarehouseInputDetailBarcodeActiveAllChange() {
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
      this.WarehouseInputDetailBarcodeService.List[i].Active = this.IsWarehouseInputDetailBarcodeActiveAll;
    }
  }
  WarehouseInputDetailBarcodeSaveAll() {
    this.WarehouseInputService.IsShowLoading = true;
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
      this.WarehouseInputDetailBarcodeService.List[i].Active = this.IsWarehouseInputDetailBarcodeActiveAll;
    }
    this.WarehouseInputDetailBarcodeService.BaseParameter.List = this.WarehouseInputDetailBarcodeService.List;
    this.WarehouseInputDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeSync() {
    this.WarehouseInputService.IsShowLoading = true;   
    this.WarehouseInputDetailBarcodeService.SyncStockAsync().subscribe(
      res => {
        //this.WarehouseInputDetailSearch();
        this.WarehouseInputDetailBarcodeSearch();
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeMaterialModal(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeMaterialService.BaseParameter.GeneralID = element.ID;    
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseInputDetailBarcodeMaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
  WarehouseInputDetailCountSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailCountService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailCountService.BaseParameter.SearchString = this.WarehouseInputDetailCountService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputDetailCountService.DataSource) {
          this.WarehouseInputDetailCountService.DataSource.filter = this.WarehouseInputDetailCountService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputDetailCountService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailCountService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailCountService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
            this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
            this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
            this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailCountService.List = [];
      this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
      this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
      this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;
    }
  }
  WarehouseInputDetailCountDelete(element: WarehouseInputDetailCount) {
    this.WarehouseInputDetailCountService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailCountService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailCountSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailCountSave(element: WarehouseInputDetailCount) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailCountService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailCountService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailCountSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputMaterialSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputMaterialService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputMaterialService.BaseParameter.SearchString = this.WarehouseInputMaterialService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputMaterialService.DataSource) {
          this.WarehouseInputMaterialService.DataSource.filter = this.WarehouseInputMaterialService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputMaterialService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputMaterialService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseInputMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
            this.WarehouseInputMaterialService.DataSource = new MatTableDataSource(this.WarehouseInputMaterialService.List);
            this.WarehouseInputMaterialService.DataSource.sort = this.WarehouseInputMaterialSort;
            this.WarehouseInputMaterialService.DataSource.paginator = this.WarehouseInputMaterialPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputMaterialService.List = [];
      this.WarehouseInputMaterialService.DataSource = new MatTableDataSource(this.WarehouseInputMaterialService.List);
      this.WarehouseInputMaterialService.DataSource.sort = this.WarehouseInputMaterialSort;
      this.WarehouseInputMaterialService.DataSource.paginator = this.WarehouseInputMaterialPaginator;
    }
  }
  WarehouseInputMaterialDelete(element: WarehouseInputMaterial) {
    this.WarehouseInputMaterialService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputMaterialService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputMaterialSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputMaterialSave(element: WarehouseInputMaterial) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputMaterialService.BaseParameter.BaseModel = element;
    this.WarehouseInputMaterialService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
}