import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryLayer } from 'src/app/shared/ERP/CategoryLayer.model';
import { CategoryLayerService } from 'src/app/shared/ERP/CategoryLayer.service';

import { CategoryRack } from 'src/app/shared/ERP/CategoryRack.model';
import { CategoryRackService } from 'src/app/shared/ERP/CategoryRack.service';

import { CategoryLocation } from 'src/app/shared/ERP/CategoryLocation.model';
import { CategoryLocationService } from 'src/app/shared/ERP/CategoryLocation.service';

import { CategoryLocationMaterial } from 'src/app/shared/ERP/CategoryLocationMaterial.model';
import { CategoryLocationMaterialService } from 'src/app/shared/ERP/CategoryLocationMaterial.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { CategoryLocationModalComponent } from 'src/app/Category/category-location-modal/category-location-modal.component';
import { WarehouseInputDetailBarcodeDiagramComponent } from '../warehouse-input-detail-barcode-diagram/warehouse-input-detail-barcode-diagram.component';

@Component({
  selector: 'app-location-diagram',
  templateUrl: './location-diagram.component.html',
  styleUrls: ['./location-diagram.component.css']
})
export class LocationDiagramComponent {


  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryLayerService: CategoryLayerService,
    public CategoryRackService: CategoryRackService,
    public CategoryLocationService: CategoryLocationService,
    public CategoryLocationMaterialService: CategoryLocationMaterialService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,

  ) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.Active = false;
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CompanySearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CompanyService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CompanyService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CompanyService.ListFilter) {
          if (this.CompanyService.ListFilter.length > 0) {
            this.CategoryLayerService.BaseParameter.CompanyID = this.CompanyService.ListFilter[0].ID;
            this.CategoryDepartmentSearch();
            this.CategoryLocationSearch();
            this.CategoryLayerSearch();
            this.CategoryRackSearch();
          }
        }
      },
      err => {
      },
      () => {
        //this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
    this.CategoryLayerSearch();
    this.CategoryRackSearch();
    this.CategoryLocationSearch();
  }
  CategoryDepartmentChange() {
    this.CategoryRackSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.CategoryLayerService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.CategoryLayerService);
  }
  CategoryLocationMaterialSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLocationMaterialService.BaseParameter.Active = true;
    this.CategoryLocationMaterialService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryLocationMaterialService.GetByCompanyIDToListAsync().subscribe(
      res => {
        this.CategoryLocationMaterialService.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryLocationMaterialService.ListFilter = this.CategoryLocationMaterialService.ListParent;
        if (this.CategoryLayerService.BaseParameter.SearchString && this.CategoryLayerService.BaseParameter.SearchString.length > 0) {
          this.CategoryLayerService.BaseParameter.SearchString = this.CategoryLayerService.BaseParameter.SearchString.trim();
          this.CategoryLocationMaterialService.ListFilter = this.CategoryLocationMaterialService.ListParent.filter(o => o.MaterialName == this.CategoryLayerService.BaseParameter.SearchString);
        }
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLocationMaterialSearch001(CategoryLocation: CategoryLocation) {
    let List = [];
    if (this.CategoryLocationMaterialService.ListFilter) {
      if (this.CategoryLocationMaterialService.ListFilter.length > 0) {
        List = this.CategoryLocationMaterialService.ListFilter.filter(o => o.ParentID == CategoryLocation.ID).sort((a, b) => (a.Count < b.Count ? 1 : -1));
      }
    }
    return List;
  }
  CategoryLayerActiveChange() {
    for (let i = 0; i < this.CategoryLocationService.ListFilter.length; i++) {
      this.CategoryLocationService.ListFilter[i].Active = this.WarehouseInputDetailBarcodeService.BaseParameter.Active;
    }
  }
  CategoryLocationSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.Active = true;
    this.CategoryLocationService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryLocationService.GetByCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.CategoryLocationService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.CategoryLayerActiveChange();
        this.CategoryLocationMaterialSearch();
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLocationActiveChange(CategoryLocation: CategoryLocation) {
    CategoryLocation.Active = !CategoryLocation.Active;
  }
  CategoryLocationSearch001(CategoryRack: CategoryRack) {
    let List = [];
    if (this.CategoryLocationService.ListFilter) {
      if (this.CategoryLocationService.ListFilter.length > 0) {
        List = this.CategoryLocationService.ListFilter.filter(o => o.ParentID == CategoryRack.ID && o.CategoryLayerID == this.CategoryLayerService.BaseParameter.BaseModel.ID);
        List = List.sort((a, b) => (a.Name > b.Name ? 1 : -1));
      }
    }
    return List;
  }
  CategoryLayerSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLayerService.BaseParameter.Active = true;
    this.CategoryLayerService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryLayerService.GetByCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.CategoryLayerService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryLayerService.ListFilter) {
          if (this.CategoryLayerService.ListFilter.length > 0) {
            for (let i = 0; i < this.CategoryLayerService.ListFilter.length; i++) {
              this.CategoryLayerService.ListFilter[i].Active = false;
            }
            this.CategoryLayerService.ListFilter[0].Active = true;
            this.CategoryLayerService.BaseParameter.BaseModel = this.CategoryLayerService.ListFilter[0];
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryRackSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryRackService.BaseParameter.Active = true;
    this.CategoryRackService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryRackService.BaseParameter.ParentID = this.CategoryLayerService.BaseParameter.CategoryDepartmentID;
    this.CategoryRackService.GetByParentIDAndCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.CategoryRackService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        console.log(this.CategoryRackService.ListFilter);
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLayerView(CategoryLayer: CategoryLayer) {
    this.CategoryLayerService.IsShowLoading = true;
    for (let i = 0; i < this.CategoryLayerService.ListFilter.length; i++) {
      this.CategoryLayerService.ListFilter[i].Active = false;
    }
    CategoryLayer.Active = true;
    this.CategoryLayerService.BaseParameter.BaseModel = CategoryLayer;
    this.CategoryLayerService.IsShowLoading = false;
  }
  CategoryLocationModal(element: CategoryLocation) {
    this.CategoryLocationService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(CategoryLocationModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.CategoryLocationSearch();

    });
  }
  CreateAutoAsync() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLocationMaterialService.BaseParameter.CompanyID = this.CategoryLayerService.BaseParameter.CompanyID;
    this.CategoryLocationMaterialService.BaseParameter.CategoryDepartmentID = this.CategoryLayerService.BaseParameter.CategoryDepartmentID;
    this.CategoryLocationMaterialService.CreateAutoAsync().subscribe(
      res => {
        this.CategoryLocationSearch();
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodeModal(CategoryLocationMaterial: CategoryLocationMaterial) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.GeneralID = CategoryLocationMaterial.MaterialID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = CategoryLocationMaterial.ParentID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.Code = CategoryLocationMaterial.ParentName;
    this.WarehouseInputDetailBarcodeService.BaseParameter.Name = CategoryLocationMaterial.MaterialName;
    this.WarehouseInputDetailBarcodeService.BaseParameter.CategoryDepartmentID = this.CategoryLayerService.BaseParameter.CategoryDepartmentID;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(WarehouseInputDetailBarcodeDiagramComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
    });
  }
}
