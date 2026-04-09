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

import { WarehouseRequest } from 'src/app/shared/ERP/WarehouseRequest.model';
import { WarehouseRequestService } from 'src/app/shared/ERP/WarehouseRequest.service';

import { WarehouseRequestDetail } from 'src/app/shared/ERP/WarehouseRequestDetail.model';
import { WarehouseRequestDetailService } from 'src/app/shared/ERP/WarehouseRequestDetail.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { WarehouseRequestConfirm } from 'src/app/shared/ERP/WarehouseRequestConfirm.model';
import { WarehouseRequestConfirmService } from 'src/app/shared/ERP/WarehouseRequestConfirm.service';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

@Component({
  selector: 'app-warehouse-request-by-category-department-info',
  templateUrl: './warehouse-request-by-category-department-info.component.html',
  styleUrls: ['./warehouse-request-by-category-department-info.component.css']
})
export class WarehouseRequestByCategoryDepartmentInfoComponent {

  @ViewChild('WarehouseRequestDetailSort') WarehouseRequestDetailSort: MatSort;
  @ViewChild('WarehouseRequestDetailPaginator') WarehouseRequestDetailPaginator: MatPaginator;

  @ViewChild('WarehouseRequestConfirmSort') WarehouseRequestConfirmSort: MatSort;
  @ViewChild('WarehouseRequestConfirmPaginator') WarehouseRequestConfirmPaginator: MatPaginator;

  URLTemplate: string;

  IsSNPAll: boolean = false;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseRequestService: WarehouseRequestService,
    public WarehouseRequestDetailService: WarehouseRequestDetailService,
    public WarehouseRequestConfirmService: WarehouseRequestConfirmService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
    public ProductionOrderService: ProductionOrderService,

  ) {
    this.URLTemplate = this.WarehouseRequestService.APIRootURL + "Download/WarehouseRequestByCategoryDepartment.xlsx";
    this.ProductionOrderSearch();
    this.CategoryUnitSearch();
    //this.MaterialSearch();
    this.CompanySearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseRequestService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.WarehouseRequestSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  WarehouseRequestChange(event, files: FileList) {
    if (files) {
      this.WarehouseRequestService.FileToUpload = files;
      this.WarehouseRequestService.BaseParameter.Event = event;
    }
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseRequestService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseRequestService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseRequestService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseRequestService);
  }
  ProductionOrderSearch() {
    this.ProductionOrderService.ComponentGetByMembershipID_Active_IsCompleteToListAsync(this.WarehouseRequestService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.WarehouseRequestService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.ComponentGetByActiveToListAsync(this.WarehouseRequestService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  Date(value) {
    this.WarehouseRequestService.BaseParameter.BaseModel.Date = new Date(value);
  }
  WarehouseRequestFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseRequestService.FileToUpload = files;
      this.WarehouseRequestService.File = files.item(0);
    }
  }
  WarehouseRequestSearch() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseRequestService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CompanyChange();
        this.WarehouseRequestDetailSearch();
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("WarehouseRequestInfo/" + ID);
    this.WarehouseRequestService.BaseParameter.ID = ID;
    this.WarehouseRequestSearch();
  }
  Save() {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestService.BaseParameter.BaseModel.CompanyID = this.WarehouseRequestService.BaseParameter.CompanyID;
    this.WarehouseRequestService.BaseParameter.BaseModel.CustomerID = this.WarehouseRequestService.BaseParameter.CategoryDepartmentID;
    this.WarehouseRequestService.SaveAndUploadFilesByCategoryDepartmentAsync().subscribe(
      res => {
        this.WarehouseRequestService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseRequestService.FileToUpload = null;
        this.WarehouseRequestService.BaseParameter.Event = null;
        this.Add(this.WarehouseRequestService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  WarehouseRequestDetailSearch() {
    this.WarehouseRequestDetailService.List = [];
    if (this.WarehouseRequestService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseRequestDetailService.BaseParameter.SearchString.length > 0) {
        this.WarehouseRequestDetailService.BaseParameter.SearchString = this.WarehouseRequestDetailService.BaseParameter.SearchString.trim();
        if (this.WarehouseRequestDetailService.DataSource) {
          this.WarehouseRequestDetailService.DataSource.filter = this.WarehouseRequestDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseRequestDetailService.BaseParameter.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
        this.WarehouseRequestService.IsShowLoading = true;
        this.WarehouseRequestDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseRequestDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseRequestDetailService.DataSource = new MatTableDataSource(this.WarehouseRequestDetailService.List);
            this.WarehouseRequestDetailService.DataSource.sort = this.WarehouseRequestDetailSort;
            this.WarehouseRequestDetailService.DataSource.paginator = this.WarehouseRequestDetailPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseRequestService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseRequestDetailService.List = [];
      this.WarehouseRequestDetailService.DataSource = new MatTableDataSource(this.WarehouseRequestDetailService.List);
      this.WarehouseRequestDetailService.DataSource.sort = this.WarehouseRequestDetailSort;
      this.WarehouseRequestDetailService.DataSource.paginator = this.WarehouseRequestDetailPaginator;
    }
  }
  WarehouseRequestDetailDelete(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseRequestDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestDetailSave(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    element.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
    this.WarehouseRequestDetailService.BaseParameter.BaseModel = element;
    this.WarehouseRequestDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseRequestDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  WarehouseRequestConfirmSearch() {
    if (this.WarehouseRequestConfirmService.BaseParameter.SearchString.length > 0) {
      this.WarehouseRequestConfirmService.BaseParameter.SearchString = this.WarehouseRequestConfirmService.BaseParameter.SearchString.trim();
      if (this.WarehouseRequestConfirmService.DataSource) {
        this.WarehouseRequestConfirmService.DataSource.filter = this.WarehouseRequestConfirmService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseRequestConfirmService.BaseParameter.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestConfirmService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.WarehouseRequestConfirmService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.WarehouseRequestConfirmService.DataSource = new MatTableDataSource(this.WarehouseRequestConfirmService.List);
          this.WarehouseRequestConfirmService.DataSource.sort = this.WarehouseRequestConfirmSort;
          this.WarehouseRequestConfirmService.DataSource.paginator = this.WarehouseRequestConfirmPaginator;
        },
        err => {
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestConfirmDelete(element: WarehouseRequestConfirm) {
    this.WarehouseRequestService.IsShowLoading = true;
    this.WarehouseRequestConfirmService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseRequestService.IsShowLoading = true;
      this.WarehouseRequestConfirmService.RemoveAsync().subscribe(
        res => {
          this.WarehouseRequestSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseRequestService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseRequestConfirmSave(element: WarehouseRequestConfirm) {
    this.WarehouseRequestService.IsShowLoading = true;
    element.ParentID = this.WarehouseRequestService.BaseParameter.BaseModel.ID;
    element.MembershipID = this.WarehouseRequestConfirmService.BaseParameter.BaseModel.UpdateUserID;
    this.WarehouseRequestConfirmService.BaseParameter.BaseModel = element;
    this.WarehouseRequestConfirmService.SaveAsync().subscribe(
      res => {
        this.WarehouseRequestSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
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
  MaterialModal(element: WarehouseRequestDetail) {
    this.WarehouseRequestService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.WarehouseRequestService.IsShowLoading = false;
      }
    );
  }
  PlannedOrderModal() {
  }
  IsSNPAllChange() {
    if (this.WarehouseRequestDetailService.List) {
      if (this.WarehouseRequestDetailService.List.length > 0) {
        for (let i = 0; i < this.WarehouseRequestDetailService.List.length; i++) {
          let element = this.WarehouseRequestDetailService.List[i];
          if (element.ID > 0) {
            element.IsSNP = this.IsSNPAll;
            this.WarehouseRequestDetailIsSNPChange(element);
          }
        }
      }
    }
  }
  WarehouseRequestDetailIsSNPChange(element: WarehouseRequestDetail) {
    if (element.IsSNP == true) {
      if (element.QuantitySNP == 0 || element.QuantitySNP == null) {
        element.QuantitySNP = 1;
      }
      let Part = Math.floor(element.QuantityInvoice / element.QuantitySNP);
      let Remainder = element.QuantityInvoice % element.QuantitySNP;
      if (Remainder > 0) {
        Part = Part + 1;
      }
      element.Quantity = element.QuantitySNP * Part;
    }
    else {
      element.Quantity = element.QuantityInvoice;
    }
  }
}
